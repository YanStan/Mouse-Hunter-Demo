import json


class BoundBox:
    def __init__(self, description, image_label, confidence, coords):
        self.description = description
        self.image_label = image_label
        self.confidence = confidence
        self.coords = coords


class BoundBoxEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, BoundBox):
            return obj.__dict__
        return json.JSONEncoder.default(self, obj)
