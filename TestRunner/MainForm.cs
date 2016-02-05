using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using CsvHelper;

using TestHarness;

namespace TestRunner
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource _CancellationTokenSource;
        private TestSuite _TestSuite;

        private string _OutputFolder;

        public MainForm()
        {
            InitializeComponent();

            _OutputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PrecedaTestHarness");
            Directory.CreateDirectory(_OutputFolder);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            lsvTests.Items.Clear();

            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _TestSuite = new TestSuite(dialog.FileName);

                lsvTests.Items.Clear();
                foreach (UnitTest unitTest in _TestSuite.UnitTests)
                { 
                    var newItem = lsvTests.Items.Add(unitTest.Name);
                    newItem.SubItems.Add(unitTest.Description);
                    newItem.SubItems.Add(unitTest.TestType.ToString());
                    newItem.SubItems.Add("");

                    newItem.Tag = unitTest;
                }

                EnableButtons(true);
            }            
        }

        private async void btnRunAll_Click(object sender, EventArgs e)
        {
            EnableButtons(false);

            lblTestsTotal.Text = _TestSuite.UnitTests.Count.ToString();
            lblTestsPassed.Text = "0";
            lblTestsFailed.Text = "0";
            lblTestsNotRun.Text = _TestSuite.UnitTests.Count.ToString();

            foreach (ListViewItem item in lsvTests.Items)
                item.SubItems[3].Text = "";

            var variables = new Dictionary<string, string>();            
            variables["SERVER"] = txtServer.Text;
            variables["FILELIBRARY"] = txtFileLibrary.Text;
            variables["USER"] = txtUser.Text;
            variables["PASSWORD"] = txtPassword.Text;

            _CancellationTokenSource = new CancellationTokenSource();        
            Progress<TestRunProgress> progress = new Progress<TestRunProgress>(OnTestRunProgress);
         
            try
            {
                var outputFolder = Path.Combine(_OutputFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                Directory.CreateDirectory(outputFolder);

                var result = await _TestSuite.RunAllAsync(variables, outputFolder, _CancellationTokenSource.Token, progress);
            }
            catch (OperationCanceledException)
            {
                // User cancelled processing
                
            }

            EnableButtons(true);
        }

        private void OnTestRunProgress(TestRunProgress progress)
        {
          string[] resultDescriptions = new string[5] {"Not Run", "In Progress", "Setup Failed", "Passed", "Failed"};

            foreach (ListViewItem item in lsvTests.Items)
            {
                if (item.Text == progress.TestName)
                {
                    item.SubItems[3].Text = resultDescriptions[(int)progress.TestResult];
                    break;
                }
            }

            if ((progress.TestResult == TestResult.Failed) || (progress.TestResult == TestResult.SetupFailed))
            {
                lblTestsFailed.Text = (int.Parse(lblTestsFailed.Text) + 1).ToString();
                lblTestsNotRun.Text = (int.Parse(lblTestsNotRun.Text) - 1).ToString();
            } 
            else if (progress.TestResult == TestResult.Passed)
            {
                lblTestsPassed.Text = (int.Parse(lblTestsPassed.Text) + 1).ToString();
                lblTestsNotRun.Text = (int.Parse(lblTestsNotRun.Text) - 1).ToString();
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
                var unitTest = lsvTests.FocusedItem.Tag as UnitTest;

                var unitTestForm = new UnitTestForm();
                unitTestForm.ShowUnitTest(unitTest);

            }
        }

        private void btnRunSingle_Click(object sender, EventArgs e)
        {
            if (lsvTests.FocusedItem != null)
            {
                var unitTest = lsvTests.FocusedItem.Tag as UnitTest;

                var variables = new Dictionary<string, string>();
                variables["SERVER"] = txtServer.Text;
                variables["FILELIBRARY"] = txtFileLibrary.Text;
                variables["USER"] = txtUser.Text;
                variables["PASSWORD"] = txtPassword.Text;  


                var unitTestForm = new UnitTestForm();
                unitTestForm.RunUnitTest(unitTest, variables);                
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
            dialog.DefaultExt = ".csv";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var streamWriter = new StreamWriter(dialog.FileName , false);
                var csvWriter = new CsvWriter(streamWriter);

                csvWriter.WriteHeader<TestOutputRecord>();
                foreach(var unitTest in _TestSuite.UnitTests)
                {
                    var outputRecord = new TestOutputRecord();

                    outputRecord.Test = unitTest.Name;
                    outputRecord.Description = unitTest.Description;
                    outputRecord.Type = unitTest.TestType.ToString();
                    outputRecord.Result = unitTest.Result.ToString();

                    PayrollExchangeUploadBodTask task;
                    if (unitTest.Result == TestResult.SetupFailed) 
                        task = unitTest.SetupTasks[0] as PayrollExchangeUploadBodTask;
                    else
                        task = unitTest.TestTasks[0] as PayrollExchangeUploadBodTask;

                    outputRecord.BodId = task.BodId;
                    outputRecord.PrecedaId = task.PrecedaId;
                    outputRecord.ExpectedStage = task.ExpectedResult.ProcessingStage;
                    outputRecord.ExpectedStatus = task.ExpectedResult.Status;
                    if (task.ActualResult != null)
                    {
                        outputRecord.ActualStage = task.ActualResult.ProcessingStage;
                        outputRecord.ActualStatus = task.ActualResult.Status;
                    }
                    else
                    {
                        outputRecord.ActualStage = "";
                        outputRecord.ActualStatus = "";
                    }


                    csvWriter.WriteRecord(outputRecord);
                }

                streamWriter.Close();
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
