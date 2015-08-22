﻿using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents a numeric value.
    /// </summary>
    public class NumericValue : IAstObject, INumericExpression, IEquatable<NumericValue>
    {
        public int Value { get; set; }

        public int ChildrenCount
        {
            get
            {
                return 0;
            }
        }

        public int EvaluateNumericExpression()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as NumericValue);
        }

        public bool Equals(NumericValue val)
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

        public static bool operator ==(NumericValue lhs, NumericValue rhs)
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

        public static bool operator !=(NumericValue lhs, NumericValue rhs)
        {
            return !(lhs == rhs);
        }
    }
}