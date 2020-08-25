using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    public static class Utility
    {
        public static String TrySubString(this String value, Int32 MaxLength)
        {
            value = (value ?? "");
            if (value.Length > MaxLength)
            {
                value = value.Substring(0, MaxLength);
            }
            return value;
        }

        public static string CleanInputString(string value, int MaxLength)
        {
            return CleanInputString(value, MaxLength, true, true, true, true, true, null);
        }

        public static string Parse(this string Value, int MaxLength, bool Letters = true, bool Digits = true, bool Symbol = true, bool Punctuation = true, bool WhiteSpace = true, char[] AllowedChars = null)
        {
            return CleanInputString(Value, MaxLength, Letters, Digits, Symbol, Punctuation, WhiteSpace, AllowedChars);
        }

        public static string CleanInputString(string value, int MaxLength, bool Letters, bool Digits, bool Symbol, bool Punctuation, bool WhiteSpace, char[] AllowedChars)
        {
            value = value ?? "";
            var sTemp = new StringBuilder(value.Length);
            foreach (char chr in value)
            {
                if (Letters && char.IsLetter(chr)) { sTemp.Append(chr); }
                if (Digits && char.IsDigit(chr)) { sTemp.Append(chr); }
                if (Symbol && char.IsSymbol(chr)) { sTemp.Append(chr); }
                if (Punctuation && char.IsPunctuation(chr)) { sTemp.Append(chr); }
                if (WhiteSpace && char.IsWhiteSpace(chr)) { sTemp.Append(chr); }
                if (AllowedChars != null)
                {
                    if (Array.IndexOf(AllowedChars, chr) >= 0) { sTemp.Append(chr); }
                }
                if (sTemp.Length == MaxLength) break;
            }
            return sTemp.ToString().Trim();
        }

    }
}
