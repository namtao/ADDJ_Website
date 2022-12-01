using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using System.Linq;
using ADDJ.Core.Provider;
using System.Web;
using System.IO;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis.Standard;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của Province
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>21/08/2013</date>

    public class ProvinceImpl : BaseImpl<ProvinceInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "Province";
            IndexLocation = Path.Combine(Config.PathIndexLucene, TableName) + @"\";
            MaxFieldLength = 10;
            IsUseLucene = true;
            IsUpdateLucene = true;
        }
        public List<ProvinceInfo> Suggestion(string query)
        {
            string[] fields = new string[] { "Id", "Name", "ParentId" };

            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);

            query = query + "*";

            var q = parser.Parse(query);

            return this.Search(q, null, null, 10, false);
        }

        public List<ProvinceInfo> SuggestionGetListByListLevelNbr(string query, string lstParentId)
        {
            string[] fields = new string[] { "Id", "Name", "ParentId", "LevelNbr" };
            string[] strnum = lstParentId.Split(',');
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            List<ProvinceInfo> list = new List<ProvinceInfo>();
            if (lstParentId != "1")
            {
                //query = string.Format("Id:{0} OR ParentId:{0}", lstParentId);
                query = string.Format("Id:{0}", lstParentId);
                var q = parser.Parse(query);
                list = Search(q, null, null, int.MaxValue, false);
            }
            else
            {
                query = query + "*";
                var q = parser.Parse(query);
                list = Search(q, null, null, int.MaxValue, false);
                list = (from a in list where strnum.Contains(a.LevelNbr.ToString()) select a).OrderBy(x => x.Name).ToList();
            }
            
            
            
            //list = (from a in list where strnum.Contains(a.LevelNbr.ToString()) select a).OrderBy(x => x.Name).ToList();
            return list;
        }

        public List<ProvinceInfo> SuggestionGetListByListParent(string query, string lstParentId)
        {
            string[] fields = new string[] { "Id", "Name", "ParentId", "LevelNbr" };
            string[] strnum = lstParentId.Split(',');
            var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_29, fields, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29));
            parser.SetAllowLeadingWildcard(true);
            parser.SetDefaultOperator(QueryParser.Operator.OR);
            query = query + "*";
            var q = parser.Parse(query);
            List<ProvinceInfo> list = Search(q, null, null, int.MaxValue, false);
            list = (from a in list where strnum.Contains(a.ParentId.ToString()) select a).OrderBy(x => x.Name).ToList();
            return list;
        }

        private static List<ProvinceInfo> _ListProvince;
        public static List<ProvinceInfo> ListProvince
        {
            get
            {
                if (_ListProvince == null)
                    _ListProvince = new ProvinceImpl().GetList();
                return _ListProvince;
            }
            set { _ListProvince = value; }
        }

        public static List<ProvinceInfo> BuildTree(List<ProvinceInfo> lst, int pId, int cap, string replaceSpace)
        {
            List<ProvinceInfo> lstRet = new List<ProvinceInfo>();
            IEnumerable<ProvinceInfo> lstPar = null;
            if(pId ==0)
                lstPar = lst.Where(t => t.ParentId == 0);
            else
                lstPar = lst.Where(t => t.ParentId == pId);
            string strSpace = string.Empty;

            for (int i = 0; i < cap; i++)
                strSpace += replaceSpace;

            foreach (var item in lstPar)
            {
                //item.AbbRev = HttpUtility.HtmlDecode(strSpace + item.Name);
                item.Name = HttpUtility.HtmlDecode(strSpace + item.Name);
                lstRet.Add(item);
                var q = BuildTree(lst, item.Id, cap + 1, replaceSpace);
                if (q.Count > 0)
                {
                    foreach (var i in q)
                    {
                        //i.AbbRev = i.AbbRev;
                        lstRet.Add(i);
                    }
                }
            }
            return lstRet;
        }

        #region Function

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 13/10/2015
        /// Todo : Lấy danh sách Tỉnh/Quận/Phường theo cấp level
        /// </summary>
        /// <param name="level">
        ///     = 1 : Lấy ra danh sách tỉnh
        ///     = 2 : Lấy danh sách tỉnh + quận
        ///     = 3 (hoặc các giá trị khác) : Lấy danh sách tỉnh + quận + phường
        /// </param>
        /// <returns></returns>
        public List<ProvinceInfo> ListProvinceOrderByName(int level)
        {
            List<ProvinceInfo> listProvinceResult = new List<ProvinceInfo>();
            List<ProvinceInfo> listProvinceLevel1 = this.GetListDynamic("*", "LevelNbr=1", "Name ASC");

            if(level == 1)
            {
                return listProvinceLevel1;
            }

            List<ProvinceInfo> listProvinceAll = this.GetListDynamic("*", "LevelNbr > 1", "Name ASC");

            if(listProvinceLevel1 != null)
            {
                for(int i=0;i<listProvinceLevel1.Count;i++)
                {
                    listProvinceResult.Add(listProvinceLevel1[i]);
                    for(int j=0;j<listProvinceAll.Count;j++)
                    {
                        if(listProvinceAll[j].ParentId == listProvinceLevel1[i].Id)
                        {                            
                            listProvinceResult.Add(listProvinceAll[j]);

                            if(level == 3)
                            {
                                for (int k = j; k < listProvinceAll.Count; k++)
                                {
                                    if (listProvinceAll[k].ParentId == listProvinceAll[j].Id)
                                    {
                                        listProvinceResult.Add(listProvinceAll[k]);
                                    }
                                }
                            } // end if(level == 3)
                        } // end if(listProvinceAll[j].ParentId == listProvinceLevel1[i].Id)
                    } // end for(int j=0;j<listProvinceAll.Count;j++)
                } // end for(int i=0;i<listProvinceLevel1.Count;i++)
            } // end if(listProvinceLevel1 != null)


            return listProvinceResult;
        }

        #endregion
    }
}
