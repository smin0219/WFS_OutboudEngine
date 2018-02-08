using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateRCF : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int flightSeq, int queueId)
        {
            RcfEntity rcfEntity = new RcfDAC().GetRCFInfoDAC(mid, flightSeq, msgType, subType, queueId);
            return buildUpRCF(rcfEntity, msgType, subType);
        }

        public string buildUpRCF(RcfEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);


            //string rcfTime = msgEntity.arrCargoDate.ToString("dd") + transMonth(msgEntity.arrCargoDate.ToString("MM")) +
            //                msgEntity.arrCargoDate.ToString("HH") + msgEntity.arrCargoDate.ToString("mm");

            /*TIMEZONE. 2015-07-02  */
            /*Local Time added      */

            // --> removed time zone. it is already local time 2016-1-13
            //int timezone = getTimezone(msgEntity.Lcode, msgEntity.queueId);
            //msgEntity.arrCargoDate = msgEntity.arrCargoDate.AddHours(timezone);

            // chagned. 2017-7-6
            //string rcfTime = msgEntity.arrCargoDate.ToString("dd") + transMonth(msgEntity.arrCargoDate.ToString("MM")) +
            //                msgEntity.arrCargoDate.ToString("HH") + msgEntity.arrCargoDate.ToString("mm");


            // modified. 2017-7-6. use flight date with day change indicator.
            string rcfTime = msgEntity.arrFlightMasterDate.ToString("dd") + transMonth(msgEntity.arrFlightMasterDate.ToString("MM"));
            rcfTime += msgEntity.arrCargoDate.ToString("HH") + msgEntity.arrCargoDate.ToString("mm");

            try
            {
                if (msgEntity.arrCargoDate.Day != msgEntity.arrFlightMasterDate.Day)
                {
                    #region Added 2017-03-06. Indicating day changes
                    switch (msgEntity.arrCargoDate.Day - msgEntity.arrFlightMasterDate.Day)
                    {
                        case -1:
                            rcfTime = rcfTime + "-P";
                            break;
                        case 1:
                            rcfTime = rcfTime + "-N";
                            break;
                        case 2:
                            rcfTime = rcfTime + "-S";
                            break;
                        case 3:
                            rcfTime = rcfTime + "-T";
                            break;
                        case 4:
                            rcfTime = rcfTime + "-A";
                            break;
                        case 5:
                            rcfTime = rcfTime + "-B";
                            break;
                        case 6:
                            rcfTime = rcfTime + "-C";
                            break;
                        case 7:
                            rcfTime = rcfTime + "-D";
                            break;
                        case 8:
                            rcfTime = rcfTime + "-E";
                            break;
                        case 9:
                            rcfTime = rcfTime + "-F";
                            break;
                        case 10:
                            rcfTime = rcfTime + "-G";
                            break;
                        case 11:
                            rcfTime = rcfTime + "-H";
                            break;
                        case 12:
                            rcfTime = rcfTime + "-I";
                            break;
                        case 13:
                            rcfTime = rcfTime + "-J";
                            break;
                        case 14:
                            rcfTime = rcfTime + "-K";
                            break;
                        case 15:
                            rcfTime = rcfTime + "-L";
                            break;
                    }
                    #endregion
                }
            }
            catch { }

            string weightFormatted = string.Format("{0:0.0}", msgEntity.weightRCF);
            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicatorRCF[0]);

            if (subType.ToUpper() == "RTF")
                subType = "RCF";

            strAWB += subType.ToUpper() + "/" + msgEntity.flightNo + "/" + rcfTime + "/" + msgEntity.destFlight + "/" +
                shipmentCode + msgEntity.pcsRCF + "K" + weightFormatted + "\r\n";

            return strAWB;
        }

    }
}
