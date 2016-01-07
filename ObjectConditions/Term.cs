using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectConditions
{
    public class Term: IExpression, IEquatable<Term>
    {
        public IEnumerable<IExpression> Children
        {
            get
            {
                return Enumerable.Empty<IExpression>();
            }
        }

        public ExpressionTypes ExpressionType { get; set; }

        public string ObjectType { get; set; }

        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Term);
        }

        public bool Equals(Term val)
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

            return val.Value == Value
                && val.ExpressionType == ExpressionType
                && val.ObjectType == ObjectType;
        }

        public override int GetHashCode()
        {
            var hash = 37;
            hash = hash * 29 + (int)ExpressionType;

            if (ObjectType != null)
            {
                hash = hash * 29 + ObjectType.GetHashCode();
            }
            else
            {
                hash = hash * 29;
            }

            if (Value != null)
            {
                hash = hash * 29 + Value.GetHashCode();
            }
            else
            {
                hash = hash * 29;
            }

            return hash;
        }

        public static bool operator ==(Term lhs, Term rhs)
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

        public static bool operator !=(Term lhs, Term rhs)
        {
            return !(lhs == rhs);
        }
    }
}
