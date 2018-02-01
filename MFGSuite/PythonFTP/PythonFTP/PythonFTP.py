from pyftpdlib.authorizers import DummyAuthorizer
from pyftpdlib.handlers import FTPHandler
from pyftpdlib.servers import FTPServer
#from pyftpdlib.servers import MultiprocessFTPServer

authorizer = DummyAuthorizer()
authorizer.add_user("root", "12345", "D:\WDS-Images", perm="elradfmwMT")
authorizer.add_anonymous("D:\FTPTest")

handler = FTPHandler
handler.authorizer = authorizer
handler.passive_ports = [5001]

server = FTPServer(("", 21), handler) #MultiprocessFTPServer(("", 21), handler) 
server.serve_forever()
