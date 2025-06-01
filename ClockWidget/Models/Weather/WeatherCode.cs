namespace ClockWidget.Models.Weather
{
    public enum WeatherCode
    {
        /// <summary>
        /// 晴れ
        /// </summary>
        Clear = 0,
        /// <summary>
        /// 曇り
        /// </summary>
        Cloudy,
        /// <summary>
        /// 煙霧
        /// </summary>
        Smoke,
        /// <summary>
        /// 霧
        /// </summary>
        Fog,
        /// <summary>
        /// 降水またはしゅう雨性の降水
        /// </summary>
        PrecipitationOrShowers,
        /// <summary>
        /// 霧雨
        /// </summary>
        Drizzle,
        /// <summary>
        /// 着氷性の霧雨
        /// </summary>
        FreezingDrizzle,
        /// <summary>
        /// 雨
        /// </summary>
        Rain,
        /// <summary>
        /// 着氷性の雨
        /// </summary>
        FreezingRain,
        /// <summary>
        /// みぞれ
        /// </summary>
        Sleet,
        /// <summary>
        /// 雪
        /// </summary>
        Snow,
        /// <summary>
        /// 凍雨
        /// </summary>
        IcePellet,
        /// <summary>
        /// 霧雪
        /// </summary>
        SnowGrain,
        /// <summary>
        /// しゅう雨または止み間のある雨
        /// </summary>
        ShowersOrIntermittentRain,
        /// <summary>
        /// しゅう雪または止み間のある雪
        /// </summary>
        SnowShowersOrIntermittentSnow,
        /// <summary>
        /// ひょう
        /// </summary>
        Hail,
        /// <summary>
        /// 雷
        /// </summary>
        Thunder,
        /// <summary>
        /// 保留 (17-29)
        /// </summary>
        Pending,
        /// <summary>
        /// 天気不明
        /// </summary>
        Unknown = 30,
        /// <summary>
        /// 欠測
        /// </summary>
        Missing,
    }
}
