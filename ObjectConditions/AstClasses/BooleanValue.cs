using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents a boolean value.
    /// </summary>
    public class BooleanValue : IAstObject, IBooleanExpression, IEquatable<BooleanValue>
    {
        public bool Value { get; set; }

        public int ChildrenCount
        {
            get
            {
                return 0;
            }
        }

        public bool EvaluateBooleanExpression()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BooleanValue);
        }

        public bool Equals(BooleanValue val)
        {
            if (Object.ReferenceEquals(val, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, val))
            {
                return true;
            }

            if (this.GetType() != val.GetType())
                return false;

            return val.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            if (this.Value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static bool operator ==(BooleanValue lhs, BooleanValue rhs)
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

        public static bool operator !=(BooleanValue lhs, BooleanValue rhs)
        {
            return !(lhs == rhs);
        }
    }
}
