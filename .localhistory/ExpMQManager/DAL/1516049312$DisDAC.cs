using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class DisDAC : BaseDAC
    {
        public DisEntity GetDISInfoDAC(int mid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);
            string strSql = "";

            strSql = @"
                        DECLARE @queueID INT = {0}
						DECLARE @FlightSeq INT = {1}
						DECLARE @tZone INT = (SELECT TOP 1 ISNULL(tzone, 0) FROM [Location] WHERE Lcode = (SELECT Lcode FROM EDI_MSG_Queue WHERE iid = @queueID))

						SELECT EDIMQ.FlightNo as FlightNo
							, RCFBCL.manPcs as ManPcs
                            , RCFBCL.LocPcs as LocPcs
							, RCFBCL.[Weight] as [Weight]
							, (CASE WHEN RCFBCL.ManPcs =  RCFBCL.LocPcs THEN 'T' ELSE 'P' END) as Partial
							, DATEADD(HOUR, @tZone, GETDATE()) as arrDate
							, ePicFM.FlightDate arrFlightMasterDate
							, RCFBCL.DIS_Type as disType
							, RCFBCL.OSI as disOsi
						FROM (SELECT iid, Carrier, Lcode, Ccode, MID, RefID, FlightSeq, FlightNo FROM EDI_MSG_Queue WHERE iid = @queueID ) as EDIMQ
						JOIN RCF_BCLItem as RCFBCL
						ON EDIMQ.RefID = RCFBCL.idnum
						LEFT JOIN (SELECT TOP 1 FlightSeq, MIN(FlightDate) as FlightDate FROM ePic_FlightMaster Where FlightSeq = @FlightSeq GROUP BY FlightSeq) as ePicFM
						ON EDIMQ.FlightSeq = ePicFM.FlightSeq
                    ";

            strSql = string.Format(strSql, queueId, flightSeq, mid);
            return GetDISfromReader(baseAWB, ExecuteReader(strSql));

        }
        public DisEntity GetDISfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            if (!reader.IsClosed)
            {
                reader.Read();


                int manPcs = 0;
                try
                {
                    pcs = Convert.ToInt32(reader["ManPcs"]);
                }
                catch
                {
                    DisEntity disEntityEx = new DisEntity();
                    return disEntityEx;
                }
                int loc Pcs = 0;
                try
                {
                    pcs = Convert.ToInt32(reader["LocPcs"]);
                }
                catch
                {
                    DisEntity disEntityEx = new DisEntity();
                    return disEntityEx;
                }
                double weight = 0.00;

                try
                {
                    weight = Convert.ToDouble(reader["weight"].ToString());
                }
                catch (Exception e)
                {
                    DisEntity disEntityEx = new DisEntity();
                    return disEntityEx;
                }

                //int pcs = 0; try { pcs = (int)reader["Pcs"]; } catch { }
                //double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                //catch (Exception e) { }

                try
                {
                    DisEntity disEntity = new DisEntity(
                        baseEntity,
                        reader["flightNo"].ToString().Trim(),
                        pcs,
                        weight,
                        reader["Partial"].ToString().Trim(),
                        Convert.ToDateTime(reader["arrDate"]),
                        Convert.ToDateTime(reader["arrFlightMasterDate"]),
                        reader["disType"].ToString().Trim(),
                        reader["disOsi"].ToString().Trim()
                        );

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return disEntity;
                }
                catch (Exception ex)
                {
                    DisEntity disEntityEx = new DisEntity();

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return disEntityEx;
                }

            }
            else
            {
                DisEntity disEntityEx = new DisEntity();

                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return disEntityEx;
            }
        }

    }
}
