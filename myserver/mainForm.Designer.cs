
namespace myserver
{
    partial class mainForm
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lblstatus = new System.Windows.Forms.Label();
            this.cmdStopStart = new System.Windows.Forms.Button();
            this.checkLog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(13, 68);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(365, 208);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // lblstatus
            // 
            this.lblstatus.AutoSize = true;
            this.lblstatus.Location = new System.Drawing.Point(13, 13);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(57, 13);
            this.lblstatus.TabIndex = 1;
            this.lblstatus.Text = "Status: Off";
            // 
            // cmdStopStart
            // 
            this.cmdStopStart.Location = new System.Drawing.Point(13, 34);
            this.cmdStopStart.Name = "cmdStopStart";
            this.cmdStopStart.Size = new System.Drawing.Size(81, 28);
            this.cmdStopStart.TabIndex = 2;
            this.cmdStopStart.Text = "Start server";
            this.cmdStopStart.UseVisualStyleBackColor = true;
            this.cmdStopStart.Click += new System.EventHandler(this.cmdStopStart_Click);
            // 
            // checkLog
            // 
            this.checkLog.AutoSize = true;
            this.checkLog.Location = new System.Drawing.Point(303, 9);
            this.checkLog.Name = "checkLog";
            this.checkLog.Size = new System.Drawing.Size(75, 17);
            this.checkLog.TabIndex = 3;
            this.checkLog.Text = "Output log";
            this.checkLog.UseVisualStyleBackColor = true;
            this.checkLog.CheckedChanged += new System.EventHandler(this.checkLog_CheckedChanged);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.IndianRed;
            this.ClientSize = new System.Drawing.Size(390, 288);
            this.Controls.Add(this.checkLog);
            this.Controls.Add(this.cmdStopStart);
            this.Controls.Add(this.lblstatus);
            this.Controls.Add(this.richTextBox1);
            this.Name = "mainForm";
            this.Text = "mainForm";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lblstatus;
        private System.Windows.Forms.Button cmdStopStart;
        private System.Windows.Forms.CheckBox checkLog;
    }
}

