namespace Segmentation
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.start = new System.Windows.Forms.Button();
            this.pictureBoxAfterwards = new System.Windows.Forms.PictureBox();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectFileButton = new System.Windows.Forms.Button();
            this.fileDirBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAfterwards)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(575, 53);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(118, 32);
            this.start.TabIndex = 26;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // pictureBoxAfterwards
            // 
            this.pictureBoxAfterwards.Location = new System.Drawing.Point(633, 91);
            this.pictureBoxAfterwards.Name = "pictureBoxAfterwards";
            this.pictureBoxAfterwards.Size = new System.Drawing.Size(576, 320);
            this.pictureBoxAfterwards.TabIndex = 25;
            this.pictureBoxAfterwards.TabStop = false;
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Location = new System.Drawing.Point(51, 91);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(576, 320);
            this.pictureBoxOriginal.TabIndex = 24;
            this.pictureBoxOriginal.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(290, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Foto";
            // 
            // selectFileButton
            // 
            this.selectFileButton.Location = new System.Drawing.Point(865, 26);
            this.selectFileButton.Name = "selectFileButton";
            this.selectFileButton.Size = new System.Drawing.Size(120, 20);
            this.selectFileButton.TabIndex = 22;
            this.selectFileButton.Text = "Ordner Auswählen";
            this.selectFileButton.UseVisualStyleBackColor = true;
            this.selectFileButton.Click += new System.EventHandler(this.selectFileButton_Click);
            // 
            // fileDirBox
            // 
            this.fileDirBox.Location = new System.Drawing.Point(293, 27);
            this.fileDirBox.Name = "fileDirBox";
            this.fileDirBox.Size = new System.Drawing.Size(568, 20);
            this.fileDirBox.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 427);
            this.Controls.Add(this.start);
            this.Controls.Add(this.pictureBoxAfterwards);
            this.Controls.Add(this.pictureBoxOriginal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.selectFileButton);
            this.Controls.Add(this.fileDirBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAfterwards)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.PictureBox pictureBoxAfterwards;
        private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectFileButton;
        private System.Windows.Forms.TextBox fileDirBox;
    }
}

