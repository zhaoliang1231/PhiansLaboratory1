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

    //搜索
    $('#maintenance_search').unbind('click').bind('click', function () {
        maintenance_search();
    });
});

//加载数据列表
function load_list() {
    //显示分页条数
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    var search = $("#search").combobox('getValue');
    var key = $("#key").textbox('getText');

    $('#Device_Library_datagrid').datagrid(
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
           //fitColumns: true,
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
           url: "Device_Library.aspx?cmd=load_maintenance",//接收一般处理程序返回来的json数据         
           columns: [[
            { field: 'id', title: 'id', hidden: 'true' },
           { field: 'equipment_nem', title: '设备名称' },
           { field: 'equipment_num', title: '设备编号' },
           { field: 'equipment_Type', title: '设备类型' },
           { field: 'range_', title: '测量范围' },
           { field: 'Manufacture', title: '制造商' },
           { field: 'effective', title: '有效期' },
           { field: 'E_state', title: '状态', formatter: function (value, row, index) {
                   if (value == "1") {
                       return "在用";
                   }
                   if (value == "2") {
                       return "停用";
                   }
                   if (value == "3") {
                       return "作废";
                   }
                   if (value == "4") {
                       return "封存";
                   }
               } },
           { field: 'remarks', title: '备注' }
           ]],
           rowStyler: function (index, row) {
               if (row.Probe_state == "2") {
                   return 'color:red;';
               }
           },
           onSelect: function (index, row) {
               //$('#Device_Library_datagrid').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
           },
           onLoadSuccess: function (data) {
               //默认选择行
               $('#Device_Library_datagrid').datagrid('selectRow', line);
               var selectRow = $("#Device_Library_datagrid").datagrid("getSelected");
               if (!selectRow) {
                   $('#Device_Library_datagrid').datagrid('selectRow', 0);
               }

           },
           sortOrder: 'asc',
           toolbar: Device_Library_datagrid_toolbar
       }).datagrid('resize');

    //搜索下拉框
    $('#search').combobox({
        data: [
           { 'value': 'equipment_nem', 'text': '设备名称' },
           { 'value': 'equipment_num', 'text': '设备编号' },
           { 'value': 'equipment_Type', 'text': '设备类型' },
           { 'value': 'Manufacture', 'text': '制造商' }
        ]
    });
}

//搜索
function maintenance_search() {

    $('#Device_Library_datagrid').datagrid(
    {
        url: "Device_Library.aspx?cmd=load_maintenance",//接收一般处理程序返回来的json数据         
        queryParams: {
            search: $("#search").combobox('getValue'),
            key: $("#key").textbox('getText')
        },
        onLoadSuccess: function (data) {
            $('#Device_Library_datagrid').datagrid('selectRow', 0);
        }
    }).datagrid('resize');
}

//修改资料
function _edit() {
    var selectRow = $('#Device_Library_datagrid').datagrid('getSelected');
    if (selectRow) {
        line = $('#Device_Library_datagrid').datagrid("getRowIndex", selectRow);
        //状态
        $('#E_state').combobox({
            data: [
               { 'value': '1', 'text': '在用' },
               { 'value': '2', 'text': '停用' },
               { 'value': '3', 'text': '作废' },
               { 'value': '4', 'text': '封存' }
            ]
        });

        $('#AddEidt_info').dialog({
            width: 650,
            height: 420,
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
        var rows = $('#Device_Library_datagrid').datagrid('getSelections');
        $('#AddEidt_info').form('load', rows[0]);
    }
    else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
}
//修改信息
function Edit_info(selectRow) {
    if ($('#equipment_nem').textbox('getText') == "") {
        $.messager.alert('提示', '设备名称不能为空！');
        return;
    } if ($('#equipment_num').textbox('getText') == "") {
        $.messager.alert('提示', '设备编号不能为空！');
        return;
    }

    $('#AddEidt_info').form('submit', {
        url: "Device_Library.aspx",
        ajax: true,
        onSubmit: function (param) {
            param.cmd = 'Device_edit';
            param.id = selectRow.id;
        },
        success: function (data) {
            if (data == 'T') {
                $('#AddEidt_info').dialog('close');
                $.messager.alert('提示', '修改信息成功');
                $('#Device_Library_datagrid').datagrid('reload');
            } else if (data == "无权操作！") {
                $.messager.alert('提示', '您没有权限编辑资料！');
            } else {
                $.messager.alert('提示', '修改信息失败');

            }
        }
    });

}

//增加资料
function _add() {
    //状态
    $('#E_state').combobox({
        data: [
           { 'value': '1', 'text': '在用' },
           { 'value': '2', 'text': '停用' },
           { 'value': '3', 'text': '作废' },
           { 'value': '4', 'text': '封存' }
        ]
    });

    $('#AddEidt_info').dialog({
        width: 650,
        height: 420,
        modal: true,
        title: '增加资料',
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
    var rows = $('#Device_Library_datagrid').datagrid('getSelections');
    $('#AddEidt_info').form('load', rows[0]);
}
//添加信息
function Add_info() {
    if ($('#equipment_nem').textbox('getText') == "") {
        $.messager.alert('提示', '设备名称不能为空！');
        return;
    } if ($('#equipment_num').textbox('getText') == "") {
        $.messager.alert('提示', '设备编号不能为空！');
        return;
    }

    $('#AddEidt_info').form('submit', {
        url: "Device_Library.aspx",
        ajax: true,
        onSubmit: function (param) {
            param.cmd = 'Device_add';
        },
        success: function (data) {
            if (data == 'T') {
                $('#AddEidt_info').dialog('close');
                $.messager.alert('提示', '添加信息成功');
                $('#Device_Library_datagrid').datagrid('reload');
            } else if (data == "无权操作！") {
                $.messager.alert('提示', '您没有权限编辑资料！');
            } else {
                $.messager.alert('提示', '添加信息失败');

            }
        }
    });
}
//删除资料
function _delete() {
    var selectRow = $("#Device_Library_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('删除任务提示', '您确认要删除选中任务吗', function (r) {
            if (r) {
                //var rows = $("#Device_Library_datagrid").datagrid('getSelections');
                //var rowIds = new Array();
                //for (var i = 0; i < rows.length; i++) {
                //    rowIds.push(rows[i].id)
                //}
                //var ids = rowIds.join(",");

                $.ajax({
                    url: "Device_Library.aspx?cmd=Device_delete",
                    type: 'POST',
                    data: {
                        //ids: ids
                        id: selectRow.id
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $.messager.alert('提示', '操作成功！');
                            $('#Device_Library_datagrid').datagrid('reload');
                        } if (data == "无权操作！") {
                            $.messager.alert('提示', '您没有权限删除资料！');

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
