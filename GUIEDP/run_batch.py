import multiprocessing
import glob
import time
import json
from tqdm import tqdm
from os.path import join as pjoin, exists
import cv2
import os
import detect_compo.ip_region_proposal as ip


def resize_height_by_longest_edge(img_path, resize_length=800):
    org = cv2.imread(img_path)
    height, width = org.shape[:2]
    if height > width:
        return resize_length
    else:
        return int(resize_length * (height / width))


if __name__ == '__main__':
    # initialization
    app_path = os.path.dirname(os.path.realpath(__file__))

    input_img_root = app_path + "\\data\\input"
    output_root = app_path + "\\data\\output"

    #NOT AVILABLE!
    #data = json.load(open('E:/Mulong/Datasets/rico/instances_test.json', 'r'))

    input_imgs = [
        app_path + '\\data\\input\\1.jpg',
        app_path + '\\data\\input\\2.jpg',
        app_path + '\\data\\input\\3.jpg',
        app_path + '\\data\\input\\4.jpg',
        app_path + '\\data\\input\\5.jpg',
        app_path + '\\data\\input\\6.jpg',
        app_path + '\\data\\input\\7.jpg',
        app_path + '\\data\\input\\8.jpg',
        app_path + '\\data\\input\\9.jpg',
        app_path + '\\data\\input\\10.jpg',
        app_path + '\\data\\input\\11.jpg',
        app_path + '\\data\\input\\12.jpg',
        app_path + '\\data\\input\\13.jpg',
        app_path + '\\data\\input\\14.jpg',
    ]
    #input_imgs = [pjoin(input_img_root, img['file_name'].split('\\')[-1]) for img in data['images']]
    input_imgs = sorted(input_imgs, key=lambda x: int(x.split('\\')[-1][:-4]))  # sorted by index

    key_params = {'min-grad': 10, 'ffl-block': 5, 'min-ele-area': 50, 'merge-contained-ele': True,
                  'max-word-inline-gap': 10, 'max-line-ingraph-gap': 4, 'merge-line-to-paragraph':False,
                  'remove-top-bar': True}


    is_ip = True
    is_clf = False
    is_ocr = True
    is_merge = True

    # Load deep learning models in advance
    compo_classifier = None
    if is_ip and is_clf:
        compo_classifier = {}
        from cnn.CNN import CNN
        # compo_classifier['Image'] = CNN('Image')
        compo_classifier['Elements'] = CNN('Elements')
        # compo_classifier['Noise'] = CNN('Noise')
    ocr_model = None
    if is_ocr:
        import detect_text.text_detection as text

    # set the range of target inputs' indices
    num = 0
    start_index = 30800  # 61728
    end_index = 100000
    for input_img in input_imgs:
        resized_height = resize_height_by_longest_edge(input_img)
        index = input_img.split('\\')[-1][:-4]
        # if int(index) < start_index:
        #     continue
        if int(index) > end_index:
            break

        if is_ocr:
            text.text_detection(input_img, output_root, show=False)

        if is_ip:
            ip.compo_detection(input_img, output_root, key_params,  classifier=compo_classifier, resize_by_height=resized_height, show=False)

        if is_merge:
            import detect_merge.merge as merge
            os.makedirs(pjoin(output_root, 'merge'), exist_ok=True)
            name = input_img.split('\\')[-1][:-4]
            #compo_path = output_root + "\\ip\\" + str(name) + ".json"
            compo_path = pjoin(output_root, 'ip', str(name) + '.json')
            ocr_path = pjoin(output_root, 'ocr', str(name) + '.json')
            merge.merge(input_img, compo_path, ocr_path, pjoin(output_root, 'merge'),
                        is_remove_bar=key_params['remove-top-bar'], is_paragraph=key_params['merge-line-to-paragraph'],
                        show=True)

        num += 1
