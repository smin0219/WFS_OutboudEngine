using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpMQManager.Data
{
    public class FfmSHCEntity
    {
        public FfmSHCEntity()
        { }

        public FfmSHCEntity(int __MID, int __Seq, string __SHCcode)
        {
            this.MID = __MID;
            this.Seq = __Seq;
            this.SHCcode = __SHCcode;
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
        private string _SHCcode = "";
        public string SHCcode
        {
            get { return _SHCcode; }
            set { _SHCcode = value; }
        }
    }
}
