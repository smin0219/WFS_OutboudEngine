using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FhlEntity : BaseEntity
    {
        public FhlEntity()
        {
            //Empty Constructor
        }

        public FhlEntity(BaseEntity baseEntity, string __HAWB, int __HPcs, int __SLAC, decimal __HWeight, string __HDest, string __HOrigin, string __Partial, string __Commodity,
            string __shipperNm, string __shipperAddr, string __shipperAddr2,
            string __shipperCity, string __shipperState, string __shipperCountry, string __shipperZip, string __shipperAddrTel, string __shipperContact, string __cneeNm,
            string __cneeAddr, string __cneeCity, string __cneeProv, string __cneeCountry,
            string __cneeZip, string __cneeAddrTel, string __cneeContact, 
            //string __agentNm, string __agentIATACd, string __agentCASSaddr, string __agentCity,
            string __currency, string __chargeCd, string __preChargeWeightCd, string __preChargeOtherCd, double __carriageVal, double __customVal, double __insuranceVal, string __SHC
            // string __awbPlace
            )
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

            this.HAWB = __HAWB;
            this.HPcs = __HPcs;
            this.SLAC = __SLAC;
            this.HWeight = __HWeight;
            this.HDest = __HDest;
            this.HOrigin = __HOrigin;
            this.Partial = __Partial;
            this.Commodity = __Commodity;

            this.shipperNm = __shipperNm;
            this.shipperAddr = __shipperAddr;
            this.shipperAddr2 = __shipperAddr2;
            this.shipperCity = __shipperCity;
            this.shipperState = __shipperState;
            this.shipperCountry = __shipperCountry;
            this.shipperZip = __shipperZip;
            this.shipperAddrTel = __shipperAddrTel;
            this.shipperContact = __shipperContact;
            this.cneeNm = __cneeNm;
            this.cneeAddr = __cneeAddr;
            this.cneeCity = __cneeCity;
            this.cneeProv = __cneeProv;
            this.cneeCountry = __cneeCountry;
            this.cneeZip = __cneeZip;
            this.cneeAddrTel = __cneeAddrTel;
            this.cneeContact = __cneeContact;
            //this.agentNm = __agentNm;
            //this.agentIATACd = __agentIATACd;
            //this.agentCASSaddr = __agentCASSaddr;
            //this.agentCity = __agentCity;
            this.currency = __currency;
            this.chargeCd = __chargeCd;
            this.preChargeWeightCd = __preChargeWeightCd;
            this.preChargeOtherCd = __preChargeOtherCd;
            this.carriageVal = __carriageVal;
            this.customVal = __customVal;
            this.insuranceVal = __insuranceVal;
            //this.awbPlace = __awbPlace;
            this.SHC = __SHC;
        }

        private string _HAWB = "";
        public string HAWB
        {
            get { return _HAWB; }
            set { _HAWB = value; }
        }

        private int _HPcs = 0;
        public int HPcs
        {
            get { return _HPcs; }
            set { _HPcs = value; }
        }

        private int _SLAC = 0;
        public int SLAC
        {
            get { return _SLAC; }
            set { _SLAC = value; }
        }

        private decimal _HWeight = 0;
        public decimal HWeight
        {
            get { return _HWeight; }
            set { _HWeight = value; }
        }

        private string _HDest = "";
        public string HDest
        {
            get { return _HDest; }
            set { _HDest = value; }
        }

        private string _HOrigin = "";
        public string HOrigin
        {
            get { return _HOrigin; }
            set { _HOrigin = value; }
        }

        private string _Partial = "";
        public string Partial
        {
            get { return _Partial; }
            set { _Partial = value; }
        }

        private string _Commodity = "";
        public string Commodity
        {
            get 
            {
                if (_Commodity.Length > 15)
                    return _Commodity.Substring(0, 15);
                else
                    return _Commodity; 
            }
            set 
            {
                if (value.Length > 15)
                    _Commodity = value.Substring(0, 15);
                else
                    _Commodity = value;
            }
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
        private string _shipperAddrTel = "";
        public string shipperAddrTel
        {
            get { return _shipperAddrTel; }
            set { _shipperAddrTel = value; }
        }
        private string _shipperContact = "";
        public string shipperContact
        {
            get { return _shipperContact; }
            set { _shipperContact = value; }
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
        private string _cneeAddrTel = "";
        public string cneeAddrTel
        {
            get { return _cneeAddrTel; }
            set { _cneeAddrTel = value; }
        }
        private string _cneeContact = "";
        public string cneeContact
        {
            get { return _cneeContact; }
            set { _cneeContact = value; }
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

        private List<FhlTXTEntity> _colTXT = default(List<FhlTXTEntity>);
        public List<FhlTXTEntity> colTXT
        {
            get
            {
                if (_colTXT == default(List<FhlTXTEntity>))
                    _colTXT = new List<FhlTXTEntity>();

                return _colTXT;
            }
        }

        //2015-09-30
        private List<FhlTXTEntity> _colnewDBTXT = default(List<FhlTXTEntity>);
        public List<FhlTXTEntity> colnewDBTXT
        {
            get
            {
                if (_colnewDBTXT == default(List<FhlTXTEntity>))
                    _colnewDBTXT = new List<FhlTXTEntity>();

                return _colnewDBTXT;
            }
        }

        private List<FhlHTSEntity> _colHTS = default(List<FhlHTSEntity>);
        public List<FhlHTSEntity> colHTS
        {
            get
            {
                if (_colHTS == default(List<FhlHTSEntity>))
                    _colHTS = new List<FhlHTSEntity>();

                return _colHTS;
            }
        }

        private List<FhlHTSEntity> _colnewDBHTS = default(List<FhlHTSEntity>);
        public List<FhlHTSEntity> colnewDBHTS
        {
            get
            {
                if (_colnewDBHTS == default(List<FhlHTSEntity>))
                    _colnewDBHTS = new List<FhlHTSEntity>();

                return _colnewDBHTS;
            }
        }

        private List<FhlOCIEntity> _colOCI = default(List<FhlOCIEntity>);
        public List<FhlOCIEntity> colOCI
        {
            get
            {
                if (_colOCI == default(List<FhlOCIEntity>))
                    _colOCI = new List<FhlOCIEntity>();

                return _colOCI;
            }
        }

        private List<FhlOCIEntity> _colnewDBOCI = default(List<FhlOCIEntity>);
        public List<FhlOCIEntity> colnewDBOCI
        {
            get
            {
                if (_colnewDBOCI == default(List<FhlOCIEntity>))
                    _colnewDBOCI = new List<FhlOCIEntity>();

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
    }
}
