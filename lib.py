from keras.models import load_model
import keras
import tensorflow as tf

import math
import numpy as np 
import matplotlib.pyplot as plt
import pickle


img_width = 150
img_height = 150

dataset_name = 'lego_new_converted'
classes = ['1x4 flat','2x10 flat']

# Paths
dataset_dir  = f'datasets/{dataset_name}'
train_dir    = f'{dataset_dir}/train'
model_path      = f'{dataset_dir}/model.h5'
history_path = f'{dataset_dir}/history.pickle'
session_path = f'{dataset_dir}/session.ckpt'



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