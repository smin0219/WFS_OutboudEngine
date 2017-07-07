using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class AwdEntity : BaseEntity
    {
        public AwdEntity()
        {
            //Empty Constructor
        }

        public AwdEntity(BaseEntity baseEntity, int __pcsAWD, double __weightAWD,
            string __shipmentIndicatorAWD, DateTime __docAvailDate, string __cnee)
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
            this.pcsAWD = __pcsAWD;
            this.weightAWD = __weightAWD;
            this.shipmentIndicatorAWD = __shipmentIndicatorAWD;
            this.docAvailDate = __docAvailDate;
            this.cnee = __cnee;
        }

        private int _pcsAWD = 0;
        public int pcsAWD
        {
            get { return _pcsAWD; }
            set { _pcsAWD = value; }
        }

        private double _weightAWD = 0.00;
        public double weightAWD
        {
            get { return _weightAWD; }
            set { _weightAWD = value; }
        }

        private string _shipmentIndicatorAWD = "";
        public string shipmentIndicatorAWD
        {
            get
            {
                if (_shipmentIndicatorAWD == "") return "T";
                else return _shipmentIndicatorAWD;
            }
            set { _shipmentIndicatorAWD = value; }
        }

        private DateTime _docAvailDate = Convert.ToDateTime("1/1/0001");
        public DateTime docAvailDate
        {
            get { return _docAvailDate; }
            set { _docAvailDate = value; }
        }

        private string _cnee = "";
        public string cnee
        {
            get { return _cnee; }
            set { _cnee = value; }
        }

    }
}
