# Hướng dẫn giao việc cho Codex: mục 2-3 (hoàn phí + công thức phí môi giới)

## 1) Công thức phí môi giới (đã áp dụng trong code)

Hiện tại hệ thống dùng công thức:

```text
TongHocPhiDuKien = sotienMotBuoi * soBuoiCamKet
PhiTheoTyLe = TongHocPhiDuKien * 15%
PhiMoiGioi = clamp(PhiTheoTyLe, min = 20.000, max = 500.000)
```

Trong đó:
- `clamp(x, min, max)` nghĩa là chặn giá trị trong đoạn `[min, max]`.
- Kết quả được làm tròn đến đơn vị VNĐ.

## 2) Rule hoàn phí khi lớp hỏng trong 2 buổi đầu (đã áp dụng trong code)

- Đếm `soBuoiDaHoc` theo bảng `LopHoc_BuoiHoc` với điều kiện đã kết thúc buổi học.
- Nếu `soBuoiDaHoc > 2` -> không hoàn phí theo rule này.
- Nếu `soBuoiDaHoc <= 2`:
  - 0 hoặc 1 buổi -> hoàn 100% phí môi giới.
  - 2 buổi -> hoàn 50% phí môi giới.
- Tạo bản ghi `HoanPhi`, cập nhật trạng thái `HopDong` và `LopHoc`.

## 3) Câu lệnh/prompt mẫu để Codex làm luôn (có commit)

Bạn có thể gửi cho Codex nguyên văn như sau:

```text
Bỏ qua yêu cầu matching, chỉ làm mục 2 và 3.

Mục 2: Triển khai API xử lý hoàn phí khi lớp hỏng trong 2 buổi đầu.
- Đếm số buổi đã học từ LopHoc_BuoiHoc.
- Nếu >2 thì từ chối hoàn phí.
- Nếu <=2 thì tính tiền hoàn theo rule:
  + 0-1 buổi: hoàn 100% phí môi giới
  + 2 buổi: hoàn 50% phí môi giới
- Tạo bản ghi HoanPhi và cập nhật trạng thái HopDong/LopHoc.

Mục 3: Chuẩn hóa công thức phí môi giới, không hardcode.
- Dùng công thức:
  PhiMoiGioi = clamp((sotienMotBuoi * soBuoiCamKet * 0.15), 20000, 500000)
- Áp dụng nhất quán khi tạo hợp đồng và khi duyệt hợp đồng.

Yêu cầu bắt buộc:
- Sửa interface + logic + controller endpoint đầy đủ.
- Chạy dotnet build để kiểm tra.
- Commit thay đổi với message rõ nghĩa.
- Tạo PR title/body nêu rõ business rule, endpoint, và công thức.
```
