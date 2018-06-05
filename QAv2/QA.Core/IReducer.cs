using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Core
{
    public interface IReducer
    {
        object Reduce(IDictionary<string, object> Pairs, object Data);
    }
}
