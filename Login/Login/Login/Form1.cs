using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Login
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=ttuananh;Initial Catalog=qms;Persist Security Info=True;User ID=sa;Password=Avxlcdm@1";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = string.Empty;
            string pass = string.Empty;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from tbAccounts", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    user = dt.Rows[0]["username"].ToString();
                    pass = dt.Rows[0]["password"].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if ((txtUser.Text == "") || (txtPass.Text == ""))
                MessageBox.Show("Input user name and password");
            else if ((txtUser.Text == user) && (txtPass.Text == pass))
            {
                Form2 f2 = new Form2();
                f2.Show();
                this.Visible = false;
            }
            else
                MessageBox.Show("User name or password is incorrect!");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangPW f1 = new ChangPW();
            f1.Show();
            this.Visible = false;
        }
    }
}
