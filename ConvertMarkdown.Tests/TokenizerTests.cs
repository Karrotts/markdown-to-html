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
            Tokenizer tokenizer = new Tokenizer();
            string result = "Hello World!!  ";
            tokenizer.InsertLineBreak(ref result);
            Assert.AreEqual("Hello World!!<br>", result);
        }

        [Test]
        public void LineBreakEmptyString()
        {
            Tokenizer tokenizer = new Tokenizer();
            string result = "";
            tokenizer.InsertLineBreak(ref result);
            Assert.AreEqual("", result);
        }

        [Test]
        public void LineBreakAllSpaces()
        {
            Tokenizer tokenizer = new Tokenizer();
            string result = "  ";
            tokenizer.InsertLineBreak(ref result);
            Assert.AreEqual("  ", result);
        }

        [Test]
        public void MarkdownBasicTest()
        {
            List<string> lines = new List<string>(){
                "[Hello World!](https://www.example.com/ \"This is an example\")",
                "**This is a test of my parser** Testing  ",
                "> This should *be* in a blockquote",
                "## This should be a H2 with an *italic*",
                "This should have any ***formating!***",
                "*What am I doing with my life?*"
            };
            string result = Markdown.Convert(lines);
            Console.WriteLine(result);
        }

        [Test]
        public void TabCount1()
        {
            Tokenizer tokenizer = new Tokenizer();
            Assert.AreEqual(1, tokenizer.CurrentTab("\tHello World!"));
        }
        [Test]
        public void TabCount2()
        {
            Tokenizer tokenizer = new Tokenizer();
            Assert.AreEqual(0, tokenizer.CurrentTab("Hello World!"));
        }
        [Test]
        public void TabCount3()
        {
            Tokenizer tokenizer = new Tokenizer();
            Assert.AreEqual(2, tokenizer.CurrentTab("\t\tHello World!"));
        }
        [Test]
        public void TabCount4()
        {
            Tokenizer tokenizer = new Tokenizer();
            Assert.AreEqual(3, tokenizer.CurrentTab("\t\t\tHello World!"));
        }
        [Test]
        public void LinkTest()
        {
            List<string> lines = new List<string>(){
                "[Hello_World!](https://www.example.com/example_test \"This is an example\")",
            };
            string result = Markdown.Convert(lines);
            Assert.AreEqual("\n<p><a href=\"https://www.example.com/example_test\" title=\"This is an example\">Hello_World!</a></p>\n", result);
        }
    }
}
