using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClockWidget.Models.Setting
{
    public static class SettingReader
    {
        public static IReadonlySetting Read(string filePath)
        {
            if (!File.Exists(filePath)) return new Setting();

            using var sr = new StreamReader(filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<Setting>(sr.ReadToEnd());
        }

        public static async Task<IReadonlySetting> ReadAsync(string filePath)
        {
            if (!File.Exists(filePath)) return new Setting();

            using var sr = new StreamReader(filePath, Encoding.UTF8);
            return JsonSerializer.Deserialize<Setting>(await sr.ReadToEndAsync());
        }
    }
}
