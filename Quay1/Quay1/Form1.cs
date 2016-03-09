using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Media;
using System.IO;

namespace Gate
{
    public partial class Form1 : Form
    {
        string[] filename = { @"d:\Sound\0.wav", @"d:\Sound\1.wav", @"d:\Sound\2.wav", @"d:\Sound\3.wav", @"d:\Sound\4.wav", @"d:\Sound\5.wav", @"d:\Sound\6.wav", @"d:\Sound\7.wav", @"d:\Sound\8.wav", @"d:\Sound\9.wav" };
        string today = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
        public Form1()
        {
            InitializeComponent();
        }
        public string getConnectionString()
        {
            StreamReader srd = new StreamReader("D:\\Sound\\config.txt");
            return srd.ReadLine();
        }

        public int getstt()
        {
            int a;
            using (SqlConnection con = new SqlConnection(getConnectionString()))
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
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='"+today+"' order by datecreate,stt", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    int count = dt.Rows.Count;
                    if (dt.Rows[0]["status"].ToString() == "new")
                    {
                        string sqlInsert = "update TbQms set status='in progress',quay=1 where stt=" + dt.Rows[0]["stt"].ToString();
                        SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                        int stt = Int32.Parse(dt.Rows[0]["stt"].ToString());
                        sqlCom.ExecuteNonQuery();
                        LoadSoundFile(@"d:\Sound\XM.wav").Play();
                        Thread.Sleep(3000);
                        Speck(stt);
                        Thread.Sleep(1500);
                        LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                        Thread.Sleep(2500);
                        LoadSoundFile(@"d:\Sound\1.wav").Play();
                        MessageBox.Show("Next in Gate 1: " + getStringstt(Int32.Parse(dt.Rows[0]["stt"].ToString())));
                        lbServing1.Text = getStringstt(Int32.Parse(dt.Rows[0]["stt"].ToString()));
                    }
                    else if (dt.Rows[count-1]["status"].ToString() == "com")
                    {
                        string sqlInsert2 = "update TbQms set status='com',datecom=@datecom where status='in progress' and quay=1";
                        SqlCommand sqlCom2 = new SqlCommand(sqlInsert2, con);
                        sqlCom2.Parameters.AddWithValue("@datecom", DateTime.Now);
                        sqlCom2.ExecuteNonQuery();
                        lbServing1.Text = "";
                        MessageBox.Show("Đã hết số vào quầy!");
                    }
                    else if ((dt.Rows[count - 1]["status"].ToString() == "in progress") && (dt.Rows[count - 2]["status"].ToString()=="com"))
                    {
                        if (dt.Rows[count-1]["quay"].ToString() == "1")
                        {
                            string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[count-1]["stt"].ToString();
                            SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                            sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                            sqlCom.ExecuteNonQuery();
                        }
                        lbServing1.Text = "";
                        MessageBox.Show("Đã hết số vào quầy!");
                    }
                    else if ((dt.Rows[count - 1]["status"].ToString() == "in progress") && (dt.Rows[count - 2]["status"].ToString() == "in progress"))
                    {
                        if (dt.Rows[count - 2]["quay"].ToString() == "1")
                        {
                            string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[count - 2]["stt"].ToString();
                            SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                            sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                            sqlCom.ExecuteNonQuery();
                        }
                        else if (dt.Rows[count - 1]["quay"].ToString() == "1")
                        {
                            string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[count - 1]["stt"].ToString();
                            SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                            sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                            sqlCom.ExecuteNonQuery();
                        }
                        lbServing1.Text = "";
                        MessageBox.Show("Đã hết số vào quầy!");
                    }

                    else
                    {
                        for (int i = 0; i < count - 2; i++)
                        {
                            int j = i + 1;
                            int z = j + 1;
                            if ((dt.Rows[i]["status"].ToString() == "in progress") && (dt.Rows[i]["quay"].ToString() == "1") && (dt.Rows[j]["status"].ToString() == "new"))
                            {
                                string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[i]["stt"].ToString();
                                SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                int stt = Int32.Parse(dt.Rows[j]["stt"].ToString());
                                sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                sqlCom.ExecuteNonQuery();
                                string sqlInsert2 = "update TbQms set status='com',datecom=@datecom where status='in progress' and quay=1";
                                SqlCommand sqlCom2 = new SqlCommand(sqlInsert2, con);
                                sqlCom2.Parameters.AddWithValue("@datecom", DateTime.Now);
                                sqlCom2.ExecuteNonQuery();
                                string sqlInsert1 = "update TbQms set status='in progress',quay=1 where stt=" + dt.Rows[j]["stt"].ToString();
                                SqlCommand sqlCom1 = new SqlCommand(sqlInsert1, con);
                                sqlCom1.ExecuteNonQuery();
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\1.wav").Play();
                                MessageBox.Show("Next in Gate 1: " + getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1));
                                lbServing1.Text = getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1);
                                break;
                            }
                            else if ((dt.Rows[i]["status"].ToString() == "in progress") && (dt.Rows[j]["status"].ToString() == "in progress") && (dt.Rows[z]["status"].ToString() == "new"))
                            {
                                string sqlInsert2 = "update TbQms set status='com',datecom=@datecom where status='in progress' and quay=1";
                                SqlCommand sqlCom2 = new SqlCommand(sqlInsert2, con);
                                sqlCom2.Parameters.AddWithValue("@datecom", DateTime.Now);
                                sqlCom2.ExecuteNonQuery();
                                string sqlInsert1 = "update TbQms set status='in progress',quay=1 where stt=" + dt.Rows[z]["stt"].ToString();
                                SqlCommand sqlCom1 = new SqlCommand(sqlInsert1, con);
                                sqlCom1.ExecuteNonQuery();
                                if (dt.Rows[i]["quay"].ToString() == "1")
                                {
                                    string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[i]["stt"].ToString();
                                    SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                    sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                    sqlCom.ExecuteNonQuery();
                                }
                                else if (dt.Rows[j]["quay"].ToString() == "1")
                                {
                                    string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[j]["stt"].ToString();
                                    SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                    sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                    sqlCom.ExecuteNonQuery();
                                }
                                int stt = Int32.Parse(dt.Rows[z]["stt"].ToString());
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\1.wav").Play();
                                MessageBox.Show("Next in Gate 1: " + getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1));
                                lbServing1.Text = getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1);
                                break;
                            }
                            else if ((dt.Rows[i]["status"].ToString() == "com") && (dt.Rows[j]["status"].ToString() == "in progress") && (dt.Rows[z]["status"].ToString() == "new"))
                            {
                                if (dt.Rows[j]["quay"].ToString() == "1")
                                {
                                    string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[j]["stt"].ToString();
                                    SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                    sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                    sqlCom.ExecuteNonQuery();
                                }
                                string sqlInsert2 = "update TbQms set status='com',datecom=@datecom where status='in progress' and quay=1";
                                SqlCommand sqlCom2 = new SqlCommand(sqlInsert2, con);
                                sqlCom2.Parameters.AddWithValue("@datecom", DateTime.Now);
                                sqlCom2.ExecuteNonQuery();
                                string sqlInsert1 = "update TbQms set status='in progress',quay=1 where stt=" + dt.Rows[z]["stt"].ToString();
                                SqlCommand sqlCom1 = new SqlCommand(sqlInsert1, con);
                                sqlCom1.ExecuteNonQuery();
                                int stt = Int32.Parse(dt.Rows[z]["stt"].ToString());
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\1.wav").Play();
                                MessageBox.Show("Next in Gate 1: " + getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1));
                                lbServing1.Text = getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1);
                                break;
                            }
                            else if ((dt.Rows[j]["status"].ToString() == "com") && (dt.Rows[z]["status"].ToString() == "new"))
                            {
                                string sqlInsert = "update TbQms set status='in progress',quay=1 where stt=" + dt.Rows[z]["stt"].ToString();
                                SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                int stt = Int32.Parse(dt.Rows[z]["stt"].ToString());
                                sqlCom.ExecuteNonQuery();
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\1.wav").Play();
                                MessageBox.Show("Next in Gate 1: " + getStringstt(Int32.Parse(dt.Rows[z]["stt"].ToString())));
                                lbServing1.Text = getStringstt(Int32.Parse(dt.Rows[z]["stt"].ToString()));
                                break;
                            }
                        }
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
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from TbQms where LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "' order by datecreate,stt", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    int count = dt.Rows.Count;
                    if (dt.Rows[0]["status"].ToString() == "new")
                    {
                        string sqlInsert = "update TbQms set status='in progress',quay=2 where stt=" + dt.Rows[0]["stt"].ToString();
                        SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                        int stt = Int32.Parse(dt.Rows[0]["stt"].ToString());
                        sqlCom.ExecuteNonQuery();
                        LoadSoundFile(@"d:\Sound\XM.wav").Play();
                        Thread.Sleep(3000);
                        Speck(stt);
                        Thread.Sleep(1500);
                        LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                        Thread.Sleep(2500);
                        LoadSoundFile(@"d:\Sound\2.wav").Play();
                        MessageBox.Show("Next in Gate 2: " + getStringstt(Int32.Parse(dt.Rows[0]["stt"].ToString())));
                        lbServing2.Text = getStringstt(Int32.Parse(dt.Rows[0]["stt"].ToString()));
                    }
                    else if (dt.Rows[count - 1]["status"].ToString() == "com")
                    {
                        string sqlInsert2 = "update TbQms set status='com',datecom=@datecom where status='in progress' and quay=2";
                        SqlCommand sqlCom2 = new SqlCommand(sqlInsert2, con);
                        sqlCom2.Parameters.AddWithValue("@datecom", DateTime.Now);
                        sqlCom2.ExecuteNonQuery();
                        lbServing2.Text = "";
                        MessageBox.Show("Đã hết số vào quầy!");
                    }
                    else if ((dt.Rows[count - 1]["status"].ToString() == "in progress") && (dt.Rows[count - 2]["status"].ToString() == "com"))
                    {
                        if (dt.Rows[count - 1]["quay"].ToString() == "2")
                        {
                            string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[count - 1]["stt"].ToString();
                            SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                            sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                            sqlCom.ExecuteNonQuery();
                        }
                        lbServing2.Text = "";
                        MessageBox.Show("Đã hết số vào quầy!");
                    }
                    else if ((dt.Rows[count - 1]["status"].ToString() == "in progress") && (dt.Rows[count - 2]["status"].ToString() == "in progress"))
                    {
                        if (dt.Rows[count - 2]["quay"].ToString() == "2")
                        {
                            string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[count - 2]["stt"].ToString();
                            SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                            sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                            sqlCom.ExecuteNonQuery();
                        }
                        else if (dt.Rows[count - 1]["quay"].ToString() == "2")
                        {
                            string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[count - 1]["stt"].ToString();
                            SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                            sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                            sqlCom.ExecuteNonQuery();
                        }
                        lbServing2.Text = "";
                        MessageBox.Show("Đã hết số vào quầy!");
                    }

                    else
                    {
                        for (int i = 0; i < count - 2; i++)
                        {
                            int j = i + 1;
                            int z = j + 1;
                            if ((dt.Rows[i]["status"].ToString() == "in progress") && (dt.Rows[i]["quay"].ToString() == "2") && (dt.Rows[j]["status"].ToString() == "new"))
                            {
                                string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[i]["stt"].ToString();
                                SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                int stt = Int32.Parse(dt.Rows[j]["stt"].ToString());
                                sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                sqlCom.ExecuteNonQuery();
                                string sqlInsert2 = "update TbQms set status='com',datecom=@datecom where status='in progress' and quay=2";
                                SqlCommand sqlCom2 = new SqlCommand(sqlInsert2, con);
                                sqlCom2.Parameters.AddWithValue("@datecom", DateTime.Now);
                                sqlCom2.ExecuteNonQuery();
                                string sqlInsert1 = "update TbQms set status='in progress',quay=2 where stt=" + dt.Rows[j]["stt"].ToString();
                                SqlCommand sqlCom1 = new SqlCommand(sqlInsert1, con);
                                sqlCom1.ExecuteNonQuery();
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\2.wav").Play();
                                MessageBox.Show("Next in Gate 1: " + getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1));
                                lbServing2.Text = getStringstt(Int32.Parse(dt.Rows[i]["stt"].ToString()) + 1);
                                break;
                            }
                            else if ((dt.Rows[i]["status"].ToString() == "in progress") && (dt.Rows[j]["status"].ToString() == "in progress") && (dt.Rows[z]["status"].ToString() == "new"))
                            {
                                string sqlInsert1 = "update TbQms set status='in progress',quay=2 where stt=" + dt.Rows[z]["stt"].ToString();
                                SqlCommand sqlCom1 = new SqlCommand(sqlInsert1, con);
                                sqlCom1.ExecuteNonQuery();
                                if (dt.Rows[i]["quay"].ToString() == "2")
                                {
                                    string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[i]["stt"].ToString();
                                    SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                    sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                    sqlCom.ExecuteNonQuery();
                                }
                                else if (dt.Rows[j]["quay"].ToString() == "2")
                                {
                                    string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[j]["stt"].ToString();
                                    SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                    sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                    sqlCom.ExecuteNonQuery();
                                }
                                int stt = Int32.Parse(dt.Rows[z]["stt"].ToString());
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\2.wav").Play();
                                MessageBox.Show("Next in Gate 2: " + getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1));
                                lbServing2.Text = getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1);
                                break;
                            }
                            else if ((dt.Rows[i]["status"].ToString() == "com") && (dt.Rows[j]["status"].ToString() == "in progress") && (dt.Rows[z]["status"].ToString() == "new"))
                            {
                                if (dt.Rows[j]["quay"].ToString() == "2")
                                {
                                    string sqlInsert = "update TbQms set status='com',datecom=@datecom where stt=" + dt.Rows[j]["stt"].ToString();
                                    SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                    sqlCom.Parameters.AddWithValue("@datecom", DateTime.Now);
                                    sqlCom.ExecuteNonQuery();
                                }
                                string sqlInsert2 = "update TbQms set status='com',datecom=@datecom where status='in progress' and quay=2";
                                SqlCommand sqlCom2 = new SqlCommand(sqlInsert2, con);
                                sqlCom2.Parameters.AddWithValue("@datecom", DateTime.Now);
                                sqlCom2.ExecuteNonQuery();
                                string sqlInsert1 = "update TbQms set status='in progress',quay=2 where stt=" + dt.Rows[z]["stt"].ToString();
                                SqlCommand sqlCom1 = new SqlCommand(sqlInsert1, con);
                                sqlCom1.ExecuteNonQuery();
                                int stt = Int32.Parse(dt.Rows[z]["stt"].ToString());
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\2.wav").Play();
                                MessageBox.Show("Next in Gate 2: " + getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1));
                                lbServing2.Text = getStringstt(Int32.Parse(dt.Rows[j]["stt"].ToString()) + 1);
                                break;
                            }
                            else if ((dt.Rows[j]["status"].ToString() == "com") && (dt.Rows[z]["status"].ToString() == "new"))
                            {
                                string sqlInsert = "update TbQms set status='in progress',quay=2 where stt=" + dt.Rows[z]["stt"].ToString();
                                SqlCommand sqlCom = new SqlCommand(sqlInsert, con);
                                int stt = Int32.Parse(dt.Rows[z]["stt"].ToString());
                                sqlCom.ExecuteNonQuery();
                                LoadSoundFile(@"d:\Sound\XM.wav").Play();
                                Thread.Sleep(3000);
                                Speck(stt);
                                Thread.Sleep(1500);
                                LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                                Thread.Sleep(2500);
                                LoadSoundFile(@"d:\Sound\2.wav").Play();
                                MessageBox.Show("Next in Gate 2: " + getStringstt(Int32.Parse(dt.Rows[z]["stt"].ToString())));
                                lbServing2.Text = getStringstt(Int32.Parse(dt.Rows[z]["stt"].ToString()));
                                break;
                            }
                        }
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
            try
            {
                lbServing1.Text = "";
                lbServing2.Text = "";
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * from TbQms Where status='in progress' and LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    int count = dt.Rows.Count;
                    if (count == 0)
                    {
                        lbServing1.Text = "";
                        lbServing2.Text = "";
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (dt.Rows[i]["quay"].ToString().Trim().Equals("1"))
                            {
                                lbServing1.Text = dt.Rows[i]["stt"].ToString();
                            }
                            if (dt.Rows[i]["quay"].ToString().Trim().Equals("2"))
                            {
                                lbServing2.Text = dt.Rows[i]["stt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public SoundPlayer LoadSoundFile(string filename)
        {
            SoundPlayer sound = null;

            try
            {
                sound = new SoundPlayer();
                sound.SoundLocation = filename;
                sound.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading sound");
            }

            return sound;
        }

        private void Speck(int num)
        {
            if (num < 10)
            {
                num = num % 10;
                switch (num)
                {
                    case 0:
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        LoadSoundFile(filename[9]).Play();
                        break;
                }

            }
            else if (num < 100)
            {
                int div = num / 10;
                num = num % 10;
                LoadSoundFile(filename[0]).Play();
                Thread.Sleep(1500);
                LoadSoundFile(filename[0]).Play();
                Thread.Sleep(1500);
                switch (div)
                {
                    case 0:
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (num)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
            }
            else if (num < 1000)
            {
                int a = num / 100;
                int b = (num-(a*100))/10;
                int c = (num - (a * 100)) % 10;
                LoadSoundFile(filename[0]).Play();
                Thread.Sleep(1500);
                switch (a)
                {
                    case 0:
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (b)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (c)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
            }
            else if(num<10000)
            {
                int a = num / 1000;
                int b = (num - (a * 1000)) / 100;
                int c = (num - (a * 1000) - (b * 100)) / 10;
                int d = (num - (a * 1000) - (b * 100) - (c * 10));
                switch (a)
                {
                    case 0:
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (b)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (c)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (d)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
            }
            else if(num<100000)
            {
                int a = num / 10000;
                int b = (num - (a * 10000)) / 1000;
                int c = (num - (a * 10000) - (b * 1000)) / 100;
                int d = (num - (a * 10000) - (b * 1000) - (c * 100))/10;
                int e = (num - (a * 10000) - (b * 1000) - (c * 100)-(d*10));
                switch (a)
                {
                    case 0:
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (b)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (c)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (d)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
                switch (e)
                {
                    case 0:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[0]).Play();
                        break;
                    case 1:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[1]).Play();
                        break;
                    case 2:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[2]).Play();
                        break;
                    case 3:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[3]).Play();
                        break;
                    case 4:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[4]).Play();
                        break;
                    case 5:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[5]).Play();
                        break;
                    case 6:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[6]).Play();
                        break;
                    case 7:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[7]).Play();
                        break;
                    case 8:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[8]).Play();
                        break;
                    case 9:
                        Thread.Sleep(1500);
                        LoadSoundFile(filename[9]).Play();
                        break;
                }
            }
        }

        private void btnRecall1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * From TbQms Where quay=1 and status='in progress' and LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("Chưa có khách vào!");
                    }
                    else
                    {
                        LoadSoundFile(@"d:\Sound\XM.wav").Play();
                        Thread.Sleep(3000);
                        Speck(Int32.Parse(dt.Rows[0]["stt"].ToString()));
                        Thread.Sleep(1500);
                        LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                        Thread.Sleep(2200);
                        LoadSoundFile(@"d:\Sound\1.wav").Play();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnRecall2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(getConnectionString()))
                {
                    con.Open();
                    SqlDataAdapter sDa = new SqlDataAdapter("Select * From TbQms Where quay=2 and status='in progress' and LEFT(convert(VARCHAR,datecreate,120),10)='" + today + "'", con);
                    DataTable dt = new DataTable();
                    sDa.Fill(dt);
                    if (dt.Rows.Count == 0)
                        MessageBox.Show("Chưa có khách vào!");
                    else
                    {
                        LoadSoundFile(@"d:\Sound\XM.wav").Play();
                        Thread.Sleep(3000);
                        Speck(Int32.Parse(dt.Rows[0]["stt"].ToString()));
                        Thread.Sleep(1500);
                        LoadSoundFile(@"d:\Sound\DQ.wav").Play();
                        Thread.Sleep(2200);
                        LoadSoundFile(@"d:\Sound\2.wav").Play();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
