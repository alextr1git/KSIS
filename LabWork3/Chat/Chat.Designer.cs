namespace Chat
{
    partial class Chat
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
            this.tbChatWindow = new System.Windows.Forms.RichTextBox();
            this.txtToSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbChatWindow
            // 
            this.tbChatWindow.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tbChatWindow.Location = new System.Drawing.Point(-3, -1);
            this.tbChatWindow.Margin = new System.Windows.Forms.Padding(2);
            this.tbChatWindow.Name = "tbChatWindow";
            this.tbChatWindow.ReadOnly = true;
            this.tbChatWindow.Size = new System.Drawing.Size(620, 643);
            this.tbChatWindow.TabIndex = 0;
            this.tbChatWindow.Text = "";
            // 
            // txtToSend
            // 
            this.txtToSend.Location = new System.Drawing.Point(11, 659);
            this.txtToSend.Margin = new System.Windows.Forms.Padding(2);
            this.txtToSend.Name = "txtToSend";
            this.txtToSend.Size = new System.Drawing.Size(503, 26);
            this.txtToSend.TabIndex = 1;
            this.txtToSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtToSend_KeyDown);
            // 
            // btnSend
            // 
            this.btnSend.Image = global::Chat.Properties.Resources.friends_link_send_share_icon_123609;
            this.btnSend.Location = new System.Drawing.Point(535, 652);
            this.btnSend.Margin = new System.Windows.Forms.Padding(2);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(71, 41);
            this.btnSend.TabIndex = 2;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(617, 704);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtToSend);
            this.Controls.Add(this.tbChatWindow);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Chat";
            this.Text = "Chat Online";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.RichTextBox tbChatWindow;
        private System.Windows.Forms.TextBox txtToSend;
        private System.Windows.Forms.Button btnSend;

        #endregion
    }
}