using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateRCT : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int flightSeq, int queueId)
        {
            RctEntity dlvEntity = new RctDAC().GetRCTInfoDAC(mid, flightSeq, msgType, subType, queueId);
            return buildUpRCT(dlvEntity, msgType, subType);
        }

        public string buildUpRCT(RctEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);


            /*TIMEZONE. 2015-07-02  */
            /*Local Time added      */
            int timezone = getTimezone(msgEntity.Lcode, msgEntity.queueId);
            msgEntity.rcsTime = msgEntity.rcsTime.AddHours(timezone);

            string rcsTime = msgEntity.rcsTime.ToString("dd") + transMonth(msgEntity.rcsTime.ToString("MM")) +
                            msgEntity.rcsTime.ToString("HH") + msgEntity.rcsTime.ToString("mm");

            /*Original*/
            //string rcsTime = msgEntity.rcsTime.ToString("dd") + transMonth(msgEntity.rcsTime.ToString("MM")) +
            //                msgEntity.rcsTime.ToString("HH") + msgEntity.rcsTime.ToString("mm");
            
            
            
            
            string weightFormatted = string.Format("{0:0.0}", msgEntity.weight);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicator[0]);
            string cnee = truncateString(msgEntity.cnee, 35);


            //strAWB += subType.ToUpper() + "/" + rcsTime + "/" + msgEntity.origin + "/" + shipmentCode +
            //            msgEntity.pcs + "K" + weightFormatted + "/" + cnee.ToUpper() + "\r\n";

            // 2014-04-07
            //strAWB += subType.ToUpper() + "/" + rcsTime + "/" + msgEntity.origin + "/" + shipmentCode +
            //            msgEntity.pcs + "K" + weightFormatted;


            // 2015-08-12 RDT added
            // IF SubMsgType == RDT then change it RCT.
            subType = subType.ToUpper();
            if (subType == "RDT")
                subType = "RCT";

            strAWB += subType.ToUpper() + "/" + "XD" + "/" + rcsTime + "/" + msgEntity.forigin + "/" + shipmentCode +
                        msgEntity.pcs + "K" + weightFormatted;

            //if (cnee.ToString() != "")
            //    strAWB += "/" + cnee.ToUpper() + "\r\n";
            //else
            strAWB += "\r\n";

            return strAWB;
        }

    }
}
