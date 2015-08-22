using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents numeric binary relation.
    /// </summary>
    public class NumericBinaryRelation : IAstObject, ILogicalExpression, ITerminalExpression, IEquatable<NumericBinaryRelation>
    {
        public bool IsNegated { get; set; }

        public INumericExpression LeftOperand { get; set; }

        public NumericBinaryOperators Operator { get; set; }

        public INumericExpression RightOperand { get; set; }

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
                case NumericBinaryOperators.Equality:
                    result = LeftOperand.EvaluateNumericExpression() == RightOperand.EvaluateNumericExpression();
                    break;
                case NumericBinaryOperators.Inequality:
                    result = LeftOperand.EvaluateNumericExpression() != RightOperand.EvaluateNumericExpression();
                    break;
                case NumericBinaryOperators.GreaterThan:
                    result = LeftOperand.EvaluateNumericExpression() > RightOperand.EvaluateNumericExpression();
                    break;
                case NumericBinaryOperators.GreaterOrEqual:
                    result = LeftOperand.EvaluateNumericExpression() >= RightOperand.EvaluateNumericExpression();
                    break;
                case NumericBinaryOperators.LessThan:
                    result = LeftOperand.EvaluateNumericExpression() < RightOperand.EvaluateNumericExpression();
                    break;
                case NumericBinaryOperators.LessOrEqual:
                    result = LeftOperand.EvaluateNumericExpression() <= RightOperand.EvaluateNumericExpression();
                    break;
                default:
                    throw new AstEvaluationException(string.Format("Unknown Numeric Binary operator {0}", Operator));
            }

            return IsNegated ? !result : result;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as NumericBinaryRelation);
        }

        public bool Equals(NumericBinaryRelation rel)
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

        public static bool operator ==(NumericBinaryRelation lhs, NumericBinaryRelation rhs)
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

        public static bool operator !=(NumericBinaryRelation lhs, NumericBinaryRelation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
