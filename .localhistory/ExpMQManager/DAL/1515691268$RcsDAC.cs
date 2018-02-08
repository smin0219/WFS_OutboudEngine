using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class RcsDAC : BaseDAC
    {
        public RcsEntity GetRCSInfoDAC(int mid, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, flightSeq, msgType, subType, queueId);

            string strSql = "";
            if (subType == "RCS")
            {

                strSql = @" SELECT MID, cnee, shipper, Pcs, Weight,
                               (SELECT CreatedDate FROM Exp_MasterAccept WHERE iid =
                                (SELECT MAX(iid) FROM Exp_MasterAccept WHERE MID = A.MID)) rcsTime
                        FROM Exp_Master A
                        WHERE A.MID = {0} 
                    ";
                strSql = string.Format(strSql, mid);
            }
            else
            {
                strSql = @" SELECT MID, cnee, shipper, Pcs, Weight, (SELECT CreatedDate FROM EDI_MSG_Queue WHERE iid = {1}) as rcsTime
                        FROM Exp_Master A
                        WHERE A.MID = {0} 
                    ";
                strSql = string.Format(strSql, mid, queueId);
            }

            return GetRCSfromReader(baseAWB, ExecuteReader(strSql));
        }

        public RcsEntity GetRCSfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            RcsEntity rcsEntity = new RcsEntity();
            if (reader.Read())
            {
                try
                {
                    rcsEntity = new RcsEntity(
                        baseEntity,
                        Convert.ToDateTime(reader["rcsTime"]),
                        reader["cnee"].ToString().Trim(),
                        reader["shipper"].ToString().Trim());


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
                    rcsEntity = new RcsEntity();
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
