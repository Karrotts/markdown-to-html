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
            Builder.Initialize();
            List<string> html = new List<string>();

            foreach (string markdown in markdownLines)
            {
                html.Add(tokenizer.Tokenize(markdown, html));
            }

            tokenizer.Close();
            //html.RemoveAll(string.IsNullOrWhiteSpace);
            return string.Join('\0', html.ToArray());
        }

        // convert from file
        public static string ConvertFile(string path)
        {
            return "";
        }
    }
}
