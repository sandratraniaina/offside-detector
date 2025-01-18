using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using offside_detector.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace offside_detector.Services
{
    public class ImageAnalyzer
    {
        private readonly Mat _originalImage;

        public ImageAnalyzer(Mat originalImage)
        {
            _originalImage = originalImage;
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
            CvInvoke.CvtColor(_originalImage, hsvImage, ColorConversion.Bgr2Hsv);

            var teamMask = new Mat();
            CvInvoke.InRange(
                hsvImage,
                new ScalarArray(new MCvScalar(minColor.Hue, minColor.Satuation, minColor.Value)),
                new ScalarArray(new MCvScalar(maxColor.Hue, maxColor.Satuation, maxColor.Value)),
                teamMask);

            var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            CvInvoke.MorphologyEx(teamMask, teamMask, MorphOp.Open, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
            CvInvoke.MorphologyEx(teamMask, teamMask, MorphOp.Close, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            var hierarchy = new Mat();
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(
                teamMask,
                contours,
                hierarchy,
                RetrType.External,
                ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);
                if (area < 100) continue;

                var moments = CvInvoke.Moments(contours[i]);
                var centerX = (int)(moments.M10 / moments.M00);
                var centerY = (int)(moments.M01 / moments.M00);
                var radius = Math.Sqrt(area / Math.PI);

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
            Mat hsvImage = new Mat();
            CvInvoke.CvtColor(_originalImage, hsvImage, ColorConversion.Bgr2Hsv);

            ScalarArray lowerBound = new ScalarArray(new MCvScalar(0, 0, 0));
            ScalarArray upperBound = new ScalarArray(new MCvScalar(180, 255, 50));

            Mat ballMask = new Mat();
            CvInvoke.InRange(hsvImage, lowerBound, upperBound, ballMask);

            var contours = new VectorOfVectorOfPoint();
            var hierarchy = new Mat();
            CvInvoke.FindContours(ballMask, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

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
                RotatedRect ellipse = CvInvoke.FitEllipse(contours[maxIndex]);
                return new Point(Convert.ToInt32(ellipse.Center.X), Convert.ToInt32(ellipse.Center.Y));
            }

            throw new Exception("Ball not found in image");
        }

        public List<Goal> DetectGoals()
        {
            var goals = new List<Goal>();
            int imageWidth = _originalImage.Width;
            int imageHeight = _originalImage.Height;
            int leftBoundary = (int)(imageWidth * 0.2);
            int rightBoundary = (int)(imageWidth * 0.8);

            var contours = DetectGoalContours();

            foreach (var contour in contours)
            {
                if (IsValidGoal(contour, imageHeight))
                {
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

                    if (goals.Count == 2) break;
                }
            }

            return goals;
        }

        private List<Rectangle> DetectGoalContours()
        {
            var contours = new List<Rectangle>();

            var grayImage = new Mat();
            var cannyOutput = new Mat();
            CvInvoke.CvtColor(_originalImage, grayImage, ColorConversion.Bgr2Gray);
            CvInvoke.Canny(grayImage, cannyOutput, 100, 200);

            var contourVectors = new VectorOfVectorOfPoint();
            var hierarchy = new Mat();
            CvInvoke.FindContours(
                cannyOutput,
                contourVectors,
                hierarchy,
                RetrType.Tree,
                ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contourVectors.Size; i++)
            {
                Rectangle boundingRect = CvInvoke.BoundingRectangle(contourVectors[i]);
                contours.Add(boundingRect);
            }

            return contours;
        }

        private bool IsValidGoal(Rectangle contour, int imageHeight)
        {
            const int MinWidth = 50;
            const int MinHeight = 50;
            int maxHeight = imageHeight / 2;

            return contour.Width >= MinWidth &&
                   contour.Height >= MinHeight &&
                   contour.Height <= maxHeight;
        }
    }
}
