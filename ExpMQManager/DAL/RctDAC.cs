using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class RctDAC : BaseDAC
    {
        public RctEntity GetRCTInfoDAC(int mid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);

            string strSql = "";

            if (subType == "RCT")
            {
                strSql = @" SELECT MID, cnee, Pcs, Weight,
                               (SELECT CreatedDate FROM Exp_MasterAccept WHERE iid =
                                (SELECT MAX(iid) FROM Exp_MasterAccept WHERE MID = A.MID)) rcsTime
                        FROM Exp_Master A
                        WHERE A.MID = {0} 
                    ";
                strSql = string.Format(strSql, mid);
            }
            else
            {
                strSql = @" SELECT MID, cnee, Pcs, Weight, (SELECT CreatedDate FROM EDI_MSG_Queue WHERE iid = {1}) as rcsTime
                        FROM Exp_Master A
                        WHERE A.MID = {0} 
                    ";
                strSql = string.Format(strSql, mid, queueId);

            }

            return GetRCTfromReader(baseAWB, ExecuteReader(strSql));
        }

        public RctEntity GetRCTfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            RctEntity rcsEntity = new RctEntity();
            if (reader.Read())
            {
                try
                {
                    rcsEntity = new RctEntity(
                        baseEntity,
                        Convert.ToDateTime(reader["rcsTime"]),
                        reader["cnee"].ToString().Trim());


                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return rcsEntity;
                }
                catch
                {
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    rcsEntity = new RctEntity();
                    return rcsEntity;
                }
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return rcsEntity;
        }
    }
}
