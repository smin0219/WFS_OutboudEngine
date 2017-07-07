using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FwbNGEntity
    {
        public FwbNGEntity()
        {
            //Empty Constructor
        }

        public FwbNGEntity(int __seq, string __type, string __naturegoods)
        {
            this.seq = __seq;
            this.type = __type;
            this.naturegoods = __naturegoods;
        }

        private int _seq = 0;
        public int seq
        {
            get { return _seq; }
            set { _seq = value; }
        }

        private string _type = "";
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _naturegoods = "";
        public string naturegoods
        {
            get { return _naturegoods; }
            set { _naturegoods = value; }
        }
    }
}
