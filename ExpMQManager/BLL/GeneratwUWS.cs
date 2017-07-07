using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;
using System.Text.RegularExpressions;

namespace ExpMQManager.BLL
{
    public class GeneratwUWS : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int flightSeq, int queueId)
        {
            BaseEntity baseEntity = new BaseDAC().GetBaseAWBInfoDAC(mid, flightSeq, msgType, subType, queueId);
            List<UwsEntity> uwsEntityCol = new UwsDAC().GetUWSColDAC(flightSeq);
            return buildupUWS(baseEntity, uwsEntityCol, msgType, subType);
        }
        public string buildupUWS(BaseEntity baseEntity, List<UwsEntity> uwsEntityCol, string msgType, string subType)
        {
            string strMSG = "";

            strMSG += base.buildUpBase(baseEntity, msgType, subType);

            // added. 2016-11-28. UTC time convert
            int tzone = getTimezone(baseEntity.Lcode, baseEntity.queueId);
            DateTime convertedUTC = baseEntity.flightDate.AddHours(tzone * -1).ToUniversalTime();
            string fltDate = convertedUTC.ToString("dd");

            //string fltDate = baseEntity.flightDate.ToString("dd");

            strMSG += baseEntity.flightNo + "/" + fltDate + "." + baseEntity.origin;
            if (uwsEntityCol.Where(x => x.weightindicator == "P").Count() == 0)
            {
                strMSG += ".FINAL";
            }
            strMSG += "\r\n";

            int bulkCNT = 0;
            foreach (UwsEntity row in uwsEntityCol)
            {
                if(row.uld != "BULK")
                {
                    // ULD
                    strMSG += "-" + row.uld + "/" + row.pou + "/" + row.weight.ToString("G29") + row.weightindicator + "/" + row.loadCategory;
                    if(row.shc != null && row.shc.Trim() != string.Empty)
                    {
                        strMSG += "." + row.shc;
                    }
                    if (row.shc2 != null && row.shc2.Trim() != string.Empty)
                    {
                        strMSG += "." + row.shc2;
                    }
                    if (row.shc3 != null && row.shc3.Trim() != string.Empty)
                    {
                        strMSG += "." + row.shc3;
                    }
                    strMSG += "\r\n";
                }
                else
                {
                    // BULK
                    if (bulkCNT == 0)
                    {
                        strMSG += "BULK" + "\r\n";
                        bulkCNT++;
                    }

                    strMSG += "-" + row.pou + "/" + row.weight.ToString("G29") + row.weightindicator + "/" + row.loadCategory;
                    if (row.shc != null && row.shc.Trim() != string.Empty)
                    {
                        strMSG += "." + row.shc;
                    }
                    if (row.shc2 != null && row.shc2.Trim() != string.Empty)
                    {
                        strMSG += "." + row.shc2;
                    }
                    if (row.shc3 != null && row.shc3.Trim() != string.Empty)
                    {
                        strMSG += "." + row.shc3;
                    }
                    strMSG += "\r\n";
                }
            }
            // requested by Mike 2016-11-30.
            // if there is no BULK, then add BULK line with wegiht 0.
            if(bulkCNT == 0)
            {
                strMSG += "BULK" + "\r\n";
                strMSG += "-" + baseEntity.dest + "/" + "0A" + "/" + "C";
            }

            strMSG = strMSG.TrimEnd('\r', '\n');
            return strMSG;
        }
    }
}
 