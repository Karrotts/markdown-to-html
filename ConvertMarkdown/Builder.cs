using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public static class Builder
    {
        public static Dictionary<char, string> IllegalCharacters;

        public static void Initialize()
        {
            IllegalCharacters = new Dictionary<char, string>();
            IllegalCharacters.Add('<', "&lt;");
            //IllegalCharacters.Add('>', "&gt;");
        }

        public static string RepaceInString(string original, string replacement, int startIndex, int endIndex)
        {
            string output = original.Substring(0, startIndex);
            output += replacement;

            if (endIndex != original.Length + 1)
                output += original.Substring(endIndex + 1);

            return output;
        }

        public static string Clean(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                foreach (char c in IllegalCharacters.Keys)
                {
                    if(input[i] == c)
                    {
                        input = RepaceInString(input, IllegalCharacters[c], i, i);
                    }
                }
            }
            return input;
        }
    }
}
