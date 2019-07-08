using MergeRowSample.Samples;
using System;

namespace MergeRowSample
{
    class Program
    {
        static void Main(string[] args)
        {
            ISample simpleSample = new SimpleSample();
            simpleSample.Execute();

            Console.Read();
        }
    }
}
