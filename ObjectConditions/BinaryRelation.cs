﻿using System;
using System.Collections.Generic;

namespace ObjectConditions
{
    public class BinaryRelation: IExpression, IEquatable<BinaryRelation>
    {
        public IExpression Left { get; set; }

        public BinaryOperators Operator { get; set; }

        public IExpression Right { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BinaryRelation);
        }

        public bool Equals(BinaryRelation rel)
        {
            if (ReferenceEquals(rel, null))
            {
                return false;
            }

            if (ReferenceEquals(this, rel))
            {
                return true;
            }

            if (GetType() != rel.GetType())
                return false;

            return Equals(rel.Left, Left)
                && rel.Operator == Operator
                && Equals(rel.Right, Right);
        }

        public override int GetHashCode()
        {
            var hash = 37;
            hash = hash * 29 + (Left?.GetHashCode() ?? 0);
            hash = hash * 29 + (int)Operator;
            hash = hash * 29 + (Right?.GetHashCode() ?? 0);
            return hash;
        }

        public static bool operator ==(BinaryRelation lhs, BinaryRelation rhs)
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

        public static bool operator !=(BinaryRelation lhs, BinaryRelation rhs)
        {
            return !(lhs == rhs);
        }

        public List<IExpression> Children => new List<IExpression>() { Left, Right };
    }
}
