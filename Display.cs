﻿using offside_checker.Services;
using offside_checker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace offside_checker
{
    public partial class Display : Form
    {

        private String _imagePath;
        private ImageProcessor _imageProcessor;
        private OffsideDetector _offsideDetector;

        public Display()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _imagePath = openFileDialog.FileName;
                this.inputBox.Image = Image.FromFile(_imagePath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _imageProcessor = new ImageProcessor(_imagePath);
            var teamA = _imageProcessor.DetectTeam(new Emgu.CV.Structure.Hsv(0, 200, 200), new Emgu.CV.Structure.Hsv(10, 255, 255));
            var teamB = _imageProcessor.DetectTeam(new Emgu.CV.Structure.Hsv(100, 150, 50), new Emgu.CV.Structure.Hsv(140, 255, 255));

            string message = $"Here are the teams: teamA: {teamA.Players.Count}, teamB: {teamB.Players.Count}";

            MessageBox.Show(message, "Message Title", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
