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
                int _serverPort = 587;
                string _emailSender = "";
                string _serverID = "";
                string _serverPassword = "";

                try
                {

                    /**
                     * Commented and re-written by Jacob Min on 07/30/2018
                     * Getting server info from AWS database (serverinfo)
                     */

                    isLiveParsing = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveParsing"]);
                    _serverIP = ServerInfoDAC.Instance.ServerIP;
                    _serverPort = Convert.ToInt32(ServerInfoDAC.Instance.ServerPort);
                    _emailSender = ServerInfoDAC.Instance.SenderEmail;
                    _serverID = ServerInfoDAC.Instance.ServerID;
                    _serverPassword = ServerInfoDAC.Instance.ServerPassword;

                    // changed. get info from ServerInfo talbe. 2017-01-6
                    // _serverIP = ServerInfoDAC.Instance.ServerIP;
                    //try { _serverPort = Convert.ToInt32(ServerInfoDAC.Instance.ServerPort); } catch { _serverPort = 587; }
                    //_emailSender = ServerInfoDAC.Instance.SenderEmail;

                    //if (_serverIP.Equals("") || _serverIP == null)
                    //{
                    //    _serverIP = Convert.ToString(ConfigurationManager.AppSettings["EmailServerHostname"]);
                    //    _emailSender = "ePic@wfs.aero";
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
                    m.To.Add("ckim@wfs.aero");
                    m.To.Add("mserzo@wfs.aero");
                    m.To.Add("czhao@wfs.aero");
                    m.To.Add("david.johnson@wfs.aero");
                    m.To.Add("gwen.schulze@wfs.aero");
                    m.To.Add("jacob.min@wfs.aero");
                }

                // for monitoring
                if (!(Subject.IndexOf("SMT_") > -1))
                {
                    // removed requested by Mike on 2017-5-15
                    // added requested by CAS.
                    //m.To.Add(new MailAddress("epic@wfs.aero"));
                }

                //Optional
                if (_serverID != null && _serverID != "")
                {
                    smtp.Credentials = new NetworkCredential(_serverID, _serverPassword);
                    smtp.EnableSsl = true;
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
                    m.Body = Msg;
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
