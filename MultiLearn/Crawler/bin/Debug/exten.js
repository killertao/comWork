var isFirst = false;
//加入数据
$(document).ajaxComplete(function (event, xhr, settings) {
    if (settings.url === "/List/ListContent") {
        //var docIds = [];
        //alert(text);
        //ext.replace(/"文书ID":"([a-z0-9-]{36})/g, function (a, b) { docIds.push(b) });
        //lert(docIds.join("1"));
       
        if (ltPageIndex > 0) {

            var text = xhr.responseText.toString();
            window.external.AddDocId(text,ltPageIndex);
        }
    }
    if (settings.url === "/List/GetAllRelateFiles") {
        //表示已经页面数据已经加载完了
        //继续下一页
        if (!isFirst) {
            var isCondition = $("#condtionText .condtionText").eq(0).html();
            var b = isCondition ? isCondition.indexOf("上传日期") >= 0 : false;
            window.external.Condition(b);
            isFirst = true;
        }
        ltPageIndex++;
        //当超出限制的时候不在抓抓取
        if (ltPageIndex > 100) {
            window.external.Success();
            return;
        }
        nextPage(ltPageIndex);
    }
    ///List/GetDicValue
    //VM163: 3 / List / ListContent
    //VM163: 3 / List / GetAllRelateFiles
});


//在验证或者报错的时候，根据url重新跳到所需要的页数
var s = window.location.href.match(/&PageIndex=(\d+)/);
var ltPageIndex = s ? s[1] : 0;
//劫持ajax改变参数
var _ajax = $.ajax;
$.ajax = function () {
    var b = arguments[0] && arguments[0].url;
    if (b && arguments[0].url.indexOf("/List/ListContent") === 0) {
        //劫持改变页数参数
        arguments[0].data.Index = ltPageIndex;
        arguments[0].data.Page = 20;
    }
    if (b && arguments[0].url.indexOf("/List/ValiYzm") === 0 && $("#txthidtype").val() == 3) {
       //1. 获取guid  赋值给  $("#txthidGuid").val()
       //2.改变 data 的参数     data: { "number": $("#txtValidateCode").val(), "valiGuid": valiguid }, 
        //===========================
}
_ajax.apply(this, arguments);
}

function nextPage(index) {
    $("#pageNumber a").eq(0).click();
    var yzm = $("#txtValidateCode:visible");
    if (yzm) {
        yzm.focus();
        yzm.val("abcd");
        $("#btn_yzmsure").click();//点击无效的话，可以模拟enter或者是直接 写一个请求下一页
        
    }
}

