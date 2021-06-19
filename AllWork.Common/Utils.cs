using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

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
            string strRet = null;
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
    }
}
