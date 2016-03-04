using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Gate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=TTUANANH;Initial Catalog=qms;Integrated Security=True";
        public int getstt()
        {
            int a;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlDataAdapter sDa = new SqlDataAdapter("Select * from TbQms", con);
                DataTable dt = new DataTable();
                sDa.Fill(dt);
                a = Int32.Parse(dt.Rows.Count.ToString());
            }
            return a;
        }

        public string getStringstt(int stt)
        {
            if (stt < 10)
                return "000" + stt;
            else if (stt < 100)
                return "00" + stt;
            else if (stt < 1000)
                return "0" + stt;
            else
                return stt.ToString();
        }

        private void btnNext1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from TbQms order by stt", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    int count = dt.Rows.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        string sqlInsert = "insert into TbQms values(@stt,@quay)";
                        SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                        sqlCom.Parameters.AddWithValue("stt", Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1);
                        sqlCom.Parameters.AddWithValue("quay", 1);
                        sqlCom.ExecuteNonQuery();
                        MessageBox.Show("Next in Gate 1: " + getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1));
                        lbServing1.Text = getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnNext2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from TbQms order by stt", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    int count = dt.Rows.Count;
                    for (int i = count - 1; i >= 0; i--)
                    {
                        string sqlInsert = "insert into TbQms values(@stt,@quay)";
                        SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                        sqlCom.Parameters.AddWithValue("stt", Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1);
                        sqlCom.Parameters.AddWithValue("quay", 2);
                        sqlCom.ExecuteNonQuery();
                        MessageBox.Show("Next in Gate 2: " + getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1));
                        lbServing2.Text = getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbServing1.Text = "";
            lbServing2.Text = "";
        }
    }
}
