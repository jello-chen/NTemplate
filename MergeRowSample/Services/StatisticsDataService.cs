using System.Collections.Generic;

namespace MergeRowSample
{
    class StatisticsDataService
    {
        public List<SimpleStatisticsData> GetSimpleStatisticsDatas()
            => new List<SimpleStatisticsData>
            {
                new SimpleStatisticsData{Dimension1 = "D1", Dimension2 = "D11", Numerator = 1, Denominator = 100, Quotient = 0.01 },
                new SimpleStatisticsData{Dimension1 = "D1", Dimension2 = "D11", Numerator = 2, Denominator = 200, Quotient = 0.01 },
                new SimpleStatisticsData{Dimension1 = "D1", Dimension2 = "D11", Numerator = 4, Denominator = 200, Quotient = 0.02 },
                new SimpleStatisticsData{Dimension1 = "D1", Dimension2 = "D12", Numerator = 11, Denominator = 500, Quotient = 0.22 },
                new SimpleStatisticsData{Dimension1 = "D1", Dimension2 = "D12", Numerator = 6, Denominator = 200, Quotient = 0.03 },
                new SimpleStatisticsData{Dimension1 = "D2", Dimension2 = "D21", Numerator = 1, Denominator = 100, Quotient = 0.01 },
                new SimpleStatisticsData{Dimension1 = "D2", Dimension2 = "D21", Numerator = 2, Denominator = 200, Quotient = 0.01 },
                new SimpleStatisticsData{Dimension1 = "D2", Dimension2 = "D21", Numerator = 4, Denominator = 200, Quotient = 0.02 },
                new SimpleStatisticsData{Dimension1 = "D2", Dimension2 = "D22", Numerator = 13, Denominator = 500, Quotient = 0.26 },
                new SimpleStatisticsData{Dimension1 = "D2", Dimension2 = "D22", Numerator = 6, Denominator = 200, Quotient = 0.03 },
            };

        public List<ComplexStatisticsData> GetComplexStatisticsDatas()
            => new List<ComplexStatisticsData>
            {
                new ComplexStatisticsData
                {
                    Dimension1 = "D1", Dimension2 = "D11", SubStatisticsDatas = new List<SubStatisticsData> { 
                        new SubStatisticsData{ Date = "2019/01", Numerator = 1, Denominator = 100, Quotient = 0.01 },
                        new SubStatisticsData{ Date = "2019/02", Numerator = 2, Denominator = 100, Quotient = 0.02 },
                        new SubStatisticsData{ Date = "2019/03", Numerator = 3, Denominator = 100, Quotient = 0.03 },
                } },
                new ComplexStatisticsData
                {
                    Dimension1 = "D1", Dimension2 = "D12", SubStatisticsDatas = new List<SubStatisticsData> {
                        new SubStatisticsData{ Date = "2019/01", Numerator = 1, Denominator = 100, Quotient = 0.01 },
                        new SubStatisticsData{ Date = "2019/02", Numerator = 2, Denominator = 100, Quotient = 0.02 },
                        new SubStatisticsData{ Date = "2019/03", Numerator = 3, Denominator = 100, Quotient = 0.03 },
                } },
                new ComplexStatisticsData
                {
                    Dimension1 = "D1", Dimension2 = "D11", SubStatisticsDatas = new List<SubStatisticsData> {
                        new SubStatisticsData{ Date = "2019/01", Numerator = 1, Denominator = 100, Quotient = 0.01 },
                        new SubStatisticsData{ Date = "2019/02", Numerator = 2, Denominator = 100, Quotient = 0.02 },
                        new SubStatisticsData{ Date = "2019/03", Numerator = 3, Denominator = 100, Quotient = 0.03 },
                } },
                new ComplexStatisticsData
                {
                    Dimension1 = "D1", Dimension2 = "D11", SubStatisticsDatas = new List<SubStatisticsData> {
                        new SubStatisticsData{ Date = "2019/01", Numerator = 1, Denominator = 100, Quotient = 0.01 },
                        new SubStatisticsData{ Date = "2019/02", Numerator = 2, Denominator = 100, Quotient = 0.02 },
                        new SubStatisticsData{ Date = "2019/03", Numerator = 3, Denominator = 100, Quotient = 0.03 },
                } },
                new ComplexStatisticsData
                {
                    Dimension1 = "D1", Dimension2 = "D11", SubStatisticsDatas = new List<SubStatisticsData> {
                        new SubStatisticsData{ Date = "2019/01", Numerator = 1, Denominator = 100, Quotient = 0.01 },
                        new SubStatisticsData{ Date = "2019/02", Numerator = 2, Denominator = 100, Quotient = 0.02 },
                        new SubStatisticsData{ Date = "2019/03", Numerator = 3, Denominator = 100, Quotient = 0.03 },
                } },
            };
    }
}
