using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents logical binary relation. Should reflect binary relations from preposion logic.
    /// </summary>
    public class LogicalBinaryRelation : IAstObject, ILogicalExpression, IEquatable<LogicalBinaryRelation>
    {

        public bool IsNegated { get; set; }

        public ILogicalExpression LeftOperand { get; set; }

        public LogicalBinaryOperators Operator { get; set; }

        public ILogicalExpression RightOperand { get; set; }

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
                case LogicalBinaryOperators.Conjunction:
                    result = LeftOperand.EvaluateLogicalExpression() && RightOperand.EvaluateLogicalExpression();
                    break;
                case LogicalBinaryOperators.Disjunction:
                    result = LeftOperand.EvaluateLogicalExpression() || RightOperand.EvaluateLogicalExpression();
                    break;
                case LogicalBinaryOperators.Implication:
                    result = LeftOperand.EvaluateLogicalExpression() ? RightOperand.EvaluateLogicalExpression() : true;
                    break;
                default:
                    throw new AstEvaluationException(string.Format("Unknown Binary Logical Operator {0}", Operator));
            }

            return IsNegated ? !result : result;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as LogicalBinaryRelation);
        }

        public bool Equals(LogicalBinaryRelation rel)
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

        public static bool operator ==(LogicalBinaryRelation lhs, LogicalBinaryRelation rhs)
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

        public static bool operator !=(LogicalBinaryRelation lhs, LogicalBinaryRelation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
