//System_log
$(function () {
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    $('#System_log').datagrid({
        border: false,
        nowrap: false,
        striped: true,
        rownumbers: true,
        //singleSelect: true,
        //singleSelect: true,
        ctrlSelect: true,
        //autoRowHeight: true,
        fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num + 30],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "System_log_new.ashx?&cmd=system_log_info",//接收一般处理程序返回来的json数据         
        columns: [[
        { field: 'operation_user', title: '登录用户', sortable: 'true' },
        { field: 'operation_name', title: '登录用户名' },      
        { field: 'operation_date', title: '操作时间' },
        { field: 'operation_type', title: '操作类别' },
        { field: 'operation_info', title: '操作内容' },        
        { field: 'operation_ip', title: 'ip' },
    
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#System_log').datagrid('selectRow', 0);
        },
        sortable:'asc',
        toolbar: "#message_show_toolbar"
    });
    //var data = System_log();
    //$('#System_log').datagrid('loadData', data);


    
    $('#search2').combobox({
       panelHeight: 120,
        data: [
                { 'value': 'operation_user', 'text': '登录用户' },
                { 'value': 'operation_name', 'text': '登录用户名' },
                { 'value': 'operation_date', 'text': '操作时间' },
                { 'value': 'operation_type', 'text': '操作类别' },
                { 'value': 'operation_info', 'text': '操作内容' }
        ]
    })


    //搜索
    $('#report_search').unbind('click').bind('click', function () {
        var search3 = $('#search3').textbox('getText');
        var search2 = $('#search2').combobox('getValue');
        //switch (search2) {
        //    case "登陆用户": search2 = "operation_user"; break;
        //    case "登录用户名": search2 = "operation_name"; break;
        //    case "操作时间": search2 = "operation_date"; break;
        //    case "操作类别": search2 = "operation_type"; break;
        //    default: search2 = "";
        //}
        $('#System_log').datagrid({
            type: 'POST',
            dataType: "json",
            url: "System_log_new.ashx?&cmd=system_log_search",//接收一般处理程序返回来的json数据                
            queryParams: {
                search3: search3,
                search2: search2
            }
        });
    });

    $('#click_ok').unbind('click').bind('click', function () {
        $('#S1_dialog').dialog({
            width: 350,
            height: 400,
            title: "查看",
            modal: true,
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',               
                handler: function () {
                    $('#S1_dialog').dialog('close');
                }
            }]
        }).dialog('close');


        var selectRow = $("#System_log").datagrid("getSelected");
        if (selectRow) {
            var rowss = $('#System_log').datagrid('getSelections');
            var h_select_id = selectRow.id;
            $('#S1_dialog').form('load', rowss[0]);
            $('#S1_dialog').dialog('open');

        }
        else {
            $.messager.alert('提示', '请选择要操作的行！');
        }    
    });

});




