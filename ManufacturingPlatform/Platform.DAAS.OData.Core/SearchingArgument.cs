using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.DAAS.OData.Core
{
    public class SearchingArgument
    {
        public string FieldName { get; set; }
        public object FieldValue { get; set; }

        public OperatorEnum Operator { get; set; }

        public LogicalOperatorEnum LogicalOperator { get; set; }
    }

    public enum OperatorEnum
    {
        EqualTo = 0,
        NotEqualTo = 1,
        GreaterThan = 2,
        GreaterThanOrEqualTo = 3,
        In = 4,
        NotIn = 5,
        Is = 6,
        IsNot = 7,
        LessThan = 8,
        LessThanOrEqualTo = 9,
        StartsWith = 10,
        NotStartWith = 11,
        EndsWith = 12,
        NotEndWith = 13,
        Includes = 14,
        NotInclude = 15
    }

    public enum LogicalOperatorEnum
    {
        And = 0,
        Or = 1
    }
}
