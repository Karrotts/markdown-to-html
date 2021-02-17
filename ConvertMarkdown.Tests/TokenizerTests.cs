using NUnit.Framework;
using ConvertMarkdown;
using System;

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
            string result = tokenizer.Tokenize("# **RegExr** created *s e x y* *hello world* **This is bold of you!**");
            Console.WriteLine(result);
        }
    }
}
