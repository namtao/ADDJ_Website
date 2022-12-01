using System;

namespace ADDJ.News.Entity
{
	/// <summary>
    /// Class Mapping table Category in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/07/2012</date>
	
	[Serializable]
	public class CategoryInfo
	{
		public int Id { get; set; }
		public int ParentId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
        public int Sort { get; set; }
        public byte Cap { get; set; }
        public byte Status { get; set; }


		public CategoryInfo()
		{
			Id = 0;
			ParentId = 0;
			Name = string.Empty;
			Description = string.Empty;
            Sort = 0;
            Cap = 0;
            Status = 0;

		}
	}
}
