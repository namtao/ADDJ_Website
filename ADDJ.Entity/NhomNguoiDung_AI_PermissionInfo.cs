using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table NhomNguoiDung_AI_Permission in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>
	
	[Serializable]
	public class NhomNguoiDung_AI_PermissionInfo
	{
		public int Id { get; set; }
		public int NhomNguoiDung_AIId { get; set; }
		public int PermissionSchemeId { get; set; }
		public bool IsAllow { get; set; }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string MoTa { get; set; }
				
		public NhomNguoiDung_AI_PermissionInfo()
		{
			Id = 0;
			NhomNguoiDung_AIId = 0;
			PermissionSchemeId = 0;
			IsAllow = false;
			
		}
	}
}
