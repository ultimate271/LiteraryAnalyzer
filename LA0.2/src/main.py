import json
import os
from LAMetadata import LAMetadata

MyMetadata = LAMetadata('test1', 'test2')
mydict = MyMetadata.__dict__

f = open("metadata.json", "w")
f.write(json.dumps(MyMetadata.__dict__))
