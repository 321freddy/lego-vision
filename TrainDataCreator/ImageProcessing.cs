using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging.Filters.EdgeDetection;
using ImageProcessor.Imaging.Filters.Photo;
using System.Drawing;



namespace TrainDataCreator
{
     class ImageProcessing
    {
        private string aimDir;
        private string startDir;
        private int aimWidth;
        private int aimHeight;

        private string[] filePaths;
        public ImageFactory imageProcessor;


        public ImageProcessing(string aimDir, string startDir, int aimHeight, int aimWidth)
        {
            this.aimDir = aimDir;
            this.startDir = startDir;
            imageProcessor = new ImageFactory(preserveExifData: true);
            this.aimWidth = aimWidth;
            this.aimHeight = aimHeight;
        }

        public bool processImages()
        {
            collectImmages(startDir);
            //IEdgeFilter filter = new RobertsCrossEdgeFilter();
            //IMatrixFilter filter = new GreyScaleFilter();

            for (int i = 0; i < filePaths.Length; i++)
            {
                Image original = Image.FromFile((filePaths[i]));
                Bitmap resized = new Bitmap(original, new Size(aimWidth, aimHeight));

                Bitmap greyscale = Makegreyscale(resized);
                greyscale.Save(aimDir + "/res" + i + ".png", System.Drawing.Imaging.ImageFormat.Png);
                /*imageProcessor.Load(filePaths[i]);
                imageProcessor.Filter(filter);
                imageProcessor.Save(aimDir+"/res" +i + ".jpg");
                */
            }


            return true;
        }

        public void collectImmages(string path)
        {
            filePaths = Directory.GetFiles(path);
        }

        public Bitmap Makegreyscale(Bitmap original)
        {
            
            //make an empty bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = original.GetPixel(i, j);

                    //create the grayscale version of the pixel
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59) //TODO: QUellen für diese Zahelen
                        + (originalColor.B * .11));

                    //create the color object
                    Color newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    //set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }


    }


}
