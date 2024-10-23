using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoTranslate
{
    internal class FileStream
    {
        public static async void FolderTranslate(string regex,string folderPath,string to = "zh",string from = "en") 
        {
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(regex))
                    {
                        int startIndex = lines[i].IndexOf('"') + 1;
                        int endIndex = lines[i].LastIndexOf('"');

                        if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
                        {
                            string originalText = lines[i].Substring(startIndex, endIndex - startIndex);
                            string newText = originalText.Replace(originalText, await TranslateAPI.Translate(originalText,from,to));

                            lines[i] = lines[i].Remove(startIndex, endIndex - startIndex).Insert(startIndex, newText);
                        }
                    }
                }

                File.WriteAllLines(filePath, lines);
            }

        }
        public static async void FolderRegexTranslate(string regexstr, string folderPath)
        {
            string[] filePaths = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in filePaths)
            {
                string[] lines = File.ReadAllLines(filePath);

                for (int i = 0; i < lines.Length; i++)
                {

                    string originalText = RegexExtract(lines[i], regexstr);
                    if (originalText == null||originalText.Length==0)
                        continue;
                    lines[i] = lines[i].Replace(originalText, await TranslateAPI.Translate(originalText, "en", "zh"));

                }

                File.WriteAllLines(filePath, lines);

            }
        }
        public static async void AdConfigTranslate(string filePath,string from,string to) 
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                    int startIndex = lines[i].IndexOf('`') + 2;
                    int blockIndex = lines[i].IndexOf('`');
                    int endIndex = lines[i].LastIndexOf('@');

                    if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
                    {
                        string allIndex = lines[i];
                        string originalText = lines[i].Substring(0, blockIndex);
                    if (originalText == "")
                        continue;
                        string newText = originalText.Replace(originalText, await TranslateAPI.Translate(originalText,from,to));
                        lines[i] = allIndex.Insert(startIndex, newText);
                    }
                
            }

            File.WriteAllLines(filePath, lines);
        }

        public static string RegexExtract(string origin,string regexstr) 
        {
            

            // 创建正则表达式对象
            Regex regex = new Regex(regexstr);

            // 执行匹配
            Match match = regex.Match(origin);

            // 检查是否找到匹配项
            if (match.Success)
            {
                // 提取匹配到的字符串
                string result = match.Groups[1].Value;
                return result;
            }
            else
            {
                return "Error";
            }
        }
    }
}
