
$(function () {
   
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
    if (Request['state']==-1) {

        $.messager.alert("提示", "session到期，请重新登录系统");
    }

    $.supersized({

        // 功能
        slide_interval: 4000,    // 转换之间的长度
        transition: 1,    // 0 - 无，1 - 淡入淡出，2 - 滑动顶，3 - 滑动向右，4 - 滑底，5 - 滑块向左，6 - 旋转木马右键，7 - 左旋转木马
        transition_speed: 1000,    // 转型速度
        performance: 1,    // 0 - 正常，1 - 混合速度/质量，2 - 更优的图像质量，三优的转换速度//（仅适用于火狐/ IE浏览器，而不是Webkit的）

        // 大小和位置
        min_width: 0,    // 最小允许宽度（以像素为单位）
        min_height: 0,    // 最小允许高度（以像素为单位）
        vertical_center: 1,    // 垂直居中背景
        horizontal_center: 1,    // 水平中心的背景
        fit_always: 0,    // 图像绝不会超过浏览器的宽度或高度（忽略分钟。尺寸）
        fit_portrait: 1,    // 纵向图像将不超过浏览器高度
        fit_landscape: 0,    // 景观的图像将不超过宽度的浏览器

        // 组件
        slide_links: 'blank',    // 个别环节为每张幻灯片（选项：假的，'民'，'名'，'空'）
        slides: [    // 幻灯片影像
                                 { image: '/image/login_2.jpg' },
                                 { image: '/image/login.jpg' }
                                
        ]

    });

  
    $('#login').unbind('click').bind('click', function () {
        
        var phians_name = encode(document.getElementById("phians_name").value);
        //alert(encode(phians_name));
        
        var phians_pwd =encode( document.getElementById("phians_pwd").value);
        if (phians_name == "") {
            $('#phians_name').focus();
        } else
            if (phians_pwd == "") {
                $('#phians_pwd').focus();
            }
            else {
                //登录数据提交
                $.ajax({
                    url: "login.aspx?state=1",
                    type: 'POST',
                    data: {
                        cmd:"login",
                        phians_name1: phians_name,
                        phians_pwd1: phians_pwd
                    },
                    timeout: 3000,
                    error: function (e) {
                        $.messager.progress('close');
                        $.messager.alert("登录失败", "登陆超时，请检查网络是否连接");
                    },
                    beforeSend: function () {

                        $.messager.progress({


                            text: "正在登录中"

                        }
                        );

                    },

                    success: function (data, response, status) {
                      
                        setTimeout("close_progress", 3000);
                    
                        if (data == 'Y') {
                            // var url = '../mainform/mainform.aspx';
                            window.location.href = '/mainform/mainform.aspx';
                            // window.open(url, '_self ' )
                        }
                        else if (data == 'V') {

                            $.messager.alert("登录失败", "系统已失效！");
                            location.href = '/mainform/errorPage.html';
                        }
                        else {
                            $.messager.progress('close');
                            //$.messager.alert("登录失败", "用户名或密码错误", $('#password').select());
                            $.messager.alert("登录失败", data);

                        }

                    }

                });


            }

    });
   

});

function close_progress() {
    $.messager.progress('close');

}

// public method for encoding
function encode(input) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    var output = "";
    var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
    var i = 0;
    input = _utf8_encode(input);
    while (i < input.length) {
        chr1 = input.charCodeAt(i++);
        chr2 = input.charCodeAt(i++);
        chr3 = input.charCodeAt(i++);
        enc1 = chr1 >> 2;
        enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
        enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
        enc4 = chr3 & 63;
        if (isNaN(chr2)) {
            enc3 = enc4 = 64;
        } else if (isNaN(chr3)) {
            enc4 = 64;
        }
        output = output +
        _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
        _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
    }
    return output;
}

// public method for decoding
function decode(input) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    var output = "";
    var chr1, chr2, chr3;
    var enc1, enc2, enc3, enc4;
    var i = 0;
    input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
    while (i < input.length) {
        enc1 = _keyStr.indexOf(input.charAt(i++));
        enc2 = _keyStr.indexOf(input.charAt(i++));
        enc3 = _keyStr.indexOf(input.charAt(i++));
        enc4 = _keyStr.indexOf(input.charAt(i++));
        chr1 = (enc1 << 2) | (enc2 >> 4);
        chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
        chr3 = ((enc3 & 3) << 6) | enc4;
        output = output + String.fromCharCode(chr1);
        if (enc3 != 64) {
            output = output + String.fromCharCode(chr2);
        }
        if (enc4 != 64) {
            output = output + String.fromCharCode(chr3);
        }
    }
    output = _utf8_decode(output);
    return output;
}

// private method for UTF-8 encoding
function _utf8_encode(string) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    string = string.replace(/\r\n/g, "\n");
    var utftext = "";
    for (var n = 0; n < string.length; n++) {
        var c = string.charCodeAt(n);
        if (c < 128) {
            utftext += String.fromCharCode(c);
        } else if ((c > 127) && (c < 2048)) {
            utftext += String.fromCharCode((c >> 6) | 192);
            utftext += String.fromCharCode((c & 63) | 128);
        } else {
            utftext += String.fromCharCode((c >> 12) | 224);
            utftext += String.fromCharCode(((c >> 6) & 63) | 128);
            utftext += String.fromCharCode((c & 63) | 128);
        }

    }
    return utftext;
}

// private method for UTF-8 decoding
function _utf8_decode(utftext) {
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    var string = "";
    var i = 0;
    var c = c1 = c2 = 0;
    while (i < utftext.length) {
        c = utftext.charCodeAt(i);
        if (c < 128) {
            string += String.fromCharCode(c);
            i++;
        } else if ((c > 191) && (c < 224)) {
            c2 = utftext.charCodeAt(i + 1);
            string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
            i += 2;
        } else {
            c2 = utftext.charCodeAt(i + 1);
            c3 = utftext.charCodeAt(i + 2);
            string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
            i += 3;
        }
    }
    return string;
}