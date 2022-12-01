using System;
using ADDJ.Core.AILucene;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table DoiTac in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class DoiTacInfo
	{
        [AIFieldUnikey("Id")]
		public int Id { get; set; }
		public int DonViTrucThuoc { get; set; }
		public int ProvinceId { get; set; }
		public int ProvinceHuyenId { get; set; }
		public string MaDoiTac { get; set; }
        [AIField("TenDoiTac")]
		public string TenDoiTac { get; set; }
		public string DienThoai { get; set; }
		public string MaSoThue { get; set; }
		public string Fax { get; set; }
		public string DiaChi { get; set; }
		public string Website { get; set; }
		public string NguoiDaiDien { get; set; }
		public string ChucVu { get; set; }
		public int LoaiGiayTo { get; set; }
		public DateTime NgayCap { get; set; }
		public string NoiCap { get; set; }
		public string SoGiayTo { get; set; }
		public string SoHopDong { get; set; }
		public DateTime NgayHopDong { get; set; }
		public string GhiChu { get; set; }
		public int Sort { get; set; }
		
		public DateTime CDate { get; set; }
		public DateTime LDate { get; set; }
		public string CUser { get; set; }
		public string LUser { get; set; }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 18/11/2013
        /// Todo : thêm trường DoiTacType để xác định đối tác thuộc loại gì
        /// 1:ĐKT (VNP), 2 : TTTC, 3: VAS, 4:VNPT TT, 5: Đối tác của VNP
        /// </summary>
        public byte DoiTacType { get; set; }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 07/05/2014
        /// Todo : Thêm trường DonViTrucThuocChoBaoCao để phục vụ cho việc hiển thị trên báo cáo tổng hợp
        ///     bởi vì hiện tại các VNPTT TT và TDD các tỉnh thành đều thuộc VNP chứ không thuộc VNPT và TDD khu vực
        ///     trường này sẽ xác định khu vực của các VNPT TT và TDD TT
        /// </summary>
        public int DonViTrucThuocChoBaoCao { get; set; }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 10/12/2014
        /// Todo : Thuộc tính Level phục vụ cho việc đánh dấu cấp của đối tác để hiển thị trên báo cáo
        /// </summary>
        public int Level { get; set; }
				
		public DoiTacInfo()
		{
			Id = 0;
			DonViTrucThuoc = 0;
			ProvinceId = 0;
			ProvinceHuyenId = 0;
			MaDoiTac = string.Empty;
			TenDoiTac = string.Empty;
			DienThoai = string.Empty;
			MaSoThue = string.Empty;
			Fax = string.Empty;
			DiaChi = string.Empty;
			Website = string.Empty;
			NguoiDaiDien = string.Empty;
			ChucVu = string.Empty;
			LoaiGiayTo = 0;
			NgayCap = DateTime.Now;
			NoiCap = string.Empty;
			SoGiayTo = string.Empty;
			SoHopDong = string.Empty;
			NgayHopDong = DateTime.Now;
			GhiChu = string.Empty;
			Sort = 0;
            DoiTacType = 0;
			
		}

        public class DoiTacIdValue
        {
            public static readonly int VNP = 1;
            public static readonly int VNP1 = 2;
            public static readonly int VNP2 = 3;
            public static readonly int VNP3 = 5;

            public static readonly int DKT1 = 7;
            public static readonly int DKT2 = 14;
            public static readonly int DKT3 = 19;

            public static readonly int TTTC = 10100;
            public static readonly int PTDV = 10101;

            public static readonly int VNPT_NET = 10194;
            public static readonly int VNPT_MEDIA = 10208;
            public static readonly int NET_HATANGMANG = 10195;
            public static readonly int NET_DHTT = 10196;
        }

        public class DoiTacTypeValue
        {
            public static readonly byte DKT_VNP = 1;
            public static readonly byte TTTC = 2;
            public static readonly byte VAS = 3;
            public static readonly byte VNPTTT = 4;
            public static readonly byte DOI_TAC_VNP = 5;
            public static readonly byte TDD_VNP = 6;
        }
	}
}
