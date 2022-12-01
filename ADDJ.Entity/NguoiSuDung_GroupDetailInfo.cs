using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table NguoiSuDung_GroupDetail in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class NguoiSuDung_GroupDetailInfo
	{
		public int Id { get; set; }
		public int GroupId { get; set; }
		public int NguoiSuDungId { get; set; }
		public int Active { get; set; }
		
				
		public NguoiSuDung_GroupDetailInfo()
		{
			Id = 0;
			GroupId = 0;
			NguoiSuDungId = 0;
			Active = 0;
			
		}
	}
}
