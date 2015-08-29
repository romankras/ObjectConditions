using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectConditions;
using Sprache;

namespace ObjectConditions.Tests
{
    /// <summary>
    /// Main class for tests of missed boundary conditions.
    /// </summary>
    [TestClass]
    public class BoundaryConditions
    {
        /// <summary>
        /// Strings which starts with number should be parsed without any errors.
        /// </summary>
        [TestMethod]
        public void StringValueStartsWithNumber()
        {
            var test = "(  (  7 =7 ) And (  !(  ConfigValue::ypynjkdfv1z ='3gkgddyw2jd' )) )";
            LanguageGrammar.ParseAst.Parse(test);

            var test2 = "ConfigValue::j1veqg10zux   = '0nkw5llpkej'";
            var parsed = LanguageGrammar.ParseAst.Parse(test2);
            Assert.AreEqual<Type>(typeof(StringBinaryRelation), parsed.GetType());
        }

        /// <summary>
        /// Negated expression should follow this format: !(Expr)
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void NegationWithoutParentheses()
        {
            var test = "!ConfigValue::ypynjkdfv1z ='3gkgddyw2jd'";
            LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Test that checks all input to be parsed until the end of input (line terminator).
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidTokenInsteadOfEndOfInput()
        {
            var test = "ConfigValue::TheValue = true )))))))))";
            LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Operators could not be separated by whitespaces from expressions.
        /// </summary>
        [TestMethod]
        public void OperatorsWithoutWhitespaces2()
        {
            var test = "ConfigValue::j1veqg10zux   = '0nkw5llpkej'And ConfigValue::j1veqg10zux   = '0nkw5llpkej'";
            var parsed = LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Operators could not be separated by whitespaces from expressions.
        /// </summary>
        [TestMethod]
        public void OperatorsWithoutWhitespaces3()
        {
            var test = "(  (  7 =7 ) And(  !(  ConfigValue::ypynjkdfv1z ='3gkgddyw2jd' )) )";
            LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Binary relation that consists of ConfigValue elements should be parsed to ConfigBinaryRelation object.
        /// </summary>
        [TestMethod]
        public void TwoConfigsInOneRelation()
        {
            var ast = new ConfigBinaryRelation()
                {
                    LeftOperand = (ConfigValue)Helper.GetRandomAstObject(typeof(ConfigValue), 0, 0),
                    Operator = ConfigBinaryOperators.Equality,
                    RightOperand = (ConfigValue)Helper.GetRandomAstObject(typeof(ConfigValue), 0, 0)
                };

            var test = Helper.AstObjectToString(ast);
            var parsed = LanguageGrammar.ParseAst.Parse(test);

            Assert.AreEqual<bool>(ast.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression());
            Assert.AreEqual(ast, parsed);
        }

        /// <summary>
        /// Test for every BooleanBinaryOperators members.
        /// </summary>
        [TestMethod]
        public void TestBooleanBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.BooleanBinaryOperator.Parse(str);
            Assert.AreEqual<BooleanBinaryOperators>(BooleanBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.BooleanBinaryOperator.Parse(str);
            Assert.AreEqual<BooleanBinaryOperators>(BooleanBinaryOperators.Inequality, result);
        }

        /// <summary>
        /// Test for every LogicalBinaryOperators members.
        /// </summary>
        [TestMethod]
        public void TestLogicalBinaryOperators()
        {
            var str = "=>";
            var result = LanguageGrammar.LogicalBinaryOperator.Parse(str);
            Assert.AreEqual<LogicalBinaryOperators>(LogicalBinaryOperators.Implication, result);

            str = "And";
            result = LanguageGrammar.LogicalBinaryOperator.Parse(str);
            Assert.AreEqual<LogicalBinaryOperators>(LogicalBinaryOperators.Conjunction, result);

            str = "Or";
            result = LanguageGrammar.LogicalBinaryOperator.Parse(str);
            Assert.AreEqual<LogicalBinaryOperators>(LogicalBinaryOperators.Disjunction, result);
        }

        /// <summary>
        /// Test for every StringBinaryOperators members.
        /// </summary>
        [TestMethod]
        public void TestStringBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.StringBinaryOperator.Parse(str);
            Assert.AreEqual<StringBinaryOperators>(StringBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.StringBinaryOperator.Parse(str);
            Assert.AreEqual<StringBinaryOperators>(StringBinaryOperators.Inequality, result);
        }

        /// <summary>
        /// Test for every NumericBinaryOperators members.
        /// </summary>
        [TestMethod]
        public void TestNumericBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.Inequality, result);

            str = "<";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.LessThan, result);

            str = "<=";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.LessOrEqual, result);

            str = ">";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.GreaterThan, result);

            str = ">=";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.GreaterOrEqual, result);
        }

        /// <summary>
        /// Test for every ConfigBinaryOperators members.
        /// </summary>
        [TestMethod]
        public void TestConfigBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual<ConfigBinaryOperators>(ConfigBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual<ConfigBinaryOperators>(ConfigBinaryOperators.Inequality, result);

            str = "<";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual<ConfigBinaryOperators>(ConfigBinaryOperators.LessThan, result);

            str = "<=";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual<ConfigBinaryOperators>(ConfigBinaryOperators.LessOrEqual, result);

            str = ">";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual<ConfigBinaryOperators>(ConfigBinaryOperators.GreaterThan, result);

            str = ">=";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual<ConfigBinaryOperators>(ConfigBinaryOperators.GreaterOrEqual, result);
        }

        /// <summary>
        /// Tests for BooleanBinaryRelation object.
        /// </summary>
        [TestMethod]
        public void TestBooleanBinaryRelation()
        {
            var teststr = "true=      true";
            var result = LanguageGrammar.BooleanBinaryRelation.Parse(teststr);
            Assert.AreEqual<bool>(true, result.LeftOperand.EvaluateBooleanExpression());
            Assert.AreEqual<bool>(true, result.RightOperand.EvaluateBooleanExpression());
            Assert.AreEqual<BooleanBinaryOperators>(BooleanBinaryOperators.Equality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            teststr = "false   !=false";
            result = LanguageGrammar.BooleanBinaryRelation.Parse(teststr);
            Assert.AreEqual<bool>(false, result.LeftOperand.EvaluateBooleanExpression());
            Assert.AreEqual<bool>(false, result.RightOperand.EvaluateBooleanExpression());
            Assert.AreEqual<BooleanBinaryOperators>(BooleanBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual<bool>(false, result.EvaluateLogicalExpression());

            teststr = "false   !=   ConfigValue::true";
            result = LanguageGrammar.BooleanBinaryRelation.Parse(teststr);
            Assert.AreEqual<bool>(false, result.LeftOperand.EvaluateBooleanExpression());
            Assert.AreEqual<bool>(true, result.RightOperand.EvaluateBooleanExpression());
            Assert.AreEqual<BooleanBinaryOperators>(BooleanBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());
        }

        /// <summary>
        /// Tests for StringBinaryRelation object.
        /// </summary>
        [TestMethod]
        public void TestStringBinaryRelation()
        {
            var teststr = "'123'=      '123'";
            var result = LanguageGrammar.StringBinaryRelation.Parse(teststr);
            Assert.AreEqual<string>("123", result.LeftOperand.EvaluateStringExpression());
            Assert.AreEqual<string>("123", result.RightOperand.EvaluateStringExpression());
            Assert.AreEqual<StringBinaryOperators>(StringBinaryOperators.Equality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            teststr = "'false'   !='true'";
            result = LanguageGrammar.StringBinaryRelation.Parse(teststr);
            Assert.AreEqual<string>("false", result.LeftOperand.EvaluateStringExpression());
            Assert.AreEqual<string>("true", result.RightOperand.EvaluateStringExpression());
            Assert.AreEqual<StringBinaryOperators>(StringBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            teststr = "'asd'   !=ConfigValue::asd";
            result = LanguageGrammar.StringBinaryRelation.Parse(teststr);
            Assert.AreEqual<string>("asd", result.LeftOperand.EvaluateStringExpression());
            Assert.AreEqual<string>("test", result.RightOperand.EvaluateStringExpression());
            Assert.AreEqual<StringBinaryOperators>(StringBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());
        }

        /// <summary>
        /// Tests for NumericBinaryRelation object.
        /// </summary>
        [TestMethod]
        public void TestNumericBinaryRelation()
        {
            var teststr = "123=      123";
            var result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual<int>(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual<int>(123, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.Equality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            teststr = "123     !=4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual<int>(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual<int>(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            teststr = "123>4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual<int>(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual<int>(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.GreaterThan, result.Operator);
            Assert.AreEqual<bool>(false, result.EvaluateLogicalExpression());

            teststr = "123>=4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual<int>(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual<int>(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.GreaterOrEqual, result.Operator);
            Assert.AreEqual<bool>(false, result.EvaluateLogicalExpression());

            teststr = "123<4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual<int>(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual<int>(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.LessThan, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            teststr = "123<=4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual<int>(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual<int>(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.LessOrEqual, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            teststr = "123   !=ConfigValue::456";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual<int>(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual<int>(0, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual<NumericBinaryOperators>(NumericBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual<bool>(true, result.EvaluateLogicalExpression());

            var test = "ConfigValue::l3h31c1nyyp   !=  23";
            var ast = new NumericBinaryRelation()
            {
                LeftOperand = new ConfigValue()
                {
                    Value = "l3h31c1nyyp"
                },
                Operator = NumericBinaryOperators.Inequality,
                RightOperand = new NumericValue()
                {
                    Value = 23
                }
            };

            var parsed = LanguageGrammar.ParseAst.Parse(test);
            Assert.AreEqual(ast, parsed);
        }

        /// <summary>
        /// Tests for different types of commentaries.
        /// </summary>
        [TestMethod]
        public void TestCommentBlocks()
        {
            var str = "!(   true   =   ConfigValue::dvfyBPS4   )";
            var ast = new BooleanBinaryRelation()
            {
                IsNegated = true,
                LeftOperand = new BooleanValue()
                {
                    Value = true
                },
                Operator = BooleanBinaryOperators.Equality,
                RightOperand = new ConfigValue()
                {
                    Value = "dvfyBPS4"
                }
            };

            var parsed = LanguageGrammar.ParseAst.Parse(str);
            Assert.AreEqual(ast, parsed);

            str = @"!(  //asdsa dad   
                true   =   ConfigValue::dvfyBPS4   
                
                /* assdasdad asdasa
                asjkdajk */

                /* assdasdad asdasgggga
                asjkdajk */
                )";
            parsed = LanguageGrammar.ParseAst.Parse(str);
            Assert.AreEqual(ast, parsed);
        }
    }
}
