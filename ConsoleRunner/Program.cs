using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

using TestHarness;

namespace ConsoleRunner
{
    class Program
    {

        /* Usage 
         * 
         *  ConsoleRunner {testfile} -server={server} -filelibrary={filelibrary} -user={user} -password={password} 
         * 
         */

        static void Main(string[] args)
        {    
            var testFile = args[0];
            var variables = ParseCommandLineParameters(args.Skip(1));

            var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PrecedaTestHarness");
            Directory.CreateDirectory(outputFolder);
        
            var testRunOutputFolder = Path.Combine(outputFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            Directory.CreateDirectory(testRunOutputFolder);


            var consoleSuiteRunner = new ConsoleSuiteRunner();

            consoleSuiteRunner.RunTest(testFile, variables, testRunOutputFolder);

            var transform = new XslCompiledTransform();
            transform.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Report.xsl"));
            transform.Transform(Path.Combine(testRunOutputFolder, "result.xml"), Path.Combine(testRunOutputFolder, "result.html"));
        }

        private static Dictionary<string, string> ParseCommandLineParameters(IEnumerable<string> args)
        {
            var variables = new Dictionary<string, string>();

            foreach (var arg in args)
            {
                var argComponents = arg.Split('=');

                var argName = argComponents[0];
                var argValue = argComponents[1];

                if (argName.StartsWith("-"))
                {
                    argName = argName.Substring(1);
                }

                variables[argName.ToUpper()] = argValue;
            }

            return variables;
        }
    }
}
