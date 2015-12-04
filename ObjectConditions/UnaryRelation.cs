using System;
using System.Collections.Generic;

namespace ObjectConditions
{
    public class UnaryRelation: IExpression, IEquatable<UnaryRelation>
    {
        public IExpression Expression { get; set; }

        public UnaryOperators Operator { get; set; }

        public List<IExpression> Children => new List<IExpression>() { Expression };

        public override bool Equals(object obj)
        {
            return Equals(obj as UnaryRelation);
        }

        public bool Equals(UnaryRelation rel)
        {
            if (ReferenceEquals(rel, null))
            {
                return false;
            }

            if (ReferenceEquals(this, rel))
            {
                return true;
            }

            if (GetType() != rel.GetType())
                return false;

            return Equals(rel.Expression, Expression)
                && rel.Operator == Operator;
        }

        public override int GetHashCode()
        {
            var hash = 37;
            hash = hash * 29 + (Expression?.GetHashCode() ?? 0);
            hash = hash * 29 + (int)Operator;
            return hash;
        }

        public static bool operator ==(UnaryRelation lhs, UnaryRelation rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                if (ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(UnaryRelation lhs, UnaryRelation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
