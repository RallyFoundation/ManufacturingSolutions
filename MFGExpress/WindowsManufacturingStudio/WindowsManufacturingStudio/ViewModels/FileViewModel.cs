using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsManufacturingStudio.ViewModels
{
    public class FileViewModel
    {
        public string ID { get; set; }

        public string Url { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string AuthenticationScheme { get; set; }

        public string Path { get; set; }

        public string Content { get; set; }

        public byte[] Bytes { get; set; }

        public string Description { get; set; }
    }
}
