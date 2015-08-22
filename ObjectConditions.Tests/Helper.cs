using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectConditions;
using Sprache;

namespace ObjectConditions.Tests
{
    public class Helper
    {

        // Char primitives
        private static readonly char OpenParenthesis = '(';
        private static readonly char CloseParenthesis = ')';
        private static readonly char ExclamationMark = '!';
        private static readonly char Whitespace = ' ';

        private static readonly Random Random = new Random();

        /// <summary>
        /// Function that returns all types which implement given interface in current app domain.
        /// </summary>
        /// <param name="type">Type of the interface.</param>
        /// <returns>List of types.</returns>
        public static List<Type> GetAstObjectTypes(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "Type cannot be null");
            }

            if (!type.IsInterface)
            {
                throw new ArgumentOutOfRangeException("type", "Type should be an interface.");
            }

            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface)
                .ToList();
        }

        /// <summary>
        /// Function that returns random type that implements given interface.
        /// </summary>
        /// <param name="type">The interface.</param>
        /// <returns>Random type.</returns>
        public static Type GetRandomAstType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "Type cannot be null");
            }

            if (!type.IsInterface)
            {
                throw new ArgumentOutOfRangeException("type", "Type should be an interface.");
            }

            var types = GetAstObjectTypes(type);
            var index = Random.Next(types.Count);

            return types[index];
        }

        /// <summary>
        /// Function that returns random boolean value.
        /// </summary>
        /// <returns>Random boolean value.</returns>
        public static bool GetRandomBoolean()
        {
            var val = Random.Next(100);
            return val < 20 ? true : false;
        }

        /// <summary>
        /// Function that returns random string value.
        /// </summary>
        /// <param name="isConfig">Boolean value that indicates whether the string would be generated for ConfigValue or not.</param>
        /// <returns>Random string value.</returns>
        public static string GetRandomString(bool isConfig)
        {
            var chars = isConfig ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_."
                                 : "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_. ";

            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray());
        }

        /// <summary>
        /// Function that returns random integer value.
        /// </summary>
        /// <param name="max">Maximum allowed value.</param>
        /// <returns>Random integer value.</returns>
        public static int GetRandomInt(int max)
        {
            return Random.Next(0, max);
        }

        /// <summary>
        /// Function that returns random comment block.
        /// </summary>
        /// <returns>Random comment block..</returns>
        public static string GetRandomCommentOrWhitespace()
        {
            var blocks = GetRandomInt(5);
            var result = new StringBuilder();

            for (int i = 0; i < blocks; i++)
            {
                var type = GetRandomInt(3);
                switch (type)
                {
                    // random whitespaces
                    case 0:
                        result.Append("          ".Substring(0, GetRandomInt(4)));
                        break;
                    // random linebreaks
                    case 1:
                        result.Append("\n\n\n\n\n\n\n\n\n".Substring(0, GetRandomInt(4)));
                        break;
                    // random single-line comment
                    case 2:
                        result.Append(String.Format("//{0}\n", GetRandomString(false)));
                        break;
                    // random multi-line comment
                    case 3:
                        result.Append(String.Format("/*{0}\n{1}*/", GetRandomString(false), GetRandomString(false)));
                        break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Function that returns random value from the given enumeration.
        /// </summary>
        /// <typeparam name="T">The enumeration.</typeparam>
        /// <returns>Random value.</returns>
        public static T GetRandomEnumValue<T>()
        {
            // enum type constraints are not supported yet
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentOutOfRangeException("Type argument", String.Format("Type argument {0} should be a enumeration.", typeof(T)));
            }

            var values = Enum.GetValues(typeof(T));

            return (T)values.GetValue(Random.Next(1, values.Length));
        }

        /// <summary>
        /// Function that generates node of abstract syntax tree with random data.
        /// </summary>
        /// <param name="type">Node type.</param>
        /// <param name="level">Current tree depth.</param>
        /// <param name="maxLevel">Maximum allowed depth.</param>
        /// <returns>Abstract syntax tree.</returns>
        public static IAstObject GetRandomAstObject(Type type, int depth, int maxDepth)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "Type cannot be null");
            }

            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException("level", "Level cannot be negative.");
            }

            if (maxDepth < 0)
            {
                throw new ArgumentOutOfRangeException("maxLevel", "Level cannot be negative.");
            }

            if (type == typeof(ConfigValue))
            {
                return new ConfigValue()
                {
                    Value = GetRandomString(true)
                };
            }
            else if (type == typeof(BooleanValue))
            {
                return new BooleanValue()
                {
                    Value = GetRandomBoolean()
                };
            }
            else if (type == typeof(NumericValue))
            {
                return new NumericValue()
                {
                    Value = GetRandomInt(100)
                };
            }
            else if (type == typeof(StringValue))
            {
                return new StringValue()
                {
                    Value = GetRandomString(false)
                };
            }
            else if (type == typeof(ConfigBinaryRelation))
            {
                return new ConfigBinaryRelation()
                {
                    IsNegated = GetRandomBoolean(),
                    LeftOperand = (ConfigValue)GetRandomAstObject(typeof(ConfigValue), depth + 1, maxDepth),
                    Operator = GetRandomEnumValue<ConfigBinaryOperators>(),
                    RightOperand = (ConfigValue)GetRandomAstObject(typeof(ConfigValue), depth + 1, maxDepth)
                };
            }
            else if (type == typeof(StringBinaryRelation))
            {
                var typeLeft = GetRandomAstType(typeof(IStringExpression));
                var typeRight = GetRandomAstType(typeof(IStringExpression));

                var rel = new StringBinaryRelation()
                {
                    IsNegated = GetRandomBoolean(),
                    LeftOperand = (IStringExpression)GetRandomAstObject(typeLeft, depth + 1, maxDepth),
                    Operator = GetRandomEnumValue<StringBinaryOperators>(),
                    RightOperand = (IStringExpression)GetRandomAstObject(typeRight, depth + 1, maxDepth)
                };

                if (rel.LeftOperand.GetType() == typeof(ConfigValue) && rel.RightOperand.GetType() == typeof(ConfigValue))
                {
                    return GetRandomAstObject(typeof(StringBinaryRelation), depth, maxDepth);
                }
                else
                {
                    return rel;
                }
            }
            else if (type == typeof(NumericBinaryRelation))
            {
                var typeLeft = GetRandomAstType(typeof(INumericExpression));
                var typeRight = GetRandomAstType(typeof(INumericExpression));

                var rel = new NumericBinaryRelation()
                {
                    IsNegated = GetRandomBoolean(),
                    LeftOperand = (INumericExpression)GetRandomAstObject(typeLeft, depth + 1, maxDepth),
                    Operator = GetRandomEnumValue<NumericBinaryOperators>(),
                    RightOperand = (INumericExpression)GetRandomAstObject(typeRight, depth + 1, maxDepth)
                };

                if (rel.LeftOperand.GetType() == typeof(ConfigValue) && rel.RightOperand.GetType() == typeof(ConfigValue))
                {
                    return GetRandomAstObject(typeof(NumericBinaryRelation), depth, maxDepth);
                }
                else
                {
                    return rel;
                }
            }
            else if (type == typeof(BooleanBinaryRelation))
            {
                var typeLeft = GetRandomAstType(typeof(IBooleanExpression));
                var typeRight = GetRandomAstType(typeof(IBooleanExpression));

                var rel = new BooleanBinaryRelation()
                {
                    IsNegated = GetRandomBoolean(),
                    LeftOperand = (IBooleanExpression)GetRandomAstObject(typeLeft, depth + 1, maxDepth),
                    Operator = GetRandomEnumValue<BooleanBinaryOperators>(),
                    RightOperand = (IBooleanExpression)GetRandomAstObject(typeRight, depth + 1, maxDepth)
                };

                if (rel.LeftOperand.GetType() == typeof(ConfigValue) && rel.RightOperand.GetType() == typeof(ConfigValue))
                {
                    return GetRandomAstObject(typeof(BooleanBinaryRelation), depth, maxDepth);
                }
                else
                {
                    return rel;
                }
            }
            else if (type == typeof(LogicalBinaryRelation))
            {
                // since our grammar is right-recursive we allow only teminal expressions in the left hand
                var typeLeft = GetRandomAstType(typeof(ITerminalExpression));
                // random terminal if maximum recursion level is reached
                var typeRight = depth >= maxDepth ? GetRandomAstType(typeof(ITerminalExpression)) : GetRandomAstType(typeof(ILogicalExpression));

                return new LogicalBinaryRelation()
                {
                    IsNegated = GetRandomBoolean(),
                    LeftOperand = (ILogicalExpression)GetRandomAstObject(typeLeft, depth + 1, maxDepth),
                    Operator = GetRandomEnumValue<LogicalBinaryOperators>(),
                    RightOperand = (ILogicalExpression)GetRandomAstObject(typeRight, depth + 1, maxDepth)
                };
            }

            throw new ArgumentOutOfRangeException("type", String.Format("Unknown object type {0}.", type));
        }

        /// <summary>
        /// Function that converts given object to string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>String representation.</returns>
        public static string AstObjectToString(object obj)
        {
            var par = GetRandomBoolean();
            var ws1 = GetRandomCommentOrWhitespace();
            var ws2 = GetRandomCommentOrWhitespace();
            var ws3 = GetRandomCommentOrWhitespace();
            var ws4 = GetRandomCommentOrWhitespace();
            var upper = GetRandomBoolean();
            var result = new StringBuilder();

            if (obj.GetType() == typeof(ConfigValue))
            {
                var confvalue = obj as ConfigValue;
                result.AppendFormat("ConfigValue::{0}", confvalue.Value);
                return result.ToString();
            }
            else if (obj.GetType() == typeof(BooleanValue))
            {
                var boolvalue = obj as BooleanValue;
                var temp = boolvalue.Value.ToString();
                result.Append(upper ? temp[0].ToString().ToUpper() : temp[0].ToString().ToLower());
                result.Append(temp.Substring(1, temp.Length - 1));
                return result.ToString();
            }
            else if (obj.GetType() == typeof(NumericValue))
            {
                var intvalue = obj as NumericValue;
                result.AppendFormat("{0}", intvalue.Value);
                return result.ToString();
            }
            else if (obj.GetType() == typeof(StringValue))
            {
                var strvalue = obj as StringValue;
                result.AppendFormat("'{0}'", strvalue.Value);
                return result.ToString();
            }
            else if (obj.GetType() == typeof(ConfigBinaryRelation))
            {
                var confrel = obj as ConfigBinaryRelation;
                if (par || confrel.IsNegated)
                {
                    if (confrel.IsNegated) result.Append(ExclamationMark);
                    result.Append(OpenParenthesis);
                    result.Append(ws1);
                }

                result.Append(AstObjectToString(confrel.LeftOperand));
                result.Append(ws2);
                result.Append(AstObjectToString(confrel.Operator));
                result.Append(ws3);
                result.Append(AstObjectToString(confrel.RightOperand));

                if (par || confrel.IsNegated)
                {
                    result.Append(ws4);
                    result.Append(CloseParenthesis);
                }

                return result.ToString();
            }
            else if (obj.GetType() == typeof(StringBinaryRelation))
            {
                var strrel = obj as StringBinaryRelation;
                if (par || strrel.IsNegated)
                {
                    if (strrel.IsNegated) result.Append(ExclamationMark);
                    result.Append(OpenParenthesis);
                    result.Append(ws1);
                }

                result.Append(AstObjectToString(strrel.LeftOperand));
                result.Append(ws2);
                result.Append(AstObjectToString(strrel.Operator));
                result.Append(ws3);
                result.Append(AstObjectToString(strrel.RightOperand));

                if (par || strrel.IsNegated)
                {
                    result.Append(ws4);
                    result.Append(CloseParenthesis);
                }

                return result.ToString();
            }
            else if (obj.GetType() == typeof(NumericBinaryRelation))
            {
                var intrel = obj as NumericBinaryRelation;
                if (par || intrel.IsNegated)
                {
                    if (intrel.IsNegated) result.Append(ExclamationMark);
                    result.Append(OpenParenthesis);
                    result.Append(ws1);
                }

                result.Append(AstObjectToString(intrel.LeftOperand));
                result.Append(ws2);
                result.Append(AstObjectToString(intrel.Operator));
                result.Append(ws3);
                result.Append(AstObjectToString(intrel.RightOperand));

                if (par || intrel.IsNegated)
                {
                    result.Append(ws4);
                    result.Append(CloseParenthesis);
                }

                return result.ToString();
            }
            else if (obj.GetType() == typeof(BooleanBinaryRelation))
            {
                var boolrel = obj as BooleanBinaryRelation;
                if (par || boolrel.IsNegated)
                {
                    if (boolrel.IsNegated) result.Append(ExclamationMark);
                    result.Append(OpenParenthesis);
                    result.Append(ws1);
                }

                result.Append(AstObjectToString(boolrel.LeftOperand));
                result.Append(ws2);
                result.Append(AstObjectToString(boolrel.Operator));
                result.Append(ws3);
                result.Append(AstObjectToString(boolrel.RightOperand));

                if (par || boolrel.IsNegated)
                {
                    result.Append(ws4);
                    result.Append(CloseParenthesis);
                }

                return result.ToString();
            }
            else if (obj.GetType() == typeof(LogicalBinaryRelation))
            {
                var logrel = obj as LogicalBinaryRelation;
                if (par || logrel.IsNegated)
                {
                    if (logrel.IsNegated) result.Append(ExclamationMark);
                    result.Append(OpenParenthesis);
                    result.Append(ws1);
                }

                result.Append(AstObjectToString(logrel.LeftOperand));
                result.Append(ws2);
                result.Append(Whitespace);
                result.Append(AstObjectToString(logrel.Operator));
                result.Append(ws3);
                result.Append(Whitespace);
                result.Append(AstObjectToString(logrel.RightOperand));

                if (par || logrel.IsNegated)
                {
                    result.Append(ws4);
                    result.Append(CloseParenthesis);
                }

                return result.ToString();
            }
            else if (obj.GetType() == typeof(LogicalBinaryOperators))
            {
                var op = (LogicalBinaryOperators)obj;
                switch (op)
                {
                    case LogicalBinaryOperators.Conjunction:
                        result.Append("And");
                        return result.ToString();
                    case LogicalBinaryOperators.Disjunction:
                        result.Append("Or");
                        return result.ToString();
                    case LogicalBinaryOperators.Implication:
                        result.Append("=>");
                        return result.ToString();
                    default:
                        throw new ArgumentOutOfRangeException(
                            "obj", String.Format("Unknown enum value or LogicalBinaryOperators.None. Value: {0}", obj));
                }
            }
            else if (obj.GetType() == typeof(BooleanBinaryOperators))
            {
                var op = (BooleanBinaryOperators)obj;
                switch (op)
                {
                    case BooleanBinaryOperators.Equality:
                        result.Append("=");
                        return result.ToString();
                    case BooleanBinaryOperators.Inequality:
                        result.Append("!=");
                        return result.ToString();
                    default:
                        throw new ArgumentOutOfRangeException(
                            "obj", String.Format("Unknown enum value or BooleanBinaryOperators.None. Value: {0}", obj));
                }
            }
            else if (obj.GetType() == typeof(StringBinaryOperators))
            {
                var op = (StringBinaryOperators)obj;
                switch (op)
                {
                    case StringBinaryOperators.Equality:
                        result.Append("=");
                        return result.ToString();
                    case StringBinaryOperators.Inequality:
                        result.Append("!=");
                        return result.ToString();
                    default:
                        throw new ArgumentOutOfRangeException(
                            "obj", String.Format("Unknown enum value or StringBinaryOperators.None. Value: {0}", obj));
                }
            }
            else if (obj.GetType() == typeof(NumericBinaryOperators))
            {
                var op = (NumericBinaryOperators)obj;
                switch (op)
                {
                    case NumericBinaryOperators.Equality:
                        result.Append("=");
                        return result.ToString();
                    case NumericBinaryOperators.Inequality:
                        result.Append("!=");
                        return result.ToString();
                    case NumericBinaryOperators.GreaterOrEqual:
                        result.Append(">=");
                        return result.ToString();
                    case NumericBinaryOperators.GreaterThan:
                        result.Append(">");
                        return result.ToString();
                    case NumericBinaryOperators.LessOrEqual:
                        result.Append("<=");
                        return result.ToString();
                    case NumericBinaryOperators.LessThan:
                        result.Append("<");
                        return result.ToString();
                    default:
                        throw new ArgumentOutOfRangeException(
                            "obj", String.Format("Unknown enum value or NumericBinaryOperators.None. Value: {0}", obj));
                }
            }
            else if (obj.GetType() == typeof(ConfigBinaryOperators))
            {
                var op = (ConfigBinaryOperators)obj;
                switch (op)
                {
                    case ConfigBinaryOperators.Equality:
                        result.Append("=");
                        return result.ToString();
                    case ConfigBinaryOperators.Inequality:
                        result.Append("!=");
                        return result.ToString();
                    case ConfigBinaryOperators.GreaterOrEqual:
                        result.Append(">=");
                        return result.ToString();
                    case ConfigBinaryOperators.GreaterThan:
                        result.Append(">");
                        return result.ToString();
                    case ConfigBinaryOperators.LessOrEqual:
                        result.Append("<=");
                        return result.ToString();
                    case ConfigBinaryOperators.LessThan:
                        result.Append("<");
                        return result.ToString();
                    default:
                        throw new ArgumentOutOfRangeException(
                            "obj", String.Format("Unknown enum value or ConfigBinaryOperators.None. Value: {0}", obj));
                }
            }

            throw new ArgumentOutOfRangeException("obj", String.Format("Unknown object type {0}.", obj.GetType()));
        }

        /// <summary>
        /// Function that generates random abstract syntax tree.
        /// </summary>
        /// <param name="maxDepth">Maximum allowed depth of the tree.</param>
        /// <returns>Abstract syntax tree.</returns>
        public static ILogicalExpression GetRandomAst(int maxDepth)
        {
            var root = GetRandomAstType(typeof(ILogicalExpression));
            return (ILogicalExpression)GetRandomAstObject(root, 0, maxDepth);
        }

        /// <summary>
        /// Wrapper for standard Parse() function that attaches text to body of error message.
        /// </summary>
        /// <param name="parser">Parser.</param>
        /// <param name="input">Text to include.</param>
        /// <returns>Output of the parser.</returns>
        public static IAstObject ParseExtended(Parser<IAstObject> parser, string input)
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

        /// <summary>
        /// Wrapper for standard Assert.AreEqual() function that attaches text to body of error message.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <param name="input">Text to include.</param>
        public static void AreEqualExtended(object expected, object actual, string input)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch (AssertFailedException ex)
            {
                throw new AssertFailedException(String.Format("Exception: {0} ; Input {1}", ex.Message, input), ex);
            }
        }

        /// <summary>
        /// Function that creates text file in gnuplot format with data that has been extracted during performance test.
        /// If the file already exist, overwrites it.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <param name="data">Data.</param>
        public static void CreateDataFile(string filename, List<Tuple<int, double>> data)
        {
            using(var stream = new StreamWriter(filename, false))
            {
                foreach (var measurement in data)
                {
                    stream.WriteLine(String.Format("{0}   {1}", measurement.Item1, measurement.Item2));
                }
            }
        }
    }
}
