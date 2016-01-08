using System.Linq;
using NUnit.Framework;
using Sprache;

namespace ObjectConditions.Tests
{
    [TestFixture]
    public class HappyPath
    {
        [Test]
        public void StringConstantTest()
        {
            const string str = "\"somestring\"";

            var ast = new Term()
            {
                ExpressionType = ExpressionTypes.String,
                Value = "somestring"
            };

            var parsed = LanguageGrammar.StringConstant.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void IntegerConstantTest()
        {
            const string str = "123";

            var ast = new Term()
            {
                ExpressionType = ExpressionTypes.Integer,
                Value = "123"
            };

            var parsed = LanguageGrammar.IntegerConstant.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void BooleanConstantTest()
        {
            var str = "true";

            var ast = new Term()
            {
                ExpressionType = ExpressionTypes.Boolean,
                Value = "true"
            };

            var parsed = LanguageGrammar.BooleanConstant.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "True";

            ast = new Term()
            {
                ExpressionType = ExpressionTypes.Boolean,
                Value = "True"
            };

            parsed = LanguageGrammar.BooleanConstant.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "false";

            ast = new Term()
            {
                ExpressionType = ExpressionTypes.Boolean,
                Value = "false"
            };

            parsed = LanguageGrammar.BooleanConstant.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "False";

            ast = new Term()
            {
                ExpressionType = ExpressionTypes.Boolean,
                Value = "False"
            };

            parsed = LanguageGrammar.BooleanConstant.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void TypedObjectTest()
        {
            const string str = "type::somestring";

            var ast = new Term()
            {
                ExpressionType = ExpressionTypes.SystemObject,
                ObjectType = "type",
                Value = "somestring"
            };

            var parsed = LanguageGrammar.SystemObject.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void TermTest()
        {
            var str = "type::somestring";

            var ast = new Term()
            {
                ExpressionType = ExpressionTypes.SystemObject,
                ObjectType = "type",
                Value = "somestring"
            };

            var parsed = LanguageGrammar.Term.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "True";

            ast = new Term()
            {
                ExpressionType = ExpressionTypes.Boolean,
                Value = "True"
            };

            parsed = LanguageGrammar.Term.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "\"somestring\"";

            ast = new Term()
            {
                ExpressionType = ExpressionTypes.String,
                Value = "somestring"
            };

            parsed = LanguageGrammar.Term.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "123";

            ast = new Term()
            {
                ExpressionType = ExpressionTypes.Integer,
                Value = "123"
            };

            parsed = LanguageGrammar.Term.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void UnaryRelationTest()
        {
            const string str = "! type::somestring";

            var ast = new UnaryRelation()
            {
                Expression = new Term()
                {
                    ExpressionType = ExpressionTypes.SystemObject,
                    ObjectType = "type",
                    Value = "somestring"
                },
                ExpressionType = ExpressionTypes.UnaryRelation,
                Operator = UnaryOperators.Negation
            };

            var parsed = LanguageGrammar.UnaryRelation.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void BinaryRelationTest()
        {
            const string str = "type::somestring = 123";

            var ast = new BinaryRelation()
            {
                Left = new Term()
                {
                    ExpressionType = ExpressionTypes.SystemObject,
                    ObjectType = "type",
                    Value = "somestring"
                },
                ExpressionType = ExpressionTypes.BinaryRelation,
                Operator = BinaryOperators.Equality,
                Right = new Term()
                {
                    ExpressionType = ExpressionTypes.Integer,
                    Value = "123"
                }
            };

            var parsed = LanguageGrammar.BinaryRelation.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void UnaryOperatorsTest()
        {
            var str = "!";
            var parsed = LanguageGrammar.UnaryOperator.Parse(str);
            Assert.AreEqual(UnaryOperators.Negation, parsed);

            str = "NotExist";
            parsed = LanguageGrammar.UnaryOperator.Parse(str);
            Assert.AreEqual(UnaryOperators.NotExist, parsed);

            str = "Exist";
            parsed = LanguageGrammar.UnaryOperator.Parse(str);
            Assert.AreEqual(UnaryOperators.Exist, parsed);
        }

        [Test]
        public void BinaryOperatorsTest()
        {
            var str = "=";
            var parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.Equality, parsed);

            str = "!=";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.Inequality, parsed);

            str = ">";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.GreaterThan, parsed);

            str = ">=";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.GreaterOrEqual, parsed);

            str = "<";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.LessThan, parsed);

            str = "<=";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.LessOrEqual, parsed);

            str = "=>";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.Implication, parsed);

            str = "And";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.Conjunction, parsed);

            str = "Or";
            parsed = LanguageGrammar.BinaryOperator.Parse(str);
            Assert.AreEqual(BinaryOperators.Disjunction, parsed);
        }

        [Test]
        public void ChildrenTest()
        {
            var term1 = new Term()
            {
                Value = "value1",
                ExpressionType = ExpressionTypes.String
            };

            var term2 = new Term()
            {
                Value = "value2",
                ExpressionType = ExpressionTypes.SystemObject,
                ObjectType = "type2"
            };

            var ast = new BinaryRelation()
            {
                Left = term1,
                Right = term2
            };

            Assert.AreEqual(2, ast.Children.ToList().Count);
            Assert.IsTrue(ast.Children.Contains(term1));
            Assert.IsTrue(ast.Children.Contains(term2));

            var ast2 = new UnaryRelation()
            {
                Expression = term2
            };

            Assert.AreEqual(1, ast2.Children.ToList().Count);
            Assert.IsTrue(ast2.Children.Contains(term2));

            var ast3 = new Term();

            Assert.AreEqual(0, ast3.Children.ToList().Count);
        }
    }
}
