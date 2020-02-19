using Keras;
using Keras.Layers;
using Keras.Models;
using Keras.Optimizers;
using Keras.PreProcessing.Image;
using Numpy;
using System;
using System.IO;

namespace LegoVision
{
    class Program
    {
        private const string data_set = "cats_dogs";
        private const string data_dir = data_set + "/data";
        private const string models_dir = data_set + "/models";
        private const string train_data_dir = data_dir + "/train";
        private const string valid_data_dir = data_dir + "/validation";

        static void Main(string[] args)
        {
            var img_width = 150;
            var img_height = 150;

            var datagen = new ImageDataGenerator(rescale: 1f / 255f);

            var train_generator = datagen.FlowFromDirectory(
                directory: train_data_dir,
                target_size: (img_width, img_height).ToTuple(),
                classes: new string[] { "dogs", "cats" },
                class_mode: "binary",
                batch_size: 16);

            var validation_generator = datagen.FlowFromDirectory(
                directory: valid_data_dir,
                target_size: (img_width, img_height).ToTuple(),
                classes: new string[] { "dogs", "cats" },
                class_mode: "binary",
                batch_size: 32);


            var model = new Sequential();

            model.Add(new Conv2D(32, (3, 3).ToTuple(), input_shape: (img_width, img_height, 3)));
            model.Add(new Activation("relu"));
            model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));

            model.Add(new Conv2D(32, (3, 3).ToTuple(), input_shape: (img_width, img_height, 3)));
            model.Add(new Activation("relu"));
            model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));

            model.Add(new Conv2D(64, (3, 3).ToTuple(), input_shape: (img_width, img_height, 3)));
            model.Add(new Activation("relu"));
            model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));

            model.Add(new Flatten());
            model.Add(new Dense(64));
            model.Add(new Activation("relu"));
            model.Add(new Dropout(0.5));
            model.Add(new Dense(1));
            model.Add(new Activation("sigmoid"));

            model.Compile(
                loss: "binary_crossentropy", 
                optimizer: "rmsprop", 
                metrics: new string[] { "accuracy" });

            print("model complied!!");

            print("starting training....");
            var training = model.FitGenerator(
                generator: train_generator,
                steps_per_epoch: 2048 / 16,
                epochs: 20,
                validation_data: validation_generator,
                validation_steps: 832 / 16);
            print("training finished!!");

            // Save model and weights
            print("saving weights to catdog1.h5");
            var file = $"{models_dir}/{data_set}_1";
            File.WriteAllText(file+".json", model.ToJson());
            model.SaveWeight(file+".h5");
            print("all weights saved successfully !!");

            //model.LoadWeight("models/catdog1.h5");

            print("prediction:");
            var img = np.expand_dims(ImageUtil.ImageToArray(ImageUtil.LoadImg($"{train_data_dir}/dogs/dog.480.jpg", target_size: (img_width, img_height))), 0);

            NDarray result = model.Predict(img);
            print(result);
            if (result.flatten().GetData<float>()[0] == 1)
            {
                print("cat");
            }
            else
            {
                print("dog");
            }

            Console.ReadKey();
        }


        public static void print(object msg)
        {
            Console.WriteLine(msg.ToString());
        }


    }
}
