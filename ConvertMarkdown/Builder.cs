using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public static class Builder
    {
        public static string RepaceInString(string original, string replacement, int startIndex, int endIndex)
        {
            //Original: Welcome to the *Party*
            //Replacement: <em>Party</em>
            //Start Index: 15
            //End Index: 21

            string output = original.Substring(0, startIndex);
            output += replacement;

            if (endIndex != original.Length + 1)
                output += original.Substring(endIndex + 1);

            return output;
        }
    }
}
