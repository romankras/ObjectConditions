using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectConditions;
using Sprache;

namespace ObjectConditions.Tests
{
    [TestClass]
    public class PerformanceTest
    {
        private static List<Tuple<int, long>> memoryData;

        public static void GetMemory(int count)
        {
            Process currentProcess = Process.GetCurrentProcess();

            if (memoryData != null && currentProcess != null)
            {
                memoryData.Add(new Tuple<int, long>(count, currentProcess.WorkingSet64));
            }

            Thread.Sleep(10);
        }

        [TestMethod]
        public void GeneralPerformance()
        {
            var data = new List<Tuple<int, double>>();
            memoryData = new List<Tuple<int, long>>();

            for (int i = 0; i < 10000; i++)
            {
                var sw = new Stopwatch();
                var test = Helper.GetRandomAst(100) as IAstObject;
                var str = String.Format(
                    "{0}{1}{2}",
                    Helper.GetRandomCommentOrWhitespace(),
                    Helper.AstObjectToString(test),
                    Helper.GetRandomCommentOrWhitespace());
                var measure = new Thread(() => GetMemory(test.ChildrenCount));
                measure.Start();
                sw.Start();
                LanguageGrammar.ParseAst.Parse(str);
                sw.Stop();
                measure.Abort();
                // number of elements in the ast: milliseconds
                data.Add(new Tuple<int, double>(test.ChildrenCount, sw.Elapsed.TotalMilliseconds));
            }

            Helper.CreateDataFile("time.dat", data);
            Helper.CreateDataFile("memory.dat", memoryData);
        }
    }
}
