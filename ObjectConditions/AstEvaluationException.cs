using System;

namespace ObjectConditions
{
    /// <summary>
    /// Exception that is being thrown during evaluation of abstract syntax tree.
    /// </summary>
    public class AstEvaluationException: Exception
    {
        public AstEvaluationException()
        {
        }

        public AstEvaluationException(string message)
            : base(message)
        {
        }

        public AstEvaluationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
