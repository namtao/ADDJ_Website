using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADDJ.Entity
{
    public class OutputJSONToAJax
    {
        public int ErrorId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Content { get; set; }

        public static string ToJSON(int errorId = 0, string mess = "", string content = "", string title = "error")
        {
            OutputJSONToAJax obj = new OutputJSONToAJax();
            obj.ErrorId = errorId;
            obj.Title = title;
            obj.Message = mess;
            obj.Content = content;

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

    }
}
