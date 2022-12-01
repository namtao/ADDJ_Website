using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    
    [Serializable]
    public class KhieuNai_TruyVanNameValue
    {

        public int Value { get; set; }
        public string Name { get; set; }
        public string PhepToan { get; set; }
        public string GiaTri { get; set; }

        public KhieuNai_TruyVanNameValue()
        {

            Value = 0;
            Name = string.Empty;            

        }
    }
}
