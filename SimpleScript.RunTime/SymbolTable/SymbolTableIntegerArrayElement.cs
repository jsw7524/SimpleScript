using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class SymbolTableIntegerArrayElement : SymbolTableElement
    {
        private long size;
        public List<long> container;
        public SymbolTableIntegerArrayElement(string name, long s) : base(name)
        {
            size = s;
            container = new List<long>((int)size);

            for (int i=0;i< size;i++)
            {
                container.Add(0);
            }

        }



    }
}
