using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public static class Markdown
    {
        public static string Convert(List<string> markdownLines)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<string> htmlLines = new List<string>();

            foreach (string markdown in markdownLines)
            {
                htmlLines.Add(tokenizer.Tokenize(markdown));
            }

            htmlLines.Add(tokenizer.Close());
            return string.Join('\n', htmlLines.ToArray());
        }

        // convert from file
        public static string ConvertFile(string path)
        {
            return "";
        }
    }
}
