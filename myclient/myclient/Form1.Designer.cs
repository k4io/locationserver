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
            this.cmdConnect = new System.Windows.Forms.Button();
            this.cmdSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rctMessages
            // 
            this.rctMessages.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.rctMessages.Location = new System.Drawing.Point(13, 119);
            this.rctMessages.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rctMessages.Name = "rctMessages";
            this.rctMessages.Size = new System.Drawing.Size(390, 213);
            this.rctMessages.TabIndex = 0;
            this.rctMessages.Text = "";
            // 
            // cmdSet
            // 
            this.cmdSet.Location = new System.Drawing.Point(301, 13);
            this.cmdSet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmdSet.Name = "cmdSet";
            this.cmdSet.Size = new System.Drawing.Size(102, 60);
            this.cmdSet.TabIndex = 1;
            this.cmdSet.Text = "Network Settings";
            this.cmdSet.UseVisualStyleBackColor = true;
            this.cmdSet.Click += new System.EventHandler(this.cmdSet_Click);
            // 
            // txtNewMsg
            // 
            this.txtNewMsg.Location = new System.Drawing.Point(128, 90);
            this.txtNewMsg.Name = "txtNewMsg";
            this.txtNewMsg.Size = new System.Drawing.Size(167, 24);
            this.txtNewMsg.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(13, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter message:";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(16, 13);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(83, 33);
            this.cmdConnect.TabIndex = 4;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // cmdSend
            // 
            this.cmdSend.Location = new System.Drawing.Point(301, 80);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(102, 32);
            this.cmdSend.TabIndex = 5;
            this.cmdSend.Text = "Send";
            this.cmdSend.UseVisualStyleBackColor = true;
            this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::myclient.Properties.Resources.angryimg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(416, 345);
            this.Controls.Add(this.cmdSend);
            this.Controls.Add(this.cmdConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNewMsg);
            this.Controls.Add(this.cmdSet);
            this.Controls.Add(this.rctMessages);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button cmdConnect;
        public System.Windows.Forms.RichTextBox rctMessages;
        private System.Windows.Forms.Button cmdSend;
    }
}

