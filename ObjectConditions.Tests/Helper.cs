using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sprache;

namespace ObjectConditions.Tests
{
    public class Helper
    {
        private static readonly Random Random = new Random();

        public static readonly List<ExpressionTypes> Terminals = 
            new List<ExpressionTypes>()
            {
                ExpressionTypes.String,
                ExpressionTypes.Integer,
                ExpressionTypes.Boolean,
                ExpressionTypes.SystemObject
            };

        public static readonly List<ExpressionTypes> NonTerminals =
            new List<ExpressionTypes>()
            {
                ExpressionTypes.UnaryRelation,
                ExpressionTypes.BinaryRelation
            };

        public static T GetRandomListItem<T>(List<T> list)
        {
            return list
                    .OrderBy(x => Guid.NewGuid())
                    .FirstOrDefault();
        }

        public static bool GetRandomBoolean()
        {
            var val = Random.Next(100);
            return val < 20;
        }

        public static string GetRandomString(bool allowEmpty)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var length = GetRandomInt(8, false);

            if (!allowEmpty && length == 0)
                length++;

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray());
        }

        public static int GetRandomInt(int max, bool allowNegative)
        {
            var val = Random.Next(0, max);

            if (GetRandomBoolean() && allowNegative)
            {
                val *= -1;
            }

            return val;
        }

        public static string GetRandomCommentOrWhitespace()
        {
            var blocks = GetRandomInt(5, false);
            var result = new StringBuilder();

            for (var i = 0; i < blocks; i++)
            {
                var type = GetRandomInt(3, false);
                switch (type)
                {
                    case 0:
                        result.Append("          ".Substring(0, GetRandomInt(4, false)));
                        break;
                    case 1:
                        result.Append("\n\n\n\n\n\n\n\n\n".Substring(0, GetRandomInt(4, false)));
                        break;
                    case 2:
                        result.Append(String.Format("//{0}\n", GetRandomString(true)));
                        break;
                    case 3:
                        result.Append(String.Format("/*{0}\n{1}*/", GetRandomString(true), GetRandomString(true)));
                        break;
                }
            }

            return result.ToString() == string.Empty ? " " : result.ToString();
        }

        public static T GetRandomEnumValue<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception(String.Format("Type argument {0} should be a enumeration.", typeof(T)));
            }

            var values = Enum.GetValues(typeof(T));

            return (T)values.GetValue(Random.Next(1, values.Length));
        }

        public static IExpression GetRandomObject(ExpressionTypes type, int depth, int maxDepth, bool calculateTypes)
        {

            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException("depth", "depth cannot be negative.");
            }

            if (maxDepth < 0)
            {
                throw new ArgumentOutOfRangeException("maxDepth", "maxDepth cannot be negative.");
            }

            switch (type)
            {
                case ExpressionTypes.Term:
                {
                    return GetRandomObject(GetRandomListItem<ExpressionTypes>(Terminals), depth, maxDepth, calculateTypes);
                }
                case ExpressionTypes.Boolean:
                {
                    return new Term()
                    {
                        Value = GetRandomBoolean().ToString(),
                        ExpressionType = type
                    };
                }
                case ExpressionTypes.Integer:
                {
                    return new Term()
                    {
                        Value = GetRandomInt(1000, true).ToString(),
                        ExpressionType = type
                    };
                }
                case ExpressionTypes.String:
                {
                    return new Term()
                    {
                        Value = GetRandomString(true),
                        ExpressionType = type
                    };
                }
                case ExpressionTypes.SystemObject:
                {
                    return new Term()
                    {
                        ObjectType = GetRandomString(false),
                        Value = GetRandomString(false),
                        ExpressionType = ExpressionTypes.SystemObject
                    };
                }
                case ExpressionTypes.UnaryRelation:
                {
                    ExpressionTypes newType;
                    UnaryOperators op;

                    if (calculateTypes)
                    {
                        newType = GetRandomListItem<ExpressionTypes>(new List<ExpressionTypes>() {ExpressionTypes.SystemObject, ExpressionTypes.BinaryRelation, ExpressionTypes.UnaryRelation} );
                        op = GetRandomListItem<UnaryOperators>(Evaluator.OperatorsAndTypesInUnaryRelation[newType]);
                    }
                    else
                    {
                        newType =
                            depth >= maxDepth ? GetRandomListItem<ExpressionTypes>(Terminals) : GetRandomListItem<ExpressionTypes>(NonTerminals);
                        op = GetRandomEnumValue<UnaryOperators>();
                    }

                    return new UnaryRelation()
                    {
                            Expression = GetRandomObject(newType, depth + 1, maxDepth, calculateTypes),
                            Operator = op,
                            ExpressionType = ExpressionTypes.UnaryRelation
                    };
                }
                case ExpressionTypes.BinaryRelation:
                {
                    var typeLeft = calculateTypes ? GetRandomListItem<ExpressionTypes>(new List<ExpressionTypes>() { ExpressionTypes.Boolean, ExpressionTypes.SystemObject }) : GetRandomListItem<ExpressionTypes>(Terminals);
                    ExpressionTypes typeRight;
                    BinaryOperators op;
                    
                    if (calculateTypes)
                    {
                        typeRight =
                            GetRandomListItem<ExpressionTypes>(Evaluator.TypesInBinaryRelation[typeLeft]);

                        op = GetRandomListItem<BinaryOperators>(Evaluator.OperatorsAndTypesInBinaryRelation[typeRight].Intersect(Evaluator.OperatorsAndTypesInBinaryRelation[typeLeft]).ToList());
                    }
                    else
                    {
                        typeRight =
                            depth >= maxDepth ? GetRandomListItem<ExpressionTypes>(Terminals) : GetRandomListItem<ExpressionTypes>(NonTerminals);
                        op = GetRandomEnumValue<BinaryOperators>();
                    }

                    return new BinaryRelation()
                    {
                        Left = GetRandomObject(typeLeft, depth + 1, maxDepth, calculateTypes),
                        Operator = op,
                        Right = GetRandomObject(typeRight, depth + 1, maxDepth, calculateTypes),
                        ExpressionType = ExpressionTypes.BinaryRelation
                    };
                }
                default:
                {
                    return new Term()
                    {
                        Value = GetRandomString(false),
                        ExpressionType = type
                    };
                }
            }
        }

        public static string ExpressionToString(object obj, bool useParenthesis)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var par = useParenthesis && GetRandomBoolean();
            var result = new StringBuilder();

            if (obj.GetType() == typeof(Term))
            {
                var typedobj = obj as Term;

                if (par)
                {
                    result.Append('(');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                if (typedobj.ExpressionType == ExpressionTypes.SystemObject)
                {
                    result.AppendFormat("{0}::{1}", typedobj.ObjectType, typedobj.Value);
                }
                else
                {
                    if (typedobj.ExpressionType == ExpressionTypes.String)
                    {
                        result.AppendFormat("\"{0}\"", typedobj.Value);
                    }
                    else
                    {
                        result.Append(typedobj.Value);
                    }
                }

                if (par)
                {
                    result.Append(GetRandomCommentOrWhitespace());
                    result.Append(')');
                }

                return result.ToString();
            }
            if (obj.GetType() == typeof(UnaryOperators))
            {
                var op = (UnaryOperators)obj;
                switch (op)
                {
                    case UnaryOperators.None:
                        throw new Exception("UnaryOperators.None is provided.");
                    case UnaryOperators.Negation:
                        return "!";
                    case UnaryOperators.Exist:
                        return "Exist";
                    case UnaryOperators.NotExist:
                        return "NotExist";
                    default:
                        throw new Exception(String.Format("Unknown operator {0}", op));
                }
            }
            if (obj.GetType() == typeof(UnaryRelation))
            {
                var rel = obj as UnaryRelation;

                var nonTerminal = NonTerminals.Contains(rel.Expression.ExpressionType);

                if (par)
                {
                    result.Append('(');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                result.Append(ExpressionToString(rel.Operator, true));
                result.Append(" ");

                if (nonTerminal)
                {
                    result.Append('(');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                result.Append(ExpressionToString(rel.Expression, true));

                if (nonTerminal)
                {
                    result.Append(GetRandomCommentOrWhitespace());
                    result.Append(')');
                }

                if (par)
                {
                    result.Append(GetRandomCommentOrWhitespace());
                    result.Append(')');
                }

                return result.ToString();
            }
            if (obj.GetType() == typeof(BinaryOperators))
            {
                var op = (BinaryOperators)obj;
                switch (op)
                {
                    case BinaryOperators.None:
                        throw new Exception("BinaryOperators.None is provided.");
                    case BinaryOperators.Conjunction:
                        return "And";
                    case BinaryOperators.Disjunction:
                        return "Or";
                    case BinaryOperators.Equality:
                        return "=";
                    case BinaryOperators.GreaterOrEqual:
                        return ">=";
                    case BinaryOperators.GreaterThan:
                        return ">";
                    case BinaryOperators.Implication:
                        return "=>";
                    case BinaryOperators.Inequality:
                        return "!=";
                    case BinaryOperators.LessOrEqual:
                        return "<=";
                    case BinaryOperators.LessThan:
                        return "<";
                    default:
                        throw new Exception(String.Format("Unknown operator {0}", op));
                }
            }
            if (obj.GetType() == typeof(BinaryRelation))
            {
                var rel = obj as BinaryRelation;

                if (par)
                {
                    result.Append('(');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                result.Append(ExpressionToString(rel.Left, true));
                result.Append(GetRandomCommentOrWhitespace());
                result.Append(ExpressionToString(rel.Operator, true));
                result.Append(GetRandomCommentOrWhitespace());
                result.Append(ExpressionToString(rel.Right, true));

                if (par)
                {
                    result.Append(GetRandomCommentOrWhitespace());
                    result.Append(')');
                }

                return result.ToString();
            }

            throw new ArgumentOutOfRangeException("obj", String.Format("Unknown object type {0}.", obj.GetType()));
        }

        public static IExpression GetRandomAst(int maxDepth, bool calculateTypes)
        {
            return GetRandomObject(ExpressionTypes.BinaryRelation, 0, maxDepth, calculateTypes);
        }

        public static IExpression ParseExtended(Parser<IExpression> parser, string input)
        {
            try
            {
                return parser.Parse(input);
            }
            catch (ParseException ex)
            {
                throw new ParseException(String.Format("Exception: {0} ; Input {1}", ex.Message, input), ex);
            }
        }

        public static void CheckTypesExtended(IExpression expr, string input)
        {
            try
            {
                Evaluator.CheckTypes(expr);
            }
            catch (TypeCheckerException ex)
            {
                throw new TypeCheckerException(String.Format("Exception: {0} ; Input {1}", ex.Message, input), ex, expr);
            }
        }

        public static void AreEqualExtended(object expected, object actual, string input)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Exception: {0} ; Input {1}", ex.Message, input), ex);
            }
        }

        public static List<IExpression> GetFlatTree(IExpression ast)
        {
            if (ast == null)
            {
                throw new ArgumentNullException("ast", "Ast element should be not null.");
            }

            return ast
                    .Children
                    .SelectMany(GetFlatTree)
                    .Concat(new [] { ast })
                    .ToList();
        }
    }
}
