using System.Collections.Generic;

namespace ObjectConditions
{
    public interface IExpression
    {
        List<IExpression> Children { get; }
    }
}
