using System;

namespace Core.Config
{
    /// <summary>
    /// 配置文件
    /// </summary>
    [Serializable]
    public class SetInfo
    {
        public static string GetValue(string skey, string[] setting)
        {
            string ss = string.Empty;

            foreach (var s in setting)
                if (s.StartsWith($"{skey}="))
                    ss = s.Replace($"{skey}=", "");

            return ss;
        }
    }
}
