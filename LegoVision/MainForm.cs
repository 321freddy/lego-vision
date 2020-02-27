using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegoVision
{
    public partial class MainForm : Form
    {
        private ListBox listBox1;
        private Label label1;
        private GroupBox groupBox1;
        private Button predictButton;
        private Button trainButton;
        private Label predictLabel;
        private PictureBox pictureBox1;
        private LegoModel model;

        public MainForm()
        {
            InitializeComponent();

            // Extract dataset names from datasets directory (subfolders)
            listBox1.DataSource =
                Directory.GetDirectories("datasets")
                .Select(
                    dataset => string.Concat(dataset.Reverse().TakeWhile(s => !"/\\".Contains(s)).Reverse())
                )
                .Prepend("")
                .ToArray();
        }

        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.predictLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.predictButton = new System.Windows.Forms.Button();
            this.trainButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.DataSource = this.listBox1.CustomTabOffsets;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(12, 28);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(194, 292);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dataset";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.predictLabel);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.predictButton);
            this.groupBox1.Controls.Add(this.trainButton);
            this.groupBox1.Location = new System.Drawing.Point(212, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 292);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "dataset";
            this.groupBox1.Visible = false;
            // 
            // predictLabel
            // 
            this.predictLabel.AutoSize = true;
            this.predictLabel.Location = new System.Drawing.Point(7, 225);
            this.predictLabel.Name = "predictLabel";
            this.predictLabel.Size = new System.Drawing.Size(79, 17);
            this.predictLabel.TabIndex = 3;
            this.predictLabel.Text = "Prediction: ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(7, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(238, 196);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // predictButton
            // 
            this.predictButton.Location = new System.Drawing.Point(129, 256);
            this.predictButton.Name = "predictButton";
            this.predictButton.Size = new System.Drawing.Size(116, 30);
            this.predictButton.TabIndex = 1;
            this.predictButton.Text = "Predict";
            this.predictButton.UseVisualStyleBackColor = true;
            this.predictButton.Click += new System.EventHandler(this.predictButton_Click);
            // 
            // trainButton
            // 
            this.trainButton.Location = new System.Drawing.Point(7, 256);
            this.trainButton.Name = "trainButton";
            this.trainButton.Size = new System.Drawing.Size(116, 30);
            this.trainButton.TabIndex = 0;
            this.trainButton.Text = "Train";
            this.trainButton.UseVisualStyleBackColor = true;
            this.trainButton.Click += new System.EventHandler(this.trainButton_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(481, 331);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Name = "MainForm";
            this.Text = "LegoVision";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = (string)listBox1.SelectedItem;

            if (selected == "")
            {
                model = null;
                groupBox1.Visible = false;
            }
            else
            {
                model = LegoModel.load(new DataSet(selected));

                groupBox1.Text = selected;
                groupBox1.Visible = true;
            }

        }

        private void trainButton_Click(object sender, EventArgs e)
        {
            model.train();
        }

        private void predictButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "Bilder (*.jpg)|*.jpg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var file = openFileDialog.FileName;
                    pictureBox1.Image = Image.FromFile(file);
                    predictLabel.Text = "Prediction: " + model.predict(file);
                }
            }
            
        }
    }
}
