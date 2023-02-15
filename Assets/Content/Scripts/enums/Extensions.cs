using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class EnumExtensions
    {
        public static IEnumerable<T> GetUniqueFlags<T>(this T flags) where T: Enum
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<T>())
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