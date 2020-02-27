using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor;
using ImageProcessor.Imaging.Filters.EdgeDetection;
using Emgu.CV;


namespace TrainDataCreator
{
     class ImageProcessing
    {
        private string aimDir;
        private string startDir;

        private string[] filePaths;
        public ImageFactory imageProcessor;


        public ImageProcessing(string aimDir, string startDir)
        {
            this.aimDir = aimDir;
            this.startDir = startDir;
            imageProcessor = new ImageFactory(preserveExifData: true);
        }

        public bool processImages()
        {
            collectImmages(startDir);
            IEdgeFilter filter = new RobertsCrossEdgeFilter();

            for (int i = 0; i < filePaths.Length; i++)
            {
                imageProcessor.Load(filePaths[i]);
                imageProcessor.DetectEdges(filter, false);
                imageProcessor.Save(aimDir+"/res" +i + ".jpg");
            }


            return true;
        }

        public void collectImmages(string path)
        {
            filePaths = Directory.GetFiles(path);
        }

    }


}
