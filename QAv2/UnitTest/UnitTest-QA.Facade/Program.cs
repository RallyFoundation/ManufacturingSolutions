using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Facade;

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

            QA.Facade.Facade.InstantiateInputData();

            if (QA.Facade.Facade.Data != null)
            {
                foreach (string key in QA.Facade.Facade.Data.Keys)
                {
                    Console.WriteLine(key);
                }
            }

            Console.WriteLine("Ready.");

            Console.Read();
        }
    }
}
