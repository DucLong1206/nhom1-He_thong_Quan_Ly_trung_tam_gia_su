using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.Security;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.AspNetCore.Mvc;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Controllers;

public class MonHocController : Controller
{
    private readonly IMonhocLogic _mh;
    private readonly ILopHocLogic _lh;
    private readonly Appdbcontext _context;

    public MonHocController(IMonhocLogic mh, ILopHocLogic lh, Appdbcontext context)
    {
        _mh = mh;
        _lh = lh;
        _context = context;
    }

    public IActionResult Index()
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null)
            return guardResult;

        return View();
    }
    public JsonResult getlist(string keyword, int idmon, decimal gia, int sapxep)
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null)
            return Json(new { success = false, message = "Bạn không có quyền truy cập vào khu vực này." });

        var vm = _mh.GetListGiaSu(keyword, idmon, gia, sapxep);

        return Json(new { data = vm });
    }

    [HttpGet]
    public IActionResult TutorDetail(int id, int idmon)
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null) return guardResult;

        var tutorList = _mh.GetListGiaSu("", 0, 0, 0); 
        var tutor = tutorList.FirstOrDefault(x => x.ID == id && x.IDMon == idmon);

        if (tutor == null) return RedirectToAction("Index"); 

        ViewData["TutorName"] = tutor.Name;
        ViewData["Mon"] = tutor.mon;
        ViewData["Xa"] = tutor.xa;
        ViewData["GiaTheoGio"] = tutor.GiaTheoGio;
        ViewData["ID"] = tutor.ID;
        ViewData["IDXa"] = tutor.IDXa;
        ViewData["IDMon"] = tutor.IDMon;
        ViewData["Trinhdo"] = tutor.Trinhdo;
        ViewData["avata"] = tutor.avata;

        // --- LẤY SỐ LIỆU THẬT TỪ DATABASE ---
        // Đếm số môn đang dạy (Dựa vào các lớp có trạng thái 3: Đang dạy, 4: Kết thúc)
        int soMon = _context.LopHoc
            .Where(x => x.idnguoinhan == id && (x.TrangThai == 3 || x.TrangThai == 4))
            .Select(x => x.idmon).Distinct().Count();

        // Đếm số học sinh đã nhận (Dựa vào id người tạo lớp)
        int soHocSinh = _context.LopHoc
            .Where(x => x.idnguoinhan == id && (x.TrangThai == 3 || x.TrangThai == 4))
            .Select(x => x.idnguoitao).Distinct().Count();

        // Gán vào ViewBag (Nếu bằng 0 thì gán tạm số 1 cho đẹp đội hình)
        ViewBag.SoMon = soMon > 0 ? soMon : 1; 
        ViewBag.SoHocSinh = soHocSinh > 0 ? soHocSinh : 1;
        ViewBag.DanhGia = "4.8/5"; // Tạm thời giữ nguyên

        return View();
    }

    [HttpPost]
    public JsonResult Save([FromBody] SaveLop model)
    {
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2);
        if (guardResult != null)
            return Json(new { success = false, message = "Bạn không có quyền truy cập vào khu vực này." });

        string mess = "";
        model.lop.ngaytao = DateTime.Now;
        model.lop.idnguoitao = HttpContext.Session.GetInt32("UserId");
        model.lop.isdetele = false;
        model.lop.TrangThai = 1;

        var save = _lh.SaveLopHoc(model, out mess);

        if (mess.Length > 0) return Json(new { success = false, mess = mess });
        return Json(new { success = true });
    }

    [HttpPost]
    public JsonResult DangBinhLuan(int idGiaSu, string noiDung)
    {
        // 1. Chốt chặn bảo mật: CHỈ CHO PHÉP TÀI KHOẢN LOẠI 2 (PHỤ HUYNH) VÀO ĐÂY
        var guardResult = SessionAccessGuard.EnsureUserType(this, 2); 
        if (guardResult != null) 
            return Json(new { success = false, message = "Chỉ Phụ huynh/Học sinh mới được bình luận." });

        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) 
            return Json(new { success = false, message = "Vui lòng đăng nhập." });

        if (string.IsNullOrWhiteSpace(noiDung)) 
            return Json(new { success = false, message = "Nội dung không được để trống." });

        try
        {
            var bl = new BinhLuan
            {
                IDGiaSu = idGiaSu,
                IDPhuHuynh = userId.Value,
                NoiDung = noiDung,
                NgayTao = DateTime.Now
            };

            _context.BinhLuan.Add(bl);
            _context.SaveChanges();

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Lỗi khi lưu bình luận: " + ex.Message });
        }
    }

    [HttpGet]
    public JsonResult GetBinhLuan(int idGiaSu)
    {
        // Lấy danh sách bình luận kèm tên người đăng
        var data = (from b in _context.BinhLuan
                    join u in _context.USER on b.IDPhuHuynh equals u.ID
                    where b.IDGiaSu == idGiaSu
                    orderby b.NgayTao descending
                    select new {
                        ten = u.Name,
                        noiDung = b.NoiDung,
                        ngayTao = b.NgayTao.ToString("dd/MM/yyyy HH:mm")
                    }).ToList();

        return Json(data);
    }
}
