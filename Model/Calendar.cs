using System.Collections;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System;
using System.Globalization;
public class Calendar
{
    #region Persian Conversion
    [SqlFunction(IsDeterministic = false)]
    public static string ToGregorian(string xCalendarType, string xDateTime)
    {
        if (string.IsNullOrWhiteSpace(xDateTime))
            return null;
        xDateTime = xDateTime.Trim();
        if (string.IsNullOrWhiteSpace(xDateTime))
            return null;

        var xParts = xDateTime.ToString().Split(' ');
        var strArrDateParts = xParts[0].Split('/');
        DateTime dt = new DateTime();
        switch (xCalendarType.ToLower())
        {
            case "hijrighamari":
                var hijriCalendar = new HijriCalendar();
                dt = hijriCalendar.ToDateTime(
                           int.Parse(strArrDateParts[0]),
                           int.Parse(strArrDateParts[1]),
                           int.Parse(strArrDateParts[2]), 0, 0, 0, 0);
                dt = Convert.ToDateTime(dt.ToString());
                break;
            case "hijrishamsi":
                var persianCalendar = new PersianCalendar();
                dt = persianCalendar.ToDateTime(
                           int.Parse(strArrDateParts[0]),
                           int.Parse(strArrDateParts[1]),
                           int.Parse(strArrDateParts[2]), 0, 0, 0, 0);
                dt = Convert.ToDateTime(dt.ToString());
                break;
            default:
                return null;
        }
        var time = "";
        if (xParts.Length == 2)
        {
            time = xParts[1];
        }
        return dt.Year.ToString("0000") + "/" + dt.Month.ToString("00") + "/" + dt.Day.ToString("00") + (!string.IsNullOrWhiteSpace(time) ? " " + time : "");
    }

    [SqlFunction(IsDeterministic = false)]
    public static string GregorianTo(string xCalendarType, string xDateTime)
    {
        string result = null;
        if (string.IsNullOrWhiteSpace(xDateTime))
            return null;
        var _xDateTime = Convert.ToDateTime(xDateTime);
        var _xTime = xDateTime.Split(' ');
        switch (xCalendarType.ToLower())
        {
            case "hijrishamsi":
                var pc = new PersianCalendar();
                result = pc.GetYear(_xDateTime).ToString("0000") + "/" + pc.GetMonth(_xDateTime).ToString("00") + "/" +
                       pc.GetDayOfMonth(_xDateTime).ToString("00") + (_xTime.Length == 2 ? " " + _xTime[1] : "");
                break;
            case "hijrighamari":
                var hc = new HijriCalendar();
                result = hc.GetYear(_xDateTime).ToString("0000") + "/" + hc.GetMonth(_xDateTime).ToString("00") + "/" +
                       hc.GetDayOfMonth(_xDateTime).ToString("00") + (_xTime.Length == 2 ? " " + _xTime[1] : "");
                break;
            default:
                return null;
        }
        return result;
    }
    #endregion
}