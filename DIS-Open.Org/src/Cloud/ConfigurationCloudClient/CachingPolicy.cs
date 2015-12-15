using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DISConfigurationCloud.Client
{
    public enum CachingPolicy
    {
        RemoteOnly = 0,
        LocalOnly = 1,
        MergedAll = 2,
        MergedRemoteFirst = 3,
        MergedLocalFirst = 4,
        IntersectedRemoteFirst = 5,
        IntersectedLocalFirst = 6
    }
}
