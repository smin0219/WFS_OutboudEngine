using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;
using System.Text.RegularExpressions;


namespace ExpMQManager.BLL
{
    public class GenerateFBR : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            BaseEntity baseEntity = new BaseDAC().GetBaseAWBInfoDAC(mid, refID,  flightSeq, msgType, subType, queueId);
            // need only baseEntity

            return buildupFBR(baseEntity, msgType, subType);
        }

        public string buildupFBR(BaseEntity baseEntity, string msgType, string subType)
        {
            string strMSG = "";

            // buildup header
            strMSG += base.buildUpBase(baseEntity, msgType, subType);

            string fltDate = baseEntity.flightDate.ToString("dd") + transMonth(baseEntity.flightDate.ToString("MM"));
            strMSG += "FLT/" + baseEntity.flightNo + "/" + fltDate + "/" + baseEntity.origin + baseEntity.dest;
            strMSG += "\r\n";
            strMSG += "REF/" + "JFKCDCR";

            return strMSG;
        }
    }
}

