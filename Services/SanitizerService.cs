using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotRDb.Web.Services
{
    public static class SanitizerService
    {
        private static readonly Func<string, bool> SingleQuotesAreUnbalanced =
            delegate(string str) { return str.Count(ch => ch == '\'')%2 != 0; };

        public static string SanitizeForSql(string str)
        {
            //Remove unbalanced single quotes
            //e.g. Name = "FakeName ' SELECT * FROM [dbo].[Account]"
            if (SingleQuotesAreUnbalanced(str))
            {
                str = str.Insert(str.IndexOf('\''), "'");
            }

            return str;
        }

        public static bool InputsNeedSanitizing(IEnumerable<string> strings)
        {
            return strings.Any(SingleQuotesAreUnbalanced);
        }
    }
}