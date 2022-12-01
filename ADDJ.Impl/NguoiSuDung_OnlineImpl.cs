using ADDJ.Core.Provider;
using ADDJ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Impl
{
    public class NguoiSuDung_OnlineImpl : BaseImpl<NguoiSuDung_OnlineInfo>
    {
         public NguoiSuDung_OnlineImpl()
            : base()
        { }

         public NguoiSuDung_OnlineImpl(string connection)
            : base(connection)
        { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "NguoiSuDung_Online";
        }
    }
}
