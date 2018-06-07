using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;

namespace QA.Reducer
{
    public class OA3ToolReducer : IReducer
    {
        public object Reduce(object Pairs, object Data)
        {
            IDictionary<string, bool> results = Pairs as IDictionary<string, bool>;
            //IDictionary<string, Dictionary<string, bool>> groupedResults = Data as IDictionary<string, Dictionary<string, bool>>;

            if (results != null)
            {
                if (results.ContainsKey("OA3Tool") && results.ContainsKey("ToolBuild") && results.ContainsKey("ToolVersion"))
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

            return results;
        }
    }
}
