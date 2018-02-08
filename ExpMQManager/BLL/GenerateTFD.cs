using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateTFD : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            TfdEntity tfdEntity = new TfdDAC().GetTfdInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            return buildUpTfd(tfdEntity, msgType, subType);
        }

        public string buildUpTfd(TfdEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            /*TIMEZONE. 2015-07-02  */
            /*Local Time added      */
            int timezone = getTimezone(msgEntity.Lcode, msgEntity.queueId);
            msgEntity.whcftime = msgEntity.whcftime.AddHours(timezone);

            string tfdTime = msgEntity.whcftime.ToString("dd") + transMonth(msgEntity.whcftime.ToString("MM")) +
                            msgEntity.whcftime.ToString("HH") + msgEntity.whcftime.ToString("mm");

            /*Original*/
            //string tfdTime = msgEntity.tday + transMonth(msgEntity.tmonth) + msgEntity.ttime;
            
            
            string weightFormatted = string.Format("{0:0.0}", msgEntity.weightTFD);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicator[0]);

            strAWB += subType.ToUpper() + "/" + msgEntity.carrier + "/" + tfdTime + "/" + msgEntity.OrgPort + "/" + shipmentCode + msgEntity.pcsTFD + "K" + weightFormatted + "\r\n"; ;
            
            return strAWB;
        }

    }
}
