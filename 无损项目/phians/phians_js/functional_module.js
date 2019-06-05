$(function () {
    //页面信息
    $('#page_info').tree({
        url: "../mainform/functional_module.ashx?&cmd=load_page_tree",
        method: 'post',
        required: true,
        onContextMenu: function(e, node){  
            e.preventDefault();  
            $('#page_info').tree('select', node.target);
            $('#keyMenu').menu('show', {  
                left: e.pageX,  
                top: e.pageY
            });
        },
        //onAfterEdit: function () {
        //    var node = $('#page_info').tree('getSelected');
        //    $.ajax({
        //        url: "../mainform/functional_module.ashx?&cmd=treeedit",
        //        type: 'POST',
        //        data: {
        //            ids: node.id,
        //            Project_name: node.text
        //        },
        //        success: function (data) {
        //            if (data == 'T') {
        //                $.messager.alert('提示','编辑信息成功');
        //            }
        //        }
        //    })
        //},
        onSelect: function () {
        }
    });
    //$('#tree_add_dialog').show();
    //document.getElementById("tree_add_dialog").style.display = "none";

    //删除一个节点
    $('#tree_del').unbind('click').bind('click', function () {
        var nodes = $('#page_info').tree('getSelected');
        if (nodes) {
            $.ajax({
                url: "../mainform/functional_module.ashx?&cmd=treedel",
                type: 'POST',
                data: {
                    fid: nodes.id
                },
                success: function (data) {
                    if (data == 'T') {
                        $.messager.alert('提示', '页面删除成功！');
                        $('#page_info').tree('reload');
                    }
                },
                error: function () {
                    $.messager.alert('提示', '删除信息失败！');
                }
            })
        } else {
            $.messager.alert('提示', '请选择要操作的页面！');
        }

    });
    //编辑项目
    $('#tree_edit').unbind('click').bind('click', function () {
        var node = $('#page_info').tree('getSelected');
        if (node) {
            //$('#tree_add_dialog').dialog('reset');//重新加载一下
            $.ajax({
                url: "../mainform/functional_module.ashx?&cmd=load_page_info",
                type: 'POST',
                data: {
                    fid: node.id,
                },
                success: function (data) {
                    var strs = new Array(); //定义一数组 
                    strs = (data).split("，"); //字符分割 
                    $('#m_name').textbox('setText', strs[0]);
                    $('#i_iconCls').textbox('setText', strs[1]);
                    $('#u_url').textbox('setText', strs[2]);
                    $('#remarks').textbox('setText', strs[3]);
                }
            })

            $('#tree_add_dialog').dialog({
                title: '修改页面',
                width: 300,
                height: 350,
                left: 50,
                top: 100,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    id: 'edit_save'
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    id: 'edit_cancel'
                }]
            });
            $('#edit_save').unbind('click').bind('click', function () {
                if ($('#m_name').textbox('getText') != "") {
                    if ($('#i_iconCls').textbox('getText') != "") {
                        if ($('#u_url').textbox('getText') != "") {
                            $.ajax({
                                url: "../mainform/functional_module.ashx?&cmd=treeedit",
                                type: 'POST',
                                data: {
                                    fid: node.id,
                                    m_name: $('#m_name').textbox('getText'),
                                    remarks: $('#remarks').textbox('getText'),
                                    i_iconCls: $('#i_iconCls').textbox('getText'),
                                    u_url: $('#u_url').textbox('getText')
                                },
                                success: function (data) {
                                    if (data == 'T') {
                                        $('#tree_add_dialog').dialog('close');
                                        $.messager.alert('提示', '编辑信息成功');
                                        $('#page_info').tree('reload');
                                    }
                                }
                            })
                        } else {
                            $.messager.alert('提示', '请填写页面链接');
                        }
                    } else {
                        $.messager.alert('提示', '请填写页面图标');
                    }
                } else {
                    $.messager.alert('提示', '请填写页面名称');
                }
            })
        }
    })
    //添加同级项目
    $('#tree_add').unbind('click').bind('click', function () {
        var node_add = $('#page_info').tree('getSelected');
        $('#tree_add_dialog_input').textbox({ })

        $('#tree_add_dialog').dialog({
            title: '添加项目',
            width: 300,
            height: 350,
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
            if ($('#m_name').textbox('getText') != "") {
                if ($('#i_iconCls').textbox('getText') != "") {
                    if ($('#u_url').textbox('getText') != "") {
                        $.ajax({
                            url: "../mainform/functional_module.ashx?&cmd=treeadd",
                            type: 'POST',
                            data: {
                                fid: node_add.id,
                                m_name: $('#m_name').textbox('getText'),
                                remarks: $('#remarks').textbox('getText'),
                                i_iconCls: $('#i_iconCls').textbox('getText'),
                                u_url: $('#u_url').textbox('getText')
                            },
                            success: function (data) {
                                if (data == 'T') {
                                    $('#tree_add_dialog').dialog('close');
                                    $.messager.alert('提示', '添加信息成功');
                                    $('#page_info').tree('reload');
                                }
                            }
                        })
                    } else {
                        $.messager.alert('提示', '请填写页面链接');
                    }
                } else {
                    $.messager.alert('提示', '请填写页面图标');
                }
            } else {
                $.messager.alert('提示', '请填写页面名称');
            }
        });
        //取消添加同级项目
        $('#tree_add_cancel').unbind('click').bind('click', function () {
            $('#tree_add_dialog').dialog('close');

        });
    });
    //添加下级项目
    $('#tree_add_next').unbind('click').bind('click', function () {
        var node_add = $('#page_info').tree('getSelected');
        $('#tree_add_dialog_input').textbox({
        })

        $('#tree_add_dialog').dialog({
            title: '添加项目',
            width: 300,
            height: 350,
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
            if ($('#m_name').textbox('getText') != "") {
                if ($('#i_iconCls').textbox('getText') != "") {
                    if ($('#u_url').textbox('getText') != "") {
                        $.ajax({
                            url: "../mainform/functional_module.ashx?&cmd=treeadd_next",
                            type: 'POST',
                            data: {
                                fid: node_add.id,
                                m_name: $('#m_name').textbox('getText'),
                                remarks: $('#remarks').textbox('getText'),
                                i_iconCls: $('#i_iconCls').textbox('getText'),
                                u_url: $('#u_url').textbox('getText')
                            },
                            success: function (data) {
                                if (data == 'T') {
                                    $('#tree_add_dialog').dialog('close');
                                    $.messager.alert('提示', '添加信息成功');
                                    $('#page_info').tree('reload');
                                }
                            }
                        })
                    } else {
                        $.messager.alert('提示', '请填写页面链接');
                    }
                } else {
                    $.messager.alert('提示', '请填写页面图标');
                }
            } else {
                $.messager.alert('提示', '请填写页面名称');
            }
        });
        $('#tree_add_next_cancel').unbind('click').bind('click', function () {
            $('#tree_add_dialog').dialog('close');
        });
    })

});