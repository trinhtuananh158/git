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

        public string getConnectionString()
        {
            StreamReader srd = new StreamReader("D:\\Sound\\config.txt");
            return srd.ReadLine();
        }
        private void ICS_Load(object sender, EventArgs e)
        {
            try
            {
                string today = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'  order by stt", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    DataTable data = new DataTable();
                    DataRow dr;
                    data.Columns.Add("STT");
                    data.Columns.Add("Trạng thái");
                    data.Columns.Add("Thời gian tạo");
                    data.Columns.Add("Thời gian hoàn thành");
                    data.Columns.Add("Quầy");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = data.NewRow();
                        dr["STT"] = dt.Rows[i]["stt"].ToString();
                        dr["Trạng thái"] = dt.Rows[i]["status"].ToString();
                        dr["Thời gian tạo"] = dt.Rows[i]["datecreate"].ToString();
                        dr["Thời gian hoàn thành"] = dt.Rows[i]["datecom"].ToString();
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
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + day + "'  order by stt", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    DataTable data = new DataTable();
                    DataRow dr;
                    data.Columns.Add("STT");
                    data.Columns.Add("Trạng thái");
                    data.Columns.Add("Thời gian tạo");
                    data.Columns.Add("Thời gian hoàn thành");
                    data.Columns.Add("Quầy");
                    if (dt.Rows.Count == 0)
                        MessageBox.Show("Không có dữ liệu ngày "+stringday);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = data.NewRow();
                        dr["STT"] = dt.Rows[i]["stt"].ToString();
                        dr["Trạng thái"] = dt.Rows[i]["status"].ToString();
                        dr["Thời gian tạo"] = dt.Rows[i]["datecreate"].ToString();
                        dr["Thời gian hoàn thành"] = dt.Rows[i]["datecom"].ToString();
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
            try
            {
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from TbQms order by datecreate", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    int count = dt.Rows.Count;
                    DateTime dtcreate = (DateTime)dt.Rows[count - 1]["datecreate"];
                    string strdtcreate = String.Format("{0:yyyy-MM-dd}",dtcreate);
                    string today = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    if (strdtcreate == today)
                    {
                        string sqlInsert = "insert into TbQms(stt,status,datecreate) values(@stt,@status,@datecreate)";
                        SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                        int stt = Int32.Parse(dt.Rows[count-1]["stt"].ToString()) + 1;
                        sqlCom.Parameters.AddWithValue("stt", stt);
                        sqlCom.Parameters.AddWithValue("status","new");
                        sqlCom.Parameters.AddWithValue("datecreate",DateTime.Now);
                        sqlCom.ExecuteNonQuery();
                        MessageBox.Show("Phat so ke tiep "+stt);
                        SqlDataAdapter sda = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'  order by stt", con);
                        DataTable dt1 = new DataTable();
                        sda.Fill(dt1);
                        DataTable data = new DataTable();
                        DataRow dr;
                        data.Columns.Add("STT");
                        data.Columns.Add("Trạng thái");
                        data.Columns.Add("Thời gian tạo");
                        data.Columns.Add("Thời gian hoàn thành");
                        data.Columns.Add("Quầy");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dr = data.NewRow();
                            dr["STT"] = dt1.Rows[i]["stt"].ToString();
                            dr["Trạng thái"] = dt1.Rows[i]["status"].ToString();
                            dr["Thời gian tạo"] = dt1.Rows[i]["datecreate"].ToString();
                            dr["Thời gian hoàn thành"] = dt1.Rows[i]["datecom"].ToString();
                            dr["Quầy"] = dt1.Rows[i]["quay"].ToString();
                            data.Rows.Add(dr);
                        }
                        dataGridView1.DataSource = data;
                    }
                    else
                    {
                        string sqlInsert = "insert into TbQms(stt,status,datecreate) values(@stt,@status,@datecreate)";
                        SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                        int stt = Int32.Parse(dt.Rows[count - 1]["stt"].ToString()) + 1;
                        sqlCom.Parameters.AddWithValue("stt", 1);
                        sqlCom.Parameters.AddWithValue("status", "new");
                        sqlCom.Parameters.AddWithValue("datecreate", DateTime.Now);
                        sqlCom.ExecuteNonQuery();
                        MessageBox.Show("Phat so ke tiep " + 1);
                        SqlDataAdapter sda = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'  order by stt", con);
                        DataTable dt1 = new DataTable();
                        sda.Fill(dt1);
                        DataTable data = new DataTable();
                        DataRow dr;
                        data.Columns.Add("STT");
                        data.Columns.Add("Trạng thái");
                        data.Columns.Add("Thời gian tạo");
                        data.Columns.Add("Thời gian hoàn thành");
                        data.Columns.Add("Quầy");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dr = data.NewRow();
                            dr["STT"] = dt1.Rows[i]["stt"].ToString();
                            dr["Trạng thái"] = dt1.Rows[i]["status"].ToString();
                            dr["Thời gian tạo"] = dt1.Rows[i]["datecreate"].ToString();
                            dr["Thời gian hoàn thành"] = dt1.Rows[i]["datecom"].ToString();
                            dr["Quầy"] = dt1.Rows[i]["quay"].ToString();
                            data.Rows.Add(dr);
                        }
                        dataGridView1.DataSource = data;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string today = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'  order by stt", con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    DataTable data = new DataTable();
                    DataRow dr;
                    data.Columns.Add("STT");
                    data.Columns.Add("Trạng thái");
                    data.Columns.Add("Thời gian tạo");
                    data.Columns.Add("Thời gian hoàn thành");
                    data.Columns.Add("Quầy");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr = data.NewRow();
                        dr["STT"] = dt.Rows[i]["stt"].ToString();
                        dr["Trạng thái"] = dt.Rows[i]["status"].ToString();
                        dr["Thời gian tạo"] = dt.Rows[i]["datecreate"].ToString();
                        dr["Thời gian hoàn thành"] = dt.Rows[i]["datecom"].ToString();
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

    }
}
