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
            txtkey.Text = "1234567892587413";
            txtvector.Text = "9632587412589632";
            label1.Text = textBox1.Text.AesStr(txtkey.Text,txtvector.Text);
            label2.Text = label1.Text.UnAesStr(txtkey.Text, txtvector.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1_Load(null, null);
        }

    }
}
