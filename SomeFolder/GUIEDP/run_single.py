import json
from datetime import time
from os.path import join as pjoin
import cv2
import os
import numpy as np
import sys
import win32pipe, win32file, pywintypes

from configparser import ConfigParser
from PIL import Image

from bound_box import BoundBox, BoundBoxEncoder
from lobe import ONNXModel


def resize_height_by_longest_edge(img_path, resize_length=800):
    org = cv2.imread(img_path)
    height, width = org.shape[:2]
    if height > width:
        return resize_length
    else:
        return int(resize_length * (height / width))


def color_tips():
    color_map = {'Text': (0, 0, 255), 'Compo': (0, 255, 0), 'Block': (0, 255, 255), 'Text Content': (255, 0, 255)}
    board = np.zeros((200, 200, 3), dtype=np.uint8)

    board[:50, :, :] = (0, 0, 255)
    board[50:100, :, :] = (0, 255, 0)
    board[100:150, :, :] = (255, 0, 255)
    board[150:200, :, :] = (0, 255, 255)
    cv2.putText(board, 'Text', (10, 20), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 0), 2)
    cv2.putText(board, 'Non-text Compo', (10, 70), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 0), 2)
    cv2.putText(board, "Compo's Text Content", (10, 120), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 0), 2)
    cv2.putText(board, "Block", (10, 170), cv2.FONT_HERSHEY_SIMPLEX, 0.5, (0, 0, 0), 2)
    ##cv2.imshow('colors', board)


def run_singlescreen_processing(file_name, screen_panel_num, args_array=["10", "5", "600", "False"]):
    print("start")
    '''
        ele:min-grad: gradient threshold to produce binary map         
        ele:ffl-block: fill-flood threshold
        ele:min-ele-area: minimum area for selected elements 
        ele:merge-contained-ele: if True, merge elements contained in others
        text:max-word-inline-gap: words with smaller distance than the gap are counted as a line
        text:max-line-gap: lines with smaller distance than the gap are counted as a paragraph

        Tips:
        1. Larger *min-grad* produces fine-grained binary-map while prone to over-segment element to small pieces
        2. Smaller *min-ele-area* leaves tiny elements while prone to produce noises
        3. If not *merge-contained-ele*, the elements inside others will be recognized, while prone to produce noises
        4. The *max-word-inline-gap* and *max-line-gap* should be dependent on the input image size and resolution
        
        mobile: {'min-grad':4, 'ffl-block':5, 'min-ele-area':50, 'max-word-inline-gap':6, 'max-line-gap':1}
        web   : {'min-grad':3, 'ffl-block':5, 'min-ele-area':25, 'max-word-inline-gap':4, 'max-line-gap':4}
    '''
    #we need this cause bool("string") checks if string have any chars.
    if args_array[3] == "False":
        args_array[3] = ""
    key_params = {'min-grad':int(args_array[0]), 'ffl-block':int(args_array[1]), 'min-ele-area':int(args_array[2]),
                  'merge-contained-ele':bool(args_array[3]), 'merge-line-to-paragraph':False, 'remove-bar':False}


    if getattr(sys, 'frozen', False):
        app_path = os.path.dirname(sys.executable)
    elif __file__:
        app_path = os.path.dirname(os.path.realpath(__file__))

    #GET INPUT IMAGE NAME FROM CONFIG
    config = ConfigParser()
    config.read(os.path.join(app_path, 'config.cfg'))
    path_to_input_dir = config['paths']['path_to_input_dir']
    path_to_output_dir = config['paths']['path_to_output_dir']

    input_path_img = os.path.join(app_path, path_to_input_dir, file_name)
    output_root = os.path.join(app_path, path_to_output_dir)
    #
    print("Path to input image is: " + input_path_img)
    print("Path to output folder is: " + output_root)
    #
    #resized_height = resize_height_by_longest_edge(input_path_img, resize_length=800)
    color_tips()

    is_ip = True
    is_clf = False
    is_ocr = False
    is_merge = False

    if is_ocr:
        import detect_text.text_detection as text
        os.makedirs(pjoin(output_root, 'ocr'), exist_ok=True)
        text.text_detection(input_path_img, output_root, show=True)

    if is_ip:
        import detect_compo.ip_region_proposal as ip
        os.makedirs(pjoin(output_root, 'ip'), exist_ok=True)
        # switch of the classification func
        classifier = None
        if is_clf:
            classifier = {}
            from cnn.CNN import CNN
            # classifier['Image'] = CNN('Image')
            classifier['Elements'] = CNN('Elements')
            # classifier['Noise'] = CNN('Noise')
        box_coords = ip.compo_detection(input_path_img, output_root, key_params,
                           classifier=classifier, show=True)
        i = 0;
        bound_boxes = []

        dir_to_del_files = output_root + f"\\splits"
        for file in os.listdir(dir_to_del_files):
            os.remove(os.path.join(dir_to_del_files, file))

        for box_coord in box_coords:
            with Image.open(input_path_img) as im:
                # The crop method from the Image module takes four coordinates as input.
                # The right can also be represented as (left+width)
                # and lower can be represented as (upper+height).
                box_width = box_coord[2] - box_coord[0]
                box_height = box_coord[3] - box_coord[1]
                if box_width > 8 and box_height > 2:
                    metaimg_name = f"{i}.jpg"
                    (left, upper, right, lower) = (box_coord[0], box_coord[1], box_coord[2], box_coord[3])
                    # Here the image "im" is cropped and assigned to new variable im_crop
                    im_crop = im.crop((left, upper, right, lower))
                    im_crop.save(output_root + f"\\splits\\{i}.jpg")

                    outputs = ONNXModel.execute_recognition(output_root, f"\\splits\\{i}.jpg", screen_panel_num)
                    f_predict = outputs['predictions'][0]
                    description = f_predict['label']
                    confidence = f_predict['confidence']
                    bound_box = BoundBox(description=description, image_label=metaimg_name, confidence=confidence, coords=box_coord)
                    bound_boxes.append(bound_box)
                i += 1
        json_string = json.dumps(bound_boxes, cls=BoundBoxEncoder, ensure_ascii=False)
        json_output = open(output_root + f"\\splitjson\\splits.json", 'w')
        json.dump(bound_boxes, json_output, cls=BoundBoxEncoder, ensure_ascii=False)
        return json_string


        # satellite_path = output_root + f"\\splitjson\\satellite.txt"
        # new_satel_path = output_root + f"\\splitjson\\s2.txt"
        # f = open(satellite_path, "w+")
        # f.close()
        # os.rename(satellite_path, new_satel_path)
        # os.chmod(satellite_path, os.stat.S_IRUSR | os.stat.S_IRGRP | os.stat.S_IROTH)
        #os.remove(new_satel_path)



    if is_merge:
        import detect_merge.merge as merge
        os.makedirs(pjoin(output_root, 'merge'), exist_ok=True)
        name = input_path_img.split('\\')[-1][:-4]
        compo_path = pjoin(output_root, 'ip', str(name) + '.json')
        ocr_path = pjoin(output_root, 'ocr', str(name) + '.json')
        merge.merge(input_path_img, compo_path, ocr_path, pjoin(output_root, 'merge'),
                    is_remove_bar=key_params['remove-bar'], is_paragraph=key_params['merge-line-to-paragraph'], show=True)
