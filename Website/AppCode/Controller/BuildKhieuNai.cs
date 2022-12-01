using System.Text;
using System.Collections.Generic;
using Website.AppCode;
using AIVietNam.Core;
using AIVietNam.GQKN.Entity;
using System.Transactions;
using System;
using AIVietNam.Admin;
using AIVietNam.GQKN.Impl;
using AIVietNam.Log.Impl;
using System.IO;
using AIVietNam.Core.Cache;
using System.Data;
using Website.Components.Info;

namespace Website.AppCode.Controller
{
    public class BuildKhieuNai
    {
        #region VNPT
        // Edited by	: Dao Van Duong
        // Datetime		: 8.8.2016 14:26
        // Note			: Cần chỉnh sửa lỗi trả về từ Service là 3
        public static int DongKhieuNaiVNPT(AdminInfo userLogin, KhieuNaiInfo item, bool IsDongKN = false)
        {
            int resultReturn = 0;
            string messLog = string.Empty;
            // Đóng KN nếu KN từ VNPT
            if (item.KhieuNaiFrom == 1)
            {
                string messageVNPT = "SoThueBao: " + item.SoThueBao;
                try
                {
                    //Utility.LogEvent("Bug Test:" + 3);
                    ServiceSyncVNPT_HNI.VINAPHONE objVNPT = new ServiceSyncVNPT_HNI.VINAPHONE();
                    ServiceSyncVNPT_HNI.XACTHUC xacthuc = new ServiceSyncVNPT_HNI.XACTHUC();
                    xacthuc.Username = "gqkn_vnp";
                    xacthuc.Password = "gqkn_vnp";
                    objVNPT.XACTHUCValue = xacthuc;

                    int kqMaLoi = 0;
                    string kqMaMoTa = string.Empty;
                    List<KhieuNaiRef_VNPTInfo> lstItemRef = new AIVietNam.GQKN.Impl.KhieuNaiRef_VNPTImpl().GetListDynamic("", "KhieuNaiIdVNP=" + item.Id, "");
                    if (lstItemRef == null || lstItemRef.Count == 0)
                    {
                        throw new GQKNMessageException("Không tìm thấy mã khiếu nại của hệ thống VNPT từ khiếu nại: " + item.Id + " của VNP.");
                    }

                    KhieuNaiRef_VNPTInfo itemRef = lstItemRef[0];
                    messageVNPT += ", Id Main VNPT: " + itemRef.KhieuNaiIdVNPTMain;

                    try
                    {
                        DoiTacInfo DoiTacItem = ServiceFactory.GetInstanceDoiTac().GetInfo(item.DoiTacId);
                        string NoiDungXuLyVNP = string.Format("{0} /// {1}-{2}", item.NoiDungXuLyDongKN, userLogin.FullName, userLogin.Phone);

                        #region "Thử IsDebug"
                        if (Config.IsDebug)
                        {
                            Utility.LogEvent("Bug Test:" + 7);
                            if (itemRef == null)
                            { Utility.LogEvent("Bug Tesst: " + 8); }
                            if (DoiTacItem == null)
                            { Utility.LogEvent("Bug Tesst: " + 9); }
                            Utility.LogEvent(item.DoiTacId);
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("Bug Test STB: {0} Ma KN: {1}", item.SoThueBao, item.Id).AppendLine();
                            sb.AppendFormat("KhieuNaiIdVNPTMain:{0}", itemRef.KhieuNaiIdVNPTMain).AppendLine();
                            sb.AppendFormat("KhieuNaiIdVNPT:{0}", itemRef.KhieuNaiIdVNPT).AppendLine();
                            sb.AppendFormat("MaDoiTac:{0}", DoiTacItem.MaDoiTac).AppendLine();
                            sb.AppendFormat("NoiDungXuLyVNP:{0}", NoiDungXuLyVNP).AppendLine();
                            Utility.LogEvent(sb.ToString());
                        }
                        #endregion
                        if (IsDongKN)
                        {
                            if (DoiTacItem.MaDoiTac.Equals("HNI"))
                                objVNPT.vnpKhoaKhieuNai(itemRef.KhieuNaiIdVNPTMain, itemRef.KhieuNaiIdVNPT, NoiDungXuLyVNP, ref kqMaLoi, ref kqMaMoTa, item.Id, 1, userLogin.Username, string.Empty, DoiTacItem.MaDoiTac);
                            else
                            {
                                objVNPT.vnpKhoaKhieuNai(itemRef.KhieuNaiIdVNPTMain, itemRef.KhieuNaiIdVNPT, NoiDungXuLyVNP, ref kqMaLoi, ref kqMaMoTa, item.Id, 0, userLogin.Username, string.Empty, DoiTacItem.MaDoiTac);
                                objVNPT.vnpKhoaKhieuNai(itemRef.KhieuNaiIdVNPTMain, itemRef.KhieuNaiIdVNPT, NoiDungXuLyVNP, ref kqMaLoi, ref kqMaMoTa, item.Id, 1, userLogin.Username, string.Empty, DoiTacItem.MaDoiTac);
                            }
                        }
                        else
                        {
                            objVNPT.vnpKhoaKhieuNai(itemRef.KhieuNaiIdVNPTMain, itemRef.KhieuNaiIdVNPT, NoiDungXuLyVNP, ref kqMaLoi, ref kqMaMoTa, item.Id, 0, userLogin.Username, string.Empty, DoiTacItem.MaDoiTac);
                        }
                    }
                    catch (Exception ex)
                    {
                        messLog = "Không gọi được service khóa phiếu của HNI. Mã lỗi service HNI: " + ex.Message;
                        Helper.GhiLogs(ex.Message);
                        throw new Exception(messLog);
                    }

                    if (kqMaLoi != 0 && kqMaLoi != 3) // Quẳng ra Exception lỗi
                    {
                        messLog = "Không khóa được khiếu nại từ service VNPT HNI. Mã lỗi VNPT HNI: " + kqMaLoi + ", Mô tả: " + kqMaMoTa + " của khiếu nại: " + item.Id;
                        throw new GQKNMessageException(messLog);
                    }
                    // Edited by	: Dao Van Duong
                    // Datetime		: 10.8.2016 12:05
                    // Note			: Khóa thành công trường hợp còn lại

                    messLog = "Khóa khiếu nại tự động về VNPT thành công.";
                    ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                    {
                        CUser = item.NguoiXuLy,
                        ThaoTac = messLog,
                        GiaTriMoi = string.Format("Tham số truyền về VNPT: Main={0}, IdVnpt={1}, Content={2}", itemRef.KhieuNaiIdVNPTMain, itemRef.KhieuNaiIdVNPT, item.NoiDungXuLyDongKN),
                        PhongBanId = item.PhongBanXuLyId,
                        KhieuNaiId = item.Id,
                    });
                    resultReturn = 10000;
                }
                catch (GQKNMessageException ge)
                {
                    Utility.LogEvent(ge);
                    resultReturn = 10001;
                    messageVNPT += ge.Message;
                    //ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                    //{
                    //    CUser = item.NguoiXuLy,
                    //    ThaoTac = messageVNPT,
                    //    PhongBanId = item.PhongBanXuLyId,
                    //    KhieuNaiId = item.Id,
                    //});
                    throw ge;
                }
                catch (Exception ex)
                {
                    Utility.LogEvent(ex);
                    resultReturn = 10099;
                    messageVNPT += ". " + ex.Message;
                    //ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                    //{
                    //    CUser = item.NguoiXuLy,
                    //    ThaoTac = messageVNPT,
                    //    PhongBanId = item.PhongBanXuLyId,
                    //    KhieuNaiId = item.Id,
                    //});
                    throw ex;
                }
                finally
                {
                    //messageVNPT = "Id VNPT:";
                    LogImpl.Log(item.Id, AIVietNam.Log.Entity.ObjTypeLog.SERVICE_VNPT_HNI, string.Empty, messageVNPT, AIVietNam.Log.Entity.ActionLog.Khoa_Khieu_Nai);
                }
            }

            return resultReturn;
        }

        public static string ThemFileVNPT(KhieuNaiInfo item, KhieuNai_FileDinhKemInfo fileInfo, string loaiFile, byte[] fileDinhKem, string maTTP)
        {
            string messLog = string.Empty;
            //Đóng KN nếu KN từ VNPT
            if (item.KhieuNaiFrom == 1)
            {
                string messageVNPT = "SoThueBao: " + item.SoThueBao;
                try
                {
                    ServiceSyncVNPT_HNI.VINAPHONE objVNPT = new ServiceSyncVNPT_HNI.VINAPHONE();
                    ServiceSyncVNPT_HNI.XACTHUC xacthuc = new ServiceSyncVNPT_HNI.XACTHUC();
                    xacthuc.Username = "gqkn_vnp";
                    xacthuc.Password = "gqkn_vnp";
                    objVNPT.XACTHUCValue = xacthuc;

                    int kqMaLoi = 0;
                    string kqMaMoTa = "";

                    //Get Info VNPT
                    var KhieuNaiRefInfo = new AIVietNam.GQKN.Impl.KhieuNaiRef_VNPTImpl().GetInfoByKhieuNaiIdVNP(item.Id);


                    messageVNPT += ", Id Phụ VNPT: " + KhieuNaiRefInfo.KhieuNaiIdVNPT;

                    string idFile = string.Empty;

                    try
                    {
                        objVNPT.vnpInsertAttachments(KhieuNaiRefInfo.KhieuNaiIdVNPTMain, KhieuNaiRefInfo.KhieuNaiIdVNPT, item.Id.ToString(), fileInfo.TenFile, loaiFile, fileInfo.GhiChu, fileDinhKem, ref kqMaLoi, ref kqMaMoTa, ref idFile, maTTP);
                    }
                    catch (Exception ex)
                    {
                        messLog = "Không gọi được service thêm file đính kèm của HNI. Mã lỗi service HNI: " + ex.Message;
                        throw new GQKNMessageException(messLog);
                    }

                    if (kqMaLoi != 0)
                    {
                        messLog = "Thêm mới file về VNPT có lỗi. Mã lỗi VNPT HNI: " + kqMaLoi + ", Thông báo lỗi VNPT HNI: " + kqMaMoTa + " của khiếu nại: " + item.Id;
                        throw new GQKNMessageException(messLog);
                    }

                    messLog = "Thêm mới file về VNPT thành công.";
                    ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                    {
                        CUser = item.NguoiXuLy,
                        ThaoTac = messLog,
                        PhongBanId = item.PhongBanXuLyId,
                        KhieuNaiId = item.Id,
                    });
                }
                catch (GQKNMessageException ge)
                {
                    messageVNPT += ge.Message;
                    ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                    {
                        CUser = item.NguoiXuLy,
                        ThaoTac = messageVNPT,
                        PhongBanId = item.PhongBanXuLyId,
                        KhieuNaiId = item.Id,
                    });
                }
                catch (Exception ex)
                {
                    messageVNPT += ex.Message;
                    ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                    {
                        CUser = item.NguoiXuLy,
                        ThaoTac = messageVNPT,
                        PhongBanId = item.PhongBanXuLyId,
                        KhieuNaiId = item.Id,
                    });
                }
                finally
                {
                    LogImpl.Log(item.Id, AIVietNam.Log.Entity.ObjTypeLog.SERVICE_VNPT_HNI, "", messageVNPT, AIVietNam.Log.Entity.ActionLog.Them_File_Khieu_Nai);
                }
            }
            return messLog;
        }

        // Edited by	: Dao Van Duong
        // Datetime		: 2.8.2016 11:50
        // Note			: Chỉnh sửa lại Service gọi cho HNI vì bị lỗi
        // Form chỉ dùng cho tiếp nhận HNI
        public static MessageInfo TiepNhanVNPT_HNI(KhieuNaiInfo khieuNaiInfo, string NoiDungXuLyVNP, string tenfile, string loaifile, byte[] fileDinhkem, string maTTP)
        {
            MessageInfo retMsg = new MessageInfo();
            retMsg.Message = "Số thuê bao: " + khieuNaiInfo.SoThueBao;

            khieuNaiInfo = ServiceFactory.GetInstanceKhieuNai().GetInfo(khieuNaiInfo.Id);

            ServiceSyncVNPT_HNI.VINAPHONE objVNPT = new ServiceSyncVNPT_HNI.VINAPHONE();
            ServiceSyncVNPT_HNI.XACTHUC xacthuc = new ServiceSyncVNPT_HNI.XACTHUC();
            xacthuc.Username = "gqkn_vnp";
            xacthuc.Password = "gqkn_vnp";
            objVNPT.XACTHUCValue = xacthuc;

            int kqMaLoi = 0;
            string kqMaMoTa = string.Empty;
            string ngaytiepnhan = string.Empty;
            int idVnptMain = 0;
            int idVnptPhu = 0;
            int idVnptTTKN = 0;


            ServiceVNP.ServiceVinaphone1 obj = new ServiceVNP.ServiceVinaphone1();
            ServiceVNP.RequestParamSubinfo requestParam = new ServiceVNP.RequestParamSubinfo();
            requestParam.SoThueBao = "84" + khieuNaiInfo.SoThueBao.ToString();

            AdminInfo userInfo = LoginAdmin.AdminLogin();

            #region Thông tin User đang xử lý

            string userName = string.Empty;
            int khuvucId = 0;

            // Trường hợp chạy không cần User đăng nhập (Dùng cho việc fix lại chuyển HNI do bị lỗi service)
            if (userInfo == null)
            {
                // Lấy User người xử lý
                userName = khieuNaiInfo.NguoiTienXuLyCap2;

                // Lấy khu vực người xử lý
                khuvucId = khieuNaiInfo.KhuVucId;
            }
            else
            {
                userName = userInfo.Username;
                khuvucId = userInfo.KhuVucId;
            }
            #endregion

            requestParam.Username = userName;
            requestParam.KhuVucId = khuvucId;
            requestParam.Note = string.Empty;

            ServiceVNP.TBTraTruocFullInfo result = obj.GetInfo(requestParam); // Tìm kiếm "Mã đường thu"

            if (string.IsNullOrEmpty(result.MA_NV))
                throw new Exception("Để chuyển khiếu nại số thuê bao phải là trả sau, và có mã đường thu");

            objVNPT.vnpTiepNhanKhieuNai150406(khieuNaiInfo.SoThueBao.ToString(), result.MA_KH, result.MA_KH, result.MA_NV, khieuNaiInfo.HoTenLienHe.Trim(),
              khieuNaiInfo.DiaChiLienHe.Trim(), khieuNaiInfo.LoaiKhieuNaiId, khieuNaiInfo.IsTraSau ? 1 : 0, khieuNaiInfo.NoiDungPA.Trim(), ConvertUtility.ToInt32(khieuNaiInfo.SDTLienHe), khieuNaiInfo.HoTenLienHe.Trim(), "",
              NoiDungXuLyVNP, tenfile, loaifile, fileDinhkem, ref kqMaLoi, ref kqMaMoTa, ref ngaytiepnhan, khieuNaiInfo.Id, ref idVnptMain, ref idVnptPhu, ref idVnptTTKN, maTTP);

            if (kqMaLoi != 1)
            {
                string messLog = "Tiếp nhận khiếu nại về VNPT có lỗi. Mã lỗi VNPT HNI: " + kqMaLoi + ", Thông báo lỗi VNPT HNI: " + kqMaMoTa + " của khiếu nại: " + khieuNaiInfo.Id;
                retMsg.Code = -1;
                retMsg.Message = messLog;
            }
            else // Tiếp nhận thành công
            {
                #region "Nếu lần đầu gửi KN về HNI"
                if (khieuNaiInfo.KhieuNaiFrom != 1)
                {
                    List<KhieuNai_FileDinhKemInfo> lstFile = ServiceFactory.GetInstanceKhieuNai_FileDinhKem().GetListByKhieuNaiId(khieuNaiInfo.Id);
                    if (lstFile != null && lstFile.Count > 0)
                    {
                        foreach (KhieuNai_FileDinhKemInfo itemFile in lstFile)
                        {
                            byte[] bFile = RemoteFileToFtp.DownloadFileToByte(itemFile.URLFile);
                            string idFile = "";
                            string strExten = Path.GetExtension(itemFile.URLFile).ToUpper();
                            string fileType = "application/vnd.ms-excel";
                            switch (strExten)
                            {
                                case ".XLS":
                                case ".XLSX":
                                    fileType = "application/vnd.ms-excel";
                                    break;
                                case ".CSV":
                                    fileType = "text/csv";
                                    break;
                                case ".DOC":
                                case ".DOCX":
                                    fileType = "application/msword";
                                    break;
                                case ".JPG":
                                case ".PNG":
                                    fileType = "image/jpeg";
                                    break;
                                case ".PDF":
                                    fileType = "application/pdf";
                                    break;
                                case ".ZIP":
                                case ".RAR":
                                    fileType = "application/octet-stream";
                                    break;
                                case ".7Z":
                                    fileType = "application/x-7z-compressed";
                                    break;
                                case ".PPT":
                                case ".PPTX":
                                    fileType = "application/vnd.ms-powerpoint";
                                    break;
                                case ".MP3":
                                    fileType = "audio/mpeg";
                                    break;
                            }
                            // Đẩy file
                            objVNPT.vnpInsertAttachments(idVnptMain.ToString(), idVnptPhu.ToString(), khieuNaiInfo.Id.ToString(), itemFile.TenFile, fileType, itemFile.GhiChu, bFile, ref kqMaLoi, ref kqMaMoTa, ref idFile, maTTP);
                        }
                    }
                }
                #endregion

                // Cập nhật khiếu nại của HNI
                ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("KhieuNaiFrom = 1", "Id = " + khieuNaiInfo.Id);
                new KhieuNaiRef_VNPTImpl().Add(new KhieuNaiRef_VNPTInfo()
                {
                    KhieuNaiIdVNP = khieuNaiInfo.Id,
                    KhieuNaiIdVNPT = idVnptPhu.ToString(),
                    KhieuNaiIdVNPTMain = idVnptMain.ToString(),
                    KhieuNaiIdTTKN = idVnptTTKN.ToString()
                });

                retMsg.Message = "Tiếp nhận khiếu nại về VNPT thành công.";
                retMsg.Code = 10000;
            }
            ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
            {
                CUser = khieuNaiInfo.NguoiXuLy,
                ThaoTac = retMsg.Message,
                PhongBanId = khieuNaiInfo.PhongBanXuLyId,
                KhieuNaiId = khieuNaiInfo.Id,
            });

            // Ghi lại log hệ thống
            LogImpl.Log(khieuNaiInfo.Id, AIVietNam.Log.Entity.ObjTypeLog.SERVICE_VNPT_HNI, string.Empty, retMsg.Message, AIVietNam.Log.Entity.ActionLog.Them_File_Khieu_Nai);
            return retMsg;
        }
        #endregion

        #region PTDV
        public static MessageInfo TiepNhanVAS(KhieuNaiInfo khieuNaiInfo, KhieuNai_ActivityInfo activityInfo)
        {
            int resultService = 1;
            string messageVNPT = "Số thuê bao: " + khieuNaiInfo.SoThueBao;
            string messLog = "Tiếp nhật thành công";
            try
            {
                khieuNaiInfo = ServiceFactory.GetInstanceKhieuNai().GetInfo(khieuNaiInfo.Id);

                ServiceSyncVAS.WS_SyncVNP objVAS = new ServiceSyncVAS.WS_SyncVNP();

                int kqMaLoi = 0;
                string kqMaMoTa = "";

                try
                {
                    ServiceSyncVAS.KhieuNaiInfo KhieuNaiItemVAS = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceSyncVAS.KhieuNaiInfo>(Newtonsoft.Json.JsonConvert.SerializeObject(khieuNaiInfo));
                    ServiceSyncVAS.KhieuNai_ActivityInfo ActivityItemVAS = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceSyncVAS.KhieuNai_ActivityInfo>(Newtonsoft.Json.JsonConvert.SerializeObject(activityInfo));
                    objVAS.TiepNhanKhieuNai(KhieuNaiItemVAS, ActivityItemVAS, ref kqMaLoi, ref kqMaMoTa);
                }
                catch (Exception ex)
                {
                    messLog = "Không gọi được service tiếp nhận khiếu nại của VAS. Mã lỗi service: " + ex.Message;
                    throw new GQKNMessageException(messLog);
                }

                if (kqMaLoi != 0)
                {
                    messLog = "Tiếp nhận khiếu nại về VAS có lỗi. Mã lỗi: " + kqMaLoi + ", Thông báo lỗi : " + kqMaMoTa + " của khiếu nại: " + khieuNaiInfo.Id;
                    throw new GQKNMessageException(messLog);
                }
                else
                {
                    messLog = "Tiếp nhận khiếu nại về VAS thành công.";
                    resultService = 1000;
                }
                ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                {
                    CUser = khieuNaiInfo.NguoiXuLy,
                    ThaoTac = messLog,
                    PhongBanId = khieuNaiInfo.PhongBanXuLyId,
                    KhieuNaiId = khieuNaiInfo.Id,
                });
            }
            catch (GQKNMessageException ge)
            {
                resultService = 1001;
                messageVNPT += ge.Message;
                ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                {
                    CUser = khieuNaiInfo.NguoiXuLy,
                    ThaoTac = messageVNPT,
                    PhongBanId = khieuNaiInfo.PhongBanXuLyId,
                    KhieuNaiId = khieuNaiInfo.Id,
                });
                throw ge;
            }
            catch (Exception ex)
            {
                resultService = 1099;
                Utility.LogEvent(ex);
                messageVNPT = "Tiếp nhận khiếu nại về VAS có lỗi. Mã lỗi VNP: " + ex.Message;
                ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
                {
                    CUser = khieuNaiInfo.NguoiXuLy,
                    ThaoTac = messageVNPT,
                    PhongBanId = khieuNaiInfo.PhongBanXuLyId,
                    KhieuNaiId = khieuNaiInfo.Id,
                });
                throw ex;
            }
            finally
            {
                LogImpl.Log(khieuNaiInfo.Id, AIVietNam.Log.Entity.ObjTypeLog.SERVICE_VNPT_HNI, string.Empty, messageVNPT, AIVietNam.Log.Entity.ActionLog.Them_File_Khieu_Nai);
            }

            return new MessageInfo(resultService, messLog);

            // return resultService.ToString();
        }
        #endregion

        //public static void ChuyenPhanHoiVNPT(KhieuNaiInfo item, string NoiDungChuyenPhanHoi)
        //{
        //    //Đóng KN nếu KN từ VNPT
        //    if (item.KhieuNaiFrom == 1)
        //    {
        //        string messageVNPT = string.Empty;
        //        try
        //        {
        //            ServiceSyncVNPT_HNI.VINAPHONE objVNPT = new ServiceSyncVNPT_HNI.VINAPHONE();
        //            ServiceSyncVNPT_HNI.XACTHUC xacthuc = new ServiceSyncVNPT_HNI.XACTHUC();
        //            xacthuc.Username = "gqkn_vnp";
        //            xacthuc.Password = "gqkn_vnp";
        //            objVNPT.XACTHUCValue = xacthuc;

        //            int kqMaLoi = 0;
        //            string kqMaMoTa = "";

        //            //Get Id VNPT
        //            var idVnpt = new AIVietNam.GQKN.Impl.KhieuNaiRef_VNPTImpl().GetIdVNPTByIdVNP(item.Id);
        //            if (string.IsNullOrEmpty(idVnpt))
        //            {
        //                throw new Exception("Không tìm thấy mã khiếu nại từ hệ thống VNPT của khiếu nại: " + item.Id);
        //            }
        //            var flagVNPT = objVNPT.vnpUpdateKhieuNai(idVnpt, NoiDungChuyenPhanHoi, ref kqMaLoi, ref kqMaMoTa);

        //            if (!flagVNPT)
        //            {
        //                throw new Exception("Không chuyển phản hồi được khiếu nại từ service VNPT HNI. Mã lỗi VNPT HNI: " + kqMaLoi + ", Thông báo lỗi VNPT HNI: " + kqMaMoTa + " của khiếu nại: " + item.Id);
        //            }
        //            else
        //            {
        //                ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
        //                {
        //                    CUser = item.NguoiXuLy,
        //                    ThaoTac = "Chuyển phản hồi khiếu nại tự động về VNPT thành công.",
        //                    PhongBanId = item.PhongBanXuLyId,
        //                    KhieuNaiId = item.Id,
        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            messageVNPT = ex.Message;
        //            ServiceFactory.GetInstanceKhieuNai_Log().Add(new KhieuNai_LogInfo()
        //            {
        //                CUser = item.NguoiXuLy,
        //                ThaoTac = messageVNPT,
        //                PhongBanId = item.PhongBanXuLyId,
        //                KhieuNaiId = item.Id,
        //            });
        //        }
        //        finally
        //        {
        //            LogImpl.Log(AIVietNam.Log.Entity.ObjTypeLog.SERVICE_VNPT_HNI, AIVietNam.Log.Entity.ActionLog.Khoa_Khieu_Nai, messageVNPT);
        //        }
        //    }
        //}

        public static int DongKhieuNai(int id, string GhiChu, ref int resultService, int nguyenNhanLoiId, int chiTietLoiId, bool isCheckNoiDungXuLy = false, int DoHaiLong = 2)
        {
            KhieuNaiInfo item = ServiceFactory.GetInstanceKhieuNai().GetInfo(ConvertUtility.ToInt32(id));

            if (item != null)
            {
                AdminInfo admin = LoginAdmin.AdminLogin();

                if (string.IsNullOrEmpty(item.NguoiXuLy))
                {
                    if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Tiếp_nhận_khiếu_nại))
                    {
                        throw new GQKNMessageException("Bạn không có quyền tiếp nhận khiếu nại.");
                    }
                }

                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Đóng_khiếu_nại))
                {
                    if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Đóng_khiếu_nại_do_phòng_ban_mình_tạo_ra) && item.PhongBanTiepNhanId == admin.PhongBanId)
                    { throw new GQKNMessageException("Bạn không có quyền đóng khiếu nại " + id); }
                }

                if (nguyenNhanLoiId == 0 || chiTietLoiId < 0)
                {
                    throw new GQKNMessageException("Bạn phải chọn nguyên nhân lỗi và chi tiết lỗi");
                }

                // Kiểm tra nếu là khiếu nại giảm trừ cước mà chưa nhập kết quả xử lý thì không cho đóng
                DataSet dsKhieuNaiSoTien = ServiceFactory.GetInstanceKhieuNai_SoTien().CheckValidCloseKhieuNai(id);
                if (dsKhieuNaiSoTien != null)
                {
                    if (dsKhieuNaiSoTien.Tables.Count > 0 && dsKhieuNaiSoTien.Tables[0].Rows.Count > 0)
                    {
                        if (ConvertUtility.ToInt32(dsKhieuNaiSoTien.Tables[0].Rows[0][0]) == 0)
                        {
                            throw new GQKNMessageException("Bạn phải nhập kết quả giải quyết khiếu nại cước của khiếu nại có mã " + id);
                        }
                    }

                    if (dsKhieuNaiSoTien.Tables.Count > 1 && dsKhieuNaiSoTien.Tables[1].Rows.Count > 0)
                    {
                        throw new GQKNMessageException("Khiếu nại có mã " + id + " chưa xác nhận bù tiền cho các trường hợp giảm trừ");
                    }
                }
                //bool isValidToClose = new KhieuNai_KetQuaXuLyImpl().IsKetQuaXuLyValidToClose(id);
                //if (!isValidToClose)
                //{
                //    throw new GQKNMessageException("Bạn phải nhập kết quả giải quyết khiếu nại cước của khiếu nại có mã " + id);
                //}

                if (isCheckNoiDungXuLy)
                {
                    string selectClause = "Top 1 *";
                    string whereClause = string.Format("KhieuNaiId={0}", id);
                    string orderClause = "Id desc";
                    var lstCheck = ServiceFactory.GetInstanceKhieuNai_BuocXuLy().GetListDynamic(selectClause, whereClause, orderClause);
                    if (lstCheck != null && lstCheck.Count > 0)
                    {
                        var itemCheck = lstCheck[0];
                        if (itemCheck.IsAuto || !itemCheck.CUser.Equals(admin.Username))
                        {
                            throw new GQKNMessageException("Bạn phải nhập nội dung xử lý.");
                        }
                        else
                        {
                            GhiChu = itemCheck.NoiDung;
                        }
                    }
                    else
                        throw new GQKNMessageException("Bạn phải nhập nội dung xử lý.");
                }

                KhieuNai_ActivityInfo activityCurr = ServiceFactory.GetInstanceKhieuNai_Activity().GetActivityCurrent(item.Id);
                activityCurr.ActivityTruoc = activityCurr.Id;
                activityCurr.GhiChu = GhiChu;
                activityCurr.HanhDong = (byte)KhieuNai_Actitivy_HanhDong.Đóng_KN;
                activityCurr.IsCurrent = true;
                activityCurr.NguoiDuocPhanHoi = string.Empty;
                activityCurr.NguoiXuLy = admin.Username;
                activityCurr.NguoiXuLyTruoc = admin.Username;
                activityCurr.PhongBanXuLyTruocId = admin.PhongBanId;

                // HaiPH                
                activityCurr.NgayTiepNhan_NguoiXuLyTruoc = activityCurr.NgayTiepNhan_NguoiXuLy;
                activityCurr.NgayTiepNhan_PhongBanXuLyTruoc = activityCurr.NgayTiepNhan;
                activityCurr.NgayQuaHan_PhongBanXuLyTruoc = activityCurr.NgayQuaHan;

                item.NoiDungXuLyDongKN = GhiChu;
                bool flag = false;

                PhongBanInfo PhongBanItem = ServiceFactory.GetInstancePhongBan().GetInfo(admin.PhongBanId);

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        ServiceFactory.GetInstanceKhieuNai().DongKhieuNai(item.Id, item.NoiDungXuLyDongKN, (byte)KhieuNai_TrangThai_Type.Đóng, DateTime.Now, admin.Id, admin.Username, DoHaiLong, nguyenNhanLoiId, chiTietLoiId);
                        ServiceFactory.GetInstanceKhieuNai_Activity().UpdateCurentActivity(activityCurr.Id, item.Id, admin.Username);
                        ServiceFactory.GetInstanceKhieuNai_Activity().Add(activityCurr);

                        // Đóng KN nếu KN từ VNPT
                        if (PhongBanItem != null && PhongBanItem.IsChuyenHNI)
                            resultService = BuildKhieuNai.DongKhieuNaiVNPT(admin, item, true);

                        scope.Complete();
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        Utility.LogEvent(ex);
                        return 1;
                    }
                }

                if (flag)
                {


                    KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                    buocXuLyInfo.NoiDung = "Đóng khiếu nại.";
                    buocXuLyInfo.LUser = item.NguoiXuLy;
                    buocXuLyInfo.KhieuNaiId = item.Id;
                    buocXuLyInfo.NguoiXuLyId = admin.Id;
                    buocXuLyInfo.PhongBanXuLyId = admin.PhongBanId;
                    buocXuLyInfo.IsAuto = true;

                    if (!isCheckNoiDungXuLy)
                        ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(new KhieuNai_BuocXuLyInfo() { NguoiXuLyId = admin.Id, PhongBanXuLyId = admin.PhongBanId, LUser = item.NguoiXuLy, NoiDung = GhiChu, IsAuto = false, KhieuNaiId = item.Id });

                    ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);

                    BuildKhieuNai_Log.LogKhieuNai(ConvertUtility.ToInt32(id), "Đóng khiếu nại", "Trạng thái", Enum.GetName(typeof(KhieuNai_TrangThai_Type), item.TrangThai).Replace("_", " "), Enum.GetName(typeof(KhieuNai_TrangThai_Type), (byte)KhieuNai_TrangThai_Type.Đóng).Replace("_", " "));
                }

            }
            else
            {
                throw new GQKNMessageException("Khiếu nại không hợp lệ.");
            }
            return 0;
        }

        public static int UpdateKhieuNaiToHangLoat(string listID, AdminInfo userInfo)
        {

            try
            {
                if (!string.IsNullOrEmpty(listID))
                {
                    listID = listID.Substring(0, listID.Length - 1);
                    ServiceFactory.GetInstanceKhieuNai().UpdateDynamic("NguoiXuLyId = " + userInfo.Id + ", NguoiXuLy=N'" + userInfo.Username + "', LDate=getdate(),LUser=N'" + userInfo.Username + "',KNHangLoat=1", "Id in " + "(" + listID + ")");
                }
                return 1;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return -1;
            }

        }

        public static bool TiepNhanKhieuNai(int Id, AdminInfo infoUser)
        {
            try
            {
                var info = ServiceFactory.GetInstanceKhieuNai().GetInfo(Id);
                if (!BuildPhongBan_Permission.CheckPermission(PermissionSchemes.Xử_lý_khiếu_nại))
                {
                    return false;
                }
                if (info.TrangThai == (byte)KhieuNai_TrangThai_Type.Đóng)
                {
                    return false;
                }
                if (info.NguoiXuLy == string.Empty && info.PhongBanXuLyId == infoUser.PhongBanId)
                {
                    info.NguoiXuLyId = infoUser.Id;
                    info.NguoiXuLy = infoUser.Username;
                    if (info.TrangThai == (byte)KhieuNai_TrangThai_Type.Chờ_xử_lý)
                    {
                        info.TrangThai = (byte)KhieuNai_TrangThai_Type.Đang_xử_lý;
                    }
                    var flag = false;
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            ServiceFactory.GetInstanceKhieuNai().UpdateTrangThaiTiepNhan(Id, info.TrangThai, infoUser.Id, infoUser.Username, 0, string.Empty);
                            ServiceFactory.GetInstanceKhieuNai_Activity().UpdateTiepNhan(Id, infoUser.Username, infoUser.PhongBanId);



                            scope.Complete();
                            flag = true;
                        }
                        catch
                        {
                            return false;
                        }
                    }

                    if (flag)
                    {
                        KhieuNai_BuocXuLyInfo buocXuLyInfo = new KhieuNai_BuocXuLyInfo();
                        buocXuLyInfo.NoiDung = "Tiếp nhận khiếu nại " + ServiceFactory.GetInstancePhongBan().GetNamePhongBan(info.PhongBanXuLyId);
                        buocXuLyInfo.LUser = infoUser.Username;
                        buocXuLyInfo.KhieuNaiId = Id;
                        buocXuLyInfo.NguoiXuLyId = infoUser.Id;
                        buocXuLyInfo.PhongBanXuLyId = infoUser.PhongBanId;
                        buocXuLyInfo.IsAuto = true;
                        ServiceFactory.GetInstanceKhieuNai_BuocXuLy().Add(buocXuLyInfo);
                        BuildKhieuNai_Log.LogKhieuNai(Id, "Tiếp nhận khiếu nại", "Người xử lý", "", infoUser.Username);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                return false;
            }

        }

        public static int CountKhieuNaiPhanHoi(string username)
        {
            try
            {
                string key = string.Format("KNPH_{0}", username.ToLower());
                if (CustomCache.Exists(key))
                {
                    return Convert.ToInt32(CustomCache.Get(key));
                }
                else
                {
                    var Id = ServiceFactory.GetInstanceNguoiSuDung().GetIdByUsername(username);
                    var result = 0;
                    if (Id != 0)
                    {
                        result = ServiceFactory.GetInstanceKhieuNai().CountKhieuNaiPhanHoi(Id);
                    }

                    CustomCache.Add(key, result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEventService(ex);
                return 0;
            }
        }
    }
}
