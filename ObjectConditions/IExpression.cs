using System.Collections.Generic;

namespace ObjectConditions
{
    public interface IExpression
    {
        bool IsNegated { get; set; }

        List<IExpression> Children { get; }
    }
}
