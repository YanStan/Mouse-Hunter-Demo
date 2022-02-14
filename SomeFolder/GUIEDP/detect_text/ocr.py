import cv2
import os
import codecs
import requests
import json
from base64 import b64encode
import time


def Google_OCR_makeImageData(imgpath):
    with open(imgpath, 'rb') as f:
        ctxt_predecoded = b64encode(f.read()).decode()
        #DECODING in unicode_escape
        ctxt_decoded = codecs.decode(ctxt_predecoded, 'unicode_escape')
        img_req = {
            'image': {
                'content': ctxt_decoded
            },
            'features': [{
                'type': 'DOCUMENT_TEXT_DETECTION',
                # 'type': 'TEXT_DETECTION',
                'maxResults': 1
            }]
        }
    return json.dumps({"requests": img_req}, ensure_ascii=False).encode('utf8')


def ocr_detection_google(imgpath):
    start = time.process_time()
    url = 'https://vision.googleapis.com/v1/images:annotate'
    api_key = 'AIzaSyDUc4iOUASJQYkVwSomIArTKhE2C6bHK8U'             # *** Replace with your own Key ***
    imgdata = Google_OCR_makeImageData(imgpath)
    response = requests.post(url,
                             data=imgdata,
                             params={'key': api_key},
                             headers={'Content_Type': 'application/json'})
    # print('*** Text Detection Time Taken:%.3fs ***' % (time.clock() - start))
    print("*** Please replace the Google OCR key at detect_text/ocr.py line 28 with your own (apply in https://cloud.google.com/vision) ***")
    if response.json()['responses'] == [{}]:
        # No Text
        return None
    else:
        return response.json()['responses'][0]['textAnnotations'][1:]
