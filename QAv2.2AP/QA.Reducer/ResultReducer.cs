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
            IDictionary<string, Dictionary<string, List<bool>>> groupedResults = Data as IDictionary<string, Dictionary<string, List<bool>>>;

            IDictionary<string, Dictionary<string, bool>> groupedResultSums = new Dictionary<string, Dictionary<string, bool>>();

            IDictionary<string, bool> results = Pairs as IDictionary<string,bool>;

            bool groupResult = true;

            if ((groupedResults != null) && (groupedResults.Count > 0))
            {
                foreach (string field in groupedResults.Keys)
                {
                    foreach (string group in groupedResults[field].Keys)
                    {
                        for (int i = 0; i < groupedResults[field][group].Count; i++)
                        {
                            groupResult = groupedResults[field][group][i];

                            if (groupResult == false)
                            {
                                break;
                            }
                        }

                        //if (!results.ContainsKey(field))
                        if (!groupedResultSums.ContainsKey(field))
                        {
                            //results.Add(field, groupedResults[field][group]);
                            groupedResultSums.Add(field, new Dictionary<string, bool>() { { group, groupResult } });
                        }
                        else if(!groupedResultSums[field].ContainsKey(group))
                        {
                            //results[field] = true;
                            groupedResultSums[field].Add(group, groupResult);
                        }
                    }
                }

                if (results == null)
                {
                    results = new Dictionary<string, bool>();
                }

                if (groupedResultSums != null)
                {
                    foreach (string field in groupedResultSums.Keys)
                    {
                        foreach (string group in groupedResultSums[field].Keys)
                        {
                            if (!results.ContainsKey(field))
                            {
                                results.Add(field, groupedResultSums[field][group]);
                            }
                            else if(groupedResultSums[field][group] == true)
                            {
                                results[field] = true;
                            }
                        }
                    }
                }
            }

            return results;
        }
    }
}
