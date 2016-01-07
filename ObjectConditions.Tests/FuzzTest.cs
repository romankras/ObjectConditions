using System;
using NUnit.Framework;

namespace ObjectConditions.Tests
{
    [TestFixture]
    public class FuzzTest
    {
        [Test]
        public void MainTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomAst(10);
                var str = String.Format(
                    "{0}{1}{2}",
                    Helper.GetRandomCommentOrWhitespace(),
                    Helper.ExpressionToString(ast, true),
                    Helper.GetRandomCommentOrWhitespace());
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseExpression, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }

        [Test]
        public void StringConstantTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomObject("String", 0, 0);
                var str = Helper.ExpressionToString(ast, false);
                var parsed = Helper.ParseExtended(LanguageGrammar.StringConstant, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }

        [Test]
        public void BooleanConstantTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomObject("Boolean", 0, 0);
                var str = Helper.ExpressionToString(ast, false);
                var parsed = Helper.ParseExtended(LanguageGrammar.BooleanConstant, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }

        [Test]
        public void IntegerConstantTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomObject("Integer", 0, 0);
                var str = Helper.ExpressionToString(ast, false);
                var parsed = Helper.ParseExtended(LanguageGrammar.IntegerConstant, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }

        [Test]
        public void TypedObjectTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomObject(Helper.GetRandomString(false), 0, 0);
                var str = Helper.ExpressionToString(ast, false);
                var parsed = Helper.ParseExtended(LanguageGrammar.TypedObject, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }

        [Test]
        public void UnaryRelationTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomObject("UnaryRelation", 0, 0);
                var str = Helper.ExpressionToString(ast, false);
                var parsed = Helper.ParseExtended(LanguageGrammar.UnaryRelation, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }

        [Test]
        public void BinaryRelationTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomObject("BinaryRelation", 0, 0);
                var str = Helper.ExpressionToString(ast, false);
                var parsed = Helper.ParseExtended(LanguageGrammar.BinaryRelation, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }

        [Test]
        public void TermTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomObject("Term", 0, 0);
                var str = Helper.ExpressionToString(ast, false);
                var parsed = Helper.ParseExtended(LanguageGrammar.Term, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }
    }
}
