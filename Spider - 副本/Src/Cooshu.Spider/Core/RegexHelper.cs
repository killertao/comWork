using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Cooshu.Spider.Core
{
    /// <summary>
    /// 正则表达式帮助类
    /// </summary>
    public static class RegexHelper
    {
        /// <summary>
        /// 获得匹配的数据字典列表
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static List<Dictionary<string, string>> GetList(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
            {
                return new List<Dictionary<string, string>>();
            }

            var matchedGroups = Regex.Matches(text, pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
            var results = new List<Dictionary<string, string>>();
            foreach (Match item in matchedGroups)
            {
                results.Add(GetGroupName(item).ToDictionary(a => a, a => item.Groups[(string) a].Value));
            }
            return results;
        }

        /// <summary>
        /// 获得匹配的数据字典
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(string text, string pattern)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
            {
                return new Dictionary<string, string>();
            }

            var matched = Regex.Match(text, pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
            if (matched.Success)
            {
                return GetGroupName(matched).ToDictionary(a => a, a => matched.Groups[a].Value);
            }

            return new Dictionary<string, string>();
        }


        public static List<Match> GetListMatch(string input,Regex reg)
        {
            List<Match> results = new List<Match>();
            var matchedGroups = reg.Matches(input);
            foreach (Match item in matchedGroups)
            {
                results.Add(item);
            }
            return results;
        }

        /// <summary>
        /// 获得匹配结果的所有分组名称，分组名称不能是纯数字
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetGroupName(Match match)
        {
            if (match == null)
            {
                return new string[0];
            }

            int temp;
            var regex = RegexFieldInfo?.GetValue(match) as Regex;
            return regex?.GetGroupNames().Where(a => !int.TryParse(a, out temp));
        }

        private static readonly FieldInfo RegexFieldInfo = typeof(Match).GetField("_regex", BindingFlags.NonPublic | BindingFlags.Instance);


        
    }
}