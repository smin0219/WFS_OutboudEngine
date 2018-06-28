using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class FwbDAC : BaseDAC
    {
        public FwbEntity GetFWBInfoDAC(int mid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);

            string strSql = "";
            strSql = @"  SELECT MID ,Shipper ,ShpAddr ,ShpAddr2 ,ShpAddrCity 
	                           ,ShpAddrState ,ShpAddrCountry ,ShpAddrZipcode ,Cnee ,CneeAddr ,CneeAddrCity
	                           ,CneeAddrProvince ,CneeAddrCountry ,CneeAddrZipcode ,Replace(AgentName,'''','') AgentName ,AgentCode
	                           ,AgentCity ,Currency ,WTVAL ,WTVAL2 ,WTVAL3 ,DVCarriage 
	                           ,DVCustoms ,Insurance ,AgentCassAddr
                                ,
                                  CASE SHC WHEN 'OTHER' THEN 
                                    LTRIM(RTRIM(isnull(SHCNote,''))) + 
                                    LTRIM(RTRIM(isnull(SHC2,''))) + 
                                    LTRIM(RTRIM(isnull(SHC3,''))) + 
                                    LTRIM(RTRIM(isnull(SHC4,''))) + 
                                    LTRIM(RTRIM(isnull(SHC5,''))) 
                                  ELSE 
                                    LTRIM(RTRIM(isnull(SHC,''))) + 
                                    LTRIM(RTRIM(isnull(SHC2,''))) + 
                                    LTRIM(RTRIM(isnull(SHC3,''))) + 
                                    LTRIM(RTRIM(isnull(SHC4,''))) + 
                                    LTRIM(RTRIM(isnull(SHC5,''))) 
                                  END SHC
                               ,(SELECT TOP 1 Acode FROM Location WHERE Lcode = Exp_Master.Lcode) Acode 
                         FROM Exp_Master 
                         WHERE MID = {0}
                    ";
            strSql = string.Format(strSql, mid);

            FwbEntity fwbEntity = GetFWBfromReader(baseAWB, ExecuteReader(strSql));

            //Obtaining Rate Data
            #region Original
            strSql = @"   SELECT MID ,PCs ,Weight ,Class ,ChargeWeight
                                            ,Rate ,Total ,Type ,NatureGoods
                                     FROM Exp_IATA
                                     WHERE MID = {0}
                                ";
            strSql = string.Format(strSql, mid);
            fwbEntity = AddRateValFromReader(fwbEntity, ExecuteReader(strSql));
            #endregion

            // 2015-09-29 if Exp_IATA doesn't exist.. check new Table :: Exp_Master_RTD && _NG || _NC
            if (fwbEntity.colRTD == null || fwbEntity.colRTD.Count == 0)
            {
                #region 2015-09-29 new added
                // 2015-09-29 new RTD.
                strSql = @" select MID, r_numpieces as PCs, r_weight_p as [Weight], r_rateclasscd as Class, r_weight_w as ChargeWeight, 
                                r_ratecharge as Rate, 
                                CASE WHEN ISNUMERIC(r_chargeamt) = 1 then CONVERT(DECIMAL(10,2), r_chargeamt) else 
                                ( 
									(
										(CASE WHEN (ISNUMERIC(r_weight_w) = 1) THEN r_weight_w ELSE 0 END)
										* 
										(CASE WHEN (ISNUMERIC(r_ratecharge)  = 1) THEN CONVERT(DECIMAL(10,2), r_ratecharge) ELSE 0 end)
									)
								) end as Total,
                                '' as [Type], '' as NatureGoods
                        From exp_master_rtd
                        where mid = {0}
                        ";
                strSql = string.Format(strSql, mid);
                fwbEntity = AddRTDFromReader(fwbEntity, ExecuteReader(strSql));

                // 2015-09-29 RTD_NG || NC
                strSql = @"
                        select seq, r_goodsdataidc as Type, r_naturenqog as NatureGoods from exp_master_rtd_nc
                        where mid = {0}
                        union
                        select seq, r_goodsdataidg as Type, r_naturenqog as NatureGoods from exp_master_rtd_ng
                        where mid = {0}
                        ";
                strSql = string.Format(strSql, mid);
                fwbEntity = AddRTD_NGFromReader(fwbEntity, ExecuteReader(strSql));
                #endregion
            } 

            //Obtaining Rate Data
            strSql = @"   SELECT MID ,PcsWeight ,Width ,Lenght
                                ,Height ,unit ,Pcs ,VolWeight
                         FROM Exp_DimWt
                         WHERE MID = {0}
                    ";
            strSql = string.Format(strSql, mid);
            fwbEntity = AddVolumeFromReader(fwbEntity, ExecuteReader(strSql));

            //Obtaining Rate Data
            strSql = @"   SELECT MID ,ChargeCode ,ChargeTo Entitlement, Amount ChargeAmt, Type PrepaidIndicator
                         FROM EXP_OtherCharge
                         WHERE MID = {0}
                    ";
            strSql = string.Format(strSql, mid);

            fwbEntity = AddOtherChargeFromReader(fwbEntity, ExecuteReader(strSql));

            // 2015-09-29 if EXP_OtherCharge doesn't exist.. check new Table :: Exp_Master_OTH
            if(fwbEntity.colCharge == null || fwbEntity.colCharge.Count == 0)
            {
                #region 2015-09-29 new added
                strSql = @"   SELECT MID, o_otherchcd as ChargeCode, o_entitlementcd as Entitlement, o_chargeamt as ChargeAmt, o_pcother as PrepaidIndicator 
                            FROM Exp_Master_OTH
                            WHERE MID = {0}
                    ";
                strSql = string.Format(strSql, mid);

                fwbEntity = AddOTHFromReader(fwbEntity, ExecuteReader(strSql));
                #endregion
            }

            //OCI
            strSql = @"   select MID, HID, CountryCode, Infold, CustomsId, CustomsInfo from Exp_OCI
                         WHERE MID = {0} 
                    ";
            strSql = string.Format(strSql, mid);
            fwbEntity = AddOCIValFromReader(fwbEntity, ExecuteReader(strSql));

            if(fwbEntity.colOCI == null || fwbEntity.colOCI.Count == 0)
            {
                strSql = @"   SELECT MID, o_isocountrycd as CountryCode, o_informationid as Infold, o_customsinfoid as CustomsId, o_suppcustomsinfo as CustomsInfo 
                            FROM Exp_Master_OCI
                            WHERE MID = {0} 
                    ";
                strSql = string.Format(strSql, mid);
                fwbEntity = AddOCIFromReader(fwbEntity, ExecuteReader(strSql));
            }


            //2015-10-06 _SSR
            strSql = @" SELECT MID, Seq, s_ssr as SSRtext
                        FROM Exp_Master_SSR
                        WHERE MID = {0}
                        ORDER by Seq
                    ";
            strSql = string.Format(strSql, mid);
            fwbEntity = AddSSRFromReader(fwbEntity, ExecuteReader(strSql));


            //2015-10-06. PPD
            //2017-11-21. add PPD for WTVAL is NC. Requested by Cecile 2017-11-21 9:12am.  
            strSql = @"
                        DECLARE @typeSelector VARCHAR(5)
                        DECLARE @MID INT = {0}
                        DECLARE @ppdTotal	INT
                        DECLARE @colTotal	INT

                        SET @typeSelector = ( SELECT RTRIM(LTRIM(ISNULL(WTVAL, ''))) FROM Exp_Master WHERE MID = @MID)

                        IF(@typeselector = 'NC' or SUBSTRING(@typeselector, 1, 1) = 'P')
                        BEGIN
	                        SELECT	MID, 'P' as ChargeType, PPDTotalWeight as TotalWeight, PPDValuation as Valuation, PPDTaxes as Taxes, PPDDueAgent as DueAgent, PPDDueCarrier as DueCarrier, 
                                    CASE WHEN (PPDTotal is NULL or PPDTotal = 0) THEN (ISNULL(PPDTotalWeight, 0) + ISNULL(PPDValuation, 0) + ISNULL(PPDTaxes, 0) + ISNULL(PPDDueAgent, 0) + ISNULL(PPDDueCarrier, 0))
										ELSE PPDTotal END as Total
                            FROM Exp_Master 
                            WHERE MID = @MID
                        END
                        ElSE IF(SUBSTRING(@typeselector, 1, 1) = 'C')
                        BEGIN
	                        SELECT	MID, 'C' as ChargeType, COLTotalWeight as TotalWeight, COLValuation as Valuation, COLTaxes as Taxes, COLDueAgent as DueAgent, COLDueCarrier as DueCarrier,
                                    CASE WHEN (COLTotal is NULL or COLTotal = 0) THEN (ISNULL(COLTotalWeight, 0) + ISNULL(COLValuation, 0) + ISNULL(COLTaxes, 0) + ISNULL(COLDueAgent, 0) + ISNULL(COLDueCarrier, 0))
										ELSE COLTotal END as Total
                            FROM Exp_Master 
                            WHERE MID = @MID
                        END
                    ";
            strSql = string.Format(strSql, mid);
            fwbEntity = AddPPDFromReader(fwbEntity, ExecuteReader(strSql));


            //            //SHC added. 2015-10-21
            //            strSql = @"
            //                    SELECT CASE WHEN UPPER(SHC) = 'OTHER' THEN SHCnote ELSE SHC END as SHC,
            //                            SHC2, SHC3, SHC4, SHC5, SHC6, SHC7, SHC8, SHC9 FROM Exp_Master WHERE MID = {0}
            //                    ";
            //            strSql = string.Format(strSql, mid);
            //            fwbEntity = AddSHCFromReader(fwbEntity, ExecuteReader(strSql));



            //2018-05-30. OSI
            strSql = @"
                        SELECT * FROM Exp_Master_OSI WHERE MID = {0} ORDER BT Seq
                    ";
            strSql = string.Format(strSql, mid);
            fwbEntity = AddPPDFromReader(fwbEntity, ExecuteReader(strSql));



            return fwbEntity;
        }

        public FwbEntity GetFWBfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            FwbEntity fwbEntity = new FwbEntity();
            if (reader.Read())
            {
                double carriageVal = 0.00; try { carriageVal = Convert.ToDouble(reader["DVCarriage"].ToString()); }
                catch (Exception e)
                {
                }
                double customVal = 0.00; try { customVal = Convert.ToDouble(reader["DVCustoms"].ToString()); }
                catch (Exception e)
                {
                }
                double insuranceVal = 0.00; try { insuranceVal = Convert.ToDouble(reader["Insurance"].ToString()); }
                catch (Exception e)
                {
                }

                fwbEntity = new FwbEntity(
                    baseEntity,
                    reader["Shipper"].ToString().Trim(),
                    reader["ShpAddr"].ToString().Trim(),
                    reader["ShpAddr2"].ToString().Trim(),
                    reader["ShpAddrCity"].ToString().Trim(),
                    reader["ShpAddrState"].ToString().Trim(),
                    reader["ShpAddrCountry"].ToString().Trim(),
                    reader["ShpAddrZipcode"].ToString().Trim(),
                    reader["Cnee"].ToString().Trim(),
                    reader["CneeAddr"].ToString().Trim(),
                    reader["CneeAddrCity"].ToString().Trim(),
                    reader["CneeAddrProvince"].ToString().Trim(),
                    reader["CneeAddrCountry"].ToString().Trim(),
                    reader["CneeAddrZipcode"].ToString().Trim(),
                    reader["AgentName"].ToString().Trim(),
                    reader["AgentCode"].ToString().Trim(),
                    reader["AgentCassAddr"].ToString().Trim(),
                    reader["AgentCity"].ToString().Trim(),
                    reader["Currency"].ToString().Trim(),
                    reader["WTVAL"].ToString().Trim(),
                    reader["WTVAL2"].ToString().Trim(),
                    reader["WTVAL3"].ToString().Trim(),
                    carriageVal,
                    customVal,
                    insuranceVal,
                    reader["Acode"].ToString().Trim(),
                    reader["SHC"].ToString().Trim()
                    );

                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return fwbEntity;
            }

            return fwbEntity;
        }

        public FwbEntity AddRateValFromReader(FwbEntity fwbEntity, IDataReader reader)
        {

            while (reader.Read())
            {
                int pcs = 0; try { pcs = Convert.ToInt32(reader["pcs"]); }
                catch { }
                double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                catch (Exception e)
                {
                }
                double chgWeight = 0.00; try { chgWeight = Convert.ToDouble(reader["ChargeWeight"].ToString()); }
                catch (Exception e)
                {
                }
                double rateChg = 0.00; try { rateChg = Convert.ToDouble(reader["Rate"].ToString()); }
                catch (Exception e)
                {
                }
                double total = 0.00; try { total = Convert.ToDouble(reader["Total"].ToString()); }
                catch (Exception e)
                {
                }

                FwbRateEntity fwbRateEntity = new FwbRateEntity(
                    pcs,
                    weight,
                    reader["Class"].ToString(),
                    chgWeight,
                    rateChg,
                    total,
                    reader["Type"].ToString(),
                    reader["NatureGoods"].ToString());

                fwbEntity.colRTD.Add(fwbRateEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddRTDFromReader(FwbEntity fwbEntity, IDataReader reader)
        {
            while (reader.Read())
            {
                int pcs = 0; try { pcs = Convert.ToInt32(reader["pcs"]); }
                catch { }
                double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                catch (Exception e)
                {
                }
                double chgWeight = 0.00; try { chgWeight = Convert.ToDouble(reader["ChargeWeight"].ToString()); }
                catch (Exception e)
                {
                }
                double rateChg = 0.00; try { rateChg = Convert.ToDouble(reader["Rate"].ToString()); }
                catch (Exception e)
                {
                }
                double total = 0.00; try { total = Convert.ToDouble(reader["Total"].ToString()); }
                catch (Exception e)
                {
                }

                Fwb_newDB_RTDEntity fwb_newDB_RTDEntity = new Fwb_newDB_RTDEntity(
                    pcs,
                    weight,
                    reader["Class"].ToString(),
                    chgWeight,
                    rateChg,
                    total,
                    reader["Type"].ToString(),
                    reader["NatureGoods"].ToString());

                fwbEntity.colnewDBRTD.Add(fwb_newDB_RTDEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddRTD_NGFromReader(FwbEntity fwbEntity, IDataReader reader)
        {
            while(reader.Read())
            {
                int seq = 0; try { seq = Convert.ToInt32(reader["seq"]); }
                catch { }

                FwbNGEntity fwbNGEntity = new FwbNGEntity(
                    seq,
                    reader["Type"].ToString(),
                    reader["NatureGoods"].ToString());
                fwbEntity.colNG.Add(fwbNGEntity);
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddVolumeFromReader(FwbEntity fwbEntity, IDataReader reader)
        {

            while (reader.Read())
            {
                int pcs = 0; try { pcs = Convert.ToInt32(reader["pcs"]); }
                catch { }
                double pcsWeight = 0.00; try { pcsWeight = Convert.ToDouble(reader["pcsWeight"].ToString()); }
                catch (Exception e)
                {
                }
                double length = 0.00; try { length = Convert.ToDouble(reader["Lenght"].ToString()); }
                catch (Exception e)
                {
                }
                double width = 0.00; try { width = Convert.ToDouble(reader["width"].ToString()); }
                catch (Exception e)
                {
                }
                double height = 0.00; try { height = Convert.ToDouble(reader["height"].ToString()); }
                catch (Exception e)
                {
                }
                double volWeight = 0.00; try { volWeight = Convert.ToDouble(reader["volWeight"].ToString()); }
                catch (Exception e)
                {
                }


                FwbVolumeEntity fwbVolEntity = new FwbVolumeEntity(
                    pcsWeight,
                    width,
                    length,
                    height,
                    reader["unit"].ToString(),
                    pcs,
                    volWeight);

                fwbEntity.colVol.Add(fwbVolEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;

        }

        public FwbEntity AddOtherChargeFromReader(FwbEntity fwbEntity, IDataReader reader)
        {

            while (reader.Read())
            {
                double chargeAmt = 0.00; try { chargeAmt = Convert.ToDouble(reader["ChargeAmt"].ToString()); }
                catch (Exception e)
                {
                }


                FwbOtherChargeEntity fwbOtherChargeEntity = new FwbOtherChargeEntity(
                    reader["PrepaidIndicator"].ToString(),
                    reader["ChargeCode"].ToString(),
                    reader["Entitlement"].ToString(),
                    chargeAmt);

                fwbEntity.colCharge.Add(fwbOtherChargeEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddOTHFromReader(FwbEntity fwbEntity, IDataReader reader)
        {

            while (reader.Read())
            {
                double chargeAmt = 0.00; try { chargeAmt = Convert.ToDouble(reader["ChargeAmt"].ToString()); }
                catch (Exception e)
                {
                }

                Fwb_newDB_OTHEntity Fwb_newDB_OTHEntity = new Fwb_newDB_OTHEntity(
                    reader["PrepaidIndicator"].ToString(),
                    reader["ChargeCode"].ToString(),
                    reader["Entitlement"].ToString(),
                    chargeAmt);

                fwbEntity.colnewDBOTH.Add(Fwb_newDB_OTHEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        //////
        public FwbEntity AddOCIValFromReader(FwbEntity fwbEntity, IDataReader reader)
        {

            while (reader.Read())
            {

                FwbOCIEntity fwbOCIEntity = new FwbOCIEntity(
                    reader["CountryCode"].ToString(),
                    reader["Infold"].ToString(),
                    reader["CustomsId"].ToString(),
                    reader["CustomsInfo"].ToString()
                    );

                fwbEntity.colOCI.Add(fwbOCIEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddOCIFromReader(FwbEntity fwbEntity, IDataReader reader)
        {
            while (reader.Read())
            {
                Fwb_newDB_OCIEntity Fwb_newDB_OCIEntity = new Fwb_newDB_OCIEntity(
                    reader["CountryCode"].ToString(),
                    reader["Infold"].ToString(),
                    reader["CustomsId"].ToString(),
                    reader["CustomsInfo"].ToString()
                    );

                fwbEntity.colnewDBOCI.Add(Fwb_newDB_OCIEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }
        
        //2015-10-06 _SSR
        public FwbEntity AddSSRFromReader(FwbEntity fwbEntity, IDataReader reader)
        {
            while (reader.Read())
            {
                int mid = 0; try { mid = Convert.ToInt32(reader["MID"].ToString()); }
                catch (Exception e)
                {
                }
                int seq = 0; try { seq = Convert.ToInt32(reader["Seq"].ToString()); }
                catch (Exception e)
                {
                }
                Fwb_newDB_SSREntity Fwb_newDB_SSREntity = new Fwb_newDB_SSREntity(
                    mid,
                    seq,
                    reader["SSRtext"].ToString()
                    );

                fwbEntity.colnewDBSSR.Add(Fwb_newDB_SSREntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddPPDFromReader(FwbEntity fwbEntity, IDataReader reader)
        {
            if (reader.Read())
            {
                int mid = 0; try { mid = Convert.ToInt32(reader["MID"].ToString()); }
                catch (Exception e)
                {
                }
                decimal totalWeight = 0; try { totalWeight = Convert.ToDecimal(reader["TotalWeight"].ToString()); }
                catch (Exception e)
                {
                }
                decimal valuation = 0; try { valuation = Convert.ToDecimal(reader["Valuation"].ToString()); }
                catch (Exception e)
                {
                }
                decimal taxes = 0; try { taxes = Convert.ToDecimal(reader["Taxes"].ToString()); }
                catch (Exception e)
                {
                }
                decimal dueAgent = 0; try { dueAgent = Convert.ToDecimal(reader["DueAgent"].ToString()); }
                catch (Exception e)
                {
                }
                decimal dueCarrier = 0; try { dueCarrier = Convert.ToDecimal(reader["DueCarrier"].ToString()); }
                catch (Exception e)
                {
                }
                decimal total = 0; try { total = Convert.ToDecimal(reader["Total"].ToString()); }
                catch (Exception e)
                {
                }
                fwbEntity.colnewDBPPDCOL.ChargeType = reader["ChargeType"].ToString();
                fwbEntity.colnewDBPPDCOL.MID = mid;
                fwbEntity.colnewDBPPDCOL.TotalWeight = totalWeight;
                fwbEntity.colnewDBPPDCOL.Valuation = valuation;
                fwbEntity.colnewDBPPDCOL.Taxes = taxes;
                fwbEntity.colnewDBPPDCOL.DueAgent = dueAgent;
                fwbEntity.colnewDBPPDCOL.DueCarrier = dueCarrier;
                fwbEntity.colnewDBPPDCOL.Total = total;
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddSHCFromReader(FwbEntity fwbEntity, IDataReader reader)
        {
            if (reader.Read())
            {
                if(reader["SHC"].ToString() != null && reader["SHC"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC2"].ToString() != null && reader["SHC2"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC2"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC3"].ToString() != null && reader["SHC3"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC3"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC4"].ToString() != null && reader["SHC4"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC4"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC5"].ToString() != null && reader["SHC5"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC5"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC6"].ToString() != null && reader["SHC6"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC6"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC7"].ToString() != null && reader["SHC7"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC8"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC8"].ToString() != null && reader["SHC8"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC8"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
                if(reader["SHC9"].ToString() != null && reader["SHC9"].ToString() != string.Empty)
                {
                    Fwb_newDB_SHCEntity temp = new Fwb_newDB_SHCEntity(
                        reader["SHC9"].ToString()
                        );
                    fwbEntity.colnewSHC.Add(temp);
                }
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }

        public FwbEntity AddOSIFromReader(FwbEntity fwbEntity, IDataReader reader)
        {
            while (reader.Read())
            {
                Fwb_newDB_OSIEntity Fwb_newDB_OSIEntity = new Fwb_newDB_OSIEntity(
                    reader["SSRtext"].ToString()
                    );

                fwbEntity.colnewOSI.Add(Fwb_newDB_OSIEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fwbEntity;
        }
    }
}
