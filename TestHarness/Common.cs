using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    public enum RunMode { Async, Sync };

    public enum TestType { Positive, Negative };

    public enum TestResult { NotRun, InProgress, SetupFailed, Passed, Failed };

    public class TestProgress
    {
        public Guid Id { get; }
        public TestResult Result { get; }

        public TestProgress(Guid id, TestResult result)
        {
            Id = id;
            Result = result;
        }
    }

    public class TestSummary
    {
        public int Total { get; set; }
        public int NotRun { get; set; }
        public int SetupFailed { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
    }

}
