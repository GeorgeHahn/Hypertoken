import clr

from System import Text
from System.Text import UTF8Encoding

class Version(object):
	pass
ParserScript = Version()

ParserScript.Name = "George Hahn"
ParserScript.Version = "George Hahn"
ParserScript.Author = "George Hahn"

def Parse(packet):
    encoder = UTF8Encoding()
    return encoder.GetString(packet) + "."