using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class InputStatement : IStatement
    {
        #region Constructor

        public InputStatement(int id)
        {
            this.symbolId = id;
        }

        #endregion

        #region Private Fields

        private int symbolId;

        #endregion

        #region IStatement Members

        public string Print()
        {
            string result = "input ";
            SymbolTableElement ste = SymbolTable.GetInstance.Get(symbolId);
            return result + ste.Name;
        }

        public void Execute()
        {
            SymbolTableElement st = SymbolTable.GetInstance.Get(symbolId);
            if (st is SymbolTableIntegerElement)
            {
                int value = Convert.ToInt32(Console.ReadLine());
                SymbolTableIntegerElement ist = st as SymbolTableIntegerElement;
                ist.Value = value;
                return;
            }
            if (st is SymbolTableBoolElement)
            {
                bool value = Convert.ToBoolean(Console.ReadLine());
                SymbolTableBoolElement bst = st as SymbolTableBoolElement;
                bst.Value = value;
                return;
            }
            if (st is SymbolTableDoubleElement)
            {
                double value = Convert.ToDouble(Console.ReadLine());
                SymbolTableDoubleElement dst = st as SymbolTableDoubleElement;
                dst.Value = value;
                return;
            }
            if (st is SymbolTableStringElement)
            {
                string value = Console.ReadLine();
                SymbolTableStringElement sst = st as SymbolTableStringElement;
                sst.Value = value;
                return;
            }
        }

        #endregion
    }
}
