using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConvertMarkdown
{
    public enum TokenType
    {
        Header,
        Paragraph,
        Bold,
        Italic,
        BoldItalic,
        BlockQuote,
        Link,
        OrderedList,
        UnorderedList,
        Line,
        Image,
        Codeblock,
        Codeline
    }
    public class Tokenizer
    {
        private List<TokenMatcher> tokenMatchers;
        private Parser parser;
        private bool InCodeBlock = false;
        private bool PrevLineCodeBlock = false;

        public Tokenizer()
        {
            parser = new Parser();
            tokenMatchers = new List<TokenMatcher>();
            tokenMatchers.Add(new TokenMatcher(TokenType.Codeblock, "^```$"));
            tokenMatchers.Add(new TokenMatcher(TokenType.BlockQuote, "^> (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.BlockQuote, "^>> (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.UnorderedList, "^\\* (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.UnorderedList, "^\\+ (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.UnorderedList, "^\\- (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.OrderedList, "^[0-9]+\\. (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Header, "^#+ (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.BoldItalic, "\\*\\*\\*(.+?)\\*\\*\\*"));
            tokenMatchers.Add(new TokenMatcher(TokenType.BoldItalic, "___(.+?)___"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Bold, "\\*\\*(.+?)\\*\\*"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Bold, "__(.+?)__"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Italic, "\\*(.+?)\\*"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Italic, "_(.+?)_"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Image, "!\\[(.+?)\\]\\((.+?)\\)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Link, "\\[(.+?)\\]\\((.+?)\\)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Line, "----"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Codeline, "`(.+?)`"));

        }

        public string Tokenize(string line, List<string> html)
        {
            line = Builder.Clean(line);
            parser.specialTokenFound = false;
            byte tabIndex = CurrentTab(line);
            foreach (TokenMatcher matcher in tokenMatchers)
            {
                bool isCodeBlock = matcher.Match(line).TokenType == TokenType.Codeblock;
                if (isCodeBlock)
                {
                    PrevLineCodeBlock = true;
                    if (!InCodeBlock)
                        html.Add("<pre><code>");
                    else
                        html.Add("</pre></code>");
                    InCodeBlock = !InCodeBlock;
                    line = "";
                    break;
                }

                if (InCodeBlock)
                {
                    if (string.IsNullOrEmpty(line)) line = " ";
                    if (!PrevLineCodeBlock)
                        html.Add('\n' + line);
                    else
                        html.Add(line);
                    PrevLineCodeBlock = false;
                    line = "";
                    break;
                }

                PrevLineCodeBlock = false;
                line.Trim(' ');
                if (string.IsNullOrEmpty(line)) break;
                line = line.Replace("\t", "");
                TokenMatch match = matcher.Match(line);
                while (match.IsMatch)
                {
                    line = parser.Parse(line, html, match, tabIndex);
                    match = matcher.Match(line);
                } 
            }
            line = parser.CloseLine(line);

            if (!parser.specialTokenFound)
                line = parser.CloseOpenTags() + line;

            return line;
        }

        public void Close() => parser.CloseOpenTags();  

        public byte CurrentTab(string line)
        {
            byte count = 0;
            while (line.IndexOf("\t", count) >= 0)
                count++;
            return count; 
        }

    }

    public class TokenMatcher
    {
        private Regex regex;
        private TokenType type;

        public TokenMatcher(TokenType type, string regexPattern)
        {
            this.regex = new Regex(regexPattern);
            this.type = type;
        }

        public TokenMatch Match(string inputString)
        {
            var match = regex.Match(inputString);
            if (match.Success)
            {
                return new TokenMatch()
                {
                    IsMatch = true,
                    TokenType = type,
                    BaseMatch = match,
                    Value = match.Groups[1].Value,
                    StartIndex = match.Index,
                    EndIndex = match.Index + match.Length - 1
                };
            }
            else
            {
                return new TokenMatch() { IsMatch = false };
            }

        }
    }

    public class TokenMatch
    {
        public bool IsMatch { get; set; }
        public TokenType TokenType { get; set; }
        public Match BaseMatch { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
