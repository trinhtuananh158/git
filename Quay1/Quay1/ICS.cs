using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Gate
{
    public partial class ICS : Form
    {
        public ICS()
        {
            InitializeComponent();
        }

        public string getConnectionString(string filename)
        {
            StreamReader srd = new StreamReader(filename);
            return srd.ReadLine();
        }

        private void ICS_Load(object sender, EventArgs e)
        {
            try
            {
                string today = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                using (SqlConnection con = new SqlConnection(getConnectionString("D:\\Sound\\config.txt")))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'  order by stt", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    DataTable data = new DataTable();
                    DataRow dr;
                    data.Columns.Add("STT");
                    data.Columns.Add("Trạng thái");
                    data.Columns.Add("Ngày giờ tạo");
                    data.Columns.Add("Ngày giờ xong");
                    data.Columns.Add("Quầy");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = data.NewRow();
                        dr["STT"]=dt.Rows[i]["stt"].ToString();
                        dr["Trạng thái"] = dt.Rows[i]["status"].ToString();
                        dr["Ngày giờ tạo"] = dt.Rows[i]["datecreate"].ToString();
                        dr["Ngày giờ xong"] = dt.Rows[i]["datecom"].ToString();
                        dr["Quầy"] = dt.Rows[i]["quay"].ToString();
                        data.Rows.Add(dr);
                    }
                    dataGridView1.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                string day = String.Format("{0:yyyy-MM-dd}", dateTimePicker1.Value);
                string stringday = String.Format("{0:dd-MM-yyyy}", dateTimePicker1.Value);
                using (SqlConnection con = new SqlConnection(getConnectionString("D:\\Sound\\config.txt")))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + day + "'  order by stt", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    DataTable data = new DataTable();
                    DataRow dr;
                    data.Columns.Add("STT");
                    data.Columns.Add("Trạng thái");
                    data.Columns.Add("Ngày giờ tạo");
                    data.Columns.Add("Ngày giờ xong");
                    data.Columns.Add("Quầy");
                    if (dt.Rows.Count == 0)
                        MessageBox.Show("Không có dữ liệu ngày "+stringday);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = data.NewRow();
                        dr["STT"] = dt.Rows[i]["stt"].ToString();
                        dr["Trạng thái"] = dt.Rows[i]["status"].ToString();
                        dr["Ngày giờ tạo"] = dt.Rows[i]["datecreate"].ToString();
                        dr["Ngày giờ xong"] = dt.Rows[i]["datecom"].ToString();
                        dr["Quầy"] = dt.Rows[i]["quay"].ToString();
                        data.Rows.Add(dr);
                    }
                    dataGridView1.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGetnum_Click(object sender, EventArgs e)
        {

        }
    }
}
