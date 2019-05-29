using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;

namespace QA.Mapper
{
    public class Decoded4KHHDataPair19H1Mapper : IMapper
    {
        public object Map(object Data)
        {
            if (Data == null)
            {
                return null;
            }

            IDictionary<string, object> result = Data as Dictionary<string, object>;

            if (result == null)
            {
                return null;
            }
            
            string primaryDiskSNKeyName = this.GetPrimaryDiskSNKeyName(result);

            if (!String.IsNullOrEmpty(primaryDiskSNKeyName) && primaryDiskSNKeyName.EndsWith(".DiskSerialNumber"))
            {
                string primaryDiskKeyPrefix = primaryDiskSNKeyName.Substring(0, primaryDiskSNKeyName.IndexOf("."));

                string primaryDiskTypeKeyName = String.Format("{0}.DiskType", primaryDiskKeyPrefix);
                string primaryDiskTotalCapacityKeyName = String.Format("{0}.DiskCapacity", primaryDiskKeyPrefix);

                if (!result.ContainsKey("PrimaryDiskType") && result.ContainsKey(primaryDiskTypeKeyName))
                {
                    result.Add("PrimaryDiskType", result[primaryDiskTypeKeyName]);
                }

                if (!result.ContainsKey("DiskTotalCapacityKeyName") && result.ContainsKey(primaryDiskTotalCapacityKeyName))
                {
                    result.Add("PrimaryDiskTotalCapacity", result[primaryDiskTotalCapacityKeyName]);
                }

                if (!result.ContainsKey("DiskSerialNumber") && result.ContainsKey(primaryDiskSNKeyName))
                {
                    result.Add("DiskSerialNumber", result[primaryDiskSNKeyName]);
                }

            }

            return result;
        }

        private string GetPrimaryDiskSNKeyName(IDictionary<string, object> data)
        {
            /*For purposes of price differentiation scenarios, this field is undocumented and must not be used. 
            To provide full information, the DiskSSNKernel may be used for device matching, similar to SmbiosSystemSerialNumber, etc. 
            The value of this field is related to the system disk at the time of boot. As such, this may have wrong value if data is collected when booted to a different image (e.g. WinPE or FactoryOS). 
            This field may change its name, the format of the data or completely disappear without warning. Please do not use.*/

            //object kernelDiskSN = (data.ContainsKey("DiskSSNKernel") && data["DiskSSNKernel"] != null) ? data["DiskSSNKernel"] : null; 

            //if (kernelDiskSN != null)
            //{
            //    foreach (string key in data.Keys)
            //    {
            //        if ((data[key] is String) && ((string)data[key] == (string)kernelDiskSN) && (key != "DiskSSNKernel"))
            //        {
            //            return key;
            //        }
            //    }
            //}

            if (data.ContainsKey("Disk1.DiskSerialNumber"))
            {
                return "Disk1.DiskSerialNumber"; //Disk1 is always the Primary no matter how much internal disks are there-Rally
            }

            return null; 
        }
    }
}
