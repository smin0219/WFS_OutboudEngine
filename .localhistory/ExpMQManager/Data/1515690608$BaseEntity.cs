using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class BaseEntity : BaseDB
    {
        public BaseEntity()
        {
            //Empty Constructor
        }

        //Constructor for FSU Messages
        public BaseEntity(int __queueId, string __msgType, string __subMsgType, string __msgDestAddr, int __msgVersion, 
            int __mid, int __refID, int __flightSeq, string __Lcode, string __Ccode, DateTime __createdDate, string __createdBy,
            string __prefix, string __awb, string __fOrigin, string __origin, string __dest, string __destFlight, string __shipmentIndicator, 
            int __pcs, double __weight, string __shipper)
        {
            this.queueId = __queueId;
            this.msgType = __msgType;
            this.subMsgType = __subMsgType;
            this.msgDestAddr = __msgDestAddr;
            this.msgVersion = __msgVersion;
            this.mid = __mid;
            this.refID = __refID;
            this.flightSeq = __flightSeq;
            this.Lcode = __Lcode;
            this.Ccode = __Ccode;
            this.createdDate = __createdDate;
            this.createdBy = __createdBy;
            this.prefix = __prefix;
            this.awb = __awb;
            this.origin = __origin;
            this.dest = __dest;
            this.destFlight = __destFlight;
            this.shipmentIndicator = __shipmentIndicator;
            this.pcs = __pcs;
            this.weight = __weight;
            this.forigin = __fOrigin;
            this.shipper = __shipper;            
         
            
        }
        // NFM ADDED BY NA 
        // added 2016-08-17  Use it for UWS
        public BaseEntity(int __queueId, string __msgType, int __flightSeq, string __Lcode, string __Ccode,
            string __msgDestAddr, int __msgVersion, string __flightno, DateTime __flightdate,
            string __origin, string __dest
            )
        {
            this.queueId = __queueId;
            this.msgType = __msgType;
            this.flightSeq = __flightSeq;
            this.Lcode = __Lcode;
            this.Ccode = __Ccode;
            this.msgDestAddr = __msgDestAddr;
            this.msgVersion = __msgVersion;
            this.flightNo = __flightno;
            this.flightDate = __flightdate;
            this.origin = __origin;
            this.dest = __dest;
        }
        //////////////////////////////////

        //Constructor for FFM Message
        public BaseEntity(int __queueId, string __msgType, string __msgDestAddr, int __msgVersion,
            int __flightSeq, DateTime __flightDate, string __origin, string __Lcode, string __Ccode,
            string __flightNo)
        {
            this.queueId = __queueId;
            this.msgType = __msgType;
            this.msgDestAddr = __msgDestAddr;
            this.msgVersion = __msgVersion;
            this.flightSeq = __flightSeq;
            this.flightDate = __flightDate;
            this.origin = __origin;
            this.Lcode = __Lcode;
            this.Ccode = __Ccode;
            this.flightNo = __flightNo;
        }

        //Constructor for IRP Message
        public BaseEntity(int __queueId, string __msgType, string __msgDestAddr, string __IRPSubject)
        {
            this.queueId = __queueId;
            this.msgType = __msgType;
            this.msgDestAddr = __msgDestAddr;
            this.IRPSubject = __IRPSubject;
        }

        //Constructor for FWB Message
        public BaseEntity(int __queueId, string __Ccode, string __msgType, string __msgDestAddr, int __msgVersion,
            int __mid, DateTime __createdDate, string __createdBy, string __prefix, string __awb,
            string __origin, string __dest, string __destFlight, int __pcs, double __weight, string __carrier) 
        {
            this.queueId = __queueId;
            this.Ccode = __Ccode;
            this.msgType = __msgType;
            this.msgDestAddr = __msgDestAddr;
            this.msgVersion = __msgVersion;
            this.mid = __mid;
            this.createdDate = __createdDate;
            this.createdBy = __createdBy;
            this.prefix = __prefix;
            this.awb = __awb;
            this.origin = __origin;
            this.dest = __dest;
            this.destFlight = __destFlight;
            this.pcs = __pcs;
            this.weight = __weight;
            this.carrier = __carrier;
            //this._COUNTRYCODE = __CountryCode;
            //this._INFOID = __InfoId;
            //this._CUSTOMSID = __CustomsId;
            //this._CUSTOMSINFO = __CustomsInfo;
        }

        private int _queueId = 0;
        public int queueId
        {
            get { return _queueId; }
            set { _queueId = value; }
        }

        private string _msgType = "";
        public string msgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        private string _subMsgType = "";
        public string subMsgType
        {
            get { return _subMsgType; }
            set { _subMsgType = value; }
        }

        private string _msgDestAddr = "";
        public string msgDestAddr
        {
            get { return _msgDestAddr; }
            set { _msgDestAddr = value; }
        }

        private static string _msgDestAddrEmail = "";
        public string msgDestAddrEmail
        {
            get { return _msgDestAddrEmail; }
            set { _msgDestAddrEmail = value; }
        }

        private int _msgVersion = 0;
        public int msgVersion
        {
            get { return _msgVersion; }
            set { _msgVersion = value; }
        }

        private int _mid = 0;
        public int mid
        {
            get { return _mid; }
            set { _mid = value; }
        }

        private int _flightSeq = 0;
        public int flightSeq
        {
            get { return _flightSeq; }
            set { _flightSeq = value; }
        }

        private string _Lcode = "";
        public string Lcode
        {
            get { return _Lcode; }
            set { _Lcode = value; }
        }

        private string _Ccode = "";
        public string Ccode
        {
            get { return _Ccode; }
            set { _Ccode = value; }
        }

        private DateTime _createdDate = Convert.ToDateTime("1/1/0001");
        public DateTime createdDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }

        private string _createdBy = "";
        public string createdBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        private string _prefix = "";
        public string prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        private string _awb = "";
        public string awb
        {
            get { return _awb; }
            set { _awb = value; }
        }

        private string _forigin = "";
        public string forigin
        {
            get { return _forigin; }
            set { _forigin = value; }
        }

        private string _origin = "";
        public string origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        private string _dest = "";
        public string dest
        {
            get { return _dest; }
            set { _dest = value; }
        }

        private string _destFlight = "";
        public string destFlight
        {
            get { return _destFlight; }
            set { _destFlight = value; }
        }

        private string _shipmentIndicator = "";
        public string shipmentIndicator
        {
            get
            {
                if (_shipmentIndicator == "") return "T";
                else return _shipmentIndicator; 
            }
            set { _shipmentIndicator = value; }
        }

        private int _pcs = 0;
        public int pcs
        {
            get { return _pcs; }
            set { _pcs = value; }
        }

        private double _weight = 0.00;
        public double weight
        {
            get { return _weight; }
            set { _weight = value; }
        }


        /* FFM Entity */
        private string _flightNo = "";
        public string flightNo
        {
            get { return _flightNo; }
            set { _flightNo = value; }
        }

        private DateTime _flightDate = Convert.ToDateTime("1/1/0001");
        public DateTime flightDate
        {
            get { return _flightDate; }
            set { _flightDate = value; }
        }

        /* FWB Entity */
        private string _carrier = "";
        public string carrier
        {
            get { return _carrier; }
            set { _carrier = value; }
        }

        private string _shipper = "";
        public string shipper
        {
            get { return _shipper; }    
            set { _shipper = value; }
        }

        private static string _IRPSubject = "";
        public string IRPSubject
        {
            get { return _IRPSubject; }
            set { _IRPSubject = value; }
        }

    }
}
