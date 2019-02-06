using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class IfCondStatement : IStatement
    {
        StatementList ifBranchStatements = null;
        StatementList elseBranchStatements = null;
        Expression conditionExpr = null;

        //bool hasElseBranch = false;
        ///// <summary>
        ///// Property indicating wether ther is an else branch.
        ///// </summary>
        //public bool HasElseBranch {
        //    get { return this.hasElseBranch; }
        //    set { this.hasElseBranch = value; }
        //}

        public IfCondStatement(Expression condition, StatementList ifBranch,
                            StatementList elseBranch)
        {
            if (condition == null)
            {
                throw new ArgumentException("Invalid condition for IF statement.");
            }
            else
            {
                this.conditionExpr = condition;
            }

            this.ifBranchStatements = ifBranch;
            this.elseBranchStatements = elseBranch;
        }

        #region IStatement Members

        public string Print()
        {
            string pattern = "If ({0}) Then\n {1} Else {2}";
            return string.Format(pattern, this.conditionExpr.Print(), ifBranchStatements.Print(), elseBranchStatements.Print());
        }

        public void Execute()
        {
            bool result = (bool)conditionExpr.Evaluate();
            if (result)
            {
                ifBranchStatements.Execute();
            }
            else
            {
                elseBranchStatements.Execute();
            }
        }

        #endregion
    }
}
