using System.Collections.Generic;
using System.Linq;

namespace FundacionOlivar.DDD.Utiles
{
    public static class HashCodeHelper
    {
        public static int CombineHashCodes(IEnumerable<object> objs)
        {
            //unchecked
            //{
            //    var hash = 17;
            //    foreach (var obj in objs)
            //    {
            //        //hash = hash * 23 + (obj != null ? obj.GetHashCode() : 0);
            //        hash = hash * 23 + obj?.GetHashCode() ?? 0;
            //    }
            //    return hash;
            //}

            //unchecked   //allow overflow
            //{
            //    int hash = 17;
            //    foreach (var o in GetAtomicValues())
            //    {
            //        hash = HashValue(hash, o);
            //    }

            //    return hash;
            //}
            return objs.Aggregate(17, (current, obj) =>
           {
               unchecked
               {
                   return HashValue(current, obj); // current * 23 + (obj?.GetHashCode() ?? 0);
               }
           });

        }


        public static int CombineHashCodeParams(params object[] objs)
        {
            return CombineHashCodes(objs);
        }


        private static int HashValue(int seed, object value)
        {
            //var currentHash = (value != null)
            //    ? value.GetHashCode()
            //    : 0;

            //return seed * 23 + currentHash;
            return seed * 23 + (value?.GetHashCode() ?? 0);
        }
    }
}
