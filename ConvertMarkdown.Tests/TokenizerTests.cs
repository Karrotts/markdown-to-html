using NUnit.Framework;
using ConvertMarkdown;
using System;
using System.Collections.Generic;

namespace ConvertMarkdown.Tests
{
    class TokenizerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LineBreakTest()
        {
            List<string> input = new List<string>()
            {
                "**Hello World** **Hello Germany**"
            };
            Markdown.Convert(input);
        }
    }
}
