using He_thong_Quan_Ly_trung_tam_gia_su_Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su_Entity.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<DmTinh> DmTinhs => Set<DmTinh>();
    public DbSet<DmXa> DmXas => Set<DmXa>();
    public DbSet<MonHoc> MonHocs => Set<MonHoc>();
    public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
    public DbSet<NhanVien> NhanViens => Set<NhanVien>();
    public DbSet<User> Users => Set<User>();

    public DbSet<GiaSuKhuVuc> GiaSuKhuVucs => Set<GiaSuKhuVuc>();
    public DbSet<GiaSuMonHoc> GiaSuMonHocs => Set<GiaSuMonHoc>();

    public DbSet<LopHoc> LopHocs => Set<LopHoc>();
    public DbSet<LopHocBuoiHoc> LopHocBuoiHocs => Set<LopHocBuoiHoc>();
    public DbSet<LopHocLichSu> LopHocLichSus => Set<LopHocLichSu>();

    public DbSet<HopDong> HopDongs => Set<HopDong>();
    public DbSet<HopDongLichSu> HopDongLichSus => Set<HopDongLichSu>();
    public DbSet<HoanPhi> HoanPhis => Set<HoanPhi>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MonHoc>().HasData(
            new MonHoc { ID = 1, Name = "Toán" },
            new MonHoc { ID = 2, Name = "Vật lý" },
            new MonHoc { ID = 3, Name = "Tiếng Anh" }
        );

        modelBuilder.Entity<TaiKhoan>().HasData(
            new TaiKhoan { ID = 1, PassWord = "123456", TypeUsser = true, IDuser = 1, Name = "admin", IsAction = true },
            new TaiKhoan { ID = 2, PassWord = "123456", TypeUsser = false, IDuser = 1, Name = "giasu01", IsAction = true },
            new TaiKhoan { ID = 3, PassWord = "123456", TypeUsser = false, IDuser = 2, Name = "phuhuynh01", IsAction = true }
        );

        modelBuilder.Entity<NhanVien>().HasData(
            new NhanVien { ID = 1, IDTK = 1, Name = "Nhân viên quản trị", DiaChi = "Trung tâm", SDT = "0900000001", IDXa = 1 }
        );

        modelBuilder.Entity<User>().HasData(
            new User { ID = 1, IDTK = 2, Name = "Gia sư Nguyễn A", DiaChi = "Quận 1", SDT = "0900000002", IDXa = 1, STK = "123456789", NganHang = 1 },
            new User { ID = 2, IDTK = 3, Name = "Phụ huynh Trần B", DiaChi = "Quận 3", SDT = "0900000003", IDXa = 2, STK = "987654321", NganHang = 2 }
        );

        modelBuilder.Entity<GiaSuKhuVuc>().HasData(
            new GiaSuKhuVuc { ID = 1, IDUser = 1, IDXa = 1 }
        );

        modelBuilder.Entity<GiaSuMonHoc>().HasData(
            new GiaSuMonHoc { ID = 1, IDMon = 1, GiaTheoGio = 180000, IDUser = 1 },
            new GiaSuMonHoc { ID = 2, IDMon = 3, GiaTheoGio = 200000, IDUser = 1 }
        );

        modelBuilder.Entity<LopHoc>().HasData(
            new LopHoc
            {
                ID = 1,
                idnguoitao = 2,
                idnguoinhan = 1,
                sobuoi = 12,
                sotienMotBuoi = 250000,
                ngaytao = new DateTime(2025, 1, 5),
                trangthai = 1,
                isdetele = 0,
                DIaChi = "12 Nguyễn Huệ",
                IDxa = 1,
                PhiMoiGioi = 1200000
            }
        );

        modelBuilder.Entity<LopHocBuoiHoc>().HasData(
            new LopHocBuoiHoc
            {
                ID = 1,
                IDLop = 1,
                BuoiSo = 1,
                NgayHoc = new DateTime(2025, 1, 7),
                GioBatDau = new TimeSpan(18, 0, 0),
                GioKetThuc = new TimeSpan(20, 0, 0),
                TrangThai = 1,
                Lydo = string.Empty
            }
        );

        modelBuilder.Entity<LopHocLichSu>().HasData(
            new LopHocLichSu
            {
                ID = 1,
                sobuoi = 12,
                sotienMotBuoi = 250000,
                ngayThayDoi = new DateTime(2025, 1, 6),
                trangthai = 1,
                DIaChi = "12 Nguyễn Huệ",
                IDxa = 1,
                PhiMoiGioi = 1200000,
                IDNGuoiThayDoi = 1,
                IDLop = 1
            }
        );

        modelBuilder.Entity<HopDong>().HasData(
            new HopDong
            {
                ID = 1,
                IDLopHoc = 1,
                IDGiaSu = 1,
                NgayBatDau = new DateTime(2025, 1, 8),
                PhiMoiGioi = 1200000,
                TrangThai = 1,
                SoBuoiCamKet = 10
            }
        );

        modelBuilder.Entity<HopDongLichSu>().HasData(
            new HopDongLichSu
            {
                ID = 1,
                IDHopDong = 1,
                NgayThayDoi = new DateTime(2025, 1, 9),
                PhiMoiGioi = 1200000,
                TrangThai = 1,
                SoBuoiCamKet = 10,
                IDnguoithaydoi = 1
            }
        );

        modelBuilder.Entity<HoanPhi>().HasData(
            new HoanPhi
            {
                ID = 1,
                IDHopDong = 1,
                SoTienHoan = 300000,
                NgayXuLy = new DateTime(2025, 2, 1),
                IDUser = 2,
                IDnhanvien = 1
            }
        );
    }
}
