using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Logging
{
    public class LogItem
    {
        private string separator = ":\r\n";

        public LogItem()
        {
        }

        public LogItem(string Separator)
        {
            this.separator = Separator;
        }

        public string Title { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public string Level { get; set; }

        public DateTime TimeStamp { get; set; }

        public string MachineName { get; set; }

        public string UserName { get; set; }

        public override string ToString()
        {
            //return base.ToString();

            //return "Title=" + this.Title + this.separator + "Message=" + this.Message + this.separator + "Categroy=" + this.Category + this.separator + "Level=" + this.Level + this.separator + "TimeStamp=" + this.TimeStamp.ToString() + this.separator + "MachineName=" + this.MachineName + this.separator + "UserName=" + this.UserName;

            return "Title" +  this.separator + this.Title + "Message" + this.separator + this.Message  + "Categroy" + this.separator + this.Category  + "Level" + this.separator + this.Level + "TimeStamp" + this.separator + this.TimeStamp.ToString() + "MachineName" + this.separator + this.MachineName + "UserName" + this.separator + this.UserName;
        }
    }
}
