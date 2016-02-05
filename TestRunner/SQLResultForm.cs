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
        private SQLTask _Task;

        public SQLResultForm()
        {
            InitializeComponent();
        }

        public SQLTask Task
        {
            set
            {
                _Task = value;

                btnViewExpectedData.Enabled = (_Task.ExpectedResult.DataFileName != "");
                btnViewActualData.Enabled = (_Task.ActualResult.DataFileName != "");
                btnCompareData.Enabled = (_Task.ExpectedResult.DataFileName != "") && (_Task.ActualResult.DataFileName != "");
            }
        }

        private void btnViewExpectedData_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = _Task.ExpectedResult.DataFileName;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

        private void btnViewActualData_Click(object sender, EventArgs e)
        {
            ProcessStartInfo notePad = new ProcessStartInfo();
            notePad.Arguments = _Task.ActualResult.DataFileName;
            notePad.FileName = Path.Combine(Environment.GetEnvironmentVariable("windir"), "system32", "notepad.exe");
            notePad.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(notePad);
        }

        private void btnCompareData_Click(object sender, EventArgs e)
        {
            ProcessStartInfo winMerge = new ProcessStartInfo();
            winMerge.Arguments = String.Format("/e /u /dl \"Expected\" /wr /dr \"Actual\" \"{0}\" \"{1}\"", _Task.ExpectedResult.DataFileName, _Task.ActualResult.DataFileName);
            winMerge.FileName = Path.Combine(Application.StartupPath, "WinMerge", "WinMergeU.exe");
            winMerge.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(winMerge);
        }
    }
}
