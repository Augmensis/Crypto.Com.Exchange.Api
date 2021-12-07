using System.ComponentModel;

namespace Crypto.Com.Exchange.Api.Consts
{
    public enum  enCandlestickPeriod
    {
        [Description("1m")]
        OneMin,

        [Description("5m")]
        FiveMins,

        [Description("15m")]
        FifteenMins,

        [Description("30m")]
        ThirtyMins,

        [Description("1h")]
        OneHour,

        [Description("4h")]
        FourHours,

        [Description("6h")]
        SixHours,

        [Description("12h")]
        TwelveHours,

        [Description("1D")]
        OneDay,

        [Description("7D")]
        OneWeek,

        [Description("14D")]
        TwoWeeks,

        [Description("1M")]
        OneMonth
    }
}
