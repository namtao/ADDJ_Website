using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Components.Info
{
    // Edited by	: Dao Van Duong
    // Datetime		: 2.8.2016 12:02
    // Note			: Lớp cũng cấp trả về client các vấn đề khi gọi Ajax
    public class MessageInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Content { get; set; }
        public MessageInfo()
        {
            this.Code = 0;
            this.Message = "Không xác định";
            this.Content = null;
        }
        public MessageInfo(int code = 0, string message = "Không xác định", string content = null)
        {
            this.Code = code;
            this.Message = message;
            this.Content = content;
        }
    }

    public class ResponseInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ResponseInfo()
        {
            this.Code = 0;
            this.Message = "Không xác định";
            this.Data = null;
        }
        public ResponseInfo(int code = 0, string message = "Không xác định", object data = null)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }
}