using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class Fwb_newDB_OSIEntity
    {
        public Fwb_newDB_OSIEntity()
        {
            //Empty Constructor
        }
        public Fwb_newDB_OSIEntity(string __OtherInfo)
        {
            this.OtherInfo = __OtherInfo;
        }

        private string _CountryCode = "";
        public string CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }
    }
}
