using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents boolean binary relation.
    /// </summary>
    public class BooleanBinaryRelation : IAstObject, ILogicalExpression, ITerminalExpression, IEquatable<BooleanBinaryRelation>
    {
        public bool IsNegated { get; set; }

        public IBooleanExpression LeftOperand { get; set; }

        public BooleanBinaryOperators Operator { get; set; }

        public IBooleanExpression RightOperand { get; set; }

        public int ChildrenCount
        {
            get
            {
                // the object itself
                return 3
                       + RightOperand.ChildrenCount
                       + LeftOperand.ChildrenCount;
            }
        }

        public bool EvaluateLogicalExpression()
        {
            bool result;

            switch (this.Operator)
            {
                case BooleanBinaryOperators.Equality:
                    result = LeftOperand.EvaluateBooleanExpression() == RightOperand.EvaluateBooleanExpression();
                    break;
                case BooleanBinaryOperators.Inequality:
                    result = LeftOperand.EvaluateBooleanExpression() != RightOperand.EvaluateBooleanExpression();
                    break;
                default:
                    throw new AstEvaluationException(string.Format("Unknown Boolean Binary Operator {0}", Operator));
            }

            return IsNegated ? !result : result;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BooleanBinaryRelation);
        }

        public bool Equals(BooleanBinaryRelation rel)
        {
            if (Object.ReferenceEquals(rel, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, rel))
            {
                return true;
            }

            if (this.GetType() != rel.GetType())
                return false;

            return rel.LeftOperand.Equals(this.LeftOperand)
                && rel.Operator.Equals(this.Operator)
                && rel.RightOperand.Equals(this.RightOperand)
                && rel.IsNegated.Equals(this.IsNegated);
        }

        public override int GetHashCode()
        {
            int hash = 37;
            hash = hash * 29 + this.LeftOperand.GetHashCode();
            hash = hash * 29 + (int)this.Operator;
            hash = hash * 29 + this.RightOperand.GetHashCode();
            hash = hash * 29 + this.IsNegated.GetHashCode();
            return hash;
        }

        public static bool operator ==(BooleanBinaryRelation lhs, BooleanBinaryRelation rhs)
        {
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(BooleanBinaryRelation lhs, BooleanBinaryRelation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
