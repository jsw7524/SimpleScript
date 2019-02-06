using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class ForStatement : IStatement
    {
        private SymbolTableIntegerElement ielem;
        private Expression initExpr;
        private Expression endExpr;
        private StatementList statements;

        public ForStatement(SymbolTableIntegerElement ielem, Expression initExpr, Expression endExpr, StatementList statements)
        {
            this.ielem = ielem;
            //TODO null
            if (initExpr.Type != SimpleScriptTypes.Integer || endExpr.Type != SimpleScriptTypes.Integer)
            {
                throw new ArgumentException();
            }
            this.initExpr = initExpr;
            this.endExpr = endExpr;
            this.statements = statements;
        }

        #region IStatement Members

        public string Print()
        {
            //TODO
            throw new NotImplementedException();
        }

        public void Execute()
        {
            long initVal = (long)initExpr.Evaluate();
            long endVal = (long)endExpr.Evaluate();
            for (ielem.Value = initVal; ielem.Value <= endVal; ielem.Value++)
            {
                statements.Execute();
            }
        }

        #endregion
    }
}
