using Keras;
using Keras.Layers;
using Keras.Models;
using Keras.Optimizers;
using Keras.PreProcessing.Image;
using Numpy;
using System;
using System.IO;
using System.Linq;

namespace LegoVision
{
    public class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            var form = new MainForm();
            form.ShowDialog();
        }


        public static void print(object msg)
        {
            Console.WriteLine(msg.ToString());
        }


    }
}
