using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIVietNam.Vinaphone.CrossSellOutBound.Entity;
using AIVietNam.Core;
using AIVietNam.Log.Entity;
using AIVietNam.Log.Impl;
using AIVietNam.GQKN.Entity;

namespace AIVietNam.GQKN.Impl
{
    public class SubinfoImpl
    {
        public static AIVietNam.Vinaphone.CrossSellOutBound.Impl.Subinfo.SubInfoInterfaceClient subInfoClient = null;

        public const string MESSAGE_ERROR_DATA_1 = "No data found";

        public const string MESSAGE_ERROR_DATA_OTHER = "System error";

        public const string MESSAGE_FUNCTION_getBasicInfo = "lấy thông tin chung thuê bao";

        public const string MESSAGE_FUNCTION_getBasicService = "lấy thông tin dịch vụ cơ bản";

        public const string MESSAGE_FUNCTION_getSubHistory = "lấy thông tin lịch sử thuê bao";

        public const string MESSAGE_FUNCTION_get3GHistory = "lấy thông tin lịch sử 3G";

        public const string MESSAGE_FUNCTION_getDeliverSMHistory = "lấy thông tin tin nhắn KH gửi";

        public const string MESSAGE_FUNCTION_getSubmitSMHistory = "lấy thông tin tin nhắn tổng đài gửi";

        public const string MESSAGE_FUNCTION_getPPSCardInfo = "lấy thông tin thẻ cào";

        public const string MESSAGE_FUNCTION_getPPSCardHistoryByMsisdn = "lấy lịch sử nạp thẻ";

        public const string MESSAGE_FUNCTION_getSubmitSMHistoryByDate = "lấy thông tin tin nhắn tổng đài gửi";

        public const string MESSAGE_FUNCTION_getDeliverSMHistoryByDate = "lấy thông tin tin nhắn KH gửi";

        public const string MESSAGE_FUNCTION_getAloInfo = "lấy thông tin khuyến mại alo";

        public const string MESSAGE_FUNCTION_getPackagePromoPPS = "lấy thông tin gói khuyến mại";

        public SubinfoImpl()
        {
            /* sửa App.config
             * 
             * <security mode="TransportCredentialOnly">

                    <transport clientCredentialType="Basic" proxyCredentialType="None"

                        realm="" />

                    <message clientCredentialType="UserName" algorithmSuite="Default" />

                </security>
             * 
             */
            //subInfoClient.ClientCredentials.UserName.UserName = "AI";
            //subInfoClient.ClientCredentials.UserName.Password = "ai#cskh";
            //subInfoClient.ClientCredentials.SupportInteractive = true;

            try
            {
                subInfoClient = new AIVietNam.Vinaphone.CrossSellOutBound.Impl.Subinfo.SubInfoInterfaceClient();

                subInfoClient.ClientCredentials.UserName.UserName = "AI";
                subInfoClient.ClientCredentials.UserName.Password = "ai#cskh";
                subInfoClient.ClientCredentials.SupportInteractive = true;
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format("Không kết nối được service {0} của hệ thống TTTC.", MESSAGE_FUNCTION_getBasicInfo));
            }
        }

        public BasicInfoFromSubinfo getBasicInfo(string MSISDN, string UserName, string UserIP, string UserNote)
        {
            BasicInfoFromSubinfo item = null;
            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getBasicInfo(new Random().Next(1, 100000), MSISDN, UserName, UserIP, UserNote);
                if (string.IsNullOrEmpty(strReturn))
                    return null;
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicInfo", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                throw new Exception("Số thuê bao này chưa đăng ký. Bạn có thể tiếp tục tạo khiếu nại cho số thuê bao này.",ex);
            }

            try
            {
                string[] spl = strReturn.Split('|');
                if (spl.Length > 12)
                {
                    item = new BasicInfoFromSubinfo();
                    item.ErrorID = spl[0];
                    if (item.ErrorID == "0")
                    {
                        item.SO_TB = spl[1].ToString();
                        item.SO_MSIN = spl[2].ToString();
                        item.TRANG_THAI = spl[3].ToString();
                        item.GOI_DI = spl[4].ToString();
                        item.GOI_DEN = spl[5].ToString();
                        item.TEN_LOAI = spl[6].ToString().Trim();

                        item.MA_TINH = spl[7].ToString();
                        item.NGAY_KH = spl[8].ToString();
                        item.SO_PIN = spl[9].ToString();
                        item.SO_PUK = spl[10].ToString();
                        item.PIN2 = spl[11].ToString();
                        item.PUK2 = spl[12].ToString();
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicInfo) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicInfo) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicInfo));
            }

            return item;
        }

        public BasicServiceFromSubinfo getBasicService(string MSISDN, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID| SO_TB| MA_DV1:TEN_DV1|..|MA_DVn:TEN_DVn";

            BasicServiceFromSubinfo item = null;
            ServiceFromSubinfo child = null;
            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getBasicService(1, MSISDN, UserName, UserIP, "CSKH lấy thông tin dịch vụ cơ bản " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicService));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicService) + ex.Message);
            }

            try
            {
                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new BasicServiceFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListService = new List<ServiceFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(':');
                            if (spl2.Length > 0)
                            {
                                child = new ServiceFromSubinfo();
                                child.MA_DV = spl2[0];
                                child.TEN_DV = spl2[1];
                                item.ListService.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicService) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicService) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicService));
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicService));
            }
            return item;
        }

        public SubHistoryFromSubinfo getSubHistory(string MSISDN, string UserName, string UserIP, string UserNote, bool viewAll)
        {
            //return "ErrorID| SO_TB|NGAY_THANG (dd/MM/yyyy HH:mm:ss), MA_DV, THAO_TAC, GHI_CHU, NGUOI_DUNG, SO_MSIN_CU, SO_MSIN_MOI, MA_TINH_CU, MA_TINH_MOI|NGAY_THANG (dd/MM/yyyy HH:mm:ss), MA_DV, THAO_TAC, GHI_CHU, NGUOI_DUNG, SO_MSIN_CU, SO_MSIN_MOI, MA_TINH_CU, MA_TINH_MOI";
            SubHistoryFromSubinfo item = null;
            HistoryFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                if (viewAll)
                    strReturn = subInfoClient.getSubHistory(1, MSISDN, UserName, UserIP, "FULL");
                else
                    strReturn = subInfoClient.getSubHistory(1, MSISDN, UserName, UserIP, "CSKH lấy lịch sử thuê bao " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubHistory));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubHistory) + ex.Message);
            }



            try
            {


                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new SubHistoryFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListHistory = new List<HistoryFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new HistoryFromSubinfo();
                                child.NGAY_THANG = spl2[0];
                                child.MA_DV = spl2[1];
                                child.THAO_TAC = spl2[2];
                                child.GHI_CHU = spl2[3];
                                child.NGUOI_DUNG = spl2[4];
                                child.SO_MSIN_CU = spl2[5];
                                child.SO_MSIN_MOI = spl2[6];
                                child.MA_TINH_CU = spl2[7];
                                child.MA_TINH_MOI = spl2[8];

                                item.ListHistory.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubHistory) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubHistory) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubHistory));
                }

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getBasicInfo));
            }
            return item;
        }

        public History3GFromSubinfo get3GHistory(string MSISDN, string UserName, string UserIP, string UserNote, bool viewAll)
        {
            //return "ErrorID| SO_TB|MA_DV, TEN_GOI, NGAY_BAT_DAU(dd/MM/yyyy HH:mm:ss), NGAY_KET_THUC(dd/MM/yyyy HH:mm:ss), ACTIVE, GIA_HAN, LOAI_TB (0: trả trước, 1: trả sau) |MA_DV, TEN_GOI, NGAY_BAT_DAU(dd/MM/yyyy HH:mm:ss), NGAY_KET_THUC(dd/MM/yyyy HH:mm:ss), ACTIVE, GIA_HAN, LOAI_TB (0: trả trước, 1: trả sau) ";
            History3GFromSubinfo item = null;
            History3GDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                if (viewAll)
                    strReturn = subInfoClient.get3GHistory(1, MSISDN, UserName, UserIP, "FULL");
                else
                    strReturn = subInfoClient.get3GHistory(1, MSISDN, UserName, UserIP, "CSKH lấy lịch sử thuê bao " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    return null;
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_get3GHistory) + ex.Message);
            }

            try
            {


                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new History3GFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListHistory3G = new List<History3GDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new History3GDetailFromSubinfo();
                                child.MA_DV = spl2[0];
                                child.TEN_GOI = spl2[1];
                                child.NGAY_BAT_DAU = spl2[2];
                                child.NGAY_KET_THUC = spl2[3];
                                child.ACTIVE = spl2[4];
                                child.GIA_HAN = spl2[5];
                                child.LOAI_TB = spl2[6];

                                item.ListHistory3G.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_get3GHistory) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_get3GHistory) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_get3GHistory));
            }
            return item;
        }

        public DeliverSMHistoryFromSubinfo getDeliverSMHistory(string requestId, string MSISDN, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID| SO_TB|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)";
            DeliverSMHistoryFromSubinfo item = null;
            DeliverSMHistoryDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getDeliverSMHistory(1, MSISDN, UserName, UserIP, "CSKH lấy thông tin tin nhắn khách hàng gửi tổng đài " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistory));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistory) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new DeliverSMHistoryFromSubinfo();
                    item.SO_TB = spl[1];
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListDeliverSMHistory = new List<DeliverSMHistoryDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new DeliverSMHistoryDetailFromSubinfo();
                                child.DIA_CHI_NHAN = spl2[0];
                                child.DIA_CHI_GUI = spl2[1];
                                child.NOI_DUNG = spl2[2];
                                child.THOI_GIAN = spl2[3];

                                item.ListDeliverSMHistory.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistory) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistory) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistory));
                }

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistory));
            }
            return item;
        }

        public SubmitSMHistoryFromSubinfo getSubmitSMHistory(string requestId, string MSISDN, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID| SO_TB|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)";
            SubmitSMHistoryFromSubinfo item = null;
            SubmitSMHistoryDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getSubmitSMHistory(1, MSISDN, UserName, UserIP, "CSKH lấy thông tin tin nhắn tổng đài gửi khách hàng " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistory));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistory) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new SubmitSMHistoryFromSubinfo();
                    item.SO_TB = spl[1];
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListSubmitSMHistory = new List<SubmitSMHistoryDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new SubmitSMHistoryDetailFromSubinfo();
                                child.DIA_CHI_NHAN = spl2[0];
                                child.DIA_CHI_GUI = spl2[1];
                                child.NOI_DUNG = spl2[2];
                                child.THOI_GIAN = spl2[3];

                                item.ListSubmitSMHistory.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistory) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistory) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistory));
                }

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistory));
            }
            return item;
        }

        public PPSCardInfoFromSubinfo getPPSCardInfo(string requestId, string MSISDN, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID|SO_TB, NGAY_GIO_NAP_THE (dd/MM/yyyy HH:mm:ss), SO_SERIAL, TRANG_THAI, MENH_GIA_THE|SO_TB, NGAY_GIO_NAP_THE (dd/MM/yyyy HH:mm:ss), SO_SERIAL, TRANG_THAI, MENH_GIA_THE";


            PPSCardInfoFromSubinfo item = null;
            PPSCardInfoDetailFromSubinfo child = null;


            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getPPSCardInfo(1, MSISDN, UserName, UserIP, "CSKH lấy thông tin thẻ cào " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardInfo));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardInfo) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new PPSCardInfoFromSubinfo();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListPPSCardInfo = new List<PPSCardInfoDetailFromSubinfo>();
                        for (int i = 1; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new PPSCardInfoDetailFromSubinfo();
                                child.SO_TB = spl2[0];
                                child.NGAY_GIO_NAP_THE = spl2[1];
                                child.SO_SERIAL = spl2[2];
                                child.TRANG_THAI = spl2[3];
                                child.MENH_GIA_THE = spl2[4];

                                item.ListPPSCardInfo.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardInfo) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardInfo) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardInfo));
                }

            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardInfo));
            }
            return item;
        }

        public PPSCardHistoryByMsisdnFromSubinfo getPPSCardHistoryByMsisdn(string requestId, string MSISDN, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID|SO_TB|NGAY_NAP | MENH_GIA | TKC_TRUOC_KHI_NAP | TKC_SAU_KHI_NAP | TKKM_TRUOC_KHI_NAP | TKKM_SAU_KHI_NAP | TKKM1_TRUOC_KHI_NAP | TKKM1_SAU_KHI_NAP | TKKM2_TRUOC_KHI_NAP | TKKM2_SAU_KHI_NAP | PHUONG_THUC_NAP|NGAY_NAP | MENH_GIA | TKC_TRUOC_KHI_NAP | TKC_SAU_KHI_NAP | TKKM_TRUOC_KHI_NAP | TKKM_SAU_KHI_NAP | TKKM1_TRUOC_KHI_NAP | TKKM1_SAU_KHI_NAP | TKKM2_TRUOC_KHI_NAP | TKKM2_SAU_KHI_NAP | PHUONG_THUC_NAP";

            PPSCardHistoryByMsisdnFromSubinfo item = null;
            PPSCardHistoryByMsisdnDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getPPSCardHistoryByMsisdn(1, MSISDN, UserName, UserIP, "CSKH lấy thông tin lịch sử nạp thẻ " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    return null;
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardHistoryByMsisdn) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new PPSCardHistoryByMsisdnFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListPPSCardHistoryByMsisdn = new List<PPSCardHistoryByMsisdnDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new PPSCardHistoryByMsisdnDetailFromSubinfo();
                                child.NGAY_NAP = spl2[0];
                                child.MENH_GIA = spl2[1];
                                child.TKC_TRUOC_KHI_NAP = spl2[2];
                                child.TKC_SAU_KHI_NAP = spl2[3];
                                child.TKKM_TRUOC_KHI_NAP = spl2[4];
                                child.TKKM1_SAU_KHI_NAP = spl2[5];
                                child.TKKM2_TRUOC_KHI_NAP = spl2[6];
                                child.TKKM2_SAU_KHI_NAP = spl2[7];
                                child.PHUONG_THUC_NAP = spl2[8];

                                item.ListPPSCardHistoryByMsisdn.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardHistoryByMsisdn) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardHistoryByMsisdn) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getPPSCardHistoryByMsisdn));
            }
            return item;
        }

        public SubmitSMHistoryByDateFromSubinfo getSubmitSMHistoryByDate(string requestId, string MSISDN, string startDate, string endDate, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID| SO_TB|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)";

            SubmitSMHistoryByDateFromSubinfo item = null;
            SubmitSMHistoryByDateDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getSubmitSMHistoryByDate(1, MSISDN, startDate, endDate, UserName, UserIP, "CSKH lấy thông tin tin nhắn tổng đài gửi khách hàng theo ngày " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistoryByDate));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistoryByDate) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new SubmitSMHistoryByDateFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListSubmitSMHistoryByDate = new List<SubmitSMHistoryByDateDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new SubmitSMHistoryByDateDetailFromSubinfo();
                                child.DIA_CHI_NHAN = spl2[0];
                                child.DIA_CHI_GUI = spl2[1];
                                child.NOI_DUNG = spl2[2];
                                child.THOI_GIAN = spl2[3];

                                item.ListSubmitSMHistoryByDate.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistoryByDate) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistoryByDate) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistoryByDate));
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getSubmitSMHistoryByDate));
            }
            return item;
        }

        public DeliverSMHistoryByDateFromSubinfo getDeliverSMHistoryByDate(string requestId, string MSISDN, string startDate, string endDate, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID| SO_TB|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)|DIA_CHI_NHAN, DIA_CHI_GUI, NOI_DUNG, THOI_GIAN(dd/MM/yyyy HH:mm:ss)";
            DeliverSMHistoryByDateFromSubinfo item = null;
            DeliverSMHistoryByDateDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getDeliverSMHistoryByDate(1, MSISDN, startDate, endDate, UserName, UserIP, "CSKH lấy thông tin tin nhắn khách hàng gửi lên tổng đài theo ngày " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistoryByDate));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistoryByDate) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new DeliverSMHistoryByDateFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListDeliverSMHistoryByDate = new List<DeliverSMHistoryByDateDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new DeliverSMHistoryByDateDetailFromSubinfo();
                                child.DIA_CHI_NHAN = spl2[0];
                                child.DIA_CHI_GUI = spl2[1];
                                child.NOI_DUNG = spl2[2];
                                child.THOI_GIAN = spl2[3];

                                item.ListDeliverSMHistoryByDate.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistoryByDate) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistoryByDate) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistoryByDate));
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getDeliverSMHistoryByDate));
            }
            return item;
        }

        public AloInfoFromSubinfo getAloInfo(string requestId, string MSISDN, string UserName, string startDate, int accAlo, string UserIP, string UserNote)
        {
            //return "ErrorID| SO_TB|Du_Lieu_ALO";

            AloInfoFromSubinfo item = null;
            AloInfoDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getAloInfo(1, MSISDN, startDate, accAlo, UserName, UserIP, "CSKH lấy thông tin khuyến mại ALO " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getAloInfo));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getAloInfo) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new AloInfoFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.Du_Lieu_ALO = new List<AloInfoDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new AloInfoDetailFromSubinfo();
                                child.Name = spl2[0];
                                child.SoTienCongHangThang = spl2[1];
                                child.DataCongHangThang = spl2[2];
                                child.SoThangConKM = spl2[3];
                                child.TongSoThangKM = spl2[4];
                                item.Du_Lieu_ALO.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getAloInfo) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getAloInfo) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getAloInfo));
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getAloInfo));
            }
            return item;
        }

        public PackagePromoPPSFromSubinfo getPackagePromoPPS(string requestId, string MSISDN, string UserName, string UserIP, string UserNote)
        {
            //return "ErrorID| SO_TB|Tên gói KM 1|…|Tên gói KM";
            PackagePromoPPSFromSubinfo item = null;
            PackagePromoPPSDetailFromSubinfo child = null;

            string strReturn = string.Empty;
            try
            {
                strReturn = subInfoClient.getPackagePromoPPS(1, MSISDN, UserName, UserIP, "CSKH lấy thông tin các gói khuyến mại " + UserNote);

                if (string.IsNullOrEmpty(strReturn))
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getPackagePromoPPS));
            }
            catch (Exception ex)
            {
                LogImpl.Log(0, ObjTypeLog.System, "getBasicService", "STB: " + MSISDN + Environment.NewLine + ex.Message, ActionLog.Gọi_Dịch_Vụ_Subinfo);
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPackagePromoPPS) + ex.Message);
            }

            try
            {

                string[] spl = strReturn.Split('|');
                if (spl.Length > 1)
                {
                    item = new PackagePromoPPSFromSubinfo();
                    item.SO_TB = spl[1].ToString();
                    item.ErrorID = spl[0];
                    if (spl[0].ToString() == "0")
                    {
                        item.ListPackagePromoPPS = new List<PackagePromoPPSDetailFromSubinfo>();
                        for (int i = 2; i <= spl.Length - 1; i++)
                        {
                            string[] spl2 = spl[i].Split(',');
                            if (spl2.Length > 0)
                            {
                                child = new PackagePromoPPSDetailFromSubinfo();
                                child.TEN_GOI = spl2[0];

                                item.ListPackagePromoPPS.Add(child);
                            }
                        }
                    }
                    else if (item.ErrorID == "1")
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPackagePromoPPS) + MESSAGE_ERROR_DATA_1);
                    }
                    else
                    {
                        throw new Exception(string.Format(Constant.MESSAGE_EXCEPTION_SERVICE_TTTC, MESSAGE_FUNCTION_getPackagePromoPPS) + MESSAGE_ERROR_DATA_OTHER);
                    }
                }
                else
                {
                    throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getPackagePromoPPS));
                }
            }
            catch (Exception ex)
            {
                Utility.LogEvent(ex);
                throw new Exception(string.Format(Constant.MESSAGE_DATA_EMPTY_SERVICE_TTTC, MESSAGE_FUNCTION_getPackagePromoPPS));
            }
            return item;
        }
    }
}
