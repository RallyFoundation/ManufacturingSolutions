﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;
using QA.Model;

namespace QA.Rule
{
    public class ReferenceRule : IRule
    {
        public string FieldName { get; set; }
        public string GroupName { get; set; }
        public string ReferenceFieldName { get; set; }
        public string ExpectedValue { get; set; }

        public bool Check(IDictionary<string, object> Pairs, out object Result)
        {
            Result result = new Result()
            {
                FieldName = FieldName,
                RuleType = RuleType.EqualTo
            };

            Result = result;

            try
            {
                if (Pairs == null)
                {
                    result.IsPassed = false;
                    return false;
                }

                if (!Pairs.ContainsKey(FieldName))
                {
                    result.IsPassed = false;
                    return false;
                }

                result.FieldValue = Pairs[FieldName];
                result.IsPassed = ((string)Pairs[ReferenceFieldName] == ExpectedValue);
                return result.IsPassed;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}