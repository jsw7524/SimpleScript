using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class AssignmentStatement : IStatement
    {
        private int id;
        private Expression exp;

        private Expression arrayIndex;

        public AssignmentStatement(int id, Expression exp)
        {
            this.id = id;
            this.exp = exp;
        }

        public AssignmentStatement(int id, Expression index, Expression exp)
        {
            this.id = id;
            arrayIndex = index;
            this.exp = exp;
        }

        #region IStatement Members

        public string Print()
        {
            return SymbolTable.GetInstance.Get(id).Name + " = " + exp.Print();
        }

        public void Execute()
        {
            SymbolTableElement elem = SymbolTable.GetInstance.Get(id);
            if (elem is SymbolTableIntegerElement)
            {
                SymbolTableIntegerElement ielem = (SymbolTableIntegerElement)elem;
                if (exp.Type == SimpleScriptTypes.Integer)
                {
                    ielem.Value = (long)exp.Evaluate();
                }
                else
                {
                    throw new InvalidOperationException("Invalide assignment.");
                }
                return;
            }
            if (elem is SymbolTableBoolElement)
            {
                SymbolTableBoolElement belem = (SymbolTableBoolElement)elem;
                if (exp.Type == SimpleScriptTypes.Boolean)
                {
                    belem.Value = (bool)exp.Evaluate();
                }
                else
                {
                    throw new InvalidOperationException("Invalide assignment.");
                }
                return;
            }
            if (elem is SymbolTableDoubleElement)
            {
                SymbolTableDoubleElement delem = (SymbolTableDoubleElement)elem;
                if (exp.Type == SimpleScriptTypes.Double || exp.Type == SimpleScriptTypes.Integer )
                {
                    delem.Value = (double)exp.Evaluate();
                }
                else
                {
                    throw new InvalidOperationException("Invalide assignment.");
                }
            }
            if (elem is SymbolTableStringElement)
            {
                SymbolTableStringElement selem = (SymbolTableStringElement)elem;
                if (exp.Type == SimpleScriptTypes.Double)
                {
                    selem.Value = exp.Evaluate().ToString();
                }
                else
                {
                    throw new InvalidOperationException("Invalide assignment.");
                }
            }

            if (elem is SymbolTableIntegerArrayElement)
            {
                SymbolTableIntegerArrayElement ielem = (SymbolTableIntegerArrayElement)elem;
                if (exp.Type == SimpleScriptTypes.Integer)
                {
                    long index = (long)arrayIndex.Evaluate();
                    ielem.container[(int)index] = (long)exp.Evaluate();
                }
                else
                {
                    throw new InvalidOperationException("Invalide assignment.");
                }
            }

        }

        #endregion
    }
}
