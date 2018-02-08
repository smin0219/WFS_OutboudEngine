using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class DlvDAC : BaseDAC
    {
        int mid_temp = 0;
        public DlvEntity GetDLVInfoDAC(int mid, int flightSeq, string msgType, string subType, int queueId)
        {
            mid_temp = mid;
            BaseEntity baseAWB = GetBaseAWBInfoDAC(mid, flightSeq, msgType, subType, queueId);

            string strSql = "";
            //            strsql = @" select a.whcftime, a.idnum, a.mid, b.cnee, 
            //                            --c.pcs,
            //                            (select sum(pcs) from epic_dlv_log where mid = {0} and resultcd <> 4 ) as pcs,
            //	                        ( case when b.weight = 0 or b.pcs = 0 then 
            //			                        convert(decimal(10,2),(case when b.pcs = 0 then 0 else (sum(isnull(e.onweight,0)) / sum(isnull(e.onpcs,0)) ) * 
            //                                    (select sum(pcs) from epic_dlv_log where mid = {0} and resultcd <> 4 ) end )) 
            //		                        else
            //			                        convert(decimal(10,2),(case when b.pcs = 0 then 0 else (b.weight / b.pcs ) * 
            //                                    (select sum(pcs) from epic_dlv_log where mid = {0} and resultcd <> 4 ) end )) 
            //		                        end ) as weight
            //                        from cc_cashtb a
            //                        join epic_master2 b on a.mid = b.mid
            //                        join epic_dlv_log c on c.cashtbid = a.idnum and isnull(c.hid,'') = isnull(a.hid,'')
            //                        join edi_msg_queue d on d.dlvlogseq = c.seq
            //                        left join epic_flightmaster e on a.mid = e.mid
            //                        where a.mid = {0} and iid = {1}
            //                        group by a.whcftime, a.idnum, a.mid, b.cnee, c.pcs, b.weight, b.pcs
            //                    ";

            //2014.07.21 Changed to Calculate Pcs by Mpart (joh)
            // 2015.01.08 Changed to (4)Where - "ResultCd <> 3" by Jacob Lee

            strSql = @"SELECT 
	                    A.WHCFTime
	                    , A.idnum
	                    , A.MID
	                    , B.cnee, 
	                    (
                            select pcs from ePic_DLV_Log where Seq =  D.DlvLogSeq and ResultCd <> 4 and ResultCd <> 3 
		                    --case when (select MPart from ePic_DLV_Log where Seq = D.DlvLogSeq) != '' 
	                        --then (select SUM(pcs) from ePic_DLV_Log where Seq =  D.DlvLogSeq and ResultCd <> 4 and ResultCd <> 3 )
	                        --else (select SUM(pcs) from ePic_DLV_Log where MID = {0} and ResultCd <> 4 and ResultCd <> 3)
	                        --end 
	                    ) as pcs,
                       Convert(decimal(10,1),round(( 
                           CASE WHEN B.Weight = 0 or B.Pcs = 0 
                           THEN 
			                    Convert(decimal(10,2),(Case when B.pcs = 0 then 0 else (sum(isnull(E.OnWeight,0)) / sum(isnull(E.OnPcs,0)) ) * 
			                    (case when (select MPart from ePic_DLV_Log where Seq = D.DlvLogSeq) != '' 
			                    then (select SUM(pcs) from ePic_DLV_Log where Seq =  D.DlvLogSeq and ResultCd <> 4 and ResultCd <> 3)
			                    else (select SUM(pcs) from ePic_DLV_Log where MID = {1} and Seq =  D.DlvLogSeq and ResultCd <> 4 and ResultCd <> 3)
			                    end  ) end )) 
		                    ELSE
			                    Convert(decimal(10,2),(Case when B.pcs = 0 then 0 else (B.Weight / B.Pcs ) * 
			                    (case when (select MPart from ePic_DLV_Log where Seq = D.DlvLogSeq) != '' 
			                    then (select SUM(pcs) from ePic_DLV_Log where Seq =  D.DlvLogSeq and ResultCd <> 4 and ResultCd <> 3)
			                    else (select SUM(pcs) from ePic_DLV_Log where MID = {2} and Seq =  D.DlvLogSeq and ResultCd <> 4 and ResultCd <> 3)
			                    end  ) end )) 
	                       END 
	                     ),1)) as weight
                    FROM CC_CashTB A
                    JOIN 
	                    ePic_Master2 B ON A.MID = B.MID
                    JOIN 
	                    ePic_DLV_Log C ON C.CashTBId = A.idnum and isnull(C.HID,'') = isnull(A.HID,'')
                    JOIN 
	                    EDI_MSG_QUEUE D ON D.DlvLogSeq = C.Seq
                    LEFT JOIN 
	                    ePic_Flightmaster E ON A.MID = E.MID
                    WHERE A.MID = {3} 
	                    and iid = {4}
                    GROUP BY 
	                    A.WHCFTime
	                    , A.idnum
	                    , A.MID
	                    , B.cnee
	                    , C.pcs
	                    , B.Weight
	                    , B.Pcs
	                    ,D.DlvLogSeq";
            strSql = string.Format(strSql, mid, mid, mid, mid, queueId.ToString());
            //strSql = string.Format(strSql, mid, queueId.ToString());
            //strSql = string.Format(strSql, mid, msgType);

            return GetDLVFromReader(baseAWB, ExecuteReader(strSql));
        }

        protected DlvEntity GetDLVFromReader(BaseEntity baseEntity, IDataReader reader)
        {
            if (!reader.IsClosed)
            {
                reader.Read();

                //int pcs = 0; try { pcs = (int)reader["pcs"]; } catch { }
                int pcs = 0;
                try
                {
                    pcs = Convert.ToInt32(reader["pcs"]);

                    // if dlv PC == 0 then not build msg. 
                    if (pcs == 0)
                    {
                        DlvEntity dlvEntityEx = new DlvEntity();
                        reader.Close();
                        reader.Dispose();
                        disConnect_dbcn_ExcuteReader();
                        return dlvEntityEx;
                    }


                    if (pcs != GetMasterPcs(mid_temp))
                        baseEntity.shipmentIndicator = "P";
                    else
                        baseEntity.shipmentIndicator = "T";
                }
                catch
                {
                    DlvEntity dlvEntityEx = new DlvEntity();
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return dlvEntityEx;
                }

                double weight = 0.00;

                try
                {
                    weight = Convert.ToDouble(reader["weight"].ToString());
                }
                catch (Exception e)
                {
                }
                try
                {
                    DlvEntity dlvEntity = new DlvEntity(
                        baseEntity,
                        Convert.ToDateTime(reader["WHCFTime"]),
                        pcs,
                        weight,
                        reader["cnee"].ToString().Trim());

                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return dlvEntity;
                }
                catch (Exception e)
                {
                    DlvEntity dlvEntityEx = new DlvEntity();
                    reader.Close();
                    reader.Dispose();
                    disConnect_dbcn_ExcuteReader();
                    return dlvEntityEx;
                }
            }
            else
            {
                DlvEntity dlvEntity = new DlvEntity();
                reader.Close();
                reader.Dispose();
                disConnect_dbcn_ExcuteReader();
                return dlvEntity;
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
