using GQKN.Archive.Core;
using GQKN.Archive.Entity;
using GQKN.Archive.Impl;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GQKN.Archive.Forms
{
    public partial class UpdateLoaiKhieuNaiForm : Form
    {
        public UpdateLoaiKhieuNaiForm()
        {
            InitializeComponent();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                LoaiKhieuNaiImpl loaiKhieuNaiImpl = new LoaiKhieuNaiImpl();

                List<LoaiKhieuNaiInfo> listLoaiKhieuNaiInfo = loaiKhieuNaiImpl.GetList();
                for (int i = 0; i < listLoaiKhieuNaiInfo.Count; i++)
                {
                    if (listLoaiKhieuNaiInfo[i].ParentId == 0)
                    {
                        for (int loopCap2 = 0; loopCap2 < listLoaiKhieuNaiInfo.Count; loopCap2++)
                        {
                            if (listLoaiKhieuNaiInfo[loopCap2].ParentId == listLoaiKhieuNaiInfo[i].Id)
                            {
                                string updateField = string.Format("ParentLoaiKhieuNaiId={0}, NameLoaiKhieuNai=N'{1}'", listLoaiKhieuNaiInfo[i].Id, listLoaiKhieuNaiInfo[i].Name);
                                string whereClause = string.Format("Id={0}", listLoaiKhieuNaiInfo[loopCap2].Id);
                                loaiKhieuNaiImpl.UpdateDynamic(updateField, whereClause);

                                for (int loopCap3 = 0; loopCap3 < listLoaiKhieuNaiInfo.Count; loopCap3++)
                                {
                                    if (listLoaiKhieuNaiInfo[loopCap3].ParentId == listLoaiKhieuNaiInfo[loopCap2].Id)
                                    {
                                        whereClause = string.Format("Id={0}", listLoaiKhieuNaiInfo[loopCap3].Id);
                                        loaiKhieuNaiImpl.UpdateDynamic(updateField, whereClause);
                                    }
                                }
                            }
                        }
                    }
                }

                System.Windows.Forms.MessageBox.Show("Đã xong");
            }
            catch(Exception ex)
            {
                Utility.LogEvent(ex);
                MessageBox.Show("Có lỗi, view log");
            }
            
        }
    }
}
