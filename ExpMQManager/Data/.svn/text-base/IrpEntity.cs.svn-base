using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class IrpEntity : BaseEntity
    {
        public IrpEntity()
        {
            //Empty Constructor
        }

        public IrpEntity(BaseEntity baseEntity, int __irpId, string __msgFrom, string __msgSubject, 
            string __msgBody, string __flightNo, DateTime __arrDate)
        {
            this.queueId = baseEntity.queueId;
            this.msgType = baseEntity.msgType;
            this.msgDestAddr = baseEntity.msgDestAddr;

            this.irpId = __irpId;
            this.msgFrom = __msgFrom;
            this.msgBody = __msgBody;
            this.msgSubject = __msgSubject;
            this.flightNo = __flightNo;
            this.arrDate = __arrDate;
        }

        private int _irpId = 0;
        public int irpId
        {
            get { return _irpId; }
            set { _irpId = value; }
        }

        private string _msgFrom = "";
        public string msgFrom
        {
            get { return _msgFrom; }
            set { _msgFrom = value; }
        }

        private string _msgSubject = "";
        public string msgSubject
        {
            get { return _msgSubject; }
            set { _msgSubject = value; }
        }

        private string _msgBody = "";
        public string msgBody
        {
            get { return _msgBody; }
            set { _msgBody = value; }
        }

        private string _flightNo = "";
        public string flightNo
        {
            get { return _flightNo; }
            set { _flightNo = value; }
        }

        private DateTime _arrDate = Convert.ToDateTime("1/1/0001");
        public DateTime arrDate
        {
            get { return _arrDate; }
            set { _arrDate = value; }
        }

    }
}
