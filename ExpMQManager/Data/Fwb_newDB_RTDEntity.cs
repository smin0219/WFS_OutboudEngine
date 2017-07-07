using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class Fwb_newDB_RTDEntity
    {
        public Fwb_newDB_RTDEntity()
        {
            //Empty Constructor
        }

        public Fwb_newDB_RTDEntity(int __pcsRTD, double __weightRTD, string __classRTD,
            double __chargeWeight, double __rateCharge, double __total, string __goodsType,
            string __natureOfGoods)
        {
            this.pcsRTD = __pcsRTD;
            this.weightRTD = __weightRTD;
            this.classRTD = __classRTD;
            this.chargeWeight = __chargeWeight;
            this.rateCharge = __rateCharge;
            this.total = __total;
            this.goodsType = __goodsType;
            this.natureOfGoods = __natureOfGoods;
        }

        private int _pcsRTD = 0;
        public int pcsRTD
        {
            get { return _pcsRTD; }
            set { _pcsRTD = value; }
        }

        private double _weightRTD = 0.00;
        public double weightRTD
        {
            get { return _weightRTD; }
            set { _weightRTD = value; }
        }

        private string _classRTD = "";
        public string classRTD
        {
            get { return _classRTD; }
            set { _classRTD = value; }
        }

        private double _chargeWeight = 0.00;
        public double chargeWeight
        {
            get { return _chargeWeight; }
            set { _chargeWeight = value; }
        }

        private double _rateCharge = 0.00;
        public double rateCharge
        {
            get { return _rateCharge; }
            set { _rateCharge = value; }
        }

        private double _total = 0.00;
        public double total
        {
            get { return _total; }
            set { _total = value; }
        }

        private string _goodsType = "";
        public string goodsType
        {
            get { return _goodsType; }
            set { _goodsType = value; }
        }

        private string _natureOfGoods = "";
        public string natureOfGoods
        {
            get { return _natureOfGoods; }
            set { _natureOfGoods = value; }
        }

    }
}
