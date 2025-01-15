﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using offside_checker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace offside_checker.Services
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
            var ballMask = new Mat();
            var lowerBlack = new ScalarArray(new MCvScalar(0, 0, 0));
            var upperBlack = new ScalarArray(new MCvScalar(50, 50, 50));
            CvInvoke.InRange(_originalImage, lowerBlack, upperBlack, ballMask);

            var hierarchy = new Mat();
            var contours = new VectorOfVectorOfPoint();

            CvInvoke.FindContours(
                ballMask,
                contours,
                hierarchy,
                RetrType.External,
                ChainApproxMethod.ChainApproxSimple);

            if (contours.Size > 0)
            {
                var moments = CvInvoke.Moments(contours[0]);
                return new Point(
                        (int)(moments.M10 / moments.M00),
                        (int)(moments.M01 / moments.M00));
            }

            throw new Exception("Ball not found in image");
        }

        public Bitmap DrawOffsideText(Team teamA, Team teamB)
        {
            // Create a copy of the original image to draw on
            var annotatedImage = _originalImage.Clone();
            teamA.Players.AddRange(teamB.Players);
            // Loop through both teams' players
            foreach (var player in teamA.Players)
            {
                if (player.IsOffside)
                {
                    // Choose a text color based on offside status
                    var textColor = new MCvScalar(0, 0, 255); // Red for offside
                    var position = player.Point;

                    // Draw the "OFF" text next to the player
                    CvInvoke.PutText(
                        annotatedImage,
                        "OFF",
                        new Point(position.X - 20, position.Y - 20),  // Position slightly above and to the left of the player
                        FontFace.HersheySimplex, // Font style
                        0.5,                    // Font scale
                        textColor,              // Color
                        2);                     // Thickness of the text
                }
            }

            // Return the annotated image
            return annotatedImage.ToBitmap();
        }

    }
}
