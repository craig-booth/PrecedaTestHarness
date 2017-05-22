namespace TestRunner
{
    partial class TestCaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Setup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Test", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Tear Down", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestCaseForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTestType = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.lsvUnitTest = new System.Windows.Forms.ListView();
            this.colTask = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(89, 25);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "label3";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(89, 42);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(35, 13);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Test Type";
            // 
            // lblTestType
            // 
            this.lblTestType.AutoSize = true;
            this.lblTestType.Location = new System.Drawing.Point(89, 59);
            this.lblTestType.Name = "lblTestType";
            this.lblTestType.Size = new System.Drawing.Size(35, 13);
            this.lblTestType.TabIndex = 5;
            this.lblTestType.Text = "label6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 75);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Result";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(89, 75);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(35, 13);
            this.lblResult.TabIndex = 7;
            this.lblResult.Text = "label8";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(433, 95);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(337, 95);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 10;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // lsvUnitTest
            // 
            this.lsvUnitTest.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTask,
            this.colMessage});
            this.lsvUnitTest.FullRowSelect = true;
            listViewGroup1.Header = "Setup";
            listViewGroup1.Name = "lvgSetup";
            listViewGroup2.Header = "Test";
            listViewGroup2.Name = "lvgTest";
            listViewGroup3.Header = "Tear Down";
            listViewGroup3.Name = "lvgTearDown";
            this.lsvUnitTest.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lsvUnitTest.Location = new System.Drawing.Point(0, 124);
            this.lsvUnitTest.Name = "lsvUnitTest";
            this.lsvUnitTest.ShowItemToolTips = true;
            this.lsvUnitTest.Size = new System.Drawing.Size(818, 310);
            this.lsvUnitTest.SmallImageList = this.imageList1;
            this.lsvUnitTest.TabIndex = 11;
            this.lsvUnitTest.UseCompatibleStateImageBehavior = false;
            this.lsvUnitTest.View = System.Windows.Forms.View.Details;
            this.lsvUnitTest.DoubleClick += new System.EventHandler(this.lsvUnitTest_DoubleClick);
            // 
            // colTask
            // 
            this.colTask.Text = "Task";
            this.colTask.Width = 156;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 648;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Failed");
            this.imageList1.Images.SetKeyName(1, "SetupFailed");
            this.imageList1.Images.SetKeyName(2, "Success");
            // 
            // UnitTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 435);
            this.Controls.Add(this.lsvUnitTest);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblTestType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "UnitTestForm";
            this.Text = "UnitTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTestType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ListView lsvUnitTest;
        private System.Windows.Forms.ColumnHeader colTask;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.ImageList imageList1;
    }
}