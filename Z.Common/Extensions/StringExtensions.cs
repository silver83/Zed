using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Z.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// Compares the string against a given glob pattern.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="wildcard">The wildcard, where "*" means any sequence of characters, and "?" means any single character.</param>
        /// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
        public static bool Like(this string str, string wildcard)
        {
            return wildcard.AsLikeRegex().IsMatch(str);
        }

        /// <summary>
        /// Builds a regex to compares a string against a glob pattern.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="wildcard">The wildcard, where "*" means any sequence of characters, and "?" means any single character.</param>
        /// <returns><c>true</c> if the string matches the given pattern; otherwise <c>false</c>.</returns>
        public static Regex AsLikeRegex(this string wildcard)
        {
            return new Regex(
                "^" + Regex.Escape(wildcard).Replace(@"\*", ".*").Replace(@"\?", ".") + "$",
                RegexOptions.IgnoreCase | RegexOptions.Singleline
            );
        }
    }
}
