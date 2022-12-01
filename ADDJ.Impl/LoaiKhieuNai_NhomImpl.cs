using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Linq;
using System.Web;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace ADDJ.Impl
{
    public partial class LoaiKhieuNai_NhomImpl : BaseImpl<LoaiKhieuNai_NhomInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoaiKhieuNai_Nhom";

            IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";
            MaxFieldLength = 10;
            IsUseLucene = true;
            IsUpdateLucene = true;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 29/11/2014
        /// </summary>
        /// <returns></returns>
        public List<LoaiKhieuNai_NhomInfo> GetAll()
        {
            List<LoaiKhieuNai_NhomInfo> lstRet = null;
            string sql = @"SELECT * FROM LoaiKhieuNai_Nhom ORDER BY TenNhom ASC";
            lstRet = ExecuteQuery(sql, CommandType.Text, null);
            return lstRet;
        }
    }
}
