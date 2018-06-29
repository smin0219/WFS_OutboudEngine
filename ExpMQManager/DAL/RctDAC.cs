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
                //strSql = @" SELECT MID, cnee, Pcs, Weight,
                //      (SELECT MAX(CreatedDate) FROM Exp_MasterAccept WHERE mid = A.mid) as rcsTime,
                //      (SELECT Carrier from Customer_Carrier as C WHERE C.Ccode = A.Ccode and c.IsMainCarrier = 'Y') as carrier
                //        FROM Exp_Master A
                //        WHERE A.MID = {0} 
                //    ";
                //strSql = string.Format(strSql, mid);


                strSql = @"
                            SELECT MID, cnee, Pcs, Weight,
		                   (SELECT MAX(CreatedDate) FROM Exp_MasterAccept WHERE mid = A.mid) as rcsTime,
		                   (SELECT TOP 1 Carrier from Customer_Carrier WHERE Ccode = (SELECT Ccode FROM EDI_Msg_Queue WHERE iid={1}) and IsMainCarrier = 'Y') as carrier
                            FROM Exp_Master A
                            WHERE A.MID = {0}
                        ";
                strSql = string.Format(strSql, mid, queueId);
            }
            else
            {
                //strSql = @" 
                //        SELECT MID, cnee, Pcs, Weight, (SELECT CreatedDate FROM EDI_MSG_Queue WHERE iid = {1}) as rcsTime
                //        FROM Exp_Master A
                //        WHERE A.MID = {0} 
                //    ";
                //strSql = string.Format(strSql, mid, queueId);

                strSql = @" 
                        SELECT MID, cnee, Pcs, Weight, (SELECT CreatedDate FROM EDI_MSG_Queue WHERE iid = {1}) as rcsTime,
                        (SELECT TOP 1 Carrier from Customer_Carrier WHERE Ccode = (SELECT Ccode FROM EDI_Msg_Queue WHERE iid={1}) and IsMainCarrier = 'Y') as carrier
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
                        reader["cnee"].ToString().Trim(),
                        reader["carrier"].ToString());


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
