var line = 0;
$(function () {

    //加载数据列表
    load_list();

    //修改
    $('#_edit').unbind('click').bind('click', function () {
        _edit();
    });

    //新增
    $('#_add').unbind('click').bind('click', function () {
        _add();
    });

    //删除
    $('#_delete').unbind('click').bind('click', function () {
        _delete();
    });
    //批量导入
    $('#batchImport').unbind('click').bind('click', function () {
        batchImport();
    });
  
    //搜索
    $('#maintenance_search').unbind('click').bind('click', function () {
        search();
    });
});

//加载数据列表
function load_list() {
    //显示分页条数
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);

    var search = $("#search").combobox('getValue');
    var key = $("#key").textbox('getText');
    $('#Probe_Library_datagrid').datagrid(
       {
           //title: '台账资料维护',
           // iconCls: 'icon-add',         
           nowrap: false,
           striped: true,
           rownumbers: true,
           //singleSelect: true,
           //autoRowHeight: true,
           ctrlSelect: true,
           border: false,
           fitColumns: true,
           //fixed:true,
           resizable: false,
           fit: true,
           pagination: true,
           pageSize: num,
           pageList: [num, num + 10, num + 20, num + 30],
           pageNumber: 1,
           type: 'POST',
           dataType: "json",
           queryParams: {
               search: search,
               key: key
           },
           url: "/LedgerManagement/load_TestBlockLibrary",//接收一般处理程序返回来的json数据         
           columns: [[
            { field: 'id', title: 'ID', hidden: 'true', width: 30 },
           { field: 'CalBlockNum', title: '试块编号', width: 100, sortable: true },
           { field: 'CalBlock', title: '校验试块', width: 100 },
           { field: 'C_S', title: '校验表面', width: 100 },
           { field: 'InstrumentSet', title: '仪器设置', width: 100 },
           { field: 'Reflector', title: '参考反射体', width: 100 },
           { field: 'CreatePerson', title: '添加人', width: 100 },
           //{
           //    field: 'CreateTime', title: '添加时间', width: 100, formatter: function (value, row, index) {
           //        if (value) {
           //            if (value.length >= 10) {
           //                value = value.substr(0, 10)
           //                return value;
           //            }
           //        }
           //    }
           //},
           {
               field: 'State', title: '状态', width: 30,
               formatter:function(value){
                   if(value == 1){return "在用"} if(value == 0){return "停用"}
                }
           },
           { field: 'Remarks', title: '说明', width: 100 },
           ]],
           rowStyler: function (index, row) {
               if (row.State == "0") {
                   return 'color:red;';
               }
           },
           onSelect: function (index, row) {
               //$('#Probe_Library_datagrid').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
           },
           onLoadSuccess: function (data) {
               //默认选择行
               $('#Probe_Library_datagrid').datagrid('selectRow', line);
               var selectRow = $("#Probe_Library_datagrid").datagrid("getSelected");
               if (!selectRow) {
                   $('#Probe_Library_datagrid').datagrid('selectRow', line);
               }
           },
           sortName: "CalBlockNum",
           sortOrder: 'asc', //排序
           toolbar: Probe_Library_datagrid_toolbar
       }).datagrid('resize');


    //搜索下拉框
    $('#search').combobox({
        editable: false,
        data: [
           { 'value': 'CalBlockNum', 'text': '试块编号' },
           { 'value': 'CalBlock', 'text': '校验试块' },
        ]
    });
}

//搜索
function search() {

    $('#Probe_Library_datagrid').datagrid(
    {
        url: "/LedgerManagement/load_TestBlockLibrary",//接收一般处理程序返回来的json数据         
        queryParams: {
            search: $("#search").combobox('getValue'),
            key: $("#key").textbox('getText')
        },
        onLoadSuccess: function (data) {
            $('#Probe_Library_datagrid').datagrid('selectRow', line);
        }
    }).datagrid('resize');
}

//修改资料
function _edit() {
    $("#CalBlockNum").textbox({
        readonly: true
    })

    var selectRow = $('#Probe_Library_datagrid').datagrid('getSelected');
    if (selectRow) {
        line = $('#Probe_Library_datagrid').datagrid("getRowIndex", selectRow);
        //状态
        $('#State').combobox({
            editable: false,
            data: [
               { 'value': '1', 'text': '在用' },
               { 'value': '0', 'text': '停用' }
            ]
        });

        $('#AddEidt_info').dialog({
            width: 670,
            height: 400,
            modal: true,
            title: '修改资料',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    Edit_info(selectRow);
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#AddEidt_info').dialog('close');
                }
            }]
        });
        $("#AddEidt_info").form("reset");
        var rows = $('#Probe_Library_datagrid').datagrid('getSelections');
        $('#AddEidt_info').form('load', rows[0]);
    }
    else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
}
//修改信息
function Edit_info(selectRow) {
    console.log(selectRow)
    $('#AddEidt_info').form('submit', {
        url: "/LedgerManagement/Edit_TestBlockLibrary",
        ajax: true,
        onSubmit: function (param) {
            param.id = selectRow.ID;
        },
        success: function (data) {
            console.log(data)
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#Probe_Library_datagrid').datagrid('reload');
                $('#AddEidt_info').dialog('close');
                $.messager.alert('提示', result.Message);
            } else {
                $.messager.alert('提示', result.Message);
            }
        }
    });

}

//增加资料
function _add() {
    $("#CalBlockNum").textbox({
        readonly: false
    })
    //状态
    $('#State').combobox({
        data: [
           { 'value': '1', 'text': '在用' },
           { 'value': '0', 'text': '停用' }
        ]
    });

    $('#AddEidt_info').dialog({
        width: 670,
        height: 400,
        modal: true,
        title: '添加资料',
        draggable: true,
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                Add_info();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#AddEidt_info').dialog('close');
            }
        }]
    });
    $("#AddEidt_info").form("reset");
    //var rows = $('#Probe_Library_datagrid').datagrid('getSelections');
    //$('#AddEidt_info').form('load', rows[0]);
}
//添加信息
function Add_info() {

    $('#AddEidt_info').form('submit', {
        url: "/LedgerManagement/Add_TestBlockLibrary",
        // ajax: true,
        onSubmit: function (param) {
            return $(this).form('enableValidation').form('validate');//验证表单
        },
        success: function (data) {
            console.log(data)
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#Probe_Library_datagrid').datagrid('reload');
                $('#AddEidt_info').dialog('close');
                $.messager.alert('提示', result.Message);
            } else {
                $.messager.alert('提示', result.Message);
            }
        }
    });
}

//删除资料
function _delete() {
    var selectRow = $("#Probe_Library_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('删除任务提示', '您确认要删除选中任务吗', function (r) {
            if (r) {

                $.ajax({
                    url: "/LedgerManagement/Del_TestBlockLibrary",
                    type: 'POST',
                    data: {
                        id: selectRow.ID,
                        CalBlock: selectRow.CalBlock
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        console.log(result);
                        if (result.Success == true) {
                            $('#Probe_Library_datagrid').datagrid('reload');
                            $.messager.alert('提示', result.Message);

                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        });

    }
    else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
}


//批量导入
function batchImport() {
    $("#importFileForm").dialog({
        width: 400,
        height: 150,
        modal: true,
        title: '批量导入',
        border: false,
        buttons: [
            {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    saveAddTemplate();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importFileForm').dialog('close');
                }
            }]
    })
}
//点击保存批量导入
function saveAddTemplate() {
    $.messager.progress({
        text: '正在导入'
    })
    $('#importFileForm').form('submit', {
        url: "/LedgerManagement/importTestBlockLibrary",
        onSubmit: function (param) {
            param.View_Temp_Folder = ''
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $.messager.progress('close');
                $('#Probe_Library_datagrid').datagrid('reload');
                $('#importFileForm').dialog('close');
                $.messager.alert('提示', result.Message);
            } else {
                $.messager.alert('提示', result.Message);
            }

        }
    });
}
