using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;
using QA.Model;

namespace QA.Reducer
{
    public class OA3ToolReducer : IReducer
    {
        public object Reduce(object Pairs, object Data)
        {
            IDictionary<string, bool> results = Pairs as IDictionary<string, bool>;
            //IDictionary<string, Dictionary<string, bool>> groupedResults = Data as IDictionary<string, Dictionary<string, bool>>;

            IDictionary<string, List<Result>> resultDetails = Data as IDictionary<string, List<Result>>;

            if (results != null)
            {
                if (!results.ContainsKey("OA3Tool"))
                {
                    results.Add("OA3Tool", true);
                }

                if (results.ContainsKey("ToolBuild") && results.ContainsKey("ToolVersion"))
                {
                    if ((results["ToolBuild"] == false) || (results["ToolVersion"] == false))
                    {
                        results["OA3Tool"] = false;
                    }  
                }
            }

            //foreach (string group in groupedResults["OA3Tool"].Keys)
            //{
            //    groupedResults["OA3Tool"][group] = results["OA3Tool"];
            //}

            if ((resultDetails != null) && resultDetails.ContainsKey("ToolBuild") && resultDetails.ContainsKey("ToolVersion"))
            {
                if (!resultDetails.ContainsKey("OA3Tool"))
                {
                    resultDetails.Add("OA3Tool", new List<Result>() { new Result() {

                         FieldName = "OA3Tool",
                         FieldValue = String.Format("{0}:{1}", resultDetails["ToolVersion"][0].FieldValue, resultDetails["ToolBuild"][0].FieldValue),
                         IsPassed = results["OA3Tool"],
                         RuleType = RuleType.EqualTo,
                         RuleInstance = new ValidationRuleItem(){
                              GroupName = (resultDetails["ToolVersion"][0].RuleInstance as ValidationRuleItem).GroupName,
                              FieldName = "OA3Tool",
                              FieldValue = String.Format("{0}:{1}", (resultDetails["ToolVersion"][0].RuleInstance as ValidationRuleItem).MinValue, (resultDetails["ToolBuild"][0].RuleInstance as ValidationRuleItem).FieldValue),
                              RuleType = RuleType.EqualTo,
                              QuotedFields = new string[]{ "ToolVersion", "ToolBuild"}
                         }
                    } });
                }
            }

            return results;
        }
    }
}
