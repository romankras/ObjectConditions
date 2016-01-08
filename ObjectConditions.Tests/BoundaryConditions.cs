using NUnit.Framework;
using Sprache;

namespace ObjectConditions.Tests
{
    [TestFixture]
    public class BoundaryConditions
    {
        [Test]
        public void EmptyStringConstantTest()
        {
            const string str = "\"\"";

            var ast = new Term()
            {
                ExpressionType = ExpressionTypes.String,
                Value = string.Empty
            };

            var parsed = LanguageGrammar.StringConstant.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void NegativeIntegerConstantTest()
        {
            const string str = "-1";

            var ast = new Term()
            {
                ExpressionType = ExpressionTypes.Integer,
                Value = "-1"
            };

            var parsed = LanguageGrammar.IntegerConstant.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void MalformedStringConstantTest()
        {
            const string str = "somestring";
            LanguageGrammar.StringConstant.Parse(str);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void EmptyTypedObjectTest1()
        {
            const string str = "::somestring";
            LanguageGrammar.SystemObject.Parse(str);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void EmptyTypedObjectTest2()
        {
            const string str = "type::";
            LanguageGrammar.SystemObject.Parse(str);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void IllegalBooleanConstantTest()
        {
            const string str = "tRue";
            LanguageGrammar.BooleanConstant.Parse(str);
        }

        [Test]
        public void UnaryRelationAndBinaryRelationNestingOrder()
        {
            const string str = "(\"\") != (NotExist g::zVBLdNY = m::3U1Fe)";

            var ast = new BinaryRelation()
            {
                ExpressionType = ExpressionTypes.BinaryRelation,
                Left = new Term()
                {
                    ExpressionType = ExpressionTypes.String,
                    Value = string.Empty
                },
                Operator = BinaryOperators.Inequality,
                Right = new BinaryRelation()
                {
                    ExpressionType = ExpressionTypes.BinaryRelation,
                    Left = new UnaryRelation()
                    {
                        ExpressionType = ExpressionTypes.UnaryRelation,
                        Operator = UnaryOperators.NotExist,
                        Expression = new Term()
                        {
                            ExpressionType = ExpressionTypes.SystemObject,
                            ObjectType = "g",
                            Value = "zVBLdNY"
                        }
                    },
                    Operator = BinaryOperators.Equality,
                    Right = new Term()
                    {
                        ExpressionType = ExpressionTypes.SystemObject,
                        ObjectType = "m",
                        Value = "3U1Fe"
                    }
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void EndOfInputTest()
        {
            const string str = "(\"\") != (NotExist g::zVBLdNY = m::3U1Fe)))";
            LanguageGrammar.ParseExpression.Parse(str);
        }

        [Test]
        public void EmptyObjects()
        {
            var ast = new BinaryRelation()
            {
                Left = new BinaryRelation()
                {
                    Left = new Term(),
                    Right = new Term()
                },
                Operator = BinaryOperators.Equality,
                Right = new UnaryRelation()
                {
                    Expression = new Term()
                }
            };

            var flat = Helper.GetFlatTree(ast);
            Assert.AreEqual(6, flat.Count);
        }
    }
}
