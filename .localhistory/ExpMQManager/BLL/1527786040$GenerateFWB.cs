using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;

namespace ExpMQManager.BLL
{
    public class GenerateFWB : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int refID, int flightSeq, int queueId)
        {
            FwbEntity fwbEntity = new FwbDAC().GetFWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);


            return buildUpFWB(fwbEntity, msgType, subType);
        }

        public string buildUpFWB(FwbEntity msgEntity, string msgType, string subType)
        {

            //TIMEZONE UPDATE TEST ==> HOLD!!!
            //int timezone = getEXPTimezone(msgEntity.mid, null);

            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            DateTime nowDate = DateTime.Now;
            string msgDate = nowDate.ToString("dd") + transMonth(nowDate.ToString("MM")) + nowDate.ToString("yy");

            string weightFormatted = string.Format("{0:0.0}", msgEntity.weight);

            char shipmentCode = replaceShipmentIndicator(msgEntity.shipmentIndicator[0]);

            //RTG 

            // added. if Exp_Master.AWBPOU <> Exp_Master.Dest then add RTG. requested by Cecile 2017-12-13 10:46AM

            //strAWB += "RTG" + "/" + msgEntity.destFlight + msgEntity.carrier + "\r\n";    // original.
            strAWB += "RTG" + "/" + msgEntity.destFlight + msgEntity.carrier;
            if (msgEntity.carrier == "BA" && msgEntity.destFlight != msgEntity.dest)
                strAWB += "/" + msgEntity.dest + msgEntity.carrier;
            strAWB += "\r\n";

            //SHP
            strAWB += "SHP" + "\r\n";
            strAWB += "/" + truncateString(msgEntity.shipperNm, 35).Replace('/', '-') + "\r\n";
            strAWB += "/" + truncateString(msgEntity.shipperAddr + msgEntity.shipperAddr2, 35).Replace('/', '-') + "\r\n";
            strAWB += "/" + truncateString(msgEntity.shipperCity, 17);

            if (msgEntity.shipperState != "")
                strAWB += "/" + truncateString(msgEntity.shipperState, 9);

            strAWB += "\r\n";

            //strAWB += "/" + "US";
            strAWB += "/" + truncateString(msgEntity.shipperCountry == "" ? "US" : msgEntity.shipperCountry, 2);

            if (msgEntity.shipperZip != "")
                strAWB += "/" + truncateString(msgEntity.shipperZip, 9);

            if (!string.IsNullOrEmpty(msgEntity.shipperAddrTel))
            {
                if (msgEntity.shipperZip == "")
                    strAWB += "/";

                msgEntity.shipperAddrTel = string.Join("", msgEntity.shipperAddrTel.ToCharArray().Where(Char.IsDigit));
                strAWB += "/TE/" + truncateString(msgEntity.shipperAddrTel, 25);
            }
            strAWB += "\r\n";


            //CNE
            strAWB += "CNE" + "\r\n";
            strAWB += "/" + truncateString(msgEntity.cneeNm, 35).Replace('/', '-') + "\r\n";
            strAWB += "/" + truncateString(msgEntity.cneeAddr, 35).Replace('/', '-') + "\r\n";
            strAWB += "/" + truncateString(msgEntity.cneeCity, 17);
            if (msgEntity.cneeProv != "")
                strAWB += "/" + truncateString(msgEntity.cneeProv, 9);
            strAWB += "\r\n";

            strAWB += "/" + truncateString(msgEntity.cneeCountry, 2);

            if (msgEntity.cneeZip != "")
                strAWB += "/" + truncateString(msgEntity.cneeZip, 9);

            if (!string.IsNullOrEmpty(msgEntity.cneeAddrTel))
            {
                if (msgEntity.cneeZip == "")
                    strAWB += "/";

                msgEntity.cneeAddrTel = string.Join("", msgEntity.cneeAddrTel.ToCharArray().Where(Char.IsDigit));
                strAWB += "/TE/" + truncateString(msgEntity.cneeAddrTel, 25);
            }
            strAWB += "\r\n";      //"/" + truncateString(msgEntity.shipperZip, 9) +


            //AGT
            strAWB += "AGT" + "/" + "/" + msgEntity.agentIATACd;
            if (msgEntity.agentCASSaddr.Length == 4)
            {
                try
                {
                    //CASS Address Numbers Only
                    Convert.ToInt16(msgEntity.agentCASSaddr);
                    strAWB += "/" + msgEntity.agentCASSaddr;
                }
                catch { }
            }
            strAWB += "\r\n";
            strAWB += "/" + truncateString(msgEntity.agentNm, 35) + "\r\n";
            strAWB += "/" + truncateString(msgEntity.agentCity, 17) + "\r\n";


            //2015-10-06 SSR
            if (msgEntity.colnewDBSSR != null && msgEntity.colnewDBSSR.Count > 0)
            {
                if (msgEntity.colnewDBSSR.First().SSRtext != null && msgEntity.colnewDBSSR.First().SSRtext != string.Empty)
                {
                    strAWB += "SSR";
                    foreach (Fwb_newDB_SSREntity entity in msgEntity.colnewDBSSR)
                    {
                        strAWB += "/" + entity.SSRtext + "\r\n";
                    }
                }
            }

            //NFY
            if(!string.IsNullOrEmpty(msgEntity.nfyNm) && !string.IsNullOrEmpty(msgEntity.nfyAddr) && !string.IsNullOrEmpty(msgEntity.nfyPlace) && !string.IsNullOrEmpty(msgEntity.nfyCountry))
            {
                strAWB += "NFY/" + truncateString(msgEntity.nfyNm, 35) + "\r\n";
                strAWB += "/" + truncateString(msgEntity.nfyAddr, 35) + "\r\n";

                strAWB += "/" + truncateString(msgEntity.nfyPlace, 17);
                if (!string.IsNullOrEmpty(msgEntity.nfyState))
                    strAWB += "/" + truncateString(msgEntity.nfyState, 9);
                strAWB += "\r\n";

                strAWB += "/" + truncateString(msgEntity.nfyCountry, 2);
                if(!string.IsNullOrEmpty(msgEntity.nfyZip))
                    strAWB += "/" + truncateString(msgEntity.nfyZip, 9);

                if (!string.IsNullOrEmpty(msgEntity.nfyAddrTel))
                {
                    if (!string.IsNullOrEmpty(msgEntity.nfyZip))
                }

                    strAWB += "\r\n";

                if(!string.IsNullOrEmpty(msgEntity.nfyAddrTel))
                {
                    msgEntity.nfyAddrTel = string.Join("", msgEntity.nfyAddrTel.ToCharArray().Where(Char.IsDigit));
                    strAWB += "/TE/" + truncateString(msgEntity.nfyAddrTel, 25) + "\r\n";
                }
            }


            //CVD
            strAWB += "CVD" + "/" + msgEntity.currency + "/" + msgEntity.chargeCd + "/";
            strAWB += msgEntity.preChargeWeightCd + msgEntity.preChargeOtherCd + "/";

            if (msgEntity.carriageVal == 0.00)
                strAWB += "NVD" + "/";
            else
                strAWB += string.Format("{0:0.00}", msgEntity.carriageVal) + "/";

            if (msgEntity.customVal == 0.00)
                strAWB += "NCV" + "/";
            else
                strAWB += string.Format("{0:0.00}", msgEntity.customVal) + "/";

            if (msgEntity.insuranceVal == 0.00)
                strAWB += "XXX" + "\r\n";
            else
                strAWB += string.Format("{0:0.00}", msgEntity.insuranceVal) + "\r\n";


            int index = 1;
            double total = 0;
            int ngIndex = 1;

            //RTD
            if (msgEntity.colRTD != null && msgEntity.colRTD.Count > 0)
            {
                #region Original
                foreach (FwbRateEntity fwbRateEntity in msgEntity.colRTD)
                {
                    if (index == 1)
                        strAWB += "RTD";

                    if (index <= 11)
                    {
                        strAWB += "/" + index++ + "/" + "P" + fwbRateEntity.pcsRTD + "/";
                        strAWB += "K" + string.Format("{0:0.0}", fwbRateEntity.weightRTD) + "/";
                        if (fwbRateEntity.classRTD != null && fwbRateEntity.classRTD.Trim() != string.Empty)
                            strAWB += "C" + fwbRateEntity.classRTD + "/";
                        strAWB += "W" + string.Format("{0:0.0}", fwbRateEntity.chargeWeight) + "/";
                        strAWB += "R" + string.Format("{0:0.00}", fwbRateEntity.rateCharge) + "/";
                        strAWB += "T" + string.Format("{0:0.0}", fwbRateEntity.total) + "\r\n";
                        total = fwbRateEntity.total;

                        if (fwbRateEntity.natureOfGoods != "")
                        {
                            strAWB += "/" + "N" + fwbRateEntity.goodsType + "/";
                            strAWB += truncateString(fwbRateEntity.natureOfGoods, 20) + "\r\n";
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region newDB RTD :: Exp_Master_RTD && Exp_Master_RTD_NG||NC
                if (msgEntity.colnewDBRTD != null && msgEntity.colnewDBRTD.Count > 0)
                {
                    // 2015-09-29 new RTD
                    foreach (Fwb_newDB_RTDEntity fwb_newDB_RTDEntity in msgEntity.colnewDBRTD)
                    {
                        if (index == 1)
                            strAWB += "RTD";

                        if (index <= 11)
                        {
                            strAWB += "/" + index++ + "/" + "P" + fwb_newDB_RTDEntity.pcsRTD + "/";
                            strAWB += "K" + string.Format("{0:0.0}", fwb_newDB_RTDEntity.weightRTD) + "/";
                            if (fwb_newDB_RTDEntity.classRTD != null && fwb_newDB_RTDEntity.classRTD.Trim() != string.Empty)
                                strAWB += "C" + fwb_newDB_RTDEntity.classRTD + "/";
                            strAWB += "W" + string.Format("{0:0.0}", fwb_newDB_RTDEntity.chargeWeight) + "/";

                            string rateCharge = string.Format("{0:0.00}", fwb_newDB_RTDEntity.rateCharge);
                            if (rateCharge.Length > 8)
                            {
                                rateCharge = fwb_newDB_RTDEntity.rateCharge.ToString("G29");
                            }
                            strAWB += "R" + rateCharge + "/";

                            strAWB += "T" + string.Format("{0:0.00}", fwb_newDB_RTDEntity.total) + "\r\n";
                            total = fwb_newDB_RTDEntity.total;
                        }
                    }
                    foreach (FwbNGEntity fwbNGEntity in msgEntity.colNG)
                    {
                        if (ngIndex != 1)
                            strAWB += "/" + ngIndex;

                        strAWB += "/" + "N" + fwbNGEntity.type + "/" + truncateString(fwbNGEntity.naturegoods, 20) + "\r\n";
                        ngIndex++;
                    }
                }
                #endregion
            }

            // IF Turkish, NO DIM info. requested by Cecile on 2018-1-8. ==> HOLD.
            //if(msgEntity.Ccode != "TURKJFK" && msgEntity.Ccode != "X TURKIAH" && msgEntity.Ccode != "WFSTKIAD")
            if (msgEntity.Ccode != "X TURKIAH")
            {
                // 2016-3-18. added. 
                int cnt_volWeight = msgEntity.colVol.Where(x => x.volWeight > 0).Count();
                if (msgEntity.colVol.Count + (ngIndex - 1) + cnt_volWeight > 12)
                {
                    strAWB += "/2/ND//NDA" + "\r\n";
                }
                else
                {
                    index = ngIndex;
                    foreach (FwbVolumeEntity fwbVolEntity in msgEntity.colVol)
                    {
                        if (index <= 12)
                        {
                            strAWB += "/" + index++ + "/" + "ND" + "/";

                            if (fwbVolEntity.pcsWeight > 0)
                            {
                                //2015-09-15 BA&IB and others. also, decimal 1 (not 2)
                                if (msgEntity.carrier == "BA" || msgEntity.carrier == "IB")
                                {
                                    strAWB += "K" + Math.Round(fwbVolEntity.pcsWeight);
                                }
                                else
                                {
                                    strAWB += "K" + string.Format("{0:0.0}", fwbVolEntity.pcsWeight);
                                }
                            }
                            else
                            {
                                strAWB += "";
                            }
                            strAWB += "/";

                            if (fwbVolEntity.length > 0 && fwbVolEntity.width > 0 && fwbVolEntity.height > 0)
                                strAWB += fwbVolEntity.unitCode + Math.Round(fwbVolEntity.length) + "-" + Math.Round(fwbVolEntity.width) + "-" + Math.Round(fwbVolEntity.height) + "/" + fwbVolEntity.pcsDim;
                            else
                            {
                                if (msgEntity.msgVersion == 9)
                                {
                                    strAWB += "CMT0-0-0/" + msgEntity.pcs;
                                }
                                else
                                {
                                    strAWB += "NDA";
                                }
                            }

                            strAWB += "\r\n";

                            if (fwbVolEntity.volWeight > 0 && index <= 12)
                            {
                                strAWB += "/" + index++ + "/" + "NV" + "/";

                                // Always MC. 2016-05-18
                                strAWB += "MC";
                                if (fwbVolEntity.unitCode == "INH")
                                {
                                    strAWB += string.Format("{0:0.00}", (fwbVolEntity.volWeight / 166)) + "\r\n";
                                }
                                else
                                {
                                    strAWB += string.Format("{0:0.00}", fwbVolEntity.volWeight) + "\r\n";
                                }
                            }
                        }
                    }
                }
            } // IF NOT TURKISH airline. END.


            //OTH
            if (msgEntity.colCharge != null && msgEntity.colCharge.Count > 0)
            {
                index = 1;
                //2017-12-21 OTH modified
                foreach (FwbOtherChargeEntity fwbOtherChargeEntity in msgEntity.colCharge.Where(x => x.chargeAmt > 0))
                {
                    if (index == 1)
                        strAWB += "OTH";

                    strAWB += "/" + fwbOtherChargeEntity.prepaidIndicator + "/"
                        + fwbOtherChargeEntity.chargeCode + fwbOtherChargeEntity.entitlement
                        + string.Format("{0:0.00}", fwbOtherChargeEntity.chargeAmt) + "\r\n";

                    index++;
                }
            }
            else
            {
                if (msgEntity.colnewDBOTH != null && msgEntity.colnewDBOTH.Count > 0)
                {
                    int OTHindex = 1;
                    foreach (Fwb_newDB_OTHEntity fwb_newDB_OTHEntity in msgEntity.colnewDBOTH.Where(x => x.chargeAmt > 0))
                    {
                        if (OTHindex == 1)
                            strAWB += "OTH";

                        strAWB += "/" + fwb_newDB_OTHEntity.prepaidIndicator + "/"
                        + fwb_newDB_OTHEntity.chargeCode + fwb_newDB_OTHEntity.entitlement
                        + string.Format("{0:0.00}", fwb_newDB_OTHEntity.chargeAmt) + "\r\n";

                        OTHindex++;
                    }
                }
            }

            //PPD or COL
            if (msgEntity.colnewDBPPDCOL.hasPPDCOL)
            {
                if (msgEntity.colnewDBPPDCOL.ChargeType == "P")
                {
                    strAWB += "PPD";
                }
                else
                {
                    strAWB += "COL";
                }

                int tempcnt = 0;
                if (msgEntity.colnewDBPPDCOL.TotalWeight != 0)
                {
                    tempcnt = 1;
                    strAWB += "/";
                    strAWB += "WT" + msgEntity.colnewDBPPDCOL.TotalWeight;
                }
                else
                {
                    tempcnt = 1;
                    strAWB += "/";
                    strAWB += "WT0.00";
                }
                if (msgEntity.colnewDBPPDCOL.Valuation != 0)
                {
                    tempcnt = 1;
                    strAWB += "/";
                    strAWB += "VC" + msgEntity.colnewDBPPDCOL.Valuation;
                }
                if (msgEntity.colnewDBPPDCOL.Taxes != 0)
                {
                    tempcnt = 1;
                    strAWB += "/";
                    strAWB += "TX" + msgEntity.colnewDBPPDCOL.Taxes;
                }
                if (tempcnt == 0)
                    strAWB += "/";

                strAWB += "\r\n";

                tempcnt = 0;
                if (msgEntity.colnewDBPPDCOL.DueAgent != 0)
                {
                    tempcnt = 1;
                    strAWB += "/";
                    strAWB += "OA" + msgEntity.colnewDBPPDCOL.DueAgent;
                }
                if (msgEntity.colnewDBPPDCOL.DueCarrier != 0)
                {
                    tempcnt = 1;
                    strAWB += "/";
                    strAWB += "OC" + msgEntity.colnewDBPPDCOL.DueCarrier;
                }
                if (msgEntity.colnewDBPPDCOL.Total != 0)
                {
                    tempcnt = 1;
                    strAWB += "/";
                    strAWB += "CT" + msgEntity.colnewDBPPDCOL.Total;
                }

                if (tempcnt == 1)
                    strAWB += "\r\n";
            }
            else
            {
                //updated Original. 2015-10-19. PPD / COL check logic
                //strAWB += string.Format("PPD" + "/WT{0:0.0}", total) + "\r\n";
                //strAWB += string.Format("/CT{0:0.0}", total) + "\r\n";
                double chgDueCarrier = 0;
                double chgtotal = 0;

                if (msgEntity.colRTD != null && msgEntity.colRTD.Count > 0)
                {
                    chgtotal = msgEntity.colRTD.Sum(x => x.total);
                }
                if (msgEntity.colCharge != null && msgEntity.colCharge.Count > 0)
                {
                    chgDueCarrier = msgEntity.colCharge.Sum(x => x.chargeAmt);
                }

                if (chgtotal != 0 || chgDueCarrier != 0)
                {
                    if (msgEntity.chargeCd == "CC")
                    {
                        strAWB += "COL";
                    }
                    else
                    {
                        strAWB += "PPD";
                    }

                    if (chgtotal != 0)
                        strAWB += "/WT" + chgtotal + "\r\n";
                    else
                        strAWB += "\r\n";

                    if (chgDueCarrier != 0)
                    {
                        strAWB += "/OC" + chgDueCarrier;
                    }

                    strAWB += "/CT" + (chgtotal + chgDueCarrier) + "\r\n";
                }
            }

            //ISU
            strAWB += "ISU" + "/" + msgDate + "/" + msgEntity.awbPlace + "\r\n";

            //OSI
            if (msgEntity.colnewOSI != null && msgEntity.colnewOSI.Count() > 0)
            {
                index = 1;
                foreach (Fwb_newDB_OSIEntity fwbOSIEntity in msgEntity.colnewOSI)
                {
                    if (index > 3)
                        break;  // maximum 3 line

                    if (!string.IsNullOrEmpty(fwbOSIEntity.OtherInfo))
                    {
                        if (index == 1)
                            strAWB += "OSI";

                        strAWB += "/" + truncateString(fwbOSIEntity.OtherInfo, 65) + "\r\n";
                        index++;
                    }
                }
            }

            //REF
            strAWB += "REF" + "/" + "JFKCDCR" + "\r\n";

            #region SHC Original
            if (msgEntity.SHC.Trim() != "")
            {
                string temp_SHC = msgEntity.SHC;
                temp_SHC = temp_SHC.Replace(" ", "").Trim();
                strAWB += "SPH";
                int shcCount = 0;
                shcCount = temp_SHC.Length / 3;
                if (shcCount > 9)
                    shcCount = 9;
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
                strAWB += "\r\n";
            }
            #endregion

            ////SHC
            //if (msgEntity.colnewSHC != null && msgEntity.colnewSHC.Count > 0)
            //{
            //    index = 1;
            //    foreach (Fwb_newDB_SHCEntity fwbSHCEntity in msgEntity.colnewSHC)
            //    {
            //        if (fwbSHCEntity.SHCvalue != null && fwbSHCEntity.SHCvalue != string.Empty)
            //        {
            //            if (index == 1)
            //                strAWB += "SPH";
            //            strAWB += "/" + fwbSHCEntity.SHCvalue;
            //        }
            //        index++;
            //    }
            //    strAWB += "\r\n";
            //}

            //OCI
            if (msgEntity.colOCI != null && msgEntity.colOCI.Count > 0)
            {
                index = 1;
                foreach (FwbOCIEntity fwbOCIEntity in msgEntity.colOCI)
                {
                    if (index == 1)
                        strAWB += "OCI";
                    strAWB += "/" + fwbOCIEntity.CountryCode + "/" + fwbOCIEntity.Infold + "/" + fwbOCIEntity.CustomsId + "/" + fwbOCIEntity.CustomsInfo + "\r\n";
                    index++;
                }
            }
            else
            {
                if (msgEntity.colnewDBOCI != null && msgEntity.colnewDBOCI.Count > 0)
                {
                    if (msgEntity.colnewDBOCI.First().CustomsInfo != null && msgEntity.colnewDBOCI.First().CustomsInfo != string.Empty)
                    {
                        int OCIindex = 1;
                        foreach (Fwb_newDB_OCIEntity Fwb_newDB_OCIEntity in msgEntity.colnewDBOCI)
                        {
                            if (OCIindex == 1)
                                strAWB += "OCI";
                            strAWB += "/" + Fwb_newDB_OCIEntity.CountryCode + "/" + Fwb_newDB_OCIEntity.Infold + "/" + Fwb_newDB_OCIEntity.CustomsId + "/" + Fwb_newDB_OCIEntity.CustomsInfo + "\r\n";
                            OCIindex++;
                        }
                    }
                }
            }

            return strAWB.ToUpper();
        }

    }
}
