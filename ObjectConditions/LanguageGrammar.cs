using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Sprache;

namespace ObjectConditions
{
    /// <summary>
    /// Grammar class that stores parsing rules as delegates.
    /// Since Sprache doesn't support left recursion, all the rules are right-resursive.
    /// </summary>
    public class LanguageGrammar
    {
        // Aliases for frequently used characters
        public static readonly Parser<char> Apostrophe = Parse.Char('\'');
        public static readonly Parser<char> CloseParenthesis = Parse.Char(')');
        public static readonly Parser<char> ExclamationMark = Parse.Char('!');
        public static readonly Parser<char> OpenParenthesis = Parse.Char('(');

        /// <summary>
        /// Comment parser from Sprache. It supports C-style multi- and single-line comments.
        /// </summary>
        public static CommentParser Comment = new CommentParser();

        /// <summary>
        /// WhitespacesOrComments =
        ///     ( [{" "}] | {Comment} | [{"\n"}] )
        /// </summary>
        public static Parser<IEnumerable<string>> WhitespacesOrComments =
            Parse.WhiteSpace.Many().Text()
                .Or(Comment.AnyComment)
                .Or(Parse.Char('\n').Many().Text())
            .Many();

        /// <summary>
        /// NumericValue =
        ///     { Digit }
        /// </summary>
        public static readonly Parser<NumericValue> NumericValue =
            from raw in Parse.Digit.AtLeastOnce().Text()
            select new NumericValue
            {
                Value = int.Parse(raw)
            };

        /// <summary>
        /// StringValue =
        ///     "'" , { ( LetterOrDigit | "_" | "." | "-" | " " ) }, "'"
        /// </summary>
        public static readonly Parser<StringValue> StringValue =
            from start in Apostrophe.Once()
            from val in Parse.LetterOrDigit.Or(Parse.Chars("_-. ")).AtLeastOnce().Text()
            from end in Apostrophe.Once()
            select new StringValue
            {
                Value = val
            };

        /// <summary>
        /// BooleanValue =
        ///     "True" | "False" | "true" | "false"
        /// </summary>
        public static readonly Parser<BooleanValue> BooleanValue =
            from raw in Parse.String("True")
                            .Or(Parse.String("False"))
                            .Or(Parse.String("false"))
                            .Or(Parse.String("true"))
                            .Text().Token()
            select new BooleanValue
            {
                Value = bool.Parse(raw)
            };

        /// <summary>
        /// ConfigValue =
        ///     "ConfigValue::" , { ( LetterOrDigit | "_" | "." | "-" ) }
        /// </summary>
        public static readonly Parser<ConfigValue> ConfigValue =
            from tag in Parse.String("ConfigValue::").Once()
            from name in Parse.LetterOrDigit.Or(Parse.Chars("_-.")).AtLeastOnce().Text()
            select new ConfigValue
            {
                Value = name
            };

        /// <summary>
        /// BooleanBinaryOperator =
        ///     ( "=" | "!=" )
        /// </summary>
        public static Parser<BooleanBinaryOperators> BooleanBinaryOperator =
            Parse.Char('=').Return(BooleanBinaryOperators.Equality)
                .Or(Parse.String("!=").Return(BooleanBinaryOperators.Inequality));

        /// <summary>
        /// LogicalBinaryOperator =
        ///     ( "=>" | "And" | "Or" )
        /// </summary>
        public static Parser<LogicalBinaryOperators> LogicalBinaryOperator =
            Parse.String("=>").Return(LogicalBinaryOperators.Implication)
                .Or(Parse.String("And").Return(LogicalBinaryOperators.Conjunction))
                .Or(Parse.String("Or").Return(LogicalBinaryOperators.Disjunction));

        /// <summary>
        /// StringBinaryOperator =
        ///     ( "=" | "!=" )
        /// </summary>
        public static Parser<StringBinaryOperators> StringBinaryOperator =
            Parse.Char('=').Return(StringBinaryOperators.Equality)
                .Or(Parse.String("!=").Return(StringBinaryOperators.Inequality));

        /// <summary>
        /// NumericBinaryOperator =
        ///     ( "=" | "!=" | ">=" | ">" | "<=" | "<" )
        /// </summary>
        public static Parser<NumericBinaryOperators> NumericBinaryOperator =
            Parse.Char('=').Return(NumericBinaryOperators.Equality)
                .Or(Parse.String("!=").Return(NumericBinaryOperators.Inequality))
                .Or(Parse.String(">=").Return(NumericBinaryOperators.GreaterOrEqual))
                .Or(Parse.String(">").Return(NumericBinaryOperators.GreaterThan))
                .Or(Parse.String("<=").Return(NumericBinaryOperators.LessOrEqual))
                .Or(Parse.String("<").Return(NumericBinaryOperators.LessThan));

        /// <summary>
        /// ConfigBinaryOperator =
        ///     ( "=" | "!=" | ">=" | ">" | "<=" | "<" )
        /// </summary>
        public static Parser<ConfigBinaryOperators> ConfigBinaryOperator =
            Parse.Char('=').Return(ConfigBinaryOperators.Equality)
                .Or(Parse.String("!=").Return(ConfigBinaryOperators.Inequality))
                .Or(Parse.String(">=").Return(ConfigBinaryOperators.GreaterOrEqual))
                .Or(Parse.String(">").Return(ConfigBinaryOperators.GreaterThan))
                .Or(Parse.String("<=").Return(ConfigBinaryOperators.LessOrEqual))
                .Or(Parse.String("<").Return(ConfigBinaryOperators.LessThan));

        /// <summary>
        /// ConfigBinaryRelation =
        ///     ConfigValue,
        ///     WhitespacesOrComments,
        ///     ConfigBinaryOperator,
        ///     WhitespacesOrComments,
        ///     BooleanValue
        /// </summary>
        public static Parser<ConfigBinaryRelation> ConfigBinaryRelation =
            from left in ConfigValue
            from ws1 in WhitespacesOrComments
            from op in ConfigBinaryOperator
            from ws2 in WhitespacesOrComments
            from right in ConfigValue
            select new ConfigBinaryRelation
            {
                IsNegated = false,
                LeftOperand = left,
                Operator = op,
                RightOperand = right
            };

        /// <summary>
        /// BooleanBinaryRelation =
        ///     ( ConfigValue | BooleanValue ),
        ///     WhitespacesOrComments,
        ///     BooleanBinaryOperator,
        ///     WhitespacesOrComments,
        ///     ( ConfigValue | BooleanValue )
        /// </summary>
        public static Parser<BooleanBinaryRelation> BooleanBinaryRelation =
            from left in ConfigValue.Or<IBooleanExpression>(BooleanValue)
            from ws1 in WhitespacesOrComments
            from op in BooleanBinaryOperator
            from ws2 in WhitespacesOrComments
            from right in ConfigValue.Or<IBooleanExpression>(BooleanValue)
            select new BooleanBinaryRelation
            {
                IsNegated = false,
                LeftOperand = left,
                Operator = op,
                RightOperand = right
            };

        /// <summary>
        /// StringBinaryRelation =
        ///     ( ConfigValue | StringValue ),
        ///     WhitespacesOrComments,
        ///     StringBinaryOperator,
        ///     WhitespacesOrComments,
        ///     ( ConfigValue | StringValue )
        /// </summary>
        public static Parser<StringBinaryRelation> StringBinaryRelation =
            from left in ConfigValue.Or<IStringExpression>(StringValue)
            from ws1 in WhitespacesOrComments
            from op in StringBinaryOperator
            from ws2 in WhitespacesOrComments
            from right in ConfigValue.Or<IStringExpression>(StringValue)
            select new StringBinaryRelation
            {
                IsNegated = false,
                LeftOperand = left,
                Operator = op,
                RightOperand = right
            };

        /// <summary>
        /// NumericBinaryRelation =
        ///     ( ConfigValue | NumericValue ),
        ///     WhitespacesOrComments,
        ///     NumericBinaryOperator,
        ///     WhitespacesOrComments,
        ///     ( ConfigValue | NumericValue )
        /// </summary>
        public static Parser<NumericBinaryRelation> NumericBinaryRelation =
            from left in ConfigValue.Or<INumericExpression>(NumericValue)
            from ws1 in WhitespacesOrComments
            from op in NumericBinaryOperator
            from ws2 in WhitespacesOrComments
            from right in ConfigValue.Or<INumericExpression>(NumericValue)
            select new NumericBinaryRelation
            {
                IsNegated = false,
                LeftOperand = left,
                Operator = op,
                RightOperand = right
            };

        /// <summary>
        /// LogicalBinaryRelation =
        ///     GenericLogicalExpression,
        ///     WhitespacesOrComments,
        ///     LogicalBinaryOperator,
        ///     WhitespacesOrComments,
        ///     ( LogicalBinaryRelation | GenericLogicalExpression )
        /// </summary>
        public static Parser<LogicalBinaryRelation> LogicalBinaryRelation =
            from left in Parse.Ref(() => GenericLogicalExpression)
            from ws1 in WhitespacesOrComments
            from op in LogicalBinaryOperator
            from ws2 in WhitespacesOrComments
            from right in LogicalBinaryRelation
                          .Or(GenericLogicalExpression)
            select new LogicalBinaryRelation
            {
                IsNegated = false,
                LeftOperand = left,
                Operator = op,
                RightOperand = right
            };

        /// <summary>
        /// Function that negates given logical expression.
        /// </summary>
        /// <param name="ex">Expression.</param>
        /// <returns>Negated expression.</returns>
        private static ILogicalExpression Negate(ILogicalExpression ex)
        {
            var newex = ex;
            newex.IsNegated = !ex.IsNegated;
            return newex;
        }

        /// <summary>
        /// NegatedExpression =
        ///     "!",
        ///     "(",
        ///     WhitespacesOrComments,
        ///     ( LogicalBinaryRelation | GenericLogicalExpression ),
        ///     WhitespacesOrComments,
        ///     ")"
        /// </summary>
        public static Parser<ILogicalExpression> NegatedExpression =
            from neg in ExclamationMark.Once()
            from start in OpenParenthesis.Once()
            from ws1 in WhitespacesOrComments
            from rel in (LogicalBinaryRelation
                        .Or<ILogicalExpression>(GenericLogicalExpression))
                        .Select<ILogicalExpression, ILogicalExpression>(Negate)
            from ws2 in WhitespacesOrComments
            from end in CloseParenthesis.Once()
            select rel;

        /// <summary>
        /// GenericLogicalExpression =
        ///     ( "(", WhitespacesOrComments, LogicalBinaryRelation, WhitespacesOrComments, ")" |
        ///       "(", WhitespacesOrComments, GenericLogicalExpression, WhitespacesOrComments, ")" |
        ///       NegatedExpression |
        ///       ConfigBinaryRelation |
        ///       BooleanBinaryRelation |
        ///       NumericBinaryRelation |
        ///       StringBinaryRelation )
        /// </summary>
        public static Parser<ILogicalExpression> GenericLogicalExpression =
            (
                from start in OpenParenthesis.Once()
                from ws1 in WhitespacesOrComments
                from rel in LogicalBinaryRelation
                from ws2 in WhitespacesOrComments
                from end in CloseParenthesis.Once()
                select rel
            )
            .Or(
                from start in OpenParenthesis.Once()
                from ws1 in WhitespacesOrComments
                from rel in GenericLogicalExpression
                from ws2 in WhitespacesOrComments
                from end in CloseParenthesis.Once()
                select rel
            )
            .Or<ILogicalExpression>(NegatedExpression)
            .Or<ILogicalExpression>(ConfigBinaryRelation)
            .Or<ILogicalExpression>(BooleanBinaryRelation)
            .Or<ILogicalExpression>(NumericBinaryRelation)
            .Or<ILogicalExpression>(StringBinaryRelation);

        /// <summary>
        /// start =
        ///     WhitespacesOrComments,
        ///     ( LogicalBinaryRelation | GenericLogicalExpression),
        ///     WhitespacesOrComments
        /// </summary>
        public static Parser<ILogicalExpression> ParseAst =
            from ws1 in WhitespacesOrComments
            from expr in LogicalBinaryRelation
                         .Or<ILogicalExpression>(GenericLogicalExpression)
            from ws2 in WhitespacesOrComments
            select expr;
    }
}
