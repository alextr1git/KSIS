using System.ComponentModel;

namespace Chat;

partial class Authorization
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.BntEnter = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bexit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(13, 76);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(342, 26);
            this.txtUsername.TabIndex = 0;
            // 
            // BntEnter
            // 
            this.BntEnter.Location = new System.Drawing.Point(487, 76);
            this.BntEnter.Margin = new System.Windows.Forms.Padding(2);
            this.BntEnter.Name = "BntEnter";
            this.BntEnter.Size = new System.Drawing.Size(112, 44);
            this.BntEnter.TabIndex = 1;
            this.BntEnter.Text = "Enter";
            this.BntEnter.UseVisualStyleBackColor = true;
            this.BntEnter.Click += new System.EventHandler(this.BntEnter_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(13, 154);
            this.tbIP.Margin = new System.Windows.Forms.Padding(2);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(342, 26);
            this.tbIP.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 128);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "IP address";
            // 
            // bexit
            // 
            this.bexit.Location = new System.Drawing.Point(487, 136);
            this.bexit.Margin = new System.Windows.Forms.Padding(2);
            this.bexit.Name = "bexit";
            this.bexit.Size = new System.Drawing.Size(112, 44);
            this.bexit.TabIndex = 5;
            this.bexit.Text = "Exit";
            this.bexit.UseVisualStyleBackColor = true;
            this.bexit.Click += new System.EventHandler(this.bexit_Click);
            // 
            // Authorization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(675, 283);
            this.Controls.Add(this.bexit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BntEnter);
            this.Controls.Add(this.txtUsername);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Authorization";
            this.Text = "Sign Up";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Button BntEnter;
    private System.Windows.Forms.Label label1;

    #endregion

    private System.Windows.Forms.TextBox tbIP;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button bexit;
}