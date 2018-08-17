using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using CsvHelper;

using TestHarness;
using TestHarness.IO;

namespace TestRunner
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource _CancellationTokenSource;
        private TestSuite _TestSuite;
        private Dictionary<string, string> _Variables;

        private string _OutputFolder;

        public MainForm()
        {
            InitializeComponent();

            //specify to use TLS 1.2 as default connection
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            _Variables = new Dictionary<string, string>();
            _OutputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PrecedaTestHarness");
            Directory.CreateDirectory(_OutputFolder);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            lsvTests.Items.Clear();

            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var suiteLoader = new XmlTestSuiteLoader();
                _TestSuite = suiteLoader.Load(dialog.FileName);

                lsvTests.Items.Clear();
                DisplayTestItem(_TestSuite.Test);

                lblTestsTotal.Text = _TestSuite.Test.TestCount.ToString();
                lblTestsPassed.Text = "0";
                lblTestsFailed.Text = "0";
                lblTestsNotRun.Text = lblTestsTotal.Text;

                EnableButtons(true);
            }            
        }

        private void DisplayTestItem(ITestItem testItem)
        {
            if (testItem is TestGroup)
                DisplayTestGroup(testItem as TestGroup);
            else if (testItem is UnitTest)
                DisplayUnitTest(testItem as UnitTest);
        }

        private void DisplayTestGroup(TestGroup testGroup)
        {
            foreach (var testItem in testGroup.Items)
                DisplayTestItem(testItem);
        }

        private void DisplayUnitTest(UnitTest unitTest)
        {
            var group = lsvTests.Groups.Add(unitTest.Id.ToString(), unitTest.Name + " - " + unitTest.Description);
            foreach (var testCase in unitTest.TestCases)
            {
                var newItem = lsvTests.Items.Add(testCase.Name);
                newItem.Tag = testCase;
                newItem.Group = group;
                newItem.SubItems.Add(testCase.Description);
                newItem.SubItems.Add(testCase.TestType.ToString());
                newItem.SubItems.Add("");
            }
        }

        private async void btnRunAll_Click(object sender, EventArgs e)
        {
            EnableButtons(false);

            lblTestsTotal.Text = _TestSuite.Test.TestCount.ToString();
            lblTestsPassed.Text = "0";
            lblTestsFailed.Text = "0";
            lblTestsNotRun.Text = lblTestsTotal.Text;

            foreach (ListViewItem item in lsvTests.Items)
                item.SubItems[3].Text = "";
           
            _Variables["SERVER"] = txtServer.Text;
            _Variables["FILELIBRARY"] = txtFileLibrary.Text;
            _Variables["USER"] = txtUser.Text;
            _Variables["PASSWORD"] = txtPassword.Text;           

            try
            {
                _CancellationTokenSource = new CancellationTokenSource();        
                Progress<TestProgress> progress = new Progress<TestProgress>(OnTestRunProgress);

                var testRunOutputFolder = Path.Combine(_OutputFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                Directory.CreateDirectory(testRunOutputFolder);

                var result = await _TestSuite.RunAllAsync(_Variables, testRunOutputFolder, _CancellationTokenSource.Token, progress);
            }
            catch (OperationCanceledException)
            {
                // User cancelled processing

            } 

            EnableButtons(true);
        }

        private void OnTestRunProgress(TestProgress progress)
        {
            string[] resultDescriptions = new string[5] {"Not Run", "In Progress", "Setup Failed", "Passed", "Failed"};

            foreach (ListViewItem item in lsvTests.Items)
            {
                if (((TestCase)item.Tag).Id == progress.Id)
                {
                    item.SubItems[3].Text = resultDescriptions[(int)progress.Result];

                    if ((progress.Result == TestResult.Failed) || (progress.Result == TestResult.SetupFailed))
                    {
                        lblTestsFailed.Text = (int.Parse(lblTestsFailed.Text) + 1).ToString();
                        lblTestsNotRun.Text = (int.Parse(lblTestsNotRun.Text) - 1).ToString();
                    }
                    else if (progress.Result == TestResult.Passed)
                    {
                        lblTestsPassed.Text = (int.Parse(lblTestsPassed.Text) + 1).ToString();
                        lblTestsNotRun.Text = (int.Parse(lblTestsNotRun.Text) - 1).ToString();
                    }
                }
            }           
        } 

        private void btnStop_Click(object sender, EventArgs e)
        {
            _CancellationTokenSource.Cancel();
        }

        private void lsvTests_DoubleClick(object sender, EventArgs e)
        {
            if (lsvTests.FocusedItem != null)
            {
                var testCase = lsvTests.FocusedItem.Tag as TestCase;

                var testCaseForm = new TestCaseForm();
                testCaseForm.ShowUnitTest(testCase);

            }
        }

        private void btnRunSingle_Click(object sender, EventArgs e)
        {
            if (lsvTests.FocusedItem != null)
            {
                var testCase = lsvTests.FocusedItem.Tag as TestCase;

                _Variables["SERVER"] = txtServer.Text;
                _Variables["FILELIBRARY"] = txtFileLibrary.Text;
                _Variables["USER"] = txtUser.Text;
                _Variables["PASSWORD"] = txtPassword.Text;  

                var testCaseForm = new TestCaseForm();
                testCaseForm.RunUnitTest(testCase, _OutputFolder, _Variables);                
            }
        }

        private void EnableButtons(bool ifStopped)
        {
            btnRunAll.Enabled = ifStopped;
            btnRunSingle.Enabled = ifStopped;
            btnSaveOutput.Enabled = ifStopped;
            btnStop.Enabled = !ifStopped; 
        }

        private void btnSaveOutput_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xml";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var resultWriter = new JUnitTestResultWriter();
                resultWriter.WriteResults(_TestSuite, _Variables, dialog.FileName);
            }       
        }

        private void btnRunSelected_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not supported");
        }
    }

    class TestOutputRecord
    {
        public string Test { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Result { get; set; }
        public Guid BodId { get; set; }
        public string PrecedaId { get; set; }
        public string ExpectedStage { get; set; }
        public string ExpectedStatus { get; set; }
        public string ActualStage { get; set; }
        public string ActualStatus { get; set; }
    }

}
