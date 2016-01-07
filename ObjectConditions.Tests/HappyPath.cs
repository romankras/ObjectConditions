﻿using NUnit.Framework;
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
                ExpressionType = "String",
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
                ExpressionType = "Integer",
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
                ExpressionType = "Boolean",
                Value = "true"
            };

            var parsed = LanguageGrammar.BooleanConstant.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "True";

            ast = new Term()
            {
                ExpressionType = "Boolean",
                Value = "True"
            };

            parsed = LanguageGrammar.BooleanConstant.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "false";

            ast = new Term()
            {
                ExpressionType = "Boolean",
                Value = "false"
            };

            parsed = LanguageGrammar.BooleanConstant.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "False";

            ast = new Term()
            {
                ExpressionType = "Boolean",
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
                ExpressionType = "type",
                Value = "somestring"
            };

            var parsed = LanguageGrammar.TypedObject.Parse(str);
            Assert.AreEqual(ast, parsed);
        }

        [Test]
        public void TermTest()
        {
            var str = "type::somestring";

            var ast = new Term()
            {
                ExpressionType = "type",
                Value = "somestring"
            };

            var parsed = LanguageGrammar.Term.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "True";

            ast = new Term()
            {
                ExpressionType = "Boolean",
                Value = "True"
            };

            parsed = LanguageGrammar.Term.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "\"somestring\"";

            ast = new Term()
            {
                ExpressionType = "String",
                Value = "somestring"
            };

            parsed = LanguageGrammar.Term.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = "123";

            ast = new Term()
            {
                ExpressionType = "Integer",
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
                    ExpressionType = "type",
                    Value = "somestring"
                },
                ExpressionType = "UnaryRelation",
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
                    ExpressionType = "type",
                    Value = "somestring"
                },
                ExpressionType = "BinaryRelation",
                Operator = BinaryOperators.Equality,
                Right = new Term()
                {
                    ExpressionType = "Integer",
                    Value = "123"
                }
            };

            var parsed = LanguageGrammar.BinaryRelation.Parse(str);
            Assert.AreEqual(ast, parsed);
        }
    }
}