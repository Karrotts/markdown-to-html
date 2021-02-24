using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public class Parser
    {
        public Parser()
        {

        }

        public string Parse(MarkdownToken token)
        {
            string test = token.TokenMatch.Value;
            Console.WriteLine(test);
            return "";
        }
    }
}
