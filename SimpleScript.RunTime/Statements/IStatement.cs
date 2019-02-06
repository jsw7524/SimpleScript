using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public interface IStatement
    {
        string Print();
        void Execute();
    }
}
