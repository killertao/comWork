using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cooshu.Spider.Core;

namespace Cooshu.Spider.WebFrame
{
    public class FaXinFrame : SiteFrame
    {
        public override string SpiderTaskSchedulerName { get; protected set; } = "FaXinScheduler";

        public override void Start(int threadCount)
        {
            if (Running)
            {
                return;
            }

            var homeTask = SpiderTask.Create(CreateFrame(), "");//
            SchedulerInstance.AddTask(homeTask);
            SchedulerInstance.Start(threadCount);
            Running = true;
        }



        private Page CreateFrame()
        {

            //详情页
         //   var detail = CreateDetailPage(this);

            //下一页
            var nextList = CreateNextPage(this); 

            //列表页
            var listPage = new Page(this)
            {
                Name = "国家法律列表",
                Pages = new List<Page> { nextList},
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    // spiderTask.Data["url"] = "http://www.faxin.cn/lib/zyfl/ZyflLibrary.aspx?libid=010101";
                    spiderTask.Url = "http://www.faxin.cn/lib/zyfl/GetZyflData.ashx";
                    spiderTask.PostData = new Dictionary<string, string>
                    {
                        { "searchtype","0"},
                        { "lib","zyfl"},
                        { "chooseNum","010101"},
                        { "firstPage","1"},
                        { "secondPage","1"},
                        { "thirdPage","1"},
                        { "fourthPage","1"},
                        { "fifthPage","1"},
                        { "sixthPage","1"},
                        { "listnum","10"},
                    };
            
                    //"xiaoli_id=02,0201,0202,0203,0204,0205,0206lib=zyfl&chooseNum=010101&firstPage=1&secondPage=1&thirdPage=1&fourthPage=1&fifthPage=1&sixthPage=1listnum=10
                },

                HtmlLoadedHandle = ts =>
                {
                 
                    //  ts.ResponseData
                }
            };
            return listPage;
        }

        Page CreateNextPage(SiteFrame sf)
        {
           return new Page(sf)
            {
                Name = "下一页",
                Pages = new List<Page> { CreateNextPage(sf)},
                TaskCreatedHandle = (spiderTask, parentSpiderTask) =>
                {
                    spiderTask.PostData = parentSpiderTask.PostData;
                    spiderTask.PostData["firstPage"] = spiderTask.PostData["secondPage"] =
                        spiderTask.PostData["thirdPage"] = spiderTask.PostData["fourthPage"] = spiderTask.PostData["fifthPage"] = spiderTask.PostData["sixthPage"]=(Convert.ToInt32( parentSpiderTask.PostData["sixthPage"])+1).ToString();
                },
                HtmlLoadedHandle = ts =>
                {
                    //
                      ts.Page.Pages.Add(CreateDetailPage(sf));          
                }
            };
        }

        Page CreateDetailPage(SiteFrame sf)
        {
          return  new Page(sf)
            {
                Name = "法律详细页",
                HtmlLoadedHandle = ts =>
                {//这里面插入数据

                }
            };
        }
    }
}
