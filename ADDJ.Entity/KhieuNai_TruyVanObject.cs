using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    [Serializable]
    public class KhieuNai_TruyVanObject
    {
        public List<Object> object_list { get; set; }
        public KhieuNai_TruyVanObject()
        {            
            object_list = null;            
		}
    }
}
