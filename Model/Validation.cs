using System.Net;
using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.RegularExpressions;

public class Validation
{
    [SqlFunction(IsDeterministic = false)]
    public static bool CheckRegEx(string xExpr, string xRegEx)
    {
        try
        {
            return string.IsNullOrWhiteSpace(xExpr) || Regex.IsMatch(xExpr.ToString(), xRegEx.ToString());
        }
        catch (Exception)
        {
            return false;
        }
    }

    [SqlFunction(IsDeterministic = false)]
    public static bool IsIPv4(string xIPv4)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(xIPv4))
                return true;
            if (xIPv4.Split('.').Length != 4)
                return false;
            IPAddress address;
            return string.IsNullOrWhiteSpace(xIPv4) || IPAddress.TryParse(xIPv4.ToString(), out address);
        }
        catch (Exception)
        {
            return false;
        }
    }

    [SqlFunction(IsDeterministic = false)]
    public static bool IsEmail(string xEmail)
    {
        try
        {
            return string.IsNullOrWhiteSpace(xEmail) || Regex.IsMatch(xEmail.ToString().ToLower(), "^(([^<>()[\\]\\.,;:\\s@\"]+(\\.[^<>()[\\]\\.,;:\\s@\"]+)*)|(\".+\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$");
        }
        catch (Exception)
        {
            return false;
        }
    }

    //[SqlFunction(IsDeterministic = true)]
    //public static bool IsIranianNationalCode(string xNationalCode)
    //{
    //    try
    //    {
    //        if (string.IsNullOrWhiteSpace(xNationalCode)) return true;
    //        //در صورتی که کد ملی وارد شده تهی باشد
    //        if (String.IsNullOrEmpty(xNationalCode.ToString()))
    //            //throw new Exception("لطفا کد ملی را صحیح وارد نمایید");
    //            return false;
    //        //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
    //        if (xNationalCode.ToString().Length != 10)

    //            //throw new Exception("طول کد ملی باید ده کاراکتر باشد");
    //            return false;
    //        //در صورتی که کد ملی ده رقم عددی نباشد
    //        var regex = new Regex(@"\d{10}");
    //        if (!regex.IsMatch(xNationalCode.ToString()))
    //            //throw new Exception("کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید");
    //            return false;
    //        //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
    //        var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
    //        if (allDigitEqual.Contains(xNationalCode.ToString())) return false;
    //        //عملیات شرح داده شده در بالا
    //        var chArray = xNationalCode.ToString().ToCharArray();
    //        var num0 = Convert.ToInt32(chArray[0]) * 10;
    //        var num2 = Convert.ToInt32(chArray[1]) * 9;
    //        var num3 = Convert.ToInt32(chArray[2]) * 8;
    //        var num4 = Convert.ToInt32(chArray[3]) * 7;
    //        var num5 = Convert.ToInt32(chArray[4]) * 6;
    //        var num6 = Convert.ToInt32(chArray[5]) * 5;
    //        var num7 = Convert.ToInt32(chArray[6]) * 4;
    //        var num8 = Convert.ToInt32(chArray[7]) * 3;
    //        var num9 = Convert.ToInt32(chArray[8]) * 2;
    //        var a = Convert.ToInt32(chArray[9]);
    //        var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
    //        var c = b % 11;
    //        return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
    //    }
    //    catch (Exception)
    //    {
    //        return false;
    //    }
    //}
}
