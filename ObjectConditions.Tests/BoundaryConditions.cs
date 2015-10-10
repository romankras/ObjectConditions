using NUnit.Framework;
using Sprache;

namespace ObjectConditions.Tests
{
    /// <summary>
    /// Main class for tests of missed boundary conditions.
    /// </summary>
    [TestFixture]
    public class BoundaryConditions
    {
        /// <summary>
        /// Strings which starts with number should be parsed without any errors.
        /// </summary>
        [Test]
        public void StringValueStartsWithNumber()
        {
            var test = "(  (  7 =7 ) And (  !(  ConfigValue::ypynjkdfv1z ='3gkgddyw2jd' )) )";
            LanguageGrammar.ParseAst.Parse(test);

            var test2 = "ConfigValue::j1veqg10zux   = '0nkw5llpkej'";
            var parsed = LanguageGrammar.ParseAst.Parse(test2);
            Assert.AreEqual(typeof(StringBinaryRelation), parsed.GetType());
        }

        /// <summary>
        /// Negated expression should follow this format: !(Expr)
        /// </summary>
        [Test]
        [ExpectedException(typeof(ParseException))]
        public void NegationWithoutParentheses()
        {
            var test = "!ConfigValue::ypynjkdfv1z ='3gkgddyw2jd'";
            LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Test that checks all input to be parsed until the end of input (line terminator).
        /// </summary>
        [Test]
        [ExpectedException(typeof(ParseException))]
        public void InvalidTokenInsteadOfEndOfInput()
        {
            var test = "ConfigValue::TheValue = true )))))))))";
            LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Operators could not be separated by whitespaces from expressions.
        /// </summary>
        [Test]
        public void OperatorsWithoutWhitespaces2()
        {
            var test = "ConfigValue::j1veqg10zux   = '0nkw5llpkej'And ConfigValue::j1veqg10zux   = '0nkw5llpkej'";
            LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Operators could not be separated by whitespaces from expressions.
        /// </summary>
        [Test]
        public void OperatorsWithoutWhitespaces3()
        {
            var test = "(  (  7 =7 ) And(  !(  ConfigValue::ypynjkdfv1z ='3gkgddyw2jd' )) )";
            LanguageGrammar.ParseAst.Parse(test);
        }

        /// <summary>
        /// Binary relation that consists of ConfigValue elements should be parsed to ConfigBinaryRelation object.
        /// </summary>
        [Test]
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

            Assert.AreEqual(ast.EvaluateLogicalExpression(), parsed.EvaluateLogicalExpression());
            Assert.AreEqual(ast, parsed);
        }

        /// <summary>
        /// Test for every BooleanBinaryOperators members.
        /// </summary>
        [Test]
        public void TestBooleanBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.BooleanBinaryOperator.Parse(str);
            Assert.AreEqual(BooleanBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.BooleanBinaryOperator.Parse(str);
            Assert.AreEqual(BooleanBinaryOperators.Inequality, result);
        }

        /// <summary>
        /// Test for every LogicalBinaryOperators members.
        /// </summary>
        [Test]
        public void TestLogicalBinaryOperators()
        {
            var str = "=>";
            var result = LanguageGrammar.LogicalBinaryOperator.Parse(str);
            Assert.AreEqual(LogicalBinaryOperators.Implication, result);

            str = "And";
            result = LanguageGrammar.LogicalBinaryOperator.Parse(str);
            Assert.AreEqual(LogicalBinaryOperators.Conjunction, result);

            str = "Or";
            result = LanguageGrammar.LogicalBinaryOperator.Parse(str);
            Assert.AreEqual(LogicalBinaryOperators.Disjunction, result);
        }

        /// <summary>
        /// Test for every StringBinaryOperators members.
        /// </summary>
        [Test]
        public void TestStringBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.StringBinaryOperator.Parse(str);
            Assert.AreEqual(StringBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.StringBinaryOperator.Parse(str);
            Assert.AreEqual(StringBinaryOperators.Inequality, result);
        }

        /// <summary>
        /// Test for every NumericBinaryOperators members.
        /// </summary>
        [Test]
        public void TestNumericBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual(NumericBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual(NumericBinaryOperators.Inequality, result);

            str = "<";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual(NumericBinaryOperators.LessThan, result);

            str = "<=";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual(NumericBinaryOperators.LessOrEqual, result);

            str = ">";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual(NumericBinaryOperators.GreaterThan, result);

            str = ">=";
            result = LanguageGrammar.NumericBinaryOperator.Parse(str);
            Assert.AreEqual(NumericBinaryOperators.GreaterOrEqual, result);
        }

        /// <summary>
        /// Test for every ConfigBinaryOperators members.
        /// </summary>
        [Test]
        public void TestConfigBinaryOperators()
        {
            var str = "=";
            var result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual(ConfigBinaryOperators.Equality, result);

            str = "!=";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual(ConfigBinaryOperators.Inequality, result);

            str = "<";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual(ConfigBinaryOperators.LessThan, result);

            str = "<=";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual(ConfigBinaryOperators.LessOrEqual, result);

            str = ">";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual(ConfigBinaryOperators.GreaterThan, result);

            str = ">=";
            result = LanguageGrammar.ConfigBinaryOperator.Parse(str);
            Assert.AreEqual(ConfigBinaryOperators.GreaterOrEqual, result);
        }

        /// <summary>
        /// Tests for BooleanBinaryRelation object.
        /// </summary>
        [Test]
        public void TestBooleanBinaryRelation()
        {
            var teststr = "true=      true";
            var result = LanguageGrammar.BooleanBinaryRelation.Parse(teststr);
            Assert.AreEqual(true, result.LeftOperand.EvaluateBooleanExpression());
            Assert.AreEqual(true, result.RightOperand.EvaluateBooleanExpression());
            Assert.AreEqual(BooleanBinaryOperators.Equality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

            teststr = "false   !=false";
            result = LanguageGrammar.BooleanBinaryRelation.Parse(teststr);
            Assert.AreEqual(false, result.LeftOperand.EvaluateBooleanExpression());
            Assert.AreEqual(false, result.RightOperand.EvaluateBooleanExpression());
            Assert.AreEqual(BooleanBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual(false, result.EvaluateLogicalExpression());

            teststr = "false   !=   ConfigValue::true";
            result = LanguageGrammar.BooleanBinaryRelation.Parse(teststr);
            Assert.AreEqual(false, result.LeftOperand.EvaluateBooleanExpression());
            Assert.AreEqual(true, result.RightOperand.EvaluateBooleanExpression());
            Assert.AreEqual(BooleanBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());
        }

        /// <summary>
        /// Tests for StringBinaryRelation object.
        /// </summary>
        [Test]
        public void TestStringBinaryRelation()
        {
            var teststr = "'123'=      '123'";
            var result = LanguageGrammar.StringBinaryRelation.Parse(teststr);
            Assert.AreEqual("123", result.LeftOperand.EvaluateStringExpression());
            Assert.AreEqual("123", result.RightOperand.EvaluateStringExpression());
            Assert.AreEqual(StringBinaryOperators.Equality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

            teststr = "'false'   !='true'";
            result = LanguageGrammar.StringBinaryRelation.Parse(teststr);
            Assert.AreEqual("false", result.LeftOperand.EvaluateStringExpression());
            Assert.AreEqual("true", result.RightOperand.EvaluateStringExpression());
            Assert.AreEqual(StringBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

            teststr = "'asd'   !=ConfigValue::asd";
            result = LanguageGrammar.StringBinaryRelation.Parse(teststr);
            Assert.AreEqual("asd", result.LeftOperand.EvaluateStringExpression());
            Assert.AreEqual("test", result.RightOperand.EvaluateStringExpression());
            Assert.AreEqual(StringBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());
        }

        /// <summary>
        /// Tests for NumericBinaryRelation object.
        /// </summary>
        [Test]
        public void TestNumericBinaryRelation()
        {
            var teststr = "123=      123";
            var result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual(123, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual(NumericBinaryOperators.Equality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

            teststr = "123     !=4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual(NumericBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

            teststr = "123>4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual(NumericBinaryOperators.GreaterThan, result.Operator);
            Assert.AreEqual(false, result.EvaluateLogicalExpression());

            teststr = "123>=4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual(NumericBinaryOperators.GreaterOrEqual, result.Operator);
            Assert.AreEqual(false, result.EvaluateLogicalExpression());

            teststr = "123<4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual(NumericBinaryOperators.LessThan, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

            teststr = "123<=4567";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual(4567, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual(NumericBinaryOperators.LessOrEqual, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

            teststr = "123   !=ConfigValue::456";
            result = LanguageGrammar.NumericBinaryRelation.Parse(teststr);
            Assert.AreEqual(123, result.LeftOperand.EvaluateNumericExpression());
            Assert.AreEqual(0, result.RightOperand.EvaluateNumericExpression());
            Assert.AreEqual(NumericBinaryOperators.Inequality, result.Operator);
            Assert.AreEqual(true, result.EvaluateLogicalExpression());

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
        [Test]
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
