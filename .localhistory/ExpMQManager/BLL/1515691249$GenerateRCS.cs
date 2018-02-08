using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateRCS : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            RcsEntity rcsEntity = new RcsDAC().GetRCSInfoDAC(mid, flightSeq, msgType, subType, queueId);
            return buildUpRCS(rcsEntity, msgType, subType);
        }

        public string buildUpRCS(RcsEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);


            /*TIMEZONE. 2015-07-02  */
            /*Local Time added      */
            int timezone = getTimezone(msgEntity.Lcode, msgEntity.queueId);
            msgEntity.rcsTime = msgEntity.rcsTime.AddHours(timezone);

            
            string rcsTime = msgEntity.rcsTime.ToString("dd") + transMonth(msgEntity.rcsTime.ToString("MM")) +
                            msgEntity.rcsTime.ToString("HH") + msgEntity.rcsTime.ToString("mm");
            
            /* ORIGINAL*/
            //string rcsTime = msgEntity.rcsTime.ToString("dd") + transMonth(msgEntity.rcsTime.ToString("MM")) +
            //                msgEntity.rcsTime.ToString("HH") + msgEntity.rcsTime.ToString("mm");



            string weightFormatted = string.Format("{0:0.0}", msgEntity.weight);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicator[0]);
            string cnee = truncateString(msgEntity.cnee, 35);
            string shipper = truncateString(msgEntity.shipper, 35);


            //strAWB += subType.ToUpper() + "/" + rcsTime + "/" + msgEntity.origin + "/" + shipmentCode +
            //            msgEntity.pcs + "K" + weightFormatted + "/" + cnee.ToUpper() + "\r\n";

            //strAWB += subType.ToUpper() + "/" + rcsTime + "/" + msgEntity.origin + "/" + shipmentCode +
            //            msgEntity.pcs + "K" + weightFormatted;




            // 2015-08-03 RDS added.
            // If SubMsgType == RDS then Change it RCS.
            subType = subType.ToUpper();
            if (subType == "RDS")
                subType = "RCS";

            // 2014-04-07
            strAWB += subType.ToUpper() + "/" + rcsTime + "/" + msgEntity.forigin + "/" + shipmentCode +
                        msgEntity.pcs + "K" + weightFormatted;

            if (shipper.ToString() != "")
                strAWB += "/" + shipper.ToUpper() + "\r\n";
            else
                strAWB += "\r\n";
            
            //if (cnee.ToString() != "")
            //    strAWB += "/" + cnee.ToUpper() + "\r\n";
            //else
            //    strAWB += "\r\n";

            return strAWB;
        }

    }
}
