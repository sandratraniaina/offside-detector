using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;

namespace offside_checker
{
    public class OffsideDetection
    {
        public void DetectOffside(string imagePath)
        {
            var image = new Image<Bgr, byte>(imagePath);

            // Step 1: Detect Ball
            var ball = DetectBall(image); // Function to detect ball (black object)

            // Step 2: Detect Players (blue/red teams)
            var bluePlayers = DetectPlayers(image, new Bgr(255, 0, 0)); // Blue players
            var redPlayers = DetectPlayers(image, new Bgr(0, 0, 255)); // Red players

            // Step 3: Detect Player with the Ball
            var playerWithBall = GetPlayerClosestToBall(ball, bluePlayers, redPlayers);

            // Step 4: Check Offside and Players After the Ball
            var offsidePlayers = CheckOffside(playerWithBall, ball, bluePlayers, redPlayers);
            var afterBallPlayers = GetPlayersAfterBall(playerWithBall, bluePlayers, redPlayers, ball);

            // Step 5: Output result with labels
            DrawLabels(image, offsidePlayers, "O"); // Draw "O" for offside players
            DrawLabels(image, afterBallPlayers, "M"); // Draw "M" for players after the ball

            // Save the output image with labeled players
            image.Save("output_with_labels.jpg");

            Console.WriteLine("Offside and Players after Ball processed.");
        }

        // Function to detect ball (black object)
        public Point DetectBall(Image<Bgr, byte> image)
        {
            // Use color segmentation to find black (ball)
            var hsvImage = image.Convert<Hsv, byte>();
            var lowerBound = new Hsv(0, 0, 0); // Black color
            var upperBound = new Hsv(179, 255, 80);

            var mask = hsvImage.InRange(lowerBound, upperBound);
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(mask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            // Find the largest circle (ball)
            double maxArea = 0;
            Point ballCenter = new Point();
            foreach (var contour in contours.ToArray())
            {
                var rect = CvInvoke.BoundingRectangle(contour);
                if (rect.Width * rect.Height > maxArea)
                {
                    maxArea = rect.Width * rect.Height;
                    ballCenter = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                }
            }
            return ballCenter;
        }

        // Function to detect players based on color
        public List<Point> DetectPlayers(Image<Bgr, byte> image, Bgr teamColor)
        {
            var hsvImage = image.Convert<Hsv, byte>();
            var lowerBound = new Hsv(teamColor.Blue, teamColor.Green, teamColor.Red);
            var upperBound = new Hsv(179, 255, 255); // Adjust for team color

            var mask = hsvImage.InRange(lowerBound, upperBound);
            var contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(mask, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            var players = new List<Point>();
            foreach (var contour in contours.ToArray())
            {
                var rect = CvInvoke.BoundingRectangle(contour);
                players.Add(new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2));
            }
            return players;
        }

        // Function to get the closest player to the ball
        public Point GetPlayerClosestToBall(Point ball, List<Point> bluePlayers, List<Point> redPlayers)
        {
            double minDistance = double.MaxValue;
            Point closestPlayer = new Point();

            // Check blue players
            foreach (var player in bluePlayers)
            {
                var dist = Math.Sqrt(Math.Pow(player.X - ball.X, 2) + Math.Pow(player.Y - ball.Y, 2));
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestPlayer = player;
                }
            }

            // Check red players
            foreach (var player in redPlayers)
            {
                var dist = Math.Sqrt(Math.Pow(player.X - ball.X, 2) + Math.Pow(player.Y - ball.Y, 2));
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestPlayer = player;
                }
            }

            return closestPlayer;
        }

        // Function to check offside based on positions and return offside players
        public List<Point> CheckOffside(Point playerWithBall, Point ball, List<Point> bluePlayers, List<Point> redPlayers)
        {
            var offsidePlayers = new List<Point>();
            int opponentGoalLine = 1000; // Example X position of opponent goal line

            // Check for blue players offside
            foreach (var player in bluePlayers)
            {
                if (player.X > playerWithBall.X && player.X > opponentGoalLine)
                {
                    offsidePlayers.Add(player); // Player is offside
                }
            }

            // Check for red players offside
            foreach (var player in redPlayers)
            {
                if (player.X > playerWithBall.X && player.X > opponentGoalLine)
                {
                    offsidePlayers.Add(player); // Player is offside
                }
            }

            return offsidePlayers;
        }

        // Function to get players after the ball (not offside but after the ball)
        public List<Point> GetPlayersAfterBall(Point playerWithBall, List<Point> bluePlayers, List<Point> redPlayers, Point ball)
        {
            var afterBallPlayers = new List<Point>();

            // Check for blue players after the ball
            foreach (var player in bluePlayers)
            {
                if (player.X > ball.X && player.X < playerWithBall.X) // Player is after the ball and not offside
                {
                    afterBallPlayers.Add(player);
                }
            }

            // Check for red players after the ball
            foreach (var player in redPlayers)
            {
                if (player.X > ball.X && player.X < playerWithBall.X) // Player is after the ball and not offside
                {
                    afterBallPlayers.Add(player);
                }
            }

            return afterBallPlayers;
        }

        // Function to draw labels (O for offside, M for after ball) on the image
        public void DrawLabels(Image<Bgr, byte> image, List<Point> players, string label)
        {
            foreach (var player in players)
            {
                // Draw a circle around the player
                CvInvoke.Circle(image, player, 10, new Bgr(0, 255, 0).MCvScalar, 2); // Green circle

                // Draw label
                var labelPosition = new Point(player.X + 15, player.Y);
                CvInvoke.PutText(image, label, labelPosition, FontFace.HersheySimplex, 0.8, new Bgr(255, 255, 255).MCvScalar, 2);
            }
        }
    }

}
