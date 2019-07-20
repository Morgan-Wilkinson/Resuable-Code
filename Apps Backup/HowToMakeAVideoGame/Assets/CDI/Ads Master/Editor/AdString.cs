using UnityEditor;

namespace cdi.ad
{
    public static class AdString
    {
        public const string VersionName = "2.1.5";

        public static Language language
        {
            get
            {
                return (Language)EditorPrefs.GetInt("cdi.ad.current.language", 0);
            }

            set
            {
                EditorPrefs.SetInt("cdi.ad.current.language", (int)value);
            }
        }

        public readonly static Pharse confirm_remove = new Pharse(
            "Are you sure you want to remove item?"
            , "Bạn có chắc chắn muốn xóa");

        public readonly static Pharse data_config_version_warn = new Pharse(
            "Client doesn't use the config is loaded from remote if it has difference data version."
            , "Client sẽ không sử dụng file config nếu nó khác data version.");

        public readonly static Pharse debug_warning = new Pharse(
            "Remember turn them off before build production."
            , "Đừng quên tắt hết các biến debug trước khi build bản release.");

        public readonly static Pharse delete_interstitial_hint = new Pharse(
            "Remove interstitial to load at beginning."
            , "Thêm một rewarded video được load lúc bắt đầu");

        public readonly static Pharse empty_list_hint = new Pharse(
            "Empty, click \"+\" to add an item."
            , "Danh sách trống, click \"+\" để thêm item.");

        public readonly static Pharse download_sdk_btn_hint = new Pharse(
            "Download SDK"
            , "Tải SDK");

        public readonly static Pharse open_dashboard_btn_hint = new Pharse(
            "Open Dashboard Network"
            , "Đi đến trang quản lý");

        public readonly static Pharse infor_popup_title = new Pharse(
            "Tip"
            , "Thông tin");

        public readonly static Pharse infor_popup_ok = new Pharse(
            "Got it"
            , "Ẩn");

        public readonly static Pharse profile_infor = new Pharse(
            "Profile allow you determine what provider of type ad will be choose to be show."
            , "Profile cho phép bạn xác định nhà cung cấp ads nào được chọn để hiển thị cho từng loại ad");

        public readonly static Pharse interstitial_limit_infor = new Pharse(
            "There are some way to reduce interstitial to improve user experience."
            , "Một số cách hạn chế hiển thị interstitial để cải thiện trải nghiệm người dùng.");

        public readonly static Pharse profile_mode_infor = new Pharse(
            "Change profile mode if you wanna change the way to determine ad providers.\n1) Priority: Pick from first providers if ad is ready.\n2) Sequence: Pick from top to bottom providers.3) Random: Show ads by random provider."
            , "Cách chọn nhà cung cấp quảng cáo cho từng loại.\n1) Priority: Ưu tiên chọn nhà cung cấp ở đầu danh sách xuống cuối.\n2) Sequence: Chọn lần lượt từng nhà cung cấp (nếu đã có ads hiển thị) theo thứ tự từ trên xuống.3) Random: hiển thị ads bởi ngẫu nhiên 1 provider.");

        public readonly static Pharse test_mode_infor = new Pharse(
            "Active test mode by ad provider sdk for development build."
            , "Kích hoạt test mode khi build cho test.");

        public readonly static Pharse remote_config_infor = new Pharse(
            "Allow you change the config from remote.\n1) Export config to json (Click Export)\n2) Put the file on your server or Dropbox, Drive...\n3)Put url to download file here and active remote ads.\nNow app always try to load remote config to use before use local config."
            , "Cho phép bạn thay đổi cấu hình qua remote file.");

        public readonly static Pharse key_label = new Pharse(
            "Key (optional)"
            , "Key (optional)");

        public readonly static Pharse key_infor = new Pharse(
            "It's an unique key to determine ad if you have multiple ads. Leave it blank if you have single ads."
            , "Là một key được sử dụng để xác định ad trong trường hợp có nhiều thực thể cho một loại ad. Không cần set trong trường hợp bạn chỉ sử dụng một ad.");

        public readonly static Pharse active_provider_tip = new Pharse(
            "Active the ad provider. All scripts used for provider will be active. Ensure that you have imported SDK."
            , "Kích hoạt nhà cung cấp quảng cáo. Các script sẽ báo lỗi nếu như bạn chưa import sdk của nhà cung cấp đó.");

        public readonly static Pharse language_label = new Pharse(
            "Language"
            , "Ngôn ngữ");

        public readonly static Pharse language_tooltip = new Pharse(
            "Language use for editor to help developer understand asset."
            , "Ngôn ngữ sử dụng cho Editor.");

        public readonly static Pharse interstitial_time_limit_tooltip = new Pharse(
            "Enable time limit for interstitial at all ad providers."
            , "Bật giới hạn thời gian cho interstitial.");

        public readonly static Pharse zero_timescale_tooltip = new Pharse(
            "Set zero for timescale while interstitial or rewarded video is showing. Last value of timescale will be saved."
            , "Đặt timescale là 0 khi đang hiển thị interstitial hoặc rewarded video. Giá trị trước đó của timescale sẽ được khôi phục.");

        public readonly static Pharse first_open_delay_tooltip = new Pharse(
            "Ignore all of show ad for interstitial at first open app for a while (seconds). Set 0 to disable the function."
            , "Bỏ qua toàn bộ hành động hiển thị interstitial ở lần đầu bật app trong một khoảng thời gian (seconds). Đặt bằng 0 để bỏ qua giới hạn này.");

        public readonly static Pharse start_delay_tooltip = new Pharse(
            "Ignore all of show ad for interstitial at beginning of start app for a while (seconds). Set 0 to disable the function."
            , "Bỏ qua toàn bộ hành động hiển thị interstitial khi bắt đầu bật app trong một khoảng thời gian (seconds). Đặt bằng 0 để bỏ qua giới hạn này.");

        public readonly static Pharse between_2ads_tooltip = new Pharse(
            "Ignore showing ad for interstitial if the duration from last showing ad it not enough that value. Use to improve user experience and reduce impressions. Set 0 to disable the function."
            , "Bỏ qua hiển thị interstitial nếu quãng thời gian từ lần show gần nhất đến lúc hiển thị chưa đạt giá trị này. Sử dụng tính năng này để giảm số lần hiển thị interstitial và cải thiện trải nghiệm người dùng. Đặt bằng 0 để bỏ qua giới hạn này.");

        public readonly static Pharse skip_inerstitial_tooltip = new Pharse(
            "Enable skip interstitial by step."
            , "Kích hoạt bỏ qua interstitial bằng số bước. Nghĩa là cứ gọi hiển thị interstitial một số lần thì ads mới hiển thị một lần.");

        public readonly static Pharse step_inerstitial_tooltip = new Pharse(
            "Step is number of calling show for interstitial that will be ignore before show ad. Example you set step = 1. App will ignore 1 impression each 2 calling show."
            , "Số lần gọi show để hiển thị được 1 interstitial.");

        public readonly static Pharse resend_delay_label = new Pharse(
            "Resend Delay");

        public readonly static Pharse resend_delay_tooltip = new Pharse(
            "Delay time to wait for resending another request if it got failed request."
            , "Thời gian chờ trước khi gửi lại request nếu nhận được request lỗi.");

        public readonly static Pharse request_timeout_label = new Pharse(
            "Request Timeout");

        public readonly static Pharse request_timeout_tooltip = new Pharse(
            "Time to determine the request get failed."
            , "Thời gian gian xác định request thất bại.");

        public readonly static Pharse ad_expiration_label = new Pharse(
            "Ad Expiration");

        public readonly static Pharse ad_expiration_tooltip = new Pharse(
            "Expiration of instance of ad unit. Over time, ap need to be reload. Recommend 3600 seconds (From Facebook Policy)."
            , "Hạn sử dụng của ad. Hết thời gian này mà ad đã load, chưa được hiển thị, thì app sẽ tự động hủy ad và load ad mới.");

        public readonly static Pharse required_connection_label = new Pharse(
            "Required Internet");

        public readonly static Pharse required_connection_tooltip = new Pharse(
            "Just show interstitial if device has internet connection."
            , "Chỉ hiển thị interstitial khi có kết nối interet.");

        public readonly static Pharse sdk_is_not_available = new Pharse(
            "SDK has not imported"
            , "Chưa import sdk");

        public readonly static Pharse sdk_is_available = new Pharse(
            "SDK is ready"
            , "SDK đã sẵn sàng");
    }

    public class Pharse
    {
        string[] texts;

        public Pharse(params string[] texts)
        {
            this.texts = texts;
        }

        public string Text
        {
            get
            {
                var index = (int)AdString.language;
                if (index < texts.Length) return texts[(int)AdString.language];
                else if (texts.Length > 0) return texts[0];
                else return "Unknow";
            }
        }
    }

    public enum Language
    {
        English = 0,
        Vietnamese = 1
    }
}