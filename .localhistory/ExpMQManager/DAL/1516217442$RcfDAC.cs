using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class RcfDAC : BaseDAC
    {
        public RcfEntity GetRCFInfoDAC(int mid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);

            string strSql = "";
            string arrColumnName = "";

            // before 2014-12-15
            // imp_FlightStatus가 2줄이상일 경우 arrDate를 잘 못 가져온다.
            //strSql = @" SELECT MIN(FlightNo) FlightNo, SUM(OnPcs) Pcs, SUM(OnWeight) Weight, MIN(Partial) Partial,
            //        (SELECT TOP 1 {2} FROM imp_FlightStats WHERE FlightSeq = {1}) arrDate
            //        FROM ePic_FlightMaster as a
            //        WHERE a.MID = {0} AND FlightSeq = {1}
            //        GROUP BY a.MID ";

            // changed. Realtime RCF. 2018-1-11
            //strSql = @" 
            //             SELECT
            //                MIN(a.FlightNo) FlightNo
            //                , SUM(OnPcs) Pcs
            //                , SUM(OnWeight) Weight
            //                , MIN(Partial) Partial
            //                , (SELECT TOP 1 {2} FROM imp_FlightStats WHERE FlightSeq = {1} and Ccode = (select ccode from EDI_Msg_Queue where iid = {3}) ) arrDate
            //                , MIN(a.FlightDate) arrFlightMasterDate
            //            FROM ePic_FlightMaster as a

            //            WHERE a.MID = {0} AND a.FlightSeq = {1}

            //            GROUP BY a.MID
            //            ";

            //if (subType.ToUpper() == "RCF")
            //    arrColumnName = "ArrCargoDate";

            //if (subType.ToUpper() == "ARR")
            //    arrColumnName = "ArrFltDate";

            //strSql = string.Format(strSql, mid, flightSeq, arrColumnName, queueId);

            if (subType.ToUpper() == "RCF" || subType.ToUpper() == "ARR")
            {
                strSql = @" 
                         SELECT
                            MIN(a.FlightNo) FlightNo
                            , SUM(OnPcs) Pcs
                            , SUM(OnWeight) Weight
                            , MIN(Partial) Partial
                            , (SELECT TOP 1 {2} FROM imp_FlightStats WHERE FlightSeq = {1} and Ccode = (select ccode from EDI_Msg_Queue where iid = {3}) ) arrDate
                            , MIN(a.FlightDate) arrFlightMasterDate
                        FROM ePic_FlightMaster as a

                        WHERE a.MID = {0} AND a.FlightSeq = {1}

                        GROUP BY a.MID";

                if (subType.ToUpper() == "RCF")
                    arrColumnName = "ArrCargoDate";

                if (subType.ToUpper() == "ARR")
                    arrColumnName = "ArrFltDate";

                strSql = string.Format(strSql, mid, flightSeq, arrColumnName, queueId);
            }
            else
            {
                // subtype = "RTF"
                if (refID != 0)
                {
                    strSql = @"
                        DECLARE @queueID INT = {0}
                        DECLARE @FlightSeq INT = {1}
                        DECLARE @MID INT = {2}
                        DECLARE @RefID INT = {3}
                        DECLARE @tZone INT = (SELECT TOP 1 ISNULL(tzone, 0) FROM [Location] WHERE Lcode = (SELECT Lcode FROM EDI_MSG_Queue WHERE iid = @queueID))

						SELECT EDIMQ.FlightNo as FlightNo
                            ,RCFBCL.RCF_PCS as Pcs
                            , RCFBCL.[Weight] as [Weight]
                            , ePicFM.[Partial] as Partial
                            , DATEADD(HOUR, @tZone, GETDATE()) as arrDate
                            , ePicFM.FlightDate as arrFlightMasterDate
                        FROM (SELECT iid, Carrier, Lcode, Ccode, MID, RefID, FlightSeq, FlightNo FROM EDI_MSG_Queue WHERE iid = @queueID ) as EDIMQ
                        JOIN (SELECT idnum, FlightSeq, MID, [Weight], RCF_PCS FROM RCF_BCLItem WHERE idnum = @RefID) as RCFBCL
                        ON EDIMQ.RefID = RCFBCL.idnum
                        JOIN (SELECT TOP 1 MID, FlightSeq, MIN(FlightDate) as FlightDate, MIN([Partial]) as [Partial] FROM ePic_FlightMaster Where MID = @MID AND FlightSeq = @FlightSeq GROUP BY MID, FlightSeq) as ePicFM
                        ON EDIMQ.MID = ePicFM.MID AND EDIMQ.FlightSeq = ePicFM.FlightSeq";

                    strSql = string.Format(strSql, queueId, flightSeq, mid, refID);
                }
                else
                {
                    strSql = @"
                        DECLARE @queueID INT = {0}
                        DECLARE @FlightSeq INT = {1}
                        DECLARE @MID INT = {2}
                        DECLARE @tZone INT = (SELECT TOP 1 ISNULL(tzone, 0) FROM [Location] WHERE Lcode = (SELECT Lcode FROM EDI_MSG_Queue WHERE iid = @queueID))

						SELECT EDIMQ.FlightNo as FlightNo
                            ,RCFBCL.RCF_PCS as Pcs
                            , RCFBCL.[Weight] as [Weight]
                            , ePicFM.[Partial] as Partial
                            , DATEADD(HOUR, @tZone, GETDATE()) as arrDate
                            , ePicFM.FlightDate as arrFlightMasterDate
                        FROM (SELECT iid, Carrier, Lcode, Ccode, MID, FlightSeq, FlightNo FROM EDI_MSG_Queue WHERE iid = @queueID ) as EDIMQ
                        JOIN (SELECT TOP 1 FlightSeq, MID, [Weight], RCF_PCS FROM RCF_BCLItem WHERE FlightSeq = @FlightSeq AND MID = @MID) as RCFBCL
                        ON EDIMQ.FlightSeq = RCFBCL.FlightSeq AND EDIMQ.MID = RCFBCL.MID
                        JOIN (SELECT TOP 1 MID, FlightSeq, MIN(FlightDate) as FlightDate, MIN([Partial]) as [Partial] FROM ePic_FlightMaster Where MID = @MID AND FlightSeq = @FlightSeq GROUP BY MID, FlightSeq) as ePicFM
                        ON EDIMQ.MID = ePicFM.MID AND EDIMQ.FlightSeq = ePicFM.FlightSeq";
                }
                

                strSql = string.Format(strSql, queueId, flightSeq, mid);
            }

            return GetRCFfromReader(baseAWB, ExecuteReader(strSql));
        }

        public RcfEntity GetRCFfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            if (!reader.IsClosed)
            {
                reader.Read();


                int pcs = 0;
                try
                {
                    pcs = Convert.ToInt32(reader["pcs"]);
                }
                catch
                {
                    RcfEntity rcfEntityEx = new RcfEntity();
                    return rcfEntityEx;
                }
                double weight = 0.00;

                try
                {
                    weight = Convert.ToDouble(reader["weight"].ToString());
                }
                catch (Exception e)
                {
                    RcfEntity rcfEntityEx = new RcfEntity();
                    return rcfEntityEx;
                }

                //int pcs = 0; try { pcs = (int)reader["Pcs"]; } catch { }
                //double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                //catch (Exception e) { }

                try
                {
                    RcfEntity rcfEntity = new RcfEntity(
                        baseEntity,
                        reader["flightNo"].ToString().Trim(),
                        pcs,
                        weight,
                        reader["Partial"].ToString().Trim(),
                        Convert.ToDateTime(reader["arrDate"]),
                        Convert.ToDateTime(reader["arrFlightMasterDate"])
                        );

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return rcfEntity;
                }
                catch
                {
                    RcfEntity rcfEntityEx = new RcfEntity();

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return rcfEntityEx;
                }
                
            }
            else
            {
                RcfEntity rcfEntity = new RcfEntity();

                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return rcfEntity;
            }
        }
    }
}
