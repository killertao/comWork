using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Crawler.Model;
using  static  Crawler.Helper.SqlHelper;
namespace Crawler.Helper
{
    public class LogicalDB
    {
        private static readonly CusDbContent Db = new CusDbContent();
        //获取抓取那一天的总任务
        public static DateTime GetDate()
        {
            var dates = Db.Set<SearchedDate>();
            var maxdate = dates.Max(d => d.SDate);
            return maxdate == null ? DateTime.Now : DateTime.Now.AddDays(-1);

        }
        public static void AddDocId(List<string> docs)
        {
            var s = docs.Cast<DocIds>();
            Db.Set<DocIds>().AddRange(s);
            Db.SaveChanges();
        }

        public static List<SearchContent> GetPreAssgin()
        {
            SqlHelper.ExecuteNonQuery(GetConnection(), CommandType.Text, "update SearchContent set S");
            //这里只获取100个任务 等执行完毕再来
            var s= Db.Set<SearchContent>().Where(sc=>sc.Status==0).Take(100);
            //将这些状态设置为正则执行 -1
           
            return  new List<SearchContent>();
        }


        public static List<ChildPre> GetPreChild(int count)
        {

            return new List<ChildPre>();
        }


        public static bool ChangePreChid(string url)
        {
            return true;
        }



    }
}
