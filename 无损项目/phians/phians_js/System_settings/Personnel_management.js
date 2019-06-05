//布局高宽度
$(function () {

    var tabs_width = screen.width - 182;

    //iframe可用高度
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 125;
    $('#department_layout').layout('panel', 'west').panel('resize', {
        width: 150,
        height: _height

    });
    $('#department_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 300,
        height: _height
    });
    //$('#test_layout').layout('panel', 'south').panel('resize', { height: 30 });
    $('#department_layout').layout('resize');
});

$(function () {
    var tabs_width = screen.width - 182;

    //组信息
    $('#department_info').tree({
        url: "/mainform/System_settings/Personnel_management.ashx?&cmd=loadtree",
        method: 'post',
        required: true,
        title: '部门',
        top: 0,
        fit: true,
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#department_info').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY

            });
        },
        onAfterEdit: function () {
            var node = $('#department_info').tree('getSelected');
            $.ajax({
                url: "/mainform/System_settings/Personnel_management.ashx?&cmd=edit",
                type: 'POST',
                data: {

                    ids: node.id,
                    Department_name: node.text

                },
                success: function (data) {
                    if (data == 'T') {
                        $('#department_info').tree('reload');
                        $.messager.alert('提示', '编辑信息成功');
                      
                    }
                    if (data == "内置组不允许修改") {
                        $('#department_info').tree('reload');
                        $.messager.alert('提示', '内置组不允许修改');
                       
                    }
                    else {
                        $('#department_info').tree('reload');
                        $.messager.alert('提示', '修改失败');
                      
                    }
                }

            })

        },
        onSelect: function () {
            view_technicians_info();
        }
    });

    //删除一个节点
    $('#tree_del').unbind('click').bind('click', function () {
        var nodes = $('#department_info').tree('getSelected');
        var ids = '';
        ids = nodes.id;
        if (ids > '29') {//id小于29的组都为必要组
            $.ajax({
                url: "/mainform/System_settings/Personnel_management.ashx?&cmd=treedel",
                type: 'POST',
                data: {
                    ids: ids
                },
                success: function (data) {
                    if (data == 'T') {
                        $.messager.alert('提示', '删除信息成功');
                        $('#department_info').tree('reload');
                    }
                },
                error: function () {
                    $.messager.alert('提示', '删除信息失败');
                }
            })
        } else {
            $.messager.alert('提示', '该组为不可删除项目！');
        }


    });
   
    //添加同级项目
    $('#tree_add').unbind('click').bind('click', function () {
        var node_add = $('#department_info').tree('getSelected');
        console.log(node_add);
        var node_Parent = $('#department_info').tree('getParent', node_add.target);
        console.log(node_Parent);
        $('#tree_add_dialog_input').textbox({
        })

        $('#tree_add_dialog').dialog({
            title: '添加项目',
            width: 400,
            height: 180,
            left: 50,
            top: 100,
            buttons: [{
                text: '保存',
                iconCls: 'icon-save',
                handler: function () {
                    $.ajax({
                        url: "/mainform/System_settings/Personnel_management.ashx?&cmd=treeadd",
                        type: 'POST',
                        data: {
                            // ids: node_add.id,
                            //发送父id
                            Parent_id: node_Parent.id,
                            Department_name: $('#tree_add_dialog_input').textbox('getText'),
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
                                $('#department_info').tree('reload');
                            }
                        }
                    });
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#tree_add_dialog').dialog('close');

                }
            }]

        })

    })
    //添加下级项目
    $('#tree_add_next').unbind('click').bind('click', function () {
        var node_add = $('#department_info').tree('getSelected');
        var node_Parent = $('#department_info').tree('getParent', node_add.target);
        $('#tree_add_dialog_input').textbox({
        })

        $('#tree_add_dialog').dialog({
            title: '添加项目',
            width: 400,
            height: 120,
            left: 50,
            top: 100,
            buttons: [{
                text: '保存',
                iconCls: 'icon-save',
                handler: function () {
                    $.ajax({
                        url: "/mainform/System_settings/Personnel_management.ashx?&cmd=treeadd_next",
                        type: 'POST',
                        data: {
                            ids: node_add.id,
                            //发送父id
                            //Parent_id: node_Parent.id,
                            Department_name: $('#tree_add_dialog_input').textbox('getText'),
                        },
                        success: function (data) {
                            if (data == 'T') {

                                $('#tree_add_dialog').dialog('close');
                                $.messager.alert('提示', '添加信息成功');
                                $('#department_info').tree('reload');
                            }

                        }

                    });

                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#tree_add_dialog').dialog('close');

                }
            }]

        })

    })
    //选中树项目相应加载实验资质详细信息
    function view_technicians_info() {
        var node_on = $('#department_info').tree('getSelected');
        $('#department_people').datagrid({
            url: "/mainform/System_settings/Personnel_management.ashx?&cmd=load_userlist",
            dataType: "json",
            type: 'POST',
            queryParams: {
                id: node_on.id
            }

        });
        $('#search').combobox({
            //url: 'combobox_data.json',
            //valueField: 'id',
            //textField: 'text'
            data: [
               { 'value': '0', 'text': '用户名' },
               { 'value': '1', 'text': '姓名' }

            ]

        })
        $('#search1').textbox({
            prompt: "请输入搜索内容"
        });
    }

    ///////////*******************部门信息加载处理end//////////////////////////

    //实验室员工
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    $('#department_people').datagrid(
    {
        nowrap: false,
        striped: true,
        //rownumbers: true,
        ctrlSelect: true,
        //singleSelect: true,
        //autoRowHeight: true,
        //border: false,
        fitColumns: true,
        fit: true,
        title: '员工',
        pagination: true,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num + 20],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        //url: "../mainform/technicians_info_management.ashx?&cmd=load" ,//接收一般处理程序返回来的json数据  
        columns: [[
           { field: 'User_count', title: '用户' },   
           { field: 'User_name', title: '姓名' },
           { field: 'User_sex', title: '性别' },
           { field: 'User_job', title: '职务' },
           { field: 'Phone', title: '手机' },
           { field: 'QQ', title: 'QQ' },
           { field: 'User_count_state', title: '用户状态' }
        ]],
        onSelect: function (index, row) {

            $('#department_people_info').form('reset');
            var selectRow_c2 = $("#department_people").datagrid("getSelected");
            if (selectRow_c2) {
                var rowss = $('#department_people').datagrid('getSelections');
                //$('#rg_sites').combobox("setText", rowss[0].rg_sites);
                $('#department_people_info').form('load', rowss[0]);
            }

        },
        onLoadSuccess: function (data) {
            $('#department_people').datagrid('selectRow', 0);
        },
        toolbar: department_people_toolbar
    });

    $('#search').combobox({
        //url: 'combobox_data.json',
        //valueField: 'id',
        //textField: 'text'
        data: [
            { 'value': '0', 'text': '用户名' },
            { 'value': '1', 'text': '姓名' }
        ]
    });
    $('#search1').textbox({
      //  value: '请输入搜索内容 '
    });
    //搜索
    $('#department_people_search').unbind('click').bind('click', function () {
        var search = $('#search').combobox('getText');
        var search1 = $('#search1').combobox('getText');
        switch (search) {
            case "用户名": search = "User_count"; break;
            case "姓名": search = "User_name"; break;
            default: search = "";
        }

        $('#department_people').datagrid({
            type: 'POST',
            dataType: "json",
            url: "/mainform/System_settings/Personnel_management.ashx?&cmd=department_people_search",//接收一般处理程序返回来的json数据                
            queryParams: {
                search: search,
                search1: search1
            }

        });

    });
    //查看未分配组人员
    $('#no_department_personnel').unbind('click').bind('click', function () {
        $('#department_people').datagrid({
            type: 'POST',
            dataType: "json",
            url: "/mainform/System_settings/Personnel_management.ashx?&cmd=no_department_personnel",//接收一般处理程序返回来的json数据                
        });

    });
    //启用
    $('#enable_personnel').unbind('click').bind('click', function () {
        var personnel = $("#department_people").datagrid("getSelected");
        if (personnel) {
            $.ajax({
                url: "/mainform/System_settings/Personnel_management.ashx?&cmd=enable_personnel",
                type: 'POST',
                data: {
                    id: personnel.id,
                    User_count: personnel.User_count
                },
                success: function (data) {
                    if (data == 'T') {
                        $('#department_people').datagrid('reload');
                        $.messager.alert('提示', '启用成功！');
                    }
                }

            });
        }
        else {

            $.messager.alert('提示', '请选择要操作的员工！');
        }

    });
    //停用
    $('#disable_personnel').unbind('click').bind('click', function () {
        var personnel = $("#department_people").datagrid("getSelected");
        if (personnel) {
            $.messager.confirm('确认', '您确认想要停用这个员工吗？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/mainform/System_settings/Personnel_management.ashx?&cmd=disable_personnel",
                        type: 'POST',
                        data: {
                            id: personnel.id,
                            User_count: personnel.User_count
                        },
                        success: function (data) {
                            if (data == 'T') {
                                $('#department_people').datagrid('reload');
                                $.messager.alert('提示', '停用成功！');
                            }
                        }

                    });
                }

            });
        }
        else {

            $.messager.alert('提示', '请选择要操作的员工！');
        }

    });
    //重置密码
    $('#Reset_password').unbind('click').bind('click', function () {
        var personnel = $("#department_people").datagrid("getSelected");
        if (personnel) {
            $.messager.confirm('确认', '您确认要重置这个员工的密码吗？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/mainform/System_settings/Personnel_management.ashx?&cmd=Reset_password",
                        type: 'POST',
                        data: {
                            id: personnel.id,
                            User_count: personnel.User_count
                        },
                        success: function (data) {
                            if (data == 'T') {
                                $('#department_people').datagrid('reload');
                                $.messager.alert('重置成功', '已重置密码为：123456');
                            }
                        }
                    });
                }

            });
        }
        else {

            $.messager.alert('提示', '请选择要操作的员工！');
        }

    });
    //删除员工
    $('#department_people_del').unbind('click').bind('click', function () {
        var node = $('#department_info').tree('getSelected');
        var selectRow = $("#department_people").datagrid("getSelected");
        if (selectRow) {
            $.messager.confirm('确认', '您确认想要删除这个员工吗？', function (r) {
                if (r) {
                    var cmd = "del";
                    $.ajax({
                        url: "/mainform/System_settings/Personnel_management.ashx?&cmd=technicians_del",
                        type: 'POST',
                        data: {
                            User_count: selectRow.User_count,
                            User_department: node.id
                        },
                        success: function (data) {
                            if (data == 'T') {
                                $('#department_people').datagrid('reload');
                            }
                        }
                    });
                }

            });

        }

        else {

            $.messager.alert('提示', '请选择要操作的员工！');
        }

    });

    //添加员工
    $('#department_people_add').unbind('click').bind('click', function () {
        var selectRow = $("#department_people").datagrid("getSelected");
        var node = $('#department_info').tree('getSelected');


        if (node) {
            $('#department_people_info_dialog').dialog({
                width: 500,
                height: 400,
                top: 40,
                modal: true,
                title: '添加员工',
                draggable: true,
                buttons: [{
                    text: '确定',
                    iconCls: 'icon-ok',
                    id: 'department_people_saveadd'
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#department_people_info_dialog').dialog('close');
                    }
                }]
            });

            $('#department_people_saveadd').unbind('click').bind('click', function () {
                var selecttree = $("#department_info").tree("getSelected");
                if ($("#Text12").textbox('getText') != '') {
                    if ($("#Text13").textbox('getText') != '') {
                            $('#department_people_info_dialog').form('submit', {
                                url: "Personnel_management.ashx",
                                ajax: true,
                                //额外提交参数
                                onSubmit: function (param) {
                                    param.cmd = 'technicians_saveadd';
                                    param.User_department = selecttree.id;
                                    param.Department_name = selecttree.text;

                                },
                                success: function (data) {
                                    if (data == 'T') {
                                        $.messager.alert('提示', '添加用户成功');
                                        $('#department_people').datagrid('reload');
                                        $('#department_people_info_dialog').dialog('close');
                                    }
                                }

                            });
                    } else {
                        $.messager.alert('提示', '姓名不能为空！');
                    }
                } else {
                    $.messager.alert('提示', '用户名不能为空！');
                }
            });

        } else {
            $.messager.alert('提示', '请选择要操作的部门！');

        }

    });
    //将员工指派到其它部门
    $('#add_others_department').unbind('click').bind('click', function () {
        var selectRow = $("#department_people").datagrid("getSelected");
        if (selectRow) {
            //部门信息显示
            $('#department_info_add').tree({
                url: "/mainform/System_settings/Personnel_management.ashx?&cmd=loadtree",
                method: 'post',
                required: true,
                title: '部门',
                top: 0
            });
            //清空树
            $('#project_room').tree('loadData', []);
            //已加部门加载
            $('#project_room').tree({
                url: "/mainform/System_settings/Personnel_management.ashx?&cmd=load_department",
                method: 'post',
                required: true,
                title: '部门',
                queryParams: {
                    User_count: selectRow.User_count
                },
                top: 0
            });
            $('#add_others_department_dialog').dialog({
                width: 550,
                height: 300,
                top: 40,
                modal: true,
                title: '将员工指派到其它部门',
                draggable: true,
                buttons: [{
                    text: '确定',
                    iconCls: 'icon-ok',
                    handler: function () {
                        $('#add_others_department_dialog').dialog('close');
                    }
                }]
            });

        } else {
            $.messager.alert('提示', '请选择要操作的员工！');

        }

    });

    //添加入新部门
    $('#tree_add_project').unbind('click').bind('click', function () {
        var selectRow = $("#department_people").datagrid("getSelected");
        var node = $('#department_info_add').tree('getSelected');
        $.ajax({
            url: "/mainform/System_settings/Personnel_management.ashx?&cmd=tree_add_project",
            type: 'POST',
            data: {
                User_count: selectRow.User_count,
                User_department: node.id,
                Department_name: node.text
            },
            success: function (data) {
                if (data = "T") {
                    $('#project_room').tree('reload');
                    //部门信息
                    //$('#project_room').tree({
                    //    url: "/mainform/technicians_info_management.ashx?&cmd=load_department",
                    //    method: 'post',
                    //    required: true,
                    //    title: '部门',
                    //    queryParams: {
                    //        User_count: selectRow.User_count
                    //    },
                    //    top: 0
                    //});
                }
            }
        });
    })
    // 查找已经项目，避免重复添加
    function searchTree(parentNode, searchCon) {
        var children;
        var content1 = '';
        var state2 = 0;
        for (var i = 0; i < parentNode.length; i++) { //循环顶级 node 
            if (parentNode[i].text == searchCon) {
                state2 = 1;
                break;
            }
            //content1 = content1 + parentNode[i].text;
        }
        //alert(content1)
        return state2;

    }
    //移除一个检测项目
    $('#tree_remove_project').unbind('click').bind('click', function () {
        var selectRow = $("#department_people").datagrid("getSelected");
        var remove_node = $('#project_room').tree('getSelected');
        $.ajax({
            url: "/mainform/System_settings/Personnel_management.ashx?&cmd=tree_remove_project",
            type: 'POST',
            data: {
                User_count: selectRow.User_count,
                User_department: remove_node.id
            },
            success: function (data) {
                if (data = "T") {
                    $('#project_room').tree('reload');
                    //部门信息
                    //$('#project_room').tree({
                    //    url: "/mainform/technicians_info_management.ashx?&cmd=load_department",
                    //    method: 'post',
                    //    required: true,
                    //    title: '部门',
                    //    queryParams: {
                    //        User_count: selectRow.User_count
                    //    },
                    //    top: 0
                    //});
                }
            }
        });
    });
    //部门信息
    //$('#department_info_add').tree({
    //    url: "../mainform/technicians_info_management.ashx?&cmd=loadtree",
    //    method: 'post',
    //    required: true,
    //    title: '部门',
    //    top: 0
    //});

    //查看人员信息
    $('#read_info').unbind('click').bind('click', function () {
        var selectRow = $("#department_people").datagrid("getSelected");
        var rowss = $('#department_people').datagrid('getSelections');
        if (selectRow) {
            $('#department_people_info').form('reset');
            var rowss = $('#department_people').datagrid('getSelections');
            //$('#rg_sites').combobox("setText", rowss[0].rg_sites);
            $('#department_people_info').form('load', rowss[0]);

            $('#department_people_info').dialog({
                width: 600,
                height: 350,
                top: 40,
                modal: true,
                title: '人员信息',
                draggable: true,
                buttons: [{
                    text: '确认修改',
                    iconCls: 'icon-ok',
                    handler: function () {
                        //更新人员信息
                        updateinfo();
                    }
                }, {
                    text: '关闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#department_people_info').dialog('close');
                    }
                }]
            });

        } else {
            $.messager.alert('提示', '请选择要操作的员工！');

        }

    });
    ////查看签名
    $('#read_Signature').unbind('click').bind('click', function () {
        var selectRow = $("#department_people").datagrid("getSelected");
        if (selectRow) {

            //var rowss = $('#department_people').datagrid('getSelections');
            ////$('#rg_sites').combobox("setText", rowss[0].rg_sites);                   
            //$('#upload_view').show();
            //$('#upload_org_code_img').show();
            //$('#uploadify').show();
            //$('#fileQueue').show();
            //$("#upload_org_code_img").attr("src", rowss[0].Signature);
            //$("#uploadify").uploadify({
            //    //指定swf文件
            //    swf: '/uploadify/uploadify.swf',
            //    //后台处理的页面
            //    uploader: '/mainform/System_settings/Personnel_management.ashx?&cmd=technicians_autograph',
            //    //按钮显示的文字
            //    buttonText: '修改签名',
            //    //'cancelImg': '../uploadify/uploadify-cancel.png',
            //    folder: '/upload_Folder',
            //    //fileTypeDesc: 'xx',  //过滤掉除了*.jpg,*.gif,*.png的文件
            //    'fileTypeDesc': 'All Files',
            //    'fileTypeExts': '*.jpg;*.png;*.gif',
            //    queueID: 'fileQueue',
            //    sizeLimit: '2048000',                         //最大允许的文件大小为2M
            //    auto: true,                                  //需要手动的提交申请
            //    multi: false,                                //一次只允许上传一张图片
            //    onUploadStart: function (file) {
            //        var ids = {}
            //        var selectRow_c2 = $("#department_people").datagrid("getSelected");
            //        ids.ids = selectRow_c2.id;
            //        $("#uploadify").uploadify('settings', 'formData', ids);
            //    },
            //    onUploadSuccess: function (file, data, response) {

            //        $("#upload_org_code_img").attr("src", "/upload_Folder/" + data);

            //    }
            //    //,
            //    //onFallback: function () { //Flash无法加载错误
            //    //    alert('提示', "您未安装FLASH控件，无法上传！请安装FLASH控件后再试。");
            //    //},
            //    //onSelectError: function (file, errorCode) {  //选择文件出错
            //    //    alert('提示', errorCode);
            //    //},
            //    //onUploadError: function (file, errorCode, errorMsg) { //上传失败
            //    //    alert('提示', file.name + "上传失败，</br>错误信息：" + errorMsg);

            //    //}
            //});
            $('#Signature_form').dialog({
                width: 400,
                height: 350,
                top: 40,
                modal: true,
                title: '签名图片',
                draggable: true,
                buttons: [ {
                    text: '关闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#Signature_form').dialog('close');
                    }
                }]
            });
            $("#upload_org_code_img").attr("src", selectRow.Signature);
            $('#save_image').unbind('click').bind('click', function () {
                save_image();
            });
            function save_image() {

                var old_url = $('#upload_org_code_img').val();
                $('#Signature_form').form('submit', {
                    url: "Personnel_management.ashx",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.cmd = 'technicians_autograph';
                        param.ids = selectRow.id;
                    },
                    success: function (data) {
                        if (data == '请上传图片') {
                            $.messager.alert('提示', '请上传图片格式签名！');

                        } else if (data == '请选择签名') {
                            $.messager.alert('提示', '请选择签名上传！');
                        } else if (data != 'F') {
                            $("#upload_org_code_img").attr("src", data);
                        }
                        else {

                            $.messager.alert('提示', '添加信息失败');
                        }


                    }
                });

            }
        } else {
            $.messager.alert('提示', '请选择要操作的员工！');

        }

    });

    ///////////*******************实验室员工加载等处理end//////////////////////////

});
//更新员工
function updateinfo() {
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('更新信息提示', '您确认想要更新这个员工信息吗？', function (r) {
            if (r) {
                var selectRow = $("#department_people").datagrid("getSelected");
                $('#department_people_info').form('submit', {
                    url: "/mainform/System_settings/Personnel_management.ashx",
                    ajax: true,
                    //额外提交参数
                    onSubmit: function (param) {
                        param.cmd = 'technicians_save';
                        param.ids = selectRow.id;
                    },
                    //error:function (XMLHttpRequest, textStatus, errorThrown) {
                    //    // 通常情况下textStatus和errorThown只有其中一个有值 
                    //    this; // the options for this ajax request
                    //},
                    success: function (data) {
                        if (data == 'T') {

                            $.messager.alert('提示', '更新信息成功');
                            $('#department_people_info').dialog('close');
                            $('#department_people').datagrid('reload');
                        }
                    }

                });

            }

        });
    } else {
        $.messager.alert('提示', '请选择要操作的员工');
    }
}
