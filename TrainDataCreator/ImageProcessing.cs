﻿using System;
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
using Emgu.CV;
using Emgu.CV.Structure;





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
            IEdgeFilter filter = new KirschEdgeFilter();
            int threshold_value = 150; //0-255

            for (int i = 0; i < filePaths.Length; i++)
            {
                string aimDirThis = aimDir + "/res" + i + ".png";
                Image original = Image.FromFile((filePaths[i]));
                Bitmap resized = new Bitmap(original, new Size(aimWidth, aimHeight));

                Bitmap greyscale = MakeGrayscale3(resized);
                //greyscale.Save(aimDir + "/res" + i + ".png", ImageFormat.Png);
                imageProcessor.Load(greyscale);
                imageProcessor.DetectEdges(filter, false);
                imageProcessor.Save(aimDirThis);

                //Als nächstes eine Binärisierung mit Threshhold auf dem Kanten Bild
                //Dann alles im greyscale Bild nur Pixel behalten, die im Binär = 1
                Image<Gray, Byte> img = new Image<Gray, Byte>(aimDirThis);
                
                img = img.ThresholdBinary(new Gray(threshold_value), new Gray(255)).Dilate(1).Erode(1);
                var labels = new Mat();
                var stats = new Mat();
                var centroids = new Mat();
                var nLabels = CvInvoke.ConnectedComponentsWithStats(img, labels, stats, centroids);

                int biggestIndex = 0;
                int biggestArea = 0;
                int[] statsData = new int[stats.Rows * stats.Cols];
                stats.CopyTo(statsData); // Inhalt der 2-D Matrix in 1D Array umwandeln

                //Suche größter weißer Bereich
                for (int j = 5; j < statsData.Length; j = j + 5){ //erste Component ist meistens das Schwarze, kann deswegen ignoriert werden
                    var area = statsData[j + 4];
                    if (area  >  biggestArea)
                    {
                        biggestArea = area;
                        biggestIndex = j;
                    }
                }



                /*
                     var x = statsData[i * stats.Cols + 0];
                     var y = statsData[i * stats.Cols + 1];
                     var width = statsData[i * stats.Cols + 2];
                     var height = statsData[i * stats.Cols + 3];
                     var area = statsData[i * stats.Cols + 4];
                 */
                //img.Save(aimDirThis);
                Bitmap source = resized;

                int componentX = statsData[biggestIndex + 0];
                int componentY =  statsData[biggestIndex + 1];
                int componentWidth = statsData[biggestIndex + 2];
                int componentHeight = statsData[biggestIndex + 3];

                Bitmap CroppedImage = cropping(componentX, componentY, componentWidth, componentHeight, source);
                
                CroppedImage.Save(aimDirThis, ImageFormat.Png);



            }


            return true;
        }

        public void collectImmages(string path)
        {
            filePaths = Directory.GetFiles(path);
        }

        /* Langsamerer GreyScale Algorithmus
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
        }*/
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

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        public Bitmap cropping(int x, int y, int width, int height, Bitmap source)
        {
            int newY = y;
            int newHeight = height;
            int newX = x;
            int newWidth = width;
            Rectangle outsideRectangle1 = new Rectangle();
            Rectangle outsideRectangle2 = new Rectangle();

            if (width > height)
            {
                newY = y - ((width - height) / 2); //damit der Stein mittig im Bild ist
                newHeight = width;
                outsideRectangle1 = new Rectangle(x, newY, width, (newHeight - height) / 2) ;
                outsideRectangle2 = new Rectangle(x, y + height, width, (newHeight - height) / 2);

            }
            else if(height > width)
            {
                newX = x - ((height - width) / 2);
                newWidth = height;
                outsideRectangle1 = new Rectangle(newX, y , (newWidth - width) / 2,height);
                outsideRectangle2 = new Rectangle(x + width, y , (newWidth - width) / 2, height);
            }

            Graphics blacked = Graphics.FromImage(source);
            blacked.FillRectangle(Brushes.Black, outsideRectangle1);
            blacked.FillRectangle(Brushes.Black, outsideRectangle2);

           

            Bitmap blackedBMP = new Bitmap(source.Width, source.Height, blacked);

            blackedBMP.Save("C: \\Users\\smith\\Documents\\GitHub\\lego - vision\\TrainDataCreator\\Fotos\\1x4 flat\\results\\test,png", ImageFormat.Png);

            Rectangle section = new Rectangle(new Point(newX, newY), new Size(newWidth, newHeight));
            Bitmap cropedImage = CropImage(blackedBMP, section);

            return cropedImage;
        }

    }


}
