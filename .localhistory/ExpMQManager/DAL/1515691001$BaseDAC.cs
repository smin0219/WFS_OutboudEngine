using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ExpMQManager.DAL
{
    public class BaseDAC : BaseEntity
    {
        public bool isLiveParsing = Convert.ToBoolean(ConfigurationManager.AppSettings["IsLiveParsing"]);

        public BaseEntity GetBaseAWBInfoDAC(int mid, int flightSeq, string msgType, string subType, int queueId)
        {
            string strSql = "";
            BaseEntity baseEntity = new BaseEntity();

            if (msgType == "FSU")
            {
                switch (subType)
                {
                    /* Import Common Message Header */
                    case "DLV":
                    case "RTF":
                    case "RCF":
                    case "DIS":
                    case "NFD":
                    case "AWD":
                    case "ARR":
                    case "TFD":
                        strSql = @" SELECT   A.iid 
                                            ,A.msgType 
                                            ,A.subMsgType 
                                            ,A.MID 
                                            ,FlightSeq 
                                            ,A.Lcode
                                            ,A.Ccode 
                                            ,A.createdDate 
                                            ,A.CreatedBy 
                                            ,Prefix 
                                            ,AWB
                                            ,Origin
                                            ,OriginPortCd OriginCd 
                                            ,DestinationPortCd DestCd
                                            ,Dest
                                            ,Partial 
                                            ,case when B.Pcs = 0 then C.Pcs else B.Pcs end as Pcs 
                                            ,case when B.Weight = 0 then C.Weight else B.Weight end as Weight  
                                            ,msgVersion 
                                            ,msgAddress
                                            ,B.shipper
                                        FROM EDI_Msg_Queue as A
                                        LEFT JOIN ePic_Master2 as B ON A.MID = B.MID
                                        LEFT JOIN (SELECT MID, MAX(Partial) Partial, FinalDest as Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs, MAX(Origin) Origin
                                            FROM ePic_FlightMaster where flightseq = {2} GROUP BY MID, FinalDest) as C ON A.MID = C.MID
                                        LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Active, Dest as Destination, SubMsgType
                                            FROM EDI_Address GROUP BY Carrier, MsgType, SubMsgType, Dest, Active) as  D 
                                            ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 

                                            --AND C.Dest = case when D.Destination <> '' or D.Destination <> null then D.Destination else C.Dest end

                                            --2015-08-25 Destination Filter
                                            -- plus. Submsgtype Like added. 2015-10-08
                                            AND ISNULL(D.Destination, '') = ( CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND Dest = C.Dest AND msgType = 'FSU' AND (SubMsgType LIKE '%{1}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN C.Dest
                                                                                   WHEN EXISTS  (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND (Dest = '' or Dest is NULL) AND msgType = 'FSU' AND (SubMsgType LIKE '%{1}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN ''
															                       ELSE 'NON' END)
                                            AND (D.subMsgType LIKE '%{1}%' or D.SubMsgType is NULL or D.SubMsgType = '')
                                            
                                    WHERE A.MID = {0} AND A.msgType = 'FSU' AND A.subMsgType = '{1}' AND A.iid = {3}
                                          	--AND A.Status = 'W' 
                            ";
                        if (isLiveParsing)
                        {
                            strSql = strSql + " AND A.Status = 'W' ";
                        }

                        strSql = string.Format(strSql, mid, subType, flightSeq, queueId);
                        break;

                    // added for real time RCF. 2018-1-11
                    case "DIS":

                        break;

                    /* Export Common Message Header */
                    case "MAN":
                    case "RCS":

                    //ADDED "RDS" (it is same as RCS) 2015-08-12
                    case "RDS":
                    case "RCT":

                    //ADDED "RDS" (it is same as RCS) 2015-08-12
                    case "RDT":

                    // added. 2016-11-08
                    case "FOH":

                    case "DEP":

                        //EDI_Address Dest filter. 2015-08-25
                        strSql = @"
                                        SELECT A.iid ,A.msgType ,A.subMsgType ,A.MID, FlightSeq ,A.Lcode
                                          ,A.Ccode ,A.createdDate ,A.CreatedBy ,Prefix ,AWB
                                          ,Origin, OriginCd ,DestCd, Dest
                                          ,Partial 
                                          ,case when B.Pcs = 0 then C.Pcs else B.Pcs end as Pcs 
                                          ,case when B.Weight = 0 then C.Weight else B.Weight end as Weight  ,msgVersion ,msgAddress
                                          ,msgVersion ,msgAddress
                                          ,B.shipper
                                    FROM EDI_Msg_Queue A
                                    LEFT JOIN Exp_Master B ON A.MID = B.MID
                                    LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(DestCd) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs, MAX(OriginCd) Origin
                                            FROM Exp_FlightMaster GROUP BY MID) C ON A.MID = C.MID
                                    LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Active, Dest as Destination, SubMsgType
                                            FROM EDI_Address GROUP BY Carrier, MsgType, SubMsgType, Dest, Active) D ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1'

                                            --AND C.Origin = case when D.Destination <> '' or D.Destination <> null then D.Destination else C.Origin end

                                            --2015-08-25
                                            -- plus. Submsgtype Like added. 2015-10-08
                                            AND ISNULL(D.Destination, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND  Dest = C.Origin AND msgType = 'FSU' AND (SubMsgType LIKE '%{1}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN C.Origin
                                                                                  WHEN EXISTS  (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND (Dest = '' or Dest is NULL) AND msgType = 'FSU' AND (SubMsgType LIKE '%{1}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN ''
                                                                                  ELSE 'NON' END)
                                            AND (D.subMsgType LIKE '%{1}%' or D.SubMsgType is NULL or D.SubMsgType = '')

                                    WHERE A.MID = {0} AND A.msgType = 'FSU' AND A.subMsgType = '{1}' AND A.iid = {2}
                                    --AND A.Status = 'W' 
                            ";

                        if (isLiveParsing)
                        {
                            strSql = strSql + " AND A.Status = 'W' ";
                        }

                        strSql = string.Format(strSql, mid, subType, queueId);
                        break;
                }

                baseEntity = GetBaseAWBFromReader(ExecuteReader(strSql));
            }


            //NFM Added by NA
            if (msgType == "NFM")
            {
                strSql = @" SELECT EMQ.iid, EMQ.msgType, EMQ.subMsgType, EMQ.FlightSeq, EMQ.Lcode,
                            EMQ.Ccode,
                            EN.MsgVersion, EN.MsgAddress,
                            EF.FlightNo, EF.FlightDate,
                            EFD.Origin as FlightOrigin, EFD.Destination as FlightDest
                            FROM EDI_Msg_Queue as EMQ
                            LEFT JOIN 
                            (SELECT MsgVersion, Exp_FlightSeq, Origin, MsgAddress 
                            FROM Exp_NFM 
                            WHERE Exp_FlightSeq = {0} 
                            GROUP BY MsgVersion, Exp_FlightSeq, Origin, MsgAddress) as EN
                            ON EMQ.FlightSeq = EN.Exp_FlightSeq

                            JOIN EXP_FlightSeq as EF
                            ON EMQ.FlightSeq = EF.FlightId

                            JOIN EXP_FlightSeqDetail as EFD
                            ON EMQ.FlightSeq = EFD.FlightSeq AND EN.Origin = EFD.Origin AND EMQ.Lcode = EFD.Lcode

                            WHERE EMQ.FlightSeq = {0}
                            AND EMQ.MsgType = 'NFM'
                            AND EMQ.iid = {1}
                           ";
                if (isLiveParsing)
                    strSql = strSql + "AND EMQ.Status = 'W'";

                strSql = string.Format(strSql, flightSeq, queueId);

                baseEntity = GetBaseNFMFromReader(ExecuteReader(strSql));

            }
            ////////////////////////////////////////////// BASE ENTITY for NFM

            // Added. UWS 2016-8-17
            if(msgType == "UWS")
            {
                strSql = @" 
                            SELECT
                            EMQ.iid, EMQ.msgType, EMQ.FlightSeq, EMQ.Lcode, EMQ.Ccode,
                            EU.MsgAddress, 0 as MsgVersion,
                            EF.FlightNo, EF.Origin as FlightOrigin, EF.Destination as FlightDest,
                            (CASE WHEN EF.UWSSentDate is NULL THEN EF.FlightDate ELSE EF.UWSSentDate END) as FlightDate

                            FROM Edi_Msg_Queue as EMQ

                            JOIN Exp_FlightSeq as EF
                            ON EMQ.FlightSeq = EF.FlightId
                            
                            LEFT JOIN
                            ( SELECT MsgAddress, FlightSeq FROM Exp_UWS
	                            WHERE FlightSeq = {0}
	                            GROUP BY MsgAddress, FlightSeq ) as EU
                            ON EMQ.FlightSeq = EU.Flightseq
                            
                            WHERE EMQ.FlightSeq = {0} AND EMQ.MsgType = 'UWS'
                            AND EMQ.iid = {1}
                           ";
                if (isLiveParsing)
                    strSql = strSql + "AND EMQ.Status = 'W'";

                strSql = string.Format(strSql, flightSeq, queueId);

                baseEntity = GetBaseUWSFromReader(ExecuteReader(strSql));
            }

            if (msgType == "FFM")
            {
                /*
                strSql = @" SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                                  ,A.FlightNo ,Origin ,DepDate, B.FlightDate
                            FROM EDI_Msg_Queue A
                            JOIN Exp_FlightSeq B ON A.FlightSeq = B.FlightId
                            LEFT JOIN (SELECT FlightId, LocalDepDate as DepDate FROM Exp_FlightSeqMsg WHERE SeqNo = 
		                            (SELECT MAX(SeqNo) FROM Exp_FlightSeqMsg WHERE FlightId = {0} AND UtcDepDateType = 'A')) 
	                            C ON A.FlightSeq = C.FlightId
                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Active 
                                    FROM EDI_Address GROUP BY Carrier, MsgType, Active) D ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 
                            WHERE FlightSeq = {0} AND A.MsgType = 'FFM' 
                            --AND A.Status = 'W'
                        ";*/

                //add condition Dest 
                // multi station engine publish할 때 exp_flightSeqDetail에 Origin값을 채우고 cc.Origin으로 바꿔야 한다.
                strSql = @" SELECT A.iid ,A.msgType ,A.subMsgType ,A.FlightSeq ,A.Lcode
                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                                  ,A.FlightNo , b.Origin,DepDate, B.FlightDate
                            FROM EDI_Msg_Queue A
                            JOIN Exp_FlightSeq B ON A.FlightSeq = B.FlightId
                            Left join
                               Exp_FlightSeqDetail cc on A.FlightSeq = cc.FlightSeq
                               and A.Lcode = cc.Lcode
                            LEFT JOIN (SELECT FlightId, LocalDepDate as DepDate FROM Exp_FlightSeqMsg WHERE SeqNo = 
		                            (SELECT MAX(SeqNo) FROM Exp_FlightSeqMsg WHERE FlightId = {0} AND UtcDepDateType = 'A')) 
	                            C ON A.FlightSeq = C.FlightId
                            LEFT JOIN (SELECT MsgType, Carrier, MsgVersion, MsgAddress, Active , Dest
                                    FROM EDI_Address) D 
                            ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1'
                            --and  ( D.Dest is null or D.Dest = '' or D.Dest like '%'+cc.Origin+'%' )

                            AND ISNULL(D.Dest, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND dest = B.Acode AND A.msgType = Msgtype AND Active = '1') THEN B.Acode
                                                           WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND (Dest = '' or Dest is NULL) AND A.MsgType = MsgType AND Active = '1') THEN ''
                                                           ELSE 'NON' END)
                            WHERE A.FlightSeq = {0} AND A.MsgType = 'FFM' 
                            and A.iid = {1}
                        ";

                if (isLiveParsing)
                {
                    strSql = strSql + " AND A.Status = 'W' ";
                }

                strSql = string.Format(strSql, flightSeq, queueId);

                baseEntity = GetBaseFFMFromReader(ExecuteReader(strSql));
            }

            if (msgType == "FWB")
            {
                #region Original
                /*strSql = @" SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                                  ,A.Carrier ,OriginCd ,DestCd ,Pcs ,Weight ,AwbPOU Dest
                                  ,Prefix ,AWB ,A.MID
                            FROM EDI_Msg_Queue A
                            JOIN Exp_Master B ON A.MID = B.MID
                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress,  Active 
                                    FROM EDI_Address GROUP BY Carrier, MsgType, Active) D ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 
                            WHERE A.MID = {0}  AND A.MsgType = 'FWB' 
                            --AND A.Status = 'W'
                        ";*/

                //                strSql = @" SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                //                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                //                                  ,A.Carrier ,OriginCd ,DestCd ,Pcs ,Weight ,AwbPOU Dest
                //                                  ,Prefix ,AWB ,A.MID
                //                            FROM EDI_Msg_Queue A
                //                            JOIN Exp_Master B ON A.MID = B.MID
                //                            
                //                            JOIN (SELECT MID, OriginCd as flightOrigin FROM EXP_Flightmaster ) as C
                //							ON B.MID = C.MID
                //                            
                //                            LEFT JOIN (SELECT MsgType, Carrier, MsgVersion, MsgAddress,  Active , Dest
                //                                       FROM EDI_Address
                //                                    ) 
                //                            D ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 
                //                            AND (D.Dest is null or D.Dest = '' or D.Dest like '%'+C.flightOrigin+'%' )
                //                            WHERE A.MID = {0}  AND A.MsgType = 'FWB' 
                //
                //                        ";


                //Modified 2015-06-30
                //AND (D.Dest is null or D.Dest = '' or D.Dest like '%'+B.awbpou+'%' ) 에서 b.awbpou 를 b.origincd 로 변경



                //if (isLiveParsing)
                //{
                //    strSql = strSql + " AND A.Status = 'W' ";
                //}
                //strSql = string.Format(strSql, mid);

                //baseEntity = GetBaseFWBFromReader(ExecuteReader(strSql)); 
                #endregion

                //2015-09-29 NEW AB ADDED :: check edi_addressbook if EDIAddressbook == 1
                strSql = @"

                    DECLARE @isNewAB		INT
					DECLARE @CustomerId		VARCHAR(20)
					DECLARE @Origin			VARCHAR(3)
					DECLARE @Dest			VARCHAR(3)
					DECLARE @EDIABidnum		INT
					DECLARE @flightSeq		INT
                    DECLARE @Carrier        VARCHAR(2)

					SET @isNewAB = (SELECT EDIAddressBook FROM EDI_MSG_QUEUE WHERE iid = {1})       --queueId
					SET @flightSeq = (SELECT flightSeq FROM EDI_MSG_QUEUE WHERE iid = {1})          --queueId
                    SET @Carrier = (SELECT Carrier FROM EDI_MSG_QUEUE WHERE iid = {1})              --queueId
					SET @CustomerId = (SELECT CustomerId FROM EDI_MSG_QUEUE WHERE iid = {1})        --queueId	    	        
					SET @Origin = (SELECT TOP 1 OriginCd  FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = {0} )    -- for EDI_AddressBook idnum
					SET @Dest = (SELECT TOP 1 DestCd FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = {0})		    -- for EDI_AddressBook idnum
					SET @EDIABidnum = ( SELECT idnum FROM EDI_Addressbook 
                                        WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB'
                                        AND Origin = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Origin = @Origin) THEN @Origin
                                                            WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Origin = 'ALL') THEN 'ALL'
                                                            ELSE '' END )

                                        AND Dest = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Dest = @Dest) THEN @Dest
                                                          WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Dest = 'ALL') THEN 'ALL'
                                                          ELSE '' END )
                                      ) --msgType

					IF(@isNewAB = 1)
					    BEGIN
                            SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                                      ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                                      ,A.Carrier ,OriginCd ,DestCd ,Pcs ,Weight ,AwbPOU Dest
                                      ,Prefix ,AWB ,A.MID
								      ,newEDIAB.MsgVersion as msgVersion, newEDIAB.MsgAddress as msgAddress
                                FROM EDI_Msg_Queue A
                                JOIN Exp_Master B ON A.MID = B.MID
                            
                                JOIN (SELECT MID, OriginCd as flightOrigin FROM EXP_Flightmaster group by mid, OriginCd ) as C
							    ON B.MID = C.MID

							    LEFT JOIN (
								    SELECT idnum, CustomerId, MsgVersion, RTRIM(STUFF(
											    (SELECT ABD.MsgAddress+' '
											    FROM EDI_AddressbookDetail as ABD 
											    JOIN EDI_Addressbook as AB
											    ON AB.idnum = ABD.EDIABidnum
											    WHERE AB.idnum = @EDIABidnum			--<<========
											    ORDER BY ABD.SendType DESC
											    for XML PATH(''), type
												).value('.', 'nvarchar(max)')
												, 1, 0,'')) as MsgAddress
								    FROM EDI_Addressbook WHERE idnum = @EDIABidnum		--<<========
								    GROUP BY idnum, CustomerId, MsgVersion
								    ) as newEDIAB
								    on newEDIAB.CustomerId = A.CustomerId

							    WHERE A.MID = {0}  AND A.MsgType = 'FWB' AND A.iid = {1}
                                {3}
					    END -- EDIAddressBook == 1
					ELSE -- EDIAddressBook == 0 or EDIAddressBook is NULL
					    BEGIN
                            SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                                  ,A.Carrier ,OriginCd ,DestCd ,Pcs ,Weight ,AwbPOU Dest
                                  ,Prefix ,AWB ,A.MID
                            FROM EDI_Msg_Queue A
                            JOIN Exp_Master B ON A.MID = B.MID
                            
                            JOIN (SELECT MID, OriginCd as flightOrigin FROM EXP_Flightmaster group by mid, OriginCd ) as C
							ON B.MID = C.MID
                            
                            LEFT JOIN (SELECT MsgType, Carrier, MsgVersion, MsgAddress,  Active , Dest
                                       FROM EDI_Address
                                    ) 
                            D ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 
                            
                            --AND (D.Dest is null or D.Dest = '' or D.Dest like '%'+C.flightOrigin+'%' )
                            AND ISNULL(D.Dest, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND Dest = C.flightOrigin AND Active = '1') THEN C.flightOrigin
                                                           WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND (Dest = '' or Dest is NULL) AND Active = '1') THEN ''
											               ELSE 'NON' END)
                            WHERE A.MID = {0}  AND A.MsgType = 'FWB' AND A.iid = {1}
                            {3}
					    END
                        ";
                string isLivechk = "";
                if (isLiveParsing)
                {
                    isLivechk = " AND A.Status = 'W' ";
                }
                strSql = string.Format(strSql, mid, queueId, flightSeq, isLivechk);

                baseEntity = GetBaseFWBFromReader(ExecuteReader(strSql));
            }

            if (msgType == "IRP")
            {
                strSql = @" SELECT A.iid ,A.msgType ,SITAAddy msgAddress, IRPSubject
                            FROM EDI_Msg_Queue A
                            JOIN AL_IRP B ON A.MID = B.idnum
                            WHERE MID = {0} AND A.MsgType = 'IRP' 
                            --AND A.Status = 'W'
                        ";
                if (isLiveParsing)
                {
                    strSql = strSql + " AND A.Status = 'W' ";
                }
                strSql = string.Format(strSql, mid);

                baseEntity = GetBaseIRPFromReader(ExecuteReader(strSql));

            }

            if (msgType == "FHL")
            {
                #region Original
                //                /*
                //                strSql = @" SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                //                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                //                                  ,A.Carrier ,B.Origin ,Dest ,C.Pcs ,C.Weight
                //                                  ,B.Prefix ,B.AWB ,A.MID, C.OriginCd, C.DestCd, C.AwbPOU
                //                            FROM EDI_Msg_Queue A
                //                            JOIN Exp_House B ON B.HID = A.HID
                //                            JOIN Exp_Master C on B.MID = C.MID
                //                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Active 
                //                                    FROM EDI_Address GROUP BY Carrier, MsgType, Active) D ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 
                //                            WHERE A.HID = {0}  AND A.MsgType = 'FHL' 
                //                            --AND A.Status = 'W'
                //                        ";*/
                //                strSql = @" SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                //                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                //                                  ,A.Carrier ,B.Origin ,B.Dest ,C.Pcs ,C.Weight
                //                                  ,B.Prefix ,B.AWB ,A.MID, C.OriginCd, C.DestCd, C.AwbPOU
                //                            FROM EDI_Msg_Queue A
                //                            JOIN Exp_House B ON B.HID = A.HID
                //                            JOIN Exp_Master C on B.MID = C.MID
                //
                //                            JOIN (SELECT MID, OriginCd as flightOrigin FROM EXP_Flightmaster ) as expfm
                //							ON C.MID = expfm.MID
                //
                //                            LEFT JOIN (SELECT MsgType, Carrier, MsgVersion, MsgAddress, Active , Dest
                //                                    FROM EDI_Address ) D 
                //                            ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 
                //                            AND (D.Dest is null or D.Dest = '' or D.Dest like '%'+expfm.flightOrigin+'%' )
                //                            WHERE A.HID = {0}  AND A.MsgType = 'FHL' ";

                //                //Modified 2015-06-30
                //                //AND (D.Dest is null or D.Dest = '' or D.Dest like '%'+C.awbpou+'%' ) 에서 c.awbpou 를 c.origincd 로 변경 


                //if (isLiveParsing)
                //{
                //    strSql = strSql + " AND A.Status = 'W' ";
                //}
                //strSql = string.Format(strSql, mid);

                #endregion

                //2015-09-30 NEW AB ADDED :: check edi_addressbook if EDIAddressbook == 1
                strSql = @"
					DECLARE @isNewAB		INT
					DECLARE @CustomerId		VARCHAR(20)
					DECLARE @Origin			VARCHAR(3)
					DECLARE @Dest			VARCHAR(3)
					DECLARE @EDIABidnum		INT
					DECLARE @flightSeq		INT
                    DECLARE @MID            INT
                    DECLARE @Carrier        VARCHAR(2)
                    
                    SET @MID = (SELECT MID FROM Exp_House WHERE HID = {0})
					SET @isNewAB = (SELECT EDIAddressBook FROM EDI_MSG_QUEUE WHERE iid = {1})                                   --queueId
					SET @flightSeq = (SELECT flightSeq FROM EDI_MSG_QUEUE WHERE iid = {1})                                      --queueId
                    SET @Carrier = (SELECT Carrier FROM EDI_MSG_QUEUE WHERE iid = {1})                                          --queueId
					SET @CustomerId = (SELECT CustomerId FROM EDI_MSG_QUEUE WHERE iid = {1})                                    --queueId
					SET @Origin = (SELECT TOP 1 OriginCd  FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = @MID )   -- for EDI_AddressBook idnum
					SET @Dest = (SELECT TOP 1 DestCd FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = @MID)		    -- for EDI_AddressBook idnum
					SET @EDIABidnum = ( SELECT idnum FROM EDI_Addressbook 
                                        WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL'
                                        AND Origin = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Origin = @Origin) THEN @Origin
                                                            WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Origin = 'ALL') THEN 'ALL'
                                                            ELSE '' END )

                                        AND Dest = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Dest = @Dest) THEN @Dest
                                                          WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Dest = 'ALL') THEN 'ALL'
                                                          ELSE '' END )
                                      ) --msgType

					IF(@isNewAB = 1)
					    BEGIN
							SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                                  ,A.Ccode ,A.createdDate ,A.CreatedBy, newEDIAB.msgVersion, newEDIAB.msgAddress
                                  ,A.Carrier ,B.Origin ,B.Dest ,C.Pcs ,C.Weight
                                  ,B.Prefix ,B.AWB ,A.MID, C.OriginCd, C.DestCd, C.AwbPOU
                            FROM EDI_Msg_Queue A
                            JOIN Exp_House B ON B.HID = A.HID
                            JOIN Exp_Master C on B.MID = C.MID

                            JOIN (SELECT MID, OriginCd as flightOrigin FROM EXP_Flightmaster group by mid, OriginCd ) as expfm
							ON C.MID = expfm.MID

							LEFT JOIN (
								    SELECT idnum, CustomerId, MsgVersion, RTRIM(STUFF(
											    (SELECT ABD.MsgAddress+' '
											    FROM EDI_AddressbookDetail as ABD 
											    JOIN EDI_Addressbook as AB
											    ON AB.idnum = ABD.EDIABidnum
											    WHERE AB.idnum = @EDIABidnum			--<<========
											    ORDER BY ABD.SendType DESC
											    for XML PATH(''), type
												).value('.', 'nvarchar(max)')
												, 1, 0,'')) as MsgAddress
								    FROM EDI_Addressbook WHERE idnum = @EDIABidnum		--<<========
								    GROUP BY idnum, CustomerId, MsgVersion
								    ) as newEDIAB
								    on newEDIAB.CustomerId = A.CustomerId
							WHERE A.HID = {0}  AND A.MsgType = 'FHL' AND A.iid = {1}
                            {3}
					    END -- EDIAddressBook == 1
					ELSE -- EDIAddressBook == 0
					    BEGIN
							SELECT A.iid ,A.msgType ,A.subMsgType ,FlightSeq ,A.Lcode
                                  ,A.Ccode ,A.createdDate ,A.CreatedBy ,msgVersion ,msgAddress
                                  ,A.Carrier ,B.Origin ,B.Dest ,C.Pcs ,C.Weight
                                  ,B.Prefix ,B.AWB ,A.MID, C.OriginCd, C.DestCd, C.AwbPOU
                            FROM EDI_Msg_Queue A
                            JOIN Exp_House B ON B.HID = A.HID
                            JOIN Exp_Master C on B.MID = C.MID

                            -- no need Flight INFO. 2015-12-31
                            --JOIN (SELECT MID, OriginCd as flightOrigin FROM EXP_Flightmaster group by mid, OriginCd ) as expfm
							--ON C.MID = expfm.MID

                            JOIN Exp_FlightSeq as expFseq
							ON A.FlightSeq = expFseq.FlightId

                            LEFT JOIN (SELECT MsgType, Carrier, MsgVersion, MsgAddress, Active , Dest
                                    FROM EDI_Address ) D 
                            ON A.msgType = D.msgType AND A.Carrier = D.Carrier AND D.Active = '1' 
                            
                            --AND (D.Dest is null or D.Dest = '' or D.Dest like '%'+expfm.flightOrigin+'%' )
                            AND ISNULL(D.Dest, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND Dest = expFseq.Origin AND Active = '1') THEN expFseq.Origin
                                                           WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND (Dest = '' or Dest is NULL) AND Active = '1') THEN ''
											               ELSE 'NON' END)
                            WHERE A.HID = {0}  AND A.MsgType = 'FHL'  AND A.iid = {1}
                            {3}
					    END
                            ";
                string isLivechk = "";
                if (isLiveParsing)
                {
                    isLivechk = " AND A.Status = 'W' ";
                }
                strSql = string.Format(strSql, mid, queueId, flightSeq, isLivechk);

                baseEntity = GetBaseFHLFromReader(ExecuteReader(strSql));

            }

            if (msgType == "FBR")
            {
                strSql = @"
                        SELECT EMQ.iid, EMQ.msgType, EMQ.subMsgType, EMQ.FlightSeq, EMQ.Lcode,
                            EMQ.Ccode,
                            EA.MsgVersion, EA.MsgAddress,
                            EF.FlightNo, EF.FlightDate,
                            EF.Origin as FlightOrigin, EF.Destination as FlightDest
                            FROM EDI_Msg_Queue as EMQ

                            JOIN EXP_FlightSeq as EF
                            ON EMQ.FlightSeq = EF.FlightId

							LEFT JOIN (SELECT MsgType, Carrier, MsgVersion, MsgAddress, Active , Dest
                                    FROM EDI_Address) EA
                            ON EA.msgType = (
												CASE WHEN EXISTS(SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND MsgType = 'FBR' AND Active = '1') THEN 'FBR'
												ELSE 'FFM' END
												)

                            AND EMQ.Carrier = EA.Carrier AND EA.Active = '1'

                            AND ISNULL(EA.Dest, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND dest = EF.Acode AND MsgType = 'FBR' AND Active = '1') THEN EF.Acode
                                                           WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND (Dest = '' or Dest is NULL) AND MsgType = 'FBR' AND Active = '1') THEN ''
														   WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND dest = EF.Acode AND MsgType = 'FFM'  AND Active = '1') THEN EF.Acode
                                                           WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND (Dest = '' or Dest is NULL) AND MsgType = 'FFM'  AND Active = '1') THEN ''
                                                           ELSE 'NON' END)
                            WHERE EMQ.FlightSeq = {0}
                            AND EMQ.MsgType = 'FBR'
                            AND EMQ.iid = {1}

                            {2}
                ";
                string isLivechk = "";
                if (isLiveParsing)
                {
                    isLivechk = " AND EMQ.Status = 'W' ";
                }
                strSql = string.Format(strSql, flightSeq, queueId, isLivechk);

                // it is same as NFM
                baseEntity = GetBaseNFMFromReader(ExecuteReader(strSql));

            }

            return baseEntity;
        }

        public int GetEdiAddressID(DataTable reader)
        {
            int addId = 0;
            try
            {
                if (reader != null)
                {
                    addId = Convert.ToInt32(reader.Rows[0]["ID"]);
                }
            }
            catch (Exception e)
            {
            }

            return addId;
        }


        // NFM ADDED by NA
        protected BaseEntity GetBaseNFMFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                int queueId = 0; try { queueId = Convert.ToInt32(reader["iid"]); }
                catch
                {
                }
                int msgVersion = 0; try { msgVersion = Convert.ToInt32(reader["MsgVersion"]); }
                catch
                {
                }
                int flightSeq = 0; try { flightSeq = Convert.ToInt32(reader["flightSeq"]); }
                catch
                {
                }
                BaseEntity baseEntity = new BaseEntity(
                    queueId,
                    reader["msgType"].ToString(),
                    flightSeq,
                    reader["Lcode"].ToString(),
                    reader["Ccode"].ToString(),
                    reader["MsgAddress"].ToString(),
                    msgVersion,
                    reader["FlightNo"].ToString(),
                    Convert.ToDateTime(reader["FlightDate"]),
                    reader["FlightOrigin"].ToString(),
                    reader["FlightDest"].ToString()
                    );

                reader.Close();
                reader.Dispose();

                return baseEntity;
            }
            else
            {
                BaseEntity baseEntity = new BaseEntity();
                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
        }

        // Added. 2016-8-17 
        protected BaseEntity GetBaseUWSFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                int queueId = 0; try { queueId = Convert.ToInt32(reader["iid"]); }
                catch
                {
                }
                int msgVersion = 0; try { msgVersion = Convert.ToInt32(reader["MsgVersion"]); }
                catch
                {
                }
                int flightSeq = 0; try { flightSeq = Convert.ToInt32(reader["flightSeq"]); }
                catch
                {
                }
                BaseEntity baseEntity = new BaseEntity(
                    queueId,
                    reader["msgType"].ToString(),
                    flightSeq,
                    reader["Lcode"].ToString(),
                    reader["Ccode"].ToString(),
                    reader["MsgAddress"].ToString(),
                    msgVersion,
                    reader["FlightNo"].ToString(),
                    Convert.ToDateTime(reader["FlightDate"]),
                    reader["FlightOrigin"].ToString(),
                    reader["FlightDest"].ToString()
                    );

                reader.Close();
                reader.Dispose();

                return baseEntity;
            }
            else
            {
                BaseEntity baseEntity = new BaseEntity();
                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
        }

        protected BaseEntity GetBaseFFMFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                int queueId = 0; try { queueId = Convert.ToInt32(reader["iid"]); }
                catch
                {
                }
                int msgVersion = 0; try { msgVersion = Convert.ToInt32(reader["msgVersion"]); }
                catch
                {
                }
                int mid = 0; try { mid = Convert.ToInt32(reader["MID"]); }
                catch
                {
                }
                int flightSeq = 0; try { flightSeq = Convert.ToInt32(reader["flightSeq"]); }
                catch
                {
                }

                BaseEntity baseEntity = new BaseEntity(
                    queueId,
                    reader["msgType"].ToString(),
                    reader["msgAddress"].ToString(),
                    msgVersion,
                    flightSeq,
                    //  2014-04-14
                    //Convert.ToDateTime(reader["DepDate"]),
                    Convert.ToDateTime(reader["FlightDate"]),
                    reader["Origin"].ToString(),
                    reader["Lcode"].ToString(),
                    reader["Ccode"].ToString(),
                    reader["FlightNo"].ToString());

                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
            else
            {
                BaseEntity baseEntity = new BaseEntity();
                reader.Close();
                reader.Dispose();
                return baseEntity;
            }


        }

        protected BaseEntity GetBaseAWBFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                int queueId = 0; try { queueId = Convert.ToInt32(reader["iid"]); }
                catch
                {
                }
                int msgVersion = 0; try { msgVersion = Convert.ToInt32(reader["msgVersion"]); }
                catch
                {
                }
                //int mid = 0; try { mid = Convert.ToInt32(reader["MID"]); }
                //catch
                //{
                //    reader.Close();
                //    reader.Dispose();
                //}
                int flightSeq = 0; try { flightSeq = Convert.ToInt32(reader["flightSeq"]); }
                catch
                {
                }
                int pcs = 0; try { pcs = Convert.ToInt32(reader["pcs"]); }
                catch
                {
                }
                double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                catch (Exception e)
                {
                }

                BaseEntity baseEntity = new BaseEntity(
                    queueId,
                    reader["msgType"].ToString().Trim(),
                    reader["subMsgType"].ToString().Trim(),
                    reader["msgAddress"].ToString().Trim(),
                    msgVersion,
                    mid,
                    flightSeq,
                    reader["Lcode"].ToString().Trim(),
                    reader["Ccode"].ToString().Trim(),
                    Convert.ToDateTime(reader["createdDate"]),
                    reader["createdBy"].ToString().Trim(),
                    reader["prefix"].ToString().Trim(),
                    reader["AWB"].ToString().Trim(),
                    reader["Origin"].ToString().Trim(),
                    reader["OriginCd"].ToString().Trim(),
                    reader["DestCd"].ToString().Trim(),
                    reader["Dest"].ToString().Trim(),
                    reader["Partial"].ToString().Trim(),
                    pcs,
                    weight,
                    reader["shipper"].ToString().Trim());

                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
            else
            {
                BaseEntity baseEntity = new BaseEntity();
                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
        }

        protected BaseEntity GetBaseIRPFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                int queueId = 0; try { queueId = Convert.ToInt32(reader["iid"]); }
                catch
                {
                }

                BaseEntity baseEntity = new BaseEntity(
                    queueId,
                    reader["msgType"].ToString().Trim(),
                    reader["msgAddress"].ToString().Trim(),
                    reader["IRPSubject"].ToString().Trim());

                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
            else
            {
                BaseEntity baseEntity = new BaseEntity();
                reader.Close();
                reader.Dispose();
                return baseEntity;
            }

        }

        protected BaseEntity GetBaseFWBFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                //reader.Read();

                int queueId = 0; try { queueId = Convert.ToInt32(reader["iid"]); }
                catch
                {
                }
                int msgVersion = 0; try { msgVersion = Convert.ToInt32(reader["msgVersion"]); }
                catch
                {
                }
                int mid = 0; try { mid = Convert.ToInt32(reader["MID"]); }
                catch
                {
                }
                int pcs = 0; try { pcs = Convert.ToInt32(reader["pcs"]); }
                catch
                {
                }
                double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                catch (Exception e)
                {
                }

                BaseEntity baseEntity = new BaseEntity(
                    queueId,
                    reader["Ccode"].ToString().Trim(),
                    reader["msgType"].ToString().Trim(),
                    reader["msgAddress"].ToString().Trim(),
                    msgVersion,
                    mid,
                    Convert.ToDateTime(reader["createdDate"]),
                    reader["createdBy"].ToString().Trim(),
                    reader["prefix"].ToString().Trim(),
                    reader["AWB"].ToString().Trim(),
                    reader["OriginCd"].ToString().Trim(),
                    reader["DestCd"].ToString().Trim(),
                    reader["Dest"].ToString().Trim(),
                    pcs,
                    weight,
                    reader["Carrier"].ToString());

                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
            else
            {
                BaseEntity baseEntity = new BaseEntity();
                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
        }

        protected BaseEntity GetBaseFHLFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                //reader.Read();

                int queueId = 0; try { queueId = Convert.ToInt32(reader["iid"]); }
                catch
                {
                }
                int msgVersion = 0; try { msgVersion = Convert.ToInt32(reader["msgVersion"]); }
                catch
                {
                }
                int mid = 0; try { mid = Convert.ToInt32(reader["MID"]); }
                catch
                {
                }
                int pcs = 0; try { pcs = Convert.ToInt32(reader["pcs"]); }
                catch
                {
                }
                double weight = 0.00; try { weight = Convert.ToDouble(reader["Weight"].ToString()); }
                catch (Exception e)
                {
                }

                BaseEntity baseEntity = new BaseEntity(
                    queueId,
                    reader["Ccode"].ToString().Trim(),
                    reader["msgType"].ToString().Trim(),
                    reader["msgAddress"].ToString().Trim(),
                    msgVersion,
                    mid,
                    Convert.ToDateTime(reader["createdDate"]),
                    reader["createdBy"].ToString().Trim(),
                    reader["prefix"].ToString().Trim(),
                    reader["AWB"].ToString().Trim(),
                    reader["OriginCd"].ToString().Trim(),
                    reader["DestCd"].ToString().Trim(),
                    reader["AwbPOU"].ToString().Trim(),
                    pcs,
                    weight,
                    reader["Carrier"].ToString());

                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
            else
            {
                BaseEntity baseEntity = new BaseEntity();
                reader.Close();
                reader.Dispose();
                return baseEntity;
            }
        }
        public int UpdateQueue(int queueId, string status, string errMsg)
        {
            string strSql = "";
            int result = 0;

            string errUpdate = "";
            if (errMsg != "")
                errUpdate = " ,ErrorMsg = '" + errMsg.Replace("'", "''") + "' ";

            strSql = @" UPDATE EDI_Msg_Queue SET Status = '{1}' {2} WHERE iid = {0}";
            strSql = string.Format(strSql, queueId, status, errUpdate);

            if (!strSql.Equals(string.Empty))
            {
                try { result = ExecCommand(strSql); }
                catch (SqlException e) { result = -1; }
            }

            return result;
        }

        public int UpdateEmailQueue(int mailQueueId, int status)
        {
            string strSql = "";
            int result = 0;

            strSql = @" UPDATE Email_Notification_Queue SET Status = {1} WHERE SeqNo = {0}";
            strSql = string.Format(strSql, mailQueueId, status);

            if (!strSql.Equals(string.Empty))
            {
                try { result = ExecCommand(strSql); }
                catch (SqlException e) { result = -1; }
            }

            return result;
        }

        public int InsertLog(int queueId, string msgBody, string msgType, string subType)
        {
            string strSql = "";
            int result = 0;

            if (msgType.ToUpper() == "IRP")
            {
                strSql = @" INSERT INTO EDI_Msg
                               (queueId, Carrier, Lcode, Ccode, MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT iid ,A.Carrier ,Lcode ,Ccode ,MID
                                  ,0 ,B.FlightNo ,SITAAddy ,NULL ,A.msgType
                                  ,subMsgType ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
                            FROM EDI_Msg_Queue A
                            JOIN AL_IRP B ON A.MID = B.idnum
                            WHERE A.iid = {0}
                    ";
                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));
            }
            else if (msgType.ToUpper() == "FWB")
            {
                // 8/10 2015.
                // EDI_MSG_QUERE에는 RDS/RDT로 Insert.(RCS와 같은 메세지지만 우리가 구분하기 위해 RDS로 이름 지었음.)
                // EDI_MSG에는 RCS/RCT로 Insert 되어야 함. 후에 다른 모듈에서 SHOW MSG를 위해.

                // 2015-08-25
                // EDI_Address filter
                #region Original
                //                strSql = @" INSERT INTO EDI_Msg
                //                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
                //	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
                //	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                //                            SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
                //	                              ,FlightSeq ,FlightNo ,msgAddress ,msgVersion ,A.msgType
                //
                //                                  -- ADDed for RDS/RDT 2015-08-12
                //								  ,CASE WHEN subMsgType = 'RDS' THEN 'RCS' 
                //										WHEN subMsgType = 'RDT' THEN 'RCT'
                //										ELSE subMsgType END
                //
                //                                  ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
                //                            FROM EDI_Msg_Queue A
                //                            LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(Acode) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
                //                                            FROM exp_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
                //                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination
                //                                    FROM EDI_Address GROUP BY Carrier, MsgType, Dest) C 
                //                                    ON A.msgType = C.msgType AND A.Carrier = C.Carrier 
                //                                 
                //                                    --AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
                //                                    --AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
                //                                    --AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Dest else C.Destination end )
                //
                //                                    --2015-08-25
                //                                    AND C.Destination = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND dest = b.Dest) THEN b.Dest
                //                                                              WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND dest = '') THEN ''
                //															  ELSE c.Destination END)
                //
                //                            WHERE A.iid = {0}
                //                        "; 
                #endregion

                //2015-09-29 NEW AB ADDED
                strSql = @"
                        DECLARE @isNewAB		INT
						DECLARE @CustomerId		VARCHAR(20)
						DECLARE @Origin			VARCHAR(3)
						DECLARE @Dest			VARCHAR(3)
						DECLARE @EDIABidnum		INT
						DECLARE @flightSeq		INT
                        DECLARE @MID            INT
                        DECLARE @Carrier        VARCHAR(2)

                        SET @MID = (SELECT MID FROM EDI_MSG_QUEUE WHERE iid = {0})
						SET @isNewAB = (SELECT EDIAddressBook FROM EDI_MSG_QUEUE WHERE iid = {0})                                   --queueId
						SET @flightSeq = (SELECT flightSeq FROM EDI_MSG_QUEUE WHERE iid = {0})                                      --queueId
                        SET @Carrier = (SELECT Carrier FROM EDI_MSG_QUEUE WHERE iid = {0})                                          --queueId
						SET @CustomerId = (SELECT CustomerId FROM EDI_MSG_QUEUE WHERE iid = {0})			                        -- for EDI_AddressBook idnum
						SET @Origin = (SELECT TOP 1 OriginCd  FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = @MID)	-- for EDI_AddressBook idnum
						SET @Dest = (SELECT TOP 1 DestCd FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = @MID)		    -- for EDI_AddressBook idnum
						SET @EDIABidnum = ( SELECT idnum FROM EDI_Addressbook 
                                        WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB'
                                        AND Origin = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Origin = @Origin) THEN @Origin
                                                            WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Origin = 'ALL') THEN 'ALL'
                                                            ELSE '' END )

                                        AND Dest = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Dest = @Dest) THEN @Dest
                                                          WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FWB' AND Dest = 'ALL') THEN 'ALL'
                                                          ELSE '' END )
                                      ) --msgType
						
						IF(@isNewAB = 1)
							BEGIN
								INSERT INTO EDI_Msg
								(queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
								
                                SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
									  ,FlightSeq ,FlightNo , newEDIAB.msgAddress ,newEDIAB.msgVersion ,A.msgType

									  -- ADDed for RDS/RDT 2015-08-12
									  ,CASE WHEN subMsgType = 'RDS' THEN 'RCS' 
											WHEN subMsgType = 'RDT' THEN 'RCT'
											ELSE subMsgType END

									  ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
								
                                FROM EDI_Msg_Queue A
								LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(Acode) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
												FROM exp_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
								
								
								LEFT JOIN (
											SELECT idnum, CustomerId, MsgVersion, RTRIM(STUFF(
											    (SELECT ABD.MsgAddress+' '
											    FROM EDI_AddressbookDetail as ABD 
											    JOIN EDI_Addressbook as AB
											    ON AB.idnum = ABD.EDIABidnum
											    WHERE AB.idnum = @EDIABidnum			--<<========
											    ORDER BY ABD.SendType DESC
											    for XML PATH(''), type
												).value('.', 'nvarchar(max)')
												, 1, 0,'')) as MsgAddress
												FROM EDI_Addressbook WHERE idnum = @EDIABidnum		--<<========
												GROUP BY idnum, CustomerId, MsgVersion
												) as newEDIAB
												on newEDIAB.CustomerId = A.CustomerId
										WHERE A.iid = {0}
							END
						ELSE
							BEGIN
								INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
								SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
									  ,FlightSeq ,FlightNo ,msgAddress ,msgVersion ,A.msgType

									  -- ADDed for RDS/RDT 2015-08-12
									  ,CASE WHEN subMsgType = 'RDS' THEN 'RCS' 
											WHEN subMsgType = 'RDT' THEN 'RCT'
											ELSE subMsgType END

									  ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
								FROM EDI_Msg_Queue A
								LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(Acode) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
												FROM exp_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
								LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination
										FROM EDI_Address GROUP BY Carrier, MsgType, Dest) C 
										ON A.msgType = C.msgType AND A.Carrier = C.Carrier 
                                 
										--AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
										--AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
										--AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Dest else C.Destination end )

										--2015-08-25
										AND ISNULL(C.Destination, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND Dest = b.Dest AND Active = '1') THEN b.Dest
																              WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND (Dest = '' or Dest is NULL) AND Active = '1') THEN ''
																              ELSE 'NON' END)
								WHERE A.iid = {0}
							END
                        ";

                // added 2015-08-12 FWB Sent and update time on exp_master table.
                if (msgType.ToUpper() == "FWB")
                {
                    strSql += @" 
                            --UPDATE Exp_Master :: Send FWB DATE & BY 2015-08-12
                            UPDATE expM 
                            SET expM.SendFWBDate	= b.CreatedDate,
	                            expM.SendFWBBy		= b.CreatedBy

                            FROM Exp_Master as expM
                            INNER JOIN Edi_Msg_Queue as b
                            ON expM.MID = b.MID
                            WHERE b.iid = {0}
                            ";
                }

                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));
            }
            //FUS EXP..  2015-10-08
            else if (msgType.ToUpper() == "FSU" && (subType.ToUpper() == "RCS" || subType.ToUpper() == "RDS" || subType.ToUpper() == "DEP" || subType.ToUpper() == "MAN" || subType.ToUpper() == "RCT" || subType.ToUpper() == "RDT" || subType.ToUpper() == "FOH"))
            {
                strSql = @" INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
	                              ,FlightSeq ,FlightNo ,msgAddress ,msgVersion ,A.msgType

                                  -- ADDed for RDS/RDT 2015-08-12
								  ,CASE WHEN A.subMsgType = 'RDS' THEN 'RCS' 
										WHEN A.subMsgType = 'RDT' THEN 'RCT'
										ELSE A.subMsgType END

                                  ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
                            FROM EDI_Msg_Queue A
                            LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(Acode) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
                                            FROM exp_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination, SubMsgType
                                    FROM EDI_Address GROUP BY Carrier, MsgType, Dest, SubMsgType) C 
                                    ON A.msgType = C.msgType AND A.Carrier = C.Carrier 
                                 
                                    --AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
                                    --AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
                                    --AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Dest else C.Destination end )

                                    --2015-08-25
                                    AND ISNULL(C.Destination, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND Dest = b.Dest AND MsgType = A.msgType AND (SubMsgType LIKE '%{2}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN b.Dest
                                                                          WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND (Dest = '' or Dest is NULL) AND MsgType = A.msgType AND (SubMsgType LIKE '%{2}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN ''
															              ELSE 'NON' END)

                                    AND (C.subMsgType LIKE '%{2}%' or C.SubMsgType is NULL or C.SubMsgType = '')

                            WHERE A.iid = {0}
                            ";
                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""), subType.ToUpper());
            }
            //FUS IMP..  2015-10-08
            else if (msgType.ToUpper() == "FSU" && (subType.ToUpper() == "DLV" || subType.ToUpper() == "RCF" || subType.ToUpper() == "NFD" || subType.ToUpper() == "AWD" || subType.ToUpper() == "ARR" || subType.ToUpper() == "TFD"))
            {
                strSql = @" INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
	                              ,FlightSeq ,FlightNo ,msgAddress ,msgVersion ,A.msgType
	                              ,A.subMsgType ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
                            FROM EDI_Msg_Queue A
                            LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(FinalDest) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
                                            FROM ePic_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination, SubMsgType
                                    FROM EDI_Address GROUP BY Carrier, MsgType, Dest, SubMsgType) C 
                                    ON A.msgType = C.msgType AND A.Carrier = C.Carrier 
                                    
                                    --AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
                                    --AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
                                    --AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Dest else C.Destination end )

                                    --2015-08-25 EDI_Address Dest Filter
                                    AND ISNULL(C.Destination, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND DEST = B.Dest AND MsgType = A.msgType AND (SubMsgType LIKE '%{2}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN B.Dest
                                                                          WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND (Dest = '' or Dest is NULL) AND MsgType = A.msgType AND (SubMsgType LIKE '%{2}%' or SubMsgType is NULL or SubMsgType = '') AND Active = '1') THEN ''
															              ELSE 'NON' END)
                                    AND (C.subMsgType LIKE '%{2}%' or C.SubMsgType is NULL or C.SubMsgType = '')

                            WHERE A.iid = {0}
                        ";
                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""), subType.ToUpper());
            }
            else if (msgType.ToUpper() == "FFM")
            {
                strSql = @" INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
	                              ,A.FlightSeq ,FlightNo ,msgAddress ,msgVersion ,A.msgType
	                              ,subMsgType ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
                            FROM EDI_Msg_Queue A

                            --LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(Acode) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs FROM exp_FlightMaster GROUP BY MID) as B ON A.MID = B.MID

							LEFT JOIN(SELECT FlightSeq, MAX(Origincd) as Origin FROM exp_Flightmaster group by flightseq) as B on A.flightseq = b.FlightSeq

                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination
                                    FROM EDI_Address WHERE Active = 1 GROUP BY Carrier, MsgType, Dest) C 
                                    ON A.msgType = C.msgType AND A.Carrier = C.Carrier 

                                    --AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
                                    --AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
                                    --AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Origin else C.Destination end )

                                    AND ISNULL(C.Destination, '') = (	CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND Dest = B.Origin AND A.msgType = Msgtype AND Active = '1') THEN B.Origin
																             WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND (Dest = '' or Dest is NULL) AND A.MsgType = MsgType AND Active = '1') THEN ''
																             ELSE 'NON' END)
                            WHERE A.iid = {0}
                        ";
                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));

            }
            else if (msgType.ToUpper() == "NFM")
            {
                strSql = @" INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
	                              ,FlightSeq ,FlightNo ,eNFM.MsgAddress, eNFM.MsgVersion ,A.msgType
	                              ,subMsgType ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
                            FROM EDI_Msg_Queue A
                            LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(Acode) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
                                            FROM exp_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination
                                    FROM EDI_Address WHERE Active = 1 GROUP BY Carrier, MsgType, Dest) C 
                                    ON A.msgType = C.msgType AND A.Carrier = C.Carrier 
                                    --AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
                                    --AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
                                    AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Dest else C.Destination end )

							join (select MsgAddress, Exp_Flightseq, MsgVersion  from Exp_NFM group by Msgaddress, Exp_FlightSeq, MsgVersion) as eNFM
							on A.flightseq = eNFM.exp_Flightseq

                            WHERE A.iid = {0}
                        ";
                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));
            }
            else if (msgType.ToUpper() == "UWS")
            {
                strSql = @" 
                            INSERT INTO EDI_Msg
                                (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT A.iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
	                                ,A.FlightSeq ,FlightNo ,eUWS.MsgAddress, '' as MsgVersion ,A.msgType
	                                ,subMsgType ,'{1} ', A.createdDate ,A.CreatedBy, ResendYN
                            FROM EDI_Msg_Queue A
                            JOIN (SELECT MsgAddress, FlightSeq from Exp_UWS group by Msgaddress, FlightSeq) as eUWS
                            ON A.flightseq = eUWS.FlightSeq
                            
                            WHERE A.iid = {0}
                        ";
                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));
            }
            else if (msgType.ToUpper() == "FHL")
            {

                strSql = @"  
                        DECLARE @isNewAB		INT
						DECLARE @CustomerId		VARCHAR(20)
						DECLARE @Origin			VARCHAR(3)
						DECLARE @Dest			VARCHAR(3)
						DECLARE @EDIABidnum		INT
						DECLARE @flightSeq		INT
                        DECLARE @MID            INT
                        DECLARE @Carrier        VARCHAR(2)
                        
                        SET @MID = (SELECT MID FROM EDI_MSG_QUEUE WHERE iid = {0})                                                  -- queueId
						SET @isNewAB = (SELECT EDIAddressBook FROM EDI_MSG_QUEUE WHERE iid = {0})                                   -- queueId
						SET @flightSeq = (SELECT flightSeq FROM EDI_MSG_QUEUE WHERE iid = {0})                                      -- queueId
                        SET @Carrier = (SELECT Carrier FROM EDI_MSG_QUEUE WHERE iid = {0})                                          -- queueId
						SET @CustomerId = (SELECT CustomerId FROM EDI_MSG_QUEUE WHERE iid = {0})			                        -- for EDI_AddressBook idnum
						SET @Origin = (SELECT TOP 1 OriginCd  FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = @MID)	-- for EDI_AddressBook idnum
						SET @Dest = (SELECT TOP 1 DestCd FROM Exp_FlightMaster WHERE Flightseq = @flightSeq and MID = @MID)		    -- for EDI_AddressBook idnum
						SET @EDIABidnum = ( SELECT idnum FROM EDI_Addressbook 
                                        WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL'
                                        AND Origin = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Origin = @Origin) THEN @Origin
                                                            WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Origin = 'ALL') THEN 'ALL'
                                                            ELSE '' END )

                                        AND Dest = ( CASE WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Dest = @Dest) THEN @Dest
                                                          WHEN EXISTS(SELECT idnum FROM EDI_Addressbook WHERE CustomerId = @CustomerId AND Carrier = @Carrier AND MsgType = 'FHL' AND Dest = 'ALL') THEN 'ALL'
                                                          ELSE '' END )
                                      ) --msgType
						
						IF(@isNewAB = 1)
							BEGIN
								INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)

								SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
									  ,FlightSeq ,FlightNo ,msgAddress ,msgVersion ,A.msgType
									  ,subMsgType ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
								FROM EDI_Msg_Queue A
								LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(FinalDest) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
												FROM ePic_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
								
								LEFT JOIN (
											SELECT idnum, CustomerId, MsgVersion, RTRIM(STUFF(
											    (SELECT ABD.MsgAddress+' '
											    FROM EDI_AddressbookDetail as ABD 
											    JOIN EDI_Addressbook as AB
											    ON AB.idnum = ABD.EDIABidnum
											    WHERE AB.idnum = @EDIABidnum			--<<========
											    ORDER BY ABD.SendType DESC
											    for XML PATH(''), type
												).value('.', 'nvarchar(max)')
												, 1, 0,'')) as MsgAddress
												FROM EDI_Addressbook WHERE idnum = @EDIABidnum		--<<========
												GROUP BY idnum, CustomerId, MsgVersion
												) as newEDIAB
												on newEDIAB.CustomerId = A.CustomerId
									WHERE A.iid = {0}
							END
						ELSE
							BEGIN
								INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, A.FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)

								SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
									  ,FlightSeq , A.FlightNo ,msgAddress ,msgVersion ,A.msgType
									  ,subMsgType ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
								FROM EDI_Msg_Queue A
                                JOIN Exp_house as B
                                on A.HID = B.HID

								--LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(FinalDest) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
								--				FROM ePic_FlightMaster GROUP BY MID) as B ON A.MID = B.MID

                                JOIN Exp_FlightSeq as expFseq
							    ON A.FlightSeq = expFseq.FlightId

								LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination
										FROM EDI_Address GROUP BY Carrier, MsgType, Dest) C 
										ON A.msgType = C.msgType AND A.Carrier = C.Carrier 
                                    
										--AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
										--AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
										--AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Dest else C.Destination end )

										--2015-08-25 EDI_Address Dest Filter
										AND ISNULL(C.Destination, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND DEST = expFseq.Origin AND Active = '1') THEN expFseq.Origin
																              WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND (Dest = '' or Dest is NULL) AND Active = '1') THEN ''
																              ELSE 'NON' END)
								WHERE A.iid = {0}
							END
                        ";



                strSql += @"  
                            --UPDATE Exp_house :: Send FHL DATE & BY 2015-08-12
                            UPDATE expH 
                            SET expH.SendFHLDate	= b.CreatedDate,
	                            expH.SendFHLBy		= b.CreatedBy

                            FROM Exp_House as expH
                            INNER JOIN Edi_Msg_Queue as b
                            ON expH.HID = b.HID
                            WHERE b.iid = {0}
                        ";

                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));
            }
            else if (msgType.ToUpper() == "FBR")
            {
                strSql += @" 
                            INSERT INTO EDI_Msg
                                           (queueId, Carrier, A.Lcode, A.Ccode, FlightSeq, FlightNo, 
                                            MsgAddress, MsgVersion, MsgType, subMsgType, MsgBody, 
                                            SendDate, SendBy, ResendYN)

                                        SELECT iid, EMQ.Carrier, EMQ.Lcode, EMQ.Ccode, EMQ.FlightSeq, EMQ.FlightNo,
                                                EA.MsgAddress, EA.MsgVersion, EMQ.msgType, EMQ.subMsgType,
                                                '{1}', EMQ.createdDate, EMQ.CreatedBy, EMQ.ResendYN
                                        FROM EDI_Msg_Queue EMQ

                                        JOIN EXP_FlightSeq as EF
                                        ON EMQ.FlightSeq = EF.FlightId
                                        
                                        LEFT JOIN (SELECT MsgType, Carrier, MsgVersion, MsgAddress, Active , Dest
                                                    FROM EDI_Address) EA

                                        ON EA.msgType = (
												CASE WHEN EXISTS(SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND MsgType = 'FBR' AND Active = '1') THEN 'FBR'
												ELSE 'FFM' END
												)


                                        AND EMQ.Carrier = EA.Carrier AND EA.Active = '1'

                                        AND ISNULL(EA.Dest, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND dest = EF.Acode AND MsgType = 'FBR' AND Active = '1') THEN EF.Acode
                                                                       WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND (Dest = '' or Dest is NULL) AND MsgType = 'FBR' AND Active = '1') THEN ''
														               WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND dest = EF.Acode AND MsgType = 'FFM'  AND Active = '1') THEN EF.Acode
                                                                       WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = EMQ.Carrier AND (Dest = '' or Dest is NULL) AND MsgType = 'FFM'  AND Active = '1') THEN ''
                                                                       ELSE 'NON' END)
                                        WHERE EMQ.iid = {0}

                        ";
                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));

            }
            else
            {
                //2015-08-25 Edi_Address Dest Filter
                strSql = @" INSERT INTO EDI_Msg
                               (queueId, Carrier, A.Lcode, A.Ccode, A.MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT iid ,A.Carrier ,A.Lcode ,A.Ccode ,A.MID
	                              ,FlightSeq ,FlightNo ,msgAddress ,msgVersion ,A.msgType
	                              ,subMsgType ,'{1}' ,A.createdDate ,A.CreatedBy, ResendYN
                            FROM EDI_Msg_Queue A
                            LEFT JOIN (SELECT MID, MAX(Partial) Partial, MAX(FinalDest) Dest , sum(OnWeight) as Weight, sum(OnPcs) as Pcs
                                            FROM ePic_FlightMaster GROUP BY MID) as B ON A.MID = B.MID
                            LEFT JOIN (SELECT MsgType, Carrier, MAX(MsgVersion) MsgVersion, MAX(MsgAddress) MsgAddress, Dest as Destination
                                    FROM EDI_Address GROUP BY Carrier, MsgType, Dest) C 
                                    ON A.msgType = C.msgType AND A.Carrier = C.Carrier 
                                    
                                    --AND B.DestinationPortCd = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.DestinationPortCd end 
                                    --AND B.Dest = case when C.Destination <> '' or C.Destination <> null then C.Destination else B.Dest end 
                                    --AND C.Destination = (case when C.Destination <> '' or C.Destination <> null then B.Dest else C.Destination end )

                                    --2015-08-25 EDI_Address Dest Filter
                                    AND ISNULL(C.Destination, '') = (CASE WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND DEST = B.Dest AND Active = '1') THEN B.Dest
                                                                          WHEN EXISTS (SELECT MsgAddress FROM EDI_Address WHERE Carrier = A.Carrier AND MsgType = A.msgType AND (Dest = '' or Dest is NULL) AND Active = '1') THEN ''
															              ELSE 'NON' END)

                            WHERE A.iid = {0}
                        ";

                strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));

            }

            if (!strSql.Equals(string.Empty))
            {
                try { result = ExecCommand(strSql); }
                catch (SqlException e) { result = -1; }
            }

            return result;
        }
        public int InsertLogforFreeSITAmsg(int queueId, string msgBody)
        {
            int result = 0;
            string strSql = "";
            strSql = @" INSERT INTO EDI_Msg
                               (queueId, Carrier, Lcode, Ccode, MID,
	                            FlightSeq, FlightNo, MsgAddress, MsgVersion, MsgType,
	                            subMsgType, MsgBody, SendDate, SendBy, ResendYN)
                            SELECT iid , Carrier, Lcode ,Ccode ,MID,
                                 FlightSeq ,FlightNo , MsgAddress_SITAfreeMSG, '', MsgType,
                                subMsgType, '{1}', createdDate, CreatedBy, ResendYN
                            FROM EDI_Msg_Queue
                            WHERE iid = {0}
                    ";
            strSql = string.Format(strSql, queueId, msgBody.Replace("'", ""));


            if (!strSql.Equals(string.Empty))
            {
                try { result = ExecCommand(strSql); }
                catch (SqlException e) { result = -1; }
            }

            return result;
        }

    }
}
