using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainDataCreator
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        private string aimDir;
        private string startDir;
        private int aimWidth =  1920;
        private int aimHeight =  1080;
        ImageProcessing processor;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void start_Click(object sender, EventArgs e)
        {
            processor = new ImageProcessing(aimDir, startDir, aimHeight, aimWidth);
            processor.processImages();
            //processor.disposeTempFiles(aimDir);
        }

        private void selectDirAim_Click(object sender, EventArgs e)
        {
            folderDlg.ShowDialog();
            aimDir = folderDlg.SelectedPath;
            aimDirBox.Text = aimDir;
        }

        private void selectDirStart_Click(object sender, EventArgs e)
        {
            folderDlg.ShowDialog();
            startDir = folderDlg.SelectedPath;
            startDirBox.Text = startDir;
        }

        private void aimWidth_TextChanged(object sender, EventArgs e)
        {
            this.aimWidth = Convert.ToInt16(aimWidthText.Text);
        }

        private void aimHeightText_TextChanged(object sender, EventArgs e)
        {
            this.aimHeight = Convert.ToInt16(aimHeightText.Text);

        }

         

    }
}
