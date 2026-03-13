using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entity;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity
{
    public class Appdbcontext : DbContext
    {
        public Appdbcontext(DbContextOptions<Appdbcontext> options) : base(options)
        { }
        public DbSet<DM_tinh> DM_tinh { get; set; }
        public DbSet<NhanVien> NhanVien { get; set; }
        public DbSet<DM_XA> DM_XA { get; set; }
        public DbSet<USER> USER { get; set; }
        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<GiaSu_KhuVuc> GiaSu_KhuVuc { get; set; }
        public DbSet<GiaSu_MonHoc> GiaSu_MonHoc { get; set; }
        public DbSet<HoanPhi> HoanPhi { get; set; }
        public DbSet<HopDong> HopDong { get; set; }
        public DbSet<HopDong_LichSu> HopDong_LichSu { get; set; }
        public DbSet<LopHoc> LopHoc { get; set; }
        public DbSet<LopHoc_BuoiHoc> LopHoc_BuoiHoc { get; set; }
        public DbSet<LopHoc_LichSu> LopHoc_LichSu { get; set; }
        public DbSet<MonHoc> MonHoc { get; set; }
        public DbSet<DM_NganHang> DM_NganHang { get; set; }
        public DbSet<LopHoc_Buoihocdangki> LopHoc_Buoihocdangki { get; set; }
        public DbSet<LopHoc_LichSu_Buoihocjdangki> LopHoc_LichSu_Buoihocjdangki { get; set; }
        public DbSet<danhsanhgiasu_List> danhsanhgiasu_List { get; set; }
        public DbSet<LopHoc_List> LopHoc_List { get; set; }
        public DbSet<lophocbyid> lophocbyid { get; set; }
        public DbSet<Lophoc_doilich> Lophoc_doilich { get; set; }
        public DbSet<LopHoc_LichHoc> LopHoc_LichHoc { get; set; }
        public DbSet<LichHomNayModel> LichHomNayModel { get; set; }
        public DbSet<ThongTinBuoiHocModel> ThongTinBuoiHocModel { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<danhsanhgiasu_List>().HasNoKey();
            builder.Entity<LopHoc_List>().HasNoKey();
            builder.Entity<lophocbyid>().HasNoKey();
            builder.Entity<LopHoc_LichHoc>().HasNoKey();
            builder.Entity<LichHomNayModel>().HasNoKey();
            builder.Entity<ThongTinBuoiHocModel>().HasNoKey();
            //builder.Entity<TrangThaiDuAn>().HasKey(tt => tt.IdTrangThaiDuAn);
            //builder.Entity<DuAn>().HasKey(tt => tt.IdDuAn);
            //builder.Entity<DuAn_Extend_GetList>().HasNoKey();
            //builder.Entity<VaiTro_NhanVien>().HasKey(tt => tt.IdVaiTroNhanVien);
            //builder.Entity<TrangThaiLoi_extend>().HasNoKey();
            //builder.Entity<NhanVien_Extend>().HasNoKey();
            //builder.Entity<LoaiLoi_extend>().HasNoKey();
            //builder.Entity<PhanLoaiLoi_extend>().HasNoKey();
            //builder.Entity<NguoiDung_extend>().HasNoKey();
            //builder.Entity<Loi_list>().HasNoKey();
            //builder.Entity<LichSu_list>().HasNoKey();
            //builder.Entity<Loi_Chart_TrangThaiLoi>().HasNoKey();
            //base.OnModelCreating(builder);
        }

    }
}
