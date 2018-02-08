using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class AwdDAC : BaseDAC
    {
        public AwdEntity GetAWDInfoDAC(int mid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);

            string strSql = "";
            // before 2014-12-15
            // imp_FlightStatus가 2줄이상일 경우 arrDate를 잘 못 가져온다.
            //strSql = @" SELECT MIN(FlightNo) FlightNo, SUM(OnPcs) Pcs, SUM(OnWeight) Weight, MIN(Partial) Partial,
            //	                (SELECT TOP 1 DocAvailDate FROM imp_FlightStats WHERE FlightSeq = {1}) DocAvailDate, MIN(cnee) cnee
            //            FROM ePic_FlightMaster A
            //            JOIN ePic_Master2 B ON A.MID = B.MID
            //            WHERE A.MID = {0} AND FlightSeq = {1}
            //            GROUP BY A.MID
            //        ";

            strSql = @" 
                        SELECT 
	                        MIN(A.FlightNo) FlightNo
	                        , SUM(OnPcs) Pcs
	                        , SUM(OnWeight) Weight
	                        , MIN(Partial) Partial
	                        ,( SELECT TOP 1 DocAvailDate FROM imp_FlightStats WHERE FlightSeq = {1} and Ccode = (select ccode from EDI_Msg_Queue where iid = {2}) ) DocAvailDate
	                        , MIN(cnee) cnee
                        FROM ePic_FlightMaster A

                        JOIN ePic_Master2 B ON A.MID = B.MID

                        WHERE A.MID = {0} AND A.FlightSeq = {1}

                        GROUP BY A.MID
                    ";
            strSql = string.Format(strSql, mid, flightSeq, queueId);

            return GetAWDfromReader(baseAWB, ExecuteReader(strSql));
        }

        public AwdEntity GetAWDfromReader(BaseEntity baseEntity, IDataReader reader)
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
                    AwdEntity awdEntityEx = new AwdEntity();
                    return awdEntityEx;
                }
                double weight = 0.00;

                try
                {
                    weight = Convert.ToDouble(reader["weight"].ToString());
                }
                catch (Exception e)
                {
                    AwdEntity awdEntityEx = new AwdEntity();
                    return awdEntityEx;
                }
                try
                {
                    AwdEntity awdEntity = new AwdEntity(
                        baseEntity,
                        pcs,
                        weight,
                        reader["Partial"].ToString().Trim(),
                        Convert.ToDateTime(reader["DocAvailDate"]),
                        reader["cnee"].ToString().Trim());


                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return awdEntity;
                }
                catch
                {
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();

                    AwdEntity awdEntityEx = new AwdEntity();
                    return awdEntityEx;
                }
            }
            else
            {
                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();

                AwdEntity awdEntity = new AwdEntity();
                return awdEntity;
            }
        }
    }
}
