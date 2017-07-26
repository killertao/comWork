//var uiprogess = new UploadProgess();

//function fileupload() {
//    var file = document.querySelector(".tarFile").files[0];
//    var task = new uiprogess.CreatePorgess(file.name);
//    var upfile = new UploadFile();
//    upfile.progressBack = task.changeProgess;
//    upfile.success = function (serverdata) {
//        task.successState();
//    }
//    upfile.error = function (filename) {
//        task.errorState.call();
//        alert(filename + "上传失败");
//    }
//    upfile.upload("/JS/UploadFile", file);
//} 示例代码



//文件上传构造函数
function UploadFile() {
    var self = this;

    function progress(evt) {
        var persent = Math.round(evt.loaded * 100 / evt.total);
        self.progressBack(persent);
    }

    //文件上传方法
    this.constructor.prototype.upload = function (url, file,data) {
        var xhr = new XMLHttpRequest();
        xhr.upload.addEventListener("progress", progress, false);
        //xhr.addEventListener("load", uploadComplete, false);
        //xhr.addEventListener("error", uploadFailed, false);
        //xhr.addEventListener("abort", uploadCanceled, false);
        xhr.open("post", url);
        xhr.onreadystatechange = function () {

            if (xhr.readyState == 4) {
                if (xhr.status == 200) {
                    var serverdata = JSON.parse(xhr.responseText);
                    self.success(serverdata);
                } else {
                    self.error(file.name);
                }
            }

        }
        xhr.send(data);

    }

    //文件上传回调方法
    this.constructor.prototype.complete = function () {

    };
    this.constructor.prototype.fail = function () {

    };
    this.constructor.prototype.cancel = function () {

    }
    this.constructor.prototype.progressBack = function () {

    }
    //文件上传的ajax的回调方法
    this.constructor.prototype.success = function () {

    }
    this.constructor.prototype.error = function () {
    }




}


//右下角ui 构造函数
function UploadProgess() {
    this.constructor.prototype.init = function () {
        if (!document.querySelector("#UploadProgess")) {
            var container = document.createElement("div");
            with (container) {
                id = "UploadProgess",
                    className = "pg-container"
            };
            document.body.appendChild(container);
        }
    }
    this.constructor.prototype.CreatePorgess = function uiprogess(filename) {
        //初始化一个任务ui
        var x = document.getElementById("UploadProgess");
        var container = document.querySelector("#UploadProgess");
        if (!container) {
            this.init();
        }
        var task = document.createElement("div");
        task.className = "pg-task";
        task.innerHTML = '<div class="task-persent">0%</div><div class="task-bar"><span class="task-progess"></span></div> <span class="task-name">' + filename + '</span>';
        container.appendChild(task);
        //创建一个任务
        this.constructor.prototype.changeProgess = function (persent) {
            var p = task.querySelector(".task-progess");
            var n = task.querySelector(".task-persent");
            n.innerHTML = p.style.width = persent + "%";

        }
        //上传失败修改ui
        this.constructor.prototype.errorState = function () {
            var p = task.querySelector(".task-progess");
            var n = task.querySelector(".task-persent");
            p.style.backgroundColor = "red";
            n.innerHTML = "上传失败";
            removeTask();
        }
        //上传成之后修改ui
        this.constructor.prototype.successState = function () {
            // var p = task.querySelector(".task-progess");
            var n = task.querySelector(".task-persent");
            n.innerHTML = "上传成功...";
            removeTask();


        }
        function removeTask(seconds) {
            setTimeout(function () {
                    container.removeChild(task);
                },
                seconds || 3000);
        }
    }
    this.init();
}
