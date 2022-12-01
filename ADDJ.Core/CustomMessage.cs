using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Core
{
    public enum CustomCode : int
    {
        [Name("Thành công")]
        OK = 1,

        [Name("Không hợp lệ")]
        KhongHopLe = 0,

        [Name("Dữ liệu không hợp lệ")]
        DuLieuKhongHopLe = -1,

        [Name("Không có dữ liệu")]
        KhongCoDuLieu = -2,

        [Name("Không xác định")]
        KhongXacDinh = -99
    }

    public class CustomMessage
    {
        public CustomCode Code { get; set; }
        public string Message { get; set; }
        public int Number { get; set; }
        public object Data { get; set; }
        public object DataEx { get; set; }
        public object DataTemplate { get; set; }
        public CustomMessage()
        {
            Code = CustomCode.DuLieuKhongHopLe;
            Message = "Dữ liệu không hợp lệ, vui lòng thử lại";
            Number = 0;
            Data = null;
            DataEx = null;
            DataTemplate = null;
        }
        public CustomMessage(CustomCode code, string message)
        {
            Code = code;
            Message = message;
            Number = 0;
            Data = null;
            DataEx = null;
            DataTemplate = null;
        }
        public CustomMessage(CustomCode code, string message, object data = null, object dataEx = null, object dataTemplate = null)
        {
            Code = code;
            Message = message;
            Number = 0;
            Data = data;
            DataEx = dataEx;
            DataTemplate = dataTemplate;
        }
        public CustomMessage(CustomCode code, string message, int number = 0, object data = null, object dataEx = null, object dataTemplate = null)
        {
            Code = code;
            Message = message;
            Number = number;
            Data = data;
            DataEx = dataEx;
            DataTemplate = dataTemplate;
        }
    }

    public class CustomTemplate
    {
        public string Name { get; set; }
        public string DataField { get; set; }
        public int STT { get; set; }
        public CustomTemplate()
        {
            this.Name = string.Empty;
            this.DataField = string.Empty;
            this.STT = 1;
        }
    }
}
