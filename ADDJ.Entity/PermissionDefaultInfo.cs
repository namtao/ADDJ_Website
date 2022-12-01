using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table PermissionDefault in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>04/10/2013</date>
	
	[Serializable]
	public class PermissionDefaultInfo
	{
		public int Id { get; set; }
		public int PermissionSchemeId { get; set; }
		public int DefaultPermissionId { get; set; }
		public bool IsAllow { get; set; }

        public int PermissionId { get; set; }
        public string Name { get; set; }
        public string MoTa { get; set; }
		
				
		public PermissionDefaultInfo()
		{
			Id = 0;
			PermissionSchemeId = 0;
			DefaultPermissionId = 0;
			IsAllow = false;
			
		}
	}

    public enum PermissionDefaultType : int
    { 
        KTV_GDV = 1,
        Xử_lý_nghiệp_vụ = 2,
        Cấp_số_liệu = 3,
        Khác_1 = 4,
        Khác_2 = 5,
    }
}
