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
            processor = new ImageProcessing(aimDir, startDir);
            processor.processImages();
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
    }
}
