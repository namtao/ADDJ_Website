using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table ThongBao in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>25/02/2014</date>
	
	[Serializable]
	public class ThongBaoInfo
	{
		public int Id { get; set; }
		public string TieuDe { get; set; }
		public string NoiDung { get; set; }
		public bool Display { get; set; }

        public bool IsNew { get; set; }

        public DateTime CDate { get; set; }
		public DateTime LDate { get; set; }
        
		public string LUser { get; set; }
				
		public ThongBaoInfo()
		{
			Id = 0;
			TieuDe = string.Empty;
			NoiDung = string.Empty;
			Display = false;
            IsNew = false;
		}
	}
}
