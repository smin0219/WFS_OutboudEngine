using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class TfdDAC : BaseDAC
    {
        int mid_temp = 0;
        public TfdEntity GetTfdInfoDAC(int mid, int refID, int flightSeq, string msgType, string subType, int queueId)
        {
            mid_temp = mid;
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, refID, flightSeq, msgType, subType, queueId);

            string strSql = "";
//            strSql = @" SELECT WHCFTime, idnum, A.MID, cnee,
//	                           (SELECT Pcs FROM ePic_DLV_Log WHERE Seq =
//	                            (SELECT MAX(Seq) FROM dbo.ePic_DLV_Log WHERE MID = {0})) pcs,
//	                           A.weight 	
//                        FROM CC_CashTB A
//                        JOIN ePic_Master2 B ON A.MID = B.MID
//                        JOIN CC_TransInfo C ON A.TransID = C.TransID
//                        WHERE A.MID = {0}
//                    ";

//            strSql = @" 
//                        select  
//		                        Substring(flightNo,1,2)												as Carrier
//	                        ,	Substring(Convert(varchar,WHCFTime,121),9,2)						as tday
//	                        ,	Substring(Convert(varchar,WHCFTime,121),6,2)						as tmonth  
//	                        ,	Replace(Substring(Convert(varchar,WHCFTime,121),12,5),':','')		as ttime  
//	                        ,	FltDest																as OrgPort
//	                        ,	PCs																	as PCs
//	                        ,	Weight																as Weight
//                            ,   WHCFTime
//                        from TX_CashTB where Mid = {0}
//                        AND TransId = ( 
//                        SELECT TOP 1 TransID FROM ePic_DLV_Log 
//                        WHERE Seq = ( SELECT dlvlogseq FROM Edi_msg_Queue WHERE iid = {1} )
//                        )
//                    ";

            // updated. 7/24/2015
            strSql = @" 


                        select  
		                        Substring(flightNo,1,2)												as Carrier
	                        ,	Substring(Convert(varchar,WHCFTime,121),9,2)						as tday
	                        ,	Substring(Convert(varchar,WHCFTime,121),6,2)						as tmonth  
	                        ,	Replace(Substring(Convert(varchar,WHCFTime,121),12,5),':','')		as ttime  
	                        ,	FltDest																as OrgPort
	                        ,	a.PCs																	as PCs
	                        ,	a.Weight																as Weight
                            ,   WHCFTime
                        from TX_CashTB as a
						join ePic_DLV_Log as b
						on a.idnum = b.cashtbid
						 WHERE Seq = ( SELECT dlvlogseq FROM Edi_msg_Queue WHERE iid = {0} )

";
            strSql = string.Format(strSql, queueId);
            //strSql = string.Format(strSql, mid, msgType);

            return GetTfdFromReader(baseAWB, ExecuteReader(strSql));
        }

        protected TfdEntity GetTfdFromReader(BaseEntity baseEntity, IDataReader reader)
        {
            if (!reader.IsClosed)
            {
                reader.Read();

                //int pcs = 0; try { pcs = (int)reader["pcs"]; } catch { }
                int pcs = 0;
                try
                {
                    pcs = Convert.ToInt32(reader["PCs"]);

                    if (pcs != GetMasterPcs(mid_temp))
                        baseEntity.shipmentIndicator = "P";
                    else
                        baseEntity.shipmentIndicator = "T";


                }
                catch 
                {
                }
                double weight = 0.00;
                try
                {
                    weight = Convert.ToDouble(reader["Weight"].ToString());
                }
                catch (Exception e)
                {
                }
                try
                {
                    TfdEntity TfdEntity = new TfdEntity(
                        baseEntity,
                        reader["Carrier"].ToString(),
                        reader["tday"].ToString(),
                        reader["tmonth"].ToString(),
                        reader["ttime"].ToString(),
                        reader["OrgPort"].ToString(),
                        pcs,
                        weight,
                        Convert.ToDateTime(reader["WHCFTime"]));    // added for local time by lcode

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return TfdEntity;
                }
                catch (Exception e)
                {
                    TfdEntity tfdEntityEx = new TfdEntity();
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return tfdEntityEx;
                }
            }
            else
            {
                TfdEntity tfdEntity = new TfdEntity();
                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return tfdEntity;
            }
        }

        protected int GetMasterPcs(int mid)
        {
            int pcs = 0;

            string strSql = "";
            strSql = @" SELECT PCS FROM ePic_Master2 
                        WHERE MID = {0}
                    ";
            strSql = string.Format(strSql, mid);

            IDataReader reader = ExecuteReader(strSql);

            if (!reader.IsClosed)
            {
                reader.Read();
                try
                {
                    pcs = Convert.ToInt32(reader["pcs"]);
                }
                catch
                {
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return 0;
                }
            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return pcs;
           
        }

    }
}
