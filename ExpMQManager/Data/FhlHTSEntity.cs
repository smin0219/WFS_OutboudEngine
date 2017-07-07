using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FhlHTSEntity
    {
        public FhlHTSEntity()
        {
            //Empty Constructor
        }

        public FhlHTSEntity(string __Descr)
        {
            this.Descr = __Descr;
        }

        private string _Descr = "";
        public string Descr
        {
            get { return _Descr; }
            set { _Descr = value; }
        }
    }
}
