using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GQKN.Archive.Entity;
using GQKN.Archive.Impl;
using GQKN.Archive.Core;

namespace GQKN.Archive
{
    public partial class ucUpdateDateKhieuNaiActivity : UserControl
    {
        public ucUpdateDateKhieuNaiActivity()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //531293
                DateTime startTime = DateTime.Now;
                int khieuNaiIdStart = Convert.ToInt32(txtKhieuNaiIdStart.Text);
                int khieuNaiIdTo = Convert.ToInt32(txtKhieuNaiIdTo.Text);
                int numberRecord = Convert.ToInt32(txtNumberRecord.Text);

                while (khieuNaiIdStart > 0 && khieuNaiIdStart <= khieuNaiIdTo)
                {
                    khieuNaiIdStart = Update(khieuNaiIdStart, numberRecord);
                }

                DateTime endTime = DateTime.Now;

                System.Windows.Forms.MessageBox.Show(string.Format("Bắt đầu : {0} - Kết thúc : {1} - Trong khoảng : {2}", startTime.ToString("HH:mm:ss"), endTime.ToString("HH:mm:ss"), endTime.Subtract(startTime).Minutes));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Có lỗi, hãy kiểm tra lại log");
                Utility.LogEvent(ex);
            }

        }

        private int Update(int khieuNaiIdStart, int numberRecord)
        {
            List<KhieuNaiInfo> listKhieuNaiInfo = new KhieuNaiImpl().GetListDynamic("TOP " + numberRecord + "*", "TrangThai = 3 AND Id >=" + khieuNaiIdStart, "Id ASC");
            if (listKhieuNaiInfo != null && listKhieuNaiInfo.Count > 0)
            {
                KhieuNai_ActivityImpl khieuNaiActivityImpl = new KhieuNai_ActivityImpl();

                for (int i = 0; i < listKhieuNaiInfo.Count; i++)
                {
                    List<KhieuNai_ActivityInfo> listKhieuNaiActivityInfo = khieuNaiActivityImpl.GetListDynamic("*", "KhieuNaiId=" + listKhieuNaiInfo[i].Id, "Id ASC");
                    if (listKhieuNaiActivityInfo != null)
                    {
                        string sql = string.Empty;
                        for (int j = 0; j < listKhieuNaiActivityInfo.Count; j++)
                        {
                            if (listKhieuNaiActivityInfo[j].NgayTiepNhan_PhongBanXuLyTruoc.Year == DateTime.MaxValue.Year)
                            {
                                if (listKhieuNaiActivityInfo[j].ActivityTruoc == 0)
                                {
                                    sql = string.Format("Update KhieuNai_Activity Set NgayTiepNhan_NguoiXuLyTruoc='{1}', NgayTiepNhan_NguoiXuLy = '{1}', NgayTiepNhan_PhongBanXuLyTruoc = '{1}', NgayQuaHan_PhongBanXuLyTruoc = '{1}' WHERE Id = {0};", listKhieuNaiActivityInfo[j].Id, DateTime.MaxValue.ToString());
                                }
                                else
                                {
                                    sql = string.Format("{0} Update KhieuNai_Activity Set NgayTiepNhan_NguoiXuLy = '{2}', NgayTiepNhan_PhongBanXuLyTruoc = '{2}', NgayQuaHan_PhongBanXuLyTruoc = '{3}', NgayTiepNhan_NguoiXuLyTruoc = '{4}' WHERE Id = {1};", sql, listKhieuNaiActivityInfo[j].Id, listKhieuNaiActivityInfo[j - 1].NgayTiepNhan, listKhieuNaiActivityInfo[j - 1].NgayQuaHan, listKhieuNaiActivityInfo[j - 1].NgayTiepNhan_NguoiXuLy);
                                }
                            }
                        }

                        if (sql.Length > 0)
                        {
                            khieuNaiActivityImpl.ExecuteNonQuery(sql, CommandType.Text, null);
                        }
                    }
                }

                return listKhieuNaiInfo[listKhieuNaiInfo.Count - 1].Id;
            }
            else
            {
                return -1;
            }

        }
    }
}
