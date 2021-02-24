using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public class Parser
    {
        private bool headerFound;
        private byte tabIndex;
        private Stack<TokenType> foundSpecialTokens;

        public Parser()
        {
            headerFound = false;
            tabIndex = 0;
            foundSpecialTokens = new Stack<TokenType>();
        }

        public string Parse(string line, List<string> html, TokenMatch match)
        {
            if (string.IsNullOrWhiteSpace(line)) return line;

            bool itemFound = true;
            bool tabFound = line.IndexOf("\t") == 0;

            byte newTabIndex = CurrentTab(line);

            line = tabFound ? line.Replace("\t", "") : line;

            switch (match.TokenType)
            {
                case TokenType.BlockQuote:
                    line = Builder.RepaceInString(line, "", 0, 1);

                    if (foundSpecialTokens.Count == 0 || foundSpecialTokens.Peek() != TokenType.BlockQuote)
                    {
                        html.Add("<blockquote>");
                        foundSpecialTokens.Push(TokenType.BlockQuote);
                    }
                    break;
                case TokenType.UnorderedList:
                    // Check if the previous token was an unordered list and pop it from the stack
                    if (foundSpecialTokens.Count > 0
                    && foundSpecialTokens.Peek() == TokenType.OrderedList)
                    {
                        html.Add("</ol>");
                        foundSpecialTokens.Pop();
                    }

                    // Add new element if there is a tab or first time seeing the match
                    if (foundSpecialTokens.Count == 0
                    || foundSpecialTokens.Peek() != TokenType.UnorderedList
                    || tabFound && newTabIndex > tabIndex && newTabIndex - tabIndex == 1)
                    {
                        html.Add("<ul>");
                        foundSpecialTokens.Push(TokenType.UnorderedList);
                        tabIndex = newTabIndex;
                    }

                    // Close child element if untabbed
                    if (newTabIndex < tabIndex)
                    {
                        while (tabIndex - newTabIndex > 0)
                        {
                            if (foundSpecialTokens.Peek() == TokenType.UnorderedList)
                            {
                                html.Add("</ul>");
                                foundSpecialTokens.Pop();
                                tabIndex--;
                            }
                        }
                    }

                    // Insert list item into line
                    line = Builder.RepaceInString(line,
                                                    Renderer.ListItem(match.Value),
                                                    match.StartIndex,
                                                    match.EndIndex);
                    headerFound = true;
                    break;
                case TokenType.OrderedList:
                    // Check if the previous token was an unordered list and pop it from the stack
                    if (foundSpecialTokens.Count > 0
                    && foundSpecialTokens.Peek() == TokenType.UnorderedList)
                    {
                        html.Add("</ul>");
                        foundSpecialTokens.Pop();
                    }

                    // Add new element if there is a tab or first time seeing the match
                    if (foundSpecialTokens.Count == 0
                    || foundSpecialTokens.Peek() != TokenType.OrderedList
                    || tabFound && newTabIndex > tabIndex && newTabIndex - tabIndex == 1)
                    {
                        html.Add("<ol>");
                        foundSpecialTokens.Push(TokenType.OrderedList);
                        tabIndex = newTabIndex;
                    }

                    // Close child element if untabbed
                    else if (newTabIndex < tabIndex)
                    {
                        while (tabIndex - newTabIndex > 0)
                        {
                            if (foundSpecialTokens.Peek() == TokenType.OrderedList)
                            {
                                html.Add("</ol>");
                                foundSpecialTokens.Pop();
                                tabIndex--;
                            }
                        }
                    }

                    // Insert list item into line
                    line = Builder.RepaceInString(line,
                                                    Renderer.ListItem(match.Value),
                                                    match.StartIndex,
                                                    match.EndIndex);
                    headerFound = true;
                    break;
                default:
                    itemFound = false;
                    break;

            }

            if (!itemFound)
            {
                html.Add(Close(line));
            }

            string replacement = "";
            switch (match.TokenType)
            {
                case TokenType.Image:
                    replacement = Renderer.Image(match.BaseMatch.Groups[1].Value, match.BaseMatch.Groups[2].Value);
                    break;
                case TokenType.Link:
                    replacement = Renderer.Link(match.BaseMatch.Groups[1].Value, match.BaseMatch.Groups[2].Value);
                    break;
                case TokenType.Header:
                    int tokenCount = line.Length - line.Replace("#", "").Length;
                    replacement = Renderer.Heading(tokenCount, match.Value);
                    headerFound = true;
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

            return line;
        }

        public string Close(string line)
        {
            if (!headerFound && !string.IsNullOrWhiteSpace(line))
            {
                line = Builder.RepaceInString(line, Renderer.Paragraph(line), 0, line.Length - 1);
            }

            tabIndex = 0;
            string output = "";
            while (foundSpecialTokens.Count > 0)
            {
                switch (foundSpecialTokens.Peek())
                {
                    case TokenType.BlockQuote:
                        output += "</blockquote>";
                        foundSpecialTokens.Pop();
                        break;
                    case TokenType.UnorderedList:
                        output += "</ul>";
                        foundSpecialTokens.Pop();
                        break;
                    case TokenType.OrderedList:
                        output += "</ol>";
                        foundSpecialTokens.Pop();
                        break;
                    default:
                        break;
                }
            }

            headerFound = false;
            return line;
        }

        public byte CurrentTab(string line)
        {
            byte count = 0;
            while (line.IndexOf("\t", count) >= 0)
                count++;
            return count;
        }
    }
}
