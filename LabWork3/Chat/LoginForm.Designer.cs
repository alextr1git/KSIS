using System.ComponentModel;

namespace Chat;

partial class LoginForm
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
        this.SuspendLayout();
        // 
        // txtUsername
        // 
        this.txtUsername.Location = new System.Drawing.Point(23, 106);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(604, 38);
        this.txtUsername.TabIndex = 0;
        // 
        // BntEnter
        // 
        this.BntEnter.Location = new System.Drawing.Point(658, 98);
        this.BntEnter.Name = "BntEnter";
        this.BntEnter.Size = new System.Drawing.Size(130, 56);
        this.BntEnter.TabIndex = 1;
        this.BntEnter.Text = "Enter";
        this.BntEnter.UseVisualStyleBackColor = true;
        this.BntEnter.Click += new System.EventHandler(this.BntEnter_Click);
        // 
        // label1
        // 
        this.label1.Location = new System.Drawing.Point(242, 34);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(246, 69);
        this.label1.TabIndex = 2;
        this.label1.Text = "Enter username";
        // 
        // LoginForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 167);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.BntEnter);
        this.Controls.Add(this.txtUsername);
        this.Name = "LoginForm";
        this.Text = "LoginForm";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.Button BntEnter;
    private System.Windows.Forms.Label label1;

    #endregion
}