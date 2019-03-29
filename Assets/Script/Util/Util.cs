using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using DG.Tweening;

/// <summary>
/// 工具辅助类
/// </summary>
public class Util
{


    public static void ChangeLayer(Transform T, int layer)
    {
        T.gameObject.layer = layer;
        for (int i = 0; i < T.childCount; i++)
        {
            Transform childTransform = T.GetChild(i);
            childTransform.gameObject.layer = layer;
            ChangeLayer(childTransform, layer);
        }
    }

    public static void ChangeClashObjLayer(Transform T, int layer)
    {
        for (int i = 0; i < T.childCount; i++)
        {
            Transform childTransform = T.GetChild(i);
            if (childTransform.name.CompareTo("Mass") == 0 && !T.name.ToLower().Contains("hull"))
            {
                childTransform.gameObject.layer = LayerMask.NameToLayer("Mass");
            }
            else
                childTransform.gameObject.layer = layer;
            
            ChangeClashObjLayer(childTransform, layer);
        }
    }

    /// <summary>
    /// 搜索所有子物体，并执行委托
    /// </summary>
    /// <param name="parent">Parent.</param>
    /// <param name="a">The alpha component.</param>
    public static void DoForAllChild(Transform parent, Action<Transform> a)
    {
        if (parent == null)
        {
            Debug.LogError("Parent Is null!");
            return;
        }
        
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform childTransform = parent.GetChild(i);
            if (a != null)
                a(childTransform);

            DoForAllChild(childTransform, a);
        }
    }

    /// <summary>
    /// 查找子物体
    /// </summary>
    /// <returns>The transform.</returns>
    /// <param name="parent">Parent.</param>
    /// <param name="childName">Child name.</param>
    public static Transform GetTransform(Transform parent, string childName)
    {  
        if (parent.name == childName)
        {  
            return parent;  
        }  
        if (parent.childCount < 1)
        {  
            return null;  
        }  
        Transform t = null;  
        for (int i = 0; i < parent.transform.childCount; i++)
        {  
            Transform temp = parent.GetChild(i);  
            t = GetTransform(temp, childName);  
            if (t != null)
            {  
                break;  
            }  
        }  
        return t;  
    }

    public static Vector3 BezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, float t)
    {
        Vector3 B = Vector3.zero;
        float t1 = (1 - t) * (1 - t);
        float t2 = t * (1 - t);
        float t3 = t * t;
        B = P0 * t1 + 2 * t2 * P1 + t3 * P2;
        return B;
    }

    public static DateTime MilliSecTimeStampToDateTime(string strTimeStamp)
    {  
        long timeStamp = long.Parse(strTimeStamp);
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));  //时间戳起始点转为目前时区
        return dtStart.AddMilliseconds(timeStamp); //加上时间间隔得到目标时间
    }

    public static DateTime MilliSecTimeStampToDateTime(long timeStamp)
    {  
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));  //时间戳起始点转为目前时区
        return dtStart.AddMilliseconds(timeStamp); //加上时间间隔得到目标时间
    }


    public static bool isTick(long stamp)
    {
        if (stamp > 9999999999999)
        {
            return true;
        } 
        return false;
    }

    public static long Tick2TimeStamp(long tick)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (tick - startTime.Ticks) / 10000000;   //除10000000调整为10位      
        return t;
    }

    public static DateTime TimeStamp2DateTime(long timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));  //时间戳起始点转为目前时区
        string timestr = timeStamp.ToString();
        if (timestr.Length > 10)
        {
            timestr = timestr.Substring(0, 10);
            timeStamp = long.Parse(timestr);

            Debug.Log("<color=#FF0000>Error Num! </color>");
        }
        return dtStart.AddSeconds(timeStamp); //加上时间间隔得到目标时间
    }

    public static DateTime TimeStamp2DateTime(double timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));  //时间戳起始点转为目前时区

        long tempTimeStamp = (long)timeStamp;
        string timestr = tempTimeStamp.ToString();
        if (timestr.Length > 10)
        {
            timestr = timestr.Substring(0, 10);
            timeStamp = double.Parse(timestr);

            Debug.Log("<c=#FF0000>Error Num! </c>");
        }
        return dtStart.AddSeconds(timeStamp); //加上时间间隔得到目标时间
    }

    public static long DateTime2TimeStamp(DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
        long t = (time.Ticks - startTime.Ticks) / 10000000;   //除10000000调整为10位      
        return t;
    }

    //获取时间戳
    //	public static long DateTime2TimeStamp(System.DateTime time)
    //	{
    //		System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
    //		long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位
    //		return t;
    //	}
    //
    //	public static DateTime TimeStamp2DateTime(long timeStamp)
    //	{
    //		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
    //		long lTime = timeStamp * 10000;
    //		TimeSpan toNow = new TimeSpan(lTime);
    //		return dtStart.Add(toNow);
    //	}
    //
    //	public static long ServerTimeStampToCSharpTimeStamp(string timeStamp)
    //	{
    //		try
    //		{
    //			long longTimeStamp = long.Parse(timeStamp);
    //			DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));  //时间戳起始点转为目前时区
    //			dtStart.AddMilliseconds(longTimeStamp); //加上时间间隔得到目标时间
    //			return dtStart.Ticks;
    //		}catch (Exception e) {
    //			Debug.LogError (e.Message);
    //		}
    //		return 0;
    //	}



    public static string SecondsToTimeStr(int sec)
    {
        string sHour;
        string sSecond;
        string sMinute;
        int iHour;
        int iSecond;
        int iMinute;

        iHour = sec / 3600;
        sHour = iHour < 10 ? ("0" + iHour) : iHour.ToString();

        iSecond = sec % 60;
        if (iSecond < 10)
            sSecond = "0" + iSecond;
        else
            sSecond = iSecond + "";

        iMinute = sec % 3600 / 60;
        sMinute = iMinute < 10 ? ("0" + iMinute) : iMinute.ToString();

		if (iHour <= 0) {
			return sMinute + ":" + sSecond;
		} else {
			return sHour + ":" + sMinute + ":" + sSecond;
		}

    }

    public static string SecondsToMinutes(int sec)
    {
        TimeSpan ts = new TimeSpan(0, 0, sec);
        sb.Length = 0;

        if (ts.Minutes < 10)
            sb.Append("0").Append(ts.Minutes);
        else
            sb.Append(ts.Minutes);

        if (ts.Seconds < 10)
            sb.Append(":").Append("0").Append(ts.Seconds);
        else
            sb.Append(":").Append(ts.Seconds);

        return sb.ToString();
    }

    static StringBuilder sb = new StringBuilder();
    static string d, h, m, s;

    public static string SecondsToTimeChineseLong(int sec)
    {
//        if (string.IsNullOrEmpty(d))
//        {
//            d = LocalizationData.Instance.GetText("Time_Day");
//            h = LocalizationData.Instance.GetText("Time_Hour");
//            m = LocalizationData.Instance.GetText("Time_Min");
//            s = LocalizationData.Instance.GetText("Time_Sec");
//        }
        

        TimeSpan ts = new TimeSpan(0, 0, sec);
        sb.Length = 0;

        if (ts.Days > 0)
            sb.Append(ts.Days).Append(d).Append(ts.Hours).Append(h).Append(ts.Minutes).Append(m).Append(ts.Seconds).Append(s);
        else if (ts.Hours > 0)
            sb.Append(ts.Hours).Append(h).Append(ts.Minutes).Append(m).Append(ts.Seconds).Append(s);
        else
            sb.Append(ts.Minutes).Append(m).Append(ts.Seconds).Append(s);

        return sb.ToString();
    }

    public static string Seconds2Minute(float sec)
    {
        int min = (int)sec / 60;
        int s = (int)sec % 60;

        string str = string.Format("{0:00}:{1:00}", min, s);
        return str;
    }

    static StringBuilder strBuilder = new StringBuilder();

    public static string SecondsToTimeChineseDay(int sec)
    {
//        if (string.IsNullOrEmpty(d))
//        {
//            d = LocalizationData.Instance.GetText("Time_Day");
//            h = LocalizationData.Instance.GetText("Time_Hour");
//            m = LocalizationData.Instance.GetText("Time_Min");
//            s = LocalizationData.Instance.GetText("Time_Sec");
//        }

        TimeSpan ts = new TimeSpan(0, 0, sec);
        strBuilder.Length = 0;

        if (ts.Days > 0)
            strBuilder.Append(ts.Days).Append(d).Append(ts.Hours).Append(h);
        else if (ts.Hours > 0)
            strBuilder.Append(ts.Hours).Append(h).Append(ts.Minutes).Append(m);
        else
            strBuilder.Append(ts.Minutes).Append(m).Append(ts.Seconds).Append(s);

        return strBuilder.ToString();
    }

    public static double SystemTicksToSecond(long tick)
    {
        return  (tick / (10000000.0));
    }

    public static double SystemTicksToDay(long tick)
    {
        return  (tick / (10000000.0) / 60.0 / 60.0 / 24.0);
    }

    public static  long SystemSecondToTicks(double second)
    {
        return   (long)(second * (10000000));
    }

    /// <summary> 
    /// 获取字符串长度，一个汉字算两个字节 
    /// </summary> 
    /// <param name="str"></param> 
    /// <returns></returns> 
    public static int GetStrLength(string str)
    { 
        if (str.Length == 0)
            return 0; 
        int tempLen = 0; 
        byte[] s = System.Text.Encoding.ASCII.GetBytes(str); 
        for (int i = 0; i < s.Length; i++)
        { 
            if ((int)s[i] == 63)
            { 
                tempLen += 2; 
            }
            else
            { 
                tempLen += 1; 
            } 
        } 
        return tempLen; 
    }

    /// <summary>   
    /// 判断两个日期是否在同一周   
    /// </summary>   
    /// <param name="dtmS">开始日期</param>   
    /// <param name="dtmE">结束日期</param>  
    /// <returns></returns>   
    public static bool IsInSameWeek(DateTime start, DateTime end)
    {  
        TimeSpan ts = end - start;  
        double dbl = ts.TotalDays;  
        int intDow = Convert.ToInt32(end.DayOfWeek);  
        if (intDow == 0)
            intDow = 7;
        
        if (dbl >= 7 || dbl >= intDow)
            return false;
        else
            return true;  
    }

    public static bool IsSameDay(DateTime last, DateTime now)
    {
        if (last.DayOfYear == now.DayOfYear)
            return true;
        return false;
    }

    public static string Vector3ToString(Vector3 vec)
    {
        string str = vec.ToString().Replace(",", "*");
        str = str.Replace("(", "");
        str = str.Replace(")", "");
        str = str.Replace("0.0", "0");
        str = str.Replace(" ", "");
        return str;
    }

    public static Vector3 StringToVector3(string str)
    {
        string[] strArr = str.Split('*');
        return new Vector3(float.Parse(strArr[0]), float.Parse(strArr[1]), float.Parse(strArr[2]));
    }


    public static string Md5Func(string source)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
        md5.Clear();

        string destString = string.Empty;
        for (int i = 0; i < md5Data.Length; i++)
        {
            //返回一个新字符串，该字符串通过在此实例中的字符左侧填充指定的 Unicode 字符来达到指定的总长度，从而使这些字符右对齐。
            // string num=12; num.PadLeft(4, '0'); 结果为为 '0012' 看字符串长度是否满足4位,不满足则在字符串左边以"0"补足
            //调用Convert.ToString(整型,进制数) 来转换为想要的进制数
            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
        }
        //使用 PadLeft 和 PadRight 进行轻松地补位
        destString = destString.PadLeft(32, '0');
        return destString;
    }


    /// <summary>
    /// 获取设备唯一识别码，每次调用获取到的值都不同，需要存储
    /// </summary>
    /// <returns>The unique identifier.</returns>
    public static string GetUniqueIdentifier()
    {
        string str = SystemInfo.graphicsDeviceID + SystemInfo.deviceName + SystemInfo.deviceUniqueIdentifier + DateTime.Now.Ticks + UnityEngine.Random.Range(1, 1000000);
        string md5Str = Util.Md5Func(str);
        return md5Str;
    }

    public static Vector3 ParseVector3(string str)
    {
        if (string.IsNullOrEmpty(str))
            return Vector3.zero;
        str = str.Replace("(", "").Replace(")", "");
        string[] s = str.Split(',');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
}
