using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FOHEntity : BaseEntity
    {
        public FOHEntity()
        {

        }

        public FOHEntity(BaseEntity baseEntity, DateTime __fohTime, string __shipper)
        {
            this.queueId = baseEntity.queueId;
            this.msgType = baseEntity.msgType;
            this.subMsgType = baseEntity.subMsgType;
            this.msgDestAddr = baseEntity.msgDestAddr;
            this.msgVersion = baseEntity.msgVersion;
            this.mid = baseEntity.mid;
            this.flightSeq = baseEntity.flightSeq;
            this.Lcode = baseEntity.Lcode;
            this.Ccode = baseEntity.Ccode;
            this.createdDate = baseEntity.createdDate;
            this.createdBy = baseEntity.createdBy;
            this.prefix = baseEntity.prefix;
            this.awb = baseEntity.awb;
            this.origin = baseEntity.origin;
            this.dest = baseEntity.dest;
            this.destFlight = baseEntity.destFlight;
            this.shipmentIndicator = baseEntity.shipmentIndicator;
            this.pcs = baseEntity.pcs;
            this.weight = baseEntity.weight;
            this.shipper = baseEntity.shipper;

            //Derived Class member
            this.fohTime = __fohTime;

            this.forigin = baseEntity.forigin;
        }
        private DateTime _fohTime = Convert.ToDateTime("1/1/0001");
        public DateTime fohTime
        {
            get { return _fohTime; }
            set { _fohTime = value; }
        }
    }
}
