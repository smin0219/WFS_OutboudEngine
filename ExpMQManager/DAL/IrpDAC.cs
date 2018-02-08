using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class IrpDAC : BaseDAC
    {
        public IrpEntity GetIRPInfoDAC(int mid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);

            string strSql = "";
            strSql = @" SELECT idnum, IRPFrom, IRPSubject, IRPText, FlightNo, ArrivalDate
                        FROM AL_IRP
                        WHERE idnum = {0}
                    ";
            strSql = string.Format(strSql, mid);

            return GetIRPFromReader(baseAWB, ExecuteReader(strSql));
        }

        protected IrpEntity GetIRPFromReader(BaseEntity baseEntity, IDataReader reader)
        {
            if (!reader.IsClosed)
            {
                reader.Read();

                int irpId = 0; 
                try { pcs = Convert.ToInt32(reader["idnum"]); }
                catch
                {
                }

                try
                {

                    IrpEntity irpEntity = new IrpEntity(
                        baseEntity,
                        irpId,
                        reader["IRPFrom"].ToString().Trim(),
                        reader["IRPSubject"].ToString().Trim(),
                        reader["IRPText"].ToString().Trim(),
                        reader["FlightNo"].ToString().Trim(),
                        Convert.ToDateTime(reader["ArrivalDate"]));

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return irpEntity;
                }
                catch
                {
                    IrpEntity irpEntity = new IrpEntity();
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return irpEntity;
                }
            }
            else
            {
                IrpEntity irpEntity = new IrpEntity();
                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return irpEntity;
            }
        }
    }
}
