using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents string binary relation.
    /// </summary>
    public class StringBinaryRelation : IAstObject, ILogicalExpression, ITerminalExpression, IEquatable<StringBinaryRelation>
    {
        public bool IsNegated { get; set; }

        public IStringExpression LeftOperand { get; set; }

        public StringBinaryOperators Operator { get; set; }

        public IStringExpression RightOperand { get; set; }

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

            switch (Operator)
            {
                case StringBinaryOperators.Equality:
                    result = string.Equals(LeftOperand.EvaluateStringExpression(), RightOperand.EvaluateStringExpression(), StringComparison.InvariantCulture);
                    break;
                case StringBinaryOperators.Inequality:
                    result = !string.Equals(LeftOperand.EvaluateStringExpression(), RightOperand.EvaluateStringExpression(), StringComparison.InvariantCulture);
                    break;
                default:
                    throw new AstEvaluationException(string.Format("Unknown String Binary Operator {0}", Operator));
            }

            return IsNegated ? !result : result;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as StringBinaryRelation);
        }

        public bool Equals(StringBinaryRelation rel)
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
            return this.LeftOperand.GetHashCode()
                 + (int)this.Operator * 0x00010000
                 + this.RightOperand.GetHashCode()
                 + this.IsNegated.GetHashCode();
        }

        public static bool operator ==(StringBinaryRelation lhs, StringBinaryRelation rhs)
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

        public static bool operator !=(StringBinaryRelation lhs, StringBinaryRelation rhs)
        {
            return !(lhs == rhs);
        }
    }
}