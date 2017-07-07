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
        public UwsEntity(string __uld, string __pou, string __loadCategory, string __shc, string __shc2, string __shc3, decimal __weight, string __weightindicator)
        {
            this.uld = __uld;
            this.pou = __pou;
            this.loadCategory = __loadCategory;
            this.shc = __shc;
            this.shc2 = __shc2;
            this.shc3 = __shc3;
            this.weight = __weight;
            this.weightindicator = __weightindicator;
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
    }
}
