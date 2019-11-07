using NTemplate;
using NTemplate.Engine.Razor;
using System;
using System.Collections.Generic;
using System.IO;

namespace MergeRowSample.Samples
{
    internal class SimpleSample : ISample
    {
        private const string TEMPLATE_FILE = "MergeRowTemplate1.cshtml";
        private const string GENERATED_FILE = "MergeRowTemplate1.html";
        private const string DEBUG_FILE = "Debug.txt";

        public void Execute()
        {
            // Prepare sample data
            StatisticsDataService statisticsDataService = new StatisticsDataService();
            List<SimpleStatisticsData> statisticsDatas = statisticsDataService.GetSimpleStatisticsDatas();

            // Convert list to the td matrix
            Td[][] array = HtmlTableHelper.GetTdMatrix(statisticsDatas,
                t => new Td[] {
                new Td{ Text = t.Dimension1 },
                new Td{ Text = t.Dimension2 },
            }, t => new Td[] {
                new Td{ Text = t.Denominator.ToString()},
                new Td{ Text = t.Numerator.ToString()},
                new Td{ Text = t.Quotient.ToString()},
            });

            // Read template content from template file
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", TEMPLATE_FILE);
            string template = File.ReadAllText(path);

            // Construct the razor template engine
            ITemplateEngine templateEngine = new RazorTemplateEngine(new StreamWriter(new FileStream(DEBUG_FILE, FileMode.OpenOrCreate)));

            // Render by template and data
            string result = templateEngine.Render(template, array);
            Console.WriteLine(result);
            File.WriteAllText(GENERATED_FILE, result);
        }
    }
}
