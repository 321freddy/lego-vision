using Keras;
using Keras.Layers;
using Keras.Models;
using Keras.Optimizers;
using Keras.PreProcessing.Image;
using Numpy;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LegoVision
{
    public class Program
    {

        public static string RootDir;

        [STAThread]
        static void Main(string[] args)
        {
            RootDir = args.Aggregate((x,y) => x+" "+y) ?? "";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }


        public static void print(object msg)
        {
            Console.WriteLine(msg.ToString());
        }


    }
}
