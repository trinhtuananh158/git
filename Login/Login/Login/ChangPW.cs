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
    public partial class ChangPW : Form
    {
        string connectionString = @"Data Source=ttuananh;Initial Catalog=qms;Persist Security Info=True;User ID=sa;Password=Avxlcdm@1";
        public ChangPW()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from tbAccounts where username='"+txtUser.Text+"'", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    if ((dt.Rows[0]["password"].ToString() == txtOldpw.Text) && (txtNewpw.Text == txtRe.Text))
                    {
                        string sqlInsert = "update tbAccounts set password = @pass where username='"+txtUser.Text+"'";
                        SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                        sqlCom.Parameters.AddWithValue("pass", txtNewpw.Text);
                        sqlCom.ExecuteNonQuery();
                        MessageBox.Show("Doi mat khau thanh cong!");
                    }
                    else
                        MessageBox.Show("Thong tin sai. Vui long kiem tra lai!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
