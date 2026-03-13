using He_thong_Quan_Ly_trung_tam_gia_su_Entity;
using Microsoft.EntityFrameworkCore;

namespace He_thong_Quan_Ly_trung_tam_gia_su.Infrastructure.BackgroundJobs;

public class LessonAlertBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LessonAlertBackgroundService> _logger;
    private readonly HashSet<string> _sentKeys = new();

    public LessonAlertBackgroundService(IServiceScopeFactory scopeFactory, ILogger<LessonAlertBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Lesson alert background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndSendAlerts(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed while checking lesson alerts.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task CheckAndSendAlerts(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Appdbcontext>();

        var now = DateTime.Now;
        var today = now.Date;
        var currentTime = now.TimeOfDay;
        var thuToday = ConvertToThu(today.DayOfWeek);

        lock (_sentKeys)
        {
            _sentKeys.RemoveWhere(x => !x.StartsWith(today.ToString("yyyyMMdd")));
        }

        var schedules = await (
            from lop in db.LopHoc
            join dk in db.LopHoc_Buoihocdangki on lop.ID equals dk.IDlophoc
            join tutor in db.USER on lop.idnguoinhan equals tutor.ID
            join tk in db.TaiKhoan on tutor.IDTK equals tk.ID
            where dk.thu == thuToday
                  && lop.TrangThai == 3
                  && (lop.isdetele == null || lop.isdetele == false)
                  && !string.IsNullOrEmpty(tk.Email)
            select new
            {
                LopId = lop.ID,
                MonId = lop.idmon,
                TutorEmail = tk.Email,
                StartTime = dk.giobatdau,
                EndTime = dk.gioketthuc
            }
        ).ToListAsync(cancellationToken);

        if (schedules.Count == 0)
            return;

        var lopIds = schedules.Select(x => x.LopId).Distinct().ToList();
        var todayLessons = await db.LopHoc_BuoiHoc
            .Where(x => lopIds.Contains(x.IDLop) && x.NgayHoc.HasValue && x.NgayHoc.Value.Date == today)
            .ToListAsync(cancellationToken);

        var monMap = await db.MonHoc
            .ToDictionaryAsync(x => x.ID, x => x.Name, cancellationToken);

        foreach (var s in schedules)
        {
            var lessonName = monMap.TryGetValue(s.MonId ?? 0, out var monName)
                ? monName
                : $"Lớp #{s.LopId}";

            var lessonToday = todayLessons
                .Where(x => x.IDLop == s.LopId)
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            var shouldSendOverdueStart = currentTime > s.StartTime
                                         && (lessonToday == null || lessonToday.TrangThai != 1 && lessonToday.TrangThai != 2);

            if (shouldSendOverdueStart)
            {
                TrySendOnce(
                    today,
                    s.LopId,
                    "overdue-start",
                    s.TutorEmail,
                    "[Cảnh báo] Quá giờ nhưng buổi học chưa bắt đầu",
                    $"<p>Buổi học <b>{lessonName}</b> đã quá giờ bắt đầu nhưng vẫn chưa được xác nhận bắt đầu.</p><p>Giờ bắt đầu dự kiến: <b>{s.StartTime:hh\\:mm}</b>.</p>");
            }

            var shouldSendOverdueEnd = currentTime > s.EndTime
                                       && lessonToday != null
                                       && lessonToday.TrangThai == 1;

            if (shouldSendOverdueEnd)
            {
                TrySendOnce(
                    today,
                    s.LopId,
                    "overdue-end",
                    s.TutorEmail,
                    "[Cảnh báo] Quá giờ nhưng buổi học chưa kết thúc",
                    $"<p>Buổi học <b>{lessonName}</b> đã quá giờ kết thúc dự kiến nhưng vẫn chưa được kết thúc.</p><p>Giờ kết thúc dự kiến: <b>{s.EndTime:hh\\:mm}</b>.</p>");
            }
        }
    }

    private void TrySendOnce(DateTime day, int lopId, string alertType, string email, string subject, string body)
    {
        var key = $"{day:yyyyMMdd}-{lopId}-{alertType}";

        lock (_sentKeys)
        {
            if (_sentKeys.Contains(key))
                return;
        }

        var emailService = new EmailService();
        var sent = emailService.SendMail(email, subject, body);

        if (!sent)
        {
            _logger.LogWarning("Cannot send lesson alert email for lop {LopId}, type {AlertType}, email {Email}", lopId, alertType, email);
            return;
        }

        lock (_sentKeys)
        {
            _sentKeys.Add(key);
        }

        _logger.LogInformation("Lesson alert email sent: lop {LopId}, type {AlertType}, email {Email}", lopId, alertType, email);
    }

    private static int ConvertToThu(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => 2,
            DayOfWeek.Tuesday => 3,
            DayOfWeek.Wednesday => 4,
            DayOfWeek.Thursday => 5,
            DayOfWeek.Friday => 6,
            DayOfWeek.Saturday => 7,
            DayOfWeek.Sunday => 8,
            _ => 2
        };
    }
}
