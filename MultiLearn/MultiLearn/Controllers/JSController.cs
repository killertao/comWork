using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MultiLearn.Controllers
{

    public class people
    {

        public string name { get; set; }
        public string sex { get; set; }
        public parents parents { get; set; }
    }

    public class parents
    {
        public string name { get; set; }
        public string sex { get; set; }
        public child Child { get; set; }
    }

    public class child
    {
        public string name { get; set; }
        public string sex { get; set; }
    }
    public class JSController : Controller
    {
        // GET: JS
        public ActionResult Index()
        {
           // button3_Click();
            return View();


        } //上传文件

        public ActionResult UploadFile(HttpPostedFileBase upfile)
        {
            try
            {
                int x = 1;
                upfile.SaveAs(@"E:\Load\x.png");


            }
            catch (Exception ex)
            {

                return Json(new {msg = ex.Message});

            }
            return Json(new {msg = true});

        }

        private void button3_Click()
        {
            Regex r = new Regex(@"/((?>[^()]+|/((?<DEPTH>)|/)(?<-DEPTH>))*(?(DEPTH)(?!))/)");
            string OutString = "（ore (nope (yes (here) okay) after)";
            MatchCollection ms = r.Matches(OutString); // 获取所有的匹配
            //StringBuilder sb = new StringBuilder();
            //MatchString("(111[222[333]]][222[333]](333))", r, sb);
            //MessageBox.Show(sb.ToString(), "取到的信息");

        }

        private void MatchString(string OutString, Regex r, StringBuilder sb)
        {
           
            //foreach (Match m in ms)
            //{
            //    if (m.Success)
            //    {
            //        sb.AppendLine(m.Groups[0].Value);
            //        MatchString(m.Groups[0].Value.Substring(1, m.Groups[0].Value.Length - 1), r,
            //            sb); // 去掉匹配到的头和尾的 "[" 和 "]"，避免陷入死循环递归中，导致溢出
            //    }
            
          
        }

    }

}