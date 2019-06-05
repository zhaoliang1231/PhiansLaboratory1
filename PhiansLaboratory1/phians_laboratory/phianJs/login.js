
$(function () {
    //you don't need this, just used for changing background 改变背景
    $('#btn-login-dark').on('click', function (e) {
        $('body').attr('class', 'login-layout');
        $('#id-text2').attr('class', 'white');
        $('#id-company-text').attr('class', 'blue');

        e.preventDefault();
    });
    $('#btn-login-light').on('click', function (e) {
        $('body').attr('class', 'login-layout light-login');
        $('#id-text2').attr('class', 'grey');
        $('#id-company-text').attr('class', 'blue');

        e.preventDefault();
    });
    $('#btn-login-blur').on('click', function (e) {
        $('body').attr('class', 'login-layout blur-login');
        $('#id-text2').attr('class', 'white');
        $('#id-company-text').attr('class', 'light-blue');

        e.preventDefault();
    });
    //获取屏幕宽度让登录框居中
    var _height = screen.availHeight;
    $(".login-container").css("marginTop", _height / 8 + "px");
   
    //登录按钮***********************************登录后台系统**************************************//
    $('#login').unbind('click').bind('click', function () {
        if ($("#User_count").val() == "") {
            layer.msg('用户名不能为空！');
            $('#User_count').focus();
            return;
        }
        if ($("#User_pwd").val() == "") {
            layer.msg('密码不能为空！');
            $('#User_pwd').focus();
            return;
        }
        //登录数据提交
        $.ajax({
            url: "/Login/login",
            type: 'POST',
            data: {
                UserCount:encode( $('#User_count').val()),
                UserPwd: encode($('#User_pwd').val())
            },
            timeout: 20000,
            error: function (e) {
                layer.alert(e);
                layer.closeAll();
            },
            beforeSend: function () {
                //进度条
                layer.load(1, {
                    shade: [0.1, '#000'] //0.1透明度的白色背景
                });
            },
            success: function (data, response, status) {
                layer.closeAll();
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    location.href = '/mainform';
                }
                else if (result.Success == false) {
                    layer.alert(result.Message, { icon: 2 });
                  
                }
                
            }
        });
    });
});


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
function _utf8_decode(utftext)
{
    var _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    var string = "";
    var i = 0;
    var c = c1 = c2 = 0;
    while (i < utftext.length)
    {
        c = utftext.charCodeAt(i);
        if (c < 128)
        {
            string += String.fromCharCode(c);
            i++;
        }
        else if ((c > 191) && (c < 224))
        {
            c2 = utftext.charCodeAt(i + 1);
            string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
            i += 2;
        }
        else
        {
            c2 = utftext.charCodeAt(i + 1);
            c3 = utftext.charCodeAt(i + 2);
            string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
            i += 3;
        }
    }
    return string;
}