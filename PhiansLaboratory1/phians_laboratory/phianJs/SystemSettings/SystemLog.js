$(function () {
    System_log();//日志列表
    search();//下拉框初始化
    //搜索
    $('#search_info').unbind('click').bind('click', function () {
        search_info();
    });

    //查看信息
    $('#click_ok').unbind('click').bind('click', function () {
        click_ok();
    });
});
//日志列表
function System_log() {
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    var search = $('#search').combobox('getText');
    var key = $('#key').textbox('getText');
    $('#System_log').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        border: true,
        pagination: true,
        fit:true,
        fitColumns: true,
        pageSize: num,
        pageList: [num, num + 30, num + 60, num + 90,num+120],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            search: search,
            key: key
        },
        url: "/SystemSettings/GetPageList",//接收一般处理程序返回来的json数据     
        columns: [[
        { field: 'UserName', title: '登录用户', sortable: 'true', width: 50 },
        { field: 'OperationDate', title: '操作日期', width: 80 },
        { field: 'OperationType', title: '操作类型', width: 100 },
        { field: 'OperationInfo', title: '操作内容', width: 100 },
        { field: 'OperationIp', title: '用户IP', width: 80 }
        //,
        //{
        //    field: 'OperationInfo', title: 'Operation Info', width: 120, formatter: function (value, row, index) {
        //        if (value) {
        //            if (value.length > 30) {
        //                var result = value.replace(" ", "");//去空
        //                var value1 = result.substr(0, 30);
        //                return '<span  title=' + value + '>' + value1 + "......" + '</span>';
        //            } else {
        //                return '<span  title=' + value + '>' + value + '</span>';
        //            }
        //        }
        //    }
        //}
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#System_log').datagrid('selectRow', 0);
        },
        sortable: 'asc',//排序
        toolbar: "#message_show_toolbar"//列表工具栏
    });
};
//搜索下拉框内容
function search() {
    $('#search').combobox({
        panelHeight: 120,
        data: [
               { 'value': 'UserName', 'text': '登录用户' },
                { 'value': 'OperationDate', 'text': '操作日期' },
                { 'value': 'OperationType', 'text': '操作类型' },
                { 'value': 'OperationInfo', 'text': '操作内容' },
                { 'value': 'OperationIp', 'text': '用户IP' }
        ],
        onSelect: function () {
            var value = $('#search').combobox('getValue');//获取搜索下拉框的值
            if (value == "OperationDate") {
                $("#key_span").html('<input id="key" style="width: 150px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span'));

            } else {
                $("#key_span").html('<input id="key" style="width: 150px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span");
            }
        }
    });
};
//搜索
function search_info() {
    var search = $('#search').combobox('getValue');//获取搜索下拉框的值
    var key = $('#key').textbox('getText');//获取搜索文本框的值
    $('#System_log').datagrid({
        type: 'POST',
        dataType: "json",
        url: "/SystemSettings/GetPageList",//接收一般处理程序返回来的json数据                
        queryParams: {
            search: search,//传搜索下拉框的内容给后台
            key: key//传搜索文本框的内容给后台
        }
    });
};
//查看信息
function click_ok() {
    var selected_report = $("#System_log").datagrid("getSelected");
    var rowss = $("#System_log").datagrid("getSelections");
    if (selected_report) {
        $('#S1_dialog').dialog({
            width: 500,
            height: 400,
            modal: true,
            title: '查看日志内容',
            border: false,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#S1_dialog').dialog('close');//关闭查看信息弹窗
                }
            }]
        });
        $("#S1_dialog").form('load', rowss[0]);//回显获取到的第一条数据给form表单
        $("#UserId").textbox('setText', rowss[0].UserName);//设置UserId文本框回显的内容
    }
    else {
    $.messager.alert('提示', '请选择要操作的行！');
    }
};




