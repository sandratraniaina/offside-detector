using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Drawing;
using offside_detector.Models;

namespace offside_detector.Services
{
    public class ScoringEvent
    {
        public string ScoringTeam { get; set; }
        public DateTime Timestamp { get; set; }
        public Point BallPosition { get; set; }
    }

    public class MatchState
    {
        public int TeamAScore { get; set; }
        public int TeamBScore { get; set; }
        public DateTime MatchStartTime { get; set; }
        public List<ScoringEvent> ScoringEvents { get; set; }

        public MatchState()
        {
            MatchStartTime = DateTime.Now;
            ScoringEvents = new List<ScoringEvent>();
        }
    }

    public class GameStateService
    {
        private const string STATE_FILE_PATH = "match_history.json";
        private MatchState _currentMatch;

        public GameStateService()
        {
            _currentMatch = new MatchState();
        }

        public bool HasSavedState()
        {
            return File.Exists(STATE_FILE_PATH);
        }

        public void SaveScoringEvent(Team scoringTeam, Point ballPosition)
        {
            LoadFromFile();

            var scoringEvent = new ScoringEvent
            {
                ScoringTeam = DetermineTeamIdentifier(scoringTeam),
                Timestamp = DateTime.Now,
                BallPosition = ballPosition
            };

            _currentMatch.ScoringEvents.Add(scoringEvent);
            UpdateScores();
            SaveToFile();
        }

        private string DetermineTeamIdentifier(Team team)
        {
            if (team == null) return "Unknown";
            // Assuming left side is Team A and right side is Team B based on goal position
            return team.Goal.PositionX < 400 ? "0" : "1";
        }

        private void UpdateScores()
        {
            var teamAGoals = 0;
            var teamBGoals = 0;

            foreach (var evt in _currentMatch.ScoringEvents)
            {
                if (evt.ScoringTeam == "0") teamAGoals++;
                else if (evt.ScoringTeam == "1") teamBGoals++;
            }

            _currentMatch.TeamAScore = teamAGoals;
            _currentMatch.TeamBScore = teamBGoals;
        }

        public MatchState LoadState()
        {
            if (!HasSavedState())
            {
                return new MatchState();
            }

            try
            {
                string jsonString = File.ReadAllText(STATE_FILE_PATH);
                _currentMatch = JsonSerializer.Deserialize<MatchState>(jsonString);
                return _currentMatch;
            }
            catch (Exception exc)
            {
                // return new MatchState();
                throw exc;
            }
        }

        private void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_currentMatch, options);
                File.WriteAllText(STATE_FILE_PATH, jsonString);
            }
            catch (Exception exc)
            {
                throw exc;
                // Handle or log error if needed
            }
        }

        private void LoadFromFile()
        {
            if (HasSavedState())
            {
                try
                {
                    string jsonString = File.ReadAllText(STATE_FILE_PATH);
                    _currentMatch = JsonSerializer.Deserialize<MatchState>(jsonString);
                }
                catch (Exception exc)
                {
                    _currentMatch = new MatchState();
                    throw exc;
                }
            }
        }

        public void ResetState()
        {
            _currentMatch = new MatchState();
            if (File.Exists(STATE_FILE_PATH))
            {
                try
                {
                    File.Delete(STATE_FILE_PATH);
                }
                catch (Exception exc)
                {
                    throw exc;
                    // Handle or log error if needed
                }
            }
        }
    }
}