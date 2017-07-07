using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class FohDAC : BaseDAC
    {
        public FOHEntity GetFOHInfoDAC(int mid, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, flightSeq, msgType, subType, queueId);

            string strSql = "";
            strSql = @" SELECT MID, shipper, Pcs, Weight, (select top 1 CreatedDate FROM Edi_Msg_Queue where iid = {1}) as fohTime
                        FROM Exp_Master A
                        WHERE A.MID = {0} 
                    ";
            strSql = string.Format(strSql, mid, queueId);

            return GetFOHfromReader(baseAWB, ExecuteReader(strSql));
        }

        public FOHEntity GetFOHfromReader(BaseEntity baseEntity, IDataReader reader)
        {
            FOHEntity fohEntity = new FOHEntity();
            if(reader.Read())
            {
                try
                {
                    fohEntity = new FOHEntity(
                        baseEntity,
                        Convert.ToDateTime(reader["fohTime"]),
                        reader["shipper"].ToString().Trim());


                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return fohEntity;
                }
                catch
                {
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    fohEntity = new FOHEntity();
                    return fohEntity;
                }
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return fohEntity;
        }
    }
}
