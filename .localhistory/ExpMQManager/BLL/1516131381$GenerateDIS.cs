using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateDIS : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            DisEntity disEntity = new DisDAC().GetDISInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            return buildUpDIS(disEntity, msgType, subType);
        }

        public string buildUpDIS(DisEntity msgEntity, string msgType, string subType)
        {
            if (msgEntity.weightRCF == 0)
                throw new Exception("DIS: weight is 0. QueueID: " + msgEntity.queueId);

            string strAWB = "";
            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicatorRCF[0]);

            string disTime = msgEntity.arrFlightMasterDate.ToString("dd") + transMonth(msgEntity.arrFlightMasterDate.ToString("MM"));
            disTime += msgEntity.arrCargoDate.ToString("HH") + msgEntity.arrCargoDate.ToString("mm");

            strAWB += msgEntity.disPrefix + "-" + msgEntity.disAWB + msgEntity.origin + msgEntity.dest + "/T" + msgEntity.manPcs + "\r\n";
            strAWB += subType.ToUpper() + "/" + msgEntity.flightNo + "/" + disTime + "/" + msgEntity.destFlight + "/" + msgEntity.disType + "/";

            if(msgEntity.disType == "FDAW" || msgEntity.disType == "MDAW")
            {
                strAWB += "T" + msgEntity.manPcs + "K" + string.Format("{0:0.0}", msgEntity.weightRCF) + "\r\n";
            }
            else if(msgEntity.disType == "MSCA")
            {
                int tempPCs = msgEntity.manPcs - msgEntity.locPcs;
                if(tempPCs < 0)
                    throw new Exception("DIS: Missing Pc is less than zero. QueueID: " + msgEntity.queueId);

                double tempWeight = (msgEntity.weightRCF / msgEntity.manPcs) * tempPCs;

                strAWB += "P" + msgEntity.locPcs+ "K" + string.Format("{0:0.0}", tempWeight) + "\r\n";
            }
            else if (msgEntity.disType == "FDCA")
            {
                int tempPCs = msgEntity.locPcs - msgEntity.manPcs;
                if (tempPCs < 0)
                    throw new Exception("DIS: Found Pc is less than zero. QueueID: " + msgEntity.queueId);

                double tempWeight = (msgEntity.weightRCF / msgEntity.manPcs) * tempPCs;
                strAWB += "P" + msgEntity.locPcs + "K" + string.Format("{0:0.0}", tempWeight) + "\r\n";
            }
            else
            {
                if(msgEntity.locPcs == 0)
                {
                    strAWB += "T" + msgEntity.manPcs+ "K" + string.Format("{0:0.0}", tempWeight) + "\r\n";
                }
                else
                {

                }
            }

            if (msgEntity.disOsi != null && msgEntity.disOsi != string.Empty)
            {
                strAWB += "OSI/";
                if (msgEntity.disOsi.Length > 65)
                {
                    strAWB += msgEntity.disOsi.Substring(0, 65) + "\r\n";
                    strAWB += "/" + msgEntity.disOsi.Substring(65, msgEntity.disOsi.Length > 130 ? 130 : msgEntity.disOsi.Length);
                }
                else
                {
                    strAWB += msgEntity.disOsi;
                }
            }

            return strAWB;
        }
    }
}
