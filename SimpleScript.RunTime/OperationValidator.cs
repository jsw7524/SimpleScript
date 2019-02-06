using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public class OperationValidator
    {
        private static SimpleScriptTypes[,] arithmeticOp = new SimpleScriptTypes[4, 4] 
        {
            { SimpleScriptTypes.Integer, SimpleScriptTypes.Double, SimpleScriptTypes.None,    SimpleScriptTypes.None },
            { SimpleScriptTypes.Double,  SimpleScriptTypes.Double, SimpleScriptTypes.None,    SimpleScriptTypes.None },
            { SimpleScriptTypes.None,    SimpleScriptTypes.None,   SimpleScriptTypes.Boolean, SimpleScriptTypes.None }, 
            { SimpleScriptTypes.None,    SimpleScriptTypes.None,   SimpleScriptTypes.None,    SimpleScriptTypes.None }
        };

        private static SimpleScriptTypes[,] logicaOp = new SimpleScriptTypes[4, 4]
        {
            { SimpleScriptTypes.Integer, SimpleScriptTypes.Double, SimpleScriptTypes.None,    SimpleScriptTypes.None },
            { SimpleScriptTypes.Double,  SimpleScriptTypes.Double, SimpleScriptTypes.None,    SimpleScriptTypes.None },
            { SimpleScriptTypes.None,    SimpleScriptTypes.None,   SimpleScriptTypes.Boolean, SimpleScriptTypes.None }, 
            { SimpleScriptTypes.None,    SimpleScriptTypes.None,   SimpleScriptTypes.None,    SimpleScriptTypes.String }
        };


        public SimpleScriptTypes CheckType(SimpleScriptTypes leftType, SimpleScriptTypes rigthType)
        {
            return arithmeticOp[(int)leftType, (int)rigthType];
        }

        public SimpleScriptTypes CheckOperation(SimpleScriptTypes leftType, SimpleScriptTypes rigthType)
        {
            return logicaOp[(int)leftType, (int)rigthType];
        }
    }
}
