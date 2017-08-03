using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomBrowser
{
    public partial class Form1 : Form
    {

        private string url = string.Empty;
        private string homeUrl = "http://www.cnblogs.com";
        public Form1()
        {
            InitializeComponent();

        }

        //private bool CheckUrl()
        //{
        //    string addressStr = this.CustomBrowser.Url.OriginalString;
        //}

        private void ForwardNavigate()
        {
            url = tsbForward.Text;
            SetNavigate();
        }

        private void SetNavigate()
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    url = homeUrl;
                else
                {
                    this.CustomBrowser.Navigate(url);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void tsbBack_Click(object sender, EventArgs e)
        {
            this.CustomBrowser.GoBack();
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tstUrl.Text))
                this.CustomBrowser.Navigate(tstUrl.Text);
            else
                this.CustomBrowser.Navigate(homeUrl);
        }

        private void tsbForward_Click(object sender, EventArgs e)
        {
            this.CustomBrowser.GoForward();
        }

        private void tsbRequest_Click(object sender, EventArgs e)
        {
            this.CustomBrowser.Url = new Uri(tstUrl.Text);
            this.CustomBrowser.GoSearch();
        }
    }
}
