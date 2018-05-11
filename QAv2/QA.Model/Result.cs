using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Model
{
    public class Result
    {
        public string FieldName { get; set; }

        public RuleType RuleType { get; set; }

        public object FieldValue { get; set; }

        public bool IsPassed { get; set; }

        public object Description { get; set; }
    }
}
