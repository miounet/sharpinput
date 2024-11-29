using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Core.Comm
{
    public class Function
    {
        /// <summary>
        /// 将几个符号转换
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetKeysString(Keys keys)
        {
            switch (keys)
            {
                case Keys.Space:
                    return "~";
                case Keys.Oemcomma:
                    return ",";
                case Keys.OemPeriod:
                    return ".";
                case Keys.OemQuestion:
                    return "/";
                case Keys.Oem1:
                    return ";";
                default:
                    return keys.ToString();
            }
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static string[] Decrypt(string sourceFile, bool des = true)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("指定的文件路径不存在！", sourceFile);

            string pToDecrypt = File.ReadAllText(sourceFile, Encoding.UTF8);
            if (pToDecrypt == "")
                return null;

            if (des)
                pToDecrypt = Security.DecryptCommon(pToDecrypt);

            return pToDecrypt.Replace("\r\n", "\n").Split('\n');
        }

        /// <summary>
        /// 替换系统变量
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSystem(string str)
        {
            str = str.Replace("$20", " ");
            str = str.Replace("$y", DateTime.Now.Year.ToString());
            str = str.Replace("$d", DateTime.Now.Day.ToString().PadLeft(2, '0'));
            str = str.Replace("$h", DateTime.Now.Hour.ToString().PadLeft(2, '0'));
            str = str.Replace("$mi", DateTime.Now.Minute.ToString().PadLeft(2, '0'));
            str = str.Replace("$m", DateTime.Now.Month.ToString().PadLeft(2, '0'));
            str = str.Replace("$s", DateTime.Now.Second.ToString().PadLeft(2, '0'));
            str = str.Replace("$w", DateTime.Now.DayOfWeek.ToString().PadLeft(2, '0'));
            str = str.Replace("$os", System.Environment.OSVersion.Platform.ToString());
            str = str.Replace("$mname", System.Environment.MachineName.ToString());

            return str;
        }

        public static string ConvertToChinese(string xx)
        {
#if DEBUG
            return xx;
#endif
            double x = 0;
            if (double.TryParse(xx, out x))
            {
                string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
                string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
                return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
            }
            else return string.Empty;
        }

        public static string SentConverToDate(string x)
        {
#if DEBUG
            return x;
#endif
            try
            {
                if (x.Length <= 4 && !x.Contains("."))
                {
                    switch (x.Length)
                    {
                        case 1:
                            return "200" + x + "年";
                        case 2:
                            return "20" + x + "年";
                        case 3:
                            return "2" + x + "年";
                        default:
                            return x + "年";
                    }
                }
                else if (x.Length <= 6 && !x.Contains("."))
                    return x.Substring(0, 4) + "年" + x.Substring(4) + "月";
                return "";
            }
            catch { }
        }

        public static string SentConverToMonth(string x)
        {
#if DEBUG
            return x;
#endif
            try
            {
                if (x.Length <= 2 && !x.Contains("."))
                {
                    if (double.TryParse(x, out double d))
                        return d > 12
                            ? x.Substring(0, 1) + "月" + x.Substring(1) + "日"
                            : x + "月";
                    else
                        return x + "月";
                }
                else if (x.Length <= 3 && !x.Contains("."))
                {
                    if (double.TryParse(x, out double d))
                        return d > 33
                            ? "200" + x.Substring(0, 1) + "年" + x.Substring(1, 1) + "月" + x.Substring(2) + "日"
                            : x.Substring(0, 2) + "月" + x.Substring(2) + "日";
                    else
                        return x.Substring(0, 2) + "月" + x.Substring(2) + "日";
                }
                else if (x.Length <= 4 && !x.Contains("."))
                {
                    if (double.TryParse(x, out double d))
                        return d > 33
                            ? "20" + x.Substring(0, 2) + "年" + x.Substring(2, 1) + "月" + x.Substring(3) + "日"
                            : x.Substring(0, 2) + "月" + x.Substring(2) + "日";
                    else
                        return x.Substring(0, 2) + "月" + x.Substring(2) + "日";
                }
                return "";
            }
            catch { }
        }

        public static string SentConverToDay(string x)
        {
#if DEBUG
            return x;
#endif
            try
            {
                if (x.Length <= 2 && !x.Contains("."))
                {
                    if (double.TryParse(x, out double d))
                        return d > 31
                            ? x.Substring(0, 1) + "月" + x.Substring(1) + "日"
                            : x + "日";
                    else
                        return x + "日";
                }
                else if (double.TryParse(x, out double d))
                    return d > 31
                        ? x.Substring(0, 1) + "月" + x.Substring(1) + "日"
                        : x + "日";
                return "";
            }
            catch { }
        }

        public static string SentConverToWeek(string x)
        {
#if DEBUG
            return x;
#endif
            try
            {
                if (x.Length == 1 && !x.Contains("."))
                {
                    switch (x)
                    {
                        case "1":
                            return "星期一";
                        case "2":
                            return "星期二";
                        case "3":
                            return "星期三";
                        case "4":
                            return "星期四";
                        case "5":
                            return "星期五";
                        case "6":
                            return "星期六";
                        case "7":
                            return "星期日";
                    }
                }
                return "";
            }
            catch { }
        }

        public static T JsonToEntity<T>(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
        }

        public static string EntityToJson(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static string QueryCodeByValue(string strc)
        {
            string vstr = "";
            for (int v = 0; v < strc.Length; v++)
            {
                string str = strc.Substring(v, 1);
                int count = 0;
                for (int i = 0; i < Win.WinInput.Input.MasterDit.Length; i++)
                {
                    if (string.IsNullOrEmpty(Win.WinInput.Input.MasterDit[i])) continue;
                    var ar = Win.WinInput.Input.MasterDit[i].Split(' ');
                    if (ar.Length < 2) continue;

                    try
                    {
                        if (ar[1].Length == 1)
                        {
                            if (ar[1] == str)
                            {
                                count++;
                                if (count == 1) vstr += "\r\n" + str + ":";
                                if (vstr.IndexOf(ar[0] + " ") < 0)
                                    vstr += ar[0] + " ";
                            }
                        }
                        else continue;
                    }
                    catch  { }
                }

                if (Win.WinInput.Input.PinYi.ContainsKey(str))
                    vstr += Win.WinInput.Input.PinYi[str];
            }

            return vstr;
        }

        #region 设置自动启动

        /// <summary> 
        /// 开机启动项 
        /// </summary> 
        /// <param name="Started">是否启动</param> 
        public static void RunWhenStart(bool Started)
        {
            try
            {
                RegistryKey HKLM = Registry.LocalMachine;
                RegistryKey Run = HKLM.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                if (Started == true)
                {
                    Run.SetValue("速录宝-srkmm.ysepan.com", Application.ExecutablePath);
                    HKLM.Close();
                }
                else
                {
                    Run.DeleteValue("速录宝-srkmm.ysepan.com");
                    HKLM.Close();
                }
            }
            catch { }
        }

        public static bool FunionKey(Keys key)
        {
            //bool rev;
            //if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right)
            //    rev = true;
            //else if (key == Keys.F1 || key == Keys.F2 || key == Keys.F3 || key == Keys.F4
            //    || key == Keys.F5 || key == Keys.F6 || key == Keys.F7 || key == Keys.F8
            //    || key == Keys.F9 || key == Keys.F10 || key == Keys.F11 || key == Keys.F12
            //    )
            //    rev = true;
            //else if (key == Keys.Tab || key == Keys.CapsLock || key == Keys.Delete || key == Keys.Insert)
            //    rev = true;
            //else if (key == Keys.Home || key == Keys.End || key == Keys.PageDown || key == Keys.PageUp)
            //    rev = true;
            //else if (key == Keys.PrintScreen || key == Keys.Print || key == Keys.Scroll || key == Keys.Pause)
            //    rev = true;

            //rev = true;
            return true;
        }

        #endregion
    }
}
