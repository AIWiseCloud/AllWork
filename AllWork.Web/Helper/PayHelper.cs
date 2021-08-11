using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace AllWork.Web.Helper
{
    /// <summary>
    /// 微信支付相关配置与函数类
    /// 注：appsettings配置文件如果有中文，则可能乱码，请用记事本另存为utf8
    /// </summary>
    public class PayHelper
    {
        /// <summary>
        /// 小程序唯一标识
        /// </summary>
        public static string AppId = AppSettingsHelper.Configuration.GetValue<string>("App:AppId");

        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public static string MchId = AppSettingsHelper.Configuration.GetValue<string>("App:Mchid");

        /// <summary>
        /// 终端IP APP和网页支付提交用户端IP
        /// </summary>
        public static string IpAddr = AppSettingsHelper.Configuration.GetValue<string>("App:IpAddr");

        /// <summary>
        /// 商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        /// </summary>
        public static string Key = AppSettingsHelper.Configuration.GetValue<string>("App:Key");

        /// <summary>
        /// 支付结果通知API(要外网能访问，不能带参数，用post方式)
        /// </summary>
        public static string NotifyUrl = AppSettingsHelper.Configuration.GetValue<string>("App:NotifyUrl");

        /// <summary>
        /// 退款结果通知API(要外网能访问，不能带参数，用post方式)
        /// </summary>
        public static string RefudNotifyUrl = AppSettingsHelper.Configuration.GetValue<string>("App:RefundNotifyUrl");

        /// <summary>
        ///  证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        /// </summary>
        public static string CertPath = AppSettingsHelper.Configuration.GetValue<string>("App:CertPath");

        /// <summary>
        /// 证书密钥（默认为商户号）
        /// </summary>
        public static string CertPassword = AppSettingsHelper.Configuration.GetValue<string>("App:CertPassword");

        /// <summary>
        /// 生成随机串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            const string key = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            if (length < 1)
                return string.Empty;

            Random rnd = new Random();
            byte[] buffer = new byte[8];

            ulong bit = 31;
            ulong result;
            int index;
            StringBuilder sb = new StringBuilder((length / 5 + 1) * 5);

            while (sb.Length < length)
            {
                rnd.NextBytes(buffer);

                buffer[5] = buffer[6] = buffer[7] = 0x00;
                result = BitConverter.ToUInt64(buffer, 0);

                while (result > 0 && sb.Length < length)
                {
                    index = (int)(bit & result);
                    sb.Append(key[index]);
                    result = result >> 5;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTime()
        {
            TimeSpan cha = (DateTime.Now - TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local));
            long t = (long)cha.TotalSeconds;
            return t;
        }

        /// <summary>
        /// MD5签名方法
        /// </summary>
        /// <param name="inputText">加密参数</param>
        /// <returns></returns>
        public static string MD5(string inputText)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(inputText));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static SortedDictionary<string, object> GetFromXml(string xmlString)
        {
            SortedDictionary<string, object> sParams = new SortedDictionary<string, object>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);
            XmlElement root = doc.DocumentElement;
            int len = root.ChildNodes.Count;
            for (int i = 0; i < len; i++)
            {
                string name = root.ChildNodes[i].Name;
                if (!sParams.ContainsKey(name))
                {
                    sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                }
            }
            return sParams;
        }

        /// <summary>
        /// Dictionary格式转化成url参数格式
        /// </summary>
        /// <param name="dictData"></param>
        /// <returns>url格式串, 该串不包含sign字段值</returns>
        public static string ToUrl(SortedDictionary<string, object> dictData)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in dictData)
            {
                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value.ToString() + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        //支付结果通知再次签名时用
        public static string GetSignInfo(SortedDictionary<string, object> strParam)
        {
            int i = 0;
            var sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> temp in strParam)
            {
                if (temp.Value == null || temp.Value.ToString() == "" || temp.Key.ToLower() == "sign")
                {
                    continue;
                }
                i++;
                sb.Append(temp.Key.Trim() + "=" + temp.Value.ToString().Trim() + "&");
            }
            sb.Append("key=" + PayHelper.Key.Trim() + "");
            sign = MD5(sb.ToString()).ToUpper();
            return sign;
        }

        public static string GetXmlValue(string strXml, string strData)
        {
            string xmlValue = string.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strXml);
            var selectSingleNode = xmlDocument.DocumentElement.SelectSingleNode(strData);
            if (selectSingleNode != null)
            {
                xmlValue = selectSingleNode.InnerText;
            }
            return xmlValue;
        }

        /// <summary>
        /// AES-256-ECB字符解密
        /// </summary>
        /// <param name="str">要解密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string DecodeAES256ECB(string str, string key)
        {
            //加密和解密必须采用相同的key，具体值自己填，但是必须为32
            string r = string.Empty;
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            //byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            byte[] toEncryptArray = Convert.FromBase64String(str); //(1) 对加密串A做base64解码，得到加密串B
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            r = UTF8Encoding.UTF8.GetString(resultArray);
            return r;
        }

        /// <summary>
        /// 回应微信服务器的内容
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static string GetReturnXml(string returnCode, string returnMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<return_code><![CDATA[" + returnCode + "]]></return_code>");
            sb.Append("<return_msg><![CDATA[" + returnMsg + "]]></return_msg>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        public static string MakeSign(SortedDictionary<string, object> dictData)
        {
            //转url格式
            string str = ToUrl(dictData);
            //在string后加入API KEY
            str += "&key=" + Key;
            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }
    }
}
