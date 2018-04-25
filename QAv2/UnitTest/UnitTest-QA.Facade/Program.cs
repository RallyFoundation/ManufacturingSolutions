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
            Global.DefaultDataPath = "oa3.Report.xml";
            Global.DefaultRuleConfigPath = "rule.json";

            QA.Facade.Facade.InitializeRules();
        }
    }
}
