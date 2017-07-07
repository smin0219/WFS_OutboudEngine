using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class DlvEntity : BaseEntity
    {
        public DlvEntity()
        {
            //Empty Constructor
        }

        public DlvEntity(BaseEntity baseEntity, DateTime __dlvTime, int __pcsDLV, double __weightDLV, string __cnee) 
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
            this.dlvTime = __dlvTime;
            this.pcsDLV = __pcsDLV;
            this.weightDLV = __weightDLV;
            this.cnee = __cnee;
        }


        private DateTime _dlvTime = Convert.ToDateTime("1/1/0001");
        public DateTime dlvTime 
        {
            get { return _dlvTime; }
            set { _dlvTime = value; }
        }

        private int _pcsDLV = 0;
        public int pcsDLV
        {
            get { return _pcsDLV; }
            set { _pcsDLV = value; }
        }

        private double _weightDLV = 0.00;
        public double weightDLV
        {
            get { return _weightDLV; }
            set { _weightDLV = value; }
        }

        private string _cnee = "";
        public string cnee
        {
            get { return _cnee; }
            set { _cnee = value; }
        }


    }
}
