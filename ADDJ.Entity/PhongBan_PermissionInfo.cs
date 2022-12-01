using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table PhongBan_Permission in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class PhongBan_PermissionInfo
	{
		public int Id { get; set; }
		public int PermissionSchemeId { get; set; }
		public int PhongBanId { get; set; }
		public bool IsAllow { get; set; }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string MoTa { get; set; }
				
		public PhongBan_PermissionInfo()
		{
			Id = 0;
			PermissionSchemeId = 0;
			PhongBanId = 0;
			IsAllow = false;
			
		}
	}
}
