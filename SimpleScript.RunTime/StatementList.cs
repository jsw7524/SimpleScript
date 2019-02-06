using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class StatementList : IStatement
    {
        public void Add(IStatement statement)
        {
            statements.Add(statement);
        }

        public void InsertFront(IStatement statement)
        {
            statements.Insert(0, statement);
        }

        #region IStatement Members

        public string Print()
        {
            StringBuilder str = new StringBuilder();
            foreach (IStatement st in statements)
            {
                str.AppendLine(st.Print());
            }

            return str.ToString();
        }

        public void Execute()
        {
            foreach (IStatement st in statements)
            {
                if (st != null)
                {
                    st.Execute();
                }
            }
        }

        #endregion

        private List<IStatement> statements = new List<IStatement>();
    }
}
