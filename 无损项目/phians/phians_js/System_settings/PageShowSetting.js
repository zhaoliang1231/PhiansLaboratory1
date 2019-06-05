//PageSettingDatagrid

$(function () {
    $('#Page').combobox({
       // url: "PageShowSetting.ashx?&cmd=loadPageCombobox",
        valueField: 'fid',
        textField: 'm_name',
        data: [
           { fid: "101", m_name: "无损报告编制" },
           { fid: "102", m_name: "无损报告审核" },
           { fid: "103", m_name: "无损报告签发" },
           { fid: "104", m_name: "无损报告管理" },
           { fid: "113", m_name: "无损监控管理" }
        ],
        onSelect: function () {
            var fid = $('#Page').combobox('getValue');
            load_list(fid);
        },
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    $('#PageSettingDatagrid').datagrid({
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
      //  url: "PageShowSetting.ashx?&cmd=loadInfo",//接收一般处理程序返回来的json数据         
        columns: [[
        
            { field: 'Title', title: '字段名称' },
            { field: 'FieldName', title: '字段' },

            //{ field: 'PageId', title: '页面ID' },
            {
                field: 'hidden', title: '是否显示', formatter: function (value, row, index) {
                    if (value == "True") {
                        value = "不显示";
                    } else {
                        value = "显示";
                    }
                    return value;
                }
            },
            {
                field: 'Sortable', title: '是否排序', formatter: function (value, row, index) {
                    if (value == "True") {
                        value = "是";
                    } else {
                        value = "否";
                    }
                    return value;
                }
            },
            { field: 'Operator', title: '操作人' },
            {
                field: 'OperateDate', title: '操作时间', formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            { field: 'remarks', title: '附注' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#System_log').datagrid('selectRow', 0);
        },
        sortable: 'asc',
        toolbar: "#message_show_toolbar"
    });
    //显示
    $('#hidden1').unbind('click').bind('click', function () {
        $("#hidden1").prop("checked", "checked");
        $("#hidden2").prop("checked", false);
        $("#hidden").val("1");
    });
    //不显示
    $('#hidden2').unbind('click').bind('click', function () {
        $("#hidden2").prop("checked", "checked");
        $("#hidden1").prop("checked", false);
        $("#hidden").val("0");
    });
    
    //排序
    $('#Sortable1').unbind('click').bind('click', function () {
        $("#Sortable1").prop("checked", "checked");
        $("#Sortable2").prop("checked", false);
        $("#Sortable").val("1");
    });
    //否排序
    $('#Sortable2').unbind('click').bind('click', function () {
        $("#Sortable2").prop("checked", "checked");
        $("#Sortable1").prop("checked", false);
        $("#Sortable").val("0");
    });
    $('#PageSettingAdd').unbind('click').bind('click', function () {
        var selectRow = $("#PageSettingDatagrid").datagrid("getSelected");
        if (selectRow) {
            $('#S1_dialog').dialog({
                width: 650,
                height: 400,
                title: "修改",
                modal: true,
                draggable: true,
                buttons: [{
                    text: '修改',
                    iconCls: 'icon-ok',
                    handler: function () {
                        $('#S1_dialog').form('submit', {
                            url: "PageShowSetting.ashx",//接收一般处理程序返回来的json数据     
                            onSubmit: function (param) {
                                param.id = selectRow.id;
                                param.cmd = 'EditInfo';
                            },
                            success: function (data) {
                                if (data == 'T') {
                                    $('#S1_dialog').dialog('close');
                                    $('#PageSettingDatagrid').datagrid('reload');
                                    $.messager.alert('提示', '修改信息成功!');
                                }
                                else if (data == 'F') {
                                    $.messager.alert('提示', '修改信息失败!');
                                }
                            }
                        });

                    },

                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#S1_dialog').dialog('close');
                    },

                }]
            })
            var rowss = $('#PageSettingDatagrid').datagrid('getSelections');
            $('#S1_dialog').form('load', rowss[0]);
            console.log(rowss[0]);
            if (rowss[0].Sortable == true || rowss[0].Sortable == "1" || rowss[0].Sortable == "True") {
                $("#Sortable1").prop("checked", "checked");
                $("#Sortable2").prop("checked", false);
                $("#Sortable").val("1");
            } else {
                $("#Sortable2").prop("checked", "checked");
                $("#Sortable1").prop("checked", false);
                $("#Sortable").val("0");
            }
            if (rowss[0].hidden == true || rowss[0].hidden == "1"||rowss[0].hidden == "True") {
                $("#hidden1").prop("checked", "checked");
                $("#hidden2").prop("checked", false);
                $("#hidden").val("1");
            } else {
                $("#hidden2").prop("checked", "checked");
                $("#hidden1").prop("checked", false);
                $("#hidden").val("0");
            }
           
        }
        else {
            $.messager.alert('提示', '请选择要操作的行！');
        }
       


     
    });

});
//加载列表
function load_list(fid) {
    $('#PageSettingDatagrid').datagrid(
        {
            dataType: "json",
            url: "PageShowSetting.ashx?&cmd=loadInfo",//接收一般处理程序返回来的json数据
            queryParams: {
                PageId: fid
            },
        
            sortOrder: 'asc',
           // toolbar: "#reports_toolbar"
        });
}




