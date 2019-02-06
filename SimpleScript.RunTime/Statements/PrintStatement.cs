using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class PrintStatement : IStatement
    {
        private Expression expr;

        public PrintStatement(Expression expr)
        {
            this.expr = expr;
        }

        #region IStatement Members

        public string Print()
        {
            return "Print " + this.expr.Print();
        }

        public void Execute()
        {
            object result = this.expr.Evaluate();
            switch (this.expr.Type)
            {
                case SimpleScriptTypes.Integer:
                    System.Console.Write((long)result);
                    break;
                case SimpleScriptTypes.Double:
                    System.Console.Write((double)result);
                    break;
                case SimpleScriptTypes.Boolean:
                    System.Console.Write((bool)result);
                    break;
                case SimpleScriptTypes.String:
                    System.Console.Write((string)result);
                    break;
                case SimpleScriptTypes.None:
                    System.Console.Write(result);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
