from keras.layers import Conv2D, MaxPooling2D, ZeroPadding2D
from keras.layers import Dense, Activation, Dropout, Flatten
from keras import optimizers
from keras.models import Sequential
from keras.preprocessing.image import ImageDataGenerator, image
import math
import numpy as np 
import matplotlib.pyplot as plt
import lib
from lib import *


# step 1: load data

train_datagen = ImageDataGenerator(
                validation_split=0.3,
                
                samplewise_center=True, # FIXME: prepare std zca
                samplewise_std_normalization=True,
                zca_whitening=True,
                rescale=1./255,
                
                horizontal_flip=True,
                vertical_flip=True,
                fill_mode="constant",
                cval=0,
                width_shift_range=0.1,
                height_shift_range=0.1,
                zoom_range=[1.0,1.2],
                rotation_range=90,)

validation_datagen = ImageDataGenerator(
                validation_split=0.3,
                
                samplewise_center=True,
                samplewise_std_normalization=True,
                zca_whitening=True,
                rescale=1./255,)

train_generator = train_datagen.flow_from_directory(
                directory     = train_dir,
                target_size   = (img_height, img_width),
                classes       = classes,
                class_mode    = "categorical", # "binary",
                color_mode    = "grayscale",
                # save_to_dir   = f'{dataset_dir}/train_generated',
                batch_size    = 1,
                subset        = "training",)

validation_generator = validation_datagen.flow_from_directory(
                directory     = train_dir,
                target_size   = (img_height, img_width),
                classes       = classes,
                class_mode    = "categorical", # "binary",
                color_mode    = "grayscale",
                # save_to_dir   = f'{dataset_dir}/validation_generated',
                batch_size    = 1,
                subset        = "validation",)

print(f'Train generator samples: {train_generator.samples}  batch size: {train_generator.batch_size}  dir: {train_dir}')
print(f'Validation generator samples: {validation_generator.samples}  batch size: {validation_generator.batch_size}  dir: {train_dir}')

# step-2 : build model

print('creating model....')
model = Sequential()

model.add(Conv2D(32,(3,3), activation='relu', input_shape=(img_width, img_height, 1)))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Conv2D(32,(3,3), activation='relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Conv2D(64,(3,3), activation='relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Flatten())
model.add(Dense(64, activation='relu'))
model.add(Dropout(0.5))
model.add(Dense(len(classes), activation='softmax'))
# model.add(Activation('sigmoid'))

# TODO:
# https://stackoverflow.com/questions/45799474/keras-model-evaluate-vs-model-predict-accuracy-difference-in-multi-class-nlp-ta
# I have found the problem. metrics=['accuracy'] calculates accuracy automatically from cost function. 
# So using binary_crossentropy shows binary accuracy, not categorical accuracy. 
# Using categorical_crossentropy automatically switches to categorical accuracy and now it is the same as 
# calculated manually using model1.predict(). 
# Yu-Yang was right to point out the cost function and activation function for multi-class problem.
# P.S: One can get both categorical and binary accuracy by using metrics=['binary_accuracy', 'categorical_accuracy']

# https://stackoverflow.com/questions/41327601/why-is-binary-crossentropy-more-accurate-than-categorical-crossentropy-for-multi/46004661#46004661
# https://stackoverflow.com/questions/42081257/why-binary-crossentropy-and-categorical-crossentropy-give-different-performances/46038271#46038271


# loss = mean squared error vom berechneten Ergebnis zum erwarteten Ergebnis
# accuracy = ist das berechnete Ergebnis richtig? --> Epoch: correct guesses / total amount of guesses
print('compiling model....')
model.compile(
    loss='categorical_crossentropy', # 'binary_crossentropy',
    optimizer='adadelta',
    metrics=['accuracy']) 
print('model compiled!!')

print('starting training....')
training = model.fit_generator(
    # epoch = full pass on entire dataset
    # steps per epoch = number of batches in one epoch
    # batch size = number of samples to work through before the model’s internal parameters 
    # are updated (using stochastic gradient decent)
    epochs = 100,
    generator = train_generator,
    steps_per_epoch = train_generator.samples // train_generator.batch_size,
    validation_data = validation_generator,
    validation_steps = validation_generator.samples // validation_generator.batch_size,
)
history = training.history
print('training finished!!')

lib.save(model, history)
lib.plot_history(history)




### predict manually

correct = 0
checked = 0
for img in train_generator:
    checked += 1
    predict_classes = model.predict_classes(img[0])
    if predict_classes[0] == np.argmax(img[1]):
        correct += 1

    print(f"TRAIN checked={checked}  correct={correct}  accuracy={correct/checked}")

    if checked == train_generator.samples:
        break

print("\n\n")
correct = 0
checked = 0
for img in validation_generator:
    checked += 1
    predict_classes = model.predict_classes(img[0])
    if predict_classes[0] == np.argmax(img[1]):
        correct += 1

    print(f"VALIDATION checked={checked}  correct={correct}  accuracy={correct/checked}")

    if checked == validation_generator.samples:
        break


    