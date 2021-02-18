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
            Assert.AreEqual("Welcome to the <em>Party</em>", Builder.RepaceInString("Welcome to the *Party*","<em>Party</em>", 15, 21));
        }
        [Test]
        public void BuilderReplaceWithEmptyString()
        {
            Assert.AreEqual("Welcome to the ", Builder.RepaceInString("Welcome to the *Party*", "", 15, 21));
        }

        [Test]
        public void BuilderReplaceAtStart()
        {
            Assert.AreEqual("Hello World", Builder.RepaceInString("> Hello World", "", 0, 1));
        }
    }
}
