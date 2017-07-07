using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class RctEntity : BaseEntity
    {
        public RctEntity()
        {
            //Empty Constructor
        }

        public RctEntity(BaseEntity baseEntity, DateTime __rcsTime, string __cnee)
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

            //Derived Class member
            this.rcsTime = __rcsTime;
            this.cnee = __cnee;

            this.forigin = baseEntity.forigin;
        }

        private DateTime _rcsTime = Convert.ToDateTime("1/1/0001");
        public DateTime rcsTime
        {
            get { return _rcsTime; }
            set { _rcsTime = value; }
        }

        private string _cnee = "";
        public string cnee
        {
            get { return _cnee; }
            set { _cnee = value; }
        }


    }
}
