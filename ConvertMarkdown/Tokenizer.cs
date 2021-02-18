using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConvertMarkdown
{
    public enum TokenType
    {
        Header1,
        Header2,
        Header3,
        Header4,
        Header5,
        Header6,
        Paragraph,
        Bold,
        Italic,
        BoldItalic,
        BlockQuote
    }

    public enum SpecialTokenType
    {
        BlockQuote,
        LineBreak,
        OrderList,
        UnorderedList,
        Code,
        Image
    }

    public class Tokenizer
    {
        private bool previousElementWasBlockQuote = false;

        private List<TokenMatcher> tokenMatchers;
        private List<TokenMatcher> specialTokenMatchers;
        public Tokenizer()
        {
            // Add definitions for each of the token types
            tokenMatchers = new List<TokenMatcher>();
            specialTokenMatchers = new List<TokenMatcher>();

            tokenMatchers.Add(new TokenMatcher(TokenType.Header1, "^# (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Header2, "^## (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Header3, "^### (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Header4, "^#### (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Header5, "^##### (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Header6, "^###### (.+)"));
            tokenMatchers.Add(new TokenMatcher(TokenType.BoldItalic, "\\*\\*\\*(.+?)\\*\\*\\*"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Bold, "\\*\\*(.+?)\\*\\*"));
            tokenMatchers.Add(new TokenMatcher(TokenType.Italic, "\\*(.+?)\\*"));

            specialTokenMatchers.Add(new TokenMatcher(TokenType.BlockQuote, "^> (.+)"));
        }

        public string Tokenize(string text, List<string> htmlLines)
        {
            SpecialTokenize(ref text, htmlLines);
            SimpleTokenize(ref text);
            return text;
        }

        public void SimpleTokenize(ref string line)
        {
            foreach (TokenMatcher matcher in tokenMatchers)
            {
                TokenMatch match = matcher.Match(line);
                while (match.IsMatch)
                {
                    string replacement = "";
                    switch (match.TokenType)
                    {
                        case TokenType.Header1:
                            replacement = Renderer.Heading(1, match.Value);
                            break;
                        case TokenType.Header2:
                            replacement = Renderer.Heading(2, match.Value);
                            break;
                        case TokenType.Header3:
                            replacement = Renderer.Heading(3, match.Value);
                            break;
                        case TokenType.Header4:
                            replacement = Renderer.Heading(4, match.Value);
                            break;
                        case TokenType.Header5:
                            replacement = Renderer.Heading(5, match.Value);
                            break;
                        case TokenType.Header6:
                            replacement = Renderer.Heading(6, match.Value);
                            break;
                        case TokenType.Italic:
                            replacement = Renderer.Italic(match.Value);
                            break;
                        case TokenType.Bold:
                            replacement = Renderer.Bold(match.Value);
                            break;
                        case TokenType.BoldItalic:
                            replacement = Renderer.BoldItalic(match.Value);
                            break;
                        default:
                            break;
                    }
                    line = Builder.RepaceInString(line, replacement, match.StartIndex, match.EndIndex);
                    match = matcher.Match(line);
                }
            }
        }

        public void SpecialTokenize(ref string line, List<string> htmlLines)
        {
            bool itemFound = false;
            foreach (TokenMatcher matcher in specialTokenMatchers)
            {
                TokenMatch match = matcher.Match(line);
                if (match.IsMatch)
                {
                    itemFound = true;
                    switch (match.TokenType)
                    {
                        case TokenType.BlockQuote:
                            line = Builder.RepaceInString(line, "", 0, 1);

                            if (!previousElementWasBlockQuote)
                                htmlLines.Add("<blockquote>");
                
                            previousElementWasBlockQuote = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!itemFound)
            {
                if (previousElementWasBlockQuote)
                    htmlLines.Add("</blockquote>");

                previousElementWasBlockQuote = false;
            }
        }

        public string Close()
        {
            string closer = ""; 
            if (previousElementWasBlockQuote)
            {
                closer += "</blockquote>";
                previousElementWasBlockQuote = false;
            }
            return closer;
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
                    BaseValue = match.Value,
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
        public string BaseValue { get; set; }
        public string Value { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
    }
}
