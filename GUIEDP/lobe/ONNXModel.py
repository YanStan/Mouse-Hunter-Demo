#  -------------------------------------------------------------
#   Copyright (c) Microsoft Corporation.  All rights reserved.
#  -------------------------------------------------------------
"""
Skeleton code showing how to load and run the ONNX export package from Lobe.
"""

import argparse
import json
import os
from configparser import ConfigParser

import numpy as np
from PIL import Image
import onnxruntime as rt
#from onnxruntime.transformers.onnx_model import OnnxModel
#import onnx

EXPORT_MODEL_VERSION = 1


class ONNXModel:
    def __init__(self, model_dir) -> None:
        """Method to get name of model file. Assumes model is in the parent directory for script."""
        with open(os.path.join(model_dir, "signature.json"), "r") as f:
            self.signature = json.load(f)
        self.model_file = os.path.join(model_dir, self.signature.get("filename"))
        if not os.path.isfile(self.model_file):
            raise FileNotFoundError(f"Model file does not exist in {self.model_file}")
        # get the signature for model inputs and outputs
        self.signature_inputs = self.signature.get("inputs")
        self.signature_outputs = self.signature.get("outputs")
        self.session = None
        if "Image" not in self.signature_inputs:
            raise ValueError("ONNX model doesn't have 'Image' input! Check signature.json, and please report issue to Lobe.")
        # Look for the version in signature file.
        # If it's not found or the doesn't match expected, print a message
        version = self.signature.get("export_model_version")
        if version is None or version != EXPORT_MODEL_VERSION:
            print(
                f"There has been a change to the model format. Please use a model with a signature 'export_model_version' that matches {EXPORT_MODEL_VERSION}."
            )

    def load(self) -> None:
        """Load the model from path to model file"""
        # Load ONNX model as session.
        self.session = rt.InferenceSession(path_or_bytes=self.model_file)

    def predict(self, image: Image.Image) -> dict:
        """
        Predict with the ONNX session!
        """
        # process image to be compatible with the model
        img = self.process_image(image, self.signature_inputs.get("Image").get("shape"))
        # run the model!
        fetches = [(key, value.get("name")) for key, value in self.signature_outputs.items()]
        # make the image a batch of 1
        feed = {self.signature_inputs.get("Image").get("name"): [img]}
        outputs = self.session.run(output_names=[name for (_, name) in fetches], input_feed=feed)
        return self.process_output(fetches, outputs)

    def process_image(self, image: Image.Image, input_shape: list) -> np.ndarray:
        """
        Given a PIL Image, center square crop and resize to fit the expected model input, and convert from [0,255] to [0,1] values.
        """
        width, height = image.size
        # ensure image type is compatible with model and convert if not
        if image.mode != "RGB":
            image = image.convert("RGB")
        # center crop image (you can substitute any other method to make a square image, such as just resizing or padding edges with 0)
        if width != height:
            square_size = min(width, height)
            left = (width - square_size) / 2
            top = (height - square_size) / 2
            right = (width + square_size) / 2
            bottom = (height + square_size) / 2
            # Crop the center of the image
            image = image.crop((left, top, right, bottom))
        # now the image is square, resize it to be the right shape for the model input
        input_width, input_height = input_shape[1:3]
        if image.width != input_width or image.height != input_height:
            image = image.resize((input_width, input_height))

        # make 0-1 float instead of 0-255 int (that PIL Image loads by default)
        image = np.asarray(image) / 255.0
        # format input as model expects
        return image.astype(np.float32)

    def process_output(self, fetches: dict, outputs: dict) -> dict:
        # un-batch since we ran an image with batch size of 1,
        # convert to normal python types with tolist(), and convert any byte strings to normal strings with .decode()
        out_keys = ["label", "confidence"]
        results = {}
        for i, (key, _) in enumerate(fetches):
            val = outputs[i].tolist()[0]
            if isinstance(val, bytes):
                val = val.decode()
            results[key] = val
        confs = results["Confidences"]
        labels = self.signature.get("classes").get("Label")
        output = [dict(zip(out_keys, group)) for group in zip(labels, confs)]
        sorted_output = {"predictions": sorted(output, key=lambda k: k["confidence"], reverse=True)}
        return sorted_output


def execute_recognition(output_root, img_path, screen_panel_num):
    # parser = argparse.ArgumentParser(description="Predict a label for an image.")
    # parser.add_argument("image", help="Path to your image file.")
    # args = parser.parse_args()
    # Assume model is in the parent directory for this
    full_img_path = os.path.join(output_root + img_path)
    if os.path.isfile(full_img_path):
        image = Image.open(full_img_path)
        project_dir = os.path.join(output_root, "..\\..\\")

        config = ConfigParser()
        config.read(os.path.join(project_dir, 'config.cfg'))
        onnx_folder = config['paths'][f'path_to_{screen_panel_num}pan_onnx_dir']
        model_dir = os.path.join(project_dir, onnx_folder)

        #DECOMMENT TO CREATE REPAIRED NOERRRORED onnx!!! THEN REPLACE FILE AND TRY AGAIN
        #THIS PREVENTS ERROR:  Initializer X appears in graph inputs and will not be treated as constant value/weight.
        # input_onnx_path = os.path.join(model_dir, 'model.onnx')
        # output_onnx_path = os.path.join(model_dir, 'output_model.onnx')
        # model = onnx.load(input_onnx_path)
        # if model.ir_version < 4:
        #     print(
        #         'Model with ir_version below 4 requires to include initilizer in graph input'
        #     )
        #     return
        # inputs = model.graph.input
        # name_to_input = {}
        # for input in inputs:
        #     name_to_input[input.name] = input
        #
        # for initializer in model.graph.initializer:
        #     if initializer.name in name_to_input:
        #         inputs.remove(name_to_input[initializer.name])
        # onnx.save(model, output_onnx_path)

        model = ONNXModel(model_dir=model_dir)
        model.load()
        outputs = model.predict(image)
        return outputs
    else:
        return f"Couldn't find image file {full_img_path}\n"
