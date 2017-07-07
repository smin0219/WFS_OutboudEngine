using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class Fwb_newDB_SHCEntity
    {
        public Fwb_newDB_SHCEntity()
        { }

        public Fwb_newDB_SHCEntity(string __SHCvalue)
        {
            this.SHCvalue = __SHCvalue;
        }
        public string _SHCvalue = "";
        public string SHCvalue
        {
            get { return _SHCvalue; }
            set { _SHCvalue = value; }
        }
    }
}
