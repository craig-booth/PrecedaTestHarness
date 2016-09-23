namespace TestRunner
{
    partial class MainForm
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
            this.btnRunAll = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lsvTests = new System.Windows.Forms.ListView();
            this.colTest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.coDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTestType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnStop = new System.Windows.Forms.Button();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtFileLibrary = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRunSingle = new System.Windows.Forms.Button();
            this.btnSaveOutput = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTestsTotal = new System.Windows.Forms.Label();
            this.lblTestsPassed = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.lblTestsFailed = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTestsNotRun = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRunSelected = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRunAll
            // 
            this.btnRunAll.Enabled = false;
            this.btnRunAll.Location = new System.Drawing.Point(315, 82);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(75, 23);
            this.btnRunAll.TabIndex = 5;
            this.btnRunAll.Text = "Run All";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(24, 82);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 4;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lsvTests
            // 
            this.lsvTests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvTests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTest,
            this.coDescription,
            this.colTestType,
            this.colResult});
            this.lsvTests.FullRowSelect = true;
            this.lsvTests.Location = new System.Drawing.Point(0, 111);
            this.lsvTests.Name = "lsvTests";
            this.lsvTests.Size = new System.Drawing.Size(1018, 307);
            this.lsvTests.TabIndex = 2;
            this.lsvTests.UseCompatibleStateImageBehavior = false;
            this.lsvTests.View = System.Windows.Forms.View.Details;
            this.lsvTests.DoubleClick += new System.EventHandler(this.lsvTests_DoubleClick);
            // 
            // colTest
            // 
            this.colTest.Text = "Test";
            this.colTest.Width = 179;
            // 
            // coDescription
            // 
            this.coDescription.Text = "Description";
            this.coDescription.Width = 600;
            // 
            // colTestType
            // 
            this.colTestType.Text = "Test Type";
            this.colTestType.Width = 96;
            // 
            // colResult
            // 
            this.colResult.Text = "Result";
            this.colResult.Width = 82;
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(802, 82);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtUser
            // 
            this.txtUser.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUser.Location = new System.Drawing.Point(73, 39);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 20);
            this.txtUser.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPassword.Location = new System.Drawing.Point(283, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // txtServer
            // 
            this.txtServer.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtServer.Location = new System.Drawing.Point(73, 13);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(100, 20);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "DEV3";
            // 
            // txtFileLibrary
            // 
            this.txtFileLibrary.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtFileLibrary.Location = new System.Drawing.Point(283, 13);
            this.txtFileLibrary.Name = "txtFileLibrary";
            this.txtFileLibrary.Size = new System.Drawing.Size(100, 20);
            this.txtFileLibrary.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Server";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(219, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "File Library";
            // 
            // btnRunSingle
            // 
            this.btnRunSingle.Enabled = false;
            this.btnRunSingle.Location = new System.Drawing.Point(603, 82);
            this.btnRunSingle.Name = "btnRunSingle";
            this.btnRunSingle.Size = new System.Drawing.Size(75, 23);
            this.btnRunSingle.TabIndex = 12;
            this.btnRunSingle.Text = "Run Single";
            this.btnRunSingle.UseVisualStyleBackColor = true;
            this.btnRunSingle.Click += new System.EventHandler(this.btnRunSingle_Click);
            // 
            // btnSaveOutput
            // 
            this.btnSaveOutput.Enabled = false;
            this.btnSaveOutput.Location = new System.Drawing.Point(142, 82);
            this.btnSaveOutput.Name = "btnSaveOutput";
            this.btnSaveOutput.Size = new System.Drawing.Size(75, 23);
            this.btnSaveOutput.TabIndex = 13;
            this.btnSaveOutput.Text = "Save Output";
            this.btnSaveOutput.UseVisualStyleBackColor = true;
            this.btnSaveOutput.Click += new System.EventHandler(this.btnSaveOutput_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(459, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Total";
            // 
            // lblTestsTotal
            // 
            this.lblTestsTotal.AutoSize = true;
            this.lblTestsTotal.Location = new System.Drawing.Point(496, 20);
            this.lblTestsTotal.Name = "lblTestsTotal";
            this.lblTestsTotal.Size = new System.Drawing.Size(13, 13);
            this.lblTestsTotal.TabIndex = 15;
            this.lblTestsTotal.Text = "0";
            // 
            // lblTestsPassed
            // 
            this.lblTestsPassed.AutoSize = true;
            this.lblTestsPassed.Location = new System.Drawing.Point(600, 20);
            this.lblTestsPassed.Name = "lblTestsPassed";
            this.lblTestsPassed.Size = new System.Drawing.Size(13, 13);
            this.lblTestsPassed.TabIndex = 17;
            this.lblTestsPassed.Text = "0";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(549, 20);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(42, 13);
            this.Label6.TabIndex = 16;
            this.Label6.Text = "Passed";
            // 
            // lblTestsFailed
            // 
            this.lblTestsFailed.AutoSize = true;
            this.lblTestsFailed.Location = new System.Drawing.Point(714, 20);
            this.lblTestsFailed.Name = "lblTestsFailed";
            this.lblTestsFailed.Size = new System.Drawing.Size(13, 13);
            this.lblTestsFailed.TabIndex = 19;
            this.lblTestsFailed.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(673, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Failed";
            // 
            // lblTestsNotRun
            // 
            this.lblTestsNotRun.AutoSize = true;
            this.lblTestsNotRun.Location = new System.Drawing.Point(851, 20);
            this.lblTestsNotRun.Name = "lblTestsNotRun";
            this.lblTestsNotRun.Size = new System.Drawing.Size(13, 13);
            this.lblTestsNotRun.TabIndex = 21;
            this.lblTestsNotRun.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(783, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Not Run";
            // 
            // btnRunSelected
            // 
            this.btnRunSelected.Enabled = false;
            this.btnRunSelected.Location = new System.Drawing.Point(396, 82);
            this.btnRunSelected.Name = "btnRunSelected";
            this.btnRunSelected.Size = new System.Drawing.Size(94, 23);
            this.btnRunSelected.TabIndex = 22;
            this.btnRunSelected.Text = "Run Selected";
            this.btnRunSelected.UseVisualStyleBackColor = true;
            this.btnRunSelected.Click += new System.EventHandler(this.btnRunSelected_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 422);
            this.Controls.Add(this.btnRunSelected);
            this.Controls.Add(this.lblTestsNotRun);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblTestsFailed);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblTestsPassed);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.lblTestsTotal);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnSaveOutput);
            this.Controls.Add(this.btnRunSingle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileLibrary);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lsvTests);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnRunAll);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunAll;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ListView lsvTests;
        private System.Windows.Forms.ColumnHeader colTest;
        private System.Windows.Forms.ColumnHeader coDescription;
        private System.Windows.Forms.ColumnHeader colTestType;
        private System.Windows.Forms.ColumnHeader colResult;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtFileLibrary;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRunSingle;
        private System.Windows.Forms.Button btnSaveOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTestsTotal;
        private System.Windows.Forms.Label lblTestsPassed;
        private System.Windows.Forms.Label Label6;
        private System.Windows.Forms.Label lblTestsFailed;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTestsNotRun;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnRunSelected;
    }
}

