using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class UwsEntity : BaseDB
    {
        public UwsEntity()
        {

        }
        public UwsEntity(string __uld, string __pou, string __loadCategory, string __shc, string __shc2, string __shc3, string __shc4, string __shc5, string __shc6, string __shc7, string __shc8, string __shc9, decimal __weight, string __weightindicator, int __isFinal)
        {
            this.uld = __uld;
            this.pou = __pou;
            this.loadCategory = __loadCategory;
            this.shc = __shc;
            this.shc2 = __shc2;
            this.shc3 = __shc3;
            this.shc4 = __shc4;
            this.shc5 = __shc5;
            this.shc6 = __shc6;
            this.shc7 = __shc7;
            this.shc8 = __shc8;
            this.shc9 = __shc9;
            this.weight = __weight;
            this.weightindicator = __weightindicator;
            this.isFinal = __isFinal;
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
        private string _loadCategory = "";
        public string loadCategory
        {
            get
            {
                return _loadCategory;
            }
            set
            {
                _loadCategory = value;
            }
        }
        private string _shc = "";
        public string shc
        {
            get
            {
                return _shc;
            }
            set
            {
                _shc = value;
            }
        }
        private string _shc2 = "";
        public string shc2
        {
            get
            {
                return _shc2;
            }
            set
            {
                _shc2 = value;
            }
        }
        private string _shc3 = "";
        public string shc3
        {
            get
            {
                return _shc3;
            }
            set
            {
                _shc3 = value;
            }
        }
        private string _shc4 = "";
        public string shc4
        {
            get
            {
                return _shc4;
            }
            set
            {
                _shc4 = value;
            }
        }
        private string _shc5 = "";
        public string shc5
        {
            get
            {
                return _shc5;
            }
            set
            {
                _shc5 = value;
            }
        }
        private string _shc6 = "";
        public string shc6
        {
            get
            {
                return _shc6;
            }
            set
            {
                _shc6 = value;
            }
        }
        private string _shc7 = "";
        public string shc7
        {
            get
            {
                return _shc7;
            }
            set
            {
                _shc7 = value;
            }
        }
        private string _shc8 = "";
        public string shc8
        {
            get
            {
                return _shc8;
            }
            set
            {
                _shc8 = value;
            }
        }
        private string _shc9 = "";
        public string shc9
        {
            get
            {
                return _shc9;
            }
            set
            {
                _shc9 = value;
            }
        }

        private decimal _weight = 0;
        public decimal weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }
        private string _weightindicator = "";
        public string weightindicator
        {
            get
            {
                return _weightindicator;
            }
            set
            {
                _weightindicator = value;
            }
        }
        private int _isFinal = 0;
        public int isFinal
        {
            get
            {
                return _isFinal;
            }
            set
            {
                _isFinal = value;
            }
        }
    }
}
