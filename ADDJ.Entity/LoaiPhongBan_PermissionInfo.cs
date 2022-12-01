using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table LoaiPhongBan_Permission in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02.04.2014</date>
	
	[Serializable]
	public class LoaiPhongBan_PermissionInfo
	{
		public int Id { get; set; }
		public int PermissionSchemeId { get; set; }
		public int LoaiPhongBanId { get; set; }
		public bool IsAllow { get; set; }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string MoTa { get; set; }
				
		public LoaiPhongBan_PermissionInfo()
		{
			Id = 0;
			PermissionSchemeId = 0;
			LoaiPhongBanId = 0;
			IsAllow = false;
			
		}
	}
}
