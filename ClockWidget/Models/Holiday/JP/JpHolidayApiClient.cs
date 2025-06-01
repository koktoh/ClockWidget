using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Net;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Holiday.JP
{
    internal class JpHolidayApiClient : RetryableApiClientBase, IHolidayApiClient
    {
        private const string API_URL = "https://www8.cao.go.jp/chosei/shukujitsu/syukujitsu.csv";

        public JpHolidayApiClient(ILogger<JpHolidayApiClient> logger, HttpClient httpClient)
            : base(logger, httpClient)
        {
        }

        public async Task<IEnumerable<Holiday>> GetHolidaysAsync()
        {
            using var _ = new LoggerScope(this._logger);

            try
            {
                this._logger.LogDebug("休日 CSV データ取得");
                var content = await this.RetrieveCsvBytesAsync();
                this._logger.LogDebug("休日 CSV データエンコード変換（S-JIS -> UTF-8）");
                var csvContent = this.DecodeShiftJisToUtf8(content);
                this._logger.LogDebug("休日 CSV データパース");
                var holidays = this.ParseCsv(csvContent);
                return holidays;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "休日データ取得失敗");
                return Enumerable.Empty<Holiday>();
            }
        }

        private async Task<byte[]> RetrieveCsvBytesAsync()
        {
            using var _ = new LoggerScope(this._logger);

            var res = await this.GetWithRetryAsync(API_URL);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsByteArrayAsync();
        }

        private string DecodeShiftJisToUtf8(byte[] src)
        {
            using var _ = new LoggerScope(this._logger);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encShiftJis = Encoding.GetEncoding("Shift-JIS");
            var utf8Data = Encoding.Convert(encShiftJis, Encoding.UTF8, src);
            return Encoding.UTF8.GetString(utf8Data);
        }

        private IEnumerable<Holiday> ParseCsv(string csvContent)
        {
            using var _ = new LoggerScope(this._logger);

            return csvContent.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries)
                .Skip(1) // ヘッダー行スキップ
                .Select(line => line.Split(','))
                .Where(columns => columns.Length == 2)
                .Select(columns => new Holiday
                {
                    Date = DateTime.Parse(columns[0]),
                    Name = columns[1].Trim()
                })
                .Where(x => x.Date.Year >= DateTime.Now.Year);
        }
    }
}
