namespace offside_detector
{
    partial class ScoreTrackerDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.beforeImageBox = new System.Windows.Forms.PictureBox();
            this.afterImageBox = new System.Windows.Forms.PictureBox();
            this.teamALabel = new System.Windows.Forms.Label();
            this.teamAScore = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dash = new System.Windows.Forms.Label();
            this.teamBLabel = new System.Windows.Forms.Label();
            this.beforeImageButto = new System.Windows.Forms.Button();
            this.afterImageButton = new System.Windows.Forms.Button();
            this.processImageButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.beforeImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.afterImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // beforeImageBox
            // 
            this.beforeImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.beforeImageBox.ErrorImage = null;
            this.beforeImageBox.Location = new System.Drawing.Point(10, 131);
            this.beforeImageBox.Name = "beforeImageBox";
            this.beforeImageBox.Size = new System.Drawing.Size(660, 440);
            this.beforeImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.beforeImageBox.TabIndex = 2;
            this.beforeImageBox.TabStop = false;
            // 
            // afterImageBox
            // 
            this.afterImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.afterImageBox.ErrorImage = null;
            this.afterImageBox.Location = new System.Drawing.Point(712, 131);
            this.afterImageBox.Name = "afterImageBox";
            this.afterImageBox.Size = new System.Drawing.Size(660, 440);
            this.afterImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.afterImageBox.TabIndex = 3;
            this.afterImageBox.TabStop = false;
            // 
            // teamALabel
            // 
            this.teamALabel.AutoSize = true;
            this.teamALabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teamALabel.Location = new System.Drawing.Point(365, 34);
            this.teamALabel.Name = "teamALabel";
            this.teamALabel.Size = new System.Drawing.Size(186, 54);
            this.teamALabel.TabIndex = 4;
            this.teamALabel.Text = "Team A";
            // 
            // teamAScore
            // 
            this.teamAScore.AutoSize = true;
            this.teamAScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teamAScore.Location = new System.Drawing.Point(637, 25);
            this.teamAScore.Name = "teamAScore";
            this.teamAScore.Size = new System.Drawing.Size(58, 63);
            this.teamAScore.TabIndex = 5;
            this.teamAScore.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(701, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 63);
            this.label1.TabIndex = 6;
            this.label1.Text = "0";
            // 
            // dash
            // 
            this.dash.AutoSize = true;
            this.dash.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dash.Location = new System.Drawing.Point(687, 44);
            this.dash.Name = "dash";
            this.dash.Size = new System.Drawing.Size(19, 25);
            this.dash.TabIndex = 7;
            this.dash.Text = "-";
            // 
            // teamBLabel
            // 
            this.teamBLabel.AutoSize = true;
            this.teamBLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teamBLabel.Location = new System.Drawing.Point(820, 34);
            this.teamBLabel.Name = "teamBLabel";
            this.teamBLabel.Size = new System.Drawing.Size(186, 54);
            this.teamBLabel.TabIndex = 8;
            this.teamBLabel.Text = "Team B";
            // 
            // beforeImageButto
            // 
            this.beforeImageButto.Location = new System.Drawing.Point(257, 509);
            this.beforeImageButto.Name = "beforeImageButto";
            this.beforeImageButto.Size = new System.Drawing.Size(167, 37);
            this.beforeImageButto.TabIndex = 9;
            this.beforeImageButto.Text = "Select Before Image";
            this.beforeImageButto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.beforeImageButto.UseVisualStyleBackColor = true;
            this.beforeImageButto.Click += new System.EventHandler(this.beforeImageButto_Click);
            // 
            // afterImageButton
            // 
            this.afterImageButton.Location = new System.Drawing.Point(959, 509);
            this.afterImageButton.Name = "afterImageButton";
            this.afterImageButton.Size = new System.Drawing.Size(167, 37);
            this.afterImageButton.TabIndex = 10;
            this.afterImageButton.Text = "Select After Image";
            this.afterImageButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.afterImageButton.UseVisualStyleBackColor = true;
            this.afterImageButton.Click += new System.EventHandler(this.afterImageButton_Click);
            // 
            // processImageButton
            // 
            this.processImageButton.Location = new System.Drawing.Point(607, 614);
            this.processImageButton.Name = "processImageButton";
            this.processImageButton.Size = new System.Drawing.Size(167, 37);
            this.processImageButton.TabIndex = 11;
            this.processImageButton.Text = "Process Images";
            this.processImageButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.processImageButton.UseVisualStyleBackColor = true;
            // 
            // ScoreTrackerDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1382, 683);
            this.Controls.Add(this.processImageButton);
            this.Controls.Add(this.afterImageButton);
            this.Controls.Add(this.beforeImageButto);
            this.Controls.Add(this.teamBLabel);
            this.Controls.Add(this.dash);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.teamAScore);
            this.Controls.Add(this.teamALabel);
            this.Controls.Add(this.afterImageBox);
            this.Controls.Add(this.beforeImageBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ScoreTrackerDisplay";
            this.Text = "ScoreTrackerDisplay";
            ((System.ComponentModel.ISupportInitialize)(this.beforeImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.afterImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox beforeImageBox;
        private System.Windows.Forms.PictureBox afterImageBox;
        private System.Windows.Forms.Label teamALabel;
        private System.Windows.Forms.Label teamAScore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label dash;
        private System.Windows.Forms.Label teamBLabel;
        private System.Windows.Forms.Button beforeImageButto;
        private System.Windows.Forms.Button afterImageButton;
        private System.Windows.Forms.Button processImageButton;
    }
}