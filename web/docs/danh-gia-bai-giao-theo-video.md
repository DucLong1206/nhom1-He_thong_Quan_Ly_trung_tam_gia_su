# Đánh giá bài giao theo video (Tutor Matching System)

## Phạm vi chấm
Đánh giá dựa trên mã nguồn hiện có trong repo, đối chiếu với 3 ý trong slide:
1. Matching gia sư theo tiêu chí.
2. Contract management (quản lý hợp đồng giao lớp).
3. Xử lý hoàn phí khi lớp hỏng trong 2 buổi đầu + có phí môi giới.

> Lưu ý: Điểm dưới đây là **ước lượng theo góc nhìn giảng viên** khi chỉ có mã nguồn (không chạy demo end-to-end do môi trường thiếu `dotnet`).

## 1) Matching gia sư theo yêu cầu — **Đạt khá tốt**

### Bằng chứng đã có
- Có trang tìm gia sư với bộ lọc:
  - từ khóa,
  - môn học,
  - giá tối đa/giờ,
  - sắp xếp tăng/giảm giá.
- Front-end gọi API `/MonHoc/getlist` với các tham số `keyword`, `idmon`, `gia`, `sapxep`.
- Backend nhận bộ lọc và gọi thủ tục `GiaSu_listdanhsanhgiasu` để trả danh sách gia sư phù hợp.

### Nhận xét theo đề bài
- Đề yêu cầu ví dụ “Môn Toán, khu vực Cầu Giấy, học phí < 200k”.
- Hệ thống đã có lọc môn + giá, và kết quả có hiển thị khu vực (`xa`).
- Tuy nhiên ở màn tìm kiếm hiện tại chưa thấy ô lọc khu vực riêng theo kiểu explicit như ví dụ đề.

**Mức hoàn thành ước lượng: 8/10 cho phần Matching.**

## 2) Contract Management — **Đạt**

### Bằng chứng đã có
- Có luồng tạo hợp đồng cho lớp qua `createhopdong(lopId)`.
- Có trang hợp đồng riêng (`/LopHoc/Contract`) và nút “Tôi đồng ý hợp đồng”.
- Có API đổi trạng thái hợp đồng (`/LopHoc/changhopdong`).
- Khi duyệt hợp đồng, hệ thống cập nhật trạng thái lớp/hợp đồng và tính lại phí môi giới.

### Nhận xét theo đề bài
- Đúng tinh thần quản lý hợp đồng giao lớp.
- Có trạng thái và có thao tác xác nhận từ gia sư.

**Mức hoàn thành ước lượng: 8.5/10 cho phần Contract Management.**

## 3) Hoàn phí khi lớp hỏng trong 2 buổi đầu + phí môi giới — **Đạt tốt**

### Bằng chứng đã có
- Có API kiểm tra điều kiện hoàn phí: `KiemTraDieuKienHoanPhi(lopId)`.
- Có API xử lý hoàn phí thực tế: `XuLyHoanPhiKhiLopHong2BuoiDau(lopId, idNhanVienXuLy)`.
- UI đã tích hợp nút “Yêu cầu hoàn phí”, hiển thị đủ/không đủ điều kiện.
- Quy tắc hiện tại:
  - Nếu đã có bản ghi hoàn phí: không cho hoàn lại.
  - Nếu đã học quá 2 buổi: không thuộc diện hoàn.
  - Nếu học 0–1 buổi: hoàn 100% `sotienMotBuoi`.
  - Nếu học 2 buổi: hoàn 50% `sotienMotBuoi`.
- Có logic tính phí môi giới:
  - `15%` tổng học phí dự kiến,
  - chặn min `20.000` và max `500.000`.

### Nhận xét theo đề bài
- Đã bám đúng ý “lớp hỏng trong 2 buổi đầu thì xử lý hoàn phí”.
- Đã có “thu phí môi giới” với công thức rõ ràng.
- Điểm trừ nhỏ: trong xử lý hoàn phí, trường `IDnhanvien` đang gán cứng `0` thay vì dùng tham số nhân viên xử lý.

**Mức hoàn thành ước lượng: 8.5/10 cho phần Hoàn phí + Phí môi giới.**

## Tổng điểm đề xuất

### Cách chấm nhanh theo 3 tiêu chí chính (thang 10)
- Matching: 8.0
- Contract: 8.5
- Hoàn phí + phí môi giới: 8.5

**Điểm trung bình đề xuất: ~8.3/10.**

## Gợi ý để nâng lên 9+ điểm
1. Bổ sung bộ lọc khu vực rõ ràng ngay tại trang Matching (quận/phường), bám sát ví dụ đề.
2. Viết test cho các rule hoàn phí (0/1/2/>2 buổi) để chứng minh tính đúng.
3. Sửa `IDnhanvien` khi tạo bản ghi hoàn phí để lưu đúng người xử lý.
4. Thêm log/audit cho các thao tác nhạy cảm: đồng ý hợp đồng, hủy lớp, hoàn phí.
