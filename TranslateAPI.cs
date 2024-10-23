using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace AutoTranslate
{
    internal class TranslateAPI
    {
        public static async Task<string> MyMethodAsync(string origin,string from,string to)
        {
            string q = origin;
            // 源语言
            //string from = "en";
            // 目标语言
            //string to = "zh";
            // 改成您的APP ID
            string appId = "20230717001746991";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "ZvI9cDt8VOOB1ftB3aVm";
            string sign = EncryptString(appId + q + salt + secretKey);
            string url = "http://api.fanyi.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(q);
            url += "&from=" + from;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 60000;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            var tcs = new TaskCompletionSource<string>();
            tcs.SetResult(retString);
            await Task.Delay(10);
            return retString;
        }
        public static async Task<string> Translate(string origin,string from,string to) 
        {
            if (origin == "")
                return "";
            //得到json并提取结果
            string jsonString = await MyMethodAsync(origin,from,to);
                //Task.Run(async () => await MyMethodAsync(origin)).GetAwaiter().GetResult();
            // 解析JSON字符串
            JObject jsonObject = JObject.Parse(jsonString);

            /*if (jsonObject["error_code"].ToString()!=null) 
            {
                return "error";
            }*/
            // 获取"dst"值
            string dstValue = jsonObject["trans_result"][0]["dst"].ToString();
            if (dstValue.Contains("“") || dstValue.Contains("”"))
            {
                dstValue = dstValue.Replace("“", "\"");
                dstValue = dstValue.Replace("”", "\"");
            }
            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.ShowLog(dstValue);
            var tcs = new TaskCompletionSource<string>();
            tcs.SetResult(dstValue);
            return dstValue;

        }



      
        // 计算MD5值
        public static string EncryptString(string str)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }
    }
}
