namespace TestRunner
{
    partial class XmlTransformResultForm
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
            this.btnViewActualResult = new System.Windows.Forms.Button();
            this.btnViewExpectedResult = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCompareResult = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblActualError = new System.Windows.Forms.Label();
            this.lblExpectedError = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnViewActualResult
            // 
            this.btnViewActualResult.Location = new System.Drawing.Point(256, 103);
            this.btnViewActualResult.Name = "btnViewActualResult";
            this.btnViewActualResult.Size = new System.Drawing.Size(75, 23);
            this.btnViewActualResult.TabIndex = 43;
            this.btnViewActualResult.Text = "View";
            this.btnViewActualResult.UseVisualStyleBackColor = true;
            this.btnViewActualResult.Click += new System.EventHandler(this.btnViewActualResult_Click);
            // 
            // btnViewExpectedResult
            // 
            this.btnViewExpectedResult.Location = new System.Drawing.Point(163, 103);
            this.btnViewExpectedResult.Name = "btnViewExpectedResult";
            this.btnViewExpectedResult.Size = new System.Drawing.Size(75, 23);
            this.btnViewExpectedResult.TabIndex = 42;
            this.btnViewExpectedResult.Text = "View";
            this.btnViewExpectedResult.UseVisualStyleBackColor = true;
            this.btnViewExpectedResult.Click += new System.EventHandler(this.btnViewExpectedResult_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "Result File";
            // 
            // btnCompareResult
            // 
            this.btnCompareResult.Location = new System.Drawing.Point(206, 132);
            this.btnCompareResult.Name = "btnCompareResult";
            this.btnCompareResult.Size = new System.Drawing.Size(75, 23);
            this.btnCompareResult.TabIndex = 40;
            this.btnCompareResult.Text = "Compare";
            this.btnCompareResult.UseVisualStyleBackColor = true;
            this.btnCompareResult.Click += new System.EventHandler(this.btnCompareResult_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(367, 184);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 39;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // lblActualError
            // 
            this.lblActualError.AutoSize = true;
            this.lblActualError.Location = new System.Drawing.Point(95, 47);
            this.lblActualError.Name = "lblActualError";
            this.lblActualError.Size = new System.Drawing.Size(13, 13);
            this.lblActualError.TabIndex = 35;
            this.lblActualError.Text = "0";
            // 
            // lblExpectedError
            // 
            this.lblExpectedError.AutoSize = true;
            this.lblExpectedError.Location = new System.Drawing.Point(95, 15);
            this.lblExpectedError.Name = "lblExpectedError";
            this.lblExpectedError.Size = new System.Drawing.Size(13, 13);
            this.lblExpectedError.TabIndex = 29;
            this.lblExpectedError.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Actual Error";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Expected Error";
            // 
            // XmlTransformResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 219);
            this.Controls.Add(this.btnViewActualResult);
            this.Controls.Add(this.btnViewExpectedResult);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnCompareResult);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblActualError);
            this.Controls.Add(this.lblExpectedError);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "XmlTransformResultForm";
            this.Text = "XmlTransformResultForm";
            this.Shown += new System.EventHandler(this.XmlTransformResultForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnViewActualResult;
        private System.Windows.Forms.Button btnViewExpectedResult;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnCompareResult;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblActualError;
        private System.Windows.Forms.Label lblExpectedError;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}