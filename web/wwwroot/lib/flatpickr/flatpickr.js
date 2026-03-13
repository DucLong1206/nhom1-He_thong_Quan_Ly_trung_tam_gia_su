(function (global) {
  function toElements(target) {
    if (!target) return [];
    if (typeof target === 'string') return Array.from(document.querySelectorAll(target));
    if (target instanceof Element) return [target];
    if (target.length) return Array.from(target);
    return [];
  }

  function normalizeDate(value, format) {
    if (!value) return '';
    if (value instanceof Date && !Number.isNaN(value.getTime())) {
      const pad = (n) => String(n).padStart(2, '0');
      const y = value.getFullYear();
      const m = pad(value.getMonth() + 1);
      const d = pad(value.getDate());
      const hh = pad(value.getHours());
      const mm = pad(value.getMinutes());
      if (format === 'H:i') return `${hh}:${mm}`;
      return `${y}-${m}-${d}`;
    }
    return String(value);
  }

  function flatpickr(target, options) {
    var opts = options || {};
    var elements = toElements(target);

    elements.forEach(function (el) {
      if (!(el instanceof HTMLInputElement)) return;

      if (opts.noCalendar) {
        el.type = 'time';
        el.step = String((opts.minuteIncrement || 1) * 60);
      } else {
        el.type = 'date';
      }

      if (opts.minDate === 'today') {
        var today = new Date();
        var y = today.getFullYear();
        var m = String(today.getMonth() + 1).padStart(2, '0');
        var d = String(today.getDate()).padStart(2, '0');
        el.min = `${y}-${m}-${d}`;
      }
    });

    return {
      setDate: function (value) {
        var formatted = normalizeDate(value, opts.dateFormat);
        elements.forEach(function (el) {
          if (el instanceof HTMLInputElement) el.value = formatted;
        });
      }
    };
  }

  global.flatpickr = global.flatpickr || flatpickr;
})(window);
