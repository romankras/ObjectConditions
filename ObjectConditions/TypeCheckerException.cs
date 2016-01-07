using System;

namespace ObjectConditions
{
    public class TypeCheckerException: Exception
    {
        public IExpression Expression { get; set; }

        public TypeCheckerException()
        {
        }

        public TypeCheckerException(string message)
            : base(message)
        {
        }

        public TypeCheckerException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TypeCheckerException(string message, IExpression expr)
            : base(message)
        {
            Expression = expr;
        }

        public TypeCheckerException(string message, Exception inner, IExpression expr)
            : base(message, inner)
        {
            Expression = expr;
        }
    }
}
