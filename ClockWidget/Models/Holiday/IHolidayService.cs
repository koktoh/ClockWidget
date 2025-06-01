using System;
using System.Collections.Generic;
using ClockWidget.Models.Service;

namespace ClockWidget.Models.Holiday
{
    public interface IHolidayService : IClockWidgetService
    {
        IEnumerable<Holiday> GetHolidays();

        Holiday GetHoliday(DateTime date);

        Holiday GetToday();

        bool IsHoliday(DateTime date);

        bool IsTodayHoliday();
    }
}
