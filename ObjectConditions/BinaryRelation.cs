using System;

namespace ObjectConditions
{
    public class BinaryRelation: IExpression, IEquatable<BinaryRelation>
    {
        public bool IsNegated { get; set; }

        public IExpression Left { get; set; }

        public BinaryOperators Operator { get; set; }

        public IExpression Right { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BinaryRelation);
        }

        public bool Equals(BinaryRelation rel)
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

            return rel.Left.Equals(Left)
                && rel.Operator.Equals(Operator)
                && rel.Right.Equals(Right)
                && rel.IsNegated.Equals(IsNegated);
        }

        public override int GetHashCode()
        {
            var hash = 37;
            hash = hash * 29 + Left.GetHashCode();
            hash = hash * 29 + (int)Operator;
            hash = hash * 29 + Right.GetHashCode();
            hash = hash * 29 + IsNegated.GetHashCode();
            return hash;
        }

        public static bool operator ==(BinaryRelation lhs, BinaryRelation rhs)
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

        public static bool operator !=(BinaryRelation lhs, BinaryRelation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
