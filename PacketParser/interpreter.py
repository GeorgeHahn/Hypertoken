import clr

from System import *
from System.Text import UTF8Encoding

def Parse(packet):
    encoder = UTF8Encoding()
    return encoder.GetString(packet)