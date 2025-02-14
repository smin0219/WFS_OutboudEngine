﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using IBM.WMQ;

namespace ExpMQManager
{
    public partial class MQ_ManagerExp : Form
    {
        CASMqm myMQ = new CASMqm();
        MQbuildMsg buildUpMsg = new MQbuildMsg();

        public const int QUEUEID1 = 1;
        public const int QUEUEID2 = 2;

        //private Thread tGetRequestThread;
        public string Msg = null;
        // MQMessage mqMessage;
        public static string GR1MQMInfo = null;
        public static string GR1MQNMInfo1 = null;
        public static string GR1MQNMInfo2 = null;
        public static string GR1MQNMRInfo = null;
        public static string GR1MQCONInfo = null;

        public static string GR2MQMInfo = null;
        public static string GR2MQNMInfo1 = null;
        public static string GR2MQNMInfo2 = null;
        public static string GR2MQNMRInfo = null;
        public static string GR2MQCONInfo = null;

        public string Texttxt = "";

        // ---- ini 파일 의 읽고 쓰기를 위한 API 함수 선언 ----
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(    // ini Read 함수
                    String section,
                    String key,
                    String def,
                    StringBuilder retVal,
                    int size,
                    String filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(  // ini Write 함수
            String section,
            String key,
            String val,
            String filePath);

        public MQ_ManagerExp()
        {
            InitializeComponent();

            readiniM();
            textBox1.Text = GR1MQMInfo;
            textBox2.Text = GR1MQCONInfo;
            textBox3.Text = GR1MQNMRInfo;

            if (BaseDB.isLive)
            {
                label6.Text = "Live Mode";
                label6.ForeColor = Color.DarkMagenta;
            }
            else
            {
                label6.Text = "Test Mode";
                label6.ForeColor = Color.DarkCyan;
            }
        }

        public String G_IniReadValue(String Section, String Key, string avsPath)
        {

            StringBuilder temp = new StringBuilder(2000);
            int i = GetPrivateProfileString(Section, Key, "", temp, 2000, Application.StartupPath + @"\MQ.ini");
            //int i = GetPrivateProfileString(Section, Key, "", temp, 2000, avsPath);

            return temp.ToString();

        }

        /// write ini file
        public void G_IniWriteValue(String Section, String Key, String Value, string avsPath)
        {
            //WritePrivateProfileString(Section, Key, Value, avsPath);
            WritePrivateProfileString(Section, Key, Value, Application.StartupPath + @"\MQ.ini");
        }

        public void readiniM()
        {

            //GRP 1
            GR1MQMInfo = G_IniReadValue("GRP1", "MQM", "./MQ.ini");
            GR1MQNMInfo1 = G_IniReadValue("GRP1", "MQNM1", "./MQ.ini");
            GR1MQNMInfo2 = G_IniReadValue("GRP1", "MQNM2", "./MQ.ini");
            GR1MQNMRInfo = G_IniReadValue("GRP1", "MQNMR", "./MQ.ini");
            GR1MQCONInfo = G_IniReadValue("GRP1", "CONN", "./MQ.ini");

            //GRP 2
            GR2MQMInfo = G_IniReadValue("GRP2", "MQM", "./MQ.ini");
            GR2MQNMInfo1 = G_IniReadValue("GRP2", "MQNM1", "./MQ.ini");
            GR2MQNMInfo2 = G_IniReadValue("GRP2", "MQNM2", "./MQ.ini");
            GR2MQNMRInfo = G_IniReadValue("GRP2", "MQNMR", "./MQ.ini");
            GR2MQCONInfo = G_IniReadValue("GRP2", "CONN", "./MQ.ini");


        }

        /// Read From File INI

        public void readini()
        {

            G_IniWriteValue("GRP1", "MQM", textBox1.Text, "./MQ.ini");
            G_IniWriteValue("GRP1", "CONN", textBox2.Text, "./MQ.ini");
            G_IniWriteValue("GRP1", "MQNMR", textBox3.Text, "./MQ.ini");

            //GRP 1
            GR1MQMInfo = G_IniReadValue("GRP1", "MQM", "./MQ.ini");
            GR1MQNMInfo1 = G_IniReadValue("GRP1", "MQNM1", "./MQ.ini");
            GR1MQNMInfo2 = G_IniReadValue("GRP1", "MQNM2", "./MQ.ini");
            GR1MQNMRInfo = G_IniReadValue("GRP1", "MQNMR", "./MQ.ini");
            GR1MQCONInfo = G_IniReadValue("GRP1", "CONN", "./MQ.ini");

            //GRP 2
            GR2MQMInfo = G_IniReadValue("GRP2", "MQM", "./MQ.ini");
            GR2MQNMInfo1 = G_IniReadValue("GRP2", "MQNM1", "./MQ.ini");
            GR2MQNMInfo2 = G_IniReadValue("GRP2", "MQNM2", "./MQ.ini");
            GR2MQNMRInfo = G_IniReadValue("GRP2", "MQNMR", "./MQ.ini");
            GR2MQCONInfo = G_IniReadValue("GRP2", "CONN", "./MQ.ini");
        }

        private void MQ_ManagerExp_Load(object sender, EventArgs e)
        {
            readini();
            button1.PerformClick();
            //this.Close();
        }

        private void MQ_ManagerExp_Activated(object sender, EventArgs e)
        {
            readini();
        }

        private void MQ_ManagerExp_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true; // Show Form
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal; // Show Normal Form
            this.Activate(); // Active to form
        }

        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false; // Remove Icon from Tray
            notifyIcon1.Dispose();

            //    Application.Exit(); //Exit Application
            System.Diagnostics.Process[] mProcess = System.Diagnostics.Process.GetProcessesByName(Application.ProductName);
            foreach (System.Diagnostics.Process p in mProcess)
                p.Kill();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            buildUpMsg.getQueueList(listBox1);
            timer1.Start();
            Thread.Sleep(2000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text != "Stop")
            {
                Thread.Sleep(800);

                // BackgroundWorker작업이 끝날때까지 대기
                //if (Connect1())
                {
                    button1.Text = "Stop";
                    timer1.Interval = 2000;
                    timer1.Enabled = true;
                    timer1.Start();
                }
            }
            else
            {

                timer1.Stop();
                textBox4.Text = myMQ.DisconnectMQ(QUEUEID1);
                button1.Text = "Start";

            }
        }

        private bool Connect1()
        {
            string strQueueManagerName;
            string strChannelInfo;


            //TODO
            //PUT Valication Code Here

            strQueueManagerName = textBox1.Text;
            strChannelInfo = textBox2.Text;
            textBox4.Text = myMQ.ConnectMQ(strQueueManagerName, strChannelInfo, QUEUEID1);

            if (textBox4.Text.IndexOf("Successful") > 0)
                return true;
            else
                return false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
