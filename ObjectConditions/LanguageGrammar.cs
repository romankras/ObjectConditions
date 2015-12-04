using System.Collections.Generic;
using Sprache;

namespace ObjectConditions
{
    public class LanguageGrammar
    {
        private static readonly CommentParser Comment = new CommentParser();

        private static readonly Parser<IEnumerable<string>> WhitespacesOrComments =
                Parse.WhiteSpace.Many().Text()
                    .Or(Comment.AnyComment)
                    .Or(Parse.Char('\n').Many().Text())
                    .Many();

        private static readonly Parser<BinaryOperators> BinaryOperator =
                Parse.String("=>").Return(BinaryOperators.Implication)
                    .Or(Parse.String("And").Return(BinaryOperators.Conjunction))
                    .Or(Parse.String("Or").Return(BinaryOperators.Disjunction))
                    .Or(Parse.Char('=').Return(BinaryOperators.Equality))
                    .Or(Parse.String("!=").Return(BinaryOperators.Inequality))
                    .Or(Parse.String(">=").Return(BinaryOperators.GreaterOrEqual))
                    .Or(Parse.String(">").Return(BinaryOperators.GreaterThan))
                    .Or(Parse.String("<=").Return(BinaryOperators.LessOrEqual))
                    .Or(Parse.String("<").Return(BinaryOperators.LessThan));

        private static readonly Parser<UnaryOperators> UnaryOperator =
                Parse.Char('!').Return(UnaryOperators.Negation)
                    .Or(Parse.String("Exist").Return(UnaryOperators.Exist)
                    .Or(Parse.String("NotExist").Return(UnaryOperators.NotExist)));

        private static readonly Parser<TypedObject> TypedObject =
                from type in Parse.LetterOrDigit.Many().Text().Token()
                from del in Parse.String("::").Once()
                from name in Parse.LetterOrDigit.Many().Text().Token()
                select new TypedObject()
                {
                    Name = name,
                    ObjectType = type
                };

        private static readonly Parser<ObjectValue> ObjectValue =
                from val in Parse.LetterOrDigit.Many().Text().Token()
                select new ObjectValue()
                {
                    Value = val
                };

        private static readonly Parser<UnaryRelation> UnaryRelation =
                from op in UnaryOperator
                from ws in WhitespacesOrComments
                from exp in Parse.Ref(() => GeneralExpression)
                select new UnaryRelation()
                {
                    Expression = exp,
                    Operator = op
                };

        private static readonly Parser<BinaryRelation> BinaryRelation =
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
                    Right = right
                };

        private static readonly Parser<IExpression> ParenthesisExpression =
                from open in Parse.Char('(').Once()
                from ws1 in WhitespacesOrComments
                from expr in BinaryRelation
                            .Or(GeneralExpression)
                from ws2 in WhitespacesOrComments
                from close in Parse.Char(')').Once()
                select expr;

        private static readonly Parser<IExpression> GeneralExpression =
                ParenthesisExpression
                .Or(UnaryRelation)
                .Or(TypedObject)
                .Or(ObjectValue);

        public static Parser<IExpression> ParseExpression =
                from ws1 in WhitespacesOrComments
                from expr in BinaryRelation
                             .Or(GeneralExpression)
                from ws2 in WhitespacesOrComments
                from end in Parse.LineTerminator
                select expr;
    }
}
