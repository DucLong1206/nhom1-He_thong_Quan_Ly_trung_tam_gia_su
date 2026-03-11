using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic
{
    public class TaiKhoanLogic : ITaiKhoanLogic
    {
        private readonly Appdbcontext _context;
        public TaiKhoanLogic(Appdbcontext context)
        {
            _context = context;
        }
        public bool save(TaiKhoan tk, string mess)
        {
            mess = "";

            var tkold = _context.TaiKhoan
                                .FirstOrDefault(x => x.Name == tk.Name);

            if (tkold != null)
            {
                mess = "Tên tài khoản đã trùng với tài khoản khác";
                return false;
            }

            if (string.IsNullOrEmpty(tk.PassWord) || tk.PassWord.Length < 4)
            {
                mess = "Mật khẩu phải có ít nhất 4 ký tự";
                return false;
            }
            tk.Changepass = false;

            // Hash password
            tk.PassWord = BCrypt.Net.BCrypt.HashPassword(tk.PassWord);
            tk.IsAction = false;
            _context.TaiKhoan.Add(tk);
            _context.SaveChanges();

            mess = "Tạo tài khoản thành công";
            return true;
        }
    }
}
