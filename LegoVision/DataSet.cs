using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoVision
{
    public class DataSet
    {
        public string name { get; set; }
        public string dir => "datasets/" + name;
        public string train_dir => dir + "/train";
        public string validation_dir => dir + "/validation";
        public string json_path => dir + "/model.json";
        public string h5_path => dir + "/model.h5";


        public int img_width { get; set; }
        public int img_height { get; set; }
        public string[] classes { get; set; }


        public DataSet(string name, int img_width = 150, int img_height = 150, string[] classes = null)
        {
            this.name = name;
            this.img_width = img_width;
            this.img_height = img_height;
            this.classes = classes ?? new string[] { "1x4 flat", "2x10 flat" };
        }
    }
}
