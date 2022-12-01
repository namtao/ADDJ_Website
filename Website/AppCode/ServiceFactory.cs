using ADDJ.Admin;
using ADDJ.Impl;
using ADDJ.News.Impl;
using ADDJ.Log.Impl;

namespace Website.AppCode
{
	/// <summary>
    /// Class khoi tao Instance Impl
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	public class ServiceFactory
    {
		private static MenuImpl _iMenu;
        public static MenuImpl GetInstanceMenu()
        {
            return _iMenu ?? (_iMenu = new MenuImpl());
        }
        private static CategoryImpl _Category;
        public static CategoryImpl GetInstanceCategory()
        {
            return _Category ?? (_Category = new CategoryImpl());
        }

        private static NewsImpl _News;
        public static NewsImpl GetInstanceNews()
        {
            return _News ?? (_News = new NewsImpl());
        }

        private static LogImpl _logImpl;
        public static LogImpl GetInstanceLog()
        {
            return _logImpl ?? (_logImpl = new LogImpl());
        }
		
		private static DoiTacImpl _DoiTac;
		public static DoiTacImpl GetInstanceDoiTac()
		{
			return _DoiTac ?? (_DoiTac = new DoiTacImpl());
		}
		private static KhieuNaiImpl _KhieuNai;
		public static KhieuNaiImpl GetInstanceKhieuNai()
		{
			return _KhieuNai ?? (_KhieuNai = new KhieuNaiImpl());
		}


		private static KhieuNai_ActivityImpl _KhieuNai_Activity;
		public static KhieuNai_ActivityImpl GetInstanceKhieuNai_Activity()
		{
			return _KhieuNai_Activity ?? (_KhieuNai_Activity = new KhieuNai_ActivityImpl());
		}
		private static KhieuNai_BuocXuLyImpl _KhieuNai_BuocXuLy;
		public static KhieuNai_BuocXuLyImpl GetInstanceKhieuNai_BuocXuLy()
		{
			return _KhieuNai_BuocXuLy ?? (_KhieuNai_BuocXuLy = new KhieuNai_BuocXuLyImpl());
		}
		private static KhieuNai_FileDinhKemImpl _KhieuNai_FileDinhKem;
		public static KhieuNai_FileDinhKemImpl GetInstanceKhieuNai_FileDinhKem()
		{
			return _KhieuNai_FileDinhKem ?? (_KhieuNai_FileDinhKem = new KhieuNai_FileDinhKemImpl());
		}
		private static KhieuNai_GiaiPhapImpl _KhieuNai_GiaiPhap;
		public static KhieuNai_GiaiPhapImpl GetInstanceKhieuNai_GiaiPhap()
		{
			return _KhieuNai_GiaiPhap ?? (_KhieuNai_GiaiPhap = new KhieuNai_GiaiPhapImpl());
		}
		private static KhieuNai_LogImpl _KhieuNai_Log;
		public static KhieuNai_LogImpl GetInstanceKhieuNai_Log()
		{
			return _KhieuNai_Log ?? (_KhieuNai_Log = new KhieuNai_LogImpl());
		}
		private static KhieuNai_WatchersImpl _KhieuNai_Watchers;
		public static KhieuNai_WatchersImpl GetInstanceKhieuNai_Watchers()
		{
			return _KhieuNai_Watchers ?? (_KhieuNai_Watchers = new KhieuNai_WatchersImpl());
		}

        private static KhieuNai_SoTienImpl _KhieuNai_SoTienImpl;
        public static KhieuNai_SoTienImpl GetInstanceKhieuNai_SoTien()
        {
            return _KhieuNai_SoTienImpl ?? (_KhieuNai_SoTienImpl = new KhieuNai_SoTienImpl());
        }
		private static LoaiKhieuNaiImpl _LoaiKhieuNai;
		public static LoaiKhieuNaiImpl GetInstanceLoaiKhieuNai()
		{
			return _LoaiKhieuNai ?? (_LoaiKhieuNai = new LoaiKhieuNaiImpl());
		}
		private static LoaiKhieuNai2PhongBanImpl _LoaiKhieuNai2PhongBan;
		public static LoaiKhieuNai2PhongBanImpl GetInstanceLoaiKhieuNai2PhongBan()
		{
			return _LoaiKhieuNai2PhongBan ?? (_LoaiKhieuNai2PhongBan = new LoaiKhieuNai2PhongBanImpl());
		}
        private static LoaiPhongBanImpl _LoaiPhongBan;
        public static LoaiPhongBanImpl GetInstanceLoaiPhongBan()
        {
            return _LoaiPhongBan ?? (_LoaiPhongBan = new LoaiPhongBanImpl());
        }
        private static LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl _LoaiPhongBan_ThoiGianXuLyKhieuNai;
        public static LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl GetInstanceLoaiPhongBan_ThoiGianXuLyKhieuNai()
        {
            return _LoaiPhongBan_ThoiGianXuLyKhieuNai ?? (_LoaiPhongBan_ThoiGianXuLyKhieuNai = new LoaiPhongBan_ThoiGianXuLyKhieuNaiImpl());
        }
		private static NguoiSuDungImpl _NguoiSuDung;
		public static NguoiSuDungImpl GetInstanceNguoiSuDung()
		{
			return _NguoiSuDung ?? (_NguoiSuDung = new NguoiSuDungImpl());
		}
		private static NguoiSuDung_GroupImpl _NguoiSuDung_Group;
		public static NguoiSuDung_GroupImpl GetInstanceNguoiSuDung_Group()
		{
			return _NguoiSuDung_Group ?? (_NguoiSuDung_Group = new NguoiSuDung_GroupImpl());
		}
		private static NguoiSuDung_GroupDetailImpl _NguoiSuDung_GroupDetail;
		public static NguoiSuDung_GroupDetailImpl GetInstanceNguoiSuDung_GroupDetail()
		{
			return _NguoiSuDung_GroupDetail ?? (_NguoiSuDung_GroupDetail = new NguoiSuDung_GroupDetailImpl());
		}
		private static PermissionSchemesImpl _PermissionSchemes;
		public static PermissionSchemesImpl GetInstancePermissionSchemes()
		{
			return _PermissionSchemes ?? (_PermissionSchemes = new PermissionSchemesImpl());
		}
		private static PhongBanImpl _PhongBan;
		public static PhongBanImpl GetInstancePhongBan()
		{
			return _PhongBan ?? (_PhongBan = new PhongBanImpl());
		}
		private static PhongBan_PermissionImpl _PhongBan_Permission;
		public static PhongBan_PermissionImpl GetInstancePhongBan_Permission()
		{
			return _PhongBan_Permission ?? (_PhongBan_Permission = new PhongBan_PermissionImpl());
		}
		private static PhongBan_UserImpl _PhongBan_User;
		public static PhongBan_UserImpl GetInstancePhongBan_User()
		{
			return _PhongBan_User ?? (_PhongBan_User = new PhongBan_UserImpl());
		}
		private static PhongBan2PhongBanImpl _PhongBan2PhongBan;
		public static PhongBan2PhongBanImpl GetInstancePhongBan2PhongBan()
		{
			return _PhongBan2PhongBan ?? (_PhongBan2PhongBan = new PhongBan2PhongBanImpl());
		}
		private static TinhImpl _Tinh;
		public static TinhImpl GetInstanceTinh()
		{
			return _Tinh ?? (_Tinh = new TinhImpl());
		}
        private static KhieuNai_KetQuaXuLyImpl _KhieuNai_KetQuaXuLy;
        public static KhieuNai_KetQuaXuLyImpl GetInstanceKhieuNai_KetQuaXuLy()
        {
            return _KhieuNai_KetQuaXuLy ?? (_KhieuNai_KetQuaXuLy = new KhieuNai_KetQuaXuLyImpl());
        }
        private static ProvinceImpl _Province;
        public static ProvinceImpl GetInstanceProvince()
        {
            return _Province ?? (_Province = new ProvinceImpl());
        }

        private static DauSoCPImpl _DauSoCP;
        public static DauSoCPImpl GetInstanceDauSoCP()
        {
            return _DauSoCP ?? (_DauSoCP = new DauSoCPImpl());
        }

        private static GetDataImpl _GetData;
        public static GetDataImpl GetInstanceGetData()
        {
            return _GetData ?? (_GetData = new GetDataImpl());
        }

        private static ConfigurationTimeImpl _ConfigurationTime;
        public static ConfigurationTimeImpl ConfigurationTime()
        {
            return _ConfigurationTime ?? (_ConfigurationTime = new ConfigurationTimeImpl());
        }

        private static NguoiSuDung_PermissionImpl _NguoiSuDung_Permission;
        public static NguoiSuDung_PermissionImpl GetInstanceNguoiSuDung_Permission()
        {
            return _NguoiSuDung_Permission ?? (_NguoiSuDung_Permission = new NguoiSuDung_PermissionImpl());
        }

        private static PermissionDefaultImpl _PermissionDefault;
        public static PermissionDefaultImpl GetInstancePermissionDefault()
        {
            return _PermissionDefault ?? (_PermissionDefault = new PermissionDefaultImpl());
        }

        private static LichSuTruyVanImpl _LichSuTruyVan;
        public static LichSuTruyVanImpl GetInstanceLichSuTruyVan()
        {
            return _LichSuTruyVan ?? (_LichSuTruyVan = new LichSuTruyVanImpl());
        }

        private static ConfigurationSystemImpl _ConfigurationSystem;
        public static ConfigurationSystemImpl GetInstanceConfigurationSystem()
        {
            return _ConfigurationSystem ?? (_ConfigurationSystem = new ConfigurationSystemImpl());
        }

        private static ArchiveConfigImpl _ArchiveConfig;
        public static ArchiveConfigImpl GetInstanceArchiveConfig()
        {
            return _ArchiveConfig ?? (_ArchiveConfig = new ArchiveConfigImpl());
        }

        private static CongTyDoiTacImpl _CongTyDoiTac;
        public static CongTyDoiTacImpl GetInstanceCongTyDoiTac()
        {
            return _CongTyDoiTac ?? (_CongTyDoiTac = new CongTyDoiTacImpl());
        }

        private static LichSuPhanViecImpl _LichSuPhanViec;
        public static LichSuPhanViecImpl GetInstanceLichSuPhanViec()
        {
            return _LichSuPhanViec ?? (_LichSuPhanViec = new LichSuPhanViecImpl());
        }

        private static ThongKeKhieuNaiImpl _ThongKeKhieuNai;
        public static ThongKeKhieuNaiImpl GetInstanceThongKeKhieuNai()
        {
            return _ThongKeKhieuNai ?? (_ThongKeKhieuNai = new ThongKeKhieuNaiImpl());
        }

        private static ConfigColumnImpl _ConfigColumn;
        public static ConfigColumnImpl GetInstanceConfigColumn()
        {
            return _ConfigColumn ?? (_ConfigColumn = new ConfigColumnImpl());
        }

        private static ConfigDisplayColumnImpl _ConfigDisplayColumn;
        public static ConfigDisplayColumnImpl GetInstanceConfigDisplayColumn()
        {
            return _ConfigDisplayColumn ?? (_ConfigDisplayColumn = new ConfigDisplayColumnImpl());
        }

        private static DichVuCPImpl _DichVuCP;
        public static DichVuCPImpl GetInstanceDichVuCP()
        {
            return _DichVuCP ?? (_DichVuCP = new DichVuCPImpl());
        }

        private static ThongBaoImpl _ThongBao;
        public static ThongBaoImpl GetInstanceThongBao()
        {
            return _ThongBao ?? (_ThongBao = new ThongBaoImpl());
        }

        private static LoaiPhongBan_PermissionImpl _LoaiPhongBan_Permission;
        public static LoaiPhongBan_PermissionImpl GetInstanceLoaiPhongBan_Permission()
        {
            return _LoaiPhongBan_Permission ?? (_LoaiPhongBan_Permission = new LoaiPhongBan_PermissionImpl());
        }

        private static NhomNguoiDung_AIImpl _NhomNguoiDung_AI;
        public static NhomNguoiDung_AIImpl GetInstanceNhomNguoiDung_AI()
        {
            return _NhomNguoiDung_AI ?? (_NhomNguoiDung_AI = new NhomNguoiDung_AIImpl());
        }

        private static NhomNguoiDung_AI_DetailImpl _NhomNguoiDung_AI_Detail;
        public static NhomNguoiDung_AI_DetailImpl GetInstanceNhomNguoiDung_AI_Detail()
        {
            return _NhomNguoiDung_AI_Detail ?? (_NhomNguoiDung_AI_Detail = new NhomNguoiDung_AI_DetailImpl());
        }

        private static NhomNguoiDung_AI_PermissionImpl _NhomNguoiDung_AI_Permission;
        public static NhomNguoiDung_AI_PermissionImpl GetInstanceNhomNguoiDung_AI_Permission()
        {
            return _NhomNguoiDung_AI_Permission ?? (_NhomNguoiDung_AI_Permission = new NhomNguoiDung_AI_PermissionImpl());
        }

        private static NhomNguoiDung_AI_UserRightImpl _NhomNguoiDung_AI_UserRight;
        public static NhomNguoiDung_AI_UserRightImpl GetInstanceNhomNguoiDung_AI_UserRight()
        {
            return _NhomNguoiDung_AI_UserRight ?? (_NhomNguoiDung_AI_UserRight = new NhomNguoiDung_AI_UserRightImpl());
        }

        private static LoaiKhieuNai_VASUpdateImpl _LoaiKhieuNai_VASUpdate;
        public static LoaiKhieuNai_VASUpdateImpl GetInstanceLoaiKhieuNai_VASUpdate()
        {
            return _LoaiKhieuNai_VASUpdate ?? (_LoaiKhieuNai_VASUpdate = new LoaiKhieuNai_VASUpdateImpl());
        }

        private static KhieuNai_BuocXuLy_AutocompleteImpl _KhieuNai_BuocXuLy_Autocomplete;
        public static KhieuNai_BuocXuLy_AutocompleteImpl GetInstanceKhieuNai_BuocXuLy_Autocomplete()
        {
            return _KhieuNai_BuocXuLy_Autocomplete ?? (_KhieuNai_BuocXuLy_Autocomplete = new KhieuNai_BuocXuLy_AutocompleteImpl());
        }

        private static LoaiKhieuNai_NhomImpl _LoaiKhieuNaiNhom;
        public static LoaiKhieuNai_NhomImpl GetInstanceLoaiKhieuNaiNhom()
        {
            return _LoaiKhieuNaiNhom ?? (_LoaiKhieuNaiNhom = new LoaiKhieuNai_NhomImpl());
        }

        private static LoiKhieuNaiImpl _LoiKhieuNai;
        public static LoiKhieuNaiImpl GetInstanceLoiKhieuNai()
        {
            return _LoiKhieuNai ?? (_LoiKhieuNai = new LoiKhieuNaiImpl());
        }

        private static NguoiSuDung_GioiHanGiamTruImpl _NguoiSuDung_GioiHanGiamTru;
        public static NguoiSuDung_GioiHanGiamTruImpl GetInstanceNguoiSuDung_GioiHanGiamTru()
        {
            return _NguoiSuDung_GioiHanGiamTru ?? (_NguoiSuDung_GioiHanGiamTru = new NguoiSuDung_GioiHanGiamTruImpl());
        }

        #region Archive
        public static KhieuNaiImpl GetInstanceKhieuNai(int ArchiveId)
        {
            KhieuNaiImpl item = null;
            if (ArchiveId > 0 && ArchiveConfigImpl.ListArchive.ContainsKey(ArchiveId))
            {
                item = new KhieuNaiImpl(ArchiveConfigImpl.ListArchive[ArchiveId].ConnectionString);
            }
            else
            {
                item = GetInstanceKhieuNai();
            }
            return item;
        }

        public static KhieuNai_BuocXuLyImpl GetInstanceKhieuNai_BuocXuLy(int ArchiveId)
        {
            KhieuNai_BuocXuLyImpl item = null;
            if (ArchiveId > 0 && ArchiveConfigImpl.ListArchive.ContainsKey(ArchiveId))
            {
                item = new KhieuNai_BuocXuLyImpl(ArchiveConfigImpl.ListArchive[ArchiveId].ConnectionString);
            }
            else
            {
                item = GetInstanceKhieuNai_BuocXuLy();
            }
            return item;
        }

        public static KhieuNai_ActivityImpl GetInstanceKhieuNai_Activity(int ArchiveId)
        {
            KhieuNai_ActivityImpl item = null;
            if (ArchiveId > 0 && ArchiveConfigImpl.ListArchive.ContainsKey(ArchiveId))
            {
                item = new KhieuNai_ActivityImpl(ArchiveConfigImpl.ListArchive[ArchiveId].ConnectionString);
            }
            else
            {
                item = GetInstanceKhieuNai_Activity();
            }
            return item;
        }
        public static KhieuNai_FileDinhKemImpl GetInstanceKhieuNai_FileDinhKem(int ArchiveId)
        {
            KhieuNai_FileDinhKemImpl item = null;
            if (ArchiveId > 0 && ArchiveConfigImpl.ListArchive.ContainsKey(ArchiveId))
            {
                item = new KhieuNai_FileDinhKemImpl(ArchiveConfigImpl.ListArchive[ArchiveId].ConnectionString);
            }
            else
            {
                item = GetInstanceKhieuNai_FileDinhKem();
            }
            return item;
        }
        

        public static KhieuNai_SoTienImpl GetInstanceKhieuNai_SoTien(int ArchiveId)
        {
            KhieuNai_SoTienImpl item = null;
            if (ArchiveId > 0 && ArchiveConfigImpl.ListArchive.ContainsKey(ArchiveId))
            {
                item = new KhieuNai_SoTienImpl(ArchiveConfigImpl.ListArchive[ArchiveId].ConnectionString);
            }
            else
            {
                item = GetInstanceKhieuNai_SoTien();
            }
            return item;
        }

        public static KhieuNai_KetQuaXuLyImpl GetInstanceKhieuNai_KetQuaXuLy(int ArchiveId)
        {
            KhieuNai_KetQuaXuLyImpl item = null;
            if (ArchiveId > 0 && ArchiveConfigImpl.ListArchive.ContainsKey(ArchiveId))
            {
                item = new KhieuNai_KetQuaXuLyImpl(ArchiveConfigImpl.ListArchive[ArchiveId].ConnectionString);
            }
            else
            {
                item = GetInstanceKhieuNai_KetQuaXuLy();
            }
            return item;
        }

        public static KhieuNai_LogImpl GetInstanceKhieuNai_Log(int ArchiveId)
        {
            KhieuNai_LogImpl item = null;
            if (ArchiveId > 0 && ArchiveConfigImpl.ListArchive.ContainsKey(ArchiveId))
            {
                item = new KhieuNai_LogImpl(ArchiveConfigImpl.ListArchive[ArchiveId].ConnectionString);
            }
            else
            {
                item = GetInstanceKhieuNai_Log();
            }
            return item;
        }
        #endregion
    }
}
