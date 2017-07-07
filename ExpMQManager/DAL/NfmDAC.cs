using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;

namespace ExpMQManager.DAL
{
    public class NfmDAC : BaseDAC
    {
        public List<NfmEntity> GetNFMColDAC(int flightseq)
        {
            string strSql = "";
            strSql = @"
                        SELECT (EB.UldPfx+EB.UldMid+EB.UldLst) as ULD, EB.DestCd as POU, EB.[Weight], EN.*
                        FROM Exp_NFM as EN
                        JOIN Exp_BuildupMaster as EB
                        ON EN.Exp_Flightseq = EB.Flightseq AND En.ULDID = EB.ULDID
                        WHERE Exp_FlightSeq = {0}
                        ";
            strSql = string.Format(strSql, flightseq);

            return GetNFMfromReader(ExecuteReader(strSql));
        }

        protected List<NfmEntity> GetNFMfromReader(IDataReader reader)
        {
            List<NfmEntity> nfmEntityCol = new List<NfmEntity>();
            while(reader.Read())
            {
                int id = 0; try { id = Convert.ToInt32(reader["ID"]); }
                catch{}
                int uldid = 0; try { uldid = Convert.ToInt32(reader["ULDID"]); }
                catch { }
                int overhangcnt = 9; try { overhangcnt = Convert.ToInt32(reader["OverhangCNT"]); }
                catch { }
                double uldweight = 0.00; try { uldweight= Convert.ToInt32(reader["Weight"]); }
                catch { }
                int unitvolume = 9; try { unitvolume = Convert.ToInt32(reader["UnitVolume"]); }
                catch { }

                NfmEntity nfmEntity = new NfmEntity(
                    id,
                    uldid,
                    reader["ULD"].ToString(),
                    reader["POU"].ToString(),
                    reader["EHC"].ToString(),
                    reader["IHC"].ToString(),
                    reader["UnitClass"].ToString(),
                    overhangcnt,
                    uldweight,
                    reader["UnitContour"].ToString(),
                    unitvolume,
                    reader["FinalDest"].ToString(),
                    reader["IHC2"].ToString()
                    );
                nfmEntityCol.Add(nfmEntity);
            }

            reader.Close();
            reader.Dispose();
            disConnect_dbcn_ExcuteReader();
            return nfmEntityCol;
        }
    }
}
