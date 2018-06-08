using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Facade;
using QA.Model;

namespace UnitTest_QA.Facade
{
    class Program
    {
        static void Main(string[] args)
        {
            Global.DefaultDataPath = "decode.xml";
            Global.DefaultRuleConfigPath = "rule.json";

            QA.Facade.Facade.InitializeRules();

            if (QA.Facade.Facade.Rules != null)
            {
                foreach (string key in QA.Facade.Facade.Rules.Keys)
                {
                    Console.WriteLine(key);
                }
            }

            QA.Facade.Facade.AddRule(new ValidationRuleItem() {

                FieldName = "ProductKeyID",
                FieldValue = 3423327065671,
                GroupName = "DEFAULT",
                RuleType = RuleType.EqualTo
            });

            QA.Facade.Facade.AddRule(new ValidationRuleItem()
            {

                FieldName = "ProcessorModel",
                FieldValue = "Intel(R) Core(TM) i5-6200U CPU @ 2.30GHz",
                GroupName = "DEFAULT",
                RuleType = RuleType.EqualTo
            });

            QA.Facade.Facade.InstantiateInputData();

            if (QA.Facade.Facade.Data != null)
            {
                foreach (string key in QA.Facade.Facade.Data.Keys)
                {
                    Console.WriteLine(key);
                }
            }

            QA.Facade.Facade.ValidateData();

            if (QA.Facade.Facade.ResultDetails != null)
            {
                //foreach (var result in QA.Facade.Facade.ResultDetails)
                //{
                //    Console.WriteLine(String.Format("{0}:{1}:{2}:{3}", result.FieldName, result.FieldValue, result.RuleType, result.IsPassed));
                //}

                foreach (var key in QA.Facade.Facade.ResultDetails.Keys)
                {
                    foreach (var result in QA.Facade.Facade.ResultDetails[key])
                    {
                        Console.WriteLine(String.Format("{0}:{1}:{2}:{3}", result.FieldName, result.FieldValue, result.RuleType, result.IsPassed));
                    }
                }

                string xml = QA.Facade.Facade.GetResultXml(new object[] { QA.Facade.Facade.Results, QA.Facade.Facade.ResultDetails });

                Console.WriteLine(xml);
            }

            Console.WriteLine("Ready.");

            Console.Read();
        }
    }
}
