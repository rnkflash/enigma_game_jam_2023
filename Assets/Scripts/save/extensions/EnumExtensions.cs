using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem {
    static class EnumExtensions
    {
        public static IEnumerable<SaveTypes> GetUniqueFlags(this SaveTypes flags)
        {
            ulong flag = 1;
            foreach (var value in SaveTypes.GetValues(flags.GetType()).Cast<SaveTypes>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
            }
        }
    }
}