using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertMarkdown
{
    public class Parser
    {
        private bool lineContained;
        public Parser()
        {
            lineContained = false;
        }

        public string Parse(string line, List<string> html, TokenMatch match)
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
                case TokenType.BlockQuote:
                    break;
                case TokenType.UnorderedList:
                    lineContained = true;

                    break;
                case TokenType.OrderedList:
                    lineContained = true;

                    break;
                default:
                    break;
            }
            return line;
        }

        public string CloseLine(string line)
        {
            if (!lineContained && !string.IsNullOrWhiteSpace(line))
            {
                line = Builder.RepaceInString(line, Renderer.Paragraph(line), 0, line.Length - 1);
            }
            lineContained = false;

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
