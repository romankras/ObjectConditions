using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents config binary relation.
    /// </summary>
    public class ConfigBinaryRelation : IAstObject, ILogicalExpression, ITerminalExpression, IEquatable<ConfigBinaryRelation>
    {
        public bool IsNegated { get; set; }

        public ConfigValue LeftOperand { get; set; }

        public ConfigBinaryOperators Operator { get; set; }

        public ConfigValue RightOperand { get; set; }

        public bool EvaluateLogicalExpression()
        {
            bool result;

            switch (Operator)
            {
                case ConfigBinaryOperators.Equality:
                    result = LeftOperand.Value == RightOperand.Value;
                    break;
                case ConfigBinaryOperators.Inequality:
                    result = LeftOperand.Value != RightOperand.Value;
                    break;
                case ConfigBinaryOperators.GreaterThan:
                    result = true;
                    break;
                case ConfigBinaryOperators.GreaterOrEqual:
                    result = true;
                    break;
                case ConfigBinaryOperators.LessThan:
                    result = true;
                    break;
                case ConfigBinaryOperators.LessOrEqual:
                    result = true;
                    break;
                default:
                    throw new AstEvaluationException(string.Format("Unknown Config Binary operator {0}", Operator));
            }

            return IsNegated ? !result : result;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ConfigBinaryRelation);
        }

        public bool Equals(ConfigBinaryRelation rel)
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

        public static bool operator ==(ConfigBinaryRelation lhs, ConfigBinaryRelation rhs)
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

        public static bool operator !=(ConfigBinaryRelation lhs, ConfigBinaryRelation rhs)
        {
            return !(lhs == rhs);
        }
    }
}
