using System;
using System.Collections.Generic;
using Sprache;

namespace ObjectConditions
{
    public class LanguageGrammar
    {
        private static readonly CommentParser Comment = new CommentParser();

        public static readonly Parser<IEnumerable<string>> WhitespacesOrComments =
                Parse.WhiteSpace.Many().Text()
                    .Or(Comment.AnyComment)
                    .Or(Parse.Char('\n').Many().Text())
                    .Many();

        public static readonly Parser<BinaryOperators> BinaryOperator =
                Parse.String("=>").Return(BinaryOperators.Implication)
                    .Or(Parse.String("And").Return(BinaryOperators.Conjunction))
                    .Or(Parse.String("Or").Return(BinaryOperators.Disjunction))
                    .Or(Parse.Char('=').Return(BinaryOperators.Equality))
                    .Or(Parse.String("!=").Return(BinaryOperators.Inequality))
                    .Or(Parse.String(">=").Return(BinaryOperators.GreaterOrEqual))
                    .Or(Parse.String(">").Return(BinaryOperators.GreaterThan))
                    .Or(Parse.String("<=").Return(BinaryOperators.LessOrEqual))
                    .Or(Parse.String("<").Return(BinaryOperators.LessThan));

        public static readonly Parser<UnaryOperators> UnaryOperator =
                Parse.Char('!').Return(UnaryOperators.Negation)
                    .Or(Parse.String("Exist").Return(UnaryOperators.Exist)
                    .Or(Parse.String("NotExist").Return(UnaryOperators.NotExist)));

        public static readonly Parser<Term> StringConstant =
                from open in Parse.Char('"').Once()
                from str in Parse.LetterOrDigit.Many().Text()
                from end in Parse.Char('"').Once()
                select new Term()
                {
                    Value = str,
                    ExpressionType = "String"
                };

        public static readonly Parser<Term> IntegerConstant =
                from op in Parse.Optional(Parse.Char('-').Token())
                from str in Parse.Decimal
                select new Term()
                {
                    Value = op.IsDefined ? String.Format("-{0}", str) : str,
                    ExpressionType = "Integer"
                };

        public static readonly Parser<Term> BooleanConstant =
                from str in Parse.String("true")
                    .Or(Parse.String("True"))
                    .Or(Parse.String("false"))
                    .Or(Parse.String("False"))
                    .Text()
                select new Term()
                {
                    Value = str,
                    ExpressionType = "Boolean"
                };

        public static readonly Parser<Term> TypedObject =
                from type in Parse.LetterOrDigit.AtLeastOnce().Text()
                from del in Parse.String("::")
                from name in Parse.LetterOrDigit.AtLeastOnce().Text()
                select new Term()
                {
                    Value = name,
                    ExpressionType = type
                };

        public static readonly Parser<Term> Term =
                    TypedObject
                    .Or(BooleanConstant)
                    .Or(IntegerConstant)
                    .Or(StringConstant);

        public static readonly Parser<UnaryRelation> UnaryRelation =
                from op in UnaryOperator
                from ws in WhitespacesOrComments
                from exp in Parse.Ref(() => GeneralExpression)
                select new UnaryRelation()
                {
                    Expression = exp,
                    Operator = op,
                    ExpressionType = "UnaryRelation"
                };

        public static readonly Parser<BinaryRelation> BinaryRelation =
                from left in Parse.Ref(() => GeneralExpression)
                from ws1 in WhitespacesOrComments
                from op in BinaryOperator
                from ws2 in WhitespacesOrComments
                from right in BinaryRelation
                              .Or(GeneralExpression)
                select new BinaryRelation()
                {
                    Left = left,
                    Operator = op,
                    Right = right,
                    ExpressionType = "BinaryRelation"
                };

        public static readonly Parser<IExpression> ParenthesisExpression =
                from open in Parse.Char('(').Once()
                from ws1 in WhitespacesOrComments
                from expr in BinaryRelation
                            .Or(GeneralExpression)
                from ws2 in WhitespacesOrComments
                from close in Parse.Char(')').Once()
                select expr;

        public static readonly Parser<IExpression> GeneralExpression =
                UnaryRelation
                .Or<IExpression>(Term)
                .Or<IExpression>(ParenthesisExpression);

        public static Parser<IExpression> ParseExpression =
                from ws1 in WhitespacesOrComments
                from expr in BinaryRelation
                             .Or(GeneralExpression)
                from ws2 in WhitespacesOrComments
                from end in Parse.LineTerminator
                select expr;
    }
}
