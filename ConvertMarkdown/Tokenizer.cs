using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConvertMarkdown
{
    public enum TokenType
    {
        Header,
        Paragraph,
        LineBreak,
        Bold,
        Italic,
        BoldItalic,
        BlockQuote,
        OrderList,
        UnorderedList,
        Code,
        Image
    }

    public class Tokenizer
    {
        private List<TokenMatcher> tokenMatchers;
        public Tokenizer()
        {
            // Add definitions for each of the token types
            tokenMatchers = new List<TokenMatcher>();
            tokenMatchers.Add(new TokenMatcher(TokenType.Header, "([A-Z])\\w+"));
        }

        public void Tokenize(string text)
        {

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
                string remainingText = string.Empty;
                if (match.Length != inputString.Length)
                    remainingText = inputString.Substring(match.Length);

                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = remainingText,
                    TokenType = type,
                    Value = match.Value
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
        public string Value { get; set; }
        public string RemainingText { get; set; }
    }
}
