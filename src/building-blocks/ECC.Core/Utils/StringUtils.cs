using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECC.Core.Utils
{
    public static class StringUtils
    {
        public static string OnlyNumbers(this string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }
    }
}
