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
            List<DictionaryEntry> list = new List<DictionaryEntry>();
            list.Add(new DictionaryEntry("Md532", "Md532"));
            list.Add(new DictionaryEntry("加权MD5", "加权MD5"));
            list.Add(new DictionaryEntry("加权SHA1", "加权SHA1"));
            list.Add(new DictionaryEntry("SHA256", "SHA256"));
            list.Add(new DictionaryEntry("SHA512", "SHA512"));
            list.Add(new DictionaryEntry("HmacSha1", "HmacSha1"));
            list.Add(new DictionaryEntry("HmacSha256", "HmacSha256"));
            list.Add(new DictionaryEntry("HmacSha384", "HmacSha384"));
            list.Add(new DictionaryEntry("HmacSha512", "HmacSha512"));
            list.Add(new DictionaryEntry("HmacMd5", "HmacMd5"));
            list.Add(new DictionaryEntry("HmacRipeMd160", "HmacRipeMd160"));
            list.Add(new DictionaryEntry("AES", "AES"));
            list.Add(new DictionaryEntry("AES Byte", "AES Byte"));
            list.Add(new DictionaryEntry("DES", "DES"));
            list.Add(new DictionaryEntry("BASE64", "BASE64"));
            txtkey.Text = "1234567892587413";
            txtvector.Text = "9632587412589632";
            label1.Text = textBox1.Text.AesStr(txtkey.Text, txtvector.Text);
            label2.Text = label1.Text.UnAesStr(txtkey.Text, txtvector.Text);
            //List<string> list = new List<string>();
            //list.Add("'123'");
            //list.Add("'456'");
            //textBox1.Text = string.Join(",", list.ToArray());
        }

        private string EncryptStr(string source)
        {
            switch (comboBox1.Text)
            {
                case "Md532":
                    return source.Md532
                default:
                    break;
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
