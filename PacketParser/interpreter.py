import clr

from System import Text
from System.Text import UTF8Encoding

def Parse(packet):
    encoder = UTF8Encoding()
    return encoder.GetString(packet)