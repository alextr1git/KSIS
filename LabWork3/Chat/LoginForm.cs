using System;
using System.Windows.Forms;

namespace Chat;

public partial class LoginForm : Form
{
    string _userinfo = "";
    
    public string UserName
    {
        get { return _userinfo; }
    }
    public LoginForm()
    {
        InitializeComponent();
        //FormClosing += LoginForm_FormClosing;
        BntEnter.Click += BntEnter_Click;
    }

    private void BntEnter_Click(object sender, EventArgs e)
    {
        _userinfo = txtUsername.Text.Trim();
        if (string.IsNullOrEmpty(_userinfo))
        {
            MessageBox.Show("Please enter a username");
            return;
        }
        //FormClosing -= LoginForm_FormClosing;
        Close();
    }
    
}