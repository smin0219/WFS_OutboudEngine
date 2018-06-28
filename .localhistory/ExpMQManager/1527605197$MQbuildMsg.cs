using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using ExpMQManager.BLL;
using ExpMQManager.Util;

using System.Configuration;

namespace ExpMQManager
{
    public class MQbuildMsg
    {
        CASMqm myMQ = new CASMqm();
        BaseDB baseDB = new BaseDB();
        //baseMail bMail = new baseMail();

        bool isLive = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveParsing"]);

        public void getQueueList(ListBox listbox)
        {
            int queueid = 0;
            try
            {
                string strSql = "";
                if (isLive)
                {
                    strSql = @" SELECT iid, MsgType, subMsgType, MID, HID, RefID, FlightSeq, ResendYN, EDIAddressBook, CustomerId, MsgBody_SITAfreeMSG, MsgAddress_SITAfreeMSG FROM EDI_Msg_Queue WHERE Status = 'W' ORDER BY iid";
                    //strSql = @" SELECT iid, MsgType, subMsgType, MID, HID, RefID, FlightSeq, ResendYN, EDIAddressBook, CustomerId, MsgBody_SITAfreeMSG, MsgAddress_SITAfreeMSG from EDI_Msg_Queue where iid = 9060349 ";


                    //strSql = @" SELECT iid, MsgType, subMsgType, MID, HID, RefID, FlightSeq, ResendYN, EDIAddressBook, CustomerId, MsgBody_SITAfreeMSG, MsgAddress_SITAfreeMSG from EDI_Msg_Queue 
                    //            where iid in (16613038)";

                    //strSql = @" SELECT iid, MsgType, subMsgType, MID, HID, FlightSeq, ResendYN, EDIAddressBook, CustomerId FROM EDI_Msg_Queue WHERE iid = 4326345";
                    //strSql = @" select * from EDI_Msg_Queue where iid in (9733306)";

                    //strSql = @" select * from EDI_Msg_Queue where iid in (14397617)";

                }
                else
                {
                    strSql = @" select iid, MsgType, subMsgType, MID, HID, RefID, FlightSeq, ResendYN, EDIAddressBook, CustomerId, MsgBody_SITAfreeMSG, MsgAddress_SITAfreeMSG from EDI_Msg_Queue where iid in (18784961)";

                    //strSql = @" SELECT iid, MsgType, subMsgType, MID, HID, RefID, FlightSeq, ResendYN, EDIAddressBook, CustomerId, MsgBody_SITAfreeMSG, MsgAddress_SITAfreeMSG from EDI_Msg_Queue where Status = 'W' and createddate >= '2018-5-24' and (Msgtype <> 'Email' or SubMsgType = 'TTN')  ";
                    
                }

                DataTable dt = baseDB.GetSqlDataTable(strSql);
                string msgReturn = "";
                string result = "";

                foreach (DataRow dr in dt.Rows)
                {

                    queueid = Convert.ToInt32(dr["iid"].ToString());
                    string msgType = dr["MsgType"].ToString().Trim();
                    string subType = dr["subMsgType"].ToString().Trim();
                    int mid = 0;
                    try { mid = Convert.ToInt32(dr["MID"].ToString().Trim()); }
                    catch { }
                    int hid = 0;
                    try { hid = Convert.ToInt32(dr["HID"].ToString().Trim()); }
                    catch { }
                    int refID = 0;
                    try { refID = Convert.ToInt32(dr["refID"].ToString().Trim()); }
                    catch { }
                    int flightSeq = 0;
                    try { flightSeq = Convert.ToInt32(dr["FlightSeq"].ToString().Trim()); }
                    catch { }
                    string resendYN = dr["ResendYN"].ToString().Trim();
                    int EDIAddressBook = 0;
                    try { EDIAddressBook = Convert.ToInt32(dr["EDIAddressBook"].ToString().Trim()); }
                    catch { }
                    string CustomerId = dr["CustomerId"].ToString().Trim();
                    string MsgBody_SITA = dr["MsgBody_SITAfreeMSG"].ToString();
                    string MsgAddress_SITA = dr["MsgAddress_SITAfreeMSG"].ToString();



                    //if (msgType != "" && msgType != "Email")      // 2016-01-20 In case of Free Text, MsgType can be empty.
                    if (msgType.ToUpper() != "EMAIL")
                    {
                        GenerateBase baseMessage = null;

                        //added. 2015-12-14
                        if (MsgBody_SITA != null && MsgBody_SITA != string.Empty)
                        {
                            //for send free SITA MSG
                            baseMessage = new sendSITAmsg();
                        }
                        else
                        {
                            #region Init.
                            if (msgType.ToUpper() == "FSU")
                            {
                                switch (subType.ToUpper())
                                {
                                    case "DLV":
                                        baseMessage = new GenerateDLV();
                                        break;

                                    case "RCF":
                                    case "RTF": // added for realtime RCF. 2018-1-11
                                    case "ARR":
                                        baseMessage = new GenerateRCF();
                                        break;

                                    // added for realtime RCF. 2018-1-11
                                    case "DIS":
                                        baseMessage = new GenerateDIS();
                                        break;

                                    case "NFD":
                                    case "AWD":
                                        baseMessage = new GenerateAWD();
                                        break;

                                    case "MAN":
                                    case "DEP":
                                        baseMessage = new GenerateMAN();
                                        break;

                                    //RDS Added 2015-08-12
                                    case "RCS":
                                    case "RDS":
                                        baseMessage = new GenerateRCS();
                                        break;

                                    //RDT Added 2015-08-12
                                    case "RCT":
                                    case "RDT":
                                        baseMessage = new GenerateRCT();
                                        break;
                                    // FOH added. 2016-11-08
                                    case "FOH":
                                        baseMessage = new GenerateFOH();
                                        break;

                                    case "TFD":
                                        baseMessage = new GenerateTFD();
                                        break;
                                }
                            }
                            //NFM added by NA
                            if (msgType.ToUpper() == "NFM")
                                baseMessage = new GenerateNFM();

                            if (msgType.ToUpper() == "FFM")
                                baseMessage = new GenerateFFM();

                            if (msgType.ToUpper() == "IRP")
                                baseMessage = new GenerateIRP();

                            if (msgType.ToUpper() == "FWB")
                                baseMessage = new GenerateFWB();

                            if (msgType.ToUpper() == "FHL")
                                baseMessage = new GenerateFHL();

                            if (msgType.ToUpper() == "FBR")
                                baseMessage = new GenerateFBR();

                            if (msgType.ToUpper() == "UWS")
                                baseMessage = new GeneratwUWS();
                        }
                        #endregion


                        if (baseMessage != null)
                        {
                            //Clear Static Variables
                            baseMessage.msgDestAddrEmail = "";

                            //added. 2015-12-14
                            if (MsgBody_SITA != null && MsgBody_SITA != string.Empty)
                            {
                                //for send free SITA MSG
                                msgReturn = MsgBody_SITA;

                                //for email list
                                if (MsgAddress_SITA != null && MsgAddress_SITA != string.Empty)
                                {
                                    string[] tmpGetEmail = MsgAddress_SITA.Split(' ');
                                    if (tmpGetEmail.Count() > 0)
                                    {
                                        for (int i = 0; i < tmpGetEmail.Count(); i++)
                                        {
                                            if (tmpGetEmail[i].IndexOf("@") > -1)
                                            {
                                                baseMessage.msgDestAddrEmail += tmpGetEmail[i];
                                                baseMessage.msgDestAddrEmail += ";";
                                            }
                                        }
                                        baseMessage.msgDestAddrEmail = baseMessage.msgDestAddrEmail.TrimEnd(';');
                                    }
                                }
                            }
                            else
                            {
                                if (msgType.ToUpper() == "FHL")
                                {
                                    msgReturn = baseMessage.doBuildUp(msgType, subType, hid, refID, flightSeq, queueid);
                                }
                                else
                                {
                                    msgReturn = baseMessage.doBuildUp(msgType, subType, mid, refID, flightSeq, queueid);
                                }
                            }

                            if (msgReturn != "")
                            {
                                /* 2014-04-14
                                msgReturn = msgReturn.ToUpper();
                                */
                                msgReturn = msgReturn.ToUpper().Replace(",", "");

                                string[] arrMsg = msgReturn.Split('|');

                                foreach (string msg in arrMsg)
                                {
                                    if (ValidationUtil.isThereSitaReciever(msg))
                                    {
                                        string test = "good";
                                    }

                                    if (isLive)
                                    {
                                        if (ValidationUtil.isThereSitaReciever(msg))
                                        {
                                            result = myMQ.WriteLocalQMsg(msg, MQ_ManagerExp.GR1MQNMRInfo, MQ_ManagerExp.QUEUEID1, MQ_ManagerExp.GR1MQCONInfo, MQ_ManagerExp.GR1MQMInfo);
                                        }
                                        else
                                            result = "Message sent to successfully";
                                    }
                                    else
                                    {
                                        result = "Message sent to the queue successfully";
                                    }


                                    if (result.IndexOf("successful") > 0)
                                    {
                                        if (isLive)
                                        {
                                            baseMessage.UpdateQueue(queueid, "S", "");

                                            //added. 2015-12-14
                                            if (MsgBody_SITA != null && MsgBody_SITA != string.Empty)
                                            {
                                                baseMessage.InsertLogforFreeSITAmsg(queueid, msg);
                                            }
                                            else
                                                baseMessage.InsertLog(queueid, msg, msgType, subType);
                                        }
                                        else
                                        {
                                            //baseMessage.UpdateQueue(queueid, "S", "");
                                            //added. 2015-12-14
                                            if (MsgBody_SITA != null && MsgBody_SITA != string.Empty)
                                            {
                                                baseMessage.InsertLogforFreeSITAmsg(queueid, msg);
                                            }
                                            else
                                            {
                                                //baseMessage.InsertLog(queueid, msg, msgType, subType);
                                            }
                                        }

                                        //added. 2015-12-14
                                        if (MsgBody_SITA != null && MsgBody_SITA != string.Empty)
                                        { }
                                        else
                                        {
                                            //Create Copy to Cargo-Spot Queue
                                            if (msgType == "UWS" || msgType == "NFM" || msgType == "FFM" || msgType == "FWB" || (msgType == "FSU" && subType == "DEP") || (msgType == "FSU" && subType == "MAN"))
                                            {
                                                if (isLive)
                                                {
                                                    if (ValidationUtil.isThereSitaReciever(msg))
                                                    {
                                                        result = myMQ.WriteLocalQMsg(msg, MQ_ManagerExp.GR2MQNMRInfo, MQ_ManagerExp.QUEUEID2, MQ_ManagerExp.GR2MQCONInfo, MQ_ManagerExp.GR2MQMInfo);
                                                    }
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        baseMessage = new GenerateDLV();
                                        baseMessage.UpdateQueue(queueid, "E", result);
                                        buildLog(queueid, result, "CASMqm.WriteLocalQMsg");
                                    }


                                    // added on 2017-11-20. requested by Mike(2017-11-20 03:04pm)
                                    if (isLive)
                                    {
                                        // changed on 2017-11-30. all SITA message. requested by Cecile(2017-11-30 12:40pm)
                                        //if(msgType == "FWB" || msgType == "FHL" || msgType == "FHL")
                                        if (msgReturn != "")
                                        {
                                            if (baseMessage.msgDestAddrEmail != null && baseMessage.msgDestAddrEmail != "")
                                            {
                                                baseMessage.msgDestAddrEmail += ";nacs.wfs@cargoquality.com";
                                            }
                                            else
                                            {
                                                baseMessage.msgDestAddrEmail = "nacs.wfs@cargoquality.com";
                                            }
                                        }
                                    }

                                    if (!isLive)
                                    {
                                        baseMessage.msgDestAddrEmail = "cpark@wfs.aero;";
                                        //baseMessage.msgDestAddrEmail = "";
                                    }

                                    if (baseMessage.msgDestAddrEmail != "")
                                    {
                                        string emailBody = "";

                                        emailBody = setMSGformat(msgReturn, msgType ?? "");

                                        //emailBody = emailBody.Replace("\r\n", "<br>");
                                        //emailBody = msgReturn.Replace("\r\n", "<br>");

                                        string emailSubj = "";
                                        if (msgType != null && msgType.ToUpper() == "IRP")
                                        {
                                            emailSubj = "IRP Message: ";
                                            emailSubj += baseMessage.IRPSubject ?? "";
                                        }
                                        else if (subType != null && subType.Trim() != string.Empty)
                                        {
                                            if (subType.ToUpper() == "RTF")
                                                subType = "RCF";

                                            emailSubj = msgType.ToUpper() + " - " + subType.ToUpper() + " message";
                                        }
                                        else
                                        {
                                            emailSubj = msgType.ToUpper() + " message";
                                        }
                                        if (isLive)
                                        {
                                            GenerateEmail email = new GenerateEmail();
                                            int emailStatus = 1;
                                            try
                                            {
                                                bool mailSent = baseMail.mailSend(baseMessage.msgDestAddrEmail, emailBody, emailSubj);
                                                if (!mailSent)
                                                {
                                                    email.UpdateQueue(queueid, "S", "Error sending email!");
                                                    emailStatus = 255;
                                                    email.UpdateEmailQueue(mid, emailStatus);
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                email.UpdateQueue(queueid, "S", "Error sending email!");
                                                emailStatus = 255;
                                                email.UpdateEmailQueue(mid, emailStatus);
                                                buildLog(queueid, e.Message, e.StackTrace);
                                            }
                                        }
                                        else
                                        {
                                            bool mailSent = baseMail.mailSend(baseMessage.msgDestAddrEmail, emailBody, emailSubj);
                                        }
                                    }
                                }
                            }
                            //Clear Static Variables
                            baseMessage.msgDestAddrEmail = "";
                        }
                    }

                    //Email Sending 
                    #region Sending Email
                    if (msgType.ToUpper() == "EMAIL")
                    {
                        GenerateEmail email = new GenerateEmail();
                        int emailResult;
                        int emailStatus = 1;
                        try
                        {
                            emailResult = email.sendEamil(mid, subType);
                            if (emailResult == 0)
                            {
                                if (resendYN.ToUpper() == "Y")
                                    emailStatus = 3;
                                email.UpdateQueue(queueid, "S", "");
                                email.UpdateEmailQueue(mid, emailStatus);

                                if (listbox.Items.Count >= 100)
                                    listbox.Items.Clear();
                                listbox.Items.Add(string.Format("Email QueueId:{0} is successfully sent!", mid));
                            }
                            else if (emailResult == -2)
                            {
                                email.UpdateQueue(queueid, "E", "No receiver email address.");
                                emailStatus = 255;
                                email.UpdateEmailQueue(mid, emailStatus);
                            }
                            else
                            {
                                email.UpdateQueue(queueid, "E", "Error sending email!");
                                emailStatus = 255;
                                email.UpdateEmailQueue(mid, emailStatus);
                            }
                        }
                        catch (Exception e)
                        {
                            email.UpdateQueue(queueid, "E", e.Message);
                            emailStatus = 255;
                            email.UpdateEmailQueue(mid, emailStatus);
                            buildLog(queueid, e.Message, e.StackTrace);
                        }
                    }
                    #endregion
                }

            }
            catch (Exception e)
            {
                GenerateBase baseMessage = new GenerateDLV();
                baseMessage.UpdateQueue(queueid, "E", e.Message);
                buildLog(queueid, e.Message, e.StackTrace);
            }
        }
        protected string setMSGformat(string msgBody, string msgType)
        {
            string result = "";
            int countCheck = 0;
            int i = 0;

            msgBody = msgBody.Replace("\r\n", "|");
            msgBody = msgBody.Replace("||", "|");
            string[] tmpMSGbody = msgBody.Split('|');

            if (msgType.ToUpper() == "IRP")
            {
                for (i = 0; i < tmpMSGbody.Count(); i++)
                {
                    if (tmpMSGbody[i].IndexOf("FLIGHT NUMBER") > -1)
                    {
                        countCheck = i;
                        break;
                    }
                }
            }
            else
            {
                for (i = 0; i < tmpMSGbody.Count(); i++)
                {
                    if (tmpMSGbody[i].IndexOf('/') > -1)
                    {
                        countCheck = i;
                        break;
                    }
                }
            }

            for (i = countCheck; i < tmpMSGbody.Count(); i++)
            {
                if (tmpMSGbody[i] != string.Empty)
                {
                    result += tmpMSGbody[i];
                    result += Environment.NewLine;
                }
            }

            // only UWS, message type is reomved. becuase UWS doesn't have message version. (not working index "/")
            if (msgType.ToUpper() == "UWS")
            {
                result = "UWS\r\n" + result;
            }

            return result;
        }
        public static int buildLog(int queueId, string errorMsg, string errorDetail)
        {
            string logmsg = "";
            logmsg += "\t Queue ID: " + queueId + "\r\n";
            logmsg += "\t Error msg: " + errorMsg + "\r\n";
            logmsg += "\t Error Detail: " + errorDetail + "\r\n";
            logmsg += "===================================================================================================";
            //Logging to Logger
            try
            { int aa = Logger(logmsg); }
            catch (IOException ex)
            { return 1; }

            //Send Notification Email to Admin
            try
            { //baseMail.mailSend("EpicError@casusa.com", logmsg.Replace("\r\n","<br>"), "ExpMQManager - Error Notification"); 
            }
            catch (Exception e) { }

            return 0;
        }

        static int Logger(string log)
        {
            string nowDate = DateTime.Now.ToString("yyyyMMdd");
            string nowTime = DateTime.Now.ToString("HH':'mm':'ss");
            string fileName = "MQEngineEXP_" + nowDate + ".txt";
            string filePath = @"C:\temp\log\" + fileName;
            try
            {
                if (!File.Exists(filePath))
                {
                    StreamWriter sw = File.CreateText(filePath);
                    sw.Close();
                }
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    log = nowDate + " " + nowTime + " " + log;
                    sw.WriteLine(log);
                    sw.Close();
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
            return 0;
        }



    }
}
