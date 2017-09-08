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

using TestHarness;

namespace TestRunner
{
    public partial class TestCaseForm : Form
    {
        private TestCase _TestCase;
        private string _OutputFolder;
        private Dictionary<string, string> _Variables;

        public TestCaseForm()
        {
            InitializeComponent();
        }

        public void ShowUnitTest(TestCase testCase)
        {
            _TestCase = testCase;

            btnRefresh.Enabled = true;
            btnRun.Enabled = false;

            LoadTaskDetails();

            Show();
        }

        public void RunUnitTest(TestCase testCase, string outputFolder, Dictionary<string, string> variables)
        {
            _TestCase = testCase;
            _OutputFolder = outputFolder;
            _Variables = variables;

            btnRefresh.Enabled = false;
            btnRun.Enabled = true;

            LoadTaskDetails();

            Show();
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void LoadTaskDetails()
        {
            lblName.Text = _TestCase.Name;
            lblDescription.Text = _TestCase.Description;
            lblTestType.Text = _TestCase.TestType.ToString();
            lblResult.Text = _TestCase.Result.ToString(); 

            lsvUnitTest.Items.Clear();

            foreach (var task in _TestCase.SetupTasks)
            {
                var listItem = new ListViewItem(task.Description);
                listItem.Tag = task;
                listItem.Group = lsvUnitTest.Groups["lvgSetup"];
                lsvUnitTest.Items.Add(listItem);

                UpdateListViewItem(listItem, task);
            }
            foreach (var task in _TestCase.TestTasks)
            {
                var listItem = new ListViewItem(task.Description);
                listItem.Tag = task;
                listItem.Group = lsvUnitTest.Groups["lvgTest"];
                lsvUnitTest.Items.Add(listItem);

                UpdateListViewItem(listItem, task);
            } 
            foreach (var task in _TestCase.TearDownTasks)
            {
                var listItem = new ListViewItem(task.Description);
                listItem.Group = lsvUnitTest.Groups["lvgTearDown"];
                listItem.Tag = task;
                lsvUnitTest.Items.Add(listItem);

                UpdateListViewItem(listItem, task);
            }
        }

        private void UpdateDisplay()
        {
            lblResult.Text = _TestCase.Result.ToString();

            int index = 0;
            foreach (var task in _TestCase.SetupTasks)
                UpdateListViewItem(lsvUnitTest.Items[index++], task);
            foreach (var task in _TestCase.TestTasks)
                UpdateListViewItem(lsvUnitTest.Items[index++], task);
            foreach (var task in _TestCase.TearDownTasks)
                UpdateListViewItem(lsvUnitTest.Items[index++], task); 
        }

        private void UpdateListViewItem(ListViewItem item, ITask task)
        {

            if (item.SubItems.Count >= 2)
                item.SubItems[1].Text = task.Message;
            else
                item.SubItems.Add(task.Message);           

            if (task.Result == TaskResult.Passed)
                item.ImageIndex = 2;
            else if ((task.Result == TaskResult.Failed) || (task.Result == TaskResult.ExceptionOccurred))
                item.ImageIndex = 0;
            else
                item.ImageIndex = -1;
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            var progress = new Progress<TestProgress>(OnTestProgress);

            var testRunOutputFolder = Path.Combine(_OutputFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            Directory.CreateDirectory(testRunOutputFolder);

            await _TestCase.RunAsync(_Variables, testRunOutputFolder, CancellationToken.None, progress);

            UpdateDisplay();  
        }


        private void OnTestProgress(TestProgress progress)
        {
            UpdateDisplay();
        } 

        private void lsvUnitTest_DoubleClick(object sender, EventArgs e)
        {           
            if (lsvUnitTest.SelectedItems.Count == 1)
            {
                var task = lsvUnitTest.FocusedItem.Tag as ITask;

                if (task is MapperTask)
                { 
                    var mapperTask = task as MapperTask;

                    var resultForm = new MapperResultForm(_TestCase, mapperTask);
                    resultForm.ShowDialog();
                }
                else if (task is SQLTask)
                {
                    var sqlTask = task as SQLTask;

                    if (sqlTask.RunMode == SQLRunMode.Query)
                    {
                        var resultForm = new SQLResultForm(_TestCase, sqlTask);
                        resultForm.ShowDialog();
                    }
                }
                else if (task is XmlTransformTask)
                {
                   var transformTask = task as XmlTransformTask;

                   var resultForm = new XmlTransformResultForm(_TestCase, transformTask);
                   resultForm.ShowDialog();
                  
                }

            }
        }
    }
}
