using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class FfmDAC : BaseDAC
    {
        public List<FfmEntity> GetFFMColDAC(int mid, int flightSeq, string msgType, string subType, string lcode)
        {
            string strSql = "";

            #region 2014-04-14
            /* 2014-04-14
		strSql = @" SELECT A.MID ,A.Prefix ,A.AWB ,A.OriginCd fOrigin ,A.DestCd fDest
	                          ,ISNULL(Partial, 'T') Partial ,OnPcs ,OnWeight ,Pcs ,Weight
	                          ,C.OriginCd Origin ,C.DestCd Dest ,Leg_seq ,Commodity, ULD 
	                          ,CASE SHC WHEN 'OTHER' THEN NULL ELSE SHCNote  END SHC
	                          ,CASE WHEN C.ChgWeight IS NOT NULL AND C.ChgWeight > 0.00 THEN C.ChgWeight
									ELSE (SELECT ISNULL(SUM(VolWeight),0) FROM Exp_DimWt WHERE MID = A.MID) END volWeight 
                        FROM Exp_FlightMaster A
                        LEFT JOIN ( SELECT flightNo, Leg_seq, MAX(Acode) Acode
			                        FROM flightTB A
			                        JOIN flightTB_Leg B ON A.idnum = B.flightTB_id
			                        WHERE flightNo = (SELECT FlightNo FROM Exp_FlightSeq WHERE FlightId = {0}) 
			                        GROUP BY flightNo, Leg_Seq) B
			                        ON A.flightNo = B.flightNo AND A.DestCd = B.Acode
                        JOIN Exp_Master C ON A.MID = C.MID
                        WHERE FlightSeq = {0} and A.[Status] = case when (A.[Status] & power(2, 3) > 0) then A.[Status] else -1 end 
                        ORDER BY Leg_seq, Case ULD WHEN 'BULK' THEN '00BULK' ELSE ULD END ASC
                    ";
            */
            /*
            strSql = @" SELECT A.MID ,A.Prefix ,A.AWB ,A.OriginCd fOrigin ,A.DestCd fDest
	                          ,ISNULL(Partial, 'T') Partial ,OnPcs ,OnWeight ,Pcs ,Weight
	                          ,C.OriginCd Origin ,C.DestCd Dest ,Leg_seq ,Commodity, ULD 
	                          ,
                                  CASE SHC WHEN 'OTHER' THEN 
		                            LTRIM(RTRIM(isnull(SHCNote,''))) + 
		                            LTRIM(RTRIM(isnull(SHC2,''))) + 
		                            LTRIM(RTRIM(isnull(SHC3,''))) + 
		                            LTRIM(RTRIM(isnull(SHC4,''))) + 
		                            LTRIM(RTRIM(isnull(SHC5,''))) 
                                  ELSE 
		                            LTRIM(RTRIM(isnull(SHC,''))) + 
		                            LTRIM(RTRIM(isnull(SHC2,''))) + 
		                            LTRIM(RTRIM(isnull(SHC3,''))) + 
		                            LTRIM(RTRIM(isnull(SHC4,''))) + 
		                            LTRIM(RTRIM(isnull(SHC5,''))) 
	                              END SHC
	                          ,CASE WHEN C.ChgWeight IS NOT NULL AND C.ChgWeight > 0.00 THEN C.ChgWeight
									ELSE (SELECT ISNULL(SUM(VolWeight),0) FROM Exp_DimWt WHERE MID = A.MID) END volWeight 
                        FROM Exp_FlightMaster A
                        LEFT JOIN ( SELECT flightNo, Leg_seq, MAX(Acode) Acode
			                        FROM flightTB A
			                        JOIN flightTB_Leg B ON A.idnum = B.flightTB_id
			                        WHERE flightNo = (SELECT FlightNo FROM Exp_FlightSeq WHERE FlightId = {0}) 
			                        GROUP BY flightNo, Leg_Seq) B
			                        ON A.flightNo = B.flightNo AND A.DestCd = B.Acode
                        JOIN Exp_Master C ON A.MID = C.MID
                        WHERE FlightSeq = {0} and A.[Status] = case when (A.[Status] & power(2, 3) > 0) then A.[Status] else -1 end 
                        ORDER BY Leg_seq, Case ULD WHEN 'BULK' THEN '00BULK' ELSE ULD END ASC
                    ";
            */

            // 2014-04-18
            //            strSql = @" 
            //                        SELECT A.MID ,A.Prefix ,A.AWB ,A.OriginCd fOrigin ,A.DestCd fDest
            //                              ,ISNULL(Partial, 'T') Partial ,OnPcs ,OnWeight ,Pcs ,C.Weight
            //                              ,C.OriginCd Origin ,C.DestCd Dest ,Leg_seq ,Commodity, ULD 
            //                              --, E.BUPC
            //                              , ( CASE WHEN isnull(E.BUPC,0) = 0 THEN OnPCS ELSE E.BUPC END ) AS BUPC
            //                              ,
            //                                  CASE SHC WHEN 'OTHER' THEN 
            //                                    LTRIM(RTRIM(isnull(SHCNote,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC2,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC3,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC4,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC5,''))) 
            //                                  ELSE 
            //                                    LTRIM(RTRIM(isnull(SHC,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC2,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC3,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC4,''))) + 
            //                                    LTRIM(RTRIM(isnull(SHC5,''))) 
            //                                  END SHC
            //                              ,CASE WHEN C.ChgWeight IS NOT NULL AND C.ChgWeight > 0.00 THEN C.ChgWeight
            //			                        ELSE (SELECT ISNULL(SUM(VolWeight),0) FROM Exp_DimWt WHERE MID = A.MID) END volWeight 
            //                        FROM Exp_FlightMaster A
            //                        LEFT JOIN ( SELECT flightNo, Leg_seq, MAX(Acode) Acode
            //                                    FROM flightTB A
            //                                    JOIN flightTB_Leg B ON A.idnum = B.flightTB_id
            //                                    WHERE flightNo = (SELECT FlightNo FROM Exp_FlightSeq WHERE FlightId = {0}) 
            //                                    GROUP BY flightNo, Leg_Seq) B
            //                                    ON A.flightNo = B.flightNo AND A.DestCd = B.Acode
            //                        JOIN Exp_Master C ON A.MID = C.MID
            //                        LEFT JOIN Exp_BuildupMaster as D on A.ULD = D.UldPfx + D.UldMid + D.UldLst and A.FlightSeq = D.FlightSeq
            //                        LEFT JOIN Exp_BuildupDetail as E on D.ULDID = E.ULDID and C.MID = E.MID
            //                        WHERE A.FlightSeq = {0} and A.[Status] = case when (A.[Status] & power(2, 3) > 0) then A.[Status] else -1 end 
            //                        ORDER BY Leg_seq, Case ULD WHEN 'BULK' THEN '00BULK' ELSE ULD END ASC
            //                    ";



            //2014.07.21 add Condition ccode in flightTB (joh) 
            #endregion

            #region 2016-6-17
            //strSql = @"
            //            SELECT 
            //             A.MID 
            //             ,A.Prefix 
            //             ,A.AWB 
            //             ,A.OriginCd fOrigin 
            //             ,A.DestCd fDest
            //             ,ISNULL(Partial, 'T') Partial ,OnPcs ,OnWeight ,Pcs ,C.Weight
            //             ,C.OriginCd Origin ,C.DestCd Dest ,Leg_seq ,Commodity, ULD 
            //             , ( CASE WHEN isnull(E.BUPC,0) = 0 THEN OnPCS ELSE E.BUPC END ) AS BUPC
            //                  ,
            //                      CASE SHC WHEN 'OTHER' THEN 
            //                        LTRIM(RTRIM(isnull(SHCNote,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC2,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC3,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC4,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC5,''))) 
            //                      ELSE 
            //                        LTRIM(RTRIM(isnull(SHC,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC2,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC3,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC4,''))) + 
            //                        LTRIM(RTRIM(isnull(SHC5,''))) 
            //                      END SHC
            //                      ,CASE WHEN C.ChgWeight IS NOT NULL AND C.ChgWeight > 0.00 THEN C.ChgWeight
            //                                    --ELSE (SELECT (ISNULL(SUM(VolWeight),0) / 166) FROM Exp_DimWt WHERE MID = A.MID) END volWeight 
            //                                      ELSE (A.OnWeight / 166) END volWeight 
            //            FROM Exp_FlightMaster A
            //            LEFT JOIN ( select b.flightno, b.Ccode, b.Leg_seq, b.Acode
            //               from
            //               (
            //                select Max(aa.CCode) Ccode, Max(bb.FlightNo)FlightNo
            //                from Exp_Master aa
            //                join Exp_FlightMaster bb
            //                on aa.MID = bb.MID
            //                and bb.FlightSeq = {0}
            //               ) a
            //               join (
            //                SELECT a.Ccode, flightNo, Leg_seq, Acode
            //                FROM flightTB A
            //                JOIN flightTB_Leg B ON A.idnum = B.flightTB_id
            //                            and A.sort = 'E'
            //               ) b
            //               on a.Ccode = b.Ccode
            //               and a.FlightNo = b.FlightNo
            //               ) B
            //            ON A.flightNo = B.flightNo 
            //            AND A.DestCd = B.Acode
            //            JOIN Exp_Master C ON A.MID = C.MID and C.Lcode = '{1}'
            //            LEFT JOIN Exp_BuildupMaster as D on A.ULD = D.UldPfx + D.UldMid + D.UldLst and A.FlightSeq = D.FlightSeq and c.Lcode = d.Lcode
            //            LEFT JOIN Exp_BuildupDetail as E on D.ULDID = E.ULDID and C.MID = E.MID
            //            WHERE 
            //                A.FlightSeq = {2} 
            //                and A.[Status] = case when (A.[Status] & power(2, 3) > 0) then A.[Status] else -1 end 
            //            ORDER BY Leg_seq,A.DestCd, Case ULD WHEN 'BULK' THEN '00BULK' ELSE ULD END ASC"; 
            #endregion

            strSql = @"
                        DECLARE @tULDIDtable TABLE (ULDID INT)
                        INSERT INTO @tULDIDtable
                        SELECT DISTINCT ULDID FROM Exp_BuildupMaster WHERE FlightSeq = {0}

                        SELECT  

                        ExpM.MID, ExpM.Prefix, ExpM.AWB
                        , ExpFSeq.Origin as fOrigin, ExpBM.DestCd as fDest

                        , (
	                        CASE WHEN (Pcs = ExpBD.BUPC) THEN 'T'
		                        ELSE 
			                        (
				                        CASE WHEN Pcs = (SELECT SUM(BUPC) FROM Exp_BuildupDetail WHERE ULDID IN (SELECT * FROM @tULDIDtable) AND MID = ExpM.MID) THEN 'S'
				                        ELSE 
					                        (
						                        CASE WHEN 1 = (SELECT COUNT(*) FROM Exp_BuildupDetail WHERE ULDID IN (SELECT * FROM @tULDIDtable) AND MID = ExpM.MID) THEN 'P' 
						                        ELSE 'D' END
					                        ) END
			                        ) END
			 
	                        ) as [Partial]


                        , ExpBD.BUPC as OnPcs, ExpBD.[Weight] as OnWeight
                        , ExpM.Pcs, ExpM.[Weight], ExpM.OriginCd as Origin, ExpM.DestCd as Dest, ExpM.Commodity

                        , (	
	                        CASE WHEN RTRIM(LTRIM(ISNULL(ExpBM.UldPfx, ''))) = 'BUL' THEN 'BULK'
	                        ELSE RTRIM(LTRIM(ISNULL(ExpBM.UldPfx, ''))) + RTRIM(LTRIM(ISNULL(ExpBM.UldMid, ''))) + RTRIM(LTRIM(ISNULL(ExpBM.UldLst, ''))) END 
	                        )as ULD

                        , ISNULL(ExpBD.BUPC, 0) as BUPC

                        , (
	                        CASE ExpM.SHC WHEN 'OTHER' THEN 
                            LTRIM(RTRIM(isnull(ExpM.SHCNote,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC2,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC3,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC4,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC5,''))) 
                            ELSE 
                            LTRIM(RTRIM(isnull(ExpM.SHC,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC2,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC3,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC4,''))) + 
                            LTRIM(RTRIM(isnull(ExpM.SHC5,''))) 
                            END
	                        ) as SHC

                        , (
	                        CASE WHEN ExpM.ChgWeight IS NOT NULL AND ExpM.ChgWeight > 0 THEN ExpM.ChgWeight
                            ELSE ( ExpBD.[Weight] / 166 ) END 
	                        ) as volWeight 
                        
                        -- added by Cecile. on 2018-5-18 9:29am
                        --,(
                        --    CASE WHEN ExpM.ChgWeight IS NOT NULL AND ExpM.ChgWeight > 0 THEN ExpM.ChgWeight
						--		   WHEN ExpDW.VolWeight IS NOT NULL AND ExpDW.VolWeight> 0 THEN (ExpDW.VolWeight / 166)
                        --         ELSE (CASE WHEN (ExpBD.[Weight] / 600) < 0.01 THEN 0.01 ELSE (ExpBD.[Weight] / 600) END) 
						--	END 
	                    --  ) as volWeight 

                        FROM Exp_BuildupMaster as ExpBM
                        JOIN Exp_BuildUpDetail as ExpBD
                        on ExpBM.ULDID = ExpBD.ULDID

                        JOIN Exp_Master as ExpM
                        on ExpBD.MID = ExpM.MID and ExpM.Lcode = '{1}'
                        
                        -- added by Cecile. on 2018-5-18 9:29am
                        LEFT JOIN (
							SELECT  MID, VolWeight FROM Exp_DimWt_FBL
							WHERE DID IN (SELECT MAX(A.DID) FROM Exp_DimWt_FBL AS A
											JOIN Exp_BuildupDetail AS B 
											ON A.MID = B.MID
											WHERE ULDID IN (SELECT * FROM @tULDIDtable)
											GROUP BY A.MID)
						) as ExpDW
						ON ExpM.MID = ExpDW.MID

                        JOIN Exp_FlightSeq as ExpFSeq
                        on ExpBM.FlightSeq = ExpFSeq.FlightId

                        WHERE ExpBM.FlightSeq = {0} AND ExpBM.[Status] = 8
                        ORDER BY fDest ASC, (CASE WHEN ExpBM.UldPfx = 'BUL' THEN '00BULK' ELSE (ExpBM.UldPfx + ExpBM.UldMid + ExpBM.UldLst) END) ASC

                        ";

            strSql = string.Format(strSql, flightSeq, lcode);

            List<FfmEntity> FFMList = GetFFMfromReader(ExecuteReader(strSql));

            if (FFMList != null && FFMList.Count > 0)
            {
                strSql = @"
                            SELECT MID, Seq, s_specialhdcd
                            FROM Exp_Master_SPH WHERE MID IN ( SELECT DISTINCT MID FROM Exp_FlightMaster WHERE Flightseq = {0})

                        ";
                strSql = string.Format(strSql, flightSeq);
                List<FfmSHCEntity> shcList = GetSHCListfromReader(ExecuteReader(strSql));

                if (shcList != null && shcList.Count > 0)
                {
                    foreach (FfmEntity row in FFMList)
                    {
                        if (shcList.Where(x => x.MID == row.mid).ToList().Count > 0)
                        {
                            row.SHCList = shcList.Where(x => x.MID == row.mid).ToList();
                        }
                    }
                }
            }
            return FFMList;
        }

        protected List<FfmEntity> GetFFMfromReader(IDataReader reader)
        {
            List<FfmEntity> ffmEntityCol = new List<FfmEntity>();
            while (reader.Read())
            {
                int mid = 0; try { mid = Convert.ToInt32(reader["MID"]); }
                catch
                {}
                int pcs = 0; try { pcs = Convert.ToInt32(reader["pcs"]); }
                catch
                {
                }
                double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                catch (Exception e)
                {
                }
                int fPcs = 0; try { fPcs = Convert.ToInt32(reader["OnPcs"]); }
                catch
                {
                }
                double fWeight = 0.00; try { fWeight = Convert.ToDouble(reader["OnWeight"].ToString()); }
                catch (Exception e)
                {
                }
                double vWeight = 0.00; try { vWeight = Convert.ToDouble(reader["volWeight"].ToString()); }
                catch (Exception e)
                {
                }

                int legSeq = 0; try { legSeq = Convert.ToInt32(reader["Leg_seq"]); }
                catch
                {
                }
                // 2014-04-18
                int builtPcs = 0; try { builtPcs = Convert.ToInt32(reader["BUPC"]); }
                catch
                {
                }
                // 2014-04-18
                FfmEntity ffmEntity = new FfmEntity(
                    mid,
                    reader["prefix"].ToString().Trim(),
                    reader["AWB"].ToString().Trim(),
                    reader["Origin"].ToString().Trim(),
                    reader["Dest"].ToString().Trim(),
                    reader["fOrigin"].ToString().Trim(),
                    reader["fDest"].ToString().Trim(),
                    reader["Partial"].ToString().Trim(),
                    pcs,
                    weight,
                    fPcs,
                    fWeight,
                    vWeight,
                    legSeq,
                    reader["Commodity"].ToString().Trim(),
                    reader["SHC"].ToString().Trim(),
                    reader["ULD"].ToString().Trim(),
                    builtPcs
                    );

                ffmEntityCol.Add(ffmEntity);

            }
            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return ffmEntityCol;

        }
        protected List<FfmSHCEntity> GetSHCListfromReader(IDataReader reader)
        {
            List<FfmSHCEntity> shcList = new List<FfmSHCEntity>();
            while (reader.Read())
            {
                int MID = 0; try { MID = Convert.ToInt32(reader["MID"]); }
                catch { }
                int Seq = 0; try { Seq = Convert.ToInt32(reader["Seq"]); }
                catch { }


                FfmSHCEntity temp = new FfmSHCEntity(
                    MID,
                    Seq,
                    reader["s_specialhdcd"].ToString()
                    );
                shcList.Add(temp);
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return shcList;
        }
    }
}
