using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Helper
{
    public class HugeInt
    {
        //TODO: this is a huge integer, consiting of an infinite list of ints (1-999). For this we create the list and set operators (+,-,/,*,...)
        private List<int> data;
        public List<int> Numbers { get { return this.data; } }

        //Konstruktors
        public HugeInt()
        {
            this.data = new List<int>();
        }

        //Operators
        public static HugeInt operator ++(HugeInt h1)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator --(HugeInt h1)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator +(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator +(HugeInt h1, int i2)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator -(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator -(HugeInt h1, int i2)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator *(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator *(HugeInt h1, int i2)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator /(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static HugeInt operator /(HugeInt h1, int i2)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static bool operator !=(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static bool operator >(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static bool operator <(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static bool operator >=(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }
        public static bool operator <=(HugeInt h1, HugeInt h2)
        {
            throw new NotImplementedException();
        }

        //Overrides
        public override string ToString()
        {
            return String.Join(".", this.data);
        }
        public override bool Equals(HugeInt c)
        {
            throw new NotImplementedException();
        }
    }
}
