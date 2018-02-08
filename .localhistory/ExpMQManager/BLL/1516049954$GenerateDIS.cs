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
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            string disTime = msgEntity.arrFlightMasterDate.ToString("dd") + transMonth(msgEntity.arrFlightMasterDate.ToString("MM"));
            disTime += msgEntity.arrCargoDate.ToString("HH") + msgEntity.arrCargoDate.ToString("mm");

            string weightFormatted = string.Format("{0:0.0}", msgEntity.weightRCF);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicatorRCF[0]);

            strAWB += msgEntity.prefix + "-" + msgEntity.awb + msgEntity.origin + msgEntity.dest + "/" + shipmentCode + msgEntity.manPcs + "\r\n";

            strAWB += subType.ToUpper() + "/" + msgEntity.flightNo + "/" + disTime + "/" + msgEntity.destFlight + "/" + msgEntity.disType + "/" +
                shipmentCode + msgEntity.locPcs + "K" + weightFormatted + "\r\n";

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
