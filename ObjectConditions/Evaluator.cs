using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectConditions
{
    public class Evaluator
    {
        public static readonly Dictionary<ExpressionTypes, List<UnaryOperators>> OperatorsAndTypesInUnaryRelation =
            new Dictionary<ExpressionTypes, List<UnaryOperators>>()
            {
                { ExpressionTypes.Integer, new List<UnaryOperators>() },
                { ExpressionTypes.String, new List<UnaryOperators>() },
                { ExpressionTypes.Boolean, new List<UnaryOperators>() { UnaryOperators.Negation } },
                { ExpressionTypes.SystemObject, new List<UnaryOperators>() { UnaryOperators.Exist, UnaryOperators.NotExist } },
                { ExpressionTypes.UnaryRelation, new List<UnaryOperators>() { UnaryOperators.Negation } },
                { ExpressionTypes.BinaryRelation, new List<UnaryOperators>() { UnaryOperators.Negation } }
            };

        public static readonly Dictionary<ExpressionTypes, List<BinaryOperators>> OperatorsAndTypesInBinaryRelation =
            new Dictionary<ExpressionTypes, List<BinaryOperators>>()
            {
                { ExpressionTypes.Integer, new List<BinaryOperators>() { BinaryOperators.Equality, BinaryOperators.Inequality, BinaryOperators.GreaterOrEqual, BinaryOperators.GreaterThan, BinaryOperators.LessOrEqual, BinaryOperators.LessThan } },
                { ExpressionTypes.String, new List<BinaryOperators>() { BinaryOperators.Equality, BinaryOperators.Inequality } },
                { ExpressionTypes.Boolean, new List<BinaryOperators>() { BinaryOperators.Equality, BinaryOperators.Inequality, BinaryOperators.Conjunction, BinaryOperators.Disjunction, BinaryOperators.Implication } },
                { ExpressionTypes.SystemObject, new List<BinaryOperators>() { BinaryOperators.Conjunction, BinaryOperators.Disjunction, BinaryOperators.Equality, BinaryOperators.GreaterOrEqual, BinaryOperators.GreaterThan, BinaryOperators.Implication, BinaryOperators.Inequality, BinaryOperators.LessOrEqual, BinaryOperators.LessThan } },
                { ExpressionTypes.UnaryRelation, new List<BinaryOperators>() { BinaryOperators.Equality, BinaryOperators.Inequality, BinaryOperators.Conjunction, BinaryOperators.Disjunction, BinaryOperators.Implication } },
                { ExpressionTypes.BinaryRelation, new List<BinaryOperators>() { BinaryOperators.Equality, BinaryOperators.Inequality, BinaryOperators.Conjunction, BinaryOperators.Disjunction, BinaryOperators.Implication }}
            };

        public static readonly Dictionary<ExpressionTypes, List<ExpressionTypes>> TypesInBinaryRelation =
            new Dictionary<ExpressionTypes, List<ExpressionTypes>>()
            {
                { ExpressionTypes.Integer, new List<ExpressionTypes>() { ExpressionTypes.SystemObject, ExpressionTypes.Integer } },
                { ExpressionTypes.String, new List<ExpressionTypes>() { ExpressionTypes.SystemObject, ExpressionTypes.String } },
                { ExpressionTypes.Boolean, new List<ExpressionTypes>() { ExpressionTypes.SystemObject, ExpressionTypes.BinaryRelation, ExpressionTypes.UnaryRelation } },
                { ExpressionTypes.SystemObject, new List<ExpressionTypes>() { ExpressionTypes.Integer, ExpressionTypes.String, ExpressionTypes.SystemObject, ExpressionTypes.Boolean } },
                { ExpressionTypes.UnaryRelation, new List<ExpressionTypes>() { ExpressionTypes.UnaryRelation, ExpressionTypes.BinaryRelation, ExpressionTypes.Boolean } },
                { ExpressionTypes.BinaryRelation, new List<ExpressionTypes>() { ExpressionTypes.BinaryRelation, ExpressionTypes.UnaryRelation, ExpressionTypes.Boolean }}
            };

        public static void CheckTypes(IExpression expr)
        {
            if (expr == null)
            {
                throw new ArgumentNullException("expr");
            }

            switch (expr.ExpressionType)
            {
                case ExpressionTypes.BinaryRelation:
                {
                    var rel = expr as BinaryRelation;
                    var typeRight = rel.Right.ExpressionType;
                    var typeLeft = rel.Left.ExpressionType;

                    if (!OperatorsAndTypesInBinaryRelation.ContainsKey(typeRight))
                    {
                        throw new TypeCheckerException(String.Format("Unknown type {0}", typeRight), expr);
                    }

                    var comply = OperatorsAndTypesInBinaryRelation.Any(x => x.Key == typeRight && x.Value.Contains(rel.Operator));

                    if (!comply)
                    {
                        var msg = new StringBuilder();

                        msg.AppendFormat("Operator {0} cannot be used with type {1}. List of avaiable operators:", rel.Operator, typeRight);
                        foreach (var op in OperatorsAndTypesInBinaryRelation[typeRight])
                        {
                            msg.AppendFormat(" {0} ", op);
                        }

                        throw new TypeCheckerException(msg.ToString(), expr);
                    }

                    if (!OperatorsAndTypesInBinaryRelation.ContainsKey(typeLeft))
                    {
                        throw new TypeCheckerException(String.Format("Unknown type {0}", typeLeft), expr);
                    }

                    comply = OperatorsAndTypesInBinaryRelation.Any(x => x.Key == typeLeft && x.Value.Contains(rel.Operator));

                    if (!comply)
                    {
                        var msg = new StringBuilder();

                        msg.AppendFormat("Operator {0} cannot be used with type {1}. List of avaiable operators:", rel.Operator, typeLeft);
                        foreach (var op in OperatorsAndTypesInBinaryRelation[typeLeft])
                        {
                            msg.AppendFormat(" {0} ", op);
                        }

                        throw new TypeCheckerException(msg.ToString(), expr);
                    }

                    comply = TypesInBinaryRelation[typeLeft].Contains(typeRight);

                    if (!comply)
                    {
                        throw new TypeCheckerException(String.Format("Types {0} and {1} cannot be used together in binary relation", typeLeft, typeRight), expr);
                    }

                    break;
                }
                case ExpressionTypes.UnaryRelation:
                {
                    var rel = expr as UnaryRelation;
                    var exprType = rel.Expression.ExpressionType;

                    var comply = OperatorsAndTypesInUnaryRelation.Any(x => x.Key == exprType && x.Value.Contains(rel.Operator));

                    if (!OperatorsAndTypesInUnaryRelation.ContainsKey(exprType))
                    {
                        throw new TypeCheckerException(String.Format("Unknown type {0}", exprType), expr);
                    }

                    if (!comply)
                    {
                        var msg = new StringBuilder();

                        msg.AppendFormat("Operator {0} cannot be used with type {1}. List of avaiable operators:", rel.Operator, exprType);
                        foreach (var op in OperatorsAndTypesInUnaryRelation[exprType])
                        {
                            msg.AppendFormat(" {0} ", op);
                        }

                        throw new TypeCheckerException(msg.ToString(), expr);
                    }

                    break;
                }
            }

            foreach (var child in expr.Children)
            {
                CheckTypes(child);
            }
        }
    }
}
