using System.Collections.Generic;

namespace MergeRowSample
{
    class ComplexStatisticsData
    {
        public string Dimension1 { get; set; }
        public string Dimension2 { get; set; }
        public List<SubStatisticsData> SubStatisticsDatas { get; set; }
    }
    class SubStatisticsData
    {
        public string Date { get; set; }
        public int Denominator { get; set; }
        public int Numerator { get; set; }
        public double Quotient { get; set; }
    }
}
