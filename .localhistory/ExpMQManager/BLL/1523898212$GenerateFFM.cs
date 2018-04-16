using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;
using System.Text.RegularExpressions;

namespace ExpMQManager.BLL
{
    public class GenerateFFM : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            BaseEntity baseEntity = new BaseDAC().GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            List<FfmEntity> ffmEntityCol = new FfmDAC().GetFFMColDAC(mid, flightSeq, msgType, subType, baseEntity.Lcode);
            return buildUpFFM(baseEntity, ffmEntityCol, msgType, subType);
        }

        public string buildUpFFM(BaseEntity baseEntity, List<FfmEntity> ffmEntityCol, string msgType, string subType)
        {
            //If no AWB Found
            if (ffmEntityCol.Count == 0)
                return "";

            string strAWB = "";

            int messageLimit = 50;
            int msgSeq = 1;
            int idx = 1;
            char seperator = '|';
            string prevULD = "";
            string portOfUnloading = "";

            string fltDate = baseEntity.flightDate.ToString("dd") + transMonth(baseEntity.flightDate.ToString("MM"));


            foreach (FfmEntity msgEntity in ffmEntityCol)
            {
                //Print Message Header
                if (idx == 1 || ((idx - 1) % messageLimit == 0))
                {
                    strAWB += base.buildUpBase(baseEntity, msgType, subType);
                    strAWB += msgSeq + "/" + baseEntity.flightNo.Trim() + "/" + fltDate + "/" + baseEntity.origin + "\r\n";
                }

                //Print Port of Unloading
                if (portOfUnloading != msgEntity.fDest.ToUpper())
                {
                    portOfUnloading = msgEntity.fDest.ToUpper();
                    strAWB += portOfUnloading + "\r\n";
                }

                //Print ULD
                if (msgEntity.uld.ToUpper() != "BULK")
                {
                    if (prevULD != msgEntity.uld.ToUpper())
                    {
                        strAWB += "ULD" + "/" + msgEntity.uld.ToUpper() + "\r\n";
                        prevULD = msgEntity.uld.ToUpper();
                    }
                }


                //Print AWB Consignment
                strAWB += msgEntity.prefix + "-" + msgEntity.awb + msgEntity.origin + msgEntity.dest + "/";

                //Added on 7/1 request. do not round weights for air china 
                // added. Ccode : IASKZDFW2, NCAJFK. 2016-02-11
                if (baseEntity.Ccode == "ARCHJFK" || baseEntity.Ccode == "IASKZDFW2" || baseEntity.Ccode == "NCAJFK"
                    || baseEntity.Ccode == "WFSSKBOS" || baseEntity.Ccode == "SASIAD" || baseEntity.Ccode == "SASEWR" || baseEntity.Ccode == "ARCHJFK1" // added on 2017-11-6. requested by Mike Serzo 11:46am
                    || baseEntity.Ccode == "IASQRIAH")  // added on 2018-4-9 16:39 requested by Cecile
                    strAWB += msgEntity.shipmentIndicator + msgEntity.fPcs + "K" + msgEntity.fWeight.ToString("F1");
                else
                {
                    //Added. 2015-12-16.
                    if (msgEntity.fWeight < 1)
                        strAWB += msgEntity.shipmentIndicator + msgEntity.fPcs + "K" + Math.Ceiling(msgEntity.fWeight);
                    else
                        strAWB += msgEntity.shipmentIndicator + msgEntity.fPcs + "K" + Math.Round(msgEntity.fWeight);
                }

                //Print Volume Detail
                strAWB += "MC";
                if (msgEntity.shipmentIndicator.ToUpper() == "T")
                    strAWB += string.Format("{0:0.00}", msgEntity.volWeight);
                else
                {
                    double volWeight = 0.00;
                    // 2014-04-18
                    //try { volWeight = ((msgEntity.volWeight / msgEntity.pcs) * msgEntity.fPcs); }
                    //catch { }
                    try { volWeight = ((msgEntity.volWeight / msgEntity.pcs) * msgEntity.builtPCs); }
                    catch { }
                    if (volWeight < 0.01)
                        volWeight = 0.01;
                    strAWB += string.Format("{0:0.00}", volWeight);
                }
                //Print Total Consignmnet Pieces
                if (msgEntity.shipmentIndicator.ToUpper() != "T")
                    strAWB += "T" + msgEntity.pcs;

                //Print Commodity
                // 2014-04-14
                //Regex rgx = new Regex("[^a-zA-Z0-9 -]");

                // add not filter dot('.'). requested by Cecile on 2018-4-16
                Regex rgx = new Regex("[^a-zA-Z0-9 -\\.]");
                string str = rgx.Replace(msgEntity.commodity, "");

                strAWB += "/" + truncateString(str, 15).ToUpper();

                // Block SHC 2014-05-13  ==> 2016-01-18 Release - ADD SHC code. Air China Only for now..
                #region SHC Code
                if (
                    (baseEntity.Ccode == "ARCHJFK" || baseEntity.Ccode == "ARCHJFK1" || baseEntity.Ccode == "ARCHEWR" || baseEntity.Ccode == "IASCADFW1" ||
                    baseEntity.Ccode == "SWRJFK" || baseEntity.Ccode == "SWREWR" || baseEntity.Ccode == "X ABCDFW" ||
                    baseEntity.Ccode == "WFSABMIA" || baseEntity.Ccode == "WFSABJFK" ||
                    baseEntity.Ccode == "IASEYDFW1" || baseEntity.Ccode == "ETDJFK" || baseEntity.Ccode == "WFSEYJFK" || baseEntity.Ccode == "WFSEYIAD" ||
                    baseEntity.Ccode == "AZMIA" || baseEntity.Ccode == "AZJFK1")

                    && baseEntity.msgVersion != 4
                    )
                {
                    if (msgEntity.shc != "")
                    {
                        string temp_SHC = msgEntity.shc;
                        temp_SHC = temp_SHC.Replace(" ", "").Trim();
                        strAWB += "\r\n";
                        int shcCount = 0;
                        shcCount = temp_SHC.Length / 3;
                        try
                        {
                            for (int c = 0; c < shcCount; c++)
                            {
                                strAWB += "/" + temp_SHC.Substring(c * 3, 3);
                            }
                        }
                        catch
                        {
                            throw new Exception("SHC data Error : MID (" + msgEntity.mid + ")");
                        }
                    }
                    else if (msgEntity.SHCList != null && msgEntity.SHCList.Count > 0)
                    {
                        try
                        {
                            int tempSHCIndex = 0;
                            foreach (FfmSHCEntity row in msgEntity.SHCList)
                            {
                                if(row.SHCcode != null && row.SHCcode.Trim().Length == 3)
                                {
                                    if(tempSHCIndex == 0)
                                        strAWB += "\r\n";

                                    strAWB += "/" + row.SHCcode;
                                }
                                tempSHCIndex++;
                            }
                        }
                        catch
                        {
                            throw new Exception("SHC data Error : MID (" + msgEntity.mid + ")");
                        }
                    }
                } 
                #endregion
                
                strAWB += "\r\n";

                //Print Message Tailer
                if (idx == ffmEntityCol.Count)
                {
                    strAWB += "LAST\r\n";
                    break;
                }
                else if (idx % messageLimit == 0)
                {
                    strAWB += "CONT\r\n";
                    strAWB += seperator;
                    portOfUnloading = "";
                    prevULD = "";
                    msgSeq++;
                }
                idx++;
            }

            return strAWB;
        }
    }
}
