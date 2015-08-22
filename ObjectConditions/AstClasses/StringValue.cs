using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents a string value.
    /// </summary>
    public class StringValue : IAstObject, IStringExpression, IEquatable<StringValue>
    {
        public string Value { get; set; }

        public int ChildrenCount
        {
            get
            {
                return 0;
            }
        }

        public string EvaluateStringExpression()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as StringValue);
        }

        public bool Equals(StringValue val)
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
            return this.Value.GetHashCode();
        }

        public static bool operator ==(StringValue lhs, StringValue rhs)
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

        public static bool operator !=(StringValue lhs, StringValue rhs)
        {
            return !(lhs == rhs);
        }
    }
}
