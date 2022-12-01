using System;

namespace Website.AppCode.Controller
{
	/// <summary>
    /// Class Mapping table UserAcc in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>07/12/2012</date>
	
	[Serializable]
	public class UserAccInfo
	{
		public Int64 Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string FullName { get; set; }
		public string Address { get; set; }
		public int DoiTacId { get; set; }
		public DateTime Block_Date { get; set; }
		public byte Status { get; set; }
		public DateTime Birthday { get; set; }
        public string BirthdayJSON { get; set; }
		public string Code { get; set; }
		public Int16 ChucVu { get; set; }
		public byte QuanLy { get; set; }
        public byte Sex { get; set; }
		public int LoginCount { get; set; }
		public DateTime LastLogin { get; set; }
		public int ParentId { get; set; }
		
		public DateTime LDate { get; set; }
		public DateTime CDate { get; set; }
        public string CDateJSON { get; set; }
		public string LUser { get; set; }
		public string CUser { get; set; }
				
		public UserAccInfo()
		{
			Id = 0;
			Username = string.Empty;
			Password = string.Empty;
			Phone = string.Empty;
			Mobile = string.Empty;
			Email = string.Empty;
			FullName = string.Empty;
			Address = string.Empty;
			DoiTacId = 0;
			Block_Date = DateTime.Now;
			Status = 0;
			Birthday = DateTime.Now;
			Code = string.Empty;
			ChucVu = 0;
			QuanLy = 0;
            Sex = 0;
			LoginCount = 0;
			LastLogin = DateTime.Now;
			ParentId = 0;
		}
	}
}
