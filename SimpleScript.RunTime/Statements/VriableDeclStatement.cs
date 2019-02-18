using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class VriableDeclStatement: IStatement 
    {
        private int varID;

        private long arraySize;

        public VriableDeclStatement(int id)
        {
            this.varID = id;
        }

        public VriableDeclStatement(int id, long size)
        {
            this.varID = id;
            this.arraySize = size;
        }

        public string Print()
        {
            return SymbolTable.GetInstance.Get(this.varID).Name;
        }

        public void Execute()
        {
            // Logic in the parser.
            return;
        }
    }
}
