using offside_detector.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace offside_detector.Services
{
    public class ScoreTracker
    {
        private readonly ImageProcessor _imageProcessor;
        private readonly OffsideDetector _offsideDetector = new OffsideDetector();

        public ScoreTracker(ImageProcessor imageProcessor, OffsideDetector offsideDetector)
        {
            _imageProcessor = imageProcessor;
            _offsideDetector = offsideDetector;
        }

        public ScoreTracker() { }

        public bool CheckGoalScored(string beforeKickImagePath, string afterKickImagePath, List<Team> teams)
        {
            // Process before kick image
            var beforeKickProcessor = new ImageProcessor(beforeKickImagePath);
            var ballPositionBefore = beforeKickProcessor.DetectBall();

            // Check for offside in the before kick image
            _offsideDetector.DetectOffside(teams, ballPositionBefore);

            // Get attacking team (team with the ball)
            var attackingTeam = teams.First(t => t.PlayerWithBall != null);

            // Only check if the player with the ball is offside
            if (attackingTeam.PlayerWithBall.PlayerStatus == PlayerStatus.OFFSIDE)
            {
                throw new Exception("Goal not permitted. The attacking player is offside");
            }

            // Process after kick image
            var afterKickProcessor = new ImageProcessor(afterKickImagePath);
            var ballPositionAfter = afterKickProcessor.DetectBall();
            var goals = afterKickProcessor.DetectGoals();

            // Check if ball is in either goal
            foreach (var goal in goals)
            {
                if (goal.IsBallInGoal(ballPositionAfter))
                {
                    // Determine if it's the correct goal (not an own goal)
                    bool isCorrectGoal = IsCorrectGoal(attackingTeam, goal, ballPositionBefore);

                    //if (isCorrectGoal)
                    //{
                        // Increment the attacking team's score
                        attackingTeam.Score++;
                        return true;
                    //}
                }
            }

            throw new Exception("No goal permitted");
        }

        private bool IsCorrectGoal(Team attackingTeam, Goal goal, Point ballPosition)
        {
            // If attacking right, goal should be on right side
            if (attackingTeam.Direction == AttackDirection.Right)
            {
                return goal.PositionX > ballPosition.X;
            }
            // If attacking left, goal should be on left side
            else
            {
                return goal.PositionX < ballPosition.X;
            }
        }
    }
}