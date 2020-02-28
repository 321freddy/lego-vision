namespace TrainDataCreator
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
            this.startDirBox = new System.Windows.Forms.TextBox();
            this.start = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.aimDirBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.selectDirAim = new System.Windows.Forms.Button();
            this.selectDirStart = new System.Windows.Forms.Button();
            this.aimWidthText = new System.Windows.Forms.TextBox();
            this.aimHeightText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startDirBox
            // 
            this.startDirBox.Location = new System.Drawing.Point(51, 37);
            this.startDirBox.Name = "startDirBox";
            this.startDirBox.Size = new System.Drawing.Size(568, 20);
            this.startDirBox.TabIndex = 1;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(625, 117);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(118, 32);
            this.start.TabIndex = 2;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Zu bearbeitender Ordner";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // aimDirBox
            // 
            this.aimDirBox.Location = new System.Drawing.Point(51, 78);
            this.aimDirBox.Name = "aimDirBox";
            this.aimDirBox.Size = new System.Drawing.Size(568, 20);
            this.aimDirBox.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Zielordner";
            // 
            // selectDirAim
            // 
            this.selectDirAim.Location = new System.Drawing.Point(623, 78);
            this.selectDirAim.Name = "selectDirAim";
            this.selectDirAim.Size = new System.Drawing.Size(120, 20);
            this.selectDirAim.TabIndex = 13;
            this.selectDirAim.Text = "Ordner Auswählen";
            this.selectDirAim.UseVisualStyleBackColor = true;
            this.selectDirAim.Click += new System.EventHandler(this.selectDirAim_Click);
            // 
            // selectDirStart
            // 
            this.selectDirStart.Location = new System.Drawing.Point(623, 36);
            this.selectDirStart.Name = "selectDirStart";
            this.selectDirStart.Size = new System.Drawing.Size(120, 20);
            this.selectDirStart.TabIndex = 14;
            this.selectDirStart.Text = "Ordner Auswählen";
            this.selectDirStart.UseVisualStyleBackColor = true;
            this.selectDirStart.Click += new System.EventHandler(this.selectDirStart_Click);
            // 
            // aimWidthText
            // 
            this.aimWidthText.Location = new System.Drawing.Point(51, 128);
            this.aimWidthText.Name = "aimWidthText";
            this.aimWidthText.Size = new System.Drawing.Size(100, 20);
            this.aimWidthText.TabIndex = 15;
            this.aimWidthText.Text = "1920";
            this.aimWidthText.TextChanged += new System.EventHandler(this.aimWidth_TextChanged);
            // 
            // aimHeightText
            // 
            this.aimHeightText.Location = new System.Drawing.Point(51, 176);
            this.aimHeightText.Name = "aimHeightText";
            this.aimHeightText.Size = new System.Drawing.Size(100, 20);
            this.aimHeightText.TabIndex = 16;
            this.aimHeightText.Text = "1080";
            this.aimHeightText.TextChanged += new System.EventHandler(this.aimHeightText_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Zielbreite";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Zielhöhe";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.aimHeightText);
            this.Controls.Add(this.aimWidthText);
            this.Controls.Add(this.selectDirStart);
            this.Controls.Add(this.selectDirAim);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.aimDirBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.start);
            this.Controls.Add(this.startDirBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox startDirBox;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox aimDirBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button selectDirAim;
        private System.Windows.Forms.Button selectDirStart;
        private System.Windows.Forms.TextBox aimWidthText;
        private System.Windows.Forms.TextBox aimHeightText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

