# BÁO CÁO BÀI TẬP LỚN

**Tên đề tài:** Hệ thống Quản lý Trung tâm Gia sư  
**Công nghệ:** ASP.NET Core 8.0 MVC, Entity Framework Core, SQL Server  
**Nhóm:** Nhóm 1  

---

## Mục lục

1. [Giới thiệu](#1-giới-thiệu)
2. [Tổng quan hệ thống](#2-tổng-quan-hệ-thống)
3. [Công nghệ sử dụng](#3-công-nghệ-sử-dụng)
4. [Phân tích và thiết kế hệ thống](#4-phân-tích-và-thiết-kế-hệ-thống)
5. [Thiết kế và cài đặt](#5-thiết-kế-và-cài-đặt)
6. [Kết quả đạt được](#6-kết-quả-đạt-được)
7. [Hạn chế](#7-hạn-chế)
8. [Hướng phát triển](#8-hướng-phát-triển)
9. [Hướng dẫn cài đặt và chạy](#9-hướng-dẫn-cài-đặt-và-chạy)
10. [Phụ lục nhanh](#10-phụ-lục-nhanh)

---

## 1. Giới thiệu

### 1.1 Giới thiệu đề tài

Đề tài "Hệ thống Quản lý Trung tâm Gia sư" hướng đến việc xây dựng một ứng dụng web quản lý toàn diện các hoạt động vận hành của một trung tâm gia sư, bao gồm quản lý tài khoản người dùng, quản lý gia sư, quản lý lớp học, quản lý môn học và các danh mục hỗ trợ.

### 1.2 Lý do chọn đề tài

Trong bối cảnh giáo dục ngoài giờ và gia sư tư nhân ngày càng phổ biến, nhu cầu quản lý thông tin một cách có hệ thống là thiết yếu. Việc theo dõi thông tin gia sư, phụ huynh, lớp học và học phí thủ công dễ dẫn đến sai sót và kém hiệu quả. Một hệ thống phần mềm chuyên biệt giúp tự động hóa quy trình, nâng cao độ chính xác và hỗ trợ ra quyết định quản lý nhanh chóng hơn.

### 1.3 Mục tiêu của hệ thống

- Xây dựng hệ thống quản lý người dùng đa vai trò (Admin, Nhân viên, Gia sư, Phụ huynh).
- Quản lý thông tin hồ sơ gia sư kết hợp với môn học giảng dạy.
- Quản lý lớp học, lịch dạy và cảnh báo tự động.
- Cung cấp giao diện web thân thiện, bảo mật thông tin người dùng.
- Hỗ trợ chức năng đặt lại mật khẩu qua email.

---

## 2. Tổng quan hệ thống

### 2.1 Mô tả hệ thống

Hệ thống là một ứng dụng web được xây dựng trên nền tảng ASP.NET Core 8.0, triển khai kiến trúc MVC kết hợp phân tầng nghiệp vụ (N-tier). Hệ thống phân chia người dùng thành bốn nhóm chức năng: Quản trị viên (Admin), Nhân viên, Gia sư và Phụ huynh. Mỗi nhóm được cấp quyền truy cập và thực hiện các thao tác khác nhau dựa trên cơ chế phiên làm việc (Session).

### 2.2 Các chức năng chính

| STT | Chức năng | Mô tả |
|-----|-----------|-------|
| 1 | Quản lý tài khoản | Tạo, chỉnh sửa, xóa tài khoản người dùng theo từng loại vai trò |
| 2 | Quản lý hồ sơ người dùng | Lưu trữ thông tin cá nhân, ảnh đại diện, địa chỉ (tỉnh/xã), ngân hàng |
| 3 | Quản lý gia sư & môn học | Gán môn học cho gia sư, quản lý danh sách môn giảng dạy |
| 4 | Quản lý lớp học | Tạo và theo dõi các lớp học |
| 5 | Cảnh báo lịch học | Dịch vụ nền tự động nhắc nhở khi lớp học sắp diễn ra |
| 6 | Đặt lại mật khẩu | Sinh mật khẩu ngẫu nhiên, mã hóa và gửi qua email |
| 7 | Khởi tạo dữ liệu mẫu | Có lớp `DemoDataSeeder`, hiện tại chưa được gọi trong `Program.cs` |

### 2.3 Ma trận vai trò và quyền truy cập

| Vai trò (`TypeUsser`) | Khu vực chính | Quyền tiêu biểu |
|-----------------------|---------------|-----------------|
| 0 - Admin | `Home` | Đăng nhập hệ thống, quản trị tổng quan |
| 1 - Nhân viên | `LopHoc`, `USER/TutorSubjects`, `Home/TutorDashboard` | Duyệt lớp, quản lý hồ sơ người dùng, xử lý lịch học |
| 2 - Phụ huynh | `MonHoc`, `LopHoc` (một số action), `Home/TutorDashboard` | Tìm gia sư, đăng ký lớp, theo dõi/điều chỉnh lịch |
| 3 - Gia sư | Hạn chế hơn, theo điều hướng `SessionAccessGuard` | Truy cập theo phiên đăng nhập và luồng được phân quyền |

> Quyền truy cập được kiểm tra bằng `SessionAccessGuard.EnsureUserType(...)` hoặc `EnsureUserTypes(...)`.

---

## 3. Công nghệ sử dụng

### 3.1 Ngôn ngữ lập trình

- **C# 12** — Ngôn ngữ lập trình chính cho toàn bộ tầng backend.
- **HTML5 / CSS3 / JavaScript** — Ngôn ngữ frontend, kết hợp với Razor template engine.

### 3.2 Framework & Thư viện

| Thành phần | Phiên bản | Mục đích sử dụng |
|------------|-----------|-----------------|
| ASP.NET Core MVC | 8.0 | Web framework chính |
| Entity Framework Core | 8.0.22 | ORM mapping và truy vấn cơ sở dữ liệu |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.22 | Provider kết nối SQL Server |
| BCrypt.Net-Next | 4.1.0 | Mã hóa mật khẩu một chiều (hashing) |
| Bootstrap | 5.x | Giao diện responsive |
| jQuery | 3.x | Xử lý AJAX và DOM |
| jquery-validation | — | Validation phía client |
| IHostedService | (built-in) | Dịch vụ nền (cảnh báo lịch học) |

### 3.3 Cơ sở dữ liệu

- **Microsoft SQL Server** — Hệ quản trị cơ sở dữ liệu quan hệ chính.
- Kết nối thông qua chuỗi `DefaultConnection` cấu hình trong `appsettings.json`.
- Truy cập dữ liệu qua EF Core (`Appdbcontext`); schema CSDL hiện cần được tạo bằng migration/script SQL.

### 3.4 Công cụ hỗ trợ

- **Visual Studio 2022** — IDE phát triển chính (phiên bản 17.14).
- **Git** — Quản lý phiên bản mã nguồn.
- **IIS Express** — Web server phát triển nội bộ.
- **Postman / trình duyệt** — Kiểm thử API và giao diện.

### 3.5 Bảng phụ thuộc theo project

| Project | Vai trò | Package nổi bật |
|---------|---------|-----------------|
| `web` | Tầng trình bày (MVC) + DI + middleware | `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools` |
| `Logic` | Tầng nghiệp vụ | `BCrypt.Net-Next`, `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer` |
| `He_thong_Quan_Ly_trung_tam_gia_su_Entity` | Tầng dữ liệu (Entity + DbContext) | `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools` |

---

## 4. Phân tích và thiết kế hệ thống

### 4.1 Kiến trúc hệ thống

#### Mô hình kiến trúc

Hệ thống áp dụng kiến trúc **N-Tier (3 tầng)** kết hợp với mô hình **MVC (Model–View–Controller)**:

```
┌──────────────────────────────────────────────────────┐
│                   PRESENTATION LAYER                 │
│         (ASP.NET Core MVC – Controllers + Views)     │
├──────────────────────────────────────────────────────┤
│                    BUSINESS LOGIC LAYER              │
│    (He_thong_Quan_Ly_trung_tam_gia_su_Logic)         │
│         ILogic interfaces ↔ Logic implementations   │
├──────────────────────────────────────────────────────┤
│                   DATA ACCESS LAYER                  │
│    (He_thong_Quan_Ly_trung_tam_gia_su_Entity)        │
│         EF Core DbContext (Appdbcontext) + Entities  │
├──────────────────────────────────────────────────────┤
│                      DATABASE                        │
│                  Microsoft SQL Server                │
└──────────────────────────────────────────────────────┘
```

- **Tầng Presentation**: Các Controller xử lý yêu cầu HTTP, trả về View (Razor) hoặc JsonResult cho các lời gọi AJAX.
- **Tầng Business Logic**: Chứa các class Logic triển khai interface ILogic tương ứng. Tầng này thực hiện toàn bộ nghiệp vụ trước khi tương tác với cơ sở dữ liệu.
- **Tầng Data Access**: Entity framework Core với `Appdbcontext` định nghĩa mapping giữa các class C# và bảng SQL Server.

#### Sơ đồ luồng xử lý yêu cầu

```
[Trình duyệt / Client]
        │
        ▼ HTTP Request
[Routing Middleware]  →  {controller}/{action}/{id?}
        │
        ▼
[Controller (e.g. USERController)]
        │
        ├── Kiểm tra quyền (SessionAccessGuard)
        │
        ▼
[ILogic Interface] → [Logic Implementation]
        │
        ▼
[Appdbcontext (EF Core)] → [SQL Server]
        │
        ▼
[Kết quả trả về: View / JsonResult]
        │
        ▼
[Trình duyệt hiển thị]
```

#### Cơ chế xác thực và phân quyền

Hệ thống không sử dụng ASP.NET Identity mà tự triển khai xác thực dựa trên **Session**. Lớp `SessionAccessGuard` chịu trách nhiệm kiểm tra loại người dùng (`TypeUser`) lưu trong phiên làm việc trước khi cho phép truy cập vào các action nhất định.

Bốn loại người dùng (`TypeUsser`):

| Giá trị | Vai trò |
|---------|---------|
| 0 | Admin (Quản trị viên) |
| 1 | Nhân viên |
| 2 | Gia sư |
| 3 | Phụ huynh |

---

### 4.2 Thiết kế cơ sở dữ liệu

> *Lưu ý: Source code tầng Entity/Logic có đầy đủ trong workspace hiện tại. Danh sách dưới đây tổng hợp trực tiếp từ `Appdbcontext` và các class entity.*

#### Danh sách DbSet/Entity trong `Appdbcontext`

| Nhóm | Entity/Model | Kiểu |
|------|--------------|------|
| Danh mục | `DM_tinh`, `DM_XA`, `DM_NganHang`, `MonHoc` | Bảng danh mục |
| Tài khoản - hồ sơ | `TaiKhoan`, `USER`, `NhanVien` | Bảng nghiệp vụ |
| Gia sư | `GiaSu_MonHoc`, `GiaSu_KhuVuc` | Bảng liên kết |
| Lớp học | `LopHoc`, `LopHoc_Buoihocdangki`, `LopHoc_BuoiHoc`, `LopHoc_LichSu`, `LopHoc_LichSu_Buoihocjdangki`, `Lophoc_doilich` | Bảng nghiệp vụ/lịch học |
| Hợp đồng - phí | `HopDong`, `HopDong_LichSu`, `HoanPhi` | Bảng nghiệp vụ |
| Tương tác | `BinhLuan`, `ThongBao` | Bảng nghiệp vụ |
| Model truy vấn không khóa | `danhsanhgiasu_List`, `LopHoc_List`, `lophocbyid`, `LopHoc_LichHoc`, `LichHomNayModel`, `ThongTinBuoiHocModel` | `HasNoKey()` |

#### Các bảng chính

**Bảng `TaiKhoan` (Tài khoản)**

| Cột | Kiểu | Mô tả |
|-----|------|-------|
| Id | int (PK) | Khóa chính tự tăng |
| Name | nvarchar | Tên hiển thị |
| Email | nvarchar | Địa chỉ email (dùng để đăng nhập) |
| PassWord | nvarchar | Mật khẩu đã được mã hóa BCrypt |
| TypeUsser | int | Loại tài khoản (0=Admin, 1=NV, 2=GiaSu, 3=PhuHuynh) |
| IsAction | bit | Trạng thái kích hoạt tài khoản |
| Changepass | bit | Yêu cầu đổi mật khẩu lần đầu đăng nhập |

**Bảng `USER` (Hồ sơ người dùng)**

| Cột | Kiểu | Mô tả |
|-----|------|-------|
| Id | int (PK) | Khóa chính |
| IDTK | int (FK) | Liên kết với `TaiKhoan` |
| Name | nvarchar | Họ tên người dùng |
| DiaChi | nvarchar | Địa chỉ |
| SDT | nvarchar | Số điện thoại |
| STK | nvarchar | Số tài khoản ngân hàng |
| avata | nvarchar | Đường dẫn ảnh đại diện |
| IDXa | int (FK) | Liên kết với `DM_Xa` |
| NganHang | int (FK) | Liên kết với `DM_NganHang` |

**Bảng `GiaSu_MonHoc` (Gia sư – Môn học)**

| Cột | Kiểu | Mô tả |
|-----|------|-------|
| Id | int (PK) | Khóa chính |
| IDUser | int (FK) | Liên kết người dùng gia sư (`USER`) |
| IDMon | int (FK) | Liên kết với `MonHoc` |
| GiaTheoGio | decimal | Học phí theo giờ |
| Trinhdo | int | Trình độ/lớp phụ trách |
| Isdelete | bit | Đánh dấu xóa mềm |

**Bảng `MonHoc` (Môn học)**

| Cột | Kiểu | Mô tả |
|-----|------|-------|
| Id | int (PK) | Khóa chính |
| Name | nvarchar | Tên môn học |

**Bảng `LopHoc` (Lớp học)**

| Cột | Kiểu | Mô tả |
|-----|------|-------|
| Id | int (PK) | Khóa chính |
| idnguoitao | int (FK, nullable) | Người tạo lớp (thường là phụ huynh) |
| idnguoinhan | int (FK) | Gia sư nhận lớp |
| idmon | int (FK, nullable) | Môn học |
| trinhdo | int (nullable) | Trình độ/lớp |
| sotienMotBuoi | decimal (nullable) | Học phí mỗi buổi |
| PhiMoiGioi | decimal (nullable) | Phí môi giới |
| NgayBatdau | datetime | Ngày bắt đầu |
| NgayKetthuc | datetime (nullable) | Ngày kết thúc |
| DIaChi | nvarchar | Địa chỉ học |
| IDxa | int (FK, nullable) | Xã/phường |
| TrangThai | int (nullable) | Trạng thái lớp |
| isdetele | bit (nullable) | Đánh dấu xóa mềm |
| ngaytao | datetime (nullable) | Thời điểm tạo lớp |

**Bảng `BinhLuan` (Bình luận gia sư)**

| Cột | Kiểu | Mô tả |
|-----|------|-------|
| ID | int (PK) | Khóa chính |
| IDGiaSu | int | ID gia sư được bình luận |
| IDPhuHuynh | int | ID phụ huynh/người viết bình luận |
| NoiDung | nvarchar | Nội dung bình luận |
| NgayTao | datetime | Thời điểm tạo bình luận |

**Bảng `ThongBao` (Thông báo hệ thống)**

| Cột | Kiểu | Mô tả |
|-----|------|-------|
| Id | int (PK) | Khóa chính |
| UserId | int | ID người nhận thông báo |
| NoiDung | nvarchar(500) | Nội dung thông báo |
| Link | nvarchar(255) | Đường dẫn điều hướng khi click |
| DaDoc | bit | Trạng thái đã đọc/chưa đọc |
| NgayTao | datetime | Thời điểm tạo thông báo |

**Các bảng danh mục (DM_)**

| Bảng | Mô tả |
|------|-------|
| `DM_Tinh` | Danh mục tỉnh/thành phố |
| `DM_Xa` | Danh mục xã/phường (quan hệ với DM_Tinh) |
| `DM_NganHang` | Danh mục ngân hàng |

#### Quan hệ giữa các bảng

```
TaiKhoan ──── USER (1:1, qua IDTK)
USER     ──── GiaSu_MonHoc (1:N, gia sư có nhiều môn)
MonHoc   ──── GiaSu_MonHoc (1:N, môn có nhiều gia sư dạy)
USER     ──── LopHoc (1:N, qua idnguoitao / idnguoinhan)
DM_Tinh  ──── DM_Xa (1:N, tỉnh gồm nhiều xã)
DM_Xa    ──── USER (1:N)
DM_NganHang── USER (1:N)
USER     ──── BinhLuan (1:N, qua IDPhuHuynh)
USER     ──── BinhLuan (1:N, qua IDGiaSu)
USER     ──── ThongBao (1:N, người dùng nhận nhiều thông báo)
```

---

### 4.3 Thiết kế chức năng

#### Chức năng 1: Quản lý người dùng (USERController)

**Luồng hoạt động:**

1. Người dùng truy cập trang `Index`/`AddorEdit` → Controller trả về View và khởi tạo `ViewBag.idtk` khi cần.
2. Kích hoạt form thêm/sửa (`AddorEdit`) → AJAX load dữ liệu `GETUSER(id)` → điền form.
3. Submit form → `[HttpPost] Save(USER model, IFormFile avatarFile)`:
   - Kiểm tra độ dài số điện thoại.
   - Nếu có file ảnh: lưu vào `wwwroot/images/`.
   - Nếu `model.Id == 0`: gọi `_user.save()`.
   - Nếu `model.Id > 0`: gọi `_user.EDIT()`.
4. Trả về `JsonResult` thể hiện kết quả thành công/thất bại.

#### Chức năng 2: Đặt lại mật khẩu

**Luồng hoạt động:**

1. Nhân viên/Admin kích hoạt chức năng reset mật khẩu cho tài khoản → gọi `checkpass(email)`.
2. Hệ thống sinh mật khẩu ngẫu nhiên 8 ký tự (`GenerateRandomPassword()`).
3. Mã hóa bằng BCrypt: `BCrypt.Net.BCrypt.HashPassword(newPass)`.
4. Gọi `_user.changepass()` để cập nhật DB.
5. Gọi `EmailService` để gửi mật khẩu mới về địa chỉ email của người dùng.

#### Chức năng 3: Cảnh báo lịch học (Background Service)

**Luồng hoạt động:**

1. `LessonAlertBackgroundService` (triển khai `IHostedService`) khởi động cùng ứng dụng.
2. Định kỳ truy vấn các lớp học sắp bắt đầu trong khoảng thời gian cấu hình.
3. Gửi cảnh báo (email hoặc thông báo nội bộ) tới gia sư và phụ huynh liên quan.

#### Chức năng 4: Quản lý môn học của gia sư

**Luồng hoạt động:**

1. Truy cập `TutorSubjects(idtk)` — được bảo vệ bởi `SessionAccessGuard.EnsureUserType(1)` (chỉ nhân viên trở lên).
2. Tải danh sách môn học qua `getlisstmonhoc()` và môn học hiện tại của gia sư qua `getlistmonhocbyidgiasu(id)`.
3. Thêm môn: `SaveMonHocGiaSu(GiaSu_MonHoc model)`.
4. Xóa môn: `DeleteMonHocGiaSu(id)`.

---

## 5. Thiết kế và cài đặt

### 5.1 Cấu trúc thư mục

```
nhom1-He_thong_Quan_Ly_trung_tam_gia_su/
│
├── He_thong_Quan_Ly_trung_tam_gia_su.csproj   # Dự án gốc (boilerplate)
├── He_thong_Quan_Ly_trung_tam_gia_su.sln      # Solution Visual Studio
├── Program.cs                                  # Entry point dự án gốc
├── appsettings.json                            # Cấu hình logging, AllowedHosts
├── appsettings.Development.json                # Cấu hình môi trường dev
│
├── Controllers/                                # Controller dự án gốc
│   └── HomeController.cs                      # Trang chủ, Privacy, Error
│
├── Models/
│   └── ErrorViewModel.cs                      # Model hiển thị lỗi
│
├── Views/                                      # Razor Views
│   ├── _ViewImports.cshtml                    # Import namespace + TagHelper
│   ├── _ViewStart.cshtml                      # Khai báo layout mặc định
│   ├── Home/
│   │   ├── Index.cshtml                       # Trang chủ
│   │   └── Privacy.cshtml                     # Trang chính sách
│   └── Shared/
│       ├── _Layout.cshtml                     # Layout Bootstrap 5 dùng chung
│       ├── Error.cshtml                       # Trang hiển thị lỗi
│       └── _ValidationScriptsPartial.cshtml   # Script validation client
│
├── Properties/
│   └── launchSettings.json                    # Cấu hình port, môi trường
│
├── wwwroot/                                    # Tài nguyên tĩnh
│   ├── css/site.css                           # CSS tùy chỉnh
│   ├── js/site.js                             # JavaScript tùy chỉnh
│   └── lib/                                   # Bootstrap, jQuery, validation
│
├── web/                                        # Dự án ứng dụng thực tế
│   ├── Program.cs                             # Entry point đầy đủ (EF, DI, Session)
│   ├── Controllers/
│   │   ├── HomeController.cs
│   │   ├── USERController.cs
│   │   ├── MonHocController.cs
│   │   └── LopHocController.cs
│   └── Data/
│       └── DemoDataSeeder.cs                  # Khởi tạo dữ liệu mẫu
│
├── He_thong_Quan_Ly_trung_tam_gia_su_Entity/   # Tầng Entity (EF Core)
│   └── [Appdbcontext + Entities]              # Source đầy đủ
│
└── Logic/                                      # Tầng Business Logic
        └── [ILogic interfaces + implementations]  # Source đầy đủ
```

---

### 5.2 Mô tả các module chính

#### Module 1: Khởi động và cấu hình ứng dụng (`web/Program.cs`)

Đây là điểm khởi động của ứng dụng thực tế. Module này thực hiện:

- **Đăng ký DbContext**: Cấu hình `Appdbcontext` với SQL Server thông qua chuỗi kết nối `DefaultConnection`.
- **Đăng ký Dependency Injection**: Toàn bộ các Logic service được đăng ký với vòng đời `Scoped`:
  - `IMonhocLogic`, `ITaiKhoanLogic`, `IDM_TinhLogic`, `IDM_XaLogic`
  - `IUSERLogic`, `ILopHocLogic`, `IDM_NganHangLogic`
- **Đăng ký Background Service**: `LessonAlertBackgroundService` hoạt động song song với ứng dụng.
- **Cấu hình Session**: Cookie HTTP-only, thời gian timeout 8 giờ.
- **Seed dữ liệu**: Hiện tại `Program.cs` chưa gọi `DemoDataSeeder.Seed(db)`.
- **Cấu hình middleware pipeline**: HTTPS Redirection → Static Files → Routing → Session → Authorization.

### 5.2.1 Bảng Controller và chức năng

| Controller | Màn hình/chức năng chính | Nhóm action tiêu biểu |
|------------|--------------------------|-----------------------|
| `HomeController` | Đăng nhập, dashboard, trang tĩnh, kiểm tra DB | `Login`, `TutorDashboard`, `DbHealth`, `Logout` |
| `USERController` | Hồ sơ người dùng, môn dạy gia sư, đổi/reset mật khẩu | `Save`, `GETUSER`, `TutorSubjects`, `checkpass`, `ChangePass` |
| `MonHocController` | Tìm gia sư, xem chi tiết gia sư, đăng ký lớp, bình luận | `getlist`, `TutorDetail`, `Save`, `DangBinhLuan`, `GetBinhLuan` |
| `LopHocController` | Quản lý lớp, lịch học, hợp đồng, hoàn phí, gửi cảnh báo | `GETDANHSACHLICHHOC`, `StartLesson`, `StopLesson`, `KiemTraDieuKienHoanPhi`, `XuLyPhanHoi` |

### 5.2.2 Bảng DI service mapping

| Interface | Implementation | Vòng đời |
|-----------|----------------|----------|
| `IMonhocLogic` | `MonhocLogic` | Scoped |
| `ITaiKhoanLogic` | `TaiKhoanLogic` | Scoped |
| `IDM_TinhLogic` | `DM_TinhLogic` | Scoped |
| `IDM_XaLogic` | `DM_XaLogic` | Scoped |
| `IUSERLogic` | `USERLogic` | Scoped |
| `ILopHocLogic` | `LopHocLogic` | Scoped |
| `IDM_NganHangLogic` | `DM_NganHangLogic` | Scoped |

#### Module 2: Quản lý người dùng (`USERController`)

Controller trung tâm quản lý toàn bộ nghiệp vụ liên quan đến người dùng:

- **Action `Index`**: Hiển thị trang danh sách người dùng.
- **Action `AddorEdit`**: Hiển thị form thêm/sửa hồ sơ, nhận `idtk` qua query string.
- **Action `TutorSubjects`**: Quản lý môn học của gia sư, được bảo vệ bởi `SessionAccessGuard`.
- **JsonResult actions**: Cung cấp dữ liệu cho frontend qua AJAX (tỉnh, xã, ngân hàng, môn học, thông tin user).
- **Action `Save`**: Lưu hồ sơ người dùng, xử lý upload ảnh đại diện.
- **Action `checkpass` / `ChangePass`**: Quản lý đặt lại và thay đổi mật khẩu với BCrypt.

#### Module 3: Khởi tạo dữ liệu (`DemoDataSeeder`)

Lớp static chạy một lần khi ứng dụng khởi động. Kiểm tra nếu bảng `TaiKhoan` rỗng, sẽ tạo 4 tài khoản mẫu đại diện cho 4 loại người dùng:

| Tên | Email | Vai trò | Đổi mật khẩu lần đầu |
|-----|-------|---------|----------------------|
| admin | admin@demo.local | Admin (0) | Không |
| nhanvien | nv@demo.local | Nhân viên (1) | Không |
| giasu | giasu@demo.local | Gia sư (2) | Không |
| phuhuynh | ph@demo.local | Phụ huynh (3) | Có |

Mật khẩu đều được mã hóa bằng `BCrypt.Net.BCrypt.HashPassword()`.

#### Module 4: Bảo mật (`SessionAccessGuard` & `BCrypt`)

- **SessionAccessGuard**: Tiện ích kiểm tra loại người dùng lưu trong Session trước khi cho phép thực thi action, thay thế cho attribute-based authorization.
- **BCrypt**: Mật khẩu không bao giờ lưu dạng plaintext. Mỗi lần tạo/thay đổi mật khẩu đều gọi `BCrypt.HashPassword()`. Xác minh dùng `BCrypt.Verify()`.
- **EmailService**: Gửi mật khẩu mới đến người dùng khi reset mật khẩu.

---

### 5.3 Giao diện người dùng

#### Cách hoạt động của Razor View

Razor là template engine tích hợp trong ASP.NET Core, cho phép kết hợp HTML và C# trong cùng một file `.cshtml`.

- **`_ViewStart.cshtml`**: Mọi View đều tự động áp dụng layout `_Layout` mà không cần khai báo lại.
- **`_ViewImports.cshtml`**: Import namespace ứng dụng và đăng ký Tag Helpers của ASP.NET Core, cho phép sử dụng cú pháp `asp-controller`, `asp-action`, `asp-for` trong Razor.
- **`_Layout.cshtml`**: Định nghĩa khung trang dùng chung gồm: thanh điều hướng navbar (Bootstrap 5), vùng nội dung `@RenderBody()`, footer và các script dùng chung (jQuery, Bootstrap JS).

#### Cách hiển thị dữ liệu

- **View với ViewModel**: Controller truyền model vào View qua `return View(model)` hoặc `ViewBag`; Razor bind trực tiếp bằng `@Model.Property`.
- **AJAX + JsonResult**: Nhiều màn hình sử dụng jQuery AJAX để gọi các JsonResult action trong Controller, nhận về JSON và cập nhật DOM động mà không cần reload trang. Ví dụ: load danh sách xã khi chọn tỉnh (`getlistxa(id)`), load thông tin user khi mở form sửa (`GETUSER(id)`).
- **Validation client-side**: `_ValidationScriptsPartial.cshtml` nhúng `jquery.validate` và `jquery.validate.unobtrusive`, phối hợp với Data Annotation trên Model để validate ngay tại trình duyệt.

---

## 6. Kết quả đạt được

Dựa trên source code phân tích được, hệ thống đã đạt được các kết quả sau:

1. **Khung ứng dụng web hoàn chỉnh**: Cấu hình đầy đủ pipeline middleware, routing, session, static files trên ASP.NET Core 8.0.
2. **Hệ thống xác thực đa vai trò**: Phân biệt và kiểm soát quyền truy cập cho 4 loại người dùng thông qua Session và `SessionAccessGuard`.
3. **Bảo mật mật khẩu**: Toàn bộ mật khẩu được lưu trữ dưới dạng hash BCrypt — không lưu plaintext.
4. **Quản lý hồ sơ người dùng**: Cho phép thêm, sửa thông tin người dùng kèm upload ảnh đại diện.
5. **Quan hệ gia sư – môn học**: Hỗ trợ gán và hủy gán môn học cho từng gia sư.
6. **Danh mục địa chính tích hợp**: Load động danh sách xã theo tỉnh thông qua AJAX.
7. **Chức năng reset mật khẩu qua email**: Tự động sinh mật khẩu ngẫu nhiên, mã hóa và gửi về email người dùng.
8. **Dịch vụ nền cảnh báo lịch học**: `LessonAlertBackgroundService` hoạt động song song, tự động nhắc nhở khi lớp học sắp diễn ra.
9. **Có sẵn lớp seed dữ liệu mẫu**: `DemoDataSeeder` đã chuẩn bị 4 tài khoản mẫu theo vai trò.
10. **Giao diện Bootstrap 5**: Layout responsive đồng nhất trên mọi trang.

---

## 7. Hạn chế


1. **Xác thực dựa trên Session tự triển khai**: Không sử dụng ASP.NET Core Identity hoặc JWT, dẫn đến thiếu các tính năng bảo mật nâng cao như refresh token, lockout tài khoản sau đăng nhập sai nhiều lần, hoặc two-factor authentication.

2. **Thiếu phân quyền chi tiết bằng Attribute**: Việc kiểm tra quyền thông qua `SessionAccessGuard` trực tiếp trong action thay vì dùng `[Authorize]` attribute làm giảm tính nhất quán và dễ bỏ sót.

3. **Không có unit test**: Không tìm thấy bất kỳ test project nào trong solution.

4. **Upload file chưa xử lý bảo mật nâng cao**: Chức năng upload ảnh đại diện cần bổ sung kiểm tra MIME type và giới hạn kích thước file để ngăn chặn nguy cơ upload file độc hại.

---

## 8. Hướng phát triển

1. **Chuyển sang ASP.NET Core Identity**: Tích hợp Identity để có sẵn các tính năng bảo mật như lockout, email confirmation, two-factor authentication và quản lý role chuẩn hóa.

2. **Xây dựng API RESTful**: Tách frontend và backend bằng cách cung cấp Web API (ASP.NET Core Web API), cho phép tích hợp với ứng dụng di động hoặc SPA.

3. **Phân quyền chi tiết hơn**: Áp dụng policy-based authorization kết hợp Claims để kiểm soát quyền truy cập từng tài nguyên.

4. **Tích hợp thông báo thời gian thực**: Sử dụng SignalR thay thế hoặc bổ sung cho Background Service để đẩy cảnh báo lịch học đến giao diện người dùng theo thời gian thực.

5. **Hệ thống quản lý học phí**: Thêm module theo dõi học phí, thanh toán qua ngân hàng (tích hợp VNPay, Momo).

6. **Báo cáo và thống kê**: Bổ sung dashboard thống kê số lớp học, doanh thu, hiệu suất gia sư bằng biểu đồ (Chart.js).

7. **Thêm unit test và integration test**: Sử dụng xUnit/NUnit để kiểm thử tầng Logic, đảm bảo chất lượng khi mở rộng hệ thống.

8. **Triển khai Docker**: Đóng gói ứng dụng và SQL Server thành Docker Compose để dễ dàng triển khai trên các môi trường khác nhau.

9. **Cải thiện giao diện**: Xây dựng giao diện quản trị chuyên nghiệp hơn (AdminLTE, Tabler) và hỗ trợ đa ngôn ngữ.

---

## 9. Hướng dẫn cài đặt và chạy

### 9.1 Yêu cầu môi trường

| Phần mềm | Phiên bản tối thiểu | Ghi chú |
|----------|--------------------|----|
| .NET SDK | 8.0 | Bắt buộc |
| SQL Server | 2019 trở lên | Hoặc SQL Server Express |
| Visual Studio | 2022 (v17.x) | Hoặc VS Code + C# Extension |
| Git | 2.x | Để clone repository |

### 9.2 Các bước cài đặt và chạy

**Bước 1: Clone repository**

```bash
git clone <url-repository>
cd nhom1-He_thong_Quan_Ly_trung_tam_gia_su
```

**Bước 2: Cấu hình chuỗi kết nối cơ sở dữ liệu**

Tạo hoặc chỉnh sửa file `web/appsettings.json` (hoặc `appsettings.Development.json`) và thêm chuỗi kết nối:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=QuanLyTrungTamGiaSu;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> Điều chỉnh `Server` theo tên SQL Server instance của máy.

**Bước 3: Khôi phục NuGet packages**

```bash
dotnet restore
```

**Bước 4: Build solution**

```bash
dotnet build
```

**Bước 5: Chạy ứng dụng**

```bash
cd web
dotnet run
```

Hoặc mở file `.sln` trong Visual Studio 2022, đặt project `web` làm Startup Project, nhấn **F5** để chạy.

**Bước 6: Truy cập ứng dụng**

Mở trình duyệt và truy cập:

- HTTP: `http://localhost:5290`
- HTTPS: `https://localhost:7149`

> Dự án hiện chưa gọi `EnsureCreated()` và chưa gọi `DemoDataSeeder.Seed(...)` trong `Program.cs`; bạn cần tạo CSDL trước (migrations hoặc script SQL) nếu máy mới hoàn toàn.

**Bước 7: Đăng nhập với tài khoản mẫu**

| Email | Mật khẩu | Vai trò |
|-------|----------|---------|
| admin@demo.local | Admin123! | Quản trị viên |
| nv@demo.local | Nhanvien123! | Nhân viên |
| giasu@demo.local | Giasu123! | Gia sư |
| ph@demo.local | Phuhuynh123! | Phụ huynh |

---

## 10. Phụ lục nhanh

### 10.1 Danh sách interface nghiệp vụ

| Interface | Chức năng |
|-----------|-----------|
| `IMonhocLogic` | Lọc danh sách gia sư, quản lý môn học gia sư |
| `ILopHocLogic` | Quản lý lớp học, lịch học, hợp đồng, hoàn phí |
| `IUSERLogic` | Quản lý hồ sơ người dùng và đổi mật khẩu |
| `ITaiKhoanLogic` | Tạo/lưu tài khoản |
| `IDM_TinhLogic` | Lấy danh sách tỉnh |
| `IDM_XaLogic` | Lấy danh sách xã theo tỉnh |
| `IDM_NganHangLogic` | Lấy danh mục ngân hàng |

### 10.2 Danh sách tài liệu nội bộ

| Tài liệu | Mục đích |
|----------|----------|
| `docs/danh-gia-tong-quan-du-an.md` | Đánh giá tổng thể dự án |
| `docs/danh-gia-bai-giao-theo-video.md` | Đánh giá bài giao theo video |
| `video_thuyet_trinh.md` | Nội dung thuyết trình dự án |


