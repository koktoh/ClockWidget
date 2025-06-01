using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Initialization;
using ClockWidget.Models.Repository;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Holiday.JP
{
    internal class JpHolidayService : AsyncInitializableBase, IHolidayService, IAsyncInitializable
    {
        private readonly IRepository<Holiday> _holidayRepository;

        private IEnumerable<Holiday> _holidays;

        public JpHolidayService(ILogger<JpHolidayService> logger, IRepository<Holiday> holidayRepository)
            : base(logger)
        {
            this._holidayRepository = holidayRepository;
        }

        protected override async Task InitializeInternalAsync()
        {
            using var _ = new LoggerScope(this._logger);

            if (this.IsInitialized) return;

            this._logger.LogInformation("初期化開始");

            var holidays = await this._holidayRepository.GetRecordsAsync();
            Interlocked.Exchange(ref this._holidays, holidays);

            this.SetInitialized();

            this._logger.LogInformation("初期化完了");
        }

        public IEnumerable<Holiday> GetHolidays()
        {
            using var _ = new LoggerScope(this._logger);

            return this._holidays;
        }

        public Holiday GetHoliday(DateTime date)
        {
            using var _ = new LoggerScope(this._logger);

            var holidays = this._holidays; // スレッドセーフにするためにローカル変数に格納
            return holidays?.FirstOrDefault(x => x.Date.Date == date.Date);
        }

        public Holiday GetToday()
        {
            using var _ = new LoggerScope(this._logger);

            return this.GetHoliday(DateTime.Today);
        }

        public bool IsHoliday(DateTime date)
        {
            using var _ = new LoggerScope(this._logger);

            var holiday = this.GetHoliday(date);

            return holiday is not null;
        }

        public bool IsTodayHoliday()
        {
            using var _ = new LoggerScope(this._logger);

            return this.IsHoliday(DateTime.Today);
        }
    }
}
