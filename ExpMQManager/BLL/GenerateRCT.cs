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
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            RctEntity rcsEntity = new RctDAC().GetRCTInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            return buildUpRCT(rcsEntity, msgType, subType);
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


            /*
             *   From: Cecile Kim 
             *   Sent: Wednesday, June 27, 2018 11:02 AM
             *   To: Alinxsoft <Alinxsoft@wfs.aero>
             *   Cc: Chuck Zhao <czhao@wfs.aero>; Michael Serzo <mserzo@wfs.aero>; David Johnson <david.johnson@wfs.aero>
             *   Subject: RCT
             *
             *   Alinx,
             *
             *   Please remove hardcoding of XD from RCT message carrier code section to Customer_Carrier.IsMain=’Y’ carrier code.
             *   Please treat this as an urgent matter since our RCT message is failing. (If Customer_Carrier.IsMain=='N', XD as default)
             *   Thank you.
             *
             **/


            try
            {

            }
            catch(Exception e)
            {
                return null;
            }

            string carrier = "";

            if (!string.IsNullOrEmpty(msgEntity.carrier))
            {
                carrier = msgEntity.carrier;
            }
            else
            {
                throw new Exception("There is no main carrier code");
            }

            strAWB += subType.ToUpper() + "/" + carrier + "/" + rcsTime + "/" + msgEntity.forigin + "/" + shipmentCode +
                        msgEntity.pcs + "K" + weightFormatted;

            //if (cnee.ToString() != "")
            //    strAWB += "/" + cnee.ToUpper() + "\r\n";
            //else
            strAWB += "\r\n";

            return strAWB;
        }

    }
}
