using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Core;
using QA.Model;

namespace QA.Reducer
{
    public class ResultReducer : IReducer
    {
        public object Reduce(object Pairs, object Data)
        {
            IDictionary<string, Dictionary<string, bool>> groupedResults = Data as IDictionary<string, Dictionary<string, bool>>;

            Dictionary<string, object> results = Pairs as Dictionary<string, object>;

            if ((groupedResults != null) && (groupedResults.Count > 0))
            {
                if (results == null)
                {
                    results = new Dictionary<string, object>();
                }

                foreach (string field in groupedResults.Keys)
                {
                    foreach (string group in groupedResults.Keys)
                    {
                        if (!results.ContainsKey(field))
                        {
                            results.Add(field, groupedResults[field][group]);
                        }
                        else if(groupedResults[field][group] == true)
                        {
                            results[field] = true;
                        }
                    }
                }
            }

            return results;
        }
    }
}
