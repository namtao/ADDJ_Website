namespace GQKN.Archive.Core
{
    public class Constant
    {
        // Connection String
        internal const string CONNECTION_STRING = "VinaphoneCCSOBConnectionstring";

        public const string FILE_UPLOAD_EXTENSIVE = "*.xls|*.xlsx|*.xlt";

        // Session name
        public const string SessionNameAccountAdmin = "AccountAdmin";

        public const string SessionNameAccount = "Account";

        public const string SESSION_PERMISSION_SCHEMES = "Permission";

        public const string SESSION_KEY_LANGUAGE = "SESSION_KEY_LANGUAGE";

        public const string QueryLanguages = "QUERY_LANG";

        public const string LANGUAGE_DEFAULT = "vi-VN";

        public const string PREFIX_KEY_PERMISSION = "PER_";

        public const string PREFIX_KEY_CACHE_USERRIGHT = "MUR_";

        public const string PREFIX_KEY_CACHE_SYSTEM = "SYS_";

        public const string PREFIX_PHIEU_KHIEU_NAI = "PA-";

        // Message throw exception
        public const string MESSAGE_NOT_RIGHT = "Bạn không có quyền thực hiện thao tác trên form này. Liên hệ Administrator";

        public const string MESSAGE_ERROR = "Có lỗi xảy ra tại function ";

        public const string MESSSAGE_NOT_PERMISSION = "Phòng ban hoặc người sử dụng chưa được phân quyền chức năng này. Vui lòng liên hệ người quản trị.";

        public const string MESSAGE_SERVER_QUA_TAI = "Server đang quá tải, bạn vui lòng thực hiện lại thao tác sau ít phút.";

        public const string MESSAGE_DU_LIEU_CHUA_HOP_LE = "Dữ liệu chưa hợp lệ, bạn vui lòng kiểm tra lại.";

        public const string MESSAGE_HET_PHIEN_LAM_VIEC = "Phiên làm việc của bạn đã hết, bạn vui lòng đăng nhập lại.";

        public const string MESSAGE_EXCEPTION_SERVICE_VAS = "Không kết nối được service {0} của hệ thống VAS. Thông báo lỗi từ VAS: ";

        public const string MESSAGE_EXCEPTION_SERVICE_TTTC = "Không kết nối được service {0} của hệ thống TTTC. Thông báo lỗi từ TTTC: ";

        public const string MESSAGE_DATA_EMPTY_SERVICE_VAS = "Không lấy được dữ liệu từ serivce {0} của hệ thống VAS";

        public const string MESSAGE_DATA_EMPTY_SERVICE_TTTC = "Không lấy được dữ liệu từ serivce {0} của hệ thống TTTC";

        public const int LIMIT_CHARACTER_EXCEL_CELL = 32000; // Số lượng tối đa ký tự trong 1 cell của excel khi xuất excel = dll using Aspose.Cells;

        // public const string MESSAGE_JSON_AJAX = "{ErrorId:{0}, Content:\"{1}\", Message:\"{2}\"}";

        public static object ObjLockFull = new object();
    }
}
