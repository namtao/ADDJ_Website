using System;

namespace ADDJ.News.Entity
{
	/// <summary>
    /// Class Mapping table News in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>13/07/2012</date>
	
	[Serializable]
	public class NewsInfo
	{
		public int Id { get; set; }
        public int CategoryId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Content { get; set; }
		public string ImagePath { get; set; }
		public int LikeCount { get; set; }
        public DateTime CDate { get; set; }
		public string LUser { get; set; }
		public DateTime LDate { get; set; }

        public string CategoryName { get; set; }
		
		public NewsInfo()
		{
			Id = 0;
            CategoryId = 0;
			Title = string.Empty;
			Description = string.Empty;
			Content = string.Empty;
			ImagePath = string.Empty;
			LikeCount = 0;
			LUser = string.Empty;
			LDate = DateTime.Now;
			
		}
	}
}
