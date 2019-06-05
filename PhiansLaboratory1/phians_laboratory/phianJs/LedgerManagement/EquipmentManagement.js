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
           url: "/LedgerManagement/load_maintenanceProbe",//接收一般处理程序返回来的json数据         
           columns: [[
           { field: 'id', title: 'id', hidden: 'true',width:30},
           { field: 'Probe_name', title: '探头名称', width: 80 },
           { field: 'Probe_num', title: '探头编号', width: 200, sortable: true },
           { field: 'Probe_type', title: '探头类型', width: 100 },
           { field: 'Probe_size', title: '探头尺寸', width: 100 },
           { field: 'Probe_Manufacture', title: '制造商', width: 100 },
           { field: 'Probe_frequency', title: '频率', width: 100 },
           { field: 'Coil_Size', title: '线圈尺寸', width: 100 },
           { field: 'Probe_Length', title: '探头长度', width: 100 },
           { field: 'Cable_Length', title: '探头扩展线缆长度', width: 100 },
           { field: 'Mode_L', title: '波型L', width: 100 },
           { field: 'Mode_T', title: '波型X', width: 100 },
           { field: 'DoublePort', title: '单S/双口', width: 100 },
           { field: 'FocalLength', title: '聚焦', width: 100 },
           { field: 'TFront', title: '前沿', width: 100 },
           { field: 'CurvedSurface', title: '曲面', width: 100 },
           { field: 'Circumferential', title: '周向/轴向', width: 100 },
           { field: 'Position', title: '位置', width: 100 },
           { field: 'Chip_size', title: '晶片尺寸', width: 100 },
           { field: 'Angle', title: '角度', width: 100 },
           { field: 'Nom_Angle', title: '标称角度', width: 100 },
           { field: 'Shoe', title: '楔块', width: 100 },
           { field: 'Radius·', title: '半径', width: 100 },
           {
               field: 'Probe_state', title: '状态', formatter: function (value, row, index) {
                   if (value == "1") {
                       return "在用";
                   }
                   if (value == "2") {
                       return "停用";
                   }
               }, width: 100
           },
           { field: 'remarks', title: '说明', width: 100 }
           ]],
           rowStyler: function (index, row) {
               if (row.Probe_state == "2") {
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
           sortName: "Probe_num",
           sortOrder: 'asc', //排序
           toolbar: Probe_Library_datagrid_toolbar
       }).datagrid('resize');


    //搜索下拉框
    $('#search').combobox({
        editable:false,
        data: [
           { 'value': 'Probe_name', 'text': '探头名称' },
           { 'value': 'Probe_type', 'text': '探头型号' },
           { 'value': 'Probe_num', 'text': '探头编号' },
           { 'value': 'Manufacture', 'text': '制造商' }
        ]
    });
}

//搜索
function search() {

    $('#Probe_Library_datagrid').datagrid(
    {
        url: "/LedgerManagement/load_maintenanceProbe",//接收一般处理程序返回来的json数据         
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
    $("#Probe_num").textbox({
        readonly: true
    })
    var selectRow = $('#Probe_Library_datagrid').datagrid('getSelected');
    if (selectRow) {
        line = $('#Probe_Library_datagrid').datagrid("getRowIndex", selectRow);
        //状态
        $('#Probe_state').combobox({
            editable: false,
            data: [
               { 'value': '1', 'text': '在用' },
               { 'value': '2', 'text': '停用' }
            ]
        });

        $('#AddEidt_info').dialog({
            width: 670,
            height: 550,
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
    
    $('#AddEidt_info').form('submit', {
        url: "/LedgerManagement/Device_editProbe",
        ajax: true,
        onSubmit: function (param) {
            //param.cmd = 'Probe_edit';
            param.id = selectRow.id;
        },
        success: function (data) {         
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
    $("#Probe_num").textbox({
        readonly: false
    })
    //状态
    $('#Probe_state').combobox({
        data: [
           { 'value': '1', 'text': '在用' },
           { 'value': '2', 'text': '停用' }
        ]
    });

    $('#AddEidt_info').dialog({
        width: 670,
        height: 550,
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
        url: "/LedgerManagement/Probe_addProbe",
       // ajax: true,
        onSubmit: function (param) {
            return $(this).form('enableValidation').form('validate');//验证表单
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#Probe_Library_datagrid').datagrid('reload');
                $('#AddEidt_info').dialog('close');
                $.messager.alert('提示', result.Message);
            }else {
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
                //var rows = $("#Probe_Library_datagrid").datagrid('getSelections');
                //var rowIds = new Array();
                //for (var i = 0; i < rows.length; i++) {
                //    rowIds.push(rows[i].id)
                //}
                //var ids = rowIds.join(",");

                $.ajax({
                    url: "/LedgerManagement/Device_deleteProbe",
                    type: 'POST',
                    data: {
                        //ids: ids
                        id: selectRow.id,
                        Probe_name: selectRow.Probe_name
                        //EquipmentName: selectRow.Probe_name
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
        url: "/LedgerManagement/importProbe",
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
