using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts
{
    public class Utilities
    {
        private static Dictionary<int, bool> UniqueMap = new Dictionary<int, bool>();
        /// <summary>
        /// Get Unique ID
        /// </summary>
        /// <returns></returns>
        public static int GetUniqueID()
        {
            var rng = new Random();
            var id = rng.Next(10000000, 99999999);

            while (UniqueMap.ContainsKey(id))
            {
                id = rng.Next(10000000, 99999999);
            }

            UniqueMap[id] = true;

            return id;

        }
    }
}
