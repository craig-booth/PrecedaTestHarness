using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestHarness
{
    public class TestOutputFileNameGenerator
    {
        public string BasePath { get; private set; }
        private int _FileNumber;

        public TestOutputFileNameGenerator(string basePath)
        {
            BasePath = basePath;
            _FileNumber = 1;
        }

        public string GetOutputFileName(string name, string extension)
        {
            var outputFileName = Path.Combine(BasePath, string.Format("{0}_{1}.{2}", name, _FileNumber++, extension));

            return outputFileName;
        }

    }
}
