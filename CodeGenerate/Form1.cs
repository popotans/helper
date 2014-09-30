using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;
using System.Runtime.Remoting.Messaging;
using System.Data.Common;
using System.Data.SqlClient;
using Coder.Core;
using Helper;
namespace Coder
{
    public partial class Form1 : Form
    {
        private BaseCore core;
        private string SaveFolder;
        public Form1()
        {
            InitializeComponent();
            SaveFolder = AppDomain.CurrentDomain.BaseDirectory + "\\CreatedFiles\\";

            //textBox1.Text = "server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=K2Sln";
            //textBox1.Text = "server=.;uid=sa;password=niejunhua";
            //textBox1.Text = "Server=localhost;Uid=root;Pwd=niejunhua;";
            comboBox1.DataSource = new List<string> { "", "Oledb", "MsSql", "MySql" };
            comboBox1.SelectedIndexChanged += new EventHandler(listBox2_SelectedIndexChanged);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;


            comboBoxDb.DropDownStyle = comboBox1.DropDownStyle;
            comboBoxDb.SelectedIndexChanged += new EventHandler(comboBoxDb_SelectedIndexChanged);

            listBox1.SelectionMode = SelectionMode.MultiExtended;
            button1.Visible = false;
            Helper.IO.FileHelper.CreateDir(AppDomain.CurrentDomain.BaseDirectory + "\\Data");

            //hide no use folder
            button2.Visible = false;
            button3.Visible = false;
        }

        void comboBoxDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatus.Text = "加载数据表...";
            listBox1.DataSource = core.GetTables(comboBoxDb.SelectedValue.ToString());// GetTables();
            lblStatus.Text = "加载数据表完成";
        }

        #region


        #endregion

        /// <summary>
        /// 选择数据库类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Visible = comboBox1.SelectedValue.ToString() == "Oledb";
            string v = comboBox1.SelectedValue.ToString();
            if (v == "Oledb")
            {

            }
            else if (v == "MsSql")
            {
                // if (string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = "server=192.168.104.117;uid=wftsa;password=jd7nTF#wM;database=K2Sln";
                core = new MsSqlCore(textBox1.Text);

            }
            else if (v == "MySql")
            {
                // if (string.IsNullOrEmpty(textBox1.Text))
                textBox1.Text = "Server=localhost;Uid=root;Pwd=niejunhua;";
                core = new Coder.Core.MySqlCore(textBox1.Text);
            }
        }


        private BaseModeCreate GetCreator()
        {
            BaseModeCreate creator = null;
            string v = comboBox1.SelectedValue.ToString();
            if (v == "Oledb")
            {
                creator = new AccessModeCreate(textBox1.Text);
            }
            else if (v == "MsSql")
            {
                creator = new SqlServerModeCreate(textBox1.Text);
            }
            else if (v == "MySql")
            {
                creator = new MySqlModeCreate(textBox1.Text);
            }
            return creator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Application.Exit();
            ///
            base.OnFormClosed(e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //tmpl.m m = new tmpl.m();
            //m.Session = new Dictionary<string, object>();
            //m.Session["db"] = textBox1.Text;
            //m.Session["table"] = listBox1.SelectedValue.ToString();
            //// CallContext.LogicalSetData("db", textBox1.Text);
            //string s = m.TransformText();
            ////MessageBox.Show(s);
            //richTextBox1.Text = s;

            MessageBox.Show("for test");
        }

        private void btnSaveEntity_Click(object sender, EventArgs e)
        {
            string tableName = listBox1.SelectedValue.ToString();
            string s = CreateEntityScript(tableName);
            richTextBox1.Text = s;
            string SaveDir = "";
            if (checkBox1.Checked)
            {
                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                DialogResult drs = saveFileDialog1.ShowDialog();
                if (drs == DialogResult.OK)
                {
                    SaveDir = saveFileDialog1.FileName;
                    Helper.IO.FileHelper.WriteFile(SaveDir, s, System.Text.Encoding.Default, false);
                }
            }
            //MessageBox.Show(SaveDir);
        }

        private string CreateEntityScript(string tableName, bool createFile = false)
        {
            Helper.VelocityHelper helper = new Helper.VelocityHelper("/tmpl/");
            string dbName = "";

            object obj = this.Invoke(new DelegateHelper.DGet(DelegateHelper.GetVal), comboBoxDb);

            if (null != obj) dbName = obj.ToString();

            List<DbColumn> list = core.GetDbColumns(dbName, tableName);
            helper.Put("EntityName", tableName);
            helper.Put("columns", list);
            if (list.Count > 0)
                helper.Put("schema", list[0].TableSchema);
            string identity = list.Count > 0 ? list[0].IdentityKeys : "";
            helper.Put("identity", identity);
            helper.Put("namespace", !string.IsNullOrEmpty(tbNamespace.Text) ? tbNamespace.Text : "DAO");
            // dbcolumn

            string rsStr = helper.Render("Model.nm");
            if (createFile)
            {
                if (!string.IsNullOrEmpty(rsStr))
                {
                    object richText = this.Invoke(new DelegateHelper.DGet(DelegateHelper.GetVal), richTextBox1);
                    Helper.IO.FileHelper.WriteFile("Data\\" + tableName + ".cs", rsStr, Encoding.Default, false);
                }
            }
            return rsStr;
        }


        private void btnSavetoFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {

                Helper.IO.FileHelper.WriteFile("Data\\entity.cs", richTextBox1.Text, Encoding.Default, false);
                lblSave.ForeColor = Color.Green;
                lblSave.Text = " 已保存。";
            }
        }


        private void CreateMode(string tableName)
        {
            BaseModeCreate creator = GetCreator();
            string rs = "";
            string np = tbNamespace.Text;
            if (np.Length == 0) np = "hjn";
            if (string.IsNullOrEmpty(tableName))
                creator.CreateAll(np, comboBoxDb.SelectedValue.ToString(), "", SaveFolder);
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            btnBatch.Enabled = false;
            ThreadStart ts = new ThreadStart(delegate()
            {
                //  SetStatus("开始批量生成........"); 

                this.Invoke(new Helper.DelegateHelper.DSet(Helper.DelegateHelper.SetVal), lblStatus, "正在删除原始文件......");
                string[] files = System.IO.Directory.GetFiles("data");
                foreach (string item in files)
                {
                    System.IO.File.Delete(item);
                }

                this.Invoke(new Helper.DelegateHelper.DSet(Helper.DelegateHelper.SetVal), lblStatus, "开始批量生成......");

                #region v1
                /*
                ListBox.SelectedObjectCollection Collection = listBox1.SelectedItems;
                string tbName = "";
                StringBuilder sb = new StringBuilder();
                foreach (object ob in Collection)
                {
                    tbName = ob.ToString();
                    sb.Append(CreateEntityScript(tbName, true));
                    sb.AppendLine();
                    sb.AppendLine();
                }
                   richTextBox1.Text = sb.ToString();
                 */
                #endregion

                #region v2
                CreateMode("");
                System.Diagnostics.Process.Start("explorer.exe", SaveFolder);
                #endregion

                btnBatch.Enabled = true;
                //SetStatus("批量生成完成，你可以点击 打开目录 按钮查看生成的文件");
                this.Invoke(new DStatus(SetStatus), lblStatus, "批量生成完成，你可以点击 打开目录 按钮查看生成的文件");

            });
            Thread t = new Thread(ts);
            t.Start();
        }

        private void SetStatus(Control c, string msg)
        {
            (c as Label).Text = msg;
            //   lblStatus.Text = msg;
            //Thread tt = new Thread(
            //   new ThreadStart(delegate()
            //   {
            //       lblStatus.Text = msg;
            //   })
            //);
            //tt.Start();
        }

        delegate void DStatus(Control c, string obj);

        private void btnConfirm_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBox1.SelectedValue.ToString() == "")
            {
                MessageBox.Show("请选择数据库类型", "Error", MessageBoxButtons.OK);
                return;
            }
            listBox2_SelectedIndexChanged(this, null);

            lblStatus.Text = "加载数据库...";
            comboBoxDb.DataSource = core.GetDatabases();// GetDatabase();
            lblStatus.Text = "加载数据库完成";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "d:\\bak";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                this.core = new OledbCore(textBox1.Text);
            }
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (comboBox1.SelectedValue.ToString() == "Oledb")
                {
                    listBox1.DataSource = core.GetTables("");
                }
            }

        }

        private void btnOpendir_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", AppDomain.CurrentDomain.BaseDirectory + "Data");
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

    }

}
