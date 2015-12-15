using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DIS.Data.DataContract
{
   public class ExportKeyList_R5
    {
        /// <summary>
        /// key xml export from 
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///  key xml export to 
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// key xml export date
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// key xml export keys
        /// </summary>
        //public List<KeyInformation> Keys { get; set; }
        public List<ExportKeyInfo> Keys { get; set; }
    }
}
