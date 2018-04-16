using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class FhlDAC : BaseDAC
    {
        public FhlEntity GetFHLInfoDAC(int hid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(hid, refID, flightSeq, msgType, subType, queueId);

            string strSql = "";
            strSql = @"  SELECT hid , hawb, Pcs as HPcs, SLAC, Weight as HWeight, Dest as HDest, Origin as HOrigin, Commodity, Shipper ,ShpAddr ,ShpAddr2 ,ShpAddrCity
	                           ,ShpAddrState ,ShpAddrCountry ,ShpAddrZipcode , ShpAddrTel, ShpContact, Cnee ,CneeAddr ,CneeAddrCity
	                           ,CneeAddrProvince ,CneeAddrCountry ,CneeAddrZipcode , CneeAddrTel, CneeContact
                                ,Currency ,WTVAL ,WTVAL2 ,WTVAL3 ,DVCarriage ,DVCustoms ,Insurance 
                                ,'T' as Partial
                                --, isnull(( Select Partial From Exp_FlightMaster where mid = A.mid and OnPCS is not null group by Partial),'T') as Partial
                               --,(SELECT TOP 1 Acode FROM Location WHERE Lcode = Exp_Master.Lcode) Acode ,AgentName ,AgentCode ,AgentCity ,AgentCassAddr
                                ,
                                  CASE SHC WHEN 'OTHER' THEN 
                                    LTRIM(RTRIM(isnull(SHC,''))) + 
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
                         FROM Exp_House as A
                         WHERE hid = {0}
                    ";
            strSql = string.Format(strSql, hid);

            FhlEntity FhlEntity = GetFHLfromReader(baseAWB, ExecuteReader(strSql));

            //TXT
            #region Original
            strSql = @"   select MID, HID, Type, Descr from dbo.Exp_FHL_Line
                         WHERE HID = {0} and Type = 'TXT'
                    ";
            strSql = string.Format(strSql, hid);

            FhlEntity = AddTXTValFromReader(FhlEntity, ExecuteReader(strSql)); 
            #endregion

            // 2015-09-30 If Exp_FHL_Line not exsit, check _TXT
            if(FhlEntity.colTXT == null || FhlEntity.colTXT.Count == 0)
            {
                strSql = @"
                            SELECT t_txt as Descr FROM Exp_House_TXT
                            WHERE HID = {0}
                        ";
                strSql = string.Format(strSql, hid);

                FhlEntity = AddnewTXTFromReader(FhlEntity, ExecuteReader(strSql)); 
            }

            //HTS
            #region Original
            strSql = @"   select MID, HID, Type, Descr from dbo.Exp_FHL_Line
                         WHERE HID = {0} and Type = 'HTS'
                    ";
            strSql = string.Format(strSql, hid);

            FhlEntity = AddHTSValFromReader(FhlEntity, ExecuteReader(strSql)); 
            #endregion

            // 2015-09-30 If Exp_FHL_Line not exsit, check _HTS
            if(FhlEntity.colHTS == null || FhlEntity.colHTS.Count == 0)
            {
                strSql = @"
                            SELECT h_hccode as Descr FROM Exp_House_HTS
                            WHERE HID = {0}
                        ";
                strSql = string.Format(strSql, hid);

                FhlEntity = AddnewHTSFromReader(FhlEntity, ExecuteReader(strSql)); 
            }

            //OCI
            #region Original
            strSql = @"   select MID, HID, CountryCode, Infold, CustomsId, CustomsInfo from Exp_OCI
                         WHERE HID = {0} 
                    ";
            strSql = string.Format(strSql, hid);

            FhlEntity = AddOCIValFromReader(FhlEntity, ExecuteReader(strSql)); 
            #endregion

            // 2015-09-30 If Exp_FHL_Line not exsit, check _OCI
            if(FhlEntity.colOCI == null || FhlEntity.colOCI.Count == 0)
            {
                strSql = @" SELECT o_cntcode as CountryCode, o_infoident as Infold, o_cinfoident as CustomsId, o_scinfo as CustomsInfo 
                            FROM Exp_House_OCI
                            WHERE HID = {0} 
                    ";
                strSql = string.Format(strSql, hid);

                FhlEntity = AddnewOCIFromReader(FhlEntity, ExecuteReader(strSql)); 
            }

            #region Comment

            //            //Obtaining Rate Data
            //            strSql = @"   SELECT MID ,PcsWeight ,Width ,Lenght
            //                                ,Height ,unit ,Pcs ,VolWeight
            //                         FROM Exp_DimWt
            //                         WHERE MID = {0}
            //                    ";
            //            strSql = string.Format(strSql, hid);

            //            FhlEntity = AddVolumeFromReader(FhlEntity, ExecuteReader(strSql));

            //            //Obtaining Rate Data
            //            strSql = @"   SELECT MID ,ChargeCode ,ChargeTo Entitlement, Amount ChargeAmt, Type PrepaidIndicator
            //                         FROM EXP_OtherCharge
            //                         WHERE MID = {0}
            //                    ";
            //            strSql = string.Format(strSql, hid);

            //            FhlEntity = AddOtherChargeFromReader(FhlEntity, ExecuteReader(strSql)); 
            #endregion

            return FhlEntity;
        }
        public FhlEntity GetFHLfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            FhlEntity FhlEntity = new FhlEntity();
            if (reader.Read())
            {
                int hpcs = 0; try { hpcs = Convert.ToInt32(reader["HPcs"].ToString()); }
                catch (Exception e)
                {
                }
                int SLAC = 0; try { SLAC = Convert.ToInt32(reader["SLAC"].ToString()); }
                catch (Exception e)
                {
                }
                decimal hweight = 0; try { hweight = Convert.ToDecimal(reader["HWeight"].ToString()); }
                catch (Exception e)
                {
                }
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

                FhlEntity = new FhlEntity(
                    baseEntity,
                    reader["HAWB"].ToString().Trim(),
                    hpcs,
                    SLAC,
                    hweight,
                    reader["HDest"].ToString().Trim(),
                    reader["HOrigin"].ToString().Trim(),
                    reader["Partial"].ToString().Trim(),
                    reader["Commodity"].ToString().Trim(),
                    reader["Shipper"].ToString().Trim(),
                    reader["ShpAddr"].ToString().Trim(),
                    reader["ShpAddr2"].ToString().Trim(),
                    reader["ShpAddrCity"].ToString().Trim(),
                    reader["ShpAddrState"].ToString().Trim(),
                    reader["ShpAddrCountry"].ToString().Trim(),
                    reader["ShpAddrZipcode"].ToString().Trim(),
                    reader["ShpAddrTel"].ToString().Trim(),
                    reader["ShpContact"].ToString().Trim(),
                    reader["Cnee"].ToString().Trim(),
                    reader["CneeAddr"].ToString().Trim(),
                    reader["CneeAddrCity"].ToString().Trim(),
                    reader["CneeAddrProvince"].ToString().Trim(),
                    reader["CneeAddrCountry"].ToString().Trim(),
                    reader["CneeAddrZipcode"].ToString().Trim(),
                    reader["CneeAddrTel"].ToString().Trim(),
                    reader["CneeContact"].ToString().Trim(),
                    //reader["AgentName"].ToString().Trim(),
                    //reader["AgentCode"].ToString().Trim(),
                    //reader["AgentCassAddr"].ToString().Trim(),
                    //reader["AgentCity"].ToString().Trim(),
                    reader["Currency"].ToString().Trim(),
                    reader["WTVAL"].ToString().Trim(),
                    reader["WTVAL2"].ToString().Trim(),
                    reader["WTVAL3"].ToString().Trim(),
                    carriageVal,
                    customVal,
                    insuranceVal,
                    reader["SHC"].ToString().Trim()
                    //reader["Acode"].ToString().Trim()
                    );

                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return FhlEntity;
            }

            return FhlEntity;
        }
        public FhlEntity AddTXTValFromReader(FhlEntity fhlEntity, IDataReader reader)
        {

            while (reader.Read())
            {

                FhlTXTEntity fhlTXTEntity = new FhlTXTEntity(
                    reader["Descr"].ToString());

                fhlEntity.colTXT.Add(fhlTXTEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fhlEntity;
        }
        public FhlEntity AddnewTXTFromReader(FhlEntity fhlEntity, IDataReader reader)
        {
            while (reader.Read())
            {

                FhlTXTEntity fhlTXTEntity = new FhlTXTEntity(
                    reader["Descr"].ToString());

                fhlEntity.colnewDBTXT.Add(fhlTXTEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fhlEntity;
        }
        public FhlEntity AddHTSValFromReader(FhlEntity fhlEntity, IDataReader reader)
        {

            while (reader.Read())
            {

                FhlHTSEntity fhlHTSEntity = new FhlHTSEntity(
                    reader["Descr"].ToString());

                fhlEntity.colHTS.Add(fhlHTSEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fhlEntity;
        }
        public FhlEntity AddnewHTSFromReader(FhlEntity fhlEntity, IDataReader reader)
        {

            while (reader.Read())
            {

                FhlHTSEntity fhlHTSEntity = new FhlHTSEntity(
                    reader["Descr"].ToString());

                fhlEntity.colnewDBHTS.Add(fhlHTSEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fhlEntity;
        }
        public FhlEntity AddOCIValFromReader(FhlEntity fhlEntity, IDataReader reader)
        {

            while (reader.Read())
            {

                FhlOCIEntity fhlOCIEntity = new FhlOCIEntity(
                    reader["CountryCode"].ToString(),
                    reader["Infold"].ToString(),
                    reader["CustomsId"].ToString(),
                    reader["CustomsInfo"].ToString()
                    );

                fhlEntity.colOCI.Add(fhlOCIEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fhlEntity;
        }
        public FhlEntity AddnewOCIFromReader(FhlEntity fhlEntity, IDataReader reader)
        {

            while (reader.Read())
            {

                FhlOCIEntity fhlOCIEntity = new FhlOCIEntity(
                    reader["CountryCode"].ToString(),
                    reader["Infold"].ToString(),
                    reader["CustomsId"].ToString(),
                    reader["CustomsInfo"].ToString()
                    );

                fhlEntity.colnewDBOCI.Add(fhlOCIEntity);
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fhlEntity;
        }
        public FhlEntity AddVolumeFromReader(FhlEntity fwbEntity, IDataReader reader)
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
        public FhlEntity AddOtherChargeFromReader(FhlEntity fwbEntity, IDataReader reader)
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

    }
}
