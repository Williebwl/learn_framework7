using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIStudio.Framework
{
    public static class StringExtension
    {
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.None);
        }
    }
}
