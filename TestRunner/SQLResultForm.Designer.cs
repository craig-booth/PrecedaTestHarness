namespace TestRunner
{
    partial class SQLResultForm
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
            this.btnViewActualData = new System.Windows.Forms.Button();
            this.btnViewExpectedData = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCompareData = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnViewActualData
            // 
            this.btnViewActualData.Location = new System.Drawing.Point(208, 35);
            this.btnViewActualData.Name = "btnViewActualData";
            this.btnViewActualData.Size = new System.Drawing.Size(75, 23);
            this.btnViewActualData.TabIndex = 43;
            this.btnViewActualData.Text = "View";
            this.btnViewActualData.UseVisualStyleBackColor = true;
            this.btnViewActualData.Click += new System.EventHandler(this.btnViewActualData_Click);
            // 
            // btnViewExpectedData
            // 
            this.btnViewExpectedData.Location = new System.Drawing.Point(118, 35);
            this.btnViewExpectedData.Name = "btnViewExpectedData";
            this.btnViewExpectedData.Size = new System.Drawing.Size(75, 23);
            this.btnViewExpectedData.TabIndex = 42;
            this.btnViewExpectedData.Text = "View";
            this.btnViewExpectedData.UseVisualStyleBackColor = true;
            this.btnViewExpectedData.Click += new System.EventHandler(this.btnViewExpectedData_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Data";
            // 
            // btnCompareData
            // 
            this.btnCompareData.Location = new System.Drawing.Point(155, 75);
            this.btnCompareData.Name = "btnCompareData";
            this.btnCompareData.Size = new System.Drawing.Size(75, 23);
            this.btnCompareData.TabIndex = 40;
            this.btnCompareData.Text = "Compare";
            this.btnCompareData.UseVisualStyleBackColor = true;
            this.btnCompareData.Click += new System.EventHandler(this.btnCompareData_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(233, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 39;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Actual";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Expected";
            // 
            // SQLResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 175);
            this.Controls.Add(this.btnViewActualData);
            this.Controls.Add(this.btnViewExpectedData);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnCompareData);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SQLResultForm";
            this.Text = "SQLResultForm";
            this.Shown += new System.EventHandler(this.SQLResultForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnViewActualData;
        private System.Windows.Forms.Button btnViewExpectedData;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnCompareData;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}