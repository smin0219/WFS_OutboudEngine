using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class ManDAC : BaseDAC
    {
        public ManEntity GetMANInfoDAC(int mid, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, flightSeq, msgType, subType, queueId);

            string strSql = "";
//            strSql = @" SELECT MIN(A.FlightNo) FlightNo, SUM(OnPcs) Pcs, SUM(OnWeight) Weight, MIN(Partial) Partial,
//                               MIN(UtcArrDate) ArrDate, MIN(UtcArrDateType) ArrDateType, MIN(UtcDepDate) DepDate,
//                               MIN(UtcDepDateType) DepDateType, MIN(DayChange) DayChange   
//                        FROM Exp_FlightMaster A
//                        JOIN (SELECT * FROM Exp_FlightSeqMsg WHERE QueueId = {2}) 
//	                        B ON A.FlightSeq = B.FlightId
//                        WHERE A.MID = {0} AND FlightSeq = {1}
//                        GROUP BY A.MID 
//                    ";

            // 2015-09-23 UTC time에서 Local time으로 변경
            strSql = @"     SELECT MIN(A.FlightNo) FlightNo, SUM(OnPcs) Pcs, SUM(OnWeight) Weight, MIN(Partial) Partial,
                                   MIN(LocalArrDate) ArrDate, MIN(LocalArrDateType) ArrDateType, 
	                               MIN(LocalDepDate) DepDate, MIN(LocalDepDateType) DepDateType, 
	                               MIN(DayChange) DayChange   
                            FROM Exp_FlightMaster A
                            JOIN (SELECT * FROM Exp_FlightSeqMsg WHERE QueueId = {2}) 
                                B ON A.FlightSeq = B.FlightId
                            WHERE A.MID = {0} AND FlightSeq = {1}
                            GROUP BY A.MID 
                        ";

            //(SELECT * FROM Exp_FlightSeqMsg WHERE SeqNo = (SELECT MAX(SeqNo) FROM Exp_FlightSeqMsg WHERE FlightId = {1}))
            strSql = string.Format(strSql, mid, flightSeq, queueId);

            return GetMANfromReader(baseAWB, ExecuteReader(strSql));
        }

        public ManEntity GetMANfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            ManEntity manEntity = new ManEntity();
            if (reader.Read())
            {
                int pcs = 0;
                try
                {
                    pcs = Convert.ToInt32(reader["pcs"]);
                }
                catch
                {
                    ManEntity manEntityEx = new ManEntity();
                    return manEntityEx;
                }
                double weight = 0.00;

                try
                {
                    weight = Convert.ToDouble(reader["weight"].ToString());
                }
                catch (Exception e)
                {
                    ManEntity manEntityEx = new ManEntity();
                    return manEntityEx;
                }
                try
                {
                    manEntity = new ManEntity(
                        baseEntity,
                        pcs,
                        weight,
                        reader["Partial"].ToString().Trim(),
                        Convert.ToDateTime(reader["DepDate"]),
                        reader["DepDateType"].ToString().Trim(),
                        Convert.ToDateTime(reader["ArrDate"]),
                        reader["ArrDateType"].ToString().Trim(),
                        reader["DayChange"].ToString().Trim(),
                        reader["FlightNo"].ToString().Trim());

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();

                    return manEntity;
                }
                catch (Exception e)
                {
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();

                    ManEntity manEntityEx = new ManEntity();
                    return manEntityEx;
                }
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();

            return manEntity;
        }
    }
}
