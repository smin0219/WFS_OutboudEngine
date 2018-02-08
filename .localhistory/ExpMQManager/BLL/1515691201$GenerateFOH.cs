using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateFOH : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int flightSeq, int queueId)
        {
            FOHEntity fohEntity = new FohDAC().GetFOHInfoDAC(mid, flightSeq, msgType, subType, queueId);
            return buildUpFoh(fohEntity, msgType, subType);
        }
        public string buildUpFoh(FOHEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            /*TIMEZONE. 2015-07-02  */
            /*Local Time added      */
            int timezone = getTimezone(msgEntity.Lcode, msgEntity.queueId);
            msgEntity.fohTime = msgEntity.fohTime.AddHours(timezone);


            string fohTime = msgEntity.fohTime.ToString("dd") + transMonth(msgEntity.fohTime.ToString("MM")) +
                            msgEntity.fohTime.ToString("HH") + msgEntity.fohTime.ToString("mm");

            string shipper = truncateString(msgEntity.shipper, 35);




            strAWB += subType.ToUpper() + "/" + fohTime + "/" + msgEntity.forigin + "/" + "T" + msgEntity.pcs;

            if(shipper != null && shipper.Trim() != string.Empty)
            {
                strAWB += "/" + shipper.ToUpper();
            }

            return strAWB;
        }
    }
}
