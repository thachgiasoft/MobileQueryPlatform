using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LicenseTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = LicenseTool.GetMacByNetworkInterface();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string expDate;
            string serialNo=textBox1.Text;
            string reportNumber = textBox2.Text;
            if (checkBox1.Checked)
            {
                expDate = "99999999";
            }
            else
            {
                expDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            }

            string license = LicenseTool.GenerateLisense(expDate, serialNo, reportNumber);
            //加密
            textBox3.AppendText(license +"\r\n");

            //解密
            
            LicenseTool.AnalyzeLisense(license,serialNo,out expDate,out reportNumber);
            textBox3.AppendText("有效期至：" + expDate + "\r\n");
            textBox3.AppendText("报表数：" + reportNumber + "\r\n");
        }
    }
}
