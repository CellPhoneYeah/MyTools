using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ChaYeFeng;
using System.Data.SqlClient;
using TestMEFInterface;

namespace TestFrm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            PluginFactory pluginFactory = new PluginFactory("TestMEF", @"..\..\REFDLL", "*.dll");
            ILog log = pluginFactory.GetPlugin<ILog>();
            MessageBox.Show(log.Prefix());
            //CYFSqlDALHelper helper = new CYFSqlDALHelper();
            //try
            //{
            //    string sql = "select * from testtable ";
            //    //DataTable dt = helper.ExecuteGetDataTable(sql);
            //    //string insert1 = " insert into testtable values('005','叶晓峰','1','13751732465','随便')";
            //    //string insert2 = " insert into testtable values('006','王者','0','13751732435','的')";
            //    //string insert3 = " insert into testtable values('007','王大者','0','13751756435','的')";
            //    //helper.ExecuteNonQuery(insert1);
            //    //helper.ExecuteNonQuery(insert2);
            //    //helper.ExecuteNonQuery(insert3);
            //    //helper.Commit();
            //    //this.dataGridView1.DataSource = dt;
            //    //DataSet ds = helper.ExecuteGetDataSet(sql);
            //    SqlDataReader reader = helper.ExecuteReader(sql);
            //    object result = reader.GetValue(0);
            //}
            //catch (Exception ex)
            //{
            //    helper.RollBack();
            //    CYFLog.WriteLog(ex.Message);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1_Load(null, null);
        }

    }
}
