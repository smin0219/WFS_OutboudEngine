using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class DisEntity : BaseEntity
    {
        public DisEntity()
        {
            //Empty Constructor
        }
        public DisEntity(BaseEntity baseEntity, string __flightNo, int __pcsRCF,
            double __weightRCF, string __shipmentIndicatorRCF, DateTime __arrCargoDate, DateTime __arrFlightMasterDate, string __disType, string __disOsi)
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

            //
            this.flightNo = __flightNo;
            this.pcsRCF = __pcsRCF;
            this.weightRCF = __weightRCF;
            this.shipmentIndicatorRCF = __shipmentIndicatorRCF;
            this.arrCargoDate = __arrCargoDate;
            this.arrFlightMasterDate = __arrFlightMasterDate;
            this.disType = __disType;
            this.disOsi = __disOsi;
        }

        private int _manPcs = 0;
        public int manPcs
        {
            get { return _manPcs; }
            set { _manPcs = value; }
        }
        private int _locPcs = 0;
        public int locPcs
        {
            get { return _locPcs; }
            set { _locPcs = value; }
        }

        private int _pcsRCF = 0;
        public int pcsRCF
        {
            get { return _pcsRCF; }
            set { _pcsRCF = value; }
        }

        private double _weightRCF = 0.00;
        public double weightRCF
        {
            get { return _weightRCF; }
            set { _weightRCF = value; }
        }

        private string _shipmentIndicatorRCF = "";
        public string shipmentIndicatorRCF
        {
            get
            {
                if (_shipmentIndicatorRCF == "") return "T";
                else return _shipmentIndicatorRCF;
            }
            set { _shipmentIndicatorRCF = value; }
        }
        private DateTime _arrCargoDate = Convert.ToDateTime("1/1/0001");
        public DateTime arrCargoDate
        {
            get { return _arrCargoDate; }
            set { _arrCargoDate = value; }
        }
        private DateTime _arrFlightMasterDate = Convert.ToDateTime("1/1/0001");
        public DateTime arrFlightMasterDate
        {
            get { return _arrFlightMasterDate; }
            set { _arrFlightMasterDate = value; }
        }
        private string _disType = "";
        public string disType
        {
            get { return _disType; }
            set { _disType = value; }
        }
        private string _disOsi = "";
        public string disOsi
        {
            get { return _disOsi; }
            set { _disOsi = value; }
        }
    }
}
