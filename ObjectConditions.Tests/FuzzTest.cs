using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectConditions;
using Sprache;

namespace ObjectConditions.Tests
{
    [TestClass]
    public class FuzzTest
    {
        /// <summary>
        /// Main fuzz test that checks any syntax tree that could be derived.
        /// </summary>
        [TestMethod]
        public void MainFuzzTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                var test = Helper.GetRandomAst(10) as ILogicalExpression;
                var str = String.Format(
                    "{0}{1}{2}",
                    Helper.GetRandomCommentOrWhitespace(),
                    Helper.AstObjectToString(test),
                    Helper.GetRandomCommentOrWhitespace());
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseAst, str) as ILogicalExpression;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks ConfigValue element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void ConfigValueFuzzTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(ConfigValue), 0, 0) as ConfigValue;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.ConfigValue, str) as ConfigValue;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateBooleanExpression(), parsed.EvaluateBooleanExpression(), str);
                Helper.AreEqualExtended(test.EvaluateNumericExpression(), parsed.EvaluateNumericExpression(), str);
                Helper.AreEqualExtended(test.EvaluateStringExpression(), parsed.EvaluateStringExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks BooleanValue element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void BooleanValueFuzzTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(BooleanValue), 0, 0) as BooleanValue;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.BooleanValue, str) as BooleanValue;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateBooleanExpression(), parsed.EvaluateBooleanExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks NumericValue element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void NumericValueFuzzTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(NumericValue), 0, 0) as NumericValue;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.NumericValue, str) as NumericValue;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateNumericExpression(), parsed.EvaluateNumericExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks StringValue element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void StringValueFuzzTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(StringValue), 0, 0) as StringValue;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.StringValue, str) as StringValue;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateStringExpression(), parsed.EvaluateStringExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks StringBinaryRelation element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void StringBinaryRelationFuzzTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(StringBinaryRelation), 0, 0) as ILogicalExpression;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseAst, str) as ILogicalExpression;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks ConfigBinaryRelation element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void ConfigBinaryRelationFuzzTest()
        {
            for (int i = 0; i < 100; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(ConfigBinaryRelation), 0, 0) as ILogicalExpression;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseAst, str) as ILogicalExpression;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks BooleanBinaryRelation element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void BooleanBinaryRelationFuzzTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(BooleanBinaryRelation), 0, 0) as ILogicalExpression;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseAst, str) as ILogicalExpression;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks NumericBinaryRelation element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void NumericBinaryRelationFuzzTest()
        {
            for (int i = 0; i < 100; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(NumericBinaryRelation), 0, 0) as ILogicalExpression;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseAst, str) as ILogicalExpression;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that checks LogicalBinaryRelation element of abstract syntax tree.
        /// </summary>
        [TestMethod]
        public void LogicalBinaryRelationFuzzTest()
        {
            for (int i = 0; i < 100; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(LogicalBinaryRelation), 0, 0) as ILogicalExpression;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseAst, str) as ILogicalExpression;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression(), str);
            }
        }

        /// <summary>
        /// Fuzz test that create random BooleanBinaryRelation object and inverts it's IsNegated flag.
        /// </summary>
        [TestMethod]
        public void NegationFuzzTest()
        {
            for (int i = 0; i < 20; i++)
            {
                var test = Helper.GetRandomAstObject(typeof(BooleanBinaryRelation), 0, 0) as ILogicalExpression;
                test.IsNegated = !test.IsNegated;
                var str = Helper.AstObjectToString(test);
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseAst, str) as ILogicalExpression;
                Helper.AreEqualExtended(test, parsed, str);
                Helper.AreEqualExtended(test.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression(), str);
            }
        }
    }
}
