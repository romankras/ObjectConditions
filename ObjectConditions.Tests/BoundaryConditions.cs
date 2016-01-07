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
                ExpressionType = "String",
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
                ExpressionType = "Integer",
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
            LanguageGrammar.TypedObject.Parse(str);
        }

        [Test]
        [ExpectedException(typeof(ParseException))]
        public void EmptyTypedObjectTest2()
        {
            const string str = "type::";
            LanguageGrammar.TypedObject.Parse(str);
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
                ExpressionType = "BinaryRelation",
                Left = new Term()
                {
                    ExpressionType = "String",
                    Value = string.Empty
                },
                Operator = BinaryOperators.Inequality,
                Right = new BinaryRelation()
                {
                    ExpressionType = "BinaryRelation",
                    Left = new UnaryRelation()
                    {
                        ExpressionType = "UnaryRelation",
                        Operator = UnaryOperators.NotExist,
                        Expression = new Term()
                        {
                            ExpressionType = "g",
                            Value = "zVBLdNY"
                        }
                    },
                    Operator = BinaryOperators.Equality,
                    Right = new Term()
                    {
                        ExpressionType = "m",
                        Value = "3U1Fe"
                    }
                }
            };

            var parsed = LanguageGrammar.ParseExpression.Parse(str);
            Assert.AreEqual(ast, parsed);
        }
    }
}
