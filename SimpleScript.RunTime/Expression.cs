using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleScript.RunTime
{
    public enum Operation
    {
        Constant,
        SymbolElement,
        UnaryMinus,
        Sub,
        Add,
        Mul,
        Div,
        Great,
        Less,
        And,
        Or,
        Not,
        Equ,
        NotEqu,
        Lt,
        Gt,
        LtEq,
        GtEq
    }

    public class Expression
    {
        private object constValue;
        private SimpleScriptTypes type;
        private SymbolTableElement element;
        private Expression leftExpression;
        private Expression rightExpression;
        private Operation operation;

        public SimpleScriptTypes Type
        {
            get { return type; }
        }

        public Operation Operation 
        { 
            get
            {
                return this.operation;
            }
        }

        public Expression(SymbolTableElement elem)
        {
            if (elem == null)
            {
                throw new ArgumentNullException("elem");
            }

            this.operation = Operation.SymbolElement;
            this.element = elem;

            if (elem is SymbolTableIntegerElement)
            {
                type = SimpleScriptTypes.Integer;
            }
            if (elem is SymbolTableBoolElement)
            {
                type = SimpleScriptTypes.Boolean;
            }
            if (elem is SymbolTableDoubleElement)
            {
                type = SimpleScriptTypes.Double;
            }
            if (elem is SymbolTableStringElement)
            {
                type = SimpleScriptTypes.String;
            }
            element = elem;
        }

        public Expression(long value)
        {
            this.operation = Operation.Constant;
            type = SimpleScriptTypes.Integer;
            constValue = value;
        }

        public Expression(bool value)
        {
            this.operation = Operation.Constant;
            type = SimpleScriptTypes.Boolean;
            constValue = value;
        }

        public Expression(double value)
        {
            this.operation = Operation.Constant;
            type = SimpleScriptTypes.Double;
            constValue = value;
        }

        public Expression(string value)
        {
            this.operation = Operation.Constant;
            type = SimpleScriptTypes.String;
            constValue = value;
        }

        /// <summary>
        /// Builds a complex expression.
        /// </summary>
        /// <param name="op">Operation</param>
        /// <param name="left">Left side expression.</param>
        /// <param name="right">Right side expression.</param>
        public Expression(Operation op, Expression left, Expression right)
        {
            #region Argument validation
            
            if (right == null)
            {
                throw new ArgumentNullException("Expression right handside is null.");
            }

            if (left == null)
            {
                // Left can be null only if the operation is unary minus.
                if (operation == Operation.UnaryMinus)
                {
                    switch (right.Type)
                    {
                        case SimpleScriptTypes.Integer:
                            type = SimpleScriptTypes.Integer;
                            break;

                        case SimpleScriptTypes.Double:
                            type = SimpleScriptTypes.Double;
                            break;

                        default:
                            throw new InvalidOperationException();
                            break;
                    }
                }
                else
                {
                    throw new ArgumentNullException("");
                }
            }
            #endregion

            this.leftExpression = left;
            this.rightExpression = right;
            this.operation = op;

            #region Operation result type validation
            OperationValidator opValid = new OperationValidator();
            switch (operation)
            {
                case Operation.Add:
                case Operation.Sub:
                case Operation.Mul:
                case Operation.Div:
                    type = opValid.CheckType(left.Type, right.Type);
                    if (type == SimpleScriptTypes.None)
                    {
                        throw new InvalidOperationException();
                    }
                    break;

                case Operation.Great:
                case Operation.Less:
                case Operation.Equ:
                case Operation.NotEqu:
                    type = opValid.CheckOperation(left.Type, right.Type);
                    if (type == SimpleScriptTypes.None)
                    {
                        throw new InvalidOperationException();
                    }
                    break;
            }
            #endregion
        }

        //ToDo: Fix in future. I don't like something here.
        public string Print()
        {
            if (this.operation == Operation.UnaryMinus)
            {
                return operation.ToString() + " " + rightExpression.Print();
            }

            if (this.operation == Operation.SymbolElement)
            {
                return element.Name;
            }

            if (this.operation == Operation.Constant)
            {
                return constValue.ToString();
            }

            return "(" + leftExpression.Print() + " " + operation.ToString() + " " + rightExpression.Print() + ")";      
        }

        public object Evaluate()
        {
            if (this.operation == Operation.SymbolElement)
            {
                if (element is SymbolTableIntegerElement)
                {
                    return ((SymbolTableIntegerElement)element).Value;
                }
                if (element is SymbolTableDoubleElement)
                {
                    return ((SymbolTableDoubleElement)element).Value;
                }
                if (element is SymbolTableBoolElement)
                {
                    return ((SymbolTableBoolElement)element).Value;
                }
                if (element is SymbolTableStringElement)
                {
                    return ((SymbolTableStringElement)element).Value;
                }
                throw new InvalidOperationException();
            }
            if (this.operation == Operation.Constant)
	        {
		        return this.constValue;
	        }
            return EvaluateComplex();
        }

        private object EvaluateComplex()
        {
            object lValue = null;
            object rValue = null;

            if (this.operation == Operation.UnaryMinus)
            {
                rValue = rightExpression.Evaluate();
                switch (rightExpression.Type)
                {
                    case SimpleScriptTypes.Integer:
                        return (-1) * (long)rValue;
                        break;

                    case SimpleScriptTypes.Double:
                        return (-1.0) * (double)rValue;
                        break;
                }
            }

            lValue = leftExpression.Evaluate();
            rValue = rightExpression.Evaluate();
            switch (operation)
            {
                case Operation.Add:
                    if (type == SimpleScriptTypes.Integer)
                    {
                        return (long)lValue + (long)rValue;
                    }
                    else
                    {
                        if (type == SimpleScriptTypes.Double)
                        {
                            if (leftExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (long)lValue + (double)rValue;
                            }
                            if (rightExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (double)lValue + (long)rValue;
                            }
                            return (double)lValue + (double)rValue;
                        }
                    }
                break;
                case Operation.Sub:
                    if (type == SimpleScriptTypes.Integer)
                    {
                        return (long)lValue - (long)rValue;
                    }
                    else
                    {
                        if (type == SimpleScriptTypes.Double)
                        {
                            if (leftExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (long)lValue - (double)rValue;
                            }
                            if (rightExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (double)lValue - (long)rValue;
                            }
                            return (double)lValue - (double)rValue;
                        }
                    }
                break;
                case Operation.Mul:    
                    if (type == SimpleScriptTypes.Integer)
                    {
                        return (long)lValue * (long)rValue;
                    }
                    else
                    {
                        if (type == SimpleScriptTypes.Double)
                        {
                            if (leftExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (long)lValue * (double)rValue;
                            }
                            if (rightExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (double)lValue * (long)rValue;
                            }
                            return (double)lValue * (double)rValue;
                        }
                    }
                break;
                case Operation.Div:
                    if (type == SimpleScriptTypes.Integer)
                    {
                        return (long)lValue / (long)rValue;
                    }
                    else
                    {
                        if (type == SimpleScriptTypes.Double)
                        {
                            if (leftExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (long)lValue / (double)rValue;
                            }
                            if (rightExpression.Type == SimpleScriptTypes.Integer)
                            {
                                return (double)lValue / (long)rValue;
                            }
                            return (double)lValue / (double)rValue;
                        }
                    }
                break;
                case Operation.Gt:
                case Operation.Great:
                    switch (type)
                    {
                        case SimpleScriptTypes.Integer:
                            return (long)lValue > (long)rValue;
                        case SimpleScriptTypes.Double:
                            return (double)lValue > (double)rValue;
                        case SimpleScriptTypes.String:
                            int v = ((string)lValue).CompareTo(rValue);
                            if (v == 1)
                            {
                                return true;
                            }
                            return false;
                    }
                    break;
                case Operation.Lt:
                case Operation.Less:
                    switch (type)
                    {
                        case SimpleScriptTypes.Integer:
                            return (long)lValue < (long)rValue;
                        case SimpleScriptTypes.Double:
                            return (double)lValue < (double)rValue;
                        case SimpleScriptTypes.String:
                            int v = ((string)lValue).CompareTo(rValue);
                            if (v == -1)
                            {
                                return true;
                            }
                            return false;
                    }
                    break;
                case Operation.Equ:
                    switch (type)
                    {
                        case SimpleScriptTypes.Integer:
                            return (long)lValue == (long)rValue;
                        case SimpleScriptTypes.Double:
                            return (double)lValue == (double)rValue;
                        case SimpleScriptTypes.String:
                            int v = ((string)lValue).CompareTo(rValue);
                            if (v == 0)
                            {
                                return true;
                            }
                            return false;
                        case SimpleScriptTypes.Boolean:
                            return (bool)lValue == (bool)rValue;
                    }
                    break;
                case Operation.NotEqu:
                    switch (type)
                    {
                        case SimpleScriptTypes.Integer:
                            return (long)lValue != (long)rValue;
                        case SimpleScriptTypes.Double:
                            return (double)lValue != (double)rValue;
                        case SimpleScriptTypes.String:
                            int v = ((string)lValue).CompareTo(rValue);
                            if (v != 0)
                            {
                                return true;
                            }
                            return false;
                        case SimpleScriptTypes.Boolean:
                            return (bool)lValue != (bool)rValue;
                    }
                    break;
                    //ToDO Add "Gt_Eq, Lt_Eq, and, or, not" operations.
                }
            throw new InvalidOperationException();
        }
    }
}
