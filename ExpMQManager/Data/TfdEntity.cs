using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class TfdEntity : BaseEntity
    {
        public TfdEntity()
        {
            //Empty Constructor
        }

        public TfdEntity(BaseEntity baseEntity, string __carrier, string __tday, string __tmonth, string __ttime, string __OrgPort, int __pcsTFD, double __weightTFD, DateTime __whcftime) 
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
            this.carrier = __carrier;
            this.tday = __tday;
            this.tmonth = __tmonth;
            this.ttime = __ttime;
            this.OrgPort = __OrgPort;
            this.pcsTFD = __pcsTFD;
            this.weightTFD = __weightTFD;
            this._whcftime = __whcftime;
        }


        private int _pcsTFD = 0;
        public int pcsTFD
        {
            get { return _pcsTFD; }
            set { _pcsTFD = value; }
        }

        private double _weightTFD = 0.00;
        public double weightTFD
        {
            get { return _weightTFD; }
            set { _weightTFD = value; }
        }

        private string _carrier = "";
        public string carrier
        {
            get { return _carrier; }
            set { _carrier = value; }
        }
        private string _tday = "";
        public string tday
        {
            get { return _tday; }
            set { _tday = value; }
        }
        private string _tmonth = "";
        public string tmonth
        {
            get { return _tmonth; }
            set { _tmonth = value; }
        }
        private string _ttime = "";
        public string ttime
        {
            get { return _ttime; }
            set { _ttime = value; }
        }
        private string _OrgPort = "";
        public string OrgPort
        {
            get { return _OrgPort; }
            set { _OrgPort = value; }
        }

        private DateTime _whcftime = Convert.ToDateTime("1/1/0001");
        public DateTime whcftime
        {
            get { return _whcftime; }
            set { _whcftime = value; }
        }

    }
}
