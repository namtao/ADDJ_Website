using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIVietNam.GQKN.Entity
{
    public class SubinfoTKTTEntity
    {
        public int ErrorId { get; set; }
        public string Balance { get; set; }
        public string BalanceKM { get; set; }
        public string BalanceKM1 { get; set; }
        public string BalanceKM2 { get; set; }
        public string BalanceData { get; set; }      
        public string HSD { get; set; }

        public SubinfoTKTTEntity()
        {
            ErrorId = 0;
            Balance = string.Empty;
            BalanceKM = string.Empty;
            BalanceKM1 = string.Empty;
            BalanceKM2 = string.Empty;
            BalanceData = string.Empty;
            HSD = string.Empty;
        }
    }
}
