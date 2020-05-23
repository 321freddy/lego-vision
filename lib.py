from keras.models import load_model
from keras.preprocessing.image import image
import keras
import tensorflow as tf

import os
import math
import numpy as np 
import matplotlib.pyplot as plt
import pickle


img_width = 200
img_height = 200

dataset_name = 'lego_mixed'

# Paths
dataset_dir     = f'datasets\\{dataset_name}'
raw_dir         = f'{dataset_dir}\\raw'
prepare_dir     = f'{dataset_dir}\\prepare'
train_dir       = f'{dataset_dir}\\train'
model_path      = f'{dataset_dir}\\model.h5'
best_model_path = f'{dataset_dir}\\best_model.h5'
history_path    = f'{dataset_dir}\\history.pickle'

classes = []
if os.path.exists(train_dir):
    classes = [f.name for f in os.scandir(train_dir) if f.is_dir()]
if len(classes) == 0 and os.path.exists(prepare_dir):
    classes = [f.name for f in os.scandir(prepare_dir) if f.is_dir()]
if len(classes) == 0 and os.path.exists(raw_dir):
    classes = [f.name for f in os.scandir(raw_dir) if f.is_dir()]
if len(classes) == 0:
    raise RuntimeError("No classes found!")



def save(model, history=None):
    print('saving....')
    # save session
    # saver = tf.compat.v1.train.Saver()
    # sess = keras.backend.get_session()
    # saver.save(sess, session_path)

    if history is not None:
        with open(history_path, 'wb') as pickle_file:
            pickle.dump(history, pickle_file)

    model.save(model_path)
    print('saved successfully !!')


def load(dataset_name=dataset_name):
    print('loading model....')
    # restore session
    # saver = tf.compat.v1.train.Saver()
    # sess = keras.backend.get_session()
    # saver.restore(sess, session_path)

    with open(history_path, 'rb') as pickle_file:
        history = pickle.load(pickle_file)

    model = load_model(model_path)
    print('model loaded!!')
    
    return model, history



def plot_history(history):
    fig = plt.figure(figsize=(10,5))

    # Plot training & validation loss values
    fig.add_subplot(1, 2, 1, title='Model loss', ylabel='Loss', xlabel='Epoch')
    plt.plot(history['loss'])
    plt.plot(history['val_loss'])
    plt.legend(['Train', 'Validation'], loc='best')

    # Plot training & validation accuracy values
    fig.add_subplot(1, 2, 2, title='Model accuracy', ylabel='Accuracy', xlabel='Epoch')
    plt.plot(history['accuracy'])
    plt.plot(history['val_accuracy'])
    plt.legend(['Train', 'Validation'], loc='best')

    plt.show()


def generateImages(generator, path, preserve_classes=False):
    from shutil import rmtree
    rmtree(path, ignore_errors=True)
    os.mkdir(path)

    cnt = 0
    for img in generator:
        if preserve_classes:
            classname = classes[int(img[1])]
            classpath = f'{path}\\{classname}'
            filepath = f'{classpath}\\res{cnt}.png'
            if not os.path.exists(classpath):
                os.mkdir(classpath)

            image.array_to_img(img[0][0], scale=True).save(filepath)

        cnt += 1
        if cnt >= generator.samples:
            break