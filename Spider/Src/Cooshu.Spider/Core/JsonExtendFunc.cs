using Newtonsoft.Json;

namespace Cooshu.Spider.Core
{
    /// <summary>
    /// Json序列化扩展方法
    /// </summary>
    public static class JsonExtendFunc
    {
        /// <summary>
        /// 反序列化成对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="str">序列化的字符串</param>
        /// <returns>目标对象</returns>
        public static T Object<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 反序列化成对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="str">序列化的字符串</param>
        /// <param name="defaultValue">当反序列化结果为空时的值</param>
        /// <returns>目标对象</returns>
        public static T Object<T>(this string str, T defaultValue)
        {
            var result = JsonConvert.DeserializeObject<T>(str);

            if (result == null)
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// 序列化成Json字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>序列化的字符串</returns>
        public static string Json<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
