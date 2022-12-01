using System;

namespace ADDJ.Entity
{
	/// <summary>
    /// Class Mapping table KhieuNaiRef_VNPT in Databasse
    /// </summary>
    /// <author>Lê Xuân Long</author>
    /// <date>06/05/2014</date>
	
	[Serializable]
    public class KhieuNaiRef_VNPTInfo
    {
        public int Id { get; set; }
        public int KhieuNaiIdVNP { get; set; }
        public string KhieuNaiIdVNPT { get; set; }
        public string KhieuNaiIdVNPTMain { get; set; }
        public string KhieuNaiIdTTKN { get; set; }

        public DateTime CDate { get; set; }
        public DateTime LDate { get; set; }

        public KhieuNaiRef_VNPTInfo()
        {
            Id = 0;
            KhieuNaiIdVNP = 0;
            KhieuNaiIdVNPT = string.Empty;
            KhieuNaiIdVNPTMain = string.Empty;
            KhieuNaiIdTTKN = string.Empty;

        }
    }

}
