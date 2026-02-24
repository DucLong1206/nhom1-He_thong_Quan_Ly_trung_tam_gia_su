using He_thong_Quan_Ly_trung_tam_gia_su.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Data;

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
}
