using NUnit.Framework;
using ConvertMarkdown;

namespace ConvertMarkdown.Tests
{
    class BuilderTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BuilderBasicTest()
        {
            Assert.AreEqual("Welcome to the <em>Party</em>", Builder.Build("Welcome to the *Party*","<em>Party</em>", 15, 21));
        }
    }
}
