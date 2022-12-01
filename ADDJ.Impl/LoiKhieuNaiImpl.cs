using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ADDJ.Core.Provider;
using ADDJ.Entity;
using System.Data.SqlClient;
using System.Data;

namespace ADDJ.Impl
{
    public class LoiKhieuNaiImpl : BaseImpl<LoiKhieuNaiInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "LoiKhieuNai";            
        }

        public LoiKhieuNaiImpl(): base()
        {

        }

        public List<LoiKhieuNaiInfo> GetNguyenNhanLoiAndChiTietLoi(int nguyenNhanLoiId, int chiTietLoiId)
        {
            List<LoiKhieuNaiInfo> listLoiKhieuNai = null;
            string sql = @"SELECT * FROM LoiKhieuNai WHERE Id IN(@NguyenNhanLoiId, @ChiTietLoiId)";
            SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@NguyenNhanLoiId", nguyenNhanLoiId),
                new SqlParameter("@ChiTietLoiId", chiTietLoiId)
            };

            listLoiKhieuNai = ExecuteQuery(sql, CommandType.Text, param);

            return listLoiKhieuNai;
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created : 20/09/2014
        /// Todo : Lấy ra danh sách lỗi khiếu nại được sắp xếp theo cây thư mục cha con
        ///        
        /// </summary>       
        /// <returns></returns>
        public List<LoiKhieuNaiInfo> GetListSortHierarchy()
        {
            List<LoiKhieuNaiInfo> listResult = new List<LoiKhieuNaiInfo>();
            List<LoiKhieuNaiInfo> listParent = this.GetListDynamic("*", "ParentId=0", "ThuTu ASC");
            if (listParent == null) return null;

            List<LoiKhieuNaiInfo> listKhieuNaiAll = this.GetList();
            if (listKhieuNaiAll != null)
            {
                for (int i = 0; i < listParent.Count; i++)
                {
                    listResult.Add(listParent[i]);

                    for (int j = 0; j < listKhieuNaiAll.Count; j++)
                    {
                        if (listParent[i].Id == listKhieuNaiAll[j].ParentId)
                        {
                            listResult.Add(listKhieuNaiAll[j]);

                            //for (int k = 0; k < listKhieuNaiAll.Count; k++)
                            //{
                            //    if (listKhieuNaiAll[j].Id == listKhieuNaiAll[k].ParentId)
                            //    {
                            //        listResult.Add(listKhieuNaiAll[k]);
                            //    }
                            //} // end for(int k=0;k<listKhieuNaiAll.Count;k++)
                        }
                    } // end  for(int j=0;j<listKhieuNaiAll.Count;j++)
                } // end for(int i=0;i<listParent.Count;i++)
            } // end if(listKhieuNaiAll != null)          

            return listResult;
        }
    }
}
