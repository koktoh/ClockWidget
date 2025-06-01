namespace ClockWidget.Models.Weather.Amedas.Json.Data
{
    public enum AQC
    {
        /// <summary>
        /// 正常
        /// </summary>
        A,
        /// <summary>
        /// 準正常（やや疑わしい）
        /// </summary>
        B,
        /// <summary>
        /// 非常に疑わしい
        /// </summary>
        C,
        /// <summary>
        /// 利用に適さない
        /// </summary>
        D,
        /// <summary>
        /// 観測値は期間内で資料数が不足している
        /// </summary>
        E,
        /// <summary>
        /// 点検又は計画休止のため欠測
        /// </summary>
        F,
        /// <summary>
        /// 障害のため欠測
        /// </summary>
        G,
        /// <summary>
        /// この要素の観測はしていない
        /// </summary>
        H,
    }
}
