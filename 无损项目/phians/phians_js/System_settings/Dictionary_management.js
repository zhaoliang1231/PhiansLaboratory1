$(function () {
  var   line = 0;
    //初始化组件大小
    init_size();  
    //树初始化
    tree_init();  
    //列表初始化
    load_datagrid(line);
   
 

})

//添加字典内容
function insert_form() {
    $('#dictionary_dialog').dialog({
        width: 380,
        height: 280,
        modal: true,
        title: '添加字典内容',
        draggable: true,
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            id: 'save'
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#dictionary_dialog').dialog('close');
            }
        }]
    });

    $('#dictionary_dialog').form('reset');
    $("#save").unbind('click').bind('click', function () {
        var node_on = $('#project_info').tree('getSelected');
        //form表单提交
        if ($("#Project_name").textbox("getText") != '') {
            if ($("#Project_value").textbox("getText") != '') {
                $('#dictionary_dialog').form('submit', {
                    url: "Dictionary_management.ashx",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.cmd = 'insert_context';
                        param.gorop_id = node_on.id
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $('#dictionary_dialog').dialog('close');
                            $('#standards_info').datagrid('reload');
                            $.messager.alert('提示', '添加信息成功');

                        } if (data == "无权操作！") {
                            $.messager.alert('提示', '您没有权限添加资料！');

                        }
                    }
                });
            } else {
                $.messager.alert('提示', '项目值不能为空！');
            }
        } else {
            $.messager.alert('提示', '项目内容不能为空！');
        }
       
    })
}
function edit_form() {
    //修改
   
        var selectRow = $("#standards_info").datagrid("getSelected");
        if (selectRow) {
            line = $('#standards_info').datagrid("getRowIndex", selectRow);
            $('#dictionary_dialog').dialog({
                width: 380,
                height: 280,
                modal: true,
                title: '修改字典内容',
                draggable: true,
                buttons: [{
                    text: '确定',
                    iconCls: 'icon-ok',
                    id: 'save1'
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#dictionary_dialog').dialog('close');
                    }
                }]
            });
            //获取选中
            var rowss = $('#standards_info').datagrid('getSelections'); //获取选中数据   
            $('#dictionary_dialog').form('load', rowss[0]);
            $("#save1").unbind('click').bind('click', function () {
                //form表单提交
                if ($("#Project_name").textbox("getText") != '') {
                    if ($("#Project_value").textbox("getText") != '') {
                        $('#dictionary_dialog').form('submit', {
                            url: "Dictionary_management.ashx",//接收一般处理程序返回来的json数据     
                            onSubmit: function (param) {
                                param.cmd = 'edit_context';
                                param.id = selectRow.id;
                            },
                            success: function (data) {
                                if (data == 'T') {
                                    $('#dictionary_dialog').dialog('close');
                                    $.messager.alert('提示', '修改信息成功');
                                    $('#standards_info').datagrid('reload');
                                } else if (data == "无权操作！") {
                                    $.messager.alert('提示', '您没有权限编辑资料！');
                                }
                                else {
                                    $.messager.alert('提示', '修改信息失败');
                                }
                            }
                        });
                    } else {
                        $.messager.alert('提示', '项目值不能为空！');
                    }
                } else {
                    $.messager.alert('提示', '项目内容不能为空！');
                }
            })
        } else {
            $.messager.alert('提示', '请选择要操作的任务！');
        }
}
//删除内容
function delete_task() {
    var selectRow = $("#standards_info").datagrid("getSelected");
    var rowss = $('#standards_info').datagrid('getSelections'); //获取选中数据 
    var ids1 = [];
    for (var i = 0; i < rowss.length; i++) {
        ids1.push(rowss[i].id);
    }
    var ids = ids1.join(';'); 
    if (selectRow) {
        $.messager.confirm('删除提示', '您确认要删除选中内容吗', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: "Dictionary_management.ashx?&cmd=del_context",
                    data: { id: ids },
                    type: "POST",
                    success: function (data) {
                        $('#standards_info').datagrid('reload');
                    }
                });
            }
        });
    } else {
        $.messager.alert('提示', '请选择要操作的内容！');
    }
}

function tree_init() {


    $('#project_info').tree({
        url: "Dictionary_management.ashx?&cmd=loadtree",
        method: 'post',
        required: true,
        fit:true,
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#project_info').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY
               
            });
        },
        onAfterEdit: function () {
            var node = $('#project_info').tree('getSelected');
            $.ajax({
                url: "Dictionary_management.ashx?&cmd=treeedit",
                type: 'POST',
                data: {
                    ids: node.id,
                    Project_name: node.text
                },
                success: function (data) {
                    if (data == 'T') {
                        $.messager.alert('提示', '编辑信息成功');
                    }
                }
            })
        },
        onSelect: function () {
            tree_onselect();
        }
    });

    tree_action();
    //添加字典内容
    $("#click_add").unbind('click').bind('click', function () {
        insert_form();
    })
    //修改字典内容
    $("#click_edit").unbind('click').bind('click', function () {
        edit_form();
    })
    //删除字典内容
    $("#click_del").unbind('click').bind('click', function () {
        delete_task();
    })
}
//树的操作
function tree_action() {



    ////删除一个节点
    //$('#tree_del').click(function () {
    //    var nodes = $('#project_info').tree('getSelected');
    //    var ids = '';
    //    ids = nodes.id;
    //    $.ajax({
    //        url: "Dictionary_management.ashx?cmd=treedel",
    //        type: 'POST',
    //        data: {
    //            ids: ids
    //        },
    //        success: function (data) {
    //            if (data == 'T') {
    //                $.messager.alert('提示', '删除信息成功');
    //                $('#project_info').tree('reload');
    //            }

    //        },
    //        error: function () {
    //            $.messager.alert('提示', '删除信息失败');
    //        }
    //    })

    //});
    //编辑项目
    $('#tree_edit').unbind('click').bind('click', function () {
        var node = $('#project_info').tree('getSelected');
        if (node) {
            //node.text = '修改';  //-->txt-->DB
            $('#project_info').tree('beginEdit', node.target);
        }
    })
    //添加同级项目
    $('#tree_add').unbind('click').bind('click', function () {
        var node_add = $('#project_info').tree('getSelected');
        var node_Parent = $('#project_info').tree('getParent', node_add.target);
        $('#tree_add_dialog_input').textbox({
        })

        $('#tree_add_dialog').dialog({
            title: '添加项目',
            width: 400,
            height: 130,
            left: 50,
            top: 100,
            buttons: [{
                text: '保存',
                iconCls: 'icon-save',
                id: 'tree_add_save'

            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                id: 'tree_add_cancel'

            }]

        });
        //保存添加同级项目
        $('#tree_add_save').unbind('click').bind('click', function () {
            $.ajax({
                url: "Dictionary_management.ashx?&cmd=treeadd",
                type: 'POST',
                data: {
                    // ids: node_add.id,
                    //发送父id
                    Parent_id: node_Parent.id,
                    Project_name: $('#tree_add_dialog_input').textbox('getText')
                },
                success: function (data) {
                    if (data == 'T') {

                        if (node_add) {
                            $('#tt').tree('insert', {
                                before: node_add.target,
                                data: {

                                    text: $('#tree_add_dialog_input').textbox('getText'),
                                }
                            });
                        }
                        $('#tree_add_dialog').dialog('close');
                        $.messager.alert('提示', '添加信息成功');
                        $('#project_info').tree('reload');
                    }
                }
            })
        });
        //取消添加同级项目
        $('#tree_add_cancel').unbind('click').bind('click', function () {
            $('#tree_add_dialog').dialog('close');

        });
    });
    //添加下级项目
    $('#tree_add_next').unbind('click').bind('click', function () {
        var node_add = $('#project_info').tree('getSelected');
        var node_Parent = $('#project_info').tree('getParent', node_add.target);
        $('#tree_add_dialog_input').textbox({
        })

        $('#tree_add_dialog').dialog({
            title: '添加项目',
            width: 400,
            height: 130,
            left: 50,
            top: 100,
            buttons: [{
                text: '保存',
                iconCls: 'icon-save',
                id: 'tree_add_next_save'

            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                id: 'tree_add_next_cancel'
            }]

        });
        // 保存添加下级项目
        $('#tree_add_next_save').unbind('click').bind('click', function () {
            $.ajax({
                url: "Dictionary_management.ashx?cmd=treeadd_next",
                type: 'POST',
                data: {
                    ids: node_add.id,
                    //发送父id
                    //Parent_id: node_Parent.id,
                    Project_name: $('#tree_add_dialog_input').textbox('getText')
                },
                success: function (data) {
                    if (data == 'T') {

                        $('#tree_add_dialog').dialog('close');
                        $.messager.alert('提示', '添加信息成功');
                        $('#project_info').tree('reload');
                    }

                }

            })


        });
        $('#tree_add_next_cancel').unbind('click').bind('click', function () {
            $('#tree_add_dialog').dialog('close');
        });
    })


}

function tree_onselect() {

    var node_on = $('#project_info').tree('getSelected');
  
    $('#standards_info').datagrid({
        url: "Dictionary_management.ashx?&cmd=load_info",
        dataType: "json",
        type: 'POST',
        queryParams: {
            project_id: node_on.id
        }
    });


}
//初始化组件大小
function init_size() {

    var tabs_width = screen.width - 182;

    //iframe可用高度
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 125;
    $('#department_layout').layout('panel', 'west').panel('resize', {
        width: 300,
        height: _height

    });
    $('#department_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 350,
        height: _height
    });
    //$('#test_layout').layout('panel', 'south').panel('resize', { height: 30 });
    $('#department_layout').layout('resize');


}

//列表初始化
function load_datagrid(line) {
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    $('#standards_info').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        title: '字典内容',
        pagination: true,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num + 20],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        columns: [[
           { title: '项目内容', field: 'Project_name' },
           { title: '项目值', field: 'Project_value' },
           { title: '排序', field: 'Sort_num' },
           { title: '说明', field: 'remarks' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#standards_info').datagrid('selectRow', line);
           
        },

        rowStyler: function (index, row) {
            if (row.expedited == "是") {
                //return 'background-color:pink;color:blue;font-weight:bold;';
                return 'color:red;';
            }
        },
        sortOrder: 'asc',
        toolbar: "#standards_info_toolbar"
    });


}
