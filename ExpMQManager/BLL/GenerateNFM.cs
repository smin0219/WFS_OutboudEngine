using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;
using System.Text.RegularExpressions;


namespace ExpMQManager.BLL
{
    public class GenerateNFM : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            BaseEntity baseEntity = new BaseDAC().GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            List<NfmEntity> nfmEntityCol = new NfmDAC().GetNFMColDAC(flightSeq);
            return buildupNFM(baseEntity, nfmEntityCol, msgType, subType);
        }
        
        public string buildupNFM(BaseEntity baseEntity, List<NfmEntity> nfmEntityCol, string msgType, string subType)
        {
            if (nfmEntityCol.Count() == 0)
                return "";

            string strMSG = "";
            //int messageLimit = 50;        // message line limit == no need
            //int msgseq = 1;                 // no limit, so no need seq
            string fltDate = baseEntity.flightDate.ToString("dd") + transMonth(baseEntity.flightDate.ToString("MM"));

            //Header + Flight Info
            strMSG += base.buildUpBase(baseEntity, msgType, subType);
            strMSG += "FLT/" + baseEntity.flightNo.Trim() + "/" + fltDate + "\r\n";
            strMSG += "RTG/" + baseEntity.origin + baseEntity.dest + "\r\n";

            // ULD list Info
            for (int i = 0; i < nfmEntityCol.Count(); i++ )
            {

                if (nfmEntityCol[i].uld.IndexOf("BUL") > -1)
                {

                }
                else
                {
                    strMSG += "ULD/" + nfmEntityCol[i].uld + "/" + nfmEntityCol[i].ehc + "/" + nfmEntityCol[i].ihc + "/" + "P" + nfmEntityCol[i].unitclass + "/";
                    strMSG += "O" + nfmEntityCol[i].overhangcnt + "/" + "GK" + nfmEntityCol[i].uldweight + "/" + "C" + nfmEntityCol[i].unitcontour + "/";
                    strMSG += "V" + nfmEntityCol[i].unitvolume + "\r\n";


                    nfmEntityCol[i].finaldest = nfmEntityCol[i].finaldest.Trim();
                    if (baseEntity.dest != nfmEntityCol[i].finaldest && (nfmEntityCol[i].ihc2 != null && nfmEntityCol[i].ihc2 != ""))
                    {
                        if (nfmEntityCol[i].finaldest != null && nfmEntityCol[i].finaldest != string.Empty)
                        {
                            strMSG += "/" + nfmEntityCol[i].finaldest + "/" + nfmEntityCol[i].ihc2 + "\r\n";
                        }
                        else
                        {
                            strMSG += "/" + nfmEntityCol[i].ihc2 + "\r\n";
                        }
                    }
                    else if (baseEntity.dest != nfmEntityCol[i].finaldest && (nfmEntityCol[i].ihc2 == null || nfmEntityCol[i].ihc2 == ""))
                    {
                        if (nfmEntityCol[i].finaldest != null && nfmEntityCol[i].finaldest != string.Empty)
                        {
                            strMSG += "/" + nfmEntityCol[i].finaldest + "\r\n";
                        }
                    }
                    else if (baseEntity.dest == nfmEntityCol[i].finaldest && (nfmEntityCol[i].ihc2 != null && nfmEntityCol[i].ihc2 != ""))
                    {
                        strMSG += "/" + nfmEntityCol[i].ihc2 + "\r\n";
                    }
                }
            }
                return strMSG;
        }
    }
}

