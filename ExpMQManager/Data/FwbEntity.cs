using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FwbEntity : BaseEntity
    {
        public FwbEntity()
        {
            //Empty Constructor
        }

        public FwbEntity(BaseEntity baseEntity, string __shipperNm, string __shipperAddr, string __shipperAddr2,
            string __shipperCity, string __shipperState, string __shipperCountry, string __shipperZip, string __cneeNm,
            string __cneeAddr, string __cneeCity, string __cneeProv, string __cneeCountry,
            string __cneeZip, string __agentNm, string __agentIATACd, string __agentCASSaddr, 
            string __agentCity, string __currency, string __chargeCd, string __preChargeWeightCd, 
            string __preChargeOtherCd, double __carriageVal, double __customVal, double __insuranceVal, string __awbPlace, string __SHC) //, string[] __CountryCode, string[] __InfoId, string[] __CustomsId, string[] __CustomsInfo)
        {
            this.queueId = baseEntity.queueId;
            this.msgType = baseEntity.msgType;
            this.msgDestAddr = baseEntity.msgDestAddr;
            this.msgVersion = baseEntity.msgVersion;
            this.mid = baseEntity.mid;
            this.createdDate = baseEntity.createdDate;
            this.createdBy = baseEntity.createdBy;
            this.prefix = baseEntity.prefix;
            this.awb = baseEntity.awb;
            this.origin = baseEntity.origin;
            this.dest = baseEntity.dest;
            this.destFlight = baseEntity.destFlight;
            this.pcs = baseEntity.pcs;
            this.weight = baseEntity.weight;
            this.carrier = baseEntity.carrier;

            this.shipperNm = __shipperNm;
            this.shipperAddr = __shipperAddr;
            this.shipperAddr2 = __shipperAddr2;
            this.shipperCity = __shipperCity;
            this.shipperState = __shipperState;
            this.shipperCountry = __shipperCountry;
            this.shipperZip = __shipperZip;
            this.cneeNm = __cneeNm;
            this.cneeAddr = __cneeAddr;
            this.cneeCity = __cneeCity;
            this.cneeProv = __cneeProv;
            this.cneeCountry = __cneeCountry;
            this.cneeZip = __cneeZip;
            this.agentNm = __agentNm;
            this.agentIATACd = __agentIATACd;
            this.agentCASSaddr = __agentCASSaddr;
            this.agentCity = __agentCity;
            this.currency = __currency;
            this.chargeCd = __chargeCd;
            this.preChargeWeightCd = __preChargeWeightCd;
            this.preChargeOtherCd = __preChargeOtherCd;
            this.carriageVal = __carriageVal;
            this.customVal = __customVal;
            this.insuranceVal = __insuranceVal;
            this.awbPlace = __awbPlace;

            this.SHC = __SHC;

            //this._COUNTRYCODE = __CountryCode;
            //this._INFOID = __InfoId;
            //this._CUSTOMSID = __CustomsId;
            //this._CUSTOMSINFO = __CustomsInfo;


        }

        private string _shipperNm = "";
        public string shipperNm
        {
            get { return _shipperNm; }
            set { _shipperNm = value; }
        }

        private string _shipperAddr = "";
        public string shipperAddr
        {
            get { return _shipperAddr; }
            set { _shipperAddr = value; }
        }

        private string _shipperAddr2 = "";
        public string shipperAddr2
        {
            get { return _shipperAddr2; }
            set { _shipperAddr2 = value; }
        }

        private string _shipperCity = "";
        public string shipperCity
        {
            get { return _shipperCity; }
            set { _shipperCity = value; }
        }

        private string _shipperState = "";
        public string shipperState
        {
            get { return _shipperState; }
            set { _shipperState = value; }
        }
        private string _shipperCountry = "";
        public string shipperCountry
        {
            get { return _shipperCountry; }
            set { _shipperCountry = value; }
        }
        private string _shipperZip = "";
        public string shipperZip
        {
            get { return _shipperZip; }
            set { _shipperZip = value; }
        }

        private string _cneeNm = "";
        public string cneeNm
        {
            get { return _cneeNm; }
            set { _cneeNm = value; }
        }

        private string _cneeAddr = "";
        public string cneeAddr
        {
            get { return _cneeAddr; }
            set { _cneeAddr = value; }
        }

        private string _cneeCity = "";
        public string cneeCity
        {
            get { return _cneeCity; }
            set { _cneeCity = value; }
        }

        private string _cneeProv = "";
        public string cneeProv
        {
            get { return _cneeProv; }
            set { _cneeProv = value; }
        }

        private string _cneeCountry = "";
        public string cneeCountry
        {
            get { return _cneeCountry; }
            set { _cneeCountry = value; }
        }

        private string _cneeZip = "";
        public string cneeZip
        {
            get { return _cneeZip; }
            set { _cneeZip = value; }
        }

        private string _agentNm = "";
        public string agentNm
        {
            get { return _agentNm; }
            set { _agentNm = value; }
        }

        private string _agentIATACd = "";
        public string agentIATACd
        {
            get { return _agentIATACd; }
            set { _agentIATACd = value; }
        }

        private string _agentCASSaddr = "";
        public string agentCASSaddr
        {
            get { return _agentCASSaddr; }
            set { _agentCASSaddr = value; }
        }

        private string _agentCity = "";
        public string agentCity
        {
            get { return _agentCity; }
            set { _agentCity = value; }
        }

        private string _currency = "";
        public string currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        private string _chargeCd = "";
        public string chargeCd
        {
            get { return _chargeCd; }
            set { _chargeCd = value; }
        }

        private string _preChargeWeightCd = "";
        public string preChargeWeightCd
        {
            get { return _preChargeWeightCd; }
            set { _preChargeWeightCd = value; }
        }

        private string _preChargeOtherCd = "";
        public string preChargeOtherCd
        {
            get { return _preChargeOtherCd; }
            set { _preChargeOtherCd = value; }
        }

        private double _carriageVal = 0.00;
        public double carriageVal
        {
            get { return _carriageVal; }
            set { _carriageVal = value; }
        }

        private double _customVal = 0.00;
        public double customVal
        {
            get {   return _customVal; }
            set { _customVal = value; }
        }

        private double _insuranceVal = 0.00;
        public double insuranceVal
        {
            get { return _insuranceVal; }
            set { _insuranceVal = value; }
        }

        private List<FwbRateEntity> _colRTD = default(List<FwbRateEntity>);
        public List<FwbRateEntity> colRTD
        {
            get
            {
                if (_colRTD == default(List<FwbRateEntity>))
                    _colRTD = new List<FwbRateEntity>();

                return _colRTD;
            }
        }

        // 2015-09-29 new _RTD
        private List<Fwb_newDB_RTDEntity> _colnewDBRTD = default(List<Fwb_newDB_RTDEntity>);
        public List<Fwb_newDB_RTDEntity> colnewDBRTD
        {
            get
            {
                if (_colnewDBRTD == default(List<Fwb_newDB_RTDEntity>))
                    _colnewDBRTD = new List<Fwb_newDB_RTDEntity>();

                return _colnewDBRTD;
            }
        }

        // 2015-09-29 new _RTD_NG||NC
        private List<FwbNGEntity> _colNG = default(List<FwbNGEntity>);
        public List<FwbNGEntity> colNG
        {
            get
            {
                if (_colNG == default(List<FwbNGEntity>))
                    _colNG = new List<FwbNGEntity>();

                return _colNG;
            }
        }

        private List<FwbVolumeEntity> _colVol = default(List<FwbVolumeEntity>);
        public List<FwbVolumeEntity> colVol
        {
            get
            {
                if (_colVol == default(List<FwbVolumeEntity>))
                    _colVol = new List<FwbVolumeEntity>();

                return _colVol;
            }
        }


        private List<FwbOCIEntity> _colOCI = default(List<FwbOCIEntity>);
        public List<FwbOCIEntity> colOCI
        {
            get
            {
                if (_colOCI == default(List<FwbOCIEntity>))
                    _colOCI = new List<FwbOCIEntity>();

                return _colOCI;
            }
        }

        // 2015-09-29 new _OCI
        private List<Fwb_newDB_OCIEntity> _colnewDBOCI = default(List<Fwb_newDB_OCIEntity>);
        public List<Fwb_newDB_OCIEntity> colnewDBOCI
        {
            get
            {
                if (_colnewDBOCI == default(List<Fwb_newDB_OCIEntity>))
                    _colnewDBOCI = new List<Fwb_newDB_OCIEntity>();

                return _colnewDBOCI;
            }
        }


        private List<FwbOtherChargeEntity> _colCharge = default(List<FwbOtherChargeEntity>);
        public List<FwbOtherChargeEntity> colCharge
        {
            get
            {
                if (_colCharge == default(List<FwbOtherChargeEntity>))
                    _colCharge = new List<FwbOtherChargeEntity>();

                return _colCharge;
            }
        }

        // 2015-09-29 new _RTD_NG||NC
        private List<Fwb_newDB_OTHEntity> _colnewDBOTH = default(List<Fwb_newDB_OTHEntity>);
        public List<Fwb_newDB_OTHEntity> colnewDBOTH
        {
            get
            {
                if (_colnewDBOTH == default(List<Fwb_newDB_OTHEntity>))
                    _colnewDBOTH = new List<Fwb_newDB_OTHEntity>();

                return _colnewDBOTH;
            }
        }

        private string _awbPlace = "";
        public string awbPlace
        {
            get { return _awbPlace; }
            set { _awbPlace = value; }
        }

        private string _SHC = "";
        public string SHC
        {
            get { return _SHC; }
            set { _SHC = value; }
        }


        //2015-10-06 new _SSR
        private List<Fwb_newDB_SSREntity> _colnewDBSSR = default(List<Fwb_newDB_SSREntity>);
        public List<Fwb_newDB_SSREntity> colnewDBSSR
        {
            get
            {
                if(_colnewDBSSR == default(List<Fwb_newDB_SSREntity>))
                    _colnewDBSSR = new List<Fwb_newDB_SSREntity>();

                return _colnewDBSSR;
            }
        }

        //2015-10-06 new PPD
        private Fwb_newDB_PPDCOLEntity _colnewDBPPDCOL = default(Fwb_newDB_PPDCOLEntity);
        public Fwb_newDB_PPDCOLEntity colnewDBPPDCOL
        {
            get
            {
                if(_colnewDBPPDCOL == default(Fwb_newDB_PPDCOLEntity))
                    _colnewDBPPDCOL = new Fwb_newDB_PPDCOLEntity();

                return _colnewDBPPDCOL;
            }
        }

        //2105-10-21 added SHC
        private List<Fwb_newDB_SHCEntity> _colnewDBSHC = default(List<Fwb_newDB_SHCEntity>);
        public List<Fwb_newDB_SHCEntity> colnewSHC
        {
            get
            {
                if (_colnewDBSHC == default(List<Fwb_newDB_SHCEntity>))
                    _colnewDBSHC = new List<Fwb_newDB_SHCEntity>();

                return _colnewDBSHC;
            }
        }


        // 
        //private string[] _countrycode = new string[9];
        //private string[] _COUNTRYCODE
        //{
        //    get { return _countrycode; }
        //    set { _countrycode = value; }
        //}

        //private string[] _InfoId = new string[9];
        //private string[] _INFOID
        //{
        //    get { return _InfoId; }
        //    set { _InfoId = value; }
        //}

        //private string[] _CustomsId = new string[9];
        //private string[] _CUSTOMSID
        //{
        //    get { return _CustomsId; }
        //    set { _CustomsId = value; }
        //}

        //private string[] _CustomsInfo = new string[9];
        //private string[] _CUSTOMSINFO
        //{
        //    get { return _CustomsInfo; }
        //    set { _CustomsInfo = value; }
        //}


    }
}
