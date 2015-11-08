using System;

namespace ObjectConditions
{
    public class ObjectValue: IExpression, ITerminalExpression, IEquatable<ObjectValue>
    {
        public bool IsNegated { get; set; }

        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectValue);
        }

        public bool Equals(ObjectValue val)
        {
            if (ReferenceEquals(val, null))
            {
                return false;
            }

            if (ReferenceEquals(this, val))
            {
                return true;
            }

            if (GetType() != val.GetType())
                return false;

            return val.Value.Equals(Value)
                && val.IsNegated.Equals(IsNegated);
        }

        public override int GetHashCode()
        {
            var hash = 37;
            hash = hash * 29 + IsNegated.GetHashCode();
            hash = hash * 29 + Value.GetHashCode();
            return hash;
        }

        public static bool operator ==(ObjectValue lhs, ObjectValue rhs)
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

        public static bool operator !=(ObjectValue lhs, ObjectValue rhs)
        {
            return !(lhs == rhs);
        }
    }
}
