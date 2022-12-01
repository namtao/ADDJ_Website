using AIVietNam.Admin;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using Website.Service;

namespace Website
{
    public partial class ExecuteSQL : System.Web.UI.Page
    {
        protected string sNoiDungExportExcel = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoginAdmin.IsLoginAdmin();
            AdminInfo admin = LoginAdmin.AdminLogin();
            if (admin.Username.ToLower() != "administrator")
            {
                Response.Redirect("/");
            }

            if (!IsPostBack)
            {
                ltMess.Text = "";
            }
            grvView.DataSource = null;
            grvView.DataBind();
        }

        protected void FileUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string filePath = @"D:\" + e.FileName;
            //-AjaxFileUpload1.SaveAs(filePath);
        }

        protected void btExecute_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                string sqlBatch = string.Empty;
                conn = new SqlConnection(Config.ConnectionString);

                SqlCommand cmd = new SqlCommand(string.Empty, conn);
                conn.Open();
                string sql = txtSQL.Text;
                foreach (string line in sql.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.ToUpperInvariant().Trim() == "GO")
                    {
                        if (string.IsNullOrEmpty(sqlBatch.Trim()))
                        {
                            continue;
                        }
                        cmd.CommandText = sqlBatch;
                        ltMess.Text += cmd.ExecuteNonQuery() + " row update <br />";
                        sqlBatch = string.Empty;
                    }
                    else
                    {
                        sqlBatch += line + "\n";
                    }
                }

                if (!string.IsNullOrEmpty(sqlBatch))
                {
                    cmd.CommandText = sqlBatch;
                    ltMess.Text += cmd.ExecuteNonQuery() + " row update <br />";
                }
            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        protected void btExecuteQuery_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            DataSet result = null;
            try
            {
                string sqlBatch = string.Empty;
                conn = new SqlConnection(Config.ConnectionString);

                SqlCommand cmd = new SqlCommand(string.Empty, conn);
                conn.Open();
                string sql = txtSQL.Text;
                foreach (string line in sql.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.ToUpperInvariant().Trim() == "GO")
                    {
                        if (string.IsNullOrEmpty(sqlBatch.Trim()))
                        {
                            continue;
                        }
                        cmd.CommandText = sqlBatch;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        result = new DataSet();
                        adapter.Fill(result);

                        ltMess.Text = result.Tables[0].Rows.Count + " rows <br />";
                        grvView.DataSource = result.Tables[0];
                        grvView.DataBind();

                        break;
                    }
                    else
                    {
                        sqlBatch += line + "\n";
                    }
                }

                if (!string.IsNullOrEmpty(sqlBatch))
                {
                    cmd.CommandText = sqlBatch;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    result = new DataSet();
                    adapter.Fill(result);

                    ltMess.Text = result.Tables[0].Rows.Count + " rows <br />";
                    grvView.DataSource = result.Tables[0];
                    grvView.DataBind();
                }
            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Author : Phi Hoang Hai
        /// Created date : 01/07/2014
        /// Todo : Xuất excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

            SqlConnection conn = null;
            DataSet result = null;
            try
            {
                string sqlBatch = string.Empty;
                conn = new SqlConnection(Config.ConnectionString);

                SqlCommand cmd = new SqlCommand(string.Empty, conn);
                conn.Open();
                string sql = txtSQL.Text;
                foreach (string line in sql.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.ToUpperInvariant().Trim() == "GO")
                    {
                        if (string.IsNullOrEmpty(sqlBatch.Trim()))
                        {
                            continue;
                        }
                        cmd.CommandText = sqlBatch;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        result = new DataSet();
                        adapter.Fill(result);

                        ltMess.Text = result.Tables[0].Rows.Count + " rows <br />";
                        grvView.DataSource = result.Tables[0];
                        grvView.DataBind();

                        break;
                    }
                    else
                    {
                        sqlBatch += line + "\n";
                    }
                }

                if (!string.IsNullOrEmpty(sqlBatch))
                {
                    cmd.CommandText = sqlBatch;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    result = new DataSet();
                    adapter.Fill(result);

                    StringBuilder sb = new StringBuilder();
                    DataTable dtResult = result.Tables[0];

                    sb.Append("<tr>");
                    foreach (DataColumn col in dtResult.Columns)
                    {
                        sb.AppendFormat("<th>{0}</th>", col.ColumnName);
                    }
                    sb.Append("</tr>");

                    foreach (DataRow row in dtResult.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn col in dtResult.Columns)
                        {
                            sb.AppendFormat("<td>{0}</td>", row[col].ToString());
                        }
                        sb.Append("</tr>");
                    }

                    sNoiDungExportExcel = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=ExecuteSql.xls");
            Response.Charset = "";
            this.EnableViewState = false;
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            string pathCSS = Server.MapPath("~/CSS");
            pathCSS += @"\BaoCao.css";
            StreamReader reader = new StreamReader(pathCSS);
            //reader.ReadToEnd();

            Response.Write("<style>");
            Response.Write(reader.ReadToEnd());
            Response.Write("</style>");
            baocao.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }


        protected void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //ServiceKhieuNai.ServiceLocal obj = new ServiceKhieuNai.ServiceLocal();
                string result = ServiceLocalImpl.UpdateKhieuNaiToTraSau(txtMaKhieuNai.Text.Trim(), Convert.ToInt32(ddlLoaiTK.SelectedValue));

                if (result == null)
                {
                    ltMess.Text = "Có lỗi.";
                    return;
                }
                ltMess.Text = result;

            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
        }

        protected void btGetKhieuNai_Click(object sender, EventArgs e)
        {
            try
            {
                //ServiceKhieuNai.ServiceLocal obj = new ServiceKhieuNai.ServiceLocal();
                string result = ServiceLocalImpl.GetFullKhieuNai(txtSoThueBao.Text.Trim(), txtMaKhieuNai.Text.Trim());

                if (result == null)
                {
                    ltMess.Text = "Có lỗi.";
                    return;
                }
                var lstObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KhieuNaiInfo>>(result);
                grvView.DataSource = lstObj;
                grvView.DataBind();

            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
        }

        protected void btGetActivity_Click(object sender, EventArgs e)
        {
            try
            {
                //ServiceKhieuNai.ServiceLocal obj = new ServiceKhieuNai.ServiceLocal();
                var result = ServiceLocalImpl.GetFullActivity(txtMaKhieuNai.Text.Trim());

                if (result == null)
                {
                    ltMess.Text = "Có lỗi.";
                    return;
                }
                List<KhieuNai_ActivityInfo> lstObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KhieuNai_ActivityInfo>>(result);
                grvView.DataSource = lstObj;
                grvView.DataBind();
            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
        }

        protected void btGetSoTien_Click(object sender, EventArgs e)
        {
            try
            {
                //ServiceKhieuNai.ServiceLocal obj = new ServiceKhieuNai.ServiceLocal();
                var result = ServiceLocalImpl.GetFullSoTien(txtMaKhieuNai.Text.Trim());

                if (result == null)
                {
                    ltMess.Text = "Có lỗi.";
                    return;
                }
                var lstObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KhieuNai_SoTienInfo>>(result);
                grvView.DataSource = lstObj;
                grvView.DataBind();
            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
        }

        protected void btGetPhongBanUser_Click(object sender, EventArgs e)
        {
            try
            {
                //ServiceKhieuNai.ServiceLocal obj = new ServiceKhieuNai.ServiceLocal();
                string result = ServiceLocalImpl.GetPhongBanUser(txtTenTruyCap.Text.Trim());

                if (result == null)
                {
                    ltMess.Text = "Có lỗi.";
                    return;
                }
                var lstObj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PhongBanInfo>>(result);
                grvView.DataSource = lstObj;
                grvView.DataBind();
            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
        }

        protected void btDelKhieuNai_Click(object sender, EventArgs e)
        {
            try
            {

                string result = ServiceLocalImpl.DeleteKhieuNai(txtMaKhieuNai.Text.Trim());

                if (result == null)
                {
                    ltMess.Text = "Có lỗi.";
                    return;
                }
                ltMess.Text = result;

            }
            catch (Exception ex)
            {
                ltMess.Text = ex.Message;
            }
        }

        ///// <summary>
        ///// Author : Phi Hoang Hai
        ///// Created date : 02/11/2015
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnExecuteSolr_Click(object sender, EventArgs e)
        //{
        //    string url = txtSolr.Text;
        //    Response.Redirect(url);
        //    // "http://10.149.34.231:8080/solr/Activity/select?qt=/dataimport&command=full-import&clean=true&commit=true";
        //    //var response = DownloadExpress.Download(url);
        //    //ScriptManager.RegisterClientScriptBlock(UpdatePanelSolr, UpdatePanelSolr.GetType(), "onloadMessage", "MessageAlert.AlertNormal('" + response.StatusCode + ".','info');", true);
        //}
    }
}