﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Model
{
    public enum RuleType
    {
        EqualTo = 0,
        NotEqualTo = 1,
        InRange = 2,
        OutOfRange = 3,
        InAndOutOfRange = 4,
        StringLength = 5,
        Min = 6,
        Max = 7,
        MinAndMax = 8,
        NotNull = 9,
        Reference = 10
    }
}
