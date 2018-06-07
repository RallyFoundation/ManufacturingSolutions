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

            IDictionary<string, bool> results = Pairs as IDictionary<string,bool>;

            if ((groupedResults != null) && (groupedResults.Count > 0))
            {
                if (results == null)
                {
                    results = new Dictionary<string, bool>();
                }

                foreach (string field in groupedResults.Keys)
                {
                    foreach (string group in groupedResults[field].Keys)
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
