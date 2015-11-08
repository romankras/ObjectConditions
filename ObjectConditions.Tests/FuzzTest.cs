using System;
using NUnit.Framework;

namespace ObjectConditions.Tests
{
    [TestFixture]
    public class FuzzTest
    {
        [Test]
        public void MainFuzzTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var ast = Helper.GetRandomAst(10);
                var str = String.Format(
                    "{0}{1}{2}",
                    Helper.GetRandomCommentOrWhitespace(),
                    Helper.ExpressionToString(ast),
                    Helper.GetRandomCommentOrWhitespace());
                var parsed = Helper.ParseExtended(LanguageGrammar.ParseExpression, str);
                Helper.AreEqualExtended(ast, parsed, str);
            }
        }
    }
}
