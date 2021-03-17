namespace myclient
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.rctMessages = new System.Windows.Forms.RichTextBox();
            this.cmdSet = new System.Windows.Forms.Button();
            this.txtNewMsg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdSend = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rctMessages
            // 
            this.rctMessages.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.rctMessages.Location = new System.Drawing.Point(13, 119);
            this.rctMessages.Margin = new System.Windows.Forms.Padding(4);
            this.rctMessages.Name = "rctMessages";
            this.rctMessages.Size = new System.Drawing.Size(390, 213);
            this.rctMessages.TabIndex = 0;
            this.rctMessages.Text = "";
            // 
            // cmdSet
            // 
            this.cmdSet.Location = new System.Drawing.Point(301, 13);
            this.cmdSet.Margin = new System.Windows.Forms.Padding(4);
            this.cmdSet.Name = "cmdSet";
            this.cmdSet.Size = new System.Drawing.Size(102, 60);
            this.cmdSet.TabIndex = 1;
            this.cmdSet.Text = "Network Settings";
            this.cmdSet.UseVisualStyleBackColor = true;
            this.cmdSet.Click += new System.EventHandler(this.cmdSet_Click);
            // 
            // txtNewMsg
            // 
            this.txtNewMsg.Location = new System.Drawing.Point(71, 53);
            this.txtNewMsg.Name = "txtNewMsg";
            this.txtNewMsg.Size = new System.Drawing.Size(221, 24);
            this.txtNewMsg.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(13, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name:";
            // 
            // cmdSend
            // 
            this.cmdSend.Location = new System.Drawing.Point(301, 83);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(102, 32);
            this.cmdSend.TabIndex = 5;
            this.cmdSend.Text = "Send";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Location = new System.Drawing.Point(16, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(95, 22);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "Output log";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(13, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 18);
            this.label2.TabIndex = 8;
            this.label2.Text = "Location:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(84, 91);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(208, 24);
            this.txtLocation.TabIndex = 7;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::myclient.Properties.Resources.angryimg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(416, 345);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cmdSend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNewMsg);
            this.Controls.Add(this.cmdSet);
            this.Controls.Add(this.rctMessages);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "KClient";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button cmdSet;
        private System.Windows.Forms.TextBox txtNewMsg;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RichTextBox rctMessages;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLocation;
    }
}

