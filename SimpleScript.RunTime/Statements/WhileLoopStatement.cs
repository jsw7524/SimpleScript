using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime.Statements
{
    public class WhileLoopStatement : IStatement
    {
        private Expression conditionExpr;
        private StatementList statements;
        public WhileLoopStatement(Expression conditionExpr, StatementList statements)
        {
            this.conditionExpr = conditionExpr;
            this.statements = statements;
        }
        public void Execute()
        {
            while ((bool)this.conditionExpr.Evaluate())
            {
                this.statements.Execute();
            }
        }

        public string Print()
        {
            throw new NotImplementedException();
        }
    }
}
