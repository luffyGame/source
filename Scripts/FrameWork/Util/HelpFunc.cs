using System;
using System.Collections.Generic;

namespace FrameWork
{
    public static class HelpFunc
    {
        public static List<KeyValuePair<T, K>> SortDicitionary<T,K>(Dictionary<T, K> dict,
            Comparison<KeyValuePair<T,K>> compare)
        {
            if (dict.Count > 0)
            {
                List<KeyValuePair<T, K>> lst = new List<KeyValuePair<T, K>>(dict);
                lst.Sort(compare);
                return lst;
            }
            return null;
        }
    }
}
