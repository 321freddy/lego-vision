from keras.preprocessing.image import image

import numpy as np 
import matplotlib.pyplot as plt
import pickle
import lib
from lib import *

model,history = lib.load()
# lib.plot_history(history)


def classify(img_path, fig=None, rows=1, cols=1, i=1):
    img = image.load_img(
        img_path,
        target_size = (img_height, img_width),
        color_mode = "grayscale"
    )

    imgArray = np.expand_dims(image.img_to_array(img), 0)
    predict = model.predict(imgArray)
    predict_classes = model.predict_classes(imgArray)
    result = [classes[i] for i in predict_classes]

    print (f'{img_path} --> predict: {predict}  predict_classes: {predict_classes}  class: {result}')
    
    if fig is not None:
        fig.add_subplot(rows, cols, i, title=result)
        plt.imshow(img, cmap='gray', vmin = 0, vmax = 255)



print('running classification....')
fig = plt.figure(figsize=(20,4))

rows = len(classes) # num classes
cols = 10           # num pics per class
for row in range(rows):
    for col in range(cols):
        classify(f'{train_dir}/{classes[row]}/res{col+18}.png', fig=fig, rows=rows, cols=cols, i=row*cols+col+1)

plt.subplots_adjust(wspace=0.5, hspace=0.5, left=0.03, right=1-0.03)
plt.show()