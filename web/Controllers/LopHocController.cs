using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
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
        public IActionResult Detail(int id)
        {
            var ds = _lh.GETDANHSACHLICHHOC_byidlophoc(id);
            ViewBag.data = ds;
            return View();
        }
        public IActionResult LichHocDetail(int id)
        {
            ViewBag.LopHocId = id;
            return View();
        }

        public IActionResult DoiNgayHoc(int lopId, string nguon = "phu-huynh")
        {
            ViewBag.LopHocId = lopId;
            ViewBag.Nguon = nguon;
            return View();
        }

        public IActionResult Contract(int lopId)
        {
            var ds = _lh.createhopdong(lopId);
            var DATA = _lh.GETDANHSACHLICHHOC_byidlophoc(lopId);
            ViewBag.data = DATA;
            ViewBag.ds = ds;
            return View();
        }

        [HttpPost]
        public JsonResult XuLyPhanHoi([FromBody] ListLopchange request)
        {
            string mess = "";
            if (request == null || request.lopid <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            //if (request.loai == "tuchoi" && string.IsNullOrWhiteSpace(request.lydo))
            //{
            //    return Json(new { success = false, message = "Vui lòng nhập lý do từ chối." });
            //}     
            var change = _lh.changestatus(request, out mess);
            return Json(new
            {
                success = change,
                message = mess ?? "Đã ghi nhận phản hồi.",
                lopid = request.lopid,
                loai = request.loai
            });
        }
        [HttpGet]
        public JsonResult GetLopHocDetailById(int id)
        {
            try
            {
                var data = _lh.GETDANHSACHLICHHOC_byidlophoc(id);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
        [HttpGet]
        public JsonResult GETDANHSACHLICHHOC()
        {
            int? id = HttpContext.Session.GetInt32("UserId");

            if (id == null)
            {
                return Json(new { success = false, message = "User chưa đăng nhập" });
            }

            var ds = _lh.GETDANHSACHLICHHOC(id.Value);

            return Json(ds);
        }
        public JsonResult changhopdong(int id, int trangthai)
        {
            try
            {
                var change = _lh.changeHopDong(id, trangthai);
                if (change)
                {
                    return Json(new { success = true, message = "Đã cập nhật trạng thái hợp đồng." });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy hợp đồng hoặc không thể cập nhật." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        public JsonResult getlopbyid(int id)
        {
            try
            {
                var data = _lh.getlopbyid(id);
                if (data != null)
                {
                    return Json(new { success = true, data = data });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy lớp học." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult LuuDoiLichHoc([FromBody] Lophoc_doilich lich)
        {
            var save = false;
            if (lich == null)
            {
                return Json(new { success = false, message = "Không nhận được dữ liệu." });
            }
            if (lich.ID != 0)
            {
                save = _lh.editlichbu(lich);
            }
            else
            {
                save = _lh.savelichbu(lich);
            }

            return Json(new
            {
                success = save,
                data = lich
            });
        }
        public JsonResult getngaygoc(int idlop)
        {
            try
            {
                var data = _lh.GetNgayGocHocBu(idlop);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult thongbaolich()
        {
            return View();
        }
        public JsonResult getlichhomnay()
        {
            var iduser = HttpContext.Session.GetInt32("UserId");

            if (iduser == null)
            {
                return Json(new List<LichHomNayModel>());
            }

            var data = _lh.LichHomNay(iduser.Value);

            return Json(data);
        }
    }
}
