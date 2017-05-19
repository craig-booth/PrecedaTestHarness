using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestHarness
{
    public enum TaskResult { NotRun, InProgress, ExceptionOccurred, Passed, Failed };

    public interface ITask
    {
        Guid Id { get; }
        string Description { get; }
        string Message { get; }
        TaskResult Result { get; }

        Task<bool> RunAsync(Dictionary<string, string> variables, TestOutputFileNameGenerator fileNameGenerator, CancellationToken cancellationToken, IProgress<TestProgress> progress);
    }
}
