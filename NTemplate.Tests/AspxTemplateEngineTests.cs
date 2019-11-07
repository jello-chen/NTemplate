using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTemplate.Engine.Aspx;
using System;
using System.IO;

namespace NTemplate.Tests
{
    [TestClass]
    public class AspxTemplateEngineTests
    {
        [TestMethod]
        public void ExecuteStringModelTest()
        {
            string template = @"<%=Model.Text%>";
            ITemplateEngine templateEngine = new AspxTemplateEngine();
            string result = templateEngine.Render(template, new { Text = "Hello" });
            Assert.AreEqual("Hello", result);
        }
    }
}
