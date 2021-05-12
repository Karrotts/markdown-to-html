using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public class Parser
    {
        public bool specialTokenFound { get; set; }

        private byte sequenceTab;
        private bool lineContained;
        private Stack<TokenType> specialTokens;

        public Parser()
        {
            sequenceTab = 0;
            lineContained = false;
            specialTokenFound = false;
            specialTokens = new Stack<TokenType>();
        }

        public string Parse(string line, List<string> html, TokenMatch match, byte tabIndex)
        {
            switch(match.TokenType)
            {
                case TokenType.BoldItalic:
                    line = Builder.RepaceInString(line, 
                                                  Renderer.BoldItalic(match.Value),
                                                  match.StartIndex, 
                                                  match.EndIndex);
                    break;
                case TokenType.Bold:
                    line = Builder.RepaceInString(line,
                                                  Renderer.Bold(match.Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.Italic:
                    line = Builder.RepaceInString(line,
                                                  Renderer.Italic(match.Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.Codeline:
                    line = Builder.RepaceInString(line,
                                                  Renderer.Code(match.Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.Header:
                    lineContained = true;
                    int weight = line.Length - line.Replace("#", "").Length;
                    line = Builder.RepaceInString(line,
                                                  Renderer.Heading(weight, match.Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.Image:
                    line = Builder.RepaceInString(line,
                                                  Renderer.Image(match.BaseMatch.Groups[1].Value, match.BaseMatch.Groups[2].Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.Link:
                    line = Builder.RepaceInString(line,
                                                  Renderer.Link(match.BaseMatch.Groups[1].Value, match.BaseMatch.Groups[2].Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.Line:
                    line = Builder.RepaceInString(line,
                                                  Renderer.Line(match.Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.BlockQuote:
                    specialTokenFound = true;
                    line = Builder.RepaceInString(line,
                           "",
                           match.StartIndex,
                           match.StartIndex + 1);
                    if (!ContainsToken(TokenType.BlockQuote))
                    {
                        specialTokens.Push(TokenType.BlockQuote);
                        html.Add("<blockquote>");
                    }
                    break;
                case TokenType.UnorderedList:
                    lineContained = true;
                    specialTokenFound = true;
                    AddUnorderedHeader(html, line, tabIndex);
                    line = Builder.RepaceInString(line,
                                                  Renderer.ListItem(match.Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                case TokenType.OrderedList:
                    lineContained = true;
                    specialTokenFound = true;
                    AddOrderedHeader(html, line, tabIndex);
                    line = Builder.RepaceInString(line,
                                                  Renderer.ListItem(match.Value),
                                                  match.StartIndex,
                                                  match.EndIndex);
                    break;
                default:
                    break;
            }
            return line;
        }

        private bool ElementIsInNewTab(byte tabIndex)
        {
            bool tabFound = tabIndex > 0;
            return tabFound && tabIndex > sequenceTab && tabIndex - sequenceTab == 1;
        }

        private void AddUnorderedHeader(List<string> html, string line, byte tabIndex)
        {
            if(specialTokens.Count > 0
            && specialTokens.Peek() == TokenType.OrderedList
            && !ElementIsInNewTab(tabIndex))
            {
                while(specialTokens.Count > 0 && specialTokens.Peek() != TokenType.UnorderedList)
                {
                    if(specialTokens.Peek() == TokenType.OrderedList)
                    {
                        sequenceTab--;
                        specialTokens.Pop();
                        html.Add("</ol>");
                    }
                }
            }

            if(specialTokens.Count == 0
            || ElementIsInNewTab(tabIndex)
            || specialTokens.Peek() == TokenType.BlockQuote)
            {
                sequenceTab = tabIndex;
                html.Add("<ul>");
                specialTokens.Push(TokenType.UnorderedList);
            }

            while (tabIndex < sequenceTab)
            {
                if (sequenceTab <= 0 || specialTokens.Count == 0) break;
            
                if (specialTokens.Peek() == TokenType.UnorderedList)
                {
                    html.Add("</ul>");
                    sequenceTab--;
                    specialTokens.Pop();
                    continue;
                }
            }
        }

        private void AddOrderedHeader(List<string> html, string line, byte tabIndex)
        {
            if (specialTokens.Count > 0
            && specialTokens.Peek() == TokenType.UnorderedList
            && !ElementIsInNewTab(tabIndex))
            {
                while (specialTokens.Count > 0 && specialTokens.Peek() != TokenType.OrderedList)
                {
                    if (specialTokens.Peek() == TokenType.UnorderedList)
                    {
                        sequenceTab--;
                        specialTokens.Pop();
                        html.Add("</ul>");
                    }
                }
            }

            if (specialTokens.Count == 0
            || ElementIsInNewTab(tabIndex)
            || specialTokens.Peek() == TokenType.BlockQuote)
            {
                sequenceTab = tabIndex;
                html.Add("<ol>");
                specialTokens.Push(TokenType.OrderedList);
            }

            while (tabIndex < sequenceTab)
            {
                if (sequenceTab <= 0 || specialTokens.Count == 0) break;

                if (specialTokens.Peek() == TokenType.OrderedList)
                {
                    html.Add("</ol>");
                    sequenceTab--;
                    specialTokens.Pop();
                    continue;
                }
            }
        }

        private void AddCodeBlock(List<string> html, string line) { }

        public string CloseLine(string line)
        {
            if (!lineContained && !string.IsNullOrWhiteSpace(line))
            {
                line = Builder.RepaceInString(line, Renderer.Paragraph(line), 0, line.Length - 1);
            }
            lineContained = false;

            return line;
        }

        public string CloseOpenTags()
        {
            sequenceTab = 0;
            specialTokenFound = false;
            string output = "";
            while (specialTokens.Count > 0)
            {
                switch (specialTokens.Peek())
                {
                    case TokenType.BlockQuote:
                        output += "</blockquote>";
                        specialTokens.Pop();
                        break;
                    case TokenType.UnorderedList:
                        output += "</ul>";
                        specialTokens.Pop();
                        break;
                    case TokenType.OrderedList:
                        output += "</ol>";
                        specialTokens.Pop();
                        break;
                    default:
                        break;
                }
            }
            return output;
        }
      
        public byte CurrentTab(string line)
        {
            byte count = 0;
            while (line.IndexOf("\t", count) >= 0)
                count++;
            return count;
        }

        public bool ContainsToken(TokenType type)
        {
            foreach(TokenType t in specialTokens)
            {
                if (t == type) return true;
            }
            return false;
        }
    }
}
