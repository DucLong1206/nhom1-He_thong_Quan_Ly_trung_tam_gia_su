using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers
{
    public class LopHocController : Controller
    {
        private readonly ILopHocLogic _lh;
        public LopHocController(ILopHocLogic lh)
        {
            _lh = lh;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LopHoc()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Getlistlophocdangkiping(int id)
        {
            try
            {
                var data = _lh.GetLopHocPing(id);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public JsonResult LopHoc_Buoihocdangki_GetList_ByIDLopHoc(int id)
        {
            try
            {
                var data = _lh.LopHoc_Buoihocdangki_GetList_ByIDLopHoc(id);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult SendLessonAlertEmail(string email, string alertType, string lessonName, string scheduledStart, string scheduledEnd)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Json(new { success = false, message = "Thiếu email nhận cảnh báo." });

                var subject = "[Thông báo lớp học]";
                var body = "";

                switch ((alertType ?? string.Empty).Trim().ToLower())
                {
                    case "start-reminder":
                        subject = "[Nhắc giờ học] Đến giờ bắt đầu buổi học";
                        body = $"<p>Buổi học <b>{lessonName}</b> đã đến giờ bắt đầu.</p><p>Giờ bắt đầu dự kiến: <b>{scheduledStart}</b>.</p>";
                        break;
                    case "overdue-start":
                        subject = "[Cảnh báo] Quá giờ nhưng buổi học chưa bắt đầu";
                        body = $"<p>Buổi học <b>{lessonName}</b> đã quá giờ bắt đầu nhưng vẫn chưa được xác nhận bắt đầu.</p><p>Giờ bắt đầu dự kiến: <b>{scheduledStart}</b>.</p>";
                        break;
                    case "overdue-end":
                        subject = "[Cảnh báo] Quá giờ nhưng buổi học chưa kết thúc";
                        body = $"<p>Buổi học <b>{lessonName}</b> đã quá giờ kết thúc dự kiến nhưng vẫn chưa được kết thúc.</p><p>Giờ kết thúc dự kiến: <b>{scheduledEnd}</b>.</p>";
                        break;
                    default:
                        return Json(new { success = false, message = "Loại thông báo không hợp lệ." });
                }

                var emailService = new EmailService();
                var sent = emailService.SendMail(email, subject, body);

                if (!sent)
                    return Json(new { success = false, message = "Gửi email thất bại." });

                return Json(new { success = true, message = "Đã gửi email thông báo." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
