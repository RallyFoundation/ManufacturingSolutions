using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsManufacturingCloud.Models
{
    public class WdsApiConfig
    {
        public string WdsApiUrl { get; set; }
        public string WdsApiUserName { get; set; }
        public string WdsApiPassword { get; set; }
        public string FileServiceUrl { get; set; }
        public string FileServiceUserName { get; set; }
        public string FileServicePassword { get; set; }
        public string WebSocketAddress { get; set; }
        public int WebSocketPort { get; set; }
        public string WebSocketUrl { get; set; }
        public string SocketIOClientUrl { get; set; }
        public string InstallImageRepository { get; set; }
        public string BootImageRepository { get; set; }
        public string FFUImageRepository { get; set; }
        public string LogRepository { get; set; }
    }
}
