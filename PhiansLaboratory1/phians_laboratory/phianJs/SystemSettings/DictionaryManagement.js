var line = 0;
$(function () {
    
    layout();//布局
    project_info();//字典树信息加载
   
    //添加同级节点
    $('#tree_add').unbind('click').bind('click', function () {
        tree_add();
    });

    //添加字典内容
    $("#click_add").unbind('click').bind('click', function () {
        insert_form();
    });
    //修改字典内容
    $("#click_edit").unbind('click').bind('click', function () {
        edit_form();
    });
    //删除字典内容
    $("#click_del").unbind('click').bind('click', function () {
        delete_task();
    });
});

//布局
function layout() {
    //除导航外内容的高度
    var _height = $(".tab-content").height();
    var tabs_width = screen.width;
    //树部分的布局
    $('#Layout').layout('panel', 'west').panel('resize', {
    });
    //列表部分的布局
    $('#Layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 500,
        //height: _height
    });
    //重置布局
    $('#Layout').layout('resize');
    //判断是否是初始化加载
    var flag = 0;
};
//字典树信息加载
function project_info() {
    $('#project_info').tree({
        url: "/SystemSettings/LoadDictionaryPageTree?id=8c20e217-5dc9-4b11-83bc-0325b47f7807&Parent_id=8c20e217-5dc9-4b11-83bc-0325b47f7808",
        method: 'post',
        required: true,
        fit: true,
        top: 0,
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#project_info').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onLoadSuccess: function (node, data) {
            //树菜单展开

            var t = $(this);
            if (data) {
                $(data).each(function (index, d) {
                    if (this.state == 'closed') {
                        t.tree('expandAll');
                    }
                })
            }
            //默认选中第一个节点
            if (data.length > 0) {
                if (data[0].id != '8c20e217-5dc9-4b11-83bc-0325b47f7807') {
                    //找到第一个元素
                    var n = $('#project_info').tree('find', data[0].id);
                    //调用选中事件
                    $('#project_info').tree('select', n.target);
                }
            };


        },
        onBeforeExpand: function (node, param) {
            $('#project_info').tree('options').url = "/SystemSettings/LoadDictionaryPageTree?Id=8c20e217-5dc9-4b11-83bc-0325b47f7807&Parent_id=" + node.id;
        },
        onSelect: function () {
            var node = $('#project_info').tree('getSelected');
            if (node.text == "数据字典管理") {
                var json = {
                    "rows": [],
                    "total": 0,
                    "success": true
                };
                $('#standards_info').datagrid("loadData", json);
            } else {
                treeDatagrid();
            }
           
        }
    });
};
//添加字典内容
function insert_form() {
    $('#dictionary_dialog').dialog({
        width: 440,
        height: 350,
        modal: true,
        title: '添加字典',
        draggable: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            id: 'save'
        }, {
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#dictionary_dialog').dialog('close');
            }
        }]
    });

    //确认添加事件
    $("#save").unbind('click').bind('click', function () {
        var node = $('#project_info').tree('getSelected');
        $('#dictionary_dialog').form('submit', {
            url: "/SystemSettings/AddDicitionaryData",//接收一般处理程序返回来的json数据     
            dataType: "json",
            onSubmit: function (param) {
                param.Parent_id = node.id
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                var result = $.parseJSON(data);

                if (result.Success == true) {
                    $('#dictionary_dialog').dialog('close');
                    $('#standards_info').datagrid('reload');
                    $.messager.alert('提示', result.Message);

                } else if (result.Success == false) {
                    $.messager.alert('提示', result.Message);

                }
            }
        });
    });
    $('#dictionary_dialog').form('reset');
};
//修改字典内容
function edit_form() {
    var selectRow = $("#standards_info").datagrid("getSelected");
    if (selectRow) {
        line = $('#standards_info').datagrid("getRowIndex", selectRow);
        $('#dictionary_dialog').dialog({
            width: 440,
            height: 350,
            modal: true,
            title: '修改字典',
            draggable: true,
            buttons: [{
                text: '保存',
                iconCls: 'icon-ok',
                id: 'save1'
            }, {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#dictionary_dialog').dialog('close');
                }
            }]
        });
        //获取选中
        //var rowss = $('#standards_info').datagrid('getSelections'); //获取选中数据   
        var select = $('#standards_info').datagrid('getSelected'); //获取选中数据   

        $('#dictionary_dialog').form('load', select);//回显
        $("#save1").unbind('click').bind('click', function () {
            var node = $('#project_info').tree('getSelected');

            //form表单提交
            $('#dictionary_dialog').form('submit', {
                url: "/SystemSettings/EditDicitionaryData",//接收一般处理程序返回来的json数据     
                onSubmit: function (param) {
                    param.Parent_id = node.id;
                    param.id = select.id //rowss[0].id
                    return $(this).form('enableValidation').form('validate');
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $('#dictionary_dialog').dialog('close');
                        $.messager.alert('提示', result.Message);
                        $('#standards_info').datagrid('reload');
                    } else {
                        $.messager.alert('提示', result.Message);
                    }
                }
            });

        })
    } else {
        $.messager.alert('提示', '请先选择需要修改的行！');
    }
};
//删除内容
function delete_task() {
    var selectRow = $("#standards_info").datagrid("getSelected");//获取选中行
    var rowss = $('#standards_info').datagrid('getSelections'); //获取选中数据 
    var ids1 = [];
    for (var i = 0; i < rowss.length; i++) {
        ids1.push(rowss[i].id);
    }
    var ids = ids1.join(';');
    if (selectRow) {
        $.messager.confirm('提示', '你确定要删除此内容吗?', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: "/SystemSettings/DelDicitionaryData ",//接收一般处理程序返回来的json数据     
                    data: {
                        Parent_id: selectRow.id,
                        Project_name: selectRow.Project_name
                    },
                    type: "POST",
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $('#standards_info').datagrid('reload');
                        } else {
                            $.messager.alert('提示', result.Message);

                        }
                    }
                });
            }
        });
    } else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
};
//树加载渲染
function treeDatagrid() {
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    var node1 = $('#project_info').tree('getSelected');
    var children = $('#project_info').tree('getChildren', node1.target);//判断是否有子节点
    if (children.length == 0) {
        $('#standards_info').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            ctrlSelect: true,
            border: false,
            fit: true,
            pagination: true,
            fitColumns: true,
            pageSize: num,
            pageList: [num, num + 10, num + 20, num + 20],
            pageNumber: 1,
            type: 'POST',
            dataType: 'json',
            queryParams: {
                nodeid: node1.id
            },
            url: "/SystemSettings/LoadDicitionaryData",//接收一般处理程序返回来的json数据   
            columns: [[
                { title: '项目名称', field: 'Project_name', width: 150, sort: true },
               { title: '项目值', field: 'Project_value', width: 100 },
               { title: '序列号', field: 'Sort_num', width: 100 },
               {
                   title: '备注', field: 'remarks', width: 150, formatter: function (value, row, index) {
                       if (value) {
                           if (value.length > 30) {
                               var result = value.replace(" ", "");//去空
                               var value1 = result.substr(0, 30);
                               return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                           } else {
                               return '<span  title=' + value + '>' + value + '</span>';
                           }
                       }
                   }
               }
            ]],
            onLoadSuccess: function (data) {
                $('#standards_info').datagrid('selectRow', line);
            },
            sortOrder: 'asc',
            toolbar: "#standards_info_toolbar"
        });
    }
};
//添加同级节点
function tree_add() {
    var data = $('#project_info').tree("getSelected");
    var node = $('#project_info').tree("getParent", data.target);
    if (node != null) {
        $('#tree_add_dialog').dialog({
            title: '添加同级节点',
            width: 300,
            height: 200,
            left: 50,
            top: 100,
            buttons: [{
                text: '保存',
                iconCls: 'icon-ok',
                id: 'tree_add_save'

            }, {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#tree_add_dialog').dialog('close');
                }

            }]
        });
    } else {
        $.messager.alert('提示', "根节点不能添加同级节点");
    }
    //保存添加同级文件类型
    $('#tree_add_save').unbind('click').bind('click', function () {
        //文件类型名称不能为空
        if ($('#tree_add_dialog_input').textbox('getText') == "") {
            $.messager.alert('提示', '文件类型名称不能为空！'); return;
        };

        //获取当前节点
        var node_add = $('#project_info').tree('getSelected');

        //获取当前节点的父节点
        var node_Parent = $('#project_info').tree('getParent', node_add.target);
        var Project_name = $('#tree_add_dialog_input').textbox('getText');
        var SortNum = $('#SortNum').numberbox('getValue');
        $.ajax({
            url: "/SystemSettings/AddDicitionaryData",
            type: 'POST',
            data: {
                // ids: node_add.id,
                //发送父id
                Parent_id: node_Parent.id,
                Project_name: Project_name,
                SortNum:SortNum
            },
            success: function (data) {
                var obj = $.parseJSON(data)
                if (obj.Success == true) {
                    if (node_add) {
                        $('#project_info').tree('insert', {
                            before: node_add.target,
                            data: {
                                id: obj.Resultdata,
                                text: $('#tree_add_dialog_input').textbox('getText'),
                            }
                        });
                    }
                    $('#tree_add_dialog').dialog('close');
                    $.messager.alert('Tips', obj.Message);
                    $('#project_info').tree('options').url = "/SystemSettings/LoadDictionaryPageTree?Id=8c20e217-5dc9-4b11-83bc-0325b47f7807&Parent_id=" + node_Parent.id;
                    // $('#project_info').tree("reload");
                } else {
                    $('#tree_add_dialog').dialog('close');
                    $.messager.alert('Tips', obj.Message);
                }
            }
        })
    });

};