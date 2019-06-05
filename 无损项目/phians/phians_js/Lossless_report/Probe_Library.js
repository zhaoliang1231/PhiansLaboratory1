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
        search();
    });
});

//加载数据列表
function load_list() {
    //显示分页条数
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
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
               search:search,
               key: key
           },
           url: "Probe_Library.aspx?cmd=load_maintenance",//接收一般处理程序返回来的json数据         
           columns: [[
            { field: 'id', title: 'id', hidden: 'true' },
           { field: 'Probe_name', title: '探头名称' },
           { field: 'Probe_num', title: '探头编号' },
           { field: 'Probe_type', title: '探头类型' },
           { field: 'Probe_size', title: '探头尺寸' },
           { field: 'Probe_Manufacture', title: '制造商' },
           { field: 'Probe_frequency', title: '频率' },
           { field: 'Coil_Size', title: '线圈尺寸' },
           { field: 'Probe_Length', title: '探头长度' },        
           { field: 'Cable_Length', title: '探头扩展线缆长度' },
           { field: 'Mode_', title: '波型' },       
           { field: 'Chip_size', title: '晶片尺寸' },
           { field: 'Angle', title: '角度' },               
           { field: 'Nom_Angle', title: '标称角度' },
           { field: 'Shoe', title: '楔块' },
           { field: 'Probe_state', title: '状态', formatter: function (value, row, index) {
                   if (value == "1") {
                       return "在用";
                   }
                   if (value == "2") {
                       return "停用";
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
               //$('#Probe_Library_datagrid').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
           },
           onLoadSuccess: function (data) {
               //默认选择行
               $('#Probe_Library_datagrid').datagrid('selectRow', line);
               var selectRow = $("#Probe_Library_datagrid").datagrid("getSelected");
               if (!selectRow) {
                   $('#Probe_Library_datagrid').datagrid('selectRow', 0);
               }
           },
           sortOrder: 'asc',
           toolbar: Probe_Library_datagrid_toolbar
       }).datagrid('resize');

    //搜索下拉框
    $('#search').combobox({
        data: [
           { 'value': 'Probe_name', 'text': '仪器名称' },
           { 'value': 'Probe_type', 'text': '仪器型号' },
           { 'value': 'Probe_num', 'text': '仪器编号' },
           { 'value': 'Manufacture', 'text': '制造商' }
        ]
    });
   }

//搜索
function search() {

       $('#Probe_Library_datagrid').datagrid(
       {
           url: "Probe_Library.aspx?cmd=load_maintenance",//接收一般处理程序返回来的json数据         
           queryParams: {
               search: $("#search").combobox('getValue'),
               key: $("#key").textbox('getText')
           },
           onLoadSuccess: function (data) {
               $('#Probe_Library_datagrid').datagrid('selectRow', 0);
           }
       }).datagrid('resize');
   }

//修改资料
function _edit() {
    var selectRow = $('#Probe_Library_datagrid').datagrid('getSelected');
    if (selectRow) {
        line = $('#Probe_Library_datagrid').datagrid("getRowIndex", selectRow);
        //状态
        $('#Probe_state').combobox({
            data: [
               { 'value': '1', 'text': '在用' },
               { 'value': '2', 'text': '停用' }
            ]
        });

        $('#AddEidt_info').dialog({
            width: 670,
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
        var rows = $('#Probe_Library_datagrid').datagrid('getSelections');
        $('#AddEidt_info').form('load', rows[0]);
    }
    else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
}
//修改信息
function Edit_info(selectRow) {
    if ($('#Probe_num').textbox('getText') == "") {
        $.messager.alert('提示', '仪器编号不能为空！');
        return;
    } if ($('#Probe_name').textbox('getText') == "") {
        $.messager.alert('提示', '仪器名称不能为空！');
        return;
    } if ($('#Probe_state').textbox('getValue') == "") {
        $.messager.alert('提示', '请选择有效的状态！');
        return;
    }

    $('#AddEidt_info').form('submit', {
        url: "Probe_Library.aspx",
        ajax: true,
        onSubmit: function (param) {
            param.cmd = 'Probe_edit';
            param.id = selectRow.id;
        },
        success: function (data) {
            if (data == 'T') {
                $('#AddEidt_info').dialog('close');
                $.messager.alert('提示', '修改信息成功');
                $('#Probe_Library_datagrid').datagrid('reload');
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
        $('#Probe_state').combobox({
            data: [
               { 'value': '1', 'text': '在用' },
               { 'value': '2', 'text': '停用' }
            ]
        });

        $('#AddEidt_info').dialog({
            width: 670,
            height: 420,
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
    if ($('#Probe_num').textbox('getText') == "") {
        $.messager.alert('提示', '仪器编号不能为空！');
        return;
    } if ($('#Probe_name').textbox('getText') == "") {
        $.messager.alert('提示', '仪器名称不能为空！');
        return;
    } if ($('#Probe_state').textbox('getValue') == "") {
        $.messager.alert('提示', '请选择有效的状态！');
        return;
    }

    $('#AddEidt_info').form('submit', {
        url: "Probe_Library.aspx",
        ajax: true,
        onSubmit: function (param) {
            param.cmd = 'Probe_add';
        },
        success: function (data) {
            if (data == 'T') {
                $('#AddEidt_info').dialog('close');
                $.messager.alert('提示', '添加信息成功');
                $('#Probe_Library_datagrid').datagrid('reload');
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
                    url: "Probe_Library.aspx?cmd=Probe_delete",
                    type: 'POST',
                    data: {
                        //ids: ids
                        id:selectRow.id
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $.messager.alert('提示', '操作成功！');
                            $('#Probe_Library_datagrid').datagrid('reload');
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


