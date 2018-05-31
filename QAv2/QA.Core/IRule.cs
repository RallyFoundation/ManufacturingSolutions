using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Core
{
    public interface IRule
    {
        string FieldName { get; set; }

        string GroupName { get; set; }

        string[] QuotedFields { get; set; }

        bool Check(IDictionary<string, object> Pairs, out object Result);
    }
}
