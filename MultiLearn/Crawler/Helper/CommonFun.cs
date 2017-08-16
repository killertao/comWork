using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Helper
{
    /// <summary>
    /// 公共方法类
    /// </summary>
   public class CommonFun
    {
        public string CreateGuid()
        {
            //function createGuid()
            //{
            //    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            //}
            string str = "";
            Random rd = new Random();
            for (int i = 0; i < 4; i++)
            {
                double s = (1 + (rd.Next(10000000)) / 10000000.0) * 0x10000;
                str += ((int)Math.Round(s)).ToString("X").Substring(1) + "-";
            }
            return str.TrimEnd('-');
        }

    }
}
