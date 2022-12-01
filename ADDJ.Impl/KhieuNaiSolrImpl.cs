using ADDJ.Core;
using ADDJ.Core.Provider;
using ADDJ.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ADDJ.Impl
{
    public class KhieuNaiSolrImpl : BaseImpl<KhieuNaiSolrInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "vKhieuNai";
        }

        public KhieuNaiSolrImpl()
            : base()
        { }

        public KhieuNaiSolrImpl(string connectionString)
            : base(connectionString)
        {

        }
    }
}
