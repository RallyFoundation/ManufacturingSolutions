using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DISOpenDataCloud.Models
{
    public class OperationListViewModel : List<OperationViewModel>
    {
        public List<string> DataTypes { get; set; }
    }
}