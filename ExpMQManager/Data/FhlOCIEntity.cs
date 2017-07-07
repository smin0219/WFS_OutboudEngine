using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FhlOCIEntity
    {
        public FhlOCIEntity()
        {
            //Empty Constructor
        }

        public FhlOCIEntity(string __CountryCode, string __Infold, string __CustomsId, string __CustomsInfo)
        {
            this.CountryCode = __CountryCode;
            this.Infold = __Infold;
            this.CustomsId = __CustomsId;
            this.CustomsInfo = __CustomsInfo;
        }

        private string _CountryCode = "";
        public string CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }
        private string _Infold = "";
        public string Infold
        {
            get { return _Infold; }
            set { _Infold = value; }
        }
        private string _CustomsId = "";
        public string CustomsId
        {
            get { return _CustomsId; }
            set { _CustomsId = value; }
        }
        private string _CustomsInfo = "";
        public string CustomsInfo
        {
            get { return _CustomsInfo; }
            set { _CustomsInfo = value; }
        }

    }
}
