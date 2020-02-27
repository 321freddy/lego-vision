using Keras;
using Keras.Layers;
using Keras.Models;
using Keras.Optimizers;
using Keras.PreProcessing.Image;
using Numpy;
using System;
using System.IO;
using static LegoVision.Program;

namespace LegoVision
{
    public class LegoModel
    {
        public BaseModel model { get; private set; }
        public DataSet data_set { get; private set; }

        private LegoModel(BaseModel model, DataSet data_set)
        {
            this.model = model;
            this.data_set = data_set;
        }

        public static LegoModel create(DataSet data_set)
        {
            print("creating model....");
            var model = new Sequential();

            model.Add(new Conv2D(32, (3, 3).ToTuple(), input_shape: (data_set.img_width, data_set.img_height, 3)));
            model.Add(new Activation("relu"));
            model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));

            model.Add(new Conv2D(32, (3, 3).ToTuple(), input_shape: (data_set.img_width, data_set.img_height, 3)));
            model.Add(new Activation("relu"));
            model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));

            model.Add(new Conv2D(64, (3, 3).ToTuple(), input_shape: (data_set.img_width, data_set.img_height, 3)));
            model.Add(new Activation("relu"));
            model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));

            model.Add(new Flatten());
            model.Add(new Dense(64));
            model.Add(new Activation("relu"));
            model.Add(new Dropout(0.5));
            model.Add(new Dense(1));
            model.Add(new Activation("sigmoid"));


            print("compiling model....");
            model.Compile(
                loss: "binary_crossentropy",
                optimizer: "rmsprop",
                metrics: new string[] { "accuracy" });
            print("model compiled!!");


            return new LegoModel(model, data_set);
        }

        public static LegoModel load(DataSet data_set)
        {
            if (File.Exists(data_set.json_path))
            {
                print("loading model and weights....");
                var json = File.ReadAllText(data_set.json_path);
                var model = BaseModel.ModelFromJson(json);
                if (File.Exists(data_set.h5_path))
                    model.LoadWeight(data_set.h5_path);
                print("loaded successfully !!");


                return new LegoModel(model, data_set);
            }
            else
            {
                return create(data_set);
            }
            
        }

        public void train()
        {
            var datagen = new ImageDataGenerator(rescale: 1f / 255f);

            var train_generator = datagen.FlowFromDirectory(
                directory: data_set.train_dir,
                target_size: (data_set.img_width, data_set.img_height).ToTuple(),
                classes: data_set.classes,
                class_mode: "binary",
                batch_size: 16); //16

            var validation_generator = datagen.FlowFromDirectory(
                directory: data_set.validation_dir,
                target_size: (data_set.img_width, data_set.img_height).ToTuple(),
                classes: data_set.classes,
                class_mode: "binary",
                batch_size: 1); //32
            

            print("starting training....");
            var training = model.FitGenerator(
                generator: train_generator,
                steps_per_epoch: 2048 / 16,
                epochs: 20,
                validation_data: validation_generator,
                validation_steps: 832 / 16);
            print("training finished!!");

            save();
            
        }

        public void save()
        {
            // Save model and weights
            print("saving model and weights....");
            File.WriteAllText(data_set.json_path, model.ToJson());
            model.SaveWeight(data_set.h5_path);
            print("saved successfully !!");
        }

        public string predict(string img_path)
        {
            var img = np.expand_dims(ImageUtil.ImageToArray(ImageUtil.LoadImg(img_path, target_size: (data_set.img_width, data_set.img_height))), 0);
            NDarray resultArray = model.Predict(img);

            return data_set.classes[ (int)resultArray.flatten().GetData<float>()[0] ];
        }
    }
}
