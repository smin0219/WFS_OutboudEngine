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
            this.CountryCode = __CountryCode;
            this.Infold = __Infold;
            this.CustomsId = __CustomsId;
            this.CustomsInfo = __CustomsInfo;
        }
    }
}
