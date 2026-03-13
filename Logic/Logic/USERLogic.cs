using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using He_thong_Quan_Ly_trung_tam_gia_su_Logic.ILogic;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Logic.Logic
{
    public class USERLogic : IUSERLogic
    {
        private readonly Appdbcontext _context;
        public USERLogic(Appdbcontext context)
        {
            _context = context;
        }
        public bool save(USER UR, string mess)
        {
            mess = "";


            _context.USER.Add(UR);
            _context.SaveChanges();

            mess = "Tạo tài khoản thành công";
            return true;
        }
        public USER GETBYIDTK(int IDTK)
        {
            try
            {
                if (IDTK <= 0)
                    return null;

                var user = _context.USER
                                   .AsNoTracking()
                                   .FirstOrDefault(x => x.ID == IDTK);

                return user;
            }
            catch (Exception ex)
            {
                // Nếu có logger thì dùng logger
                Console.WriteLine("Lỗi GETBYIDTK: " + ex.Message);

                return null;
            }
        }
        public bool EDIT(USER ur, out string mess)
        {
            mess = "";

            try
            {
                var existingUser = _context.USER.FirstOrDefault(x => x.ID == ur.ID);

                if (existingUser == null)
                {
                    mess = "Không tìm thấy người dùng";
                    return false;
                }

                // Update từng field (an toàn hơn)
                existingUser.Name = ur.Name;
                existingUser.SDT = ur.SDT;
                existingUser.DiaChi = ur.DiaChi;
                existingUser.IDXa = ur.IDXa;
                existingUser.STK = ur.STK;
                existingUser.NganHang = ur.NganHang;
                existingUser.avata = ur.avata;

                _context.SaveChanges();

                mess = "Cập nhật thông tin thành công";
                return true;
            }
            catch (Exception ex)
            {
                mess = "Lỗi: " + ex.Message;
                return false;
            }
        }
        public int checkEmailExists(string email, int id)
        {
            try
            {
                var ur = _context.TaiKhoan
                    .Where(x => x.Email == email && x.ID != id)
                    .Select(x => x.ID)
                    .FirstOrDefault();

                return ur;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi checkEmailExists: " + ex.Message);
                return 0;
            }
        }
        public bool changepass(string pass, int id, int type)
        {
            var ur2 = _context.TaiKhoan.FirstOrDefault(x => x.ID == id);
            if (ur2 != null)
            {
                if (type == 0) { ur2.Changepass = true; } else { ur2.Changepass = false; }
                ur2.PassWord = pass;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
