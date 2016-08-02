using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core.Security
{
    public interface IEncryptionManager
    {
        byte[] AesEncryption(byte[] Data, byte[] Key, byte[] IV);

        byte[] AesDecryption(byte[] Data, byte[] Key, byte[] IV);
    }
}
