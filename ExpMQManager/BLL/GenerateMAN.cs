using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateMAN : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            ManEntity rcfEntity = new ManDAC().GetMANInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            return buildUpMAN(rcfEntity, msgType, subType);
        }

        public string buildUpMAN(ManEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            string depDate = msgEntity.depDate.ToString("dd") + transMonth(msgEntity.depDate.ToString("MM"));
            string depTime = msgEntity.depDate.ToString("HH") + msgEntity.depDate.ToString("mm");
            string arrTime = msgEntity.arrDate.ToString("HH") + msgEntity.arrDate.ToString("mm");
            string weightFormatted = string.Format("{0:0.0}", msgEntity.weightMAN);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicatorMAN[0]);

            // 2014-04-07
            //strAWB += subType.ToUpper() + "/" + msgEntity.flightNo + "/" + depDate + "/" +
            //    msgEntity.origin + msgEntity.destFlight + "/" + shipmentCode + msgEntity.pcsMAN + "K" + weightFormatted + "/";

            strAWB += subType.ToUpper() + "/" + msgEntity.flightNo.Trim() + "/" + depDate + "/" +
                msgEntity.forigin + msgEntity.destFlight + "/" + shipmentCode + msgEntity.pcsMAN + "K" + weightFormatted + "/";


            //if (subType.ToUpper() == "MAN")
            //    strAWB += "E" + msgEntity.estDepDate.ToString("HH") + msgEntity.estDepDate.ToString("mm");

            //if (subType.ToUpper() == "DEP")
            //    strAWB += "A" + msgEntity.actDepDate.ToString("HH") + msgEntity.actDepDate.ToString("mm");


            // Original. 2016-02-10
            //strAWB += msgEntity.depDateType + depTime + "/" + msgEntity.arrDateType + arrTime;
            strAWB += msgEntity.depDateType + depTime;


            // added. 2016-02-10
            if (msgEntity.Ccode == "IBEMIA" || msgEntity.Ccode == "IBEJFK")
            {
                // for now.
            }
            else
            {
                strAWB += "/" + msgEntity.arrDateType + arrTime;

                if (msgEntity.dayChangeIndicator != "")
                    strAWB += "-" + msgEntity.dayChangeIndicator.ToUpper();
            }

            strAWB += "\r\n";

            return strAWB;
        }

    }
}
