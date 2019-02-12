using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class UwsDAC : BaseDAC
    {
        public List<UwsEntity> GetUWSColDAC(int flightSeq)
        {
            string strSql = "";
            strSql = @"
                SELECT 
                ULD, DestCd as POU, LoadCategory, SHC, SHC2, SHC3, [Weight], WeightIndicator, isFinal
                FROM Exp_UWS
                WHERE FlightSeq = {0}
                ORDER BY
                    CASE WHEN LEFT(ULD , 4) = 'BULK' THEN '1' ELSE '0' END,
                    ULD
                DESC
            ";
            strSql = string.Format(strSql, flightSeq);

            return GetUWSfromReader(ExecuteReader(strSql));
        }
        protected List<UwsEntity> GetUWSfromReader(IDataReader reader)
        {
            List<UwsEntity> uwsEntityCol = new List<UwsEntity>();
            while (reader.Read())
            {
                decimal weight = 0; try { weight = Convert.ToInt32(reader["Weight"]); }
                catch { }
                int isFinal = 0; try { isFinal = Convert.ToInt32(reader["isFinal"] ?? 0); } catch { isFinal = 0; }

                UwsEntity uwsEntity = new UwsEntity(
                    reader["ULD"].ToString(),
                    reader["POU"].ToString(),
                    reader["LoadCategory"].ToString(),
                    reader["SHC"].ToString(),
                    reader["SHC2"].ToString(),
                    reader["SHC3"].ToString(),
                    weight,
                    reader["WeightIndicator"].ToString(),
                    isFinal
                    );
                uwsEntityCol.Add(uwsEntity);
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return uwsEntityCol;
        }
    }
}
