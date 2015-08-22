using System;

namespace ObjectConditions
{
    /// <summary>
    /// Class that represents a config value.
    /// </summary>
    public class ConfigValue : IAstObject, IBooleanExpression, INumericExpression, IStringExpression, IEquatable<ConfigValue>
    {
        public string Value { get; set; }

        public int ChildrenCount
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Since there is no binding to real system, the function evaluate the object always in that way.
        /// </summary>
        /// <returns>Test data.</returns>
        public string EvaluateStringExpression()
        {
            return "test";
        }

        /// <summary>
        /// Since there is no binding to real system, the function evaluate the object always in that way.
        /// </summary>
        /// <returns>Test data.</returns>
        public int EvaluateNumericExpression()
        {
            return 0;
        }

        /// <summary>
        /// Since there is no binding to real system, the function evaluate the object always in that way.
        /// </summary>
        /// <returns>Test data.</returns>
        public bool EvaluateBooleanExpression()
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ConfigValue);
        }

        public bool Equals(ConfigValue val)
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

            return this.Value.Equals(val.Value);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public static bool operator ==(ConfigValue lhs, ConfigValue rhs)
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

        public static bool operator !=(ConfigValue lhs, ConfigValue rhs)
        {
            return !(lhs == rhs);
        }
    }
}
