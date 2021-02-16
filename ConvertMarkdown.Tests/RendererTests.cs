using NUnit.Framework;
using ConvertMarkdown;

namespace ConvertMarkdown.Tests
{
    public class RendererTests
    {
        [SetUp]
        public void Setup()
        {
        }

        #region HeadingTests
        [Test]
        public void HeadingTest1()
        {
            Assert.AreEqual("<h1>Hello World!</h1>", Renderer.Heading(1, "Hello World!"));
        }
        [Test]
        public void HeadingTest2()
        {
            Assert.AreEqual("<h2>Hello World!</h2>", Renderer.Heading(2, "Hello World!"));
        }
        [Test]
        public void HeadingTest3()
        {
            Assert.AreEqual("<h3>Hello World!</h3>", Renderer.Heading(3, "Hello World!"));
        }
        [Test]
        public void HeadingTest4()
        {
            Assert.AreEqual("<h4>Hello World!</h4>", Renderer.Heading(4, "Hello World!"));
        }
        [Test]
        public void HeadingTest5()
        {
            Assert.AreEqual("<h5>Hello World!</h5>", Renderer.Heading(5, "Hello World!"));
        }
        [Test]
        public void HeadingTest6()
        {
            Assert.AreEqual("<h6>Hello World!</h6>", Renderer.Heading(6, "Hello World!"));
        }
        [Test]
        public void HeadingTest7()
        {
            Assert.AreEqual("<h6>Hello World!</h6>", Renderer.Heading(7, "Hello World!"));
        }
        [Test]
        public void HeadingTest8()
        {
            Assert.AreEqual("<h6>Hello World!</h6>", Renderer.Heading(124, "Hello World!"));
        }
        #endregion
    }
}