using System;
using System.Collections.Generic;
using System.Linq;

namespace Higgs.Core
{
    public static class Enum<T>
    {
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static IEnumerable<byte> GetBytes()
        {
            return Enum.GetValues(typeof(T)).Cast<byte>();
        }
    } 
}
