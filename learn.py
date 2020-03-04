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

datagen = ImageDataGenerator(rescale = 1./255)

train_generator = datagen.flow_from_directory(
                directory     = train_dir,
                target_size   = (img_height, img_width),
                classes       = classes,
                class_mode    = "binary",
                color_mode    = "grayscale",
                # save_to_dir = data_set.train_dir + "_converted",
                batch_size    = 32)

print(f'Train generator samples: {train_generator.samples}  batch size: {train_generator.batch_size}')

# validation_generator = datagen.flow_from_directory(directory=valid_data_dir,
# 											   target_size=(img_width,img_height),
# 											   classes=classes,
# 											   class_mode='binary',
# 											   batch_size=32)


# step-2 : build model

print('creating model....')
model = Sequential()

model.add(Conv2D(32,(3,3), input_shape=(img_width, img_height, 1)))
model.add(Activation('relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Conv2D(32,(3,3)))
model.add(Activation('relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Conv2D(64,(3,3)))
model.add(Activation('relu'))
model.add(MaxPooling2D(pool_size=(2,2)))

model.add(Flatten())
model.add(Dense(64))
model.add(Activation('relu'))
model.add(Dropout(0.5))
model.add(Dense(1))
model.add(Activation('sigmoid'))

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
    loss='binary_crossentropy',
    optimizer='adadelta', # 'rmsprop',
    metrics=['accuracy']) 
print('model compiled!!')

print('starting training....')
training = model.fit_generator(
    generator = train_generator,
    # epoch = full pass on entire dataset
    # steps per epoch = number of batches in one epoch
    # batch size = number of samples to work through before the model’s internal parameters 
    # are updated (using stochastic gradient decent)
    steps_per_epoch = train_generator.samples // train_generator.batch_size,
    epochs = 100,
    # validation_data = validation_generator,
    # validation_steps = validation_generator.samples // train_generator.batch_size,
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


