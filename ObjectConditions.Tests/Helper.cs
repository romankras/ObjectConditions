using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sprache;

namespace ObjectConditions.Tests
{
    public class Helper
    {
        private static readonly Random Random = new Random();

        public static List<Type> GetAstObjectTypes(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Type cannot be null");
            }

            if (!type.IsInterface)
            {
                throw new ArgumentOutOfRangeException(nameof(type), "Type should be an interface.");
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface)
                .ToList();
        }

        public static Type GetRandomAstType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "Type cannot be null");
            }

            if (!type.IsInterface)
            {
                throw new ArgumentOutOfRangeException(nameof(type), "Type should be an interface.");
            }

            var types = GetAstObjectTypes(type);
            var index = Random.Next(types.Count);

            return types[index];
        }

        public static bool GetRandomBoolean()
        {
            var val = Random.Next(100);
            return val < 20;
        }

        public static string GetRandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray());
        }

        public static int GetRandomInt(int max)
        {
            return Random.Next(0, max);
        }

        public static string GetRandomCommentOrWhitespace()
        {
            var blocks = GetRandomInt(5);
            var result = new StringBuilder();

            for (var i = 0; i < blocks; i++)
            {
                var type = GetRandomInt(3);
                switch (type)
                {
                    case 0:
                        result.Append("          ".Substring(0, GetRandomInt(4)));
                        break;
                    case 1:
                        result.Append("\n\n\n\n\n\n\n\n\n".Substring(0, GetRandomInt(4)));
                        break;
                    case 2:
                        result.Append(String.Format("//{0}\n", GetRandomString()));
                        break;
                    case 3:
                        result.Append(String.Format("/*{0}\n{1}*/", GetRandomString(), GetRandomString()));
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

        public static IExpression GetRandomObject(Type type, int depth, int maxDepth)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type), "type cannot be null");
            }

            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(depth), "depth cannot be negative.");
            }

            if (maxDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDepth), "maxDepth cannot be negative.");
            }

            if (type == typeof(ObjectValue))
            {
                return new ObjectValue()
                {
                    Value = GetRandomString()
                };
            }
            if (type == typeof(TypedObject))
            {
                return new TypedObject()
                {
                    IsNegated = GetRandomBoolean(),
                    Name = GetRandomString(),
                    ObjectType = GetRandomString()
                };
            }
            if (type == typeof(BinaryRelation))
            {
                var typeLeft = GetRandomAstType(typeof(ITerminalExpression));
                var typeRight = 
                    depth >= maxDepth ? GetRandomAstType(typeof(ITerminalExpression)) : GetRandomAstType(typeof(IExpression));

                return new BinaryRelation()
                {
                    IsNegated = GetRandomBoolean(),
                    Left = GetRandomObject(typeLeft, depth + 1, maxDepth),
                    Operator = GetRandomEnumValue<BinaryOperators>(),
                    Right = GetRandomObject(typeRight, depth + 1, maxDepth)
                };
            }

            throw new ArgumentOutOfRangeException(nameof(type), String.Format("Unknown object type {0}.", type));
        }

        public static string ExpressionToString(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var par = GetRandomBoolean();
            var result = new StringBuilder();

            if (obj.GetType() == typeof(ObjectValue))
            {
                var objvalue = obj as ObjectValue;

                if (objvalue.IsNegated)
                {
                    result.Append('!');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                if (par)
                {
                    result.Append('(');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                result.AppendFormat("{0}", objvalue.Value);

                if (par)
                {
                    result.Append(GetRandomCommentOrWhitespace());
                    result.Append(')');
                }

                return result.ToString();
            }
            if (obj.GetType() == typeof(TypedObject))
            {
                var typedobj = obj as TypedObject;

                if (typedobj.IsNegated)
                {
                    result.Append('!');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                if (par)
                {
                    result.Append('(');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                result.AppendFormat("{0}::{1}", typedobj.ObjectType, typedobj.Name);

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

                if (rel.IsNegated)
                {
                    result.Append('!');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                if (par || rel.IsNegated)
                {
                    result.Append('(');
                    result.Append(GetRandomCommentOrWhitespace());
                }

                result.Append(ExpressionToString(rel.Left));
                result.Append(GetRandomCommentOrWhitespace());
                result.Append(ExpressionToString(rel.Operator));
                result.Append(GetRandomCommentOrWhitespace());
                result.Append(ExpressionToString(rel.Right));

                if (par || rel.IsNegated)
                {
                    result.Append(GetRandomCommentOrWhitespace());
                    result.Append(')');
                }

                return result.ToString();
            }

            throw new ArgumentOutOfRangeException(nameof(obj), String.Format("Unknown object type {0}.", obj.GetType()));
        }

        public static IExpression GetRandomAst(int maxDepth)
        {
            return GetRandomObject(typeof(BinaryRelation), 0, maxDepth);
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
                throw new ArgumentNullException(nameof(ast), "Ast element should be not null.");
            }

            return ast
                    .Children
                    .SelectMany(GetFlatTree)
                    .Concat(new List<IExpression>() { ast })
                    .ToList();
        }
    }
}
