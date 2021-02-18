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
        public void TokenizerBasicTest()
        {
            Tokenizer tokenizer = new Tokenizer();
            string result = tokenizer.Tokenize("# **RegExr** created ***this*** *hello world* **This is bold of you!**");
            Console.WriteLine(result);
        }

        [Test]
        public void MarkdownBasicTest()
        {
            List<string> lines = new List<string>(){ "> Hello World! ", "# **RegExr** created ***this*** *hello world* **This is bold of you!**" };
            string result = Markdown.Convert(lines);
            Console.WriteLine(result);
        }


    }
}
