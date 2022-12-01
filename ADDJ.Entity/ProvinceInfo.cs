using System;
using ADDJ.Core.AILucene;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table Province in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>21/08/2013</date>
	
	[Serializable]
	public class ProvinceInfo
	{
        [AIFieldUnikey("Id")]
		public int Id { get; set; }
        [AIField("ParentId")]
		public int ParentId { get; set; }
        [AIField("Name")]
		public string Name { get; set; }
        [AIField("LevelNbr")]
		public int LevelNbr { get; set; }
		public string AbbRev { get; set; }

        public int KhuVucId { get; set; }

        public string CUser { get; set; }
        public DateTime CDate { get; set; }
        public string LUser { get; set; }
        public DateTime LDate { get; set; }
		
				
		public ProvinceInfo()
		{
			Id = 0;
			ParentId = 0;
			Name = string.Empty;
			LevelNbr = 0;
			AbbRev = string.Empty;
            KhuVucId = 0;
            CUser = string.Empty;
            CDate = DateTime.MaxValue;
            LUser = string.Empty;
            LDate = DateTime.MaxValue;
		}
	}
}
