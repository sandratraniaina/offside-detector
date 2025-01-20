﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using offside_detector.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace offside_detector.Services
{
    public class ImageProcessor
    {
        private readonly Mat _originalImage;

        public ImageProcessor(string imagePath)
        {
            _originalImage = CvInvoke.Imread(imagePath);
            if (_originalImage.IsEmpty)
                throw new ArgumentException("Failed to load image", nameof(imagePath));
        }

        public Team DetectTeam(Hsv minColor, Hsv maxColor)
        {
            var team = new Team
            {
                MinColor = minColor,
                MaxColor = maxColor,
                Players = new List<Player>()
            };

            var hsvImage = new Mat();
            CvInvoke.CvtColor(_originalImage, hsvImage, ColorConversion.Bgr2Hsv);  // Convert to HSV for better color detection

            // Create a mask for the team using the provided Hsv range
            var teamMask = new Mat();
            CvInvoke.InRange(
                hsvImage,
                new ScalarArray(new MCvScalar(minColor.Hue, minColor.Satuation, minColor.Value)),
                new ScalarArray(new MCvScalar(maxColor.Hue, maxColor.Satuation, maxColor.Value)),
                teamMask);

            // Apply morphological operations to clean up the mask
            var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            CvInvoke.MorphologyEx(teamMask, teamMask, MorphOp.Open, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
            CvInvoke.MorphologyEx(teamMask, teamMask, MorphOp.Close, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            // Find contours (players) in the mask
            var hierarchy = new Mat();
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(
                teamMask,
                contours,
                hierarchy,
                RetrType.External,
                ChainApproxMethod.ChainApproxSimple);

            // Add players based on the contours
            for (int i = 0; i < contours.Size; i++)
            {
                // Filter small contours that might be noise
                double area = CvInvoke.ContourArea(contours[i]);
                if (area < 100) continue;  // Adjust the threshold based on your image size

                var moments = CvInvoke.Moments(contours[i]);
                var centerX = (int)(moments.M10 / moments.M00);
                var centerY = (int)(moments.M01 / moments.M00);
                var radius = Math.Sqrt(area / Math.PI);

                // Add the detected player to the team
                team.Players.Add(new Player
                {
                    Point = new Point(centerX, centerY),
                    Radius = radius
                });
            }

            return team;
        }

        public Point DetectBall()
        {
            // Convert to HSV color space
            Mat hsvImage = new Mat();
            CvInvoke.CvtColor(_originalImage, hsvImage, ColorConversion.Bgr2Hsv);

            // Define lower and upper bounds for ball color in HSV
            ScalarArray lowerBound = new ScalarArray(new MCvScalar(0, 0, 0)); // Adjust these values as needed
            ScalarArray upperBound = new ScalarArray(new MCvScalar(180, 255, 50)); // Adjust these values as needed

            // Create a mask for the ball color
            Mat ballMask = new Mat();
            CvInvoke.InRange(hsvImage, lowerBound, upperBound, ballMask);

            // Find contours in the mask
            var contours = new VectorOfVectorOfPoint();
            var hierarchy = new Mat();
            CvInvoke.FindContours(ballMask, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            // Find the largest contour (assuming it's the ball)
            double maxArea = 0;
            int maxIndex = -1;
            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);
                if (area > maxArea)
                {
                    maxArea = area;
                    maxIndex = i;
                }
            }

            if (maxIndex >= 0)
            {
                // Fit an ellipse to the contour
                RotatedRect ellipse = CvInvoke.FitEllipse(contours[maxIndex]);

                // Calculate the center of the ellipse (approximate ball center)
                return new Point(Convert.ToInt32(ellipse.Center.X), Convert.ToInt32(ellipse.Center.Y));
            }

            throw new Exception("Ball not found in image");
        }

        public Bitmap DrawPlayerStatus(Team teamA, Team teamB)
        {
            // Create a copy of the original image to draw on
            var annotatedImage = _originalImage.Clone();
            teamA.Players.AddRange(teamB.Players);

            var normalColor = new MCvScalar(0, 0, 0);
            var offsideColor = new MCvScalar(10, 10, 255);
            // Loop through both teams' players
            foreach (var player in teamA.Players)
            {
                if (player.PlayerStatus == PlayerStatus.OFFSIDE)
                {
                    var position = player.Point;

                    CvInvoke.Circle(annotatedImage, position, Convert.ToInt32(player.Radius + 20), offsideColor, thickness: 4);
                }
                else if (player.PlayerStatus == PlayerStatus.NORMAL)
                {
                    var position = player.Point;

                    CvInvoke.Circle(annotatedImage, position, Convert.ToInt32(player.Radius + 20), normalColor, thickness: 4);
                }
            }

            // Return the annotated image
            return annotatedImage.ToBitmap();
        }

        public Bitmap DrawVerticalLineOnBitmap(Bitmap bitmap, Point startPoint, MCvScalar color, int thickness = 2)
        {
            // Convert Bitmap to Mat
            using (Mat mat = bitmap.ToMat())
            {
                // Create end point
                Point endPoint = new Point(startPoint.X, bitmap.Height);

                // Draw the line
                CvInvoke.Line(
                    mat,
                    startPoint,
                    endPoint,
                    color,
                    thickness
                );

                // Convert back to Bitmap
                return mat.ToBitmap();
            }
        }

        public Bitmap DrawArrow(Bitmap bitmap, Point startPoint, Point endPoint)
        {
            // Convert Bitmap to Emgu CV Mat
            Mat mat = bitmap.ToMat();

            // Draw the arrowed line
            CvInvoke.ArrowedLine(
                mat,
                startPoint,                      // Starting point (ball position)
                endPoint,                    // Ending point (player position)
                new MCvScalar(255, 0, 0),          // Arrow color (blue in BGR format)
                2,                                 // Thickness of the arrow
                LineType.AntiAlias,                // Smooth anti-aliased line
                0,                                 // No fractional shift
                0.2                                // Arrowhead size relative to line length
            );

            // Convert Mat back to Bitmap
            return mat.ToBitmap();
        }

        public Bitmap DrawTeamArrow(Bitmap bitmap, Team team, Point ballPosition)
        {
            foreach (Player player in team.Players)
            {
                if (player.PlayerStatus == PlayerStatus.NORMAL)
                {
                    bitmap = DrawArrow(bitmap, ballPosition, player.Point);
                }
            }
            return bitmap;
        }

        public Bitmap DrawLastDefenderLine(Team team, Bitmap bitmap)
        {
            if (team.LastDefender != null)
            {
                return DrawVerticalLineOnBitmap(bitmap, new Point(Convert.ToInt32(team.Direction != AttackDirection.Right ? team.LastDefender.Point.X + team.LastDefender.Radius : team.LastDefender.Point.X - team.LastDefender.Radius), 0), new MCvScalar(0, 194, 234), thickness: 4);
            }
            else
            {
                return bitmap;
            }
        }

        public List<Goal> DetectGoals()
        {
            var goals = new List<Goal>();

            // Define goal detection boundaries
            int imageWidth = _originalImage.Width;
            int imageHeight = _originalImage.Height;
            int leftBoundary = (int)(imageWidth * 0.3);  // 20% from left
            int rightBoundary = (int)(imageWidth * 0.7); // 20% from right

            // Detect contours
            var contours = DetectGoalContours();

            // Process detected contours
            foreach (var contour in contours)
            {
                if (IsValidGoal(contour, imageHeight))
                {
                    // Check if goal is on left side
                    if (contour.X < leftBoundary)
                    {
                        goals.Add(new Goal
                        {
                            PositionX = contour.X,
                            PositionY = contour.Y,
                            Width = contour.Width,
                            Height = contour.Height
                        });
                    }
                    // Check if goal is on right side
                    else if (contour.X + contour.Width > rightBoundary)
                    {
                        goals.Add(new Goal
                        {
                            PositionX = contour.X,
                            PositionY = contour.Y,
                            Width = contour.Width,
                            Height = contour.Height
                        });
                    }

                    // Stop if we found both goals
                    //if (goals.Count == 2) break;
                }
            }

            return goals;
        }

        private List<Rectangle> DetectGoalContours()
        {
            var contours = new List<Rectangle>();

            // Convert to grayscale and apply edge detection
            var grayImage = new Mat();
            var cannyOutput = new Mat();
            CvInvoke.CvtColor(_originalImage, grayImage, ColorConversion.Bgr2Gray);
            CvInvoke.Canny(grayImage, cannyOutput, 100, 200);

            // Find contours
            var contourVectors = new VectorOfVectorOfPoint();
            var hierarchy = new Mat();
            CvInvoke.FindContours(
                cannyOutput,
                contourVectors,
                hierarchy,
                RetrType.Tree,
                ChainApproxMethod.ChainApproxSimple);

            // Convert contours to rectangles
            for (int i = 0; i < contourVectors.Size; i++)
            {
                Rectangle boundingRect = CvInvoke.BoundingRectangle(contourVectors[i]);
                contours.Add(boundingRect);
            }

            return contours;
        }

        private bool IsValidGoal(Rectangle contour, int imageHeight)
        {
            const int MinWidth = 50;   // Minimum goal width
            const int MinHeight = 50;  // Minimum goal height
            int maxHeight = imageHeight / 2;  // Maximum height (50% of image height)

            return contour.Width >= MinWidth &&
                   contour.Height >= MinHeight &&
                   contour.Height <= maxHeight;
        }

        public Bitmap DrawGoalsOnImage(Bitmap image, List<Goal> goals)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                Pen pen = new Pen(Color.Red, 2); // You can customize the color and width of the rectangle
                foreach (var goal in goals)
                {
                    Rectangle goalRect = new Rectangle(goal.PositionX, goal.PositionY, goal.Width, goal.Height);
                    g.DrawRectangle(pen, goalRect);
                }
            }

            return image;
        }
    }
}
