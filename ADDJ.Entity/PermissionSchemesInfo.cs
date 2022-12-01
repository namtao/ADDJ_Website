using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table PermissionSchemes in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>02/08/2013</date>
	
	[Serializable]
	public class PermissionSchemesInfo
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string MoTa { get; set; }
        public int Sort { get; set; }
		
				
		public PermissionSchemesInfo()
		{
			Id = 0;
			Name = string.Empty;
			MoTa = string.Empty;
            Sort = 0;
			
		}
	}
}
