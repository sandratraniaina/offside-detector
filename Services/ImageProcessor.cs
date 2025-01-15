using Emgu.CV;
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

        public Team DetectTeam(Bgr minColor, Bgr maxColor)
        {
            var team = new Team
            {
                MinColor = minColor,
                MaxColor = maxColor,
                Players = new List<Player>()
            };

            var hsvImage = new Mat();
            CvInvoke.CvtColor(_originalImage, hsvImage, ColorConversion.Bgr2Hsv);

            var mask = new Mat();
            CvInvoke.InRange(hsvImage, new ScalarArray(new MCvScalar(minColor.Blue, minColor.Green, minColor.Red)), new ScalarArray(new MCvScalar(maxColor.Blue, maxColor.Green, maxColor.Red)), mask);

            var hierarchy = new Mat();
            var contours = new VectorOfVectorOfPoint();

            var kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));
            CvInvoke.MorphologyEx(mask, mask, MorphOp.Open, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());
            CvInvoke.MorphologyEx(mask, mask, MorphOp.Close, kernel, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            CvInvoke.FindContours(
                mask,
                contours,
                hierarchy,
                RetrType.External,
                ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                double area = CvInvoke.ContourArea(contours[i]);
                if (area < 100) continue; // Filter out small noise

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

        public Player DetectBall()
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
                return new Player
                {
                    Point = new Point(
                        (int)(moments.M10 / moments.M00),
                        (int)(moments.M01 / moments.M00)),
                    Radius = Math.Sqrt(CvInvoke.ContourArea(contours[0]) / Math.PI)
                };
            }

            throw new Exception("Ball not found in image");
        }
    }
}
