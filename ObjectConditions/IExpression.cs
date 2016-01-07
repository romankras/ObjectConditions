using System.Collections.Generic;

namespace ObjectConditions
{
    public interface IExpression
    {
        IEnumerable<IExpression> Children { get; }

        string ExpressionType { get; set; }
    }
}
