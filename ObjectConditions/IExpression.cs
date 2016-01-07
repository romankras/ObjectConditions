using System.Collections.Generic;

namespace ObjectConditions
{
    public interface IExpression
    {
        IEnumerable<IExpression> Children { get; }

        ExpressionTypes ExpressionType { get; set; }
    }
}
