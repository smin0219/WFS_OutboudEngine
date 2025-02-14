﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpMQManager.DAL;
using ExpMQManager.Data;


namespace ExpMQManager.BLL
{
    public class GenerateIRP : GenerateBase
    {
        public override string doBuildUp(string msgType, string subType, int mid, int flightSeq, int queueId)
        {
            IrpEntity irpEntity = new IrpDAC().GetIRPInfoDAC(mid, flightSeq, msgType, subType, queueId);
            return buildUpIRP(irpEntity, msgType, subType);
        }

        public string buildUpIRP(IrpEntity msgEntity, string msgType, string subType)
        {
            string strAWB = "";

            strAWB = base.buildUpBase(msgEntity, msgType, subType);

            //strAWB += "SUBJ: " + msgEntity.msgSubject + "\r\n";
            //strAWB += "FROM: " + msgEntity.msgFrom + "\r\n";
            //strAWB += "TO: " + msgEntity.msgDestAddr + "\r\n";

            strAWB += "Flight Number: " + msgEntity.flightNo + "\r\n";
            strAWB += "Arrival Date: " + msgEntity.arrDate.ToShortDateString() + "\r\n";

            strAWB += "     \r\n";

            strAWB += msgEntity.msgBody.ToUpper() + "\r\n";
            strAWB += "\r\n";

            return strAWB;
        }

    }
}
