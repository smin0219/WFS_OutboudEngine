using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateFHL : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int hid, int flightSeq, int queueId)
        {
            FhlEntity fhlEntity = new FhlDAC().GetFHLInfoDAC(hid, flightSeq, msgType, subType, queueId);
            return buildUpFHL(fhlEntity, msgType, subType);
        }

        public string buildUpFHL(FhlEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            DateTime nowDate = DateTime.Now;
            string msgDate = nowDate.ToString("dd") + transMonth(nowDate.ToString("MM")) + nowDate.ToString("yy");

            string weightFormatted = string.Format("{0:0.0}", msgEntity.weight);

            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicator[0]);

            //MBI
            strAWB += "MBI" + "/" + msgEntity.prefix + "-" + msgEntity.awb + msgEntity.origin + msgEntity.dest + "/" + shipmentCode + msgEntity.pcs + "K" + msgEntity.weight + "\r\n";

            string hawb = msgEntity.HAWB;
            if (hawb != null && hawb.Length > 12)
                hawb = hawb.Substring(0, 12);
            hawb = hawb.Replace(" ", string.Empty);

            //HBS 
            strAWB += "HBS" + "/" + hawb + "/" + msgEntity.HOrigin + msgEntity.HDest + "/" + msgEntity.HPcs + "/" + "K" + string.Format("{0:0.0}",msgEntity.HWeight) + 
                    (msgEntity.SLAC == 0 ? "/" : "/" + msgEntity.SLAC )
                    + "/" + msgEntity.Commodity + "\r\n";


            if (msgEntity.SHC.Trim() != "")
            {
                string temp_SHC = msgEntity.SHC;
                temp_SHC = temp_SHC.Replace(" ", "").Trim();
                int shcCount = 0;
                shcCount = temp_SHC.Length / 3;
                try
                {
                    for (int c = 0; c < shcCount; c++)
                    {
                        strAWB += "/" + temp_SHC.Substring(c * 3, 3);
                    }
                }
                catch
                {
                    throw new Exception("SHC data Error : MID (" + msgEntity.mid + ")");
                }
                strAWB += "\r\n";
            }

            int index = 1;

            //TXT
            if (msgEntity.colTXT != null && msgEntity.colTXT.Count > 0)
            {
                foreach (FhlTXTEntity fhlTXTEntity in msgEntity.colTXT)
                {
                    if (index == 1)
                        strAWB += "TXT";
                    strAWB += "/" + fhlTXTEntity.Descr + "\r\n";
                    index++;

                    if (index == 10)
                        break;
                }
            }
            else
            {
                index = 1;
                foreach(FhlTXTEntity fhlTXTEntity in msgEntity.colnewDBTXT)
                {
                    if (index == 1)
                        strAWB += "TXT";
                    strAWB += "/" + fhlTXTEntity.Descr + "\r\n";
                    index++;

                    if (index == 10)
                        break;
                }
            }

            //HTS
            if (msgEntity.colHTS != null && msgEntity.colHTS.Count > 0)
            {
                index = 1;
                foreach (FhlHTSEntity fhlHTSEntity in msgEntity.colHTS)
                {
                    if (index == 1)
                        strAWB += "HTS";
                    strAWB += "/" + fhlHTSEntity.Descr + "\r\n";
                    index++;
                }
            }
            else
            {
                index = 1;
                foreach (FhlHTSEntity fhlHTSEntity in msgEntity.colnewDBHTS)
                {
                    if (index == 1)
                        strAWB += "HTS";
                    strAWB += "/" + fhlHTSEntity.Descr + "\r\n";
                    index++;
                }
            }

            //OCI
            if (msgEntity.colOCI != null && msgEntity.colOCI.Count > 0)
            {
                index = 1;
                foreach (FhlOCIEntity fhlOCIEntity in msgEntity.colOCI)
                {
                    if (index == 1)
                        strAWB += "OCI";
                    strAWB += "/" + fhlOCIEntity.CountryCode + "/" + fhlOCIEntity.Infold + "/" + fhlOCIEntity.CustomsId + "/" + fhlOCIEntity.CustomsInfo + "\r\n";
                    index++;
                }
            }
            else
            {
                index = 1;
                foreach (FhlOCIEntity fhlOCIEntity in msgEntity.colnewDBOCI)
                {
                    if (index == 1)
                        strAWB += "OCI";
                    strAWB += "/" + fhlOCIEntity.CountryCode + "/" + fhlOCIEntity.Infold + "/" + fhlOCIEntity.CustomsId + "/" + fhlOCIEntity.CustomsInfo + "\r\n";
                    index++;
                }
            }

            //SHP
            if (msgEntity.shipperNm != "")
            {
                strAWB += "SHP";
                strAWB += "/" + truncateString(msgEntity.shipperNm, 35).Replace('/', '-') + "\r\n";
                strAWB += "/" + truncateString(msgEntity.shipperAddr + msgEntity.shipperAddr2, 35).Replace('/', '-') + "\r\n";
                strAWB += "/" + truncateString(msgEntity.shipperCity, 17);

                if (msgEntity.shipperState != "")
                    strAWB += "/" + truncateString(msgEntity.shipperState, 9);

                strAWB += "\r\n";
                //strAWB += "/" + "US";

                strAWB += "/" + truncateString(msgEntity.shipperCountry == "" ? "US" : msgEntity.shipperCountry, 2);

                if (msgEntity.shipperZip.Trim() != "")
                {
                    strAWB += "/" + truncateString(msgEntity.shipperZip, 25);
                }

                if (msgEntity.shipperAddrTel != null && msgEntity.shipperAddrTel.Trim() != "")
                {
                    if (msgEntity.shipperContact == null || msgEntity.shipperContact.Trim() == "")
                        msgEntity.shipperContact = "TE";

                    if (msgEntity.shipperZip == null || msgEntity.shipperZip.Trim() == "")
                        strAWB += "/";

                    strAWB += "/" + truncateString(msgEntity.shipperContact.Trim(), 25) + "/" + truncateString(msgEntity.shipperAddrTel.Trim(), 25);
                }

                //strAWB += "/" + truncateString(msgEntity.shipperZip,9);
                //strAWB += "/" + truncateString(msgEntity.shipperContact,3);
                //strAWB += "/" + truncateString(msgEntity.shipperAddrTel,25);

                //if (msgEntity.shipperZip != "")
                //    strAWB += "/" + truncateString(msgEntity.shipperZip, 9);

                strAWB += "\r\n";
            }

            if (msgEntity.cneeNm != "")
            {
                //CNE
                strAWB += "CNE";
                strAWB += "/" + truncateString(msgEntity.cneeNm, 35).Replace('/', '-') + "\r\n";
                strAWB += "/" + truncateString(msgEntity.cneeAddr, 35).Replace('/', '-') + "\r\n";
                strAWB += "/" + truncateString(msgEntity.cneeCity, 17);
                if (msgEntity.cneeProv != "")
                    strAWB += "/" + truncateString(msgEntity.cneeProv, 9);
                strAWB += "\r\n";

                strAWB += "/" + truncateString(msgEntity.cneeCountry, 2);     //"/" + truncateString(msgEntity.shipperZip, 9) +

                if (msgEntity.cneeZip.Trim() != "")
                {
                    strAWB += "/" + truncateString(msgEntity.cneeZip, 25);
                }

                if(msgEntity.cneeAddrTel.Trim() != "")
                {
                    if (msgEntity.cneeContact == null || msgEntity.cneeContact.Trim() == "")
                        msgEntity.cneeContact = "TE";

                    if (msgEntity.cneeZip == null || msgEntity.cneeZip.Trim() == "")
                        strAWB += "/";

                    strAWB += "/" + truncateString(msgEntity.cneeContact, 25) + "/" + truncateString(msgEntity.cneeAddrTel, 25);
                }

                //strAWB += "/" + truncateString(msgEntity.cneeZip, 9);
                //strAWB += "/" + truncateString(msgEntity.cneeContact, 3);
                //strAWB += "/" + truncateString(msgEntity.cneeAddrTel, 25);

                strAWB += "\r\n";
            }
            //CVD
            //strAWB += "CVD" + "/" + msgEntity.currency + "/" + msgEntity.chargeCd + "/";
            if (msgEntity.currency != "")
            {
                strAWB += "CVD" + "/" + msgEntity.currency + "/";

                strAWB += msgEntity.preChargeWeightCd + msgEntity.preChargeOtherCd + "/";

                if (msgEntity.carriageVal == 0.00)
                    strAWB += "NVD" + "/";
                else
                    strAWB += string.Format("{0:0.00}", msgEntity.carriageVal) + "/";

                if (msgEntity.customVal == 0.00)
                    strAWB += "NCV" + "/";
                else
                    strAWB += string.Format("{0:0.00}", msgEntity.customVal) + "/";

                if (msgEntity.insuranceVal == 0.00)
                    strAWB += "XXX" + "\r\n";
                else
                    strAWB += string.Format("{0:0.00}", msgEntity.insuranceVal) + "\r\n";
            }
            

            return strAWB.ToUpper();
        }

    }
}
