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

using TestHarness;

namespace TestRunner
{
    public partial class UnitTestForm : Form
    {
        private UnitTest _UnitTest;
        private Dictionary<string, string> _Variables;

        public UnitTestForm()
        {
            InitializeComponent();
        }

        public void ShowUnitTest(UnitTest unitTest)
        {
            _UnitTest = unitTest;

            btnRefresh.Enabled = true;
            btnRun.Enabled = false;

            UpdateDisplay();

            Show();
        }

        public void RunUnitTest(UnitTest unitTest, Dictionary<string, string> variables)
        {
            _UnitTest = unitTest;
            _Variables = variables;

            btnRefresh.Enabled = false;
            btnRun.Enabled = true;

            UpdateDisplay();

            Show();
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            lblName.Text = _UnitTest.Name;
            lblDescription.Text = _UnitTest.Description;
            lblTestType.Text = _UnitTest.TestType.ToString();
            lblResult.Text = _UnitTest.Result.ToString();
            txtTestOutput.Text = _UnitTest.Output;
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            var progress = new Progress<UnitTestProgress>(OnUnitTestProgress);

            await _UnitTest.RunAsync(_Variables, "", CancellationToken.None, progress);

            UpdateDisplay(); 
        }


        private void OnUnitTestProgress(UnitTestProgress progress)
        {
            lblResult.Text = progress.Result.ToString();
            txtTestOutput.Text = progress.Output;
        } 
    }
}
