from pyftpdlib.authorizers import DummyAuthorizer
from pyftpdlib.handlers import FTPHandler
from pyftpdlib.servers import FTPServer
#from pyftpdlib.servers import MultiprocessFTPServer
import json

with open('config.json') as json_data_file:
    config = json.load(json_data_file)

authorizer = DummyAuthorizer()
authorizer.add_user(config["server"]["user"], config["server"]["password"], config["server"]["home_dir"], perm=config["server"]["permissions"])
#authorizer.add_anonymous("D:\FTPTest")

handler = FTPHandler
handler.authorizer = authorizer
handler.passive_ports = [config["server"]["passive_port"]]

server = FTPServer((config["server"]["host"], config["server"]["port"]), handler) #MultiprocessFTPServer(("", 21), handler) 
server.serve_forever()
