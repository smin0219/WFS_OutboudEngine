﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FfmEntity : BaseDB
    {
        public FfmEntity()
        {
            //Empty Constructor
        }
        // 2014-04-18 Add _BuiltPcs
        public FfmEntity(int __mid, string __prefix, string __awb, string __origin, string __dest,
            string __fOrigin, string __fDest, string __shipmentIndicator, int __pcs, double __weight,
            int __fPcs, double __fWeight, double __volWeight, int __legSeq, string __commodity, 
            string __shc, string __uld, int __builtPcs)
        {
            this.mid = __mid;
            this.prefix = __prefix;
            this.awb = __awb;
            this.origin = __origin;
            this.dest = __dest;
            this.fOrigin = __fOrigin;
            this.fDest = __fDest;
            this.shipmentIndicator = __shipmentIndicator;
            this.pcs = __pcs;
            this.weight = __weight;
            this.fPcs = __fPcs;
            this.fWeight = __fWeight;
            this.volWeight = __volWeight;
            this.legSeq = __legSeq;
            this.commodity = __commodity;
            this.shc = __shc;
            this.uld = __uld;
            this.builtPCs = __builtPcs;
        }

        private int _mid = 0;
        public int mid
        {
            get { return _mid; }
            set { _mid = value; }
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

        private string _fOrigin = "";
        public string fOrigin
        {
            get { return _fOrigin; }
            set { _fOrigin = value; }
        }

        private string _fDest = "";
        public string fDest
        {
            get { return _fDest; }
            set { _fDest = value; }
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

        private int _fPcs = 0;
        public int fPcs
        {
            get { return _fPcs; }
            set { _fPcs = value; }
        }

        private double _fWeight = 0.00;
        public double fWeight
        {
            get { return _fWeight; }
            set { _fWeight = value; }
        }

        private double _volWeight = 0.00;
        public double volWeight
        {
            get { return _volWeight; }
            set { _volWeight = value; }
        }

        private int _legSeq = 0;
        public int legSeq
        {
            get { return _legSeq; }
            set { _legSeq = value; }
        }

        private string _commodity = "";
        public string commodity
        {
            get { return _commodity; }
            set { _commodity = value; }
        }

        private string _shc = "";
        public string shc
        {
            get { return _shc; }
            set { _shc = value; }
        }

        private string _uld = "";
        public string uld
        {
            get { return _uld; }
            set { _uld = value; }
        }

        private int _builtPCs = 0;
        public int builtPCs
        {
            get { return _builtPCs; }
            set { _builtPCs = value; }
        }

        // 2016-1-18 added. for SHC code(Exp_Master_SPH)
        private List<FfmSHCEntity> _SHCList = default(List<FfmSHCEntity>);
        public List<FfmSHCEntity> SHCList
        {
            get
            {
                if (_SHCList == default(List<FfmSHCEntity>))
                    _SHCList = new List<FfmSHCEntity>();

                return _SHCList;
            }
            set
            {
                _SHCList = value;
            }
        }
    }

}
