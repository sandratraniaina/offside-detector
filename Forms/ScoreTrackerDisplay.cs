using offside_detector.Models;
using offside_detector.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace offside_detector
{
    public partial class ScoreTrackerDisplay : Form
    {
        private String _beforeImagePath;
        private String _afterImagePath;
        private ScoreTracker _scoreTracker;

        Team teamA = new Team(), teamB = new Team();
        public ScoreTrackerDisplay()
        {
            InitializeComponent();
        }

        private void beforeImageButto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _beforeImagePath = openFileDialog.FileName;
                this.beforeImageBox.Image = Image.FromFile(_beforeImagePath);
            }
        }

        private void afterImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _afterImagePath = openFileDialog.FileName;
                this.afterImageBox.Image = Image.FromFile(_afterImagePath);
            }
        }

        private void processImageButton_Click(object sender, EventArgs e)
        {
            if (this._beforeImagePath == null || this._afterImagePath == null)
            {
                MessageBox.Show("Please, provide all images");
                return;
            }
            _scoreTracker = new ScoreTracker();

            var temp = new ImageProcessor(_beforeImagePath);

            int aScore = teamA.Score;
            int bScore = teamB.Score;

            teamA = temp.DetectTeam(new Emgu.CV.Structure.Hsv(0, 200, 200), new Emgu.CV.Structure.Hsv(10, 255, 255));
            teamB = temp.DetectTeam(new Emgu.CV.Structure.Hsv(100, 150, 50), new Emgu.CV.Structure.Hsv(140, 255, 255));

            teamA.Score = aScore;
            teamB.Score = bScore;

            try
            {
                _scoreTracker.CheckGoalScored(_beforeImagePath, _afterImagePath, new List<Team> { teamB, teamA });
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                this.teamAScore.Text = $"{teamA.Score}";
                this.teamBScore.Text = $"{teamB.Score}";
            }
        }
    }
}