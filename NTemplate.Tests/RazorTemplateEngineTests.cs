using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTemplate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTemplate.Tests
{
    [TestClass()]
    public class RazorTemplateEngineTests
    {
        [TestMethod()]
        public void ExecuteStringModelTest()
        {
            string template = @"@Model.Text";
            ITemplateEngine templateEngine = new RazorTemplateEngine();
            string result = templateEngine.Execute(template, new DynamicObjectWrapper(new { Text = "Hello" }));
            Assert.AreEqual("Hello", result);
        }

        [TestMethod()]
        public void ExecuteFileTemplateTest()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "template.cshtml");
            string template = File.ReadAllText(path);
            ITemplateEngine templateEngine = new RazorTemplateEngine()
            {
                EnableDebug = true,
                DebugOutput = new StreamWriter(new FileStream("debug.txt", FileMode.OpenOrCreate))
            };
            string result = templateEngine.Execute(template, new List<DynamicObjectWrapper>
            {
                new DynamicObjectWrapper(new { ID = 1, Name = "Name1"}),
                new DynamicObjectWrapper(new { ID = 2, Name = "Name2"}),
            });

            Assert.AreEqual("<table><tr><td>1</td><td>Name1</td></tr><tr><td>2</td><td>Name2</td></tr></table>", result);
        }
    }
}