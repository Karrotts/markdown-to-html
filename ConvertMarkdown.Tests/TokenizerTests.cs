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
            //Tokenizer tokenizer = new Tokenizer();
            //string result = tokenizer.Tokenize("# **RegExr** created ***this*** *hello world* **This is bold of you!**");
            //Console.WriteLine(result);
        }

        [Test]
        public void MarkdownBasicTest()
        {
            List<string> lines = new List<string>(){
                "[Hello World!](https://www.example.com/ \"This is an example\")",
                "**This is a test of my parser** Testing",
                "> This should *be* in a blockquote",
                "## This should be a H2 with an *italic*",
                "This should have any ***formating!***",
                "*What am I doing with my life?*"
            };
            string result = Markdown.Convert(lines);
            Console.WriteLine(result);
        }


    }
}
