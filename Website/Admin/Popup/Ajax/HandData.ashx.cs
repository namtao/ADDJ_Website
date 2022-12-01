using ADDJ.Admin;
using ADDJ.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website.Admin.Popup.Ajax
{
    /// <summary>
    /// Summary description for HandData
    /// </summary>
    public class HandData : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string type = context.Request.QueryString["Type"];
            CustomMessage retValue = new CustomMessage() { Code = CustomCode.DuLieuKhongHopLe, Message = "Dữ liệu không hợp lệ" };

            if (!string.IsNullOrEmpty(type))
            {
                if (type.ToLower() == "1") // Lấy chi tiết danh sách menu
                {
                    string dsMenuIds = context.Request.QueryString["MenuIds"];
                    if (!string.IsNullOrEmpty(dsMenuIds))
                    {
                        string[] objs = dsMenuIds.Split(new string[] { ", ", ",", "; ", ";" }, StringSplitOptions.RemoveEmptyEntries);
                        if (objs != null && objs.Length > 0)
                        {
                            List<MenuInfo> menus = new MenuImpl().GetListDynamic(string.Empty, string.Format("Id IN ({0})", string.Join(", ", objs)), string.Empty);
                            if (menus != null && menus.Count > 0)
                            {
                                retValue.Code = CustomCode.OK;
                                retValue.Number = menus.Count;
                                retValue.Message = "Thành công";
                                retValue.Data = menus.Select(v => new { Id = v.ID, Link = v.Link, Name = v.Name });
                            }
                        }

                    }
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(retValue));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}