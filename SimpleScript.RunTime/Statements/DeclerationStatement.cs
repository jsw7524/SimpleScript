using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class DeclerationStatement : IStatement
    {
        #region Constructor

        public DeclerationStatement(int id)
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
            StringBuilder result = new StringBuilder("Dim ");
            SymbolTableElement ste = SymbolTable.GetInstance.Get(symbolId);
            result.Append(ste.Name);
            result.Append(" as ");
            if (ste is SymbolTableIntegerElement)
            {
                result.Append("integer");
            }
            if (ste is SymbolTableDoubleElement)
            {
                result.Append("double");
            }
            if (ste is SymbolTableStringElement)
            {
                result.Append("string");
            }
            if (ste is SymbolTableBoolElement)
            {
                result.Append("bool");
            }
            return result.ToString();
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
