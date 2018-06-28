using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

using System.Data;
using System.Configuration;

using ExpMQManager.DAL;

namespace ExpMQManager
{
    public class baseMail
    {
        public static DataTable ServerInfo = null;

        public static bool isLiveParsing = true;

        public baseMail()
        {
        }

        public static Boolean mailSend(string sAddress, string Msg, string Subject, bool needHTMLFormat = false)
        {
            try
            {
                string _serverIP = "";
                int _serverPort = 25;
                string _emailSender = "";

                try
                {
                    // changed. get info from ServerInfo talbe. 2017-01-6

                    isLiveParsing = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveParsing"]);
                    _serverIP = ServerInfoDAC.Instance.ServerIP;
                    try { _serverPort = Convert.ToInt32(ServerInfoDAC.Instance.ServerPort); } catch { _serverPort = 25; }
                    _emailSender = ServerInfoDAC.Instance.SenderEmail;

                    if (_serverIP.Equals("") || _serverIP == null)
                    {
                        _serverIP = Convert.ToString(ConfigurationManager.AppSettings["EmailServerIP"]);
                        _emailSender = "epicSupport@wfs.aero";
                    }
                }
                catch (Exception ex)
                {
                    //_serverIP = "10.3.0.71";

                    _serverIP = "192.168.188.68";
                    _serverPort = 25;
                    _emailSender = "epicSupport@wfs.aero";
                }

                SmtpClient smtp = new SmtpClient(_serverIP, _serverPort);
                MailMessage m = new MailMessage();
                m.From = new MailAddress(_emailSender, "Worldwide Flight Services");
                if (isLiveParsing)
                {
                    if (sAddress.IndexOf(";") > -1)
                    {
                        string[] result = sAddress.Split(';');
                        string msAddress = "";
                        for (int i = 0; i < result.Length; i++)
                        {
                            msAddress = result[i].ToString();
                            if (msAddress != null && msAddress != string.Empty)
                                m.To.Add(new MailAddress(msAddress));
                        }
                    }
                    else
                        m.To.Add(new MailAddress(sAddress));
                }
                else
                {
                    //m.To.Add("ckim@wfs.aero");
                    //m.To.Add("Nigeena.Popal@wfs.aero");
                    //m.To.Add("mserzo@wfs.aero");
                    //m.To.Add("mivanova@wfs.aero");
                    //m.To.Add("czhao@wfs.aero");
                    m.To.Add("cpark@wfs.aero");
                }

                // for monitoring
                if (!(Subject.IndexOf("SMT_") > -1))
                {
                    // removed requested by Mike on 2017-5-15
                    // added requested by CAS.
                    //m.To.Add(new MailAddress("epic@wfs.aero"));
                }

                m.Subject = Subject;
                
                if (needHTMLFormat)
                {
                    m.IsBodyHtml = true;
                    m.Body = Msg.Replace("\r\n", "<br />");
                }
                else
                {
                    m.IsBodyHtml = false;
                }

                smtp.Send(m);
                return true;

                //smtp.SendAsync(m,"CASUSA");
            }
            catch (Exception ex)
            {
                MQbuildMsg.buildLog(0, ex.Message, ex.StackTrace);        //Write a log to file I/O
                return false;
            }
        }


    }
}
