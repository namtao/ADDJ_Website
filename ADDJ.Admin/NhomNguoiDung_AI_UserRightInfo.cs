using System;

namespace ADDJ.Admin
{
	/// <summary>
    /// Class Mapping table NhomNguoiDung_AI_UserRight in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>03.04.2014</date>	
	
	public class NhomNguoiDung_AI_UserRightInfo
	{
		public int ID { get; set; }
		public int MenuID { get; set; }
		public int NhomNguoiDung_AIId { get; set; }
		public bool UserRead { get; set; }
		public bool UserEdit { get; set; }
		public bool UserDelete { get; set; }
		public bool Other1 { get; set; }
		public bool Other2 { get; set; }

        public int Menu_STT { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_Url { get; set; }
        public int Menu_ParentId { get; set; }
        public int Menu_Display { get; set; }
				
		public NhomNguoiDung_AI_UserRightInfo()
		{
			ID = 0;
			MenuID = 0;
			NhomNguoiDung_AIId = 0;
			UserRead = false;
			UserEdit = false;
			UserDelete = false;
			Other1 = false;
			Other2 = false;
			
		}
	}
}
