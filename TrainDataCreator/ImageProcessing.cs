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
                string aimDirThisPre = aimDir + "/respre" + i + ".png";

                Image original = Image.FromFile((filePaths[i]));
                Bitmap resized = new Bitmap(original, new Size(aimWidth, aimHeight));

                Bitmap greyscale = MakeGrayscale3(resized);
                MemoryStream outStream = new MemoryStream();
                imageProcessor.Load(greyscale);
                imageProcessor.DetectEdges(filter, false);
                imageProcessor.Save(aimDir+"/temp" + "/res" + i + ".png");
                imageProcessor.Save(outStream);


                //Als nächstes eine Binärisierung mit Threshhold auf dem Kanten Bild
                //Dann alles im greyscale Bild nur Pixel behalten, die im Binär = 1
                Image<Gray, Byte> img = new Image<Gray, Byte>(aimDir + "/temp" + "/res" + i + ".png");
                
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

                //Bitmap source = greyscale;
                Bitmap edges = new Bitmap(outStream);

                int componentX = statsData[biggestIndex + 0];
                int componentY =  statsData[biggestIndex + 1];
                int componentWidth = statsData[biggestIndex + 2];
                int componentHeight = statsData[biggestIndex + 3];



                Bitmap CroppedColor = cropping(componentX, componentY, componentWidth, componentHeight, resized,true);

                Bitmap CroppedImage = cropping(componentX, componentY, componentWidth, componentHeight, greyscale, false);

                string color = determineColor(CroppedColor);

                CroppedImage.Save(aimDir + "/res" + i +color+ ".png" , ImageFormat.Png);
                //CroppedColor.Save(aimDir + "/resC" + i + color + ".png", ImageFormat.Png);

                edges = null;

                CroppedImage = null;

                img = null;





            }




            return true;
        }

        

        public void collectImmages(string path)
        {
            filePaths = Directory.GetFiles(path);
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

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
                return bitmap;
            }
        }

        public Bitmap cropping(int x, int y, int width, int height, Bitmap source, bool onlyStone)
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
            
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            /*
            Graphics blacked = Graphics.FromImage(source);
            blacked.FillRectangle(blackBrush, outsideRectangle1);
            blacked.FillRectangle(blackBrush, outsideRectangle2);
            */


            Rectangle section;
            if(onlyStone == true)
            {
                section = new Rectangle(x, y, width, height);
            }
            else
            {
                section = new Rectangle(new Point(newX, newY), new Size(newWidth, newHeight));

            }
            Bitmap croppedImage = CropImage(source, section);

            return croppedImage;
        }

        public string determineColor(Bitmap original)
        {
            string color = "";

            float sum = 0;
            float count = 0;
            for(int i = 0; i < original.Height; i++)
            {
                for(int  j = 0; j < original.Width; j++)
                {
                    float hue  =  original.GetPixel(j, i).GetHue();
                    float saturation =  original.GetPixel(j, i).GetSaturation();
                    float lightness =  original.GetPixel(j, i).GetBrightness();

                    if(saturation > 0.5) {
                        if(lightness > 0.15)
                        {
                            if(lightness < 0.95)
                            {
                                //Not grey, white, black or background
                                if(hue > 340)
                                {
                                    hue = 0; //To catch the reds on the other end of the spectrum
                                }
                                sum = sum + hue;
                                count = count +1;
                            }
                            else
                            {
                                //white or Background
                            }
                        }
                        else
                        {
                            //Black or Background
                        }
                    }
                    else
                    {
                        //Grey or Background
                    }
                }
            }

            float average = sum / count;

            if(average < 15) //Red
            {
                color = "Red";
            }else if(15 <= average && average < 30) //Orange
            {
                color = "Orange";

            }
            else if (30 <= average && average < 65) //Yellow
            {
                color = "Yellow";

            }
            else if (65 <= average && average < 150) //Green
            {
                color = "Green";

            }
            else if (150 <= average && average < 170) //Turquois
            {
                color = "Turquois";

            }
            else if (170 <= average && average < 260) //Blue
            {
                color = "Blue";

            }
            else if (260 <= average && average < 340) //purple
            {
                color = "Purple";

            }

            return color;



        }


    }



}
