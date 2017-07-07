using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class Fwb_newDB_PPDCOLEntity
    {
        public Fwb_newDB_PPDCOLEntity()
        { }

        public Fwb_newDB_PPDCOLEntity(bool __hasPPDCOL, string __ChargeType, int __MID, decimal __TotalWeight, decimal __Valuation, decimal __Taxes, decimal __DueAgent, decimal __DueCarrier, decimal __Total)
        {
            //this.hasPPDCOL = __hasPPDCOL;
            this.ChargeType = __ChargeType;
            this.MID = __MID;
            this.TotalWeight = __TotalWeight;
            this.Valuation = __Valuation;
            this.Taxes = __Taxes;
            this.DueAgent = __DueAgent;
            this.DueCarrier = __DueCarrier;
            this.Total = __Total;
        }

        private bool _hasPPDCOL = false;
        public bool hasPPDCOL
        {
            get
            {
                decimal tempSUM = 0;
                tempSUM = TotalWeight + Valuation + Taxes + DueAgent + DueCarrier + Total;

                if (tempSUM > 0)
                    _hasPPDCOL = true;
                
                return _hasPPDCOL;
            }
        }
        private string _ChargeType = "";
        public string ChargeType
        {
            get { return _ChargeType; }
            set { _ChargeType = value; }
        }

        private int _MID = 0;
        public int MID
        {
            get{ return _MID; }
            set { _MID = value; }
        }

        private decimal _TotalWeight = 0;
        public decimal TotalWeight
        {
            get { return _TotalWeight; }
            set { _TotalWeight = value; }
        }

        private decimal _Valuation = 0;
        public decimal Valuation
        {
            get { return _Valuation; }
            set { _Valuation = value; }
        }

        private decimal _Taxes = 0;
        public decimal Taxes
        {
            get { return _Taxes; }
            set { _Taxes = value; }
        }

        private decimal _DueAgent = 0;
        public decimal DueAgent
        {
            get { return _DueAgent; }
            set { _DueAgent = value; }
        }

        private decimal _DueCarrier = 0;
        public decimal DueCarrier
        {
            get { return _DueCarrier; }
            set { _DueCarrier = value; }
        }

        private decimal _Total = 0;
        public decimal Total
        {
            get { return _Total; }
            set { _Total = value; }
        }
    }
}
