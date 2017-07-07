using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class Fwb_newDB_SSREntity
    {
        public Fwb_newDB_SSREntity()
        {

        }

        public Fwb_newDB_SSREntity(int __MID, int __Seq, string __SSRtext)
        {
            this.MID = __MID;
            this.Seq = __Seq;
            this.SSRtext = __SSRtext;
        }

        private int _MID = 0;
        public int MID
        {
            get { return _MID; }
            set { _MID = value; }
        }

        private int _Seq = 0;
        public int Seq
        {
            get { return _Seq; }
            set { _Seq = value; }
        }

        private string _SSRtext = "";
        public string SSRtext
        {
            get { return _SSRtext; }
            set { _SSRtext = value; }
        }

    }
}
