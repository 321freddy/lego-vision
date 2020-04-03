from keras.preprocessing.image import ImageDataGenerator, image

import os
import os.path

import numpy as np 
import scipy.ndimage
import matplotlib.pyplot as plt
import pickle
import lib
from lib import *

model,history = lib.load()
# lib.plot_history(history)


def removeBackground(img):
    img = np.array(img,dtype=np.uint8)

    # replace black areas with white
    mask = (img < 5)
    mask = scipy.ndimage.binary_dilation(mask)
    img[mask] = 255 # white
    # img[mask] = np.interp(np.flatnonzero(mask), np.flatnonzero(~mask), img[~mask])

    # increase contrast
    min=np.min(img)
    max=np.max(img)

    # Make a LUT (Look-Up Table) to translate image values
    LUT=np.zeros(256,dtype=np.uint8)
    LUT[min:max+1]=np.linspace(start=0,stop=255,num=(max-min)+1,endpoint=True,dtype=np.uint8)
    img = LUT[img]

    return np.array(img, dtype=np.float)


datagen = ImageDataGenerator(
                # samplewise_center=True,
                # samplewise_std_normalization=True,
                # zca_whitening=True,
                brightness_range=[2.0,2.0],
                rescale=1./255,
                fill_mode="constant",
                cval=0,
                preprocessing_function=removeBackground,
            )


def classify(img_path, fig=None, rows=1, cols=1, i=1):
    img = image.load_img(
        img_path,
        target_size = (img_height, img_width),
        color_mode = "grayscale"
    )

    imgArray = image.img_to_array(img)
    imgArray = datagen.random_transform(imgArray)
    imgArray = datagen.standardize(imgArray)
    imgArray = np.expand_dims(imgArray, 0)

    predict = model.predict(imgArray)
    predict_classes = model.predict_classes(imgArray)
    result = [classes[i] for i in predict_classes]

    np.set_printoptions(precision=2,suppress=True)
    print (f'{img_path}  \t-->\tpredict: {predict} \tpredict_classes: {predict_classes} \tclass: {result}')
    
    if fig is not None:
        fig.add_subplot(rows, cols, i, title=result)
        plt.imshow(image.array_to_img(imgArray[0,...]), cmap='gray', vmin = 0, vmax = 255)


def classifyFolder(path):
    col = 0
    skipped=0
    for dirpath, dirnames, filenames in os.walk(path):
        for filename in [f for f in filenames if f.endswith(".png")]:
            if skipped >= skip:
                classify(os.path.join(dirpath, filename), fig=fig, rows=rows, cols=cols, i=row*cols+col+1)
                col = col + 1
                if col >= cols:
                    return
            else:
                skipped = skipped + 1

print('running classification....')
fig = plt.figure(figsize=(20,4))

rows = len(classes) # num classes
cols = 10           # num pics per class
skip = 30
for row in range(rows):
    classifyFolder(f'{train_dir}\\{classes[row]}')
print('done')

plt.subplots_adjust(wspace=0.5, hspace=0.5, left=0.03, right=1-0.03)
plt.show()