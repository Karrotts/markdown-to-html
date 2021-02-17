using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public static class Markdown
    {
        // single line conversion
        public static string Convert(string text)
        {
            Tokenizer tokenizer = new Tokenizer();
            text = tokenizer.Tokenize(text);
            return text;
        }

        // convert from file
        public static string ConvertFile(string path)
        {
            return "";
        }
    }
}
