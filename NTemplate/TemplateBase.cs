using System;
using System.IO;

namespace NTemplate
{
    public abstract class TemplateBase : ITemplate, IDisposable
    {
        public TemplateBase() => this.Output = new StringWriter();

        public dynamic Model { get; set; }
        public StringWriter Output { get; private set; }

        public virtual void Write(object value) => this.WriteLiteral(value?.ToString());

        public virtual void WriteLiteral(string value) => this.Output.Write(value);

        public void WriteAttribute(string name, Tuple<string, int> prefix, Tuple<string, int> suffix, Tuple<Tuple<string, int>, Tuple<object, int>> value, bool flag)
        {
            this.Output.Write(name);
        }

        public void WriteAttribute<T1>(string name, Tuple<string, int> leader, Tuple<string, int> trailer, Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (leader == null)
            {
                throw new ArgumentNullException("leader");
            }
            if (trailer == null)
            {
                throw new ArgumentNullException("trailer");
            }
            if (part1 == null)
            {
                throw new ArgumentNullException("part1");
            }
            WriteLiteral(leader.Item1);
            WriteLiteral(part1.Item1.Item1);
            Write(part1.Item2.Item1);
            WriteLiteral(trailer.Item1);
        }

        public void WriteAttribute<T1, T2>(string name, Tuple<string, int> leader, Tuple<string, int> trailer, Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1, Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (leader == null)
            {
                throw new ArgumentNullException("leader");
            }
            if (trailer == null)
            {
                throw new ArgumentNullException("trailer");
            }
            if (part1 == null)
            {
                throw new ArgumentNullException("part1");
            }
            if (part2 == null)
            {
                throw new ArgumentNullException("part2");
            }
            WriteLiteral(leader.Item1);
            WriteLiteral(part1.Item1.Item1);
            Write(part1.Item2.Item1);
            WriteLiteral(part2.Item1.Item1);
            Write(part2.Item2.Item1);
            WriteLiteral(trailer.Item1);
        }

        public void WriteAttribute<T1, T2, T3>(string name, Tuple<string, int> leader, Tuple<string, int> trailer, Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1, Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2, Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (leader == null)
            {
                throw new ArgumentNullException("leader");
            }
            if (trailer == null)
            {
                throw new ArgumentNullException("trailer");
            }
            if (part1 == null)
            {
                throw new ArgumentNullException("part1");
            }
            if (part2 == null)
            {
                throw new ArgumentNullException("part2");
            }
            if (part3 == null)
            {
                throw new ArgumentNullException("part3");
            }
            WriteLiteral(leader.Item1);
            WriteLiteral(part1.Item1.Item1);
            Write(part1.Item2.Item1);
            WriteLiteral(part2.Item1.Item1);
            Write(part2.Item2.Item1);
            WriteLiteral(part3.Item1.Item1);
            Write(part3.Item2.Item1);
            WriteLiteral(trailer.Item1);
        }

        public void WriteAttribute<T1, T2, T3, T4>(string name, Tuple<string, int> leader, Tuple<string, int> trailer, Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1, Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2, Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3, Tuple<Tuple<string, int>, Tuple<T4, int>, bool> part4)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (leader == null)
            {
                throw new ArgumentNullException("leader");
            }
            if (trailer == null)
            {
                throw new ArgumentNullException("trailer");
            }
            if (part1 == null)
            {
                throw new ArgumentNullException("part1");
            }
            if (part2 == null)
            {
                throw new ArgumentNullException("part2");
            }
            if (part3 == null)
            {
                throw new ArgumentNullException("part3");
            }
            if (part4 == null)
            {
                throw new ArgumentNullException("part4");
            }
            WriteLiteral(leader.Item1);
            WriteLiteral(part1.Item1.Item1);
            Write(part1.Item2.Item1);
            WriteLiteral(part2.Item1.Item1);
            Write(part2.Item2.Item1);
            WriteLiteral(part3.Item1.Item1);
            Write(part3.Item2.Item1);
            WriteLiteral(part4.Item1.Item1);
            Write(part4.Item2.Item1);
            WriteLiteral(trailer.Item1);
        }

        public void WriteAttribute<T1, T2, T3, T4, T5>(string name, Tuple<string, int> leader, Tuple<string, int> trailer, Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1, Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2, Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3, Tuple<Tuple<string, int>, Tuple<T4, int>, bool> part4, Tuple<Tuple<string, int>, Tuple<T5, int>, bool> part5)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (leader == null)
            {
                throw new ArgumentNullException("leader");
            }
            if (trailer == null)
            {
                throw new ArgumentNullException("trailer");
            }
            if (part1 == null)
            {
                throw new ArgumentNullException("part1");
            }
            if (part2 == null)
            {
                throw new ArgumentNullException("part2");
            }
            if (part3 == null)
            {
                throw new ArgumentNullException("part3");
            }
            if (part4 == null)
            {
                throw new ArgumentNullException("part4");
            }
            if (part5 == null)
            {
                throw new ArgumentNullException("part5");
            }
            WriteLiteral(leader.Item1);
            WriteLiteral(part1.Item1.Item1);
            Write(part1.Item2.Item1);
            WriteLiteral(part2.Item1.Item1);
            Write(part2.Item2.Item1);
            WriteLiteral(part3.Item1.Item1);
            Write(part3.Item2.Item1);
            WriteLiteral(part4.Item1.Item1);
            Write(part4.Item2.Item1);
            WriteLiteral(part5.Item1.Item1);
            Write(part5.Item2.Item1);
            WriteLiteral(trailer.Item1);
        }

        public void WriteAttribute<T1, T2, T3, T4, T5, T6>(string name, Tuple<string, int> leader, Tuple<string, int> trailer, Tuple<Tuple<string, int>, Tuple<T1, int>, bool> part1, Tuple<Tuple<string, int>, Tuple<T2, int>, bool> part2, Tuple<Tuple<string, int>, Tuple<T3, int>, bool> part3, Tuple<Tuple<string, int>, Tuple<T4, int>, bool> part4, Tuple<Tuple<string, int>, Tuple<T5, int>, bool> part5, Tuple<Tuple<string, int>, Tuple<T6, int>, bool> part6)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (leader == null)
            {
                throw new ArgumentNullException("leader");
            }
            if (trailer == null)
            {
                throw new ArgumentNullException("trailer");
            }
            if (part1 == null)
            {
                throw new ArgumentNullException("part1");
            }
            if (part2 == null)
            {
                throw new ArgumentNullException("part2");
            }
            if (part3 == null)
            {
                throw new ArgumentNullException("part3");
            }
            if (part4 == null)
            {
                throw new ArgumentNullException("part4");
            }
            if (part5 == null)
            {
                throw new ArgumentNullException("part5");
            }
            if (part6 == null)
            {
                throw new ArgumentNullException("part6");
            }
            WriteLiteral(leader.Item1);
            WriteLiteral(part1.Item1.Item1);
            Write(part1.Item2.Item1);
            WriteLiteral(part2.Item1.Item1);
            Write(part2.Item2.Item1);
            WriteLiteral(part3.Item1.Item1);
            Write(part3.Item2.Item1);
            WriteLiteral(part4.Item1.Item1);
            Write(part4.Item2.Item1);
            WriteLiteral(part5.Item1.Item1);
            Write(part5.Item2.Item1);
            WriteLiteral(part6.Item1.Item1);
            Write(part6.Item2.Item1);
            WriteLiteral(trailer.Item1);
        }

        public void WriteEncoded(string value)
        {
            
        }

        public abstract void Execute();

        void IDisposable.Dispose() => this.Output.Dispose();
    }
}
