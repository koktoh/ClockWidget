using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClockWidget.Models.Setting
{
    public static class SettingWriter
    {
        public static void Write(string filePath, Setting setting)
        {
            using var sw = new StreamWriter(filePath, false, Encoding.UTF8);
            sw.Write(JsonSerializer.Serialize(setting, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static async Task WriteAsync(string filePath, Setting setting)
        {
            using var sw = new StreamWriter(filePath, false, Encoding.UTF8);
            await sw.WriteAsync(JsonSerializer.Serialize(setting, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
