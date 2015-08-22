using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectConditions;
using Sprache;

namespace ObjectConditions.Tests
{
    [TestClass]
    public class PerformanceTest
    {
        [TestMethod]
        public void GeneralPerformance()
        {
            var data = new List<Tuple<int, double>>();

            for (int i = 0; i < 10000; i++)
            {
                var sw = new Stopwatch();
                var test = Helper.GetRandomAst(100) as IAstObject;
                var str = String.Format(
                    "{0}{1}{2}",
                    Helper.GetRandomCommentOrWhitespace(),
                    Helper.AstObjectToString(test),
                    Helper.GetRandomCommentOrWhitespace());
                sw.Start();
                LanguageGrammar.ParseAst.Parse(str);
                sw.Stop();
                // number of elements in the ast: milliseconds
                data.Add(new Tuple<int, double>(test.ChildrenCount, sw.Elapsed.TotalMilliseconds));
            }

            Helper.CreateDataFile("general.dat", data);
        }
    }
}
