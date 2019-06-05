$(function () {

    $('#login').dialog({
        title: '用户登录',      
        width: 300,
        height: 180,
        modal: true,
        iconCls: 'icon-add', 
        buttons: '#loginbtn',
        closable: false,
        draggable: false,
        border: false      
    });
    $('#login').dialog('resize');
    
    //帐号
    $('#user').validatebox({
        required: true,
        missingMessage: '请输入用户帐号',
        invalidMessage: '用户帐号不得为空'
    });
   
    //管理员密码
    $('#password').validatebox({
        required: true,
        validType: 'length[6,30]',
        missingMessage: '请输入用户密码',
        invalidMessage: '用户员密码在6-30 位'
    });

    //加载页面时判断
    if (!$('#user').validatebox('isValid')) {
        $('#user').focus();
    } else if (!$('#password').validatebox('isValid')) {
        $('#password').focus();
    }
    //登录按钮
    $('#loginbtn').unbind('click').bind('click', function () {
        if (!$('#user').validatebox('isValid')) {
            $('#user').focus();
        } else if (!$('#password').validatebox('isValid')) {
            $('#password').focus();
        }
        else {
            //登录数据提交
            $.ajax({
                url: "/login.aspx?&cmd=login",
                type: 'POST',
                data: {
                    user: $('#user').val(),
                    password: $('#password').val()
                },
                timeout: 20000,
                error: function(e) { 
                    $.messager.progress('close');
                } ,
                beforeSend: function () {
                    $.messager.progress({
                        text: '正在登录中'
                    } );
                },
                success: function (data, response, status) {
                    $.messager.progress('close');
                    if (data == 'Y') {
                       // var url = '../mainform/mainform.aspx';
                       location.href = '/mainform/mainform.aspx';
                       // window.open(url, '_self ' )
                    }
                    else if (data == 'N') {
                        alert('提示',"用户在其他地方登录");
                    }
                    else {
                        $.messager.alert('登录失败', '用户名或密码错误', $('#password').select());
                    }
                }                              
            });            
        }
    });
});
