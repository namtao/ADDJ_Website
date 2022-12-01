using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ADDJ.Entity;
using ADDJ.Core;
using ADDJ.Core.Provider;
using System.Linq;
using ADDJ.Admin;

namespace ADDJ.Impl
{
    /// <summary>
    /// Class Insert, Update, Delete, Get dữ liệu của PhongBan_Permission
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>

    public class PhongBan_PermissionImpl : BaseImpl<PhongBan_PermissionInfo>
    {
        protected override void SetInfoDerivedClass()
        {
            TableName = "PhongBan_Permission";
        }

        #region Function

        public Dictionary<int, bool> GetPermission(AdminInfo adminInfo)
        {
            var lstPermission = new PermissionSchemesImpl().GetListDynamic("Id", "", "Sort");
            var lstPermissionPhongBan = this.GetListDynamic("", "PhongBanId=" + adminInfo.PhongBanId, "PermissionSchemeId");
            var lstPermissionUser = new NguoiSuDung_PermissionImpl().GetListDynamic("", "NguoiSuDungId=" + adminInfo.Id, "PermissionSchemeId");

            Dictionary<int, bool> lst = new Dictionary<int, bool>();
            foreach (var item in lstPermission)
            {
                lst.Add(item.Id, false);
            }

            foreach (var permission in lstPermissionPhongBan)
            {
                if (lst.ContainsKey(permission.PermissionSchemeId))
                {
                    lst[permission.PermissionSchemeId] = permission.IsAllow;
                }
            }

            foreach (var permission in lstPermissionUser)
            {
                if (lst.ContainsKey(permission.PermissionSchemeId))
                {
                    bool isAllow = lst[permission.PermissionSchemeId];
                    if (!isAllow)
                    {
                        lst[permission.PermissionSchemeId] = permission.IsAllow;
                    }
                }
            }

            if (adminInfo.LoaiPhongBanId != 0)
            {
                var lstPermissionLoaiPhongBan = new LoaiPhongBan_PermissionImpl().GetListDynamic("", "LoaiPhongBanId=" + adminInfo.LoaiPhongBanId, "PermissionSchemeId");
                var sortPermissionLoaiPhongBan = lstPermissionLoaiPhongBan.Where(t => t.IsAllow == true);
                foreach (var permission in sortPermissionLoaiPhongBan)
                {
                    if (lst.ContainsKey(permission.PermissionSchemeId))
                    {
                        bool isAllow = lst[permission.PermissionSchemeId];
                        if (!isAllow)
                        {
                            lst[permission.PermissionSchemeId] = permission.IsAllow;
                        }
                    }
                }
            }

            //Kiểm tra xem user có nằm trong group nào không
            //var lstGroup = new NhomNguoiDung_AI_DetailImpl().GetListDynamic("", "NguoiSuDungId=" + adminId, "");
            if (adminInfo.ListNhomNguoiDung != null && adminInfo.ListNhomNguoiDung.Count > 0)
            {
                foreach (var item in adminInfo.ListNhomNguoiDung)
                {
                    //Lấy ra quyền của nhóm
                    var lstPermissionNhomNguoiDung = new NhomNguoiDung_AI_PermissionImpl().GetListDynamic("", "NhomNguoiDung_AIId=" + item, "PermissionSchemeId");
                    var sortPermissionNhomNguoiDung = lstPermissionNhomNguoiDung.Where(t => t.IsAllow == true);
                    foreach (var permission in sortPermissionNhomNguoiDung)
                    {
                        if (lst.ContainsKey(permission.PermissionSchemeId))
                        {
                            bool isAllow = lst[permission.PermissionSchemeId];
                            if (!isAllow)
                            {
                                lst[permission.PermissionSchemeId] = permission.IsAllow;
                            }
                        }
                    }
                }
            }
            return lst;
        }

        public List<PhongBan_PermissionInfo> GetListByPermissionSchemeId(int _permissionSchemeId)
        {
            List<PhongBan_PermissionInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
									};
            try
            {
                list = ExecuteQuery("usp_PhongBan_Permission_GetListByPermissionSchemeId", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }


        public List<PhongBan_PermissionInfo> GetListByPhongBanId(int _phongBanId)
        {
            List<PhongBan_PermissionInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@PhongBanId",_phongBanId),
									};
            try
            {
                list = ExecuteQuery("usp_PhongBan_Permission_GetListByPhongBanId", param);
            }
            catch
            {
                throw;
            }
            return list;

        }

        public List<PhongBan_PermissionInfo> GetListByAllRef(int _permissionSchemeId, int _phongBanId)
        {
            List<PhongBan_PermissionInfo> list = null;
            SqlParameter[] param = {
										new SqlParameter("@PermissionSchemeId",_permissionSchemeId),
										new SqlParameter("@PhongBanId",_phongBanId),
									};
            try
            {
                list = ExecuteQuery("usp_PhongBan_Permission_GetListAllRef", param);
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
            }
            return list;

        }





        #endregion
    }

    [Serializable]
    public class TestLengDic
    {
        public Dictionary<int, bool> RightPermission { get; set; }
    }
}
