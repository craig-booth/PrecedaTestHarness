using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness.IO
{
    public interface ITestSuiteLoader
    {
        TestSuite Load(string fileName);
        void Save(TestSuite testSuite, string fileName);
    }

    public interface ITestSuiteResultWriter
    {
        void WriteResults(TestSuite testSuite, Dictionary<string, string> variables, string fileName);
    }

}
