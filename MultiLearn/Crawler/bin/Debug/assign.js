//加入数据
var isFirst=false;
$(document).ajaxComplete(function (event, xhr, settings) {
    if (isFirst) {
        return;
    }
    if (settings.url === "/List/GetAllRelateFiles") {
       
        var isCondition = $("#condtionText .condtionText").eq(0).html();
        var b = isCondition ? isCondition.indexOf("上传日期") >= 0 : false;
        window.external.Assign(b);
    }
});
