using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.News.Entity;
using ADDJ.Core;
using System.Linq;
using ADDJ.Core.Provider;

namespace ADDJ.News.Impl
{
	/// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của Category
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/07/2012</date>
	
	public class CategoryImpl : BaseImpl<CategoryInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "Category";
        }
		
		#region Function 

        public List<CategoryInfo> GetListCategorySortParent()
        {
            return GetListCategorySortParent(string.Empty, null, "__");
        }

        public List<CategoryInfo> GetListCategorySortParent(string replaceSpace)
        {
            return GetListCategorySortParent(string.Empty, null, replaceSpace);
        }

        public List<CategoryInfo> GetListCategorySortParent(string whereClause, string replaceSpace)
        {
            return GetListCategorySortParent(whereClause, null, replaceSpace);
        }

        public List<CategoryInfo> GetListCategorySortParent(string whereClause, CategoryInfo infoStart, string replaceSpace)
        {
            List<CategoryInfo> lstReturn = new List<CategoryInfo>();
                        
            if(infoStart != null)
                lstReturn.Add(infoStart);

            var lst = this.GetListDynamic("", whereClause, "");
            if(lst != null && lst.Count>0)
                lstReturn = BuildTree(lst, 0, 0, replaceSpace);

            return lstReturn;
        }

        private List<CategoryInfo> BuildTree(List<CategoryInfo> lst, int parent, int cap, string replaceSpace)
        {
            List<CategoryInfo> lstRet = new List<CategoryInfo>();
            
            var lstPar = lst.Where(t => t.ParentId == parent);
            string strSpace = string.Empty;

            for (int i = 0; i < cap; i++)
                strSpace += replaceSpace;

            foreach (var item in lstPar)
            {
                item.Name = strSpace + item.Name;
                lstRet.Add(item);
                var q = BuildTree(lst, item.Id, cap + 1, replaceSpace);
                if (q.Count > 0)
                {
                    foreach (var i in q)
                    {
                        i.Name = strSpace + i.Name;
                        lstRet.Add(i);
                    }
                }
            }
            return lstRet;
        }
		
		#endregion
    }
}
