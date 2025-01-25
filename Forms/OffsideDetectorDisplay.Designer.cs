namespace offside_checker.Forms
{
    partial class OffsideDetectorDisplay
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
            this.inputButton = new System.Windows.Forms.Button();
            this.inputBox = new System.Windows.Forms.PictureBox();
            this.outputBox = new System.Windows.Forms.PictureBox();
            this.outputButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.inputBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputBox)).BeginInit();
            this.SuspendLayout();
            // 
            // inputButton
            // 
            this.inputButton.Location = new System.Drawing.Point(628, 56);
            this.inputButton.Name = "inputButton";
            this.inputButton.Size = new System.Drawing.Size(130, 34);
            this.inputButton.TabIndex = 0;
            this.inputButton.Text = "Import Image";
            this.inputButton.UseVisualStyleBackColor = true;
            this.inputButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // inputBox
            // 
            this.inputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputBox.ErrorImage = null;
            this.inputBox.Location = new System.Drawing.Point(12, 188);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(660, 440);
            this.inputBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.inputBox.TabIndex = 1;
            this.inputBox.TabStop = false;
            this.inputBox.Click += new System.EventHandler(this.inputBox_Click);
            // 
            // outputBox
            // 
            this.outputBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputBox.Location = new System.Drawing.Point(710, 188);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(660, 440);
            this.outputBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.outputBox.TabIndex = 2;
            this.outputBox.TabStop = false;
            // 
            // outputButton
            // 
            this.outputButton.Location = new System.Drawing.Point(628, 106);
            this.outputButton.Name = "outputButton";
            this.outputButton.Size = new System.Drawing.Size(130, 37);
            this.outputButton.TabIndex = 3;
            this.outputButton.Text = "Process Image";
            this.outputButton.UseVisualStyleBackColor = true;
            this.outputButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(304, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Input Image";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(997, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Output Image";
            // 
            // Display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(1382, 741);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.inputButton);
            this.HelpButton = true;
            this.Name = "Display";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Offside detector";
            ((System.ComponentModel.ISupportInitialize)(this.inputBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button inputButton;
        private System.Windows.Forms.PictureBox inputBox;
        private System.Windows.Forms.PictureBox outputBox;
        private System.Windows.Forms.Button outputButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

