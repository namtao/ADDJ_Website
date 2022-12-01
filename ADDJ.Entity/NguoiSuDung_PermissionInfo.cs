using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table NguoiSuDung_Permission in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	[Serializable]
	public class NguoiSuDung_PermissionInfo
	{
		public int Id { get; set; }
		public int PermissionSchemeId { get; set; }
		public int NguoiSuDungId { get; set; }
		public bool IsAllow { get; set; }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string MoTa { get; set; }
		
				
		public NguoiSuDung_PermissionInfo()
		{
			Id = 0;
			PermissionSchemeId = 0;
			NguoiSuDungId = 0;
			IsAllow = false;
			
		}
	}
}
