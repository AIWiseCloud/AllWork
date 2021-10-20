using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

namespace AllWork.Common
{
    public static class Utils
    {
        /// <summary>
        ///  通过NetworkInterface读取网卡Mac
        /// </summary>
        /// <returns></returns>
        public static string GetMacByNetworkInterface()
        {
            var sb = new StringBuilder();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface ni in interfaces)
            {
                sb.Append(ni.GetPhysicalAddress().ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 文件转base64
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static String FileToBase64(String fileName)
        {
            FileStream filestream = new FileStream(fileName, FileMode.Open);
            byte[] arr = new byte[filestream.Length];
            filestream.Read(arr, 0, (int)filestream.Length);
            string baser64 = Convert.ToBase64String(arr);
            filestream.Close();
            return baser64;
        }

        /// <summary>
        /// 文件转换成Base64字符串
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public static String FileToBase64(Stream fs)
        {
            string strRet;
            try
            {
                if (fs == null) return null;
                byte[] bt = new byte[fs.Length];
                fs.Read(bt, 0, bt.Length);
                strRet = Convert.ToBase64String(bt);
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strRet;
        }
       

        /// <summary>
        /// 获取10位时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return (DateTime.Now.Ticks - DateTime.Parse("1970-01-01 00:00:00").Ticks) / 10000000;
        }

        /// <summary>
        /// /加密随机数生成器 生成随机种子
        /// </summary>
        /// <returns></returns>
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider r = new System.Security.Cryptography.RNGCryptoServiceProvider();
            r.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 判断输入的字符串是否是一个合法的手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1[345789]\\d{9}$");
            return regex.IsMatch(input);

        }

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNum(int min = 0, int max = 30)
        {
            return new Random(GetRandomSeed()).Next(min, max);
        }

        /// <summary>
        /// 产生13位不重复的数字码（时间戮+随机数）
        /// </summary>
        /// <returns></returns>
        public static long CreateUniqueId()
        {
            var prefix = GetTimeStamp();
            var postfix = GetRandomNum(100, 999);
            return long.Parse($"{prefix}{postfix}");
        }

        /// <summary>
        /// 获取11位日期加随机数码（不保证绝对不重复)
        /// </summary>
        /// <returns></returns>
        public static string CreateDigitSn()
        {
            var dt = DateTime.Now;
            var y = dt.Year.ToString().Substring(2, 2);
            var m = dt.Month.ToString().PadLeft(2, '0');
            var d = dt.Day.ToString().PadLeft(2, '0');
            var h = dt.Day.ToString().PadLeft(2, '0');
            return y + m + d + h + GetRandomNum(100, 999);
        }
    }
}
