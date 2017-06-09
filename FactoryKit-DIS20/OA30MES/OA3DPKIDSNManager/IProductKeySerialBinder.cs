using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA3DPKIDSNManager
{
    public interface IProductKeySerialBinder
    {
        string Bind(long ProductKeyID, string SerialNumber);

        string Bind(long ProductKeyID, string SerialNumber, string TransactionID);

        void SetDBConnectionString(string DBConnectionString);

        void SetFilePath(string FilePath);

        void SetPersistencyMode(int Mode);
    }
}
