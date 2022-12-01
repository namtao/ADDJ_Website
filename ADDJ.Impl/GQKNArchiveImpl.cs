using ADDJ.Core;
using ADDJ.Entity;
using Microsoft.Practices.ServiceLocation;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ADDJ.Impl
{
    /// <summary>
    /// Author : Phi Hoang Hai
    /// Created date : 14/07/2014
    /// Todo : Xây dựng các phương thức thực hiện archive dữ liệu khiếu nại
    /// </summary>
    public class GQKNArchiveImpl
    {
        private const string SOLR_GQKN = "GQKN";

        private string URL_SOLR_GQKN
        {
            get
            {
                return Config.ServerSolr + SOLR_GQKN;
            }
        }

        public void ArchiveData(DateTime fromDate, DateTime toDate)
        {
            // Lấy danh sách khiếu nại đánh dấu trạng thái đã Archive tại DB chính
            List<KhieuNaiInfo> listKhieuNaiInfoArchived = ListKhieuNaiArchived();

            if (listKhieuNaiInfoArchived != null && listKhieuNaiInfoArchived.Count > 0)
            {
                // Kiểm tra Solr đã được cập nhật chưa ? Nếu solr chưa được cập nhật thì cập nhật solr

                // Xóa khiếu nại ở các bảng (KhieuNai, KhieuNai_Activity,...) của DB chính
                Dictionary<int, int> dicKhieuNaiIdNeedDelete = new Dictionary<int, int>();
                for (int i = 0; i < listKhieuNaiInfoArchived.Count;i++ )
                {
                    dicKhieuNaiIdNeedDelete.Add(listKhieuNaiInfoArchived[i].Id, listKhieuNaiInfoArchived[i].ArchiveId);
                }
                
                List<int> listKhieuNaiIdError = DeleteKhieuNaiArchived(dicKhieuNaiIdNeedDelete);
                if(listKhieuNaiIdError != null && listKhieuNaiIdError.Count > 0)
                {
                    string errorMesage = string.Empty;
                    for(int i=0;i<listKhieuNaiIdError.Count;i++)
                    {
                        errorMesage = string.Format("{0}{1}, ", errorMesage, listKhieuNaiIdError[i]);
                    }

                    errorMesage = errorMesage.Trim().TrimEnd(',');
                    errorMesage = string.Format("Các khiếu nại có mã sau không lưu trữ được : {0}", errorMesage);
                }

            } // end if(listKhieuNaiInfo != null && listKhieuNaiInfo.Count > 0)
            else
            {
                // Lấy danh sách khiếu nại cần Archive theo cấu hình tại DB chính
                List<KhieuNaiInfo> listKhieuNaiNeedArchive = ListKhieuNaiNeedArchive(fromDate, toDate);
                if(listKhieuNaiNeedArchive != null && listKhieuNaiNeedArchive.Count > 0)
                {
                    
                    UpdateArchived(listKhieuNaiNeedArchive, fromDate, toDate);
                } // end if(listKhieuNaiNeedArchive != null && listKhieuNaiNeedArchive.Count > 0)
            }
        }

        public List<KhieuNaiInfo> ListKhieuNaiArchived()
        {
            List<KhieuNaiInfo> listKhieuNaiInfo = null;
            KhieuNaiImpl khieuNaiImpl = new KhieuNaiImpl();
            listKhieuNaiInfo = khieuNaiImpl.GetListDynamic("*", "ArchiveId > 0", "");

            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy ra danh sách khiếu nại cần archive
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<KhieuNaiInfo> ListKhieuNaiNeedArchive(DateTime fromDate, DateTime toDate)
        {
            List<KhieuNaiInfo> listKhieuNaiInfo = null;
            KhieuNaiImpl khieuNaiImpl = new KhieuNaiImpl();
            string whereClause = string.Format("NgayTiepNhanSort >={0} AND NgayTiepNhanSort <= {1} AND ArchiveId = 0 AND TrangThai=3", fromDate.ToString("yyyyMMdd"), toDate.ToString("yyyyMMdd"));
            listKhieuNaiInfo = khieuNaiImpl.GetListDynamic("*", whereClause, "Id ASC");
            return listKhieuNaiInfo;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Thực hiện update 
        /// </summary>
        /// <param name="listKhieuNaiArchived"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private List<int> UpdateArchived(List<KhieuNaiInfo> listKhieuNaiArchived, DateTime fromDate, DateTime toDate)
        {
            List<int> listError = new List<int>();
            int maxToCommitSolr = 100;
            ArchiveConfigInfo archiveConfig = GetArchiveConfig(fromDate.Year);

            // Trường hợp archiveConfig != null
            if(archiveConfig != null)
            {
                int indexForCommitSolr = 0;
                List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo = new List<KhieuNaiSolrInfo>();

                for(int i=0;i<listKhieuNaiArchived.Count;i++)
                {
                    int khieuNaiIdError = ArchiveDataSql(listKhieuNaiArchived[i], archiveConfig);
                    if(khieuNaiIdError > 0)
                    {
                        listError.Add(khieuNaiIdError);
                    }
                    else
                    {
                        indexForCommitSolr++;
                        KhieuNaiSolrInfo khieuNaiSolrInfo = GetByKhieuNaiId(listKhieuNaiArchived[i].Id);
                        if(khieuNaiSolrInfo != null)
                        {
                            listKhieuNaiSolrInfo.Add(khieuNaiSolrInfo);
                        }
                    }

                    // Commit Solr
                    if(indexForCommitSolr == maxToCommitSolr)
                    {
                        // Commit Solr
                        CommitSolr(listKhieuNaiSolrInfo, archiveConfig.Id);

                        // Set lại số thứ tự ban đầu
                        indexForCommitSolr = 0;
                        listKhieuNaiSolrInfo = new List<KhieuNaiSolrInfo>();
                    }
                }
            }
            else
            {
                for(int i=0;i<listKhieuNaiArchived.Count;i++)
                {
                    listError.Add(listKhieuNaiArchived[i].Id);
                }
            }
            return listError;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Thực hiện copy dữ liệu từ DB chính sang các bảng của DB lưu trữ
        /// </summary>
        /// <param name="khieuNaiInfo"></param>
        /// <param name="archiveConfig"></param>
        /// <returns></returns>
        private int ArchiveDataSql(KhieuNaiInfo khieuNaiInfo, ArchiveConfigInfo archiveConfig)
        {            
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string connectionStringArchive = archiveConfig.ConnectionString;

                    // Update lại ArchiveId
                    KhieuNaiImpl khieuNaiImpl = new KhieuNaiImpl();
                    khieuNaiInfo.ArchiveId = archiveConfig.Id;
                    khieuNaiImpl.Update(khieuNaiInfo);

                    KhieuNaiImpl khieuNaiImplArchive = new KhieuNaiImpl(connectionStringArchive);
                    khieuNaiImplArchive.AddArchive(khieuNaiInfo);

                    // Add Activity
                    KhieuNai_ActivityImpl khieuNaiActivityImpl = new KhieuNai_ActivityImpl();
                    List<KhieuNai_ActivityInfo> listKhieuNaiActivity = khieuNaiActivityImpl.GetListByKhieuNaiId(khieuNaiInfo.Id);
                    if(listKhieuNaiActivity != null)
                    {
                        KhieuNai_ActivityImpl khieuNaiActivityImplArchive = new KhieuNai_ActivityImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiActivity.Count;i++)
                        {
                            khieuNaiActivityImplArchive.AddArchive(listKhieuNaiActivity[i]);
                        }
                    }

                    //Add from KhieuNai_BuocXuLy
                    KhieuNai_BuocXuLyImpl khieuNaiBuocXuLyImpl = new KhieuNai_BuocXuLyImpl();
                    List<KhieuNai_BuocXuLyInfo> listKhieuNaiBuocXuLy = khieuNaiBuocXuLyImpl.GetListByKhieuNaiId(khieuNaiInfo.Id);
                    if(listKhieuNaiBuocXuLy != null)
                    {
                        KhieuNai_BuocXuLyImpl khieuNaiBuocXuLyImplArchive = new KhieuNai_BuocXuLyImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiBuocXuLy.Count;i++)
                        {
                            khieuNaiBuocXuLyImplArchive.AddArchive(listKhieuNaiBuocXuLy[i]);
                        }
                    }                    

                    //Add KhieuNai_FileDinhKem
                    KhieuNai_FileDinhKemImpl khieuNaiFileDinhKemImpl = new KhieuNai_FileDinhKemImpl();
                    List<KhieuNai_FileDinhKemInfo> listKhieuNaiFileDinhKem = khieuNaiFileDinhKemImpl.GetListByKhieuNaiId(khieuNaiInfo.Id);
                    if(listKhieuNaiFileDinhKem != null)
                    {
                        KhieuNai_FileDinhKemImpl khieuNaiFileDinhKemImplArchive = new KhieuNai_FileDinhKemImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiFileDinhKem.Count;i++)
                        {
                            khieuNaiFileDinhKemImplArchive.AddArchive(listKhieuNaiFileDinhKem[i]);
                        }
                    }

                    //Add KhieuNai_GiaiPhap
                    KhieuNai_GiaiPhapImpl khieuNaiGiaiPhapImpl = new KhieuNai_GiaiPhapImpl();
                    List<KhieuNai_GiaiPhapInfo> listKhieuNaiGiaiPhap = khieuNaiGiaiPhapImpl.GetListByKhieuNaiId(khieuNaiInfo.Id);
                    if(listKhieuNaiGiaiPhap != null)
                    {
                        KhieuNai_GiaiPhapImpl khieuNaiGiaiPhapImplArchive = new KhieuNai_GiaiPhapImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiGiaiPhap.Count;i++)
                        {
                            khieuNaiGiaiPhapImplArchive.AddArchive(listKhieuNaiGiaiPhap[i]);
                        }
                    }

                    //Add KhieuNai_KetQuaXuLy
                    KhieuNai_KetQuaXuLyImpl khieuNaiKetQuaXuLyImpl = new KhieuNai_KetQuaXuLyImpl();
                    List<KhieuNai_KetQuaXuLyInfo> listKhieuNaiKetQuaXuLy = khieuNaiKetQuaXuLyImpl.GetListDynamic("*", "KhieuNaiId=" + khieuNaiInfo.Id, "Id ASC");
                    if(listKhieuNaiKetQuaXuLy != null)
                    {
                        KhieuNai_KetQuaXuLyImpl khieuNaiKetQuaXuLyImplArchive = new KhieuNai_KetQuaXuLyImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiKetQuaXuLy.Count;i++)
                        {
                            khieuNaiKetQuaXuLyImplArchive.AddArchive(listKhieuNaiKetQuaXuLy[i]);
                        }
                    }

                    //Add KhieuNai_Log
                    KhieuNai_LogImpl khieuNaiLogImpl = new KhieuNai_LogImpl();
                    List<KhieuNai_LogInfo> listKhieuNaiLog = khieuNaiLogImpl.GetListDynamic("*", "KhieuNaiId=" + khieuNaiInfo.Id, "Id ASC");
                    if(listKhieuNaiLog != null)
                    {
                        KhieuNai_LogImpl khieuNaiLogImplArchive = new KhieuNai_LogImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiLog.Count;i++)
                        {
                            khieuNaiLogImplArchive.AddArchive(listKhieuNaiLog[i]);
                        }
                    }

                    //Add KhieuNai_SoTien
                    KhieuNai_SoTienImpl khieuNaiSoTienImpl = new KhieuNai_SoTienImpl();
                    List<KhieuNai_SoTienInfo> listKhieuNaiSoTien = khieuNaiSoTienImpl.GetListDynamic("*", "KhieuNaiId = " + khieuNaiInfo.Id, "Id ASC");
                    if(listKhieuNaiSoTien != null)
                    {
                        KhieuNai_SoTienImpl khieuNaiSoTienImplArchive = new KhieuNai_SoTienImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiSoTien.Count;i++)
                        {
                            khieuNaiSoTienImplArchive.AddArchive(listKhieuNaiSoTien[i]);
                        }
                    }

                    //Add KhieuNai_Watchers
                    KhieuNai_WatchersImpl khieuNaiWatchersImpl = new KhieuNai_WatchersImpl();
                    List<KhieuNai_WatchersInfo> listKhieuNaiWatchers = khieuNaiWatchersImpl.GetListByKhieuNaiId(khieuNaiInfo.Id);
                    if(listKhieuNaiWatchers != null)
                    {
                        KhieuNai_WatchersImpl khieuNaiWatchersImplArchive = new KhieuNai_WatchersImpl(connectionStringArchive);
                        for(int i=0;i<listKhieuNaiWatchers.Count;i++)
                        {
                            khieuNaiWatchersImplArchive.AddArchive(listKhieuNaiWatchers[i]);
                        }
                    }
                    
                    scope.Complete();
                }
                catch
                {
                    return khieuNaiInfo.Id;
                }
            } // end (TransactionScope scope = new TransactionScope())

            return -1;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy thông tin Archive
        /// </summary>
        /// <param name="namLuuTru"></param>
        /// <returns></returns>
        private ArchiveConfigInfo GetArchiveConfig(int namLuuTru)
        {
            ArchiveConfigImpl archiveConfigImpl = new ArchiveConfigImpl();
            string whereClause = string.Format("NamLuuTru = {0}", namLuuTru);
            List<ArchiveConfigInfo> listArchiveConfigInfo = archiveConfigImpl.GetListDynamic("*", whereClause, "Id ASC");

            return listArchiveConfigInfo[0];
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 14/07/2014
        /// Todo : Xóa các khiếu nại đã được đánh dấu IsArchived = 1
        /// </summary>
        /// <param name="listKhieuNaiIdNeedDelete">
        ///     Tkey : KhieuNaiId
        ///     TValue : ArchiveId
        /// </param>
        /// <returns></returns>
        private List<int> DeleteKhieuNaiArchived(Dictionary<int, int> dicKhieuNaiIdNeedDelete)
        {
            List<int> listError = new List<int>();
            ISolrOperations<KhieuNaiSolrInfo> solr = ServiceLocator.Current.GetInstance<ISolrOperations<KhieuNaiSolrInfo>>();

            foreach(KeyValuePair<int, int> pair in dicKhieuNaiIdNeedDelete)
            {
                List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo = new List<KhieuNaiSolrInfo>();
                string whereClause = string.Format("Id : {0}", pair.Key);
                SolrQuery solrQuery = new SolrQuery(whereClause);
                QueryOptions qoKhieuNai = new QueryOptions();
                Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
                extraParamKhieuNai.Add("fl", @"*");

                qoKhieuNai.ExtraParams = extraParamKhieuNai;

                qoKhieuNai.Start = 0;
                qoKhieuNai.Rows = int.MaxValue;
                listKhieuNaiSolrInfo = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);
                if(listKhieuNaiSolrInfo != null && listKhieuNaiSolrInfo.Count > 0)
                {                   
                    bool isSuccess = false;
                    
                    using (TransactionScope scope = new TransactionScope())
                    {                        
                        try
                        {
                            //Delete from KhieuNai_Activity
                            KhieuNai_ActivityImpl khieuNaiActivityImpl = new KhieuNai_ActivityImpl();
                            khieuNaiActivityImpl.Delete(pair.Key);

                            //Delete from KhieuNai_BuocXuLy
                            KhieuNai_BuocXuLyImpl khieuNaiBuocXuLyImpl = new KhieuNai_BuocXuLyImpl();
                            khieuNaiBuocXuLyImpl.Delete(pair.Key);

                            //Delete from KhieuNai_FileDinhKem
                            KhieuNai_FileDinhKemImpl khieuNaiFileDinhKemImpl = new KhieuNai_FileDinhKemImpl();
                            khieuNaiFileDinhKemImpl.Delete(pair.Key);

                            //Delete from KhieuNai_GiaiPhap
                            KhieuNai_GiaiPhapImpl khieuNaiGiaiPhapImpl = new KhieuNai_GiaiPhapImpl();
                            khieuNaiGiaiPhapImpl.Delete(pair.Key);

                            //Delete from KhieuNai_KetQuaXuLy
                            KhieuNai_KetQuaXuLyImpl khieuNaiKetQuaXuLyImpl = new KhieuNai_KetQuaXuLyImpl();
                            khieuNaiKetQuaXuLyImpl.Delete(pair.Key);

                            //Delete from KhieuNai_Log
                            KhieuNai_LogImpl khieuNaiLogImpl = new KhieuNai_LogImpl();
                            khieuNaiLogImpl.Delete(pair.Key);

                            //Delete from KhieuNai_SoTien
                            KhieuNai_SoTienImpl khieuNaiSoTienImpl = new KhieuNai_SoTienImpl();
                            khieuNaiSoTienImpl.Delete(pair.Key);

                            //Delete from KhieuNai_Watchers
                            KhieuNai_WatchersImpl khieuNaiWatchersImpl = new KhieuNai_WatchersImpl();
                            khieuNaiWatchersImpl.Delete(pair.Key);

                            //Delete from KhieuNai
                            KhieuNaiImpl khieuNaiImpl = new KhieuNaiImpl();
                            khieuNaiImpl.Delete(pair.Key);

                            scope.Complete();

                            isSuccess = true;
                        }
                        catch
                        {
                            listError.Add(pair.Key);
                        }
                    } // end using (TransactionScope scope = new TransactionScope())

                    if(isSuccess)
                    {
                        if (listKhieuNaiSolrInfo[0].ArchiveId == 0)
                        {
                            listKhieuNaiSolrInfo[0].ArchiveId = pair.Value;
                            solr.Add(listKhieuNaiSolrInfo[0]);
                        }
                    }
                }                
            } // end for (int i = 0; i < listKhieuNaiIdNeedDelete.Count;i++ )

            solr.Commit();
                
            return listError;
        }

        private void CommitSolr(List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo, int archiveId)
        {
            if (listKhieuNaiSolrInfo != null && listKhieuNaiSolrInfo.Count > 0)
            {
                ISolrOperations<KhieuNaiSolrInfo> solr = ServiceLocator.Current.GetInstance<ISolrOperations<KhieuNaiSolrInfo>>();
                ISolrConnection solrConnection = ServiceLocator.Current.GetInstance<ISolrConnection>();                
                //CommitOptions commitOption = new CommitOptions();
                //commitOption.WaitFlush = true;
                //solrConnection.p
                
                for (int i = 0; i < listKhieuNaiSolrInfo.Count;i++ )
                {
                    if (listKhieuNaiSolrInfo[0].ArchiveId == 0)
                    {
                        listKhieuNaiSolrInfo[0].ArchiveId = archiveId;
                        //solr.Add(listKhieuNaiSolrInfo[0]);
                        var updateXml = string.Format("<add><doc><field name='Id'>{0}</field><field name='ArchiveId' update='set'>{1}</field></doc></add>", listKhieuNaiSolrInfo[i].Id, archiveId);
                        solrConnection.Post("/update", updateXml);
                        
                    }
                } // end for (int i = 0; i < listKhieuNaiSolrInfo.Count;i++ )

                solr.Commit();
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy khiếu nại từ solr theo khieuNaiId từ DB
        /// </summary>
        /// <param name="khieuNaiId"></param>
        /// <returns></returns>
        private KhieuNaiSolrInfo GetByKhieuNaiId(int khieuNaiId)
        {
            List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo = null;
            string whereClause = string.Format("Id : {0}", khieuNaiId);
            SolrQuery solrQuery = new SolrQuery(whereClause);
            QueryOptions qoKhieuNai = new QueryOptions();
            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", @"*");

            qoKhieuNai.ExtraParams = extraParamKhieuNai;

            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;
            listKhieuNaiSolrInfo = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);

            return listKhieuNaiSolrInfo[0];
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 15/07/2014
        /// Todo : Lấy danh sách khiếu nại từ solr theo danh sách KhieuNaiId từ DB
        /// </summary>
        /// <param name="listKhieuNaiId"></param>
        /// <returns></returns>
        private List<KhieuNaiSolrInfo> GetByListKhieuNaiId(List<int> listKhieuNaiId)
        {
            if (listKhieuNaiId == null || listKhieuNaiId.Count == 0) return null;            

            List<KhieuNaiSolrInfo> listKhieuNaiSolrInfo = null;
            string whereClause = string.Empty;// string.Format("Id : {0}", khieuNaiId);
            for(int i=0;i<listKhieuNaiId.Count;i++)
            {
                whereClause = string.Format("{0}{1} ", whereClause, listKhieuNaiId[i]);
            }

            whereClause = whereClause.TrimEnd();
            whereClause = string.Format("Id : ({0})", whereClause);

            SolrQuery solrQuery = new SolrQuery(whereClause);
            QueryOptions qoKhieuNai = new QueryOptions();
            Dictionary<string, string> extraParamKhieuNai = new Dictionary<string, string>();
            extraParamKhieuNai.Add("fl", @"*");

            qoKhieuNai.ExtraParams = extraParamKhieuNai;

            qoKhieuNai.Start = 0;
            qoKhieuNai.Rows = int.MaxValue;
            listKhieuNaiSolrInfo = QuerySolrBase<KhieuNaiSolrInfo>.QuerySolr(URL_SOLR_GQKN, solrQuery, qoKhieuNai);

            return listKhieuNaiSolrInfo;
        }
    }
}
