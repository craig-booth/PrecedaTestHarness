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
    public partial class SQLResultForm : Form
    {

        public UnitTest UnitTest { get; private set; }
        public SQLTask Task { get; private set; }

        private SQLResultForm()
        {
            InitializeComponent();
        }

        public SQLResultForm(UnitTest unitTest, SQLTask task)
            : this()
        {
            UnitTest = unitTest;
            Task = task;
        }

        private void SQLResultForm_Shown(object sender, EventArgs e)
        {
            btnViewExpectedData.Enabled = (Task.ExpectedResult.DataFileName != "");
            btnViewActualData.Enabled = (Task.ActualResult.DataFileName != "");
            btnCompareData.Enabled = (Task.ExpectedResult.DataFileName != "") && (Task.ActualResult.DataFileName != "");
        }

        private void btnViewExpectedData_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = Task.ExpectedResult.DataFileName;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

        private void btnViewActualData_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = Task.ActualResult.DataFileName;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

        private void btnCompareData_Click(object sender, EventArgs e)
        {
            /*  ProcessStartInfo winMerge = new ProcessStartInfo();
              winMerge.Arguments = String.Format("/e /u /dl \"Expected\" /wr /dr \"Actual\" \"{0}\" \"{1}\"", _Task.ExpectedResult.DataFileName, _Task.ActualResult.DataFileName);
              winMerge.FileName = Path.Combine(Application.StartupPath, "WinMerge", "WinMergeU.exe");
              winMerge.WindowStyle = ProcessWindowStyle.Normal;

              Process.Start(winMerge); */

            ProcessStartInfo diffMerge = new ProcessStartInfo();
            diffMerge.Arguments = String.Format("-caption=\"{0}\" -t1=Actual -t2=Expected \"{1}\" \"{2}\"", UnitTest.Name, Task.ActualResult.DataFileName, Task.ExpectedResult.DataFileName);
            diffMerge.FileName = Path.Combine(Application.StartupPath, "DiffMerge", "sgdm.exe");
            diffMerge.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(diffMerge);
        }
    }
}
