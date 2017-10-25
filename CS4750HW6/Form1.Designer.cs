namespace CS4750HW6
{
    partial class Form1
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
            this.rtxtDisplay = new System.Windows.Forms.RichTextBox();
            this.btnPuzzle1 = new System.Windows.Forms.Button();
            this.btnPuzzle2 = new System.Windows.Forms.Button();
            this.btnPuzzle3 = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.chkSingleStep = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rtxtDisplay
            // 
            this.rtxtDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtxtDisplay.Location = new System.Drawing.Point(12, 12);
            this.rtxtDisplay.Name = "rtxtDisplay";
            this.rtxtDisplay.Size = new System.Drawing.Size(383, 341);
            this.rtxtDisplay.TabIndex = 0;
            this.rtxtDisplay.Text = "";
            // 
            // btnPuzzle1
            // 
            this.btnPuzzle1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPuzzle1.Location = new System.Drawing.Point(93, 382);
            this.btnPuzzle1.Name = "btnPuzzle1";
            this.btnPuzzle1.Size = new System.Drawing.Size(75, 23);
            this.btnPuzzle1.TabIndex = 1;
            this.btnPuzzle1.Text = "Puzzle 1";
            this.btnPuzzle1.UseVisualStyleBackColor = true;
            this.btnPuzzle1.Click += new System.EventHandler(this.btnPuzzle1_Click);
            // 
            // btnPuzzle2
            // 
            this.btnPuzzle2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPuzzle2.Location = new System.Drawing.Point(174, 382);
            this.btnPuzzle2.Name = "btnPuzzle2";
            this.btnPuzzle2.Size = new System.Drawing.Size(75, 23);
            this.btnPuzzle2.TabIndex = 2;
            this.btnPuzzle2.Text = "Puzzle 2";
            this.btnPuzzle2.UseVisualStyleBackColor = true;
            this.btnPuzzle2.Click += new System.EventHandler(this.btnPuzzle2_Click);
            // 
            // btnPuzzle3
            // 
            this.btnPuzzle3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPuzzle3.Location = new System.Drawing.Point(255, 382);
            this.btnPuzzle3.Name = "btnPuzzle3";
            this.btnPuzzle3.Size = new System.Drawing.Size(75, 23);
            this.btnPuzzle3.TabIndex = 3;
            this.btnPuzzle3.Text = "Puzzle 3";
            this.btnPuzzle3.UseVisualStyleBackColor = true;
            this.btnPuzzle3.Click += new System.EventHandler(this.btnPuzzle3_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReset.Location = new System.Drawing.Point(12, 382);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // chkSingleStep
            // 
            this.chkSingleStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSingleStep.AutoSize = true;
            this.chkSingleStep.Location = new System.Drawing.Point(12, 359);
            this.chkSingleStep.Name = "chkSingleStep";
            this.chkSingleStep.Size = new System.Drawing.Size(80, 17);
            this.chkSingleStep.TabIndex = 5;
            this.chkSingleStep.Text = "Single Step";
            this.chkSingleStep.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 417);
            this.Controls.Add(this.chkSingleStep);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnPuzzle3);
            this.Controls.Add(this.btnPuzzle2);
            this.Controls.Add(this.btnPuzzle1);
            this.Controls.Add(this.rtxtDisplay);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtDisplay;
        private System.Windows.Forms.Button btnPuzzle1;
        private System.Windows.Forms.Button btnPuzzle2;
        private System.Windows.Forms.Button btnPuzzle3;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox chkSingleStep;
    }
}

