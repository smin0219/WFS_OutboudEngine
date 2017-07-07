using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class NfmEntity : BaseDB
    {
        public NfmEntity()
        {

        }
        public NfmEntity(int __id, int __uldid, string __uld, string __pou, string __ehc, string __ihc, string __unitclass, int __overhangcnt,
            double __uldweight, string __unitcontour, int __unitvolume, string __finaldest, string __ihc2)
        {
            this.id = __id;
            this.uldid = __uldid;
            this.uld = __uld;
            this.pou = __pou;
            this.ehc = __ehc;
            this.ihc = __ihc;
            this.unitclass = __unitclass;
            this.overhangcnt = __overhangcnt;
            this.uldweight = __uldweight;
            this.unitcontour = __unitcontour;
            this.unitvolume = __unitvolume;
            this.finaldest = __finaldest;
            this.ihc2 = __ihc2;
        }

        private int _id = 0;
        public int id
        {
            get
            {
                return _id;
            }
            set{
                _id = value;
            }
        }
        private int _uldid = 0;
        public int uldid
        {
            get
            {
                return _uldid;
            }
            set
            {
                _uldid = value;
            }
        }
        private string _uld = "";
        public string uld
        {
            get
            {
                return _uld;
            }
            set
            {
                _uld = value;
            }
        }
        private string _pou = "";
        public string pou
        {
            get
            {
                return _pou;
            }
            set
            {
                _pou = value;
            }
        }
        private string _ehc = "";
        public string ehc
        {
            get
            {
                return _ehc;
            }
            set
            {
                _ehc = value;
            }
        }
        private string _ihc = "";
        public string ihc
        {
            get
            {
                return _ihc;
            }
            set
            {
                _ihc = value;
            }
        }
        private string _unitclass = "";
        public string unitclass
        {
            get
            {
                return _unitclass;
            }
            set
            {
                _unitclass = value;
            }
        }
        private int _overhangcnt = 0;
        public int overhangcnt
        {
            get
            {
                return _overhangcnt;
            }
            set
            {
                _overhangcnt = value;
            }
        }
        private double _uldweight = 0.00;
        public double uldweight
        {
            get
            {
                return _uldweight;
            }
            set
            {
                _uldweight = value;
            }
        }
        private string _unitcontour = "";
        public string unitcontour
        {
            get
            {
                return _unitcontour;
            }
            set
            {
                _unitcontour = value;
            }
        }
        private int _unitvolume = 0;
        public int unitvolume
        {
            get
            {
                return _unitvolume;
            }
            set
            {
                _unitvolume = value;
            }
        }
        private string _finaldest = "";
        public string finaldest
        {
            get
            {
                return _finaldest;
            }
            set
            {
                _finaldest = value;
            }
        }
        private string _ihc2 = "";
        public string ihc2
        {
            get
            {
                return _ihc2;
            }
            set
            {
                _ihc2 = value;
            }
        }
    }
}
