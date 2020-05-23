from keras.layers import Conv2D, MaxPooling2D, ZeroPadding2D, Lambda, SeparableConv2D, BatchNormalization
from keras.layers import Dense, Activation, Dropout, Flatten, AlphaDropout
from keras import optimizers
from keras.models import Sequential
from keras.preprocessing.image import ImageDataGenerator, image
from keras.callbacks import ModelCheckpoint,EarlyStopping,TensorBoard,CSVLogger,ReduceLROnPlateau,LearningRateScheduler
import math
import numpy as np 
import scipy.ndimage
import matplotlib.pyplot as plt
import lib
from lib import *


def rgb2gray(rgb):
    alpha = rgb[...,3]
    gray = np.dot(rgb[...,:3], [0.2989, 0.5870, 0.1140])
    return np.dstack((gray,gray,gray,alpha)).astype(np.uint8)

def removeBackground(img):
    img = np.array(img,dtype=np.uint8)
    img = rgb2gray(img)

    # replace black areas with white
    mask = (img[...,3] < 255)
    # mask = scipy.ndimage.binary_dilation(mask)
    img[mask]  = 180 # white
    img[...,3] = 255
    # img[mask] = np.interp(np.flatnonzero(mask), np.flatnonzero(~mask), img[~mask])

    gray = img[:,:,0:1] # gray only

    #bright mask
    # bright = image.apply_brightness_shift(gray, 2).astype(int)
    # # mask = ~(bright == 255)
    # # mask = ~scipy.ndimage.binary_closing(mask, structure=np.ones((50,50,1)))

    # mean = np.mean(bright)
    # threshold = 230
    # factor_range = 0.4
    # if mean > threshold:
    #     factor01 = (255 - mean) / (255 - threshold) # Zahl zw. 0 und 1
    #     bright = image.apply_brightness_shift(gray, 2-factor_range + factor01*factor_range).astype(int)

    # increase contrast
    min=np.min(gray)
    max=np.max(gray)

    # Make a LUT (Look-Up Table) to translate image values
    LUT=np.zeros(256,dtype=np.uint8)
    LUT[min:max+1]=np.linspace(start=0,stop=255,num=(max-min)+1,endpoint=True,dtype=np.uint8)
    gray = LUT[gray]

    # gray[mask] = 255
    # img[...,0:3] = bright
    img[...,0:3] = gray
    return np.array(img, dtype=np.float)


datagen = ImageDataGenerator(
                # samplewise_center=True,
                # samplewise_std_normalization=True,
                # zca_whitening=True,
                rescale=1./255,
                # fill_mode="constant",
                # cval=0,
                # brightness_range=[2.0,2.0],
                preprocessing_function=removeBackground,
            )

generator = datagen.flow_from_directory(
                directory     = prepare_dir,
                target_size   = (img_height, img_width),
                classes       = classes,
                class_mode    = "sparse",
                color_mode    = "rgba",
                shuffle       = True,
                batch_size    = 1,
                subset        = "training",)


lib.generateImages(generator, train_dir, preserve_classes=True)