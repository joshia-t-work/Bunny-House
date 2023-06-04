import os
import cv2
import numpy as np

# Get the directory where the script is located
script_directory = os.path.dirname(os.path.realpath(__file__))

# Get a list of all the items in the directory
difficulties = os.listdir(script_directory)

# Loop through the items and print the folders
for difficulty in difficulties:
    difficulty_path = os.path.join(script_directory, difficulty)
    if os.path.isdir(difficulty_path):
        with open(os.path.join(difficulty_path, "LIST BARANG.txt"), "r") as file:
            items_image_path = os.path.join(difficulty_path, "BARANG ALL.png")
            if (os.path.exists(items_image_path)):
                with open(os.path.join(difficulty_path, "positions.txt"), "w") as outputfile:
                    all_items_picture = cv2.imread(items_image_path)
                    picitems_shape = all_items_picture.imag.shape
                    contents = file.read()
                    items = contents.split('\n')
                    for item in items:
                        item_path = os.path.join(difficulty_path, f"{item.strip().upper()}.png")
                        if (os.path.exists(item_path)):
                            # Load the big picture and the object image
                            object_image = cv2.imread(item_path)

                            # Use template matching to find the position of the object in the big picture
                            result = cv2.matchTemplate(all_items_picture, object_image, cv2.TM_CCOEFF_NORMED)
                            min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(result)
                            top_left = max_loc
                            bottom_right = (top_left[0] + object_image.shape[1], top_left[1] + object_image.shape[0])
                            def divy(a, b):
                                return (a[0] / b[1], a[1]/ b[0])
                            outputfile.write(f"{item.strip().upper()}|{divy(top_left, picitems_shape)}|{divy(bottom_right, picitems_shape)}\n")
                        else:
                            print(f"Not found {item.strip()} in {difficulty_path}")
            else:
                print(f"Missing BARANG ALL.png in {difficulty_path}")