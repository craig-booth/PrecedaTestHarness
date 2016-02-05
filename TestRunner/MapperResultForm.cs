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
        private MapperTask _Task;

        public MapperResultForm()
        {
            InitializeComponent();
        }
        
        public MapperTask Task
        {
            set
            {
                _Task = value;

                lblExpectedAdded.Text = _Task.ExpectedResult.RecordsAdded.ToString();
                lblExpectedUpdated.Text = _Task.ExpectedResult.RecordsUpdated.ToString();
                lblExpectedDeleted.Text = _Task.ExpectedResult.RecordsDeleted.ToString();
                lblExpectedFailed.Text = _Task.ExpectedResult.RecordsFailed.ToString();
                lblExpectedTotal.Text = _Task.ExpectedResult.RecordsTotal.ToString();


                lblActualAdded.Text = _Task.ActualResult.RecordsAdded.ToString();
                if (_Task.ActualResult.RecordsAdded != _Task.ExpectedResult.RecordsAdded)
                    lblActualAdded.ForeColor = Color.Red;

                lblActualUpdated.Text = _Task.ActualResult.RecordsUpdated.ToString();
                if (_Task.ActualResult.RecordsUpdated != _Task.ExpectedResult.RecordsUpdated)
                    lblActualUpdated.ForeColor = Color.Red;

                lblActualDeleted.Text = _Task.ActualResult.RecordsDeleted.ToString();
                if (_Task.ActualResult.RecordsDeleted != _Task.ExpectedResult.RecordsDeleted)
                    lblActualDeleted.ForeColor = Color.Red;

                lblActualFailed.Text = _Task.ActualResult.RecordsFailed.ToString();
                if (_Task.ActualResult.RecordsFailed != _Task.ExpectedResult.RecordsFailed)
                    lblActualFailed.ForeColor = Color.Red;

                lblActualTotal.Text = _Task.ActualResult.RecordsTotal.ToString();
                if (_Task.ActualResult.RecordsTotal != _Task.ExpectedResult.RecordsTotal)
                    lblActualTotal.ForeColor = Color.Red;

                btnViewExpectedErrors.Enabled = (_Task.ExpectedResult.ErrorFile != "");
                btnViewActualErrors.Enabled = (_Task.ActualResult.ErrorFile != "");
                btnCompareErrors.Enabled = (_Task.ExpectedResult.ErrorFile != "") && (_Task.ActualResult.ErrorFile != "");
            }
        }

        private void btnCompareErrors_Click(object sender, EventArgs e)
        {
            ProcessStartInfo winMerge = new ProcessStartInfo();
            winMerge.Arguments = String.Format("/e /u /dl \"Expected\" /wr /dr \"Actual\" \"{0}\" \"{1}\"", _Task.ExpectedResult.ErrorFile, _Task.ActualResult.ErrorFile);
            winMerge.FileName = Path.Combine(Application.StartupPath, "WinMerge", "WinMergeU.exe");
            winMerge.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(winMerge);
        }

        private void btnViewExpectedErrors_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = _Task.ExpectedResult.ErrorFile;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

        private void btnViewActualErrors_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = _Task.ActualResult.ErrorFile;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
