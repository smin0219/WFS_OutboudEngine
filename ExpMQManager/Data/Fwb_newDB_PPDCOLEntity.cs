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

        public Fwb_newDB_PPDCOLEntity(bool __hasPPDCOL, string __ChargeTypePPD, string __ChargeTypeCOL, int __MID, decimal __TotalWeightPPD, decimal __TotalWeightCOL, decimal __ValuationPPD, decimal __ValuationCOL, decimal __TaxesPPD, decimal __TaxesCOL, decimal __DueAgentPPD, decimal __DueAgentCOL, decimal __DueCarrierPPD, decimal __DueCarrierCOL, decimal __TotalPPD, decimal __TotalCOL)
        {
            //this.hasPPDCOL = __hasPPDCOL;
            this.ChargeTypePPD = __ChargeTypePPD;
            this.ChargeTypeCOL = __ChargeTypeCOL;
            this.MID = __MID;
            this.TotalWeightPPD = __TotalWeightPPD;
            this.TotalWeightCOL = __TotalWeightCOL;
            this.ValuationPPD = __ValuationPPD;
            this.ValuationCOL = __ValuationCOL;
            this.TaxesPPD = __TaxesPPD;
            this.TaxesCOL = __TaxesCOL;
            this.DueAgentPPD = __DueAgentPPD;
            this.DueAgentCOL = __DueAgentCOL;
            this.DueCarrierPPD = __DueCarrierPPD;
            this.DueCarrierCOL = __DueCarrierCOL;
            this.TotalPPD = __TotalPPD;
            this.TotalCOL = __TotalCOL;
            }

        private bool _hasPPD = false;
        public bool hasPPD
        {
            get
            {
                decimal tempSUMPPD = 0;
                tempSUMPPD = TotalWeightPPD + ValuationPPD + TaxesPPD + DueAgentPPD + DueCarrierPPD + TotalPPD;

                if (tempSUMPPD > 0 )
                    _hasPPD = true;
                
                return _hasPPD;
            }
        }

        private bool _hasCOL = false;
        public bool hasCOL
        {
            get
            {
                decimal tempSUMCOL = 0;
                tempSUMCOL = TotalWeightCOL + ValuationCOL + TaxesCOL + DueAgentCOL + DueCarrierCOL + TotalCOL;

                if (tempSUMCOL > 0)
                    _hasCOL = true;

                return _hasCOL;
            }
        }
        private string _ChargeTypePPD = "";
        public string ChargeTypePPD
        {
            get { return _ChargeTypePPD; }
            set { _ChargeTypePPD = value; }
        }

        private string _ChargeTypeCOL = "";
        public string ChargeTypeCOL
        {
            get { return _ChargeTypeCOL; }
            set { _ChargeTypeCOL = value; }
        }

        private int _MID = 0;
        public int MID
        {
            get{ return _MID; }
            set { _MID = value; }
        }

        private decimal _TotalWeightPPD = 0;
        public decimal TotalWeightPPD
        {
            get { return _TotalWeightPPD; }
            set { _TotalWeightPPD = value; }
        }

        private decimal _TotalWeightCOL = 0;
        public decimal TotalWeightCOL
            {
            get { return _TotalWeightCOL; }
            set { _TotalWeightCOL = value; }
            }

        private decimal _ValuationPPD = 0;
        public decimal ValuationPPD
        {
            get { return _ValuationPPD; }
            set { _ValuationPPD = value; }
        }

        private decimal _ValuationCOL = 0;
        public decimal ValuationCOL
            {
            get { return _ValuationCOL; }
            set { _ValuationCOL = value; }
            }

        private decimal _TaxesPPD = 0;
        public decimal TaxesPPD
        {
            get { return _TaxesPPD; }
            set { _TaxesPPD = value; }
        }

        private decimal _TaxesCOL = 0;
        public decimal TaxesCOL
            {
            get { return _TaxesCOL; }
            set { _TaxesCOL = value; }
            }

        private decimal _DueAgentPPD = 0;
        public decimal DueAgentPPD
        {
            get { return _DueAgentPPD; }
            set { _DueAgentPPD = value; }
        }

        private decimal _DueAgentCOL = 0;
        public decimal DueAgentCOL
            {
            get { return _DueAgentCOL; }
            set { _DueAgentCOL = value; }
            }

        private decimal _DueCarrierPPD = 0;
        public decimal DueCarrierPPD
        {
            get { return _DueCarrierPPD; }
            set { _DueCarrierPPD = value; }
        }

        private decimal _DueCarrierCOL = 0;
        public decimal DueCarrierCOL
            {
            get { return _DueCarrierCOL; }
            set { _DueCarrierCOL = value; }
            }

        private decimal _TotalPPD = 0;
        public decimal TotalPPD
        {
            get { return _TotalPPD; }
            set { _TotalPPD = value; }
        }

        private decimal _TotalCOL = 0;
        public decimal TotalCOL
            {
            get { return _TotalCOL; }
            set { _TotalCOL = value; }
            }
        }
}
