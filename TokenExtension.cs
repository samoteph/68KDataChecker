using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _68KDataChecker
{
    public static class TokenExtension
    {
        public static string IgnoreSpace(this string str)
        {
            if(str == null)
            {
                return null;
            }

            int i = 0;

            for (i = 0; i < str.Length; i++)
            {
                if (!IsSpace(str[i]))
                {
                    break;
                }
            }

            if(i==0)
            {
                return str;
            }

            return str.Substring(i);
        }

        public static bool IsSeparator(char character)
        {
            return IsSpace(character) || character == '.' || character == ';';
        }

        public static bool IsSpace(char character)
        {
            return char.IsWhiteSpace(character) || character == '\t';
        }

        public static char? ExtractCharacter(this string str)
        {
            if(str == null || str.Length == 0)
            {
                return null;
            }

            return str[0];
        }

        public static string GetToken(this string str, params string[] tokens)
        {
            if (str == null)
            {
                return null;
            }

            foreach (string token in tokens)
            {
                if (str.StartsWith(token, StringComparison.OrdinalIgnoreCase) && (str.Length == token.Length || IsSeparator(str[token.Length])))
                {
                    return token;
                }
            }

            return null;
        }

        public static string SearchToken(this string str, params string[] tokens)
        {
            if(str == null)
            {
                return null;
            }

            foreach (string token in tokens)
            {
                if (str.StartsWith(token, StringComparison.OrdinalIgnoreCase) && (str.Length == token.Length || IsSeparator(str[token.Length])))
                {
                    if (str.Length == token.Length)
                    {
                        return string.Empty;
                    }

                    return str.Substring(token.Length);
                }
            }

            return null;
        }

        public static string Pass(this string str, int numberOfCharacter)
        {
            if(str == null)
            {
                return null;
            }

            if(str.Length > numberOfCharacter)
            {
                return str.Substring(numberOfCharacter);
            }

            return null;
        }

        public static string ExtractString(this string str, bool includeQuotes, params char[] optionalEndCharacter)
        {
            if(str == null)
            {
                return null;
            }

            if(includeQuotes == true)
            {
                throw new NotImplementedException();
            }

            int i = 0;

            for(i=0; i<str.Length; i++)
            {
                if (IsSpace(str[i]))
                {
                    break;
                }

                if (optionalEndCharacter!=null && optionalEndCharacter.Contains(str[i]))
                {
                    break;
                }
            }

            return str.Substring(0, i);
        }

        public static AssemblerType GetAssemblerType(this char? character)
        {
            if (character == null)
            {
                return AssemblerType.Unknown;
            }

            char lowerType = char.ToLower(character.Value);

            switch (lowerType)
            {
                case 'l':
                    return AssemblerType.Long;
                case 'w':
                    return AssemblerType.Word;
                case 'b':
                    return AssemblerType.Byte;
                default:
                    return AssemblerType.Unknown;
            }
        }
    }
}
