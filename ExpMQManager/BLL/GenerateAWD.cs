using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateAWD : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            AwdEntity rcfEntity = new AwdDAC().GetAWDInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            return buildUpAWD(rcfEntity, msgType, subType);
        }

        public string buildUpAWD(AwdEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);


            /*TIMEZONE. 2015-07-02  */
            /*Local Time added      */

            // --> removed time zone. it is already local time 2016-1-13
            //int timezone = getTimezone(msgEntity.Lcode, msgEntity.queueId);
            //msgEntity.docAvailDate = msgEntity.docAvailDate.AddHours(timezone);

            string rcfTime = msgEntity.docAvailDate.ToString("dd") + transMonth(msgEntity.docAvailDate.ToString("MM")) +
                            msgEntity.docAvailDate.ToString("HH") + msgEntity.docAvailDate.ToString("mm");

            /*Original*/
            //string rcfTime = msgEntity.docAvailDate.ToString("dd") + transMonth(msgEntity.docAvailDate.ToString("MM")) +
            //                msgEntity.docAvailDate.ToString("HH") + msgEntity.docAvailDate.ToString("mm");
            
            
            string weightFormatted = string.Format("{0:0.0}", msgEntity.weightAWD);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicatorAWD[0]);
            string cnee = truncateString(msgEntity.cnee, 35);

            //strAWB += subType.ToUpper() + "/" + rcfTime + "/" + msgEntity.destFlight + "/" +
            //    shipmentCode + msgEntity.pcsAWD + "K" + weightFormatted + "/" + cnee + "\r\n";

            strAWB += subType.ToUpper() + "/" + rcfTime + "/" + msgEntity.destFlight + "/" +
                shipmentCode + msgEntity.pcsAWD + "K" + weightFormatted;

            if (cnee.ToString() != "")
                strAWB += "/" + cnee.ToUpper() + "\r\n";
            else
                strAWB += "\r\n";

            return strAWB;
        }
    }
}
