using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;
using System.Data;
using System.Net.Mail;

namespace ExpMQManager.BLL
{
    public abstract class GenerateBase : BaseDAC
    {
        public abstract string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId);

        public string buildUpBase(BaseEntity msgEntity, string msgType, string subType)
        {
            string strbaseAWB = "";

            #region build Message Header

            DateTime currentGMT = DateTime.UtcNow;
            string originAddr = "JFKCDCR";  //SASCSXH
            string curTime = currentGMT.ToString("dd") + currentGMT.ToString("HH") + currentGMT.ToString("mm");
            string priority = "QD";

            // if message was sent from e3 then use JFKCTCR. requested by Cecile on 2018-01-03.
            bool isEmail = false;
            try
            {
                MailAddress checkupEmail = new MailAddress(msgEntity.createdBy);
                isEmail = true;
                checkupEmail = null;
            }
            catch
            {
                isEmail = false;
            }
            if (isEmail)
            {
                originAddr = "JFKCTCR";
            }


            if (msgEntity.msgDestAddr == "")
                throw new Exception("No Message recipient is present msg QueueID: " + msgEntity.queueId);
            else if (msgEntity.msgDestAddr == "err")
                throw new Exception("Message exception error : " + msgEntity.queueId);

            //Build TypeB Destination Address
            string destAddrOut = "";
            string[] destAddr = msgEntity.msgDestAddr.ToUpper().Split(' ');
            foreach (string addr in destAddr)
            {
                //Filter-out Email Address
                if (addr != "")
                {
                    if (addr.IndexOf('@') == -1)
                    {
                        //Adding SITA TYPE-B Address
                        if (destAddrOut != "")
                            destAddrOut += " ";
                        destAddrOut += addr.Replace(",", "");

                    }
                    else
                    {
                        //Email Address
                        if (msgEntity.msgDestAddrEmail != "")
                            msgEntity.msgDestAddrEmail += ";";
                        msgEntity.msgDestAddrEmail += addr;
                    }
                }
            }

            strbaseAWB += "\r\n";
            strbaseAWB += priority + " " + destAddrOut + "\r\n";
            //strbaseAWB += priority + " " + "JFKCTCR" + " " + "EPICDCR" + "\r\n";    
            strbaseAWB += "." + originAddr + " " + curTime + "\r\n";

            #endregion

            if ((msgType.ToUpper() == "FSU" && subType.ToUpper() != "DIS") || msgType.ToUpper() == "FWB")
            {
                #region build AWB Header

                string weightFormatted = string.Format("{0:0.0}", msgEntity.weight);

                //char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicator[0]);
                char shipmentCode = 'T';
                strbaseAWB += msgType.ToUpper() + "/" + msgEntity.msgVersion + "\r\n";

                if (subType.ToUpper() != "DIS")
                {
                    strbaseAWB += msgEntity.prefix + "-" + msgEntity.awb + msgEntity.origin + msgEntity.dest + "/" + shipmentCode + msgEntity.pcs;
                    if (subType.ToUpper() == "FOH")
                    {
                        strbaseAWB += "\r\n";
                    }
                    else
                    {
                        strbaseAWB += "K" + weightFormatted + "\r\n";
                    }
                }
                
                #endregion
            }

            if (msgType.ToUpper() == "FFM")
            {
                #region build FFM Header

                strbaseAWB += msgType.ToUpper() + "/" + msgEntity.msgVersion + "\r\n";

                #endregion
            }

            if (msgType.ToUpper() == "FHL")
            {
                strbaseAWB += msgType.ToUpper() + "/" + msgEntity.msgVersion + "\r\n";
            }

            //NFM ADDED
            if (msgType.ToUpper() == "NFM")
            {
                strbaseAWB += msgType.ToUpper() + "/" + "00" + msgEntity.msgVersion + "\r\n";
            }

            if (msgType.ToUpper() == "UWS")
            {
                strbaseAWB += msgType.ToUpper() + "\r\n";
            }

            if (msgType.ToUpper() == "FBR")
            {

                strbaseAWB += msgType.ToUpper();

                // for now, FBR msg Version is 1 as defalut;
                //msgEntity.msgVersion = 1;

                strbaseAWB += "/" + msgEntity.msgVersion + "\r\n";
            }

            return strbaseAWB;
        }

        public string transMonth(String tmpm)
        {
            string tmpmonth = "";

            switch (tmpm)
            {
                case "01":
                    tmpmonth = "JAN";
                    break;
                case "02":
                    tmpmonth = "FEB";
                    break;
                case "03":
                    tmpmonth = "MAR";
                    break;
                case "04":
                    tmpmonth = "APR";
                    break;
                case "05":
                    tmpmonth = "MAY";
                    break;
                case "06":
                    tmpmonth = "JUN";
                    break;
                case "07":
                    tmpmonth = "JUL";
                    break;
                case "08":
                    tmpmonth = "AUG";
                    break;
                case "09":
                    tmpmonth = "SEP";
                    break;
                case "10":
                    tmpmonth = "OCT";
                    break;
                case "11":
                    tmpmonth = "NOV";
                    break;
                case "12":
                    tmpmonth = "DEC";
                    break;
            }

            return tmpmonth;

        }
        public char replaceShipmentIndicator(char code)
        {
            char charReturn;
            switch (code)
            {
                case 'D':
                    charReturn = 'P';
                    break;
                case 'S':
                    charReturn = 'T';
                    break;
                default:
                    charReturn = code;
                    break;
            }

            return charReturn;
        }
        public string truncateString(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }
        // 7/2 search by Lcode. 
        public int getTimezone(string lcode, int queueID)
        {
            string sql = "";

            if (lcode != null)
            {
                sql = @"
                    SELECT MAX(tzone) as tzone FROM Location Location WHERE Lcode = '{0}' GROUP BY Lcode
                    ";
                sql = string.Format(sql, lcode);

            }

            int result = 0;
            IDataReader readData = ExecuteReader(sql);
            if (readData.Read())
            {
                try
                {
                    result = Convert.ToInt32(readData["tzone"].ToString().Trim());
                }
                catch
                {
                    throw new Exception("GET IMP TIME ZONE ERROR : QueueID (" + queueId + ")");
                }
            }
            return result;

        }
    }
}
