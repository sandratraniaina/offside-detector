﻿using offside_detector.Models;
using offside_detector.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV.Structure;

namespace offside_detector
{
    public partial class ScoreTrackerDisplay : Form
    {
        private const string IMAGE_FILTER = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

        private readonly Team _teamA;
        private readonly Team _teamB;
        private string _beforeImagePath;
        private string _afterImagePath;
        private ScoreTracker _scoreTracker;

        // HSV color ranges for team detection
        private static readonly Hsv TeamAMinColor = new Hsv(0, 200, 200);
        private static readonly Hsv TeamAMaxColor = new Hsv(10, 255, 255);
        private static readonly Hsv TeamBMinColor = new Hsv(100, 150, 50);
        private static readonly Hsv TeamBMaxColor = new Hsv(140, 255, 255);

        private readonly GameStateService _gameStateService;

        public ScoreTrackerDisplay()
        {
            InitializeComponent();
            _gameStateService = new GameStateService();
            _teamA = new Team();
            _teamB = new Team();
            CheckForPreviousState();
        }

        private void CheckForPreviousState()
        {
            if (_gameStateService.HasSavedState())
            {
                var result = MessageBox.Show(
                    "Previous match in progress. Would you like to continue?",
                    "Load Previous Match",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    LoadPreviousState();
                }
                else
                {
                    _gameStateService.ResetState();
                }
            }
        }

        private void LoadPreviousState()
        {
            try
            {
                var matchState = _gameStateService.LoadState();
                _teamA.Score = matchState.TeamAScore;
                _teamB.Score = matchState.TeamBScore;
                UpdateScoreDisplay();
            }
            catch (Exception ex)
            {
                ShowError($"Error loading previous state: {ex.Message}");
            }
        }

        private void beforeImageButto_Click(object sender, EventArgs e)
        {
            var imagePath = SelectImage();
            if (!string.IsNullOrEmpty(imagePath))
            {
                _beforeImagePath = imagePath;
                DisplayImage(beforeImageBox, imagePath);
            }
        }

        private void afterImageButton_Click(object sender, EventArgs e)
        {
            var imagePath = SelectImage();
            if (!string.IsNullOrEmpty(imagePath))
            {
                _afterImagePath = imagePath;
                DisplayImage(afterImageBox, imagePath);
            }
        }

        private void processImageButton_Click(object sender, EventArgs e)
        {
            if (!ValidateImages())
            {
                ShowError("Please, provide all images");
                return;
            }

            ProcessImages();
            UpdateScoreDisplay();
        }

        private string SelectImage()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = IMAGE_FILTER;
                return openFileDialog.ShowDialog() == DialogResult.OK ? openFileDialog.FileName : null;
            }
        }

        private void DisplayImage(PictureBox pictureBox, string imagePath)
        {
            try
            {
                pictureBox.Image = Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                ShowError($"Error loading image: {ex.Message}");
            }
        }

        private bool ValidateImages()
        {
            return !string.IsNullOrEmpty(_beforeImagePath) && !string.IsNullOrEmpty(_afterImagePath);
        }

        private void ProcessImages()
        {
            try
            {
                _scoreTracker = new ScoreTracker();
                var imageProcessor = new ImageProcessor(_beforeImagePath);
                var ballPositionBefore = imageProcessor.DetectBall();

                // Store current scores
                var teamACurrentScore = _teamA.Score;
                var teamBCurrentScore = _teamB.Score;

                // Detect teams
                UpdateTeamDetection(imageProcessor);

                // Restore scores
                _teamA.Score = teamACurrentScore;
                _teamB.Score = teamBCurrentScore;

                // Process goal detection
                try
                {
                    ProcessGoalDetection();

                    // If we get here, a goal was scored (determine which team scored)
                    var scoringTeam = _teamA.Score > teamACurrentScore ? _teamA : _teamB;

                    // Save the scoring event
                    _gameStateService.SaveScoringEvent(scoringTeam, ballPositionBefore);
                }
                catch (Exception ex)
                {
                    // Goal was not scored or was invalid
                    ShowError(ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void UpdateTeamDetection(ImageProcessor imageProcessor)
        {
            var detectedTeamA = imageProcessor.DetectTeam(TeamAMinColor, TeamAMaxColor);
            var detectedTeamB = imageProcessor.DetectTeam(TeamBMinColor, TeamBMaxColor);

            _teamA.Players = detectedTeamA.Players;
            _teamB.Players = detectedTeamB.Players;
        }

        private void ProcessGoalDetection()
        {
            _scoreTracker.CheckGoalScored(_beforeImagePath, _afterImagePath, new List<Team> { _teamB, _teamA });
        }

        private void UpdateScoreDisplay()
        {
            teamAScore.Text = _teamA.Score.ToString();
            teamBScore.Text = _teamB.Score.ToString();
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}