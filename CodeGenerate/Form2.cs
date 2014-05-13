using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using Helper.DbDataType;
using Helper.Database;
using Helper;
namespace Coder
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            button1.Visible = false;
            button1.Click += new EventHandler(button1_Click);
        }

        void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "d:\\bak";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1DbPath.Text = openFileDialog1.FileName;
            }
        }

        void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            string val = comboBox1.SelectedItem.ToString().ToLower();
            if (val == "oledb")
            {
                button1.Visible = true;
                button3.Visible = false;
            }
            else if (val == "mysql")
            {
                button3.Visible = true;
                button1.Visible = false;
                core = new MySqlCore(textBox1DbPath.Text);
                textBox1DbPath.Text = "Server=localhost;Uid=root;Pwd=niejunhua;";
            }
            else if (val == "sqlserver")
            {
                button1.Visible = false;
                textBox1DbPath.Text = "server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=K2Sln";
                button3.Visible = true;
                core = new SqlServerCore(textBox1DbPath.Text);
            }
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //  NjhData db = new NjhData(DataType.Oledb, @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=c:\\data\\nq.mdb");
            //  AccessHelper ac = new AccessHelper("c:\\data\\nq.mdb");
            //  OleDbConnection conn = ac.CreateConnection();
            //  DataTable dt = conn.GetSchema("Tables", new string[] { null, null, "Single" });
            //  dt = conn.GetSchema("DataTypes");
            ////  dataGridView1.DataSource = dt;

            //  OleDbDataReader dr = null;

        }
        Helper.BaseCore core = null;
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1DbPath.Text))
            {
                MessageBox.Show("请填写数据库地址！", "Error");
                return;
            }
            string val = comboBox1.SelectedItem.ToString().ToLower();
            if (val.ToLower() == "oledb")
            {
                core = new AccessCore(textBox1DbPath.Text);
            }
            else if (val == "mysql")
            {
                button1.Visible = false;
                core = new MySqlCore(textBox1DbPath.Text);
            }
            else if (val == "sqlserver")
            {
                button1.Visible = false;
                core = new SqlServerCore(textBox1DbPath.Text);
            }

            core.CreateAll(textBox1NameSpace.Text, comboBox2.SelectedItem == null ? string.Empty : comboBox2.SelectedItem.ToString());
            richTextBox1.Text += "[" + DateTime.Now.ToString() + "] create succcess!\r\n";

            if (checkBox1.Checked)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "CreatedFiles";
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
            labelResult.Text = "数据生成完成！";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1DbPath.Text) || comboBox1.SelectedText.ToLower() == "oledb")
            {
                return;
            }
            comboBox2.DataSource = core.GetDatabases();
            labelResult.Text = "加载数据库成功";
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
