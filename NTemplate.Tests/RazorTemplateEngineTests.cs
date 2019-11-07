using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTemplate.Engine.Razor;
using System;
using System.Collections.Generic;
using System.IO;

namespace NTemplate.Tests
{
    [TestClass]
    public class RazorTemplateEngineTests
    {
        [TestMethod]
        public void ExecuteStringModelTest()
        {
            string template = @"@Model.Text";
            ITemplateEngine templateEngine = new RazorTemplateEngine();
            string result = templateEngine.Render(template, new { Text = "Hello" });
            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void ExecuteListModelTemplateTest()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "template.cshtml");
            string template = File.ReadAllText(path);
            ITemplateEngine templateEngine = new RazorTemplateEngine(new StreamWriter(new FileStream("debug.txt", FileMode.OpenOrCreate)));
            string result = templateEngine.Render(template, new List<Product>
            {
                new Product { ID = 1, Name = "Name1"},
                new Product { ID = 2, Name = "Name2"},
            });
            Assert.AreEqual("<table><tr><td>1</td><td>Name1</td></tr><tr><td>2</td><td>Name2</td></tr></table>", result);
        }

        [TestMethod]
        public void ExecuteAnonymousModelTemplateTest()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "anonymous-model-template.cshtml");
            string template = File.ReadAllText(path);
            ITemplateEngine templateEngine = new RazorTemplateEngine();
            string result = templateEngine.Render(template, new { Num1 = 1, Num2 = 2});
            Assert.AreEqual("<span>3</span>", result);
        }

        [TestMethod]
        public void ExecuteGenericTemplateBaseTest()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "template2.cshtml");
            string template = File.ReadAllText(path);
            TemplateEngineBase templateEngine = new RazorTemplateEngine(Console.Out);
            string result = templateEngine.Render(template, 2);
            Assert.AreEqual("110", result);
        }
    }

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}