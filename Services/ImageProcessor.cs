// ImageProcessor.cs
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
    public class ImageProcessor
    {
        private readonly Mat _originalImage;

        public ImageProcessor(string imagePath)
        {
            _originalImage = CvInvoke.Imread(imagePath);
            if (_originalImage.IsEmpty)
                throw new ArgumentException("Failed to load image", nameof(imagePath));
        }

        public Bitmap DrawPlayerStatus(Team teamA, Team teamB)
        {
            var annotatedImage = _originalImage.Clone();
            teamA.Players.AddRange(teamB.Players);

            var normalColor = new MCvScalar(0, 0, 0);
            var offsideColor = new MCvScalar(10, 10, 255);
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

            return annotatedImage.ToBitmap();
        }

        public Bitmap DrawVerticalLineOnBitmap(Bitmap bitmap, Point startPoint, MCvScalar color, int thickness = 2)
        {
            using (Mat mat = bitmap.ToMat())
            {
                Point endPoint = new Point(startPoint.X, bitmap.Height);
                CvInvoke.Line(mat, startPoint, endPoint, color, thickness);
                return mat.ToBitmap();
            }
        }

        public Bitmap DrawArrow(Bitmap bitmap, Point startPoint, Point endPoint)
        {
            Mat mat = bitmap.ToMat();
            CvInvoke.ArrowedLine(
                mat,
                startPoint,
                endPoint,
                new MCvScalar(255, 0, 0),
                2,
                LineType.AntiAlias,
                0,
                0.2
            );
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
                return DrawVerticalLineOnBitmap(bitmap, new Point(Convert.ToInt32(team.IsAttackRight ? team.LastDefender.Point.X + team.LastDefender.Radius : team.LastDefender.Point.X - team.LastDefender.Radius), 0), new MCvScalar(0, 194, 234), thickness: 4);
            }
            return bitmap;
        }

        public Bitmap DrawGoalsOnImage(Bitmap image, List<Goal> goals)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                Pen pen = new Pen(Color.Red, 2);
                foreach (var goal in goals)
                {
                    Rectangle goalRect = new Rectangle(goal.PositionX, goal.PositionY, goal.Width, goal.Height);
                    g.DrawRectangle(pen, goalRect);
                }
            }
            return image;
        }

        public Mat GetOriginalImage() => _originalImage;
    }   
}