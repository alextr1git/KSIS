using System;
using System.Windows.Forms;

namespace Chat;

public partial class LoginForm : Form
{
    string _userinfo = "";
    string _userIP = "";
   
    
    public string UserName
    {
        get { return _userinfo; }
    }

    public string UserIP
    {
        get { return _userIP; }
    }
    public LoginForm()
    {
        InitializeComponent();
    }

    private void BntEnter_Click(object sender, EventArgs e)
    {
        _userinfo = txtUsername.Text.Trim();
        _userIP = tbIP.Text.Trim();
        bool flag = true;
        short counter = 0;

        foreach (char symbol in _userIP) {
            if (symbol == '.')
                counter++;
            if (((int)symbol <= 0) || ((int)symbol >= 9)){
                flag = false;
                break;
            }

        }

        if (counter != 3)
            flag = false;

        if (string.IsNullOrEmpty(_userinfo))
        {
            MessageBox.Show("Please enter a username");
            return;
        }

        if (string.IsNullOrEmpty(_userinfo) || (flag == false))
        {
            MessageBox.Show("Please enter a correct IP");
            tbIP.Text = "";
            return;
        }
        Close();
    }

    private void bexit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
}