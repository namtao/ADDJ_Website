using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace ADDJ.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của PhongBan
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class PhongBanImpl : BaseImpl<PhongBanInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "PhongBan";

            IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";
            MaxFieldLength = 10;
            IsUseLucene = true;
            IsUpdateLucene = true;
        }

        private static Dictionary<int, string> _DicPhongBan = new Dictionary<int,string>();
        public static Dictionary<int, string> DicPhongBan
        {
            get
            {
                if (_DicPhongBan == null)
                {
                    var lst = new PhongBanImpl().GetList();
                    foreach (var item in lst)
                    {
                        _DicPhongBan.Add(item.Id, item.Name);
                    }
                }
                return _DicPhongBan;
            }
            set { _DicPhongBan = value; }
        }

        public string GetNamePhongBan(int Id)
        {
            try
            {
                if (DicPhongBan.ContainsKey(Id))
                {
                    return DicPhongBan[Id];
                }
                else
                {
                    var item = GetInfo(Id);
                    DicPhongBan.Add(item.Id, item.Name);
                    return item.Name;
                }
            }
            catch(Exception ex) {
                Utility.LogEvent(ex);
                return "-1";
            }
        }

        public List<PhongBanInfo> Suggestion(string query)
        {
            string[] fields = new string[] { "Id","Name", "Description" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "* ";
            
            var q = parser.Parse(query);

            return this.Search(q, null, null, 10, false);
        }

        public PhongBanInfo CheckExistsPhongBan(string name)
        {
            string[] fields = new string[] { "Id", "Name" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            //parser.SetDefaultOperator(QueryParser.Operator.OR);

            var query = "Name:" + name;

            var q = parser.Parse(query);

            var result = this.Search(q, 1);
            if (result != null && result.Count > 0)
                return result[0];
            return null;
        }

        public List<PhongBanInfo> SuggestionGetAllList(string query)
        {
            string[] fields = new string[] { "Id", "Name", "Description" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "* ";

            var q = parser.Parse(query);
            List<PhongBanInfo> list = this.Search(q, null, null, int.MaxValue, false);
            var newList = list.OrderBy(x => x.Name).ToList();
            return newList;
        }
		#region Function 
        public List<PhongBanInfo> QLKN_PhongBanGetAll()
        {
            List<PhongBanInfo> lst = new List<PhongBanInfo>();

            try
            {
                lst = this.GetList();
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return lst;
        }
        public PhongBanInfo QLKN_PhongBangetByID(int id)
        {
            PhongBanInfo info = new PhongBanInfo();

            try
            {
                info = this.GetInfo(id);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return info;
        }

        public List<PhongBanInfo> GetListPhongBanByDoiTacId(int doiTacId = -1)
        {
            List<PhongBanInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@DoiTacId", doiTacId),
                                    };
            try
            {
                list = ExecuteQuery("ups_GetListPhongBanByDoiTacId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/11/2013
        /// Todo : Lấy ra tất cả các phòng ban của tất cả các đối tác trực thuộc parentDoiTacId
        /// </summary>
        /// <param name="parentDoiTacId"></param>
        /// <returns></returns>
        public List<PhongBanInfo> GetListPhongBanOfAllDoiTacOfParentDoiTacId(int parentDoiTacId = -1)
        {
            List<PhongBanInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@ParentDoiTacId", parentDoiTacId),
									};
            try
            {
                list = ExecuteQuery("ups_GetListPhongBanByOfAllDoiTacOfParentDoiTacId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 24/01/2014
        /// Todo : Lấy ra tất cả các phòng ban của tất cả các đối tác trực thuộc parentDoiTacId
        /// </summary>
        /// <param name="parentDoiTacId"></param>
        /// <returns></returns>
        public DataTable GetAllPhongBanOfAllDoiTacOfParentDoiTacId(int parentDoiTacId = -1)
        {
            DataTable dtResult = null;
            SqlParameter[] param = {
										new SqlParameter("@ParentDoiTacId", parentDoiTacId),
									};

            string sql = " DECLARE @ParentDoiTacId int = " + parentDoiTacId + ";" +
                         @"  WITH EntityChildren AS
                            (
	                            SELECT *, 1 AS Level  FROM DoiTac WHERE Id = @ParentDoiTacId
	                            UNION ALL
	                            SELECT e.*, Level + 1 FROM DoiTac e INNER JOIN EntityChildren e2 on e.DonViTrucThuoc = e2.Id
                            )
                            SELECT EntityChildren.Id AS DoiTacId, EntityChildren.TenDoiTac, EntityChildren.DonViTrucThuoc, DoiTac.TenDoiTac AS TenDonViTrucThuoc, EntityChildren.Sort AS DoiTacSort,
	                            PhongBan.Id AS PhongBanId, PhongBan.Name AS TenPhongBan,
	                            PhongBan.Sort AS PhongBanSort, Level
                            FROM EntityChildren
	                            LEFT JOIN PhongBan ON EntityChildren.Id = PhongBan.DoiTacId
		                        LEFT JOIN DoiTac ON EntityChildren.DonViTrucThuoc = DoiTac.Id
                            ORDER By DonViTrucThuoc ASC, TenDoiTac ASC";

            SqlCommand cmd = new SqlCommand(sql);

            dtResult = ExecuteQueryToDataSet(cmd).Tables[0];

            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 28/02/2014
        /// Todo : Lấy object PhongBan từ danh sách truyền vào
        /// </summary>
        /// <param name="listPhongBanInfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public PhongBanInfo GetPhongBanByIdFromList(List<PhongBanInfo> listPhongBanInfo, int id)
        {
            if (listPhongBanInfo == null || listPhongBanInfo.Count == 0) return null;

            for(int i = 0;i<listPhongBanInfo.Count;i++)
            {
                if(listPhongBanInfo[i].Id == id)
                {
                    return listPhongBanInfo[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/05/2014
        /// Todo : Lấy ra tất cả các phòng ban của tất cả các đối tác trực thuộc (của giá trị trường) DonViTrucThuocChoBaoCao
        /// </summary>
        /// <param name="parentDoiTacId"></param>
        /// <returns></returns>
        public DataTable GetAllPhongBanOfAllDoiTacOfDonViTrucThuocChoBaoCao(int parentDoiTacId = -1)
        {
            DataTable dtResult = null;
            SqlParameter[] param = {
										new SqlParameter("@ParentDoiTacId", parentDoiTacId),
									};

            string sql = " DECLARE @ParentDoiTacId int = " + parentDoiTacId + ";" +
                         @"  WITH EntityChildren AS
                            (
	                            SELECT *, 1 AS Level  FROM DoiTac WHERE Id = @ParentDoiTacId
	                            UNION ALL
	                            SELECT e.*, Level + 1 FROM DoiTac e INNER JOIN EntityChildren e2 on e.DonViTrucThuocChoBaoCao = e2.Id
                            )
                            SELECT EntityChildren.Id AS DoiTacId, EntityChildren.TenDoiTac, EntityChildren.DonViTrucThuocChoBaoCao, DoiTac.TenDoiTac AS TenDonViTrucThuoc, EntityChildren.Sort AS DoiTacSort,
	                            PhongBan.Id AS PhongBanId, PhongBan.Name AS TenPhongBan,
	                            PhongBan.Sort AS PhongBanSort, Level
                            FROM EntityChildren
	                            LEFT JOIN PhongBan ON EntityChildren.Id = PhongBan.DoiTacId
		                        LEFT JOIN DoiTac ON EntityChildren.DonViTrucThuocChoBaoCao = DoiTac.Id
                            ORDER By DonViTrucThuocChoBaoCao ASC, TenDoiTac ASC";

            SqlCommand cmd = new SqlCommand(sql);

            dtResult = ExecuteQueryToDataSet(cmd).Tables[0];

            return dtResult;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/05/2014
        /// Todo : Lấy danh sách phòng ban của người dùng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<PhongBanInfo> GetListPhongBanByUserId(int userId)
        {
            List<PhongBanInfo> list = null;
            SqlParameter[] param = {
                                        new SqlParameter("@UserId", userId),
                                    };
            try
            {
                string sql = @"SELECT pb.* FROM PhongBan pb
                                INNER JOIN PhongBan_User pbu ON pb.Id = pbu.PhongBanId
                                WHERE NguoiSuDungId = @UserId";
                list = ExecuteQuery(sql, CommandType.Text,  param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 03/06/2014
        /// Todo : Lấy ra tất cả các phòng ban (con cháu) có ParentId thuộc @ParentId
        /// </summary>
        /// <param name="parentDoiTacId"></param>
        /// <returns></returns>
        public List<PhongBanInfo> GetAllPhongBanOfAllOfParentId(int parentId = -1)
        {
            List<PhongBanInfo> listPhongBan = null;

            SqlParameter[] param = {
										new SqlParameter("@ParentId", parentId),
									};

            string sql = @"  WITH EntityChildren AS
                            (
	                            SELECT *, 1 AS Level  FROM PhongBan WHERE Id = @ParentId
	                            UNION ALL
	                            SELECT e.*, Level + 1 FROM PhongBan e INNER JOIN EntityChildren e2 on e.ParentId = e2.Id
                            )
                            SELECT *, Level
                            FROM EntityChildren	                            
                            ORDER By ParentId ASC, Name ASC";

            SqlCommand cmd = new SqlCommand(sql);

            listPhongBan = ExecuteQuery(sql, CommandType.Text, param);

            return listPhongBan;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 03/06/2014
        /// Todo : Lấy ra tất cả các phòng ban (con cháu) có ParentId thuộc @ParentId
        /// </summary>
        /// <param name="parentDoiTacId"></param>
        /// <returns></returns>
        public List<PhongBanInfo> GetAllPhongBanOfAllOfDoiTacId(int doiTacId)
        {
            List<PhongBanInfo> listPhongBan = null;

            SqlParameter[] param = {
										new SqlParameter("@DoiTacid", doiTacId),
									};

            string sql = @"  WITH EntityChildren AS
                            (
	                            SELECT *, 1 AS Level  FROM PhongBan WHERE DoiTacId = @DoiTacId AND Cap = 1
	                            UNION ALL
	                            SELECT e.*, Level + 1 FROM PhongBan e INNER JOIN EntityChildren e2 on e.ParentId = e2.Id
                            )
                            SELECT *, Level
                            FROM EntityChildren	                            
                            --ORDER By ParentId ASC, Name ASC";

            SqlCommand cmd = new SqlCommand(sql);

            listPhongBan = ExecuteQuery(sql, CommandType.Text, param);

            return listPhongBan;
        }

        public List<PhongBanInfo> SortListPhongBanForTree(List<PhongBanInfo> listPhongBanInfo)
        {
            if (listPhongBanInfo == null || listPhongBanInfo.Count == 0) return null;

            string sCap2 = "----";
            string sCap3 = "--------";
            for (int i = 0; i < listPhongBanInfo.Count;i++ )
            {
                if(listPhongBanInfo[i].Cap == 2)
                {
                    listPhongBanInfo[i].Name = string.Format("{0}{1}", sCap2 ,listPhongBanInfo[i].Name);
                }
                else if(listPhongBanInfo[i].Cap==3)
                {
                    listPhongBanInfo[i].Name = string.Format("{0}{1}", sCap3, listPhongBanInfo[i].Name);
                }
            }

            return listPhongBanInfo;
        }

		#endregion
    }
}
