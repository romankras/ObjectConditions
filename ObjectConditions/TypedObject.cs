using System;
using System.Collections.Generic;

namespace ObjectConditions
{
    public class TypedObject: IExpression, ITerminalExpression, IEquatable<TypedObject>
    {
        public bool IsNegated { get; set; }

        public string ObjectType { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TypedObject);
        }

        public bool Equals(TypedObject obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
                return false;

            return obj.IsNegated == IsNegated
                && obj.ObjectType == ObjectType
                && obj.Name == Name;
        }

        public override int GetHashCode()
        {
            var hash = 37;
            hash = hash * 29 + IsNegated.GetHashCode();
            hash = hash * 29 + (ObjectType?.GetHashCode() ?? 0);
            hash = hash * 29 + (Name?.GetHashCode() ?? 0);
            return hash;
        }

        public static bool operator ==(TypedObject lhs, TypedObject rhs)
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

        public static bool operator !=(TypedObject lhs, TypedObject rhs)
        {
            return !(lhs == rhs);
        }

        public List<IExpression> Children => new List<IExpression>();
    }
}
