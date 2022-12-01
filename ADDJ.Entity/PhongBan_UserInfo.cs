using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table PhongBan_User in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class PhongBan_UserInfo
	{
		public int Id { get; set; }
		public int PhongBanId { get; set; }
		public int NguoiSuDungId { get; set; }

        public string TenTruyCap { get; set; }		
		public PhongBan_UserInfo()
		{
			Id = 0;
			PhongBanId = 0;
			NguoiSuDungId = 0;
			
		}
	}
}
