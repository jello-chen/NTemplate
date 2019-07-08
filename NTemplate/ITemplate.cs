using System;

namespace NTemplate
{
    public interface ITemplate
    {
        void Write(object value);
        void WriteLiteral(string value);
        void WriteAttribute<T1>(
            string name,
            Tuple<string, int> leader,
            Tuple<string, int> trailer,
            Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1
        );
        void WriteAttribute<T1, T2>(
            string name,
            Tuple<string, int> leader,
            Tuple<string, int> trailer,
            Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1,
            Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2);
        void WriteAttribute<T1, T2, T3>(
            string name, 
            Tuple<string, int> leader,
            Tuple<string, int> trailer,
            Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1, 
            Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2, 
            Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3);
        void WriteAttribute<T1, T2, T3, T4>(
            string name, 
            Tuple<string, int> leader,
            Tuple<string, int> trailer,
            Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1,
            Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2, 
            Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3, 
            Tuple<Tuple<string, int>, Tuple<T4, int>, bool> part4);
        void WriteAttribute<T1, T2, T3, T4, T5>(
            string name, 
            Tuple<string, int> leader,
            Tuple<string, int> trailer,
            Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1,
            Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2,
            Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3, 
            Tuple<Tuple<string, int>, Tuple<T4, int>, bool> part4,
            Tuple<Tuple<string, int>, Tuple<T5, int>, bool> part5);
        void WriteAttribute<T1, T2, T3, T4, T5, T6>(
            string name, 
            Tuple<string, int> leader, 
            Tuple<string, int> trailer, 
            Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1,
            Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2,
            Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3,
            Tuple<Tuple<string, int>, Tuple<T4, int>, bool> part4, 
            Tuple<Tuple<string, int>, Tuple<T5, int>, bool> part5,
            Tuple<Tuple<string, int>, Tuple<T6, int>, bool> part6);
        void WriteEncoded(string value);
        void Execute();
    }
}
