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

        [TestMethod]
        public void ExecuteBlockStatementTest()
        {
            string template = @"<% var sum = 0; for(var i = 1; i <= Model; i++) { sum += i;%><%}%><%=sum%>";
            ITemplateEngine templateEngine = new AspxTemplateEngine(new StreamWriter(new FileStream(DateTime.Now.ToLongDateString(), FileMode.OpenOrCreate)));
            string result = templateEngine.Render(template, 100);
            Assert.AreEqual("5050", result);
        }
    }
}
