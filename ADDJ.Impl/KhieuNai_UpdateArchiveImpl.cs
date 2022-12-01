using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_UpdateArchive
    /// </summary>
    /// <author>DaoVanDuong</author>
    /// <date>23/10/2016</date>

    public class KhieuNai_UpdateArchiveImpl : BaseImpl<KhieuNai_UpdateArchiveInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_UpdateArchive";
        }

        #region Function 

        #endregion
    }
}