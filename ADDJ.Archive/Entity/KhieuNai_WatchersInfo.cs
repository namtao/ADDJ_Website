using System;

namespace GQKN.Archive.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNai_Watchers in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class KhieuNai_WatchersInfo
	{
		public Int64 Id { get; set; }
		public int KhieuNaiId { get; set; }
		public int NguoiSuDungId { get; set; }
		
				
		public KhieuNai_WatchersInfo()
		{
			Id = 0;
			KhieuNaiId = 0;
			NguoiSuDungId = 0;
			
		}
	}
}
