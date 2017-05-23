using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestHarness
{
    public interface ITestItem
    {
        Guid Id { get; }

        string Name { get; set; }
        string Description { get; set; }

        DateTime StartTime { get; }
        DateTime EndTime { get; }
        TestResult Result { get; }
        TestSummary Summary { get; }

        int TestCount { get; }

        Task<TestResult> RunAsync(Dictionary<string, string> variables, string outputFolder, CancellationToken cancellationToken, IProgress<TestProgress> progress);

        TestCase GetTestCase(Guid id);
    }
}
