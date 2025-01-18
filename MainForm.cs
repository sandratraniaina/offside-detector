using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using offside_checker.Forms;

namespace offside_detector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void offsideDetectorButton_Click(object sender, EventArgs e)
        {
            var display = new OffsideDetectorDisplay();
            display.FormClosed += (s, args) => this.Show();
            display.Show();
            this.Hide();
        }

        private void scoreTrackerButton_Click(object sender, EventArgs e)
        {
            var display = new ScoreTrackerDisplay();
            display.FormClosed += (s, args) => this.Show();
            display.Show();
            this.Hide();
        }
    }
}
