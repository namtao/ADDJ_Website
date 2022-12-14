using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADDJ.Core;

namespace ADDJ.Admin
{
    [Serializable]
    public class AdminInfo
    {
        #region Members
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        public string Phone { get; set; }
        public Int16 Status { get; set; }
        public Int16 IsLogin { get; set; }
        public int DoiTacId { get; set; }
        public int KhuVucId { get; set; }
        public int NhomNguoiDung { get; set; }
        public int PhongBanId { get; set; }
        public int LoaiPhongBanId { get; set; }
        public int LoginCount { get; set; }
        public Int16 DefaultHTTN { get; set; }
        public bool IsChuyenTiepKN { get; set; }
        public DateTime LastLogin { get; set; }

        public bool IsChuyenVNP { get; set; }
        public List<int> ListNhomNguoiDung { get; set; }

        public int CapPhongBan { get; set; }

        public int DoiTacType { get; set; }

        // Nếu IsDaBuTienAuto : true : Khi nhập số tiền giảm trừ thì tự động xác nhận luôn
        //                      false : Phải trải qua 1 bước xác nhận
        public bool IsDaBuTienAuto { get; set; }

        public Decimal GioiHanGiamTruMax { get; set; }
        public Decimal GioiHanGiamTruMin { get; set; }
        public int XOA { get; set; }

        #endregion

        #region Constructor
        public AdminInfo()
        {
            this.Id = Utility.InitializeInteger;
            this.Username = Utility.InitializeString;
            this.Password = Utility.InitializeString;
            this.FullName = Utility.InitializeString;
            this.Phone = string.Empty;
            this.Status = 0;
            this.IsLogin = 0;
            this.DoiTacId = 0;
            this.PhongBanId = 0;
            this.LoaiPhongBanId = 0;
            this.NhomNguoiDung = 0;
            this.LoginCount = 0;
            this.LastLogin = DateTime.Now;
            DefaultHTTN = 0;
            IsChuyenVNP = false;

            IsDaBuTienAuto = false;
            GioiHanGiamTruMax = 0;
            GioiHanGiamTruMin = 0;
            XOA = 0;
        }
        #endregion
    }

    public class GroupAdminInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
    }

    public class GroupAdminDetailInfo
    {
        public GroupAdminDetailInfo()
        {
            Id = 0;
            Active = 0;
        }
        public int Id { get; set; }
        public int GroupAdminId { get; set; }
        public int AdminId { get; set; }
        public int Active { get; set; }

    }

    public class UserRightGroupInfo
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int GroupAdminID { get; set; }
        public bool UserRead { get; set; }
        public bool UserEdit { get; set; }
        public bool UserDelete { get; set; }
        public bool Other1 { get; set; }
        public bool Other2 { get; set; }
        public bool Other3 { get; set; }
        public bool Other4 { get; set; }

        public string Name { get; set; }
    }

    public class MenuInfo
    {
        #region Members
        public int ID { get; set; }
        public int STT { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public string Link { get; set; }
        public int Display { get; set; }
        public int MenuType { get; set; }

        public bool UserRead { get; set; }
        public bool UserEdit { get; set; }
        public bool UserDelete { get; set; }
        public bool Other1 { get; set; }
        public bool Other2 { get; set; }
        public bool Other3 { get; set; }
        public bool Other4 { get; set; }
        #endregion

        #region Constructor
        public MenuInfo()
        {
            this.ID = Utility.InitializeInteger;
            this.STT = Utility.InitializeInteger;
            this.ParentID = Utility.InitializeInteger;
            this.Name = Utility.InitializeString;
            this.Name2 = Utility.InitializeString;
            this.Name3 = Utility.InitializeString;
            this.Link = Utility.InitializeString;
            this.Display = Utility.InitializeInteger;
            this.MenuType = Utility.InitializeInteger;
        }
        #endregion

    }

    public class RightInfo
    {
        public bool UserRead { get; set; }
        public bool UserEdit { get; set; }
        public bool UserDelete { get; set; }
        public bool Other1 { get; set; }
        public bool Other2 { get; set; }
        public bool Other3 { get; set; }
        public bool Other4 { get; set; }

        public RightInfo()
        {
            this.UserRead = false;
            this.UserEdit = false;
            this.UserDelete = false;
            this.Other1 = false;
            this.Other2 = false;
            this.Other3 = false;
            this.Other4 = false;
        }
    }

    public class UserRightInfo
    {
        #region Members
        public int ID { get; set; }
        public int AdminID { get; set; }
        public bool UserRead { get; set; }
        public bool UserEdit { get; set; }
        public bool UserDelete { get; set; }
        public bool Other1 { get; set; }
        public bool Other2 { get; set; }
        public bool Other3 { get; set; }
        public bool Other4 { get; set; }

        public int MenuID { get; set; }
        public int Menu_STT { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_Url { get; set; }
        public int Menu_ParentId { get; set; }
        public int Menu_Display { get; set; }
        #endregion

        #region Constructor
        public UserRightInfo()
        {
            this.ID = Utility.InitializeInteger;
            this.AdminID = Utility.InitializeInteger;
            this.UserRead = Utility.InitializeBool;
            this.UserEdit = Utility.InitializeBool;
            this.UserDelete = Utility.InitializeBool;
            this.Other1 = false;
            this.Other2 = false;
            this.Other3 = false;
            this.Other4 = false;
        }
        #endregion

    }

    public class FieldNames
    {
        /// <summary>
        /// Field names of table Admin.
        /// </summary>
        public class Admin
        {
            public const string ID = "ID";
            public const string Username = "Username";
            public const string Password = "Password";
            public const string FullName = "FullName";
            public const string Status = "Status";
            public const string IsLogin = "IsLogin";
        }

        /// <summary>
        /// Field names of table Menu.
        /// </summary>
        public class Menu
        {
            public const string ID = "ID";
            public const string STT = "STT";
            public const string ParentID = "ParentID";
            public const string Name = "Name";
            public const string Name2 = "Name2";
            public const string Name3 = "Name3";
            public const string Link = "Link";
        }

        /// <summary>
        /// Field names of table UserRight.
        /// </summary>
        public class UserRight
        {
            public const string ID = "ID";
            public const string MenuID = "MenuID";
            public const string AdminID = "AdminID";
            public const string UserRead = "UserRead";
            public const string UserEdit = "UserEdit";
            public const string UserDelete = "UserDelete";
            public const string Other1 = "Other1";
            public const string Other2 = "Other2";
            public const string Other3 = "Other3";
            public const string Other4 = "Other4";
        }

        public class GroupAdmin
        {
            public const string Id = "Id";
            public const string Name = "Name";
            public const string Description = "Description";
            public const string Status = "Status";
        }

        public class GroupAdminDetail
        {
            public const string Id = "Id";
            public const string GroupAdminId = "GroupAdminId";
            public const string AdminId = "AdminId";
            public const string Active = "Active";
        }

        public class UserRightGroup
        {
            public const string Id = "Id";
            public const string MenuID = "MenuID";
            public const string GroupAdminID = "GroupAdminID";
            public const string UserRead = "UserRead";
            public const string UserEdit = "UserEdit";
            public const string UserDelete = "UserDelete";
            public const string Other1 = "Other1";
            public const string Other2 = "Other2";
            public const string Other3 = "Other3";
            public const string Other4 = "Other4";
        }

    }
}
