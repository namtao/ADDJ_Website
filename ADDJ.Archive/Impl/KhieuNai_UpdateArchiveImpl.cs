using GQKN.Archive.Core.Provider;
using GQKN.Archive.Entity;

namespace GQKN.Archive.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của KhieuNai_UpdateArchive
    /// </summary>
    /// <author>DaoVanDuong</author>
    /// <date>23/10/2016</date>

    public class KhieuNai_UpdateArchiveImpl : BaseImpl<KhieuNai_UpdateArchiveInfo>
    {
        public KhieuNai_UpdateArchiveImpl() : base() { }

        public KhieuNai_UpdateArchiveImpl(string connection) : base(connection) { }
        protected override void SetInfoDerivedClass()
        {
            TableName = "KhieuNai_UpdateArchive";
        }

        #region Function 

        #endregion
    }
}