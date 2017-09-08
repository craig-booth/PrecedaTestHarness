using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using TestHarness;

namespace TestRunner
{
    public partial class XmlTransformResultForm : Form
    {
        public TestCase TestCase { get; private set; }
        public XmlTransformTask Task { get; private set; }

        public XmlTransformResultForm()
        {
            InitializeComponent();
        }

        public XmlTransformResultForm(TestCase testCase, XmlTransformTask task)
            : this()
        {
            TestCase = testCase;
            Task = task;
        }

        private void XmlTransformResultForm_Shown(object sender, EventArgs e)
        {
            lblExpectedError.Text = Task.ExpectedResult.Error;
            lblActualError.Text = Task.ActualResult.Error;
            if (Task.ActualResult.Error != Task.ExpectedResult.Error)
                lblActualError.ForeColor = Color.Red;

            btnViewExpectedResult.Enabled = (Task.ExpectedResult.ResultFile != "");
            btnViewActualResult.Enabled = (Task.ActualResult.ResultFile != "");
            btnCompareResult.Enabled = (Task.ExpectedResult.ResultFile != "") && (Task.ActualResult.ResultFile != "");
        }

        private void btnViewExpectedResult_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = Task.ExpectedResult.ResultFile;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad); 
        }

        private void btnViewActualResult_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = Task.ExpectedResult.ResultFile;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad); 
        }

        private void btnCompareResult_Click(object sender, EventArgs e)
        {
            ProcessStartInfo diffMerge = new ProcessStartInfo();
            diffMerge.Arguments = String.Format("-caption=\"{0}\" -t1=Actual -t2=Expected \"{1}\" \"{2}\"", TestCase.Name, Task.ActualResult.ResultFile, Task.ExpectedResult.ResultFile);
            diffMerge.FileName = Path.Combine(Application.StartupPath, "DiffMerge", "sgdm.exe");
            diffMerge.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(diffMerge); 
        }

   
    }
}
