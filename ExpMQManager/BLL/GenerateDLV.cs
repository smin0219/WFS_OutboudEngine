using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateDLV : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            DlvEntity dlvEntity = new DlvDAC().GetDLVInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            if (dlvEntity.pcsDLV == 0)
                throw new Exception("DLV PCs is zero. QueueID: " + dlvEntity.queueId);

            return buildUpDLV(dlvEntity, msgType, subType);
        }

        public string buildUpDLV(DlvEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);


            /*TIMEZONE. 2015-07-02  */
            /*Local Time added      */
            int timezone = getTimezone(msgEntity.Lcode, msgEntity.queueId);
            msgEntity.dlvTime = msgEntity.dlvTime.AddHours(timezone);

            string dlvTime = msgEntity.dlvTime.ToString("dd") + transMonth(msgEntity.dlvTime.ToString("MM")) +
                            msgEntity.dlvTime.ToString("HH") + msgEntity.dlvTime.ToString("mm");


            /*Original*/
            //string dlvTime = msgEntity.dlvTime.ToString("dd") + transMonth(msgEntity.dlvTime.ToString("MM")) +
            //                msgEntity.dlvTime.ToString("HH") + msgEntity.dlvTime.ToString("mm");
            
            
            string weightFormatted = string.Format("{0:0.0}", msgEntity.weight);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicator[0]);
            string cnee = truncateString(msgEntity.cnee, 35);


            //strAWB += subType.ToUpper() + "/" + dlvTime + "/" + msgEntity.destFlight + "/" + shipmentCode +
            //            msgEntity.pcsDLV + "K" + weightFormatted + "/" + cnee.ToUpper() + "\r\n";

            //strAWB += subType.ToUpper() + "/" + dlvTime + "/" + msgEntity.destFlight + "/" + shipmentCode +
            //            msgEntity.pcsDLV + "K" + weightFormatted;
            strAWB += subType.ToUpper() + "/" + dlvTime + "/" + msgEntity.dest + "/" + shipmentCode +
                        msgEntity.pcsDLV + "K" + msgEntity.weightDLV;

            if (cnee.ToString() != "")
                strAWB += "/" + cnee.ToUpper() + "\r\n";
            else
                strAWB += "\r\n";

            return strAWB;
        }

    }
}
