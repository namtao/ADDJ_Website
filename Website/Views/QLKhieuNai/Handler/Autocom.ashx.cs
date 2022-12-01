using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AIVietNam.Admin;
using Website.AppCode;
using System.Web.SessionState;
using AIVietNam.GQKN.Entity;

namespace Website.Views.QLKhieuNai.Handler
{
    /// <summary>
    /// Summary description for Autocom
    /// </summary>
    public class Autocom : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var type = context.Request.Form["key"] ?? context.Request.QueryString["key"];

            switch (type)
            {
                case "1":
                    AutoCompleteNguoiTiepNhan(context);
                    break;
                case "2"://Load autocomplete cho truy van nâng cao
                    AutoComplete(context);
                    break;
                case "3"://Load autocomplete cho truy van nâng cao
                    AutoCompleteDauSoCP(context);
                    break;
                case "4":
                    AutoCompleteLoaiKhieuNai(context);
                    break;
                case "5":
                    GetListValueDropList(context);
                    break;
                
            }
        }

        private void GetListValueDropList(HttpContext context)
        {
            string strValue = "";
            string Id = context.Request.QueryString["Id"];
            string ParentId = context.Request.QueryString["ParentId"];
            if (ParentId != "0")
            {
                strValue = Id;
                LoaiKhieuNaiInfo _LoaiKhieuNaiInfo = ServiceFactory.GetInstanceLoaiKhieuNai().GetInfo(Convert.ToInt32(ParentId));
                if (_LoaiKhieuNaiInfo.ParentId != 0)
                {
                    strValue = _LoaiKhieuNaiInfo.Id.ToString() + "#" + strValue;
                    LoaiKhieuNaiInfo Info = ServiceFactory.GetInstanceLoaiKhieuNai().GetInfo(Convert.ToInt32(_LoaiKhieuNaiInfo.ParentId));
                    if (Info.ParentId == 0)
                    {
                        strValue = Info.Id.ToString() + "#" + strValue;
                    }
                }
                else
                {
                    strValue = _LoaiKhieuNaiInfo.Id.ToString() + "#" + strValue;
                }
            }
            else
            {
                strValue = Id + "#" + "0#0";
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(strValue));
            
        }

        private void AutoCompleteLoaiKhieuNai(HttpContext context)
        {
            var obj = ServiceFactory.GetInstanceLoaiKhieuNai();
            var lst = obj.Suggestion(context.Request.QueryString["q"]);
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
        }

        private void AutoCompleteDauSoCP(HttpContext context)
        {
            var obj = ServiceFactory.GetInstanceDauSoCP();
            var lst = obj.Suggestion(context.Request.QueryString["q"]);
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
        }
        private void AutoCompleteNguoiTiepNhan(HttpContext context)
        {
            var obj = ServiceFactory.GetInstanceNguoiSuDung();
            var lst = obj.Suggestion(context.Request.QueryString["q"]);
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
        }
        private void AutoComplete(HttpContext context)
        {
            string column = context.Request.QueryString["column"];
            string search = "";
            var arr = context.Request.QueryString["q"].ToString().Split(',');
            if (arr.Length > 1)
            {
                search = arr[arr.Length - 1];
            }
            else
            {
                search = context.Request.QueryString["q"].ToString();
            }
            if (column == "NguoiTiepNhan" || column == "NguoiXuLy")
            {
                var obj = ServiceFactory.GetInstanceNguoiSuDung();
                var lst = obj.Suggestion(search);
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
            }
            else if (column == "LoaiKhieuNaiId" || column == "LinhVucChungId" || column == "LinhVucConId")
            {
                var obj = ServiceFactory.GetInstanceLoaiKhieuNai();
                var lst = obj.Suggestion(search);
                //var resParent = lst.Where(s => s.ParentId == 0).ToList();
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
            }
            else if (column == "PhongBanTiepNhanId" || column == "PhongBanXuLyId")
            {
                var obj = ServiceFactory.GetInstancePhongBan();
                var lst = obj.Suggestion(search);
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
            }
            else if (column == "MaTinhId" || column =="MaQuanId")
            {
                var obj = ServiceFactory.GetInstanceProvince();
                var lst = obj.Suggestion(search);
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
            }
            else if (column == "DoiTacId")
            {
                var obj = ServiceFactory.GetInstanceDoiTac();
                var lst = obj.Suggestion(search);
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(lst));
            }
            else if (column == "DoUuTien")
            {
                List<KhieuNai_TruyVanNameValue> list = new List<KhieuNai_TruyVanNameValue>();               
                

                foreach (byte i in Enum.GetValues(typeof(KhieuNai_DoUuTien_Type)))
                {
                    KhieuNai_TruyVanNameValue info = new KhieuNai_TruyVanNameValue();
                    info.Value = (int)i;
                    info.Name = Enum.GetName(typeof(KhieuNai_DoUuTien_Type), i).Replace("_", " ");                    
                    list.Add(info);                   

                }

                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
            else if (column == "TrangThai")
            {
                List<KhieuNai_TruyVanNameValue> list = new List<KhieuNai_TruyVanNameValue>();   
                foreach (byte i in Enum.GetValues(typeof(KhieuNai_TrangThai_Type)))
                {
                    KhieuNai_TruyVanNameValue info = new KhieuNai_TruyVanNameValue();
                    info.Value = (int)i;
                    info.Name = Enum.GetName(typeof(KhieuNai_TrangThai_Type), i).Replace("_", " ");
                    list.Add(info);

                }

                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
            else if (column == "DoHaiLong")
            {
                List<KhieuNai_TruyVanNameValue> list = new List<KhieuNai_TruyVanNameValue>();
                foreach (int i in Enum.GetValues(typeof(KhieuNai_DoHaiLong_Type)))
                {
                    KhieuNai_TruyVanNameValue info = new KhieuNai_TruyVanNameValue();
                    info.Value = i;
                    info.Name = Enum.GetName(typeof(KhieuNai_DoHaiLong_Type), i).Replace("_", " ");
                    list.Add(info);

                }

                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(list));
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}