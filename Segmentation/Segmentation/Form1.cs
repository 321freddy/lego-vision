using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Segmentation
{
    public partial class Form1 : Form
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string fileDir;
        Bitmap original;

        public Form1()
        {
            InitializeComponent();
        }

        private void selectFileButton_Click(object sender, EventArgs e)
        {
            if (fileDir != null)
            {
                openFileDialog.InitialDirectory = fileDir;

            }
            else
            {
                openFileDialog.InitialDirectory = "c:\\";

            }
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            fileDir = openFileDialog.FileName;
            fileDirBox.Text = fileDir;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var outStream = openFileDialog.OpenFile();
                original = new Bitmap(outStream);
                pictureBoxOriginal.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBoxOriginal.Image = original;
            }

        }
    }
}
