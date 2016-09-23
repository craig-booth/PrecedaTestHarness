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
    public partial class MapperResultForm : Form
    {
        public UnitTest UnitTest { get; private set; }
        public MapperTask Task { get; private set; }

        private MapperResultForm()
        {
            InitializeComponent();
        }

        public MapperResultForm(UnitTest unitTest, MapperTask task)
            : this()
        {
            UnitTest = unitTest;
            Task = task;
        }

        private void MapperResultForm_Shown(object sender, EventArgs e)
        {
            lblExpectedAdded.Text = Task.ExpectedResult.RecordsAdded.ToString();
            lblExpectedUpdated.Text = Task.ExpectedResult.RecordsUpdated.ToString();
            lblExpectedDeleted.Text = Task.ExpectedResult.RecordsDeleted.ToString();
            lblExpectedFailed.Text = Task.ExpectedResult.RecordsFailed.ToString();
            lblExpectedTotal.Text = Task.ExpectedResult.RecordsTotal.ToString();


            lblActualAdded.Text = Task.ActualResult.RecordsAdded.ToString();
            if (Task.ActualResult.RecordsAdded != Task.ExpectedResult.RecordsAdded)
                lblActualAdded.ForeColor = Color.Red;

            lblActualUpdated.Text = Task.ActualResult.RecordsUpdated.ToString();
            if (Task.ActualResult.RecordsUpdated != Task.ExpectedResult.RecordsUpdated)
                lblActualUpdated.ForeColor = Color.Red;

            lblActualDeleted.Text = Task.ActualResult.RecordsDeleted.ToString();
            if (Task.ActualResult.RecordsDeleted != Task.ExpectedResult.RecordsDeleted)
                lblActualDeleted.ForeColor = Color.Red;

            lblActualFailed.Text = Task.ActualResult.RecordsFailed.ToString();
            if (Task.ActualResult.RecordsFailed != Task.ExpectedResult.RecordsFailed)
                lblActualFailed.ForeColor = Color.Red;

            lblActualTotal.Text = Task.ActualResult.RecordsTotal.ToString();
            if (Task.ActualResult.RecordsTotal != Task.ExpectedResult.RecordsTotal)
                lblActualTotal.ForeColor = Color.Red;

            btnViewExpectedErrors.Enabled = (Task.ExpectedResult.ErrorFile != "");
            btnViewActualErrors.Enabled = (Task.ActualResult.ErrorFile != "");
            btnCompareErrors.Enabled = (Task.ExpectedResult.ErrorFile != "") && (Task.ActualResult.ErrorFile != "");
        }

        private void btnCompareErrors_Click(object sender, EventArgs e)
        {
       /*     ProcessStartInfo winMerge = new ProcessStartInfo();
            winMerge.Arguments = String.Format("/e /u /dl \"Expected\" /wr /dr \"Actual\" \"{0}\" \"{1}\"", _Task.ExpectedResult.ErrorFile, _Task.ActualResult.ErrorFile);
            winMerge.FileName = Path.Combine(Application.StartupPath, "WinMerge", "WinMergeU.exe");
            winMerge.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(winMerge); */

            ProcessStartInfo diffMerge = new ProcessStartInfo();
            diffMerge.Arguments = String.Format("-caption=\"{0}\" -t1=Actual -t2=Expected \"{1}\" \"{2}\"", UnitTest.Name, Task.ActualResult.ErrorFile, Task.ExpectedResult.ErrorFile);
            diffMerge.FileName = Path.Combine(Application.StartupPath, "DiffMerge", "sgdm.exe");
            diffMerge.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(diffMerge);
        }

        private void btnViewExpectedErrors_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = Task.ExpectedResult.ErrorFile;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

        private void btnViewActualErrors_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = Task.ActualResult.ErrorFile;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

    }
}
