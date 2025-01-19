using offside_detector.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace offside_detector.Services
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

            // Find the rightmost player across all teams
            var rightmostPlayer = teams
                .SelectMany(team => team.Players)
                .OrderByDescending(player => player.Point.X)
                .First();
            // Determine which team has the rightmost player
            var teamWithRightmostPlayer = teams.First(team => team.Players.Contains(rightmostPlayer));
            var otherTeam = teams.First(team => !team.Players.Contains(rightmostPlayer));

            // Set goalkeepers
            teamWithRightmostPlayer.GoalKeeper = rightmostPlayer;
            otherTeam.GoalKeeper = GetLeftmostPlayer(otherTeam);

            // Determine attacking team
            var attackingTeam = teams.First(team => team.Players.Contains(nearestPlayer));
            var defendingTeam = teams.First(team => !team.Players.Contains(nearestPlayer));

            // Set PlayerWithBall for the attacking team
            attackingTeam.PlayerWithBall = nearestPlayer;

            // Find the last defender for the defending team
            var lastDefender = defendingTeam.Players
                .Where(player => player != defendingTeam.GoalKeeper) // Exclude goalkeeper
                .OrderBy(player => attackingTeam == teamWithRightmostPlayer ? +player.Point.X : -player.Point.X)
                .FirstOrDefault();
            defendingTeam.LastDefender = lastDefender;

            bool isAttackingRight = attackingTeam != teamWithRightmostPlayer;

            // Set attack directions based on the determined direction
            defendingTeam.Direction = isAttackingRight ? AttackDirection.Left : AttackDirection.Right;
            attackingTeam.Direction = isAttackingRight ? AttackDirection.Right : AttackDirection.Left;

            // Check offside for attacking team players
            foreach (var player in attackingTeam.Players)
            {
                if (player != nearestPlayer) // Ignore the ball holder
                {
                    if (isAttackingRight && player.Point.X > lastDefender.Point.X)
                    {
                        player.PlayerStatus = PlayerStatus.OFFSIDE;
                    }
                    else if (!isAttackingRight && player.Point.X < lastDefender.Point.X)
                    {
                        player.PlayerStatus = PlayerStatus.OFFSIDE;
                    }
                    else if (isAttackingRight && player.Point.X >= ballPosition.X)
                    {
                        player.PlayerStatus = PlayerStatus.NORMAL;
                    }
                    else if (!isAttackingRight && player.Point.X <= ballPosition.X)
                    {
                        player.PlayerStatus = PlayerStatus.NORMAL;
                    }
                }
            }
        }

        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private Player GetLeftmostPlayer(Team team)
        {
            return team.Players
                .OrderBy(player => player.Point.X)
                .First();
        }
    }
}