var line = 0;
$(function () {
    search_message();//搜索信息的下拉框
    MessageList();//消息列表初始化

    //搜索下拉框
    $('#search_key').combobox({
        editable: false,
        data: [
           { 'value': 'UserName', 'text': '用户名' },
            { 'value': 'MessageType', 'text': '消息类型' },
            { 'value': 'Message', 'text': '消息内容' }
        ]
    });
    //搜索信息
    $('#search').unbind('click').bind('click', function () {
        search();
    });
    //确认收到消息
    $('#click_ok').unbind('click').bind('click', function () {
        click_ok();
    });
    //确认全部消息
    $('#click_all_ok').unbind('click').bind('click', function () {
        click_all_ok();
    });
});

//获取消息
function MessageList() {
    $('#MessageList').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        border: false,
        fitColumns: true,
        resizable: false,
        fit: true,
        pagination: true,
        pageSize: 25,
        pageList: [25, 50, 75, 100],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/PersonalCenter/GetUnReadMessage",//接收一般处理程序返回来的json数据 
        queryParams: {
            flag: $('#search_message').combobox('getValue'), //消息状态的标识
            search: $('#search_key').combobox('getValue'), //搜索类型
            key: $('#search_key1').textbox("getText")  //搜索字段
        },
        columns: [[
         { field: 'UserName', title: '用户名', sortable: 'true', width: 50 },
        { field: 'MessageType', title: '消息类型', sortable: 'true', width: 50 },
        { field: 'Message', title: '消息内容', width: 100 },
        {
            field: 'CreateTime', title: '消息创建时间', width: 100, formatter: function (value, row, index) {//创建时间
                if (value) {//格式化时间
                    if (value.length >= 19) {
                        value = value.substr(0, 19)
                        return value;
                    }
                }
            }
        },
        {
            field: 'ConfirmTime', title: '消息确认时间', width: 100, formatter: function (value, row, index) {//确认时间
                if (value) {//格式化时间
                    if (value.length >= 19) {
                        value = value.substr(0, 19)
                        return value;
                    }
                }
            }
        },
        { field: 'PushPersonname', title: '消息发送人', width: 50 }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#MessageList').datagrid('selectRow', line);
        },
        sortable: 'asc',
        toolbar: "#message_show_toolbar"
    });
};

//搜索信息的下拉框
function search_message() {
    $('#search_message').combobox({
        value: 0,
        editable: false,
        data: [
               { 'value': 0, 'text': '未读消息' },
               { 'value': 1, 'text': '已读消息' }
        ],
        onSelect: function () {
            var search = $('#search_message').combobox('getValue');
            if (search == "0") {
                $(".link_button").css("display", "inline-block");
            }
            else if (search == "1") {
                $(".link_button").css("display", "none");
            }
            $('#MessageList').datagrid({
                type: 'POST',
                dataType: "json",
                url: "/PersonalCenter/GetUnReadMessage",//接收一般处理程序返回来的json数据                
                queryParams: {
                    flag: $('#search_message').combobox('getValue'), //消息状态的标识
                    key: $('#search_key').combobox('getValue'), //搜索类型
                    key1: $('#search_key1').val()  //搜索字段
                }
            });
        }
    });
};

//确认收到消息
function click_ok(){
    var message_show_Row = $("#MessageList").datagrid("getSelected");
    if (message_show_Row) {
        $.messager.confirm('提示', '是否确认已经阅读该消息？', function (r) {
            if (r) {
                var selectRow = $("#MessageList").datagrid('getSelections');
                var sum = "";
                for (var i = 0; i < selectRow.length; i++) {
                    if (i == 0) {
                        sum = selectRow[i].id;
                    }
                    if (i > 0) {
                        sum = sum + "," + selectRow[i].id;
                    }
                }
                $.ajax({
                    url: "/PersonalCenter/EditMessage",
                    type: 'POST',
                    data: {
                        ids: sum
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $("#MessageList").datagrid("reload");
                        } 
                        else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        });
    } else {
        $.messager.alert( "提示", "请先选中一条消息！")
    }
};

//查询信息
function search(){
    var flag = $('#search_message').combobox('getValue');
    var search = $('#search_key').combobox('getValue');
    var key = $('#search_key1').textbox("getText");

    $('#MessageList').datagrid({
        type: 'POST',
        dataType: "json",
        url: "/PersonalCenter/GetUnReadMessage",//接收一般处理程序返回来的json数据                
        queryParams: {
            search: search,
            flag: flag,
            key: key,
        }
    });
};

//确认全部消息
function click_all_ok(){
    $.messager.confirm('提示', '是否确认阅读全部消息？', function (r) {
        if (r) {
            $.ajax({
                url: "/PersonalCenter/EditAllMessage",
                type: 'POST',
                data: {
                    //ids: sum
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $.messager.alert('提示', result.Message);
                        $("#MessageList").datagrid("reload");
                    }
                    else {
                        $.messager.alert('提示', result.Message);
                    }
                }
            });
        }
    });
};