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
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

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
            //CYFConfigHelper.GetSetting("PluginName");
            //CYFConfigHelper.GetConnectionString("test");
            //Encoding ed = Encoding.UTF8;
            //byte[] sourceByte = ed.GetBytes(txtSource.Text);
            //byte[] keyValue = ed.GetBytes(txtKey.Text);
            //byte[] vecValue = ed.GetBytes(txtvector.Text);
            //byte[] byteResult;
            //Rijndael rij = Rijndael.Create();
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (CryptoStream cs = new CryptoStream(ms,rij.CreateEncryptor(keyValue,vecValue),CryptoStreamMode.Write))
            //    {
            //        cs.Write(sourceByte, 0, sourceByte.Length);
            //        cs.FlushFinalBlock();
            //        byteResult = ms.ToArray();
            //        rtbResult.Text = Convert.ToBase64String(byteResult);
            //    }
            //}

            //byte[] tempValue = Convert.FromBase64String(rtbResult.Text.Trim());
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    using (CryptoStream cs = new CryptoStream(ms, rij.CreateDecryptor(keyValue, vecValue), CryptoStreamMode.Write))
            //    {
            //        cs.Write(tempValue, 0, tempValue.Length);
            //        cs.FlushFinalBlock();
            //        rtbResult.Text += "\t\n"+ed.GetString(ms.ToArray());
            //    }
            //}

            try
            {
                DataTable dt = new DataTable();
                DataColumn col = new DataColumn("col1", typeof(string));
                dt.Columns.Add(col);
                col = new DataColumn("col2", typeof(string));
                dt.Columns.Add(col);
                col = new DataColumn("col3", typeof(string));
                dt.Columns.Add(col);
                DataRow dr = dt.NewRow();
                dr.ItemArray = new object[] { "1", "回火", "啊啊" };
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr.ItemArray = new object[] { "2", "哦噢", "呃呃" };
                dt.Rows.Add(dr);
                XmlDocument xmldoc = CYFXMLHelper.CreateXmlDoc();
                CYFXMLHelper.CreateXmlFromDataTable(xmldoc, dt);
                rtbResult.Text = CYFXMLHelper.XmlDocToString(xmldoc, true, true);
                CYFXMLHelper.XmlToDataSet(rtbResult.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1_Load(null, null);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1_Load(null, null);
        }

    }
}
