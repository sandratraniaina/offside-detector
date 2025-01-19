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
    }
}
