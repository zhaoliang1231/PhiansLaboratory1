$(function () {

    $('#mainbox').layout({
        fit: true
    });
    //初始化菜单栏
    InitLeftMenu();
   
    tabClose();
    //时钟
    clockon();

    //初始化操作下拉框
    init_operate_click();
    //初始化消息下拉框
    init_message_click();
    //初始化消息插件
    init_message_Hub();
   
    //测试发送消息
    //$('#test_message').click(function () {

    //    document.getElementById('send_message').click();
    //});
 
    
    var timer2 = setTimeout("check_session()", 180000);
  
})


function check_session() {
   
   
        $.ajax({
            type: 'POST',
            dataType: "text",
            url: "/mainform/mainform.aspx?cmd=check_session",
         
            complete: function (data) {
                // Console.log(data)

                var obj = eval(data);                    
                    if (obj.responseText != "T") {
                        b = 1;
                        //  $('.panel-tool-close').hide();
                        
                        $.messager.confirm('操作提示', '系统登录时间到期;请重新登录', function (r) {
                            if (r) {
                                window.location.href = '/login/login.aspx';
                            }
                            else { window.location.href = '/login/login.aspx'; }
                        });
                    }
                    if (b == "0") {                                                                                       
                      
                        var timer2 = setTimeout("check_session()", 180000);
                        //var timer = setTimeout("check_session()", 12000);
                    }
            }
        });
     
      
       
       
        
}
var b = "0";
var ii = 0;
function hh() {
    if(b=="0"){
    ii = ii + 1;
   // console.log(ii);
    if (ii == "24") {
        ii = 0;
        check_session();
    }
 
    var timer2 = setTimeout("hh()", 180000);
    }
}

function tiao() {

    $('#check_session').form('submit', {
        url: "/mainform/mainform.aspx",
        ajax: true,
        onSubmit: function (param) {
            param.cmd = 'check_session';
        },
        success: function (data) {
        
        }
    });

}
    //function al() {
//    alert(1);
//}

//初始化操作下拉框
function init_operate_click() {

    $('#operate_click').menubutton({
        iconCls: 'icon-config',
        menu: '#menu_show',
        width: 100,
        hasDownArrow:true,
        title: "系统操作"
    });
    //退出登录
    $('#Sign_out').unbind('click').bind('click', function () {

        document.getElementById('Button1').click();
    })
    //修改密码
    $('#change_password').unbind('click').bind('click', function () {
        $('#old_password').textbox('setText', "");
        $('#new_password').textbox('setText', "");
        $('#R_new_password').textbox('setText', "");
        $('#password_dialog').dialog({
            title: '密码修改',
            width: 400,
            height: 200,
            closed: false,
            cache: false,
            modal: true,
            buttons: [{
                text: '确认修改',
                handler: function () {
                    var phians_old_pwd = $('#old_password').textbox('getText');
                    var new_password = $('#new_password').textbox('getText');
                    var R_new_password = $('#R_new_password').textbox('getText');
                    if (phians_old_pwd == "") {
                        $('#old_password').focus();
                        $.messager.alert("提示", '请输入原密码');


                    }

                    else if (phians_old_pwd != "" && (new_password == R_new_password) && new_password != "" && R_new_password != "") {
                        phians_old_pwd = encode(phians_old_pwd);
                        new_password = encode(new_password);
                        $.ajax({
                            url: "/mainform/mainform.aspx?&cmd=change_password",
                            type: 'POST',
                            data: {
                                phians_old_pwd: phians_old_pwd,
                                new_password: new_password
                            },
                            timeout: 3000,

                            success: function (data, response, status) {


                                if (data == 'T') {
                                    $.messager.alert("提示", '密码修改成功');
                                    $('#password_dialog').dialog('close');
                                }
                                else if (data == '原密码错误') {
                                    $.messager.alert("提示", '原密码错误，请确认再次输入');

                                }

                            }

                        });


                    }
                    else if (new_password == "" && R_new_password == "") {

                        $.messager.alert("提示", '请输入新密码');
                    }
                    else if ((new_password != R_new_password) && phians_old_pwd != "") {

                        $.messager.alert("提示", '两次密码一致');


                    }


                }
            }, {
                text: '取消修改',
                handler: function () {
                    $('#password_dialog').dialog('close');
                }
            }
            ]
        })

    });
}
//初始化消息下拉框
function init_message_click() {

    $('#message_click').menubutton({
        iconCls: 'icon-message',
        //listWidth: 300,
        noline: false,
        menu: '#message_menu',
        width: 120,
       
        title: "消息查看"
    });
 

}

//初始化左侧
function InitLeftMenu() {
    $("#m_menu").empty();
    $.ajax({
        type: 'POST',
        dataType: "json",
        url: "/mainform/mainform.aspx?&cmd=load_m_menu",
        success: function (data) {
            //转成json对象
            var obj = eval(data);
            //读取json对象数据
            //alert(obj.rows[0].username);

            //获取二级菜单
            var menulist = "";

            for (var i = 0; i < obj.total; i++) {
                if (obj.rows[i].m_number != '') {
                    var menulist = "";

                    menulist += '<ul>';
                    for (var j = 0; j < obj.total; j++) {
                        if (obj.rows[i].group_id == obj.rows[j].group_id && obj.rows[j].m_number == '') {
                            menulist += '<li><div><a  style="line-height:30px;text-decoration: none; " target="mainFrame" way="' + obj.rows[j].u_url + '" ><span class="icon ' + obj.rows[j].iconCls + '" ></span>' + obj.rows[j].m_name + '</a></div></li> ';
                        }
                    }
                    menulist += '</ul>';
                    $('#m_menu').accordion('add', {
                        title: obj.rows[i].m_name,
                        content: menulist,
                        iconCls: obj.rows[i].i_iconCls
                      
                    });
                }
              
                //加载完数据后调用布局重置，防止界面变形
                $('#mainbox').layout('resize');
                
            }
         
       
            //菜单栏点击事件
            $('.easyui-accordion li a').unbind('click').bind('click', function () {
                var tabTitle = $(this).text();
                var url = $(this).attr("way");
                addTab(tabTitle, url);
                $('.easyui-accordion  li div ').removeClass("selected");
                $(this).parent().addClass("selected");
            }).hover(function () {
                $(this).parent().addClass("hover");
            },
            function () {
                $(this).parent().removeClass("hover");
            });
          
           // $("#m_menu").accordion();

        }
    });
    //默认选择打开报告管理
    $('#m_menu').accordion({
        select: 2
    });

}
//未读消息
function addTab1() {
    var source = $("#read").attr("value");
    addTab("未读消息", source);
}
//待创建入库委托任务
function addTab2() {
    var source = $("#open_Warehousing_commissioned").attr("value");
    addTab("入库委托", source);
}
//待审核委托任务
function addTab3() {
    var source = $("#open_Test_commissioned_review").attr("value");
    addTab("试验委托评审", source);
}
//待接收样品
function addTab4() {
    var source = $("#open_Sample_accept").attr("value");
    addTab("样品接收", source);
}
//待领取任务
function addTab5() {
    var source = $("#open_Task_pool").attr("value");
    addTab("任务池", source);
}
//待检测任务
function addTab6() {
    var source = $("#open_Test_record").attr("value");
    addTab("检测记录", source);
}
//待记录审核
function addTab7() {
    var source = $("#open_Record_collate").attr("value");
    addTab("记录审核", source);
}
//待编制报告
function addTab8() {
    var source = $("#open_Report_Edit").attr("value");
    addTab("报告编制", source);
}
//待审核报告
function addTab9() {
    var source = $("#open_Report_Review").attr("value");
    addTab("报告审核", source);
}
//待签发报告
function addTab10() {
    var source = $("#open_Report_Issue").attr("value");
    addTab("报告批准", source);
}
//待异常报告申请审核
function addTab11() {
    var source = $("#open_Error_Report_Apply_Audit").attr("value");
    addTab("异常报告申请审核", source);
}
//待异常报告编辑申请编辑
function addTab12() {
    var source = $("#open_Error_Report_EditApply_Edit").attr("value");
    addTab("异常报告编辑", source);
}

//待异常报告编辑申请审核
function addTab13() {
    var source = $("#open_Error_Report_EditApply_Audit").attr("value");
    addTab("异常报告审核", source);
}
//试验标准审核
function addTab14() {
    var source = $("#open_Test_standard_review").attr("value");
    addTab("机加任务指派", source);
}
//机加任务待指派
function addTab15() {
    var source = $("#open_Task_appoint_machining").attr("value");
    addTab("机加任务待指派", source);
}
//试样机加
function addTab16() {
    var source = $("#open_Sample_machining").attr("value");
    addTab("试样机加", source);
}
function addTab(subtitle, url) {
    //判断界面是否打开
    if (!$('#tabs').tabs('exists', subtitle)) {
        $('#tabs').tabs('add', {
            title: subtitle,
            content: createFrame(url),
            closable: true,
            border: false,
            fit: true

        });

    } else {
        //如果该界面已经打开，则直接显示
        $('#tabs').tabs('select', subtitle);

    }
    tabClose();
}

function createFrame(url) {
    var s = '<iframe id="mainFrame_from" name="mainFrame" scrolling="ture" frameborder="0"  src="' + url + '"style="padding:0px;display:block;width:99.9%;height:99.9%;" ></iframe>';
    return s;

}

function tabClose() {
    /*双击关闭TAB选项卡*/
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children("span").text();
        //如果是起始页则不能关闭
        if (subtitle != "起始页") {
            $('#tabs').tabs('close', subtitle);
        }

    })


}

//页面显示当前登录用户和动态时间
function clockon() {
    var now = new Date();
    var year = now.getFullYear(); //getFullYear getYear
    var month = now.getMonth();
    var date = now.getDate();
    var day = now.getDay();
    var hour = now.getHours();
    var minu = now.getMinutes();
    var sec = now.getSeconds();
    var week;
    month = month + 1;
    if (month < 10) month = "0" + month;
    if (date < 10) date = "0" + date;
    if (hour < 10) hour = "0" + hour;
    if (minu < 10) minu = "0" + minu;
    if (sec < 10) sec = "0" + sec;
    var arr_week = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六");
    week = arr_week[day];
    var time = "";
    time = year + "年" + month + "月" + date + "日" + " " + hour + ":" + minu + ":" + sec + " " + week;

    $("#m_showtime").html(time);

    var timer = setTimeout("clockon()", 200);
}
function init_message_Hub() {

          
        $.connection.hub.logging = true;
    
        var chat = $.connection.message_Hub;
        chat.client.broadcastMessage = function (message) {

            var mes = message.split("**")
            $.messager.show({
                title: "消息通知",
                msg: "消息内容：" + mes[0] + "<br>" + "消息时间：" + mes[1],
                height: 300,
                width: 300,
                timeout: 8000,
                showType: 'slide'
            });
        };
        chat.client.showMessage = function (message) {
            $.messager.show({
                title: "消息通知",
                msg: "消息内容：" + message,
                height: 300,
                width: 300,
                timeout: 8000,
                showType: 'slide'
            });
        };
        //开始服务
        $.connection.hub.start().done(function () {
               
        });
   



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