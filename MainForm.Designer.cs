namespace offside_detector
{
    partial class MainForm
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
            this.offsideDetectorButton = new System.Windows.Forms.Button();
            this.scoreTrackerButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // offsideDetectorButton
            // 
            this.offsideDetectorButton.Location = new System.Drawing.Point(186, 76);
            this.offsideDetectorButton.Name = "offsideDetectorButton";
            this.offsideDetectorButton.Size = new System.Drawing.Size(132, 50);
            this.offsideDetectorButton.TabIndex = 0;
            this.offsideDetectorButton.Text = "Offside Detector";
            this.offsideDetectorButton.UseVisualStyleBackColor = true;
            this.offsideDetectorButton.Click += new System.EventHandler(this.offsideDetectorButton_Click);
            // 
            // scoreTrackerButton
            // 
            this.scoreTrackerButton.Location = new System.Drawing.Point(372, 76);
            this.scoreTrackerButton.Name = "scoreTrackerButton";
            this.scoreTrackerButton.Size = new System.Drawing.Size(132, 50);
            this.scoreTrackerButton.TabIndex = 1;
            this.scoreTrackerButton.Text = "Score tracker";
            this.scoreTrackerButton.UseVisualStyleBackColor = true;
            this.scoreTrackerButton.Click += new System.EventHandler(this.scoreTrackerButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(113, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(475, 36);
            this.label1.TabIndex = 2;
            this.label1.Text = "Which feature do you want to use?";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 165);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scoreTrackerButton);
            this.Controls.Add(this.offsideDetectorButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button offsideDetectorButton;
        private System.Windows.Forms.Button scoreTrackerButton;
        private System.Windows.Forms.Label label1;
    }
}