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
using System.Drawing.Imaging;




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
            IEdgeFilter filter = new RobertsCrossEdgeFilter();

            for (int i = 0; i < filePaths.Length; i++)
            {
                Image original = Image.FromFile((filePaths[i]));
                Bitmap resized = new Bitmap(original, new Size(aimWidth, aimHeight));

                Bitmap greyscale = MakeGrayscale3(resized);
                //greyscale.Save(aimDir + "/res" + i + ".png", ImageFormat.Png);
                imageProcessor.Load(greyscale);
                imageProcessor.DetectEdges(filter);
                imageProcessor.Save(aimDir+"/res" +i + ".jpg");
                
            }


            return true;
        }

        public void collectImmages(string path)
        {
            filePaths = Directory.GetFiles(path);
        }

        public Bitmap MakeGrayscale(Bitmap original)
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
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

    }


}
