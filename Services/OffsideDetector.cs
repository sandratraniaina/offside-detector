using offside_checker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offside_checker.Services
{
    public class OffsideDetector
    {
        public void DetectOffside(List<Team> teams, Point ballPosition)
        {
            // Find the player nearest to the ball
            var nearestPlayer = teams
                .SelectMany(team => team.Players)
                .OrderBy(player => GetDistance(player.Point, ballPosition))
                .First();

            // Determine the goal direction
            var attackingTeam = teams.First(team => team.Players.Contains(nearestPlayer));
            var defendingTeam = teams.First(team => !team.Players.Contains(nearestPlayer));
            bool isAttackingRight = nearestPlayer.Point.X > ballPosition.X; // Assuming X-axis indicates left-to-right field

            // Find the last defender
            Player lastDefender = defendingTeam.Players
                .OrderBy(player => isAttackingRight ? -player.Point.X : player.Point.X)
                .First();

            // Detect offside for the attacking team
            foreach (var player in attackingTeam.Players)
            {
                if (player != nearestPlayer) // Ignore ball holder
                {
                    // Check if the player is beyond the last defender
                    if (isAttackingRight && player.Point.X > lastDefender.Point.X)
                    {
                        player.IsOffside = true;
                    }
                    else if (!isAttackingRight && player.Point.X < lastDefender.Point.X)
                    {
                        player.IsOffside = true;
                    }
                }
            }
        }

        // Helper to calculate Euclidean distance
        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

    }
}
