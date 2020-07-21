using System;
using System.Collections.Generic;
using System.Text;

namespace BaseS.Data.Database
{
    public class DbReq<T>
    {
        public string ConnStr;
        public string SqlTxt;
        public IEnumerable<T> Parms = null;
        public bool Exp = false;
        public bool IsSP = false;
    }
}
