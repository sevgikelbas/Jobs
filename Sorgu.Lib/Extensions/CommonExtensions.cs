using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sorgu.Lib.Extensions
{
    public static class CommonExtensions
    {
        public static string ToNormalize(this string Source)
        {
            if (!string.IsNullOrEmpty(Source))
            {
                Regex rgx = new Regex("[^a-zA-Z0-9/-]");
                Source = rgx.Replace(Source, "");

                Source = Source.Replace("--", "");

                return Source;
            }

            return null;
        }
    }
}
