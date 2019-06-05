$(function () {
    //参数获取
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串

        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
    var Request = new Object();
    Request = GetRequest();
    //隐藏域--获取用户名
    // document.getElementById('HiddenField1').value = Request['user_count'];
    //alert(url);

    //$('#iframe1').prop('contentWindow').document.getElementById("PageOfficeCtrl1").Close();
    //alert(Request['url']);
    document.getElementById("PageOfficeCtrl1").WebOpen(Request['url'], "docNormalEdit", "admins");

});