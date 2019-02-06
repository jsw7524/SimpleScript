using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class VriableDeclStatement: IStatement 
    {
        private int varID;

        public VriableDeclStatement(int id)
        {
            this.varID = id;
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
