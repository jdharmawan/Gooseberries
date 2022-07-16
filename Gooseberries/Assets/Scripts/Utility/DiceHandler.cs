using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
    public static class DiceHandler
    {
        public static T Roll<T>(this IList<T> list)
        {
            Random rand = new Random();
            int index = rand.Next(0, list.Count);
            return list[index];
        }


    }
}

