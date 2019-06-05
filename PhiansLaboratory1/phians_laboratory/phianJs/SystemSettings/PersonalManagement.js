var line = 0;//定义列表的行
var word_link;
var WordUrlSpit = new Array();
$(function () {
    var tabs_width = screen.width - 182;
    //iframe可用高度
    var _height = $(".tab-content").height();
    //设置左边树的样式大小
    $('#department_layout').layout('panel', 'west').panel('resize', {
        width: 300,
        height: _height
    });
    //设置右边列表的样式大小
    $('#department_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 300,
        //height: _height
    });
    $('#department_layout').layout('resize');//页面重置，初始化
    //n男 0
    $('#UserNsex1').unbind('click').bind('click', function () {
        $("#UserNsex1").prop("checked", "checked");
        $("#UserNsex2").prop("checked", false);
    
        
       
        document.getElementById('UserNsex').value = 'true';
      
       
    });
    //女 1
    $('#UserNsex2').unbind('click').bind('click', function () {
        $("#UserNsex2").prop("checked", "checked");
        $("#UserNsex1").prop("checked", false);
     
        document.getElementById('UserNsex').value = 'false';
           
    });
    //是否是组长 0
    $('#GroupLeader2').unbind('click').bind('click', function () {
        $("#GroupLeader2").prop("checked", "checked");
        $("#GroupLeader1").prop("checked", false);
        $("#GroupLeader").val("false");
    });
    //是否是组长 1
    $('#GroupLeader1').unbind('click').bind('click', function () {
        $("#GroupLeader1").prop("checked", "checked");
        $("#GroupLeader2").prop("checked", false);
        $("#GroupLeader").val("true");
    });
    //组管理
    $("#group_add").unbind("click").bind("click", function () {
        groupInfo();
    });
    //NodeType 节点类型
    $("#NodeType").combobox({
        data: [
           { 'value': '0', 'text': '单位' },
           { 'value': '1', 'text': '系' }
        ]
    });
    //证书是否失效下拉框
    $("#CertificateSate").combobox({
        panelHeight: 50,
        editable: false,
        data: [
           { 'value': '0', 'text': '无效' },
           { 'value': '1', 'text': '有效' }
        ]
    });
    //BU下拉框
    $("#BU").combobox({
        url: "/Common/GetDictionaryList",
        valueField: 'Value',
        textField: 'Text',
        panelHeight: 100,
        queryParams: {
            "Parent_id": 'D23A5768-285A-41A7-8164-D17E879FF19E',
        },

        editable: false,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    department_tree_init();//******************************************************组织架构管理树初始化  
    word_link = $("#ShowOffice").attr("href");//获取office控件链接 URL
    WordUrlSpit = word_link.split("?");
    //WordUrlSpit[0]头部
    //WordUrlSpit[1]尾部
    $('#search').combobox({
        data: [
            { 'value': 'UI.UserCount', 'text': '用户账号' },
            { 'value': 'UI.UserName', 'text': '用户名' }
        ]
    });
    $('#Job').combobox({
        data: [
          { 'value': '技术员Ⅰ', 'text': '技术员Ⅰ' },
          { 'value': '技术员Ⅱ', 'text': '技术员Ⅱ' },
          { 'value': '技术员', 'text': '技术员' },
          { 'value': '高级技术员', 'text': '高级技术员' },
          { 'value': '工程师', 'text': '工程师' },
          { 'value': '高级工程师', 'text': '高级工程师' },
          { 'value': '专家Ⅰ', 'text': '专家Ⅰ' },
          { 'value': '专家Ⅱ', 'text': '专家Ⅱ' }
        ]
    });
    
    $('#search1').textbox({
        value: ''
    });
  
});

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
//********************************************************************组织架构管理树初始化********************************************************************
//********************************************************************组织架构管理树
function department_tree_init() {
    $('#department_info').tree({
        url: '/SystemSettings/GetGroupTree',
        method: 'post',
        required: true,
        top: 0,
        fit: true,
        onContextMenu: function (e, node) {//右击菜单栏操作
            e.preventDefault(); $('#department_info').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onBeforeExpand: function (node, param) {//树节点展开
            $('#department_info').tree('options').url = "/SystemSettings/GetGroupTree?ParentId=" + node.id;
        },
        onSelect: function () {
            //渲染右边列表
            treeDatagrid();
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
            };
            //默认选中第一个节点
            if (data.length > 0) {
                //找到第一个元素
                var n = $('#department_info').tree('find', data[0].id);
                //调用选中事件
                $('#department_info').tree('select', n.target);
            };
        }
    });
    ////编辑部门管理节点
    //$('#tree_edit1').unbind('click').bind('click', function () {
    //    var node = $('#department_info').tree('getSelected');
    //    if (node) {
    //        editDepartment(node);
    //    }
    //});
    ////添加部门管理下级节点
    //$('#tree_add_next1').unbind('click').bind('click', function () {
    //    addDepartment();
    //});
    ////删除部门管理节点
    //$('#tree_del1').unbind('click').bind('click', function () {
    //    deleteDepartment();
    //});

    //删除一个节点
    $('#tree_del').unbind('click').bind('click', function () {
        deleteInfo();
    });
    //编辑项目
    $('#tree_edit').unbind('click').bind('click', function () {
        var node = $('#department_info').tree('getSelected');
        if (node) {
            editInfo(node);
        } else {
            $.messager.alert('提示', '请选择要操作的树！');
        }
    });
    //添加下级项目
    $('#tree_add_next').unbind('click').bind('click', function () {
        addInfo();
    });
};
//********************************************************************编辑部门管理树节点
function editDepartment(node) {
    $('#tree_addDepartment_dialog').dialog({
        title: 'Edit Info',
        width: 500,
        height: 350,
        left: 50,
        top: 100,
        modal: true,
        draggable: true,
        beforeSubmit: function (formData, jqForm, options) {//提交前的回调方法
            return $("#tree_addDepartment_dialog").form('validate');
        },
        onOpen: function () {
            $.ajax({
                url: "/SystemSettings/LoadDepartmentInfo",
                type: 'POST',
                data: {
                    NodeId: node.id,
                },
                success: function (data) {
                    var obj = $.parseJSON(data);
                    $('#tree_addDepartment_dialog').form('load', obj.Data);// form编辑数据回显
                }
            });
        },
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                $('#tree_addDepartment_dialog').form('submit', {
                    url: "/SystemSettings/EditDepartment",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.NodeId = node.id
                        return $(this).form('enableValidation').form('validate');//验证表单
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            var node1 = $('#department_info').tree('getSelected');
                            if (node1) {
                                $('#department_info').tree('update', {
                                    target: node.target,
                                    text: $('#NodeName').textbox('getText')
                                });
                            }
                            $('#tree_addDepartment_dialog').dialog('close');//关闭编辑部门管理树节点弹窗
                            $.messager.alert('提示', result.Message);
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        }, {
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#tree_addDepartment_dialog').dialog('close');//关闭编辑部门管理树节点弹窗
            }
        }]
    });
};
//********************************************************************添加部门管理树下级节点
function addDepartment() {
    var node_add = $('#department_info').tree('getSelected');
    var node_Parent = $('#department_info').tree('getParent', node_add.target);
    $('#tree_addDepartment_dialog').dialog({
        title: 'Add Info',
        width: 500,
        height: 350,
        left: 50,
        top: 100,
        modal: true,
        draggable: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                $('#tree_addDepartment_dialog').form('submit', {
                    url: "/SystemSettings/AddDepartment",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.ParentId = node_add.id
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //更新当前选中的节点
                            var node1 = $('#department_info').tree('getSelected');
                            if (node1) {
                                $('#department_info').tree('append', {
                                    parent: node1.target,
                                    data: [{
                                        id: result.Data.NodeId,
                                       // NodeId: result.Data.NodeId,
                                        NodeParent: result.Data.NodeParent,
                                        text: $("#NodeName").textbox("getText")
                                    }]
                                });
                            }
                            //选择当前添加节点
                            //var node = $('#department_info').tree('find', result.Message);
                            //$('#department_info').tree('select', node.target);
                            $('#tree_addDepartment_dialog').dialog('close');//关闭添加部门管理树下级节点弹窗
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        }, {
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#tree_addDepartment_dialog').dialog('close');//关闭添加部门管理树下级节点弹窗
            }
        }]
    });
    $('#tree_addDepartment_dialog').form('reset');//重置添加部门管理树下级节点弹窗
};
//********************************************************************删除部门管理树节点
function deleteDepartment() {
    var nodes = $('#department_info').tree('getSelected');//获取选中树节点的信息
    if (nodes) {//id小于29的组都为必要组
        $.messager.confirm('提示', '确认操作？', function (r) {
            if (r) {
                $.ajax({
                    url: "/SystemSettings/DelDepartment",
                    type: 'POST',
                    data: {
                        NodeId: nodes.id
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //选择当前需要操作的节点
                            $('#department_info').tree('remove', nodes.target);//删除当前选中的树节点
                            $.messager.alert('提示', result.Message);
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        })
      
    } else {
        $.messager.alert('提示', '请先选择要操作的部门！');
    }
};


//********************************************************************人员详细信息列表初始化********************************************************************
//********************************************************************人员详细信息列表
function treeDatagrid() {
    //显示分页条数
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    var node = $('#department_info').tree('getSelected');//获取选中树节点的信息
    $('#department_people').datagrid(
    {
        border: true,
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        fit: true,
        fitColumns:true,
        rownumbers: true,
        pagination: true,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num + 20],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/SystemSettings/GetgroupPersonnelList",//接收一般处理程序返回来的json数据  
        queryParams: {
            GroupId: node.id//获取树节点的id传给后台
        },
        columns: [[
            { field: 'UserCount', title: '用户账号', width: 100, sortable: 'true' },
           { field: 'UserName', title: '用户名', width: 100, sortable: 'true' },
           {
               field: 'UserNsex', title: '性别', width: 100, formatter: function (value, row, index) {
                   if (value == true) {
                       return "man"
                   }
                   if (value == false) {
                       return "women"
                   }
               }
           },
           { field: 'JobNum', title: '工号', width: 100, sortable: 'true' },
            { field: 'BU', title: 'BU', width: 100 },
           { field: 'Job', title: '职位', width: 100, sortable: 'true' },
           //{ field: 'Profession', title: 'Profession', width: 100, sortable: 'true' },
           { field: 'Tel', title: '电话', width: 100, sortable: 'true' },
           { field: 'Phone', title: '手机', width: 100, sortable: 'true' },
           { field: 'Fax', title: '传真', width: 50, sortable: 'true', hidden: true },
           { field: 'Email', title: '邮箱', width: 100, sortable: 'true' },
           { field: 'Postcode', title: '邮编', width: 100, sortable: 'true', hidden: true },
           { field: 'QQ', title: 'QQ', width: 100, hidden: true },
           { field: 'Address', title: '地址', width: 100, sortable: 'true', hidden: true },
           //{ field: 'sort_num', title: 'Sort Num', width: 100 },
           { field: 'CreatePersonnel_n', title: '创建人', width: 120 },
           {
               field: 'CreateDate', title: '创建时间', width: 100, formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           {
               field: 'CountState', title: '状态', width: 100, formatter: function (value, row, index) {
                   if (value == 1) {
                       return "在用"
                   } else if (value == 0) {
                       return "停用"
                   }
               }
           },
           {
               field: 'Remarks', title: '备注', width: 150, formatter: function (value, row, index) {
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
        sortName: 'UserCount',
        sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#department_people').datagrid('selectRow', line);
        },
        rowStyler: function (index, row) {
            if (row.CountState == 0) {
                return 'color:#f00';
            }
        },
        toolbar: department_people_toolbar
    });
    //添加员工
    $('#department_people_add').unbind('click').bind('click', function () {
        department_people_add(node);
    });
    //编辑员工
    $('#read_info').unbind('click').bind('click', function () {
        read_info();
    });
    //查看证书
    $('#read_certificate').unbind('click').bind('click', function () {
        read_certificate();
    });
    //搜索
    $('#department_people_search').unbind('click').bind('click', function () {
        department_people_search();
    });
  
    //停用
    $('#disable_personnel').unbind('click').bind('click', function () {
        disable_init();
    });
    //重置密码
    $('#Reset_password').unbind('click').bind('click', function () {
        ResetPassword();
    });
    ////查看签名
    $('#read_Signature').unbind('click').bind('click', function () {
        ReadSignature();
    });
    ////查看头像
    $('#read_photo_Signature').unbind('click').bind('click', function () {
        ReadPhotoSignature();
    });
    //将员工指派到其它部门
    $('#add_others_department').unbind('click').bind('click', function () {
        AddOthersDepartment();

    });
    //启用CNAS授权
    $('#Start_CNAS_Authority').unbind('click').bind('click', function () {
        CNAS_Authority("1");

    });
    //取消CNAS授权
    $('#Stop_CNAS_Authority').unbind('click').bind('click', function () {
        CNAS_Authority("0");

    });
    //查看CNAS授权
    $('#view_CNAS_Authority').unbind('click').bind('click', function () {
        View_CNAS_Authority();

    });
    //查看已经停用的人员
    $('#view_stop').unbind('click').bind('click', function () {
        viewStop();

    });
    //查看删除的人员
    $('#del_person').unbind('click').bind('click', function () {
        delPerson();

    });
};
/*
*functionName:delPerson
*function:删除人员
*Param
*author:张慧敏
*date:2018-06-20
*/
function delPerson() {
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('提示', '是否确认删除该人员？', function (r) {
            if (r) {
                $.ajax({
                    url: "/SystemSettings/DeletePerson",
                    type: 'post',
                    data: {
                        UserId: selectRow.UserId
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $('#department_people').datagrid('reload');
                        }
                        else if (result.Success == false) {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                })
            }
        });
    }
    else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
};
/*
*functionName:viewStop
*function:查看已经停用的人员
*Param :
*author:张慧敏
*/
function viewStop() {
    $('#view_Stop_dialog').dialog({
        width: 800,
        height: 500,
        modal: true,
        title: "查看停用人员",
        // fit: true,
        draggable: true,
        buttons: [{
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#view_Stop_dialog').dialog('close');
            }
        }]
    });
    $('#Stop_datagrid').datagrid({
        url: "/SystemSettings/GetCancelPerson",
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        pagination: true,
        fitColumns: true,
        fit: true,
        //queryParams: {
        //    UserId: row.UserId,
        //},
        pageSize: 10,
        pageList: [10, 20, 30, 40],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        columns: [[
            { field: 'UserCount', title: '用户账号', width: 100, sortable: 'true' },
           { field: 'UserName', title: '用户名', width: 100, sortable: 'true' },
           {
               field: 'UserNsex', title: '性别', width: 100, formatter: function (value, row, index) {
                   if (value == true) {
                       return "man"
                   }
                   if (value == false) {
                       return "women"
                   }
               }
           },
           { field: 'JobNum', title: '工号', width: 100, sortable: 'true' },
            { field: 'BU', title: 'BU', width: 100 },
           { field: 'Job', title: '职位', width: 100, sortable: 'true' },
           //{ field: 'Profession', title: 'Profession', width: 100, sortable: 'true' },
           { field: 'Tel', title: '电话', width: 100, sortable: 'true' },
           { field: 'Phone', title: '手机', width: 100, sortable: 'true' },
           { field: 'Fax', title: '传真', width: 50, sortable: 'true', hidden: true },
           { field: 'Email', title: '邮箱', width: 100, sortable: 'true' },
           { field: 'Postcode', title: '邮编', width: 100, sortable: 'true', hidden: true },
           { field: 'QQ', title: 'QQ', width: 100, hidden: true },
           { field: 'Address', title: '地址', width: 100, sortable: 'true', hidden: true },
           //{ field: 'sort_num', title: 'Sort Num', width: 100 },
           { field: 'CreatePersonnel_n', title: '创建人', width: 120 },
           {
               field: 'CreateDate', title: '创建时间', width: 100, formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           {
               field: 'CountState', title: '状态', width: 100, formatter: function (value, row, index) {
                   if (value == 1) {
                       return "在用"
                   } else if (value == 0) {
                       return "停用"
                   }
               }
           },
           {
               field: 'Remarks', title: '备注', width: 150, formatter: function (value, row, index) {
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
        sortName: 'UserCount',
        sortOrder: 'asc',
        onLoadSuccess: function (data) {
            $('#Stop_datagrid').datagrid('selectRow', 0);
        },
        toolbar: view_Stop_toolbar
    });
    //启用
    $('#enable_personnel').unbind('click').bind('click', function () {
        enable_init();
    });
}
//*******************************************************************启用
function enable_init() {
    var personnel = $("#Stop_datagrid").datagrid("getSelected");
    line = $('#Stop_datagrid').datagrid("getRowIndex", personnel);
    if (personnel) {
        $.ajax({
            url: "/SystemSettings/DelPersonnel",
            type: 'POST',
            data: {
                UserId: personnel.UserId,
                flag: "1",
                UserName: personnel.UserName
            },
            success: function (data) {
                var result = $.parseJSON(data);

                if (result.Success == true) {
                    $('#Stop_datagrid').datagrid('reload');
                    $('#department_people').datagrid('reload');
                    $.messager.alert('提示', result.Message);
                }
                else {

                    $.messager.alert('提示', result.Message);
                }
            }
        });
    }
    else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
}
//********************************************************************添加员工dialog********************************************************************
function department_people_add(node) {
    $("#UserCount").textbox({
        readonly: false
    });
    $(".LeaveDate").css("display", "none");
    if (node) {
        $('#department_people_info_dialog').dialog({
            width: 550,
            height: 400,
            modal: true,
            title: '添加人员',
            draggable: true,
            buttons: [{
                text: '保存',
                iconCls: 'icon-ok',
                handler: function () {
                    department_peopleSave(node);//确认添加员工
                }
            }, {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#department_people_info_dialog').dialog('close');//关闭添加员工dialog
                }
            }]
        });
    } else {
        $.messager.alert('提示', '请先选择部门再进行添加人员!');
    }
    $('#department_people_info_dialog').form('reset');
};
//********************************************************************确认添加员工
function department_peopleSave(node) {
    $('#department_people_info_dialog').form('submit', {
        url: "/SystemSettings/AddPersonnel",
        ajax: true,
        //额外提交参数
        onSubmit: function (param) {
            param.NodeId = node.id;
            param.Group_id = node.id_;
            return $(this).form('enableValidation').form('validate');
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('提示', result.Message);
                    $('#department_people').datagrid('reload');//重新加载员工列表
                    $('#department_people_info_dialog').dialog('close');//关闭添加员工dialog
                } else {
                    $.messager.alert('提示', result.Message);
                }
            }

        }
    });
};
//********************************************************************修改人员信息dialog********************************************************************
function read_info() {
    $("#UserCount").textbox({
        readonly: true
    });
    $(".LeaveDate").css("display", "block");
    var selectRow = $("#department_people").datagrid("getSelected");//获取选中人员信息行
    line = $('#department_people').datagrid("getRowIndex", selectRow);
    if (selectRow) {
        $('#department_people_info_dialog').form('reset');
        var rowss = $('#department_people').datagrid('getSelections');
        $('#department_people_info_dialog').form('load', rowss[0]);
        //n男 1
        if (rowss[0].UserNsex == true) {
            $("#UserNsex1").prop("checked", "checked");
            $("#UserNsex2").prop("checked", false);

            document.getElementById('UserNsex').value = 'true';
        } else {
            $("#UserNsex2").prop("checked", "checked");
            $("#UserNsex1").prop("checked", false);
            document.getElementById('UserNsex').value = 'false';
           
        }
        $('#department_people_info_dialog').dialog({
            width: 550,
            height: 400,
            modal: true,
            title: '修改人员',
            draggable: true,
            buttons: [{
                text: '保存',
                iconCls: 'icon-ok',
                handler: function () {
                    updateinfo(selectRow); //修改员工信息
                }
            }, {
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#department_people_info_dialog').dialog('close');//关闭修改人员信息dialog
                }
            }]
        });

    } else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
};
//********************************************************************确认修改员工信息
function updateinfo(selectRow) {
    if (selectRow) {
        $.messager.confirm('提示', '确实要更新此雇员信息吗？', function (r) {
            if (r) {
                $('#department_people_info_dialog').form('submit', {
                    url: "/SystemSettings/EditPersonnel",
                    ajax: true,
                    onSubmit: function (param) { //额外提交参数
                        param.id = selectRow.id;
                        param.UserId = selectRow.UserId;
                        return $(this).form('enableValidation').form('validate');//验证修改的表单
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $('#department_people').datagrid('reload');//重新加载人员列表信息
                            $('#department_people_info_dialog').dialog('close');//关闭修改人员信息dialog
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        });
    } else {
        $.messager.alert('提示', '修改人员信息成功!');
    }
};
//*******************************************************************部门人员搜索
function department_people_search() {
    var nodes = $('#department_info').tree('getSelected');//获取选中树节点的信息
    var search = $('#search').combobox('getValue');
    var search1 = $('#search1').combobox('getText');
    $('#department_people').datagrid({
        type: 'POST',
        dataType: "json",
        url: "/SystemSettings/GetDepPerList",//接收一般处理程序返回来的json数据                
        queryParams: {
            NodeId: nodes.id,
            search: search,
            key: search1
        }
    });
}

//*******************************************************************停用
function disable_init() {
    var personnel = $("#department_people").datagrid("getSelected");
    line = $('#department_people').datagrid("getRowIndex", personnel);
    if (personnel) {
        $.messager.confirm('提示', '是否要停用该员工吗？', function (r) {
            if (r) {
                $.ajax({
                    url: "/SystemSettings/DelPersonnel",
                    type: 'POST',
                    data: {
                        UserId: personnel.UserId,
                        flag: "0",
                        UserName: personnel.UserName
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);

                        if (result.Success == true) {
                            $('#department_people').datagrid('reload');
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
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
}
//*******************************************************************重置密码
function ResetPassword() {
    var personnel = $("#department_people").datagrid("getSelected");
    if (personnel) {
        $.messager.confirm('提示', "是否要重置该员工的密码？", function (r) {
            if (r) {
                $.ajax({
                    url: "/SystemSettings/ResetPerPwd",
                    type: 'POST',
                    data: {
                        UserId: personnel.UserId,
                        UserName: personnel.UserName
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
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
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
}
//*******************************************************************查看签名
function ReadSignature() {
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        $('#Signature_form').dialog({
            width: 400,
            height: 300,
            modal: true,
            title: '查看签名',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Signature_form').dialog('close');
                }
            }]
        });
       // $('#Signature_form').form("reset");
        $("#upload_org_code_img").prop("src", selectRow.Signature);
        $('#save_image').unbind('click').bind('click', function () {
            save_image();
        });
     
        function save_image() {
            $('#Signature_form').form('submit', {
                url: "/SystemSettings/UpdateUserImg",
                onSubmit: function (param) {
                    param.UserName = selectRow.UserName;
                    param.UserId = selectRow.UserId;
                },
                success: function (data) {
                    if (data) {
                        var obj = $.parseJSON(data);
                        if (obj.Success == true) {
                            $("#upload_org_code_img").prop("src", obj.Data.Signature);
                            $('#department_people').datagrid('reload');
                        } else {
                            $.messager.alert('提示', obj.Message);
                        }
                    }
                }
            });

        }
    } else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');

    }

}
//*******************************************************************查看头像
function ReadPhotoSignature() {
    var selectRow = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        $('#Signature_photo_form').dialog({
            width: 400,
            height: 300,
            modal: true,
            title: '查看照片',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Signature_photo_form').dialog('close');
                }
            }]
        });
        $("#upload_org_code_img1").prop("src", selectRow.Portrait);
        $('#save_photo_image').unbind('click').bind('click', function () {
            save_photo_image();
        });
        $('#Signature_photo_form').form("reset");

        function save_photo_image() {
            $('#Signature_photo_form').form('submit', {
                url: "/SystemSettings/UploadUserPortrait",
                onSubmit: function (param) {
                    param.UserName = selectRow.UserName;
                    param.UserId = selectRow.UserId;

                },
                success: function (data) {
                    if (data) {
                        var obj = $.parseJSON(data);
                        if (obj.Success == true) {
                            $("#upload_org_code_img1").attr("src", obj.Data.Portrait);
                            $('#department_people').datagrid('reload');
                        } else {
                            $.messager.alert('提示', obj.Message);
                        }
                    }
                }
            });
        }
    } else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');

    }
}

/*
*functionName:CNAS_Authority_true
*function:函数功能 CNAS_Authority授权
*Param 参数 flag 1 授权 0 取消授权
*author:创建人 张慧敏
*date:时间
*/
function CNAS_Authority(flag) {
    var personnel = $("#department_people").datagrid("getSelected");
    line = $('#department_people').datagrid("getRowIndex", personnel);
    if (personnel) {
        $.ajax({
            url: "/SystemSettings/CNASAuthorization",
            type: 'POST',
            data: {
                UserId: personnel.UserId,
                Operatetype: flag
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#department_people').datagrid('reload');
                    $.messager.alert('提示', result.Message);
                }
                else {

                    $.messager.alert('提示', result.Message);
                }
            }
        });
    }
    else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
}
/*
*functionName:view_CNAS_Authority
*function:函数功能 view_CNAS_Authority查看
*Param 参数
*author:创建人 张慧敏
*date:时间
*/
function View_CNAS_Authority() {
    $('#view_CNAS_dialog').dialog({
        width: 800,
        height: 430,
        modal: true,
        title: "查看CNAS",
        // fit: true,
        draggable: true,
        buttons: [{
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#view_CNAS_dialog').dialog('close');
            }
        }]
    });
    $('#CNAS_datagrid').datagrid({
        url: "/SystemSettings/GetCNASAuthorizationList",
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        pagination: true,
        fitColumns: true,
        fit: true,
        //queryParams: {
        //    UserId: row.UserId,
        //},
        pageSize: 10,
        pageList: [10, 20, 30, 40],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        columns: [[
          // { field: 'ID', title: 'ID', width: 100 },
           { field: 'UserCount', title: '用户账号', width: 100, sortable: 'true' },
           { field: 'UserName', title: '用户名', width: 100, sortable: 'true' },
           { field: 'CNASLogo', title: 'CNAS标志', width: 50 }
        ]],
        onLoadSuccess: function (data) {
            $('#CNAS_datagrid').datagrid('selectRow', 0);
        },
        toolbar: CNAS_toolbar
    });
    //搜索combobox
    $("#CNAS_search").combobox({
        data: [
            { 'value': 'UserCount', 'text': '用户账号' },
            { 'value': 'UserName', 'text': '用户名' }
        ]

    });
    //搜索CNAS授权
    $('#CNAS_datagrid_search').unbind('click').bind('click', function () {
        CNAS_datagrid_search();
    });
}
/*
*functionName:CNAS_datagrid_search
*function:函数功能 搜索CNAS
*Param 参数
*author:创建人 张慧敏
*date:时间
*/
function CNAS_datagrid_search() {
    $('#CNAS_datagrid').datagrid({
        url: "/SystemSettings/GetCNASAuthorizationList",
        queryParams: {
            search: $("#CNAS_search").combobox("getValue"),
            key: $("#CNAS_search1").textbox("getText")
        },
    });
}
//**********************************************************************将员工指派到其它部门
function AddOthersDepartment() {
    var selectRow_people = $("#department_people").datagrid("getSelected");
    //if (selectRow_people) {
        $('#department_info_add').tree({
            url: '/SystemSettings/GetGroupTree',
            method: 'post',
            required: true,
            title: '组人员授权',
            top: 0,
            fit: true,
            onBeforeExpand: function (node, param) {//树节点展开
                $('#department_info_add').tree('options').url = "/SystemSettings/GetGroupTree?ParentId=" + node.id;
            },
            onSelect: function () {
                //渲染右边列表
                var node = $('#department_info_add').tree('getSelected');
                $('#Auth_person').datagrid({
                    queryParams: {
                        GroupId: node.id,
                    },
                    url: "/SystemSettings/GetgroupPersonnelList"

                });
            }
        });
      
        //人员搜索
        $("#person_info_search").combobox({
            data: [
               { 'value': 'UI.UserCount', 'text': '用户账号' },
               { 'value': 'UI.UserName', 'text': '用户名' }
            ]
        });
        var node = $('#department_info_add').tree('getSelected');
        //显示分页条数
        var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
        var num = parseInt(_height1 / 25);
        $('#person_info_add').datagrid({
            url: "/SystemSettings/GetPersonnelList",
            nowrap: false,
            striped: true,
            ctrlSelect: true,
            pagination: true,
            fitColumns: true,
            title: "添加组员",
            height: 700,
            queryParams: {
                search: $("#person_info_search").combobox("getValue"),
                key: $("#person_info_search1").textbox("getText"),
            },
            pageSize: num,
            pageList: [num , num+10,num + 20,num + 30,num + 40],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            columns: [[
               { field: 'UserCount', title: '用户账号', width: 50 },
               { field: 'UserName', title: '用户名', width: 50 }
            ]],

            onLoadSuccess: function (data) {
                $('#person_info_add').datagrid('selectRow', 0);
            },
           
            toolbar: person_info_toolbar
        });
        //定义pagination加载内容
        var p1_person_info_add = $('#person_info_add').datagrid('getPager');
        (p1_person_info_add).pagination({
            layout: ['first', 'prev', 'last', 'next']

        });
        //****************人员搜索
        $("#person_search").unbind("click").bind("click", function () {
            Person_search();
        });
        //人员搜索
        function Person_search() {
            $('#person_info_add').datagrid({
                url: "/SystemSettings/GetPersonnelList",
                queryParams: {
                    search: $("#person_info_search").combobox("getValue"),
                    key: $("#person_info_search1").textbox("getText"),
                },
            });
            //定义pagination加载内容
            var p1_person_info_add1 = $('#person_info_add').datagrid('getPager');
            (p1_person_info_add1).pagination({
                layout: ['first', 'prev', 'last', 'next']

            });
        }
        //已加部门加载初始化
        var node = $('#department_info_add').tree('getSelected');
        //显示分页条数
        var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
        var num = parseInt(_height1 / 25);
        $('#Auth_person').datagrid({
            //  url: "/SystemSettings/GetPerGroupTree",
            nowrap: false,
            striped: true,
            ctrlSelect: true,
            pagination: true,
            fitColumns: true,
            title: "已授权人员",
            height: 700,
            //queryParams: {
            //    UserId: selectRow_people.UserId,
            //},
            pageSize: num,
            pageList: [num, num + 10, num + 20, num + 30],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            columns: [[
               { field: 'UserCount', title: '用户账号', width: 50 },
               { field: 'UserName', title: '用户名', width: 50 },
            //{ field: 'GroupLeader', title: '是否为组长', width: 50 }
            ]],
            onLoadSuccess: function (data) {
                $('#Auth_person').datagrid('selectRow', 0);
            }
        });
        //定义pagination加载内容
        var p1_project_room = $('#Auth_person').datagrid('getPager');
        (p1_project_room).pagination({
            layout: ['first', 'prev', 'last', 'next']

        });
        $('#add_others_department_dialog').dialog({
            width: 800,
            height: 430,
            modal: true,
            title: '组人员授权',
            fit: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#add_others_department_dialog').dialog('close');
                }
            }]
        });

    //}
    //添加入新部门
    $('#tree_add_project').unbind('click').bind('click', function () {
        AddGroupPerson();
    });
    //移除一个部门
    $('#tree_remove_project').unbind('click').bind('click', function () {
        RemoveGroupPerson();
    });
    //查看人所在组
    $('#view_group').unbind('click').bind('click', function () {
        View_Group();
    });

}

/*
*functionName:View_Group
*function:函数功能 查看人所在组
*Param 参数
*author:创建人 张慧敏
*date:时间
*/
function View_Group() {
    $('#View_Group_dialog').dialog({
        width: 800,
        height: 430,
        modal: true,
        title: "组信息",
       // fit: true,
        draggable: true,
        buttons: [{
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#View_Group_dialog').dialog('close');
            }
        }]
    });
    var row = $('#person_info_add').datagrid("getSelected");
    $('#project_room').datagrid({
         url: "/SystemSettings/GetPerGroupTree",
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        pagination: true,
        fitColumns: true,
      
        fit:true,
        queryParams: {
            UserId: row.UserId,
        },
        pageSize: 10,
        pageList: [10, 20, 30, 40],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        columns: [[
           //{ field: 'UserName', title: 'User Name', width: 100 },
          { field: 'GroupName', title: '组名', width: 50 },
           { field: 'GroupLeader', title: '组长', width: 50 },
           {
               field: 'Remarks', title: '备注', width: 100, formatter: function (value, row, index) {
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
            $('#project_room').datagrid('selectRow', 0);
        }
    });
    //定义pagination加载内容
    var p1_project_room = $('#project_room').datagrid('getPager');
    (p1_project_room).pagination({
        layout: ['first', 'prev', 'last', 'next']

    });
}
//****************************************************************添加入新部门
function AddGroupPerson() {
    var selectRow = $("#department_people").datagrid("getSelected");
    var selectRow_person = $("#person_info_add").datagrid("getSelected");
    var node = $('#department_info_add').tree('getSelected');
    if (node) {
        //$('#GroupLeader_dialog').dialog({
        //    width: 400,
        //    height: 300,
        //    modal: true,
        //    title: 'isLeader',
        //    draggable: true,
        //    buttons: [{
        //        text: 'Save',
        //        iconCls: 'icon-ok',
        //        handler: function () {

        //        }
        //    }, {
        //        text: 'Close',
        //        iconCls: 'icon-cancel',
        //        handler: function () {
        //            $('#GroupLeader_dialog').dialog('close');
        //        }
        //    }]
        //});
        //$('#GroupLeader_dialog').form("reset");

        $.ajax({
            url: "/SystemSettings/AddPerToGroup",
            type: 'POST',
            data: {
                //UserId: selectRow.UserId,
                GroupId: node.id,
                Group_id: node.id_,
                UserId: selectRow_person.UserId,
                UserName: selectRow.UserName,
                GroupName: node.text,
                //GroupLeader: $("#GroupLeader").val()

            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#Auth_person').datagrid('reload')
                    $('#GroupLeader_dialog').dialog('close');
                    $.messager.alert('提示', result.Message);
                } else {
                    $.messager.alert('提示', result.Message);
                }
            }
        });
    } else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }

}
//****************************************************************移除一个部门人员
function RemoveGroupPerson() {
    var selectRow = $("#department_people").datagrid("getSelected");
    var remove_node = $('#Auth_person').datagrid('getSelected');

    if (remove_node) {//id小于29的组都为必要组
        $.messager.confirm('提示', '是否移除该人员？', function (r) {
            if (r) {
                $.ajax({
                    url: "/SystemSettings/DelPerToGroup",
                    type: 'POST',
                    data: {
                        UserId: selectRow.UserId,
                        UserName: selectRow.UserName,
                        id: remove_node.Authorization_id,
                        GroupName: remove_node.GroupName
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#Auth_person').datagrid('reload');
                            $.messager.alert('提示', result.Message);
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        });

    } else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
}




//********************************************************************查看证书列表初始化********************************************************************
//********************************************************************查看证书列表
function read_certificate() {
    var selectRow = $("#department_people").datagrid("getSelected");//获取选中人员信息行
    if (selectRow) {
        $('#certificate_dialog').dialog({
            width: 900,
            height: 500,
            modal: true,
            title: '查看证书',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#certificate_dialog').dialog('close');
                }
            }]
        });
        //证书列表
        $('#read_certificate_datagrid').datagrid({
            border: true,
            nowrap: false,
            striped: true,
            ctrlSelect: true,
            fit: true,
            rownumbers: true,
            pagination: true,
            pageSize: 15,
            pageList: [15, 30, 45, 60],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            queryParams: {
                UserID: selectRow.UserId
            },
            url: "/SystemSettings/GetUserCertificate",//接收一般处理程序返回来的json数据  
            columns: [[
               { field: 'CertificateNum', title: '证书编号', width: 100 },
               { field: 'CertificateName', title: '证书名称', width: 120 },
               { field: 'CertificateType_n', title: '证书类型', width: 100 },
               { field: 'IssuingUnit', title: '发行单位', width: 100 },
               {
                   field: 'IssueDate', title: '签发日期', width: 100, formatter: function (value, row, index) {
                       if (value) {
                           if (value.length > 10) {
                               value = value.substr(0, 10)
                               return value;
                           }
                       }
                   }
               },
               {
                   field: 'ValidDate', title: '有效日期', width: 100, formatter: function (value, row, index) {
                       if (value) {
                           if (value.length > 10) {
                               value = value.substr(0, 10)
                               return value;
                           }
                       }
                   }
               },
               { field: 'Profession', title: '职业', width: 100 },
               { field: 'Quarters', title: '岗位', width: 100 },
               { field: 'Grade', title: '等级', width: 50 },
               {
                   field: 'CertificateSate', title: '证书认证', width: 100, formatter: function (value, row, index) {//样品标识
                       if (value == 0) {
                           return "Invalid";
                       } else if (value == 1) {
                           return "Effective";
                       }
                   }
               },
               {
                   field: 'CreateDate', title: '创建日期', width: 100, formatter: function (value, row, index) {
                       if (value) {
                           if (value.length > 10) {
                               value = value.substr(0, 10)
                               return value;
                           }
                       }
                   }
               },
               { field: 'CreatePersonnel_n', title: '创建人员', width: 120 },
               {
                   field: 'remarks', title: '备注', width: 150, formatter: function (value, row, index) {
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
                $('#read_certificate_datagrid').datagrid('selectRow', line);
            },
            rowStyler: function (index, row) {
                switch (row.CertificateSate) {
                    case 0: return 'color:red;'; break;
                }
            },
            toolbar: "#read_certificate_toolbar"
        });
    } else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
    //添加证书
    $('#add_certificate').unbind('click').bind('click', function () {
        add_certificate(selectRow);
    });
    //删除证书
    $('#del_certificate').unbind('click').bind('click', function () {
        del_certificate();
    });
    //查看证书附件
    $('#view_certificate_annex').unbind('click').bind('click', function () {
        view_certificate_annex(selectRow);
    });
};
//********************************************************************添加证书dialog********************************************************************
function add_certificate(selectRow) {
    $("#add_certificate_dialog").dialog({
        width: 700,
        height: 450,
        modal: true,
        title: '添加证书',
        draggable: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            handler: function () {
                save_certificate(selectRow);//确认添加证书
            }
        }, {
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#add_certificate_dialog').dialog('close');//关闭添加证书弹窗
            }
        }]
    });
    //获取证书类别下拉框
    $("#CertificateType").combobox({
        url: "/SystemSettings/GetDictionaryList",
        valueField: 'Value',
        textField: 'Text',
        panelHeight: 100,
        editable: false,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
};
//********************************************************************确认添加证书
function save_certificate(selectRow) {
    $('#add_certificate_dialog').form('submit', {
        url: "/SystemSettings/AddCertificateData",//接收一般处理程序返回来的json数据 
        onSubmit: function (param) {
            param.UserId = selectRow.UserId;//获取UserId传给后台
            return $(this).form('enableValidation').form('validate');//验证form表单是否输入数据
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);//把JSON字符串转为与之对应的JavaScript对象
                if (result.Success == true) {
                    $('#add_certificate_dialog').dialog('close');//关闭添加证书弹窗
                    $('#add_certificate_dialog').form('clear');//添加证书表单重置
                    $.messager.alert('提示', result.Message);
                    $('#read_certificate_datagrid').datagrid('reload');//重新加载证书列表
                }
                else {
                    $.messager.alert('提示', result.Message);
                }
            }
        }
    });
};
//********************************************************************删除证书
function del_certificate() {
    var selectRow = $("#read_certificate_datagrid").datagrid("getSelected");
    var node = $("#department_people").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('删除提示 ', '确实要删除此证书吗？', function (r) {
            if (r) {
                $.ajax({
                    url: "/SystemSettings/DelCertificateAppendix",
                    type: 'POST',
                    data: {
                        Id: selectRow.Id,//获取选中行的ID传给后台
                        UserName_n: node.UserCount
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $('#read_certificate_datagrid').datagrid('reload');//重新加载文件列表
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        });
    } else {
        $.messager.alert('提示', '请先选择证书进行操作！');
    };
};
//********************************************************************证书附件列表初始化******************************************************************
//********************************************************************查看证书附件列表
function view_certificate_annex(selectRow) {
    var select_certificate = $("#read_certificate_datagrid").datagrid("getSelected");//获取选中证书信息行
    if (select_certificate) {
        $('#certificate_annex_dialog').dialog({
            width: 800,
            height: 400,
            modal: true,
            title: '附件信息',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#certificate_annex_dialog').dialog('close');//关闭证书附件列表
                }
            }]
        });
        //证书附件列表
        $('#annex_datagrid').datagrid({
            border: true,
            nowrap: false,
            striped: true,
            ctrlSelect: true,
            fit: true,
            fitColumns: true,
            rownumbers: true,
            pagination: true,
            pageSize: 15,
            pageList: [15, 30, 45, 60],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            queryParams: { Id: select_certificate.Id },
            url: "/SystemSettings/GetCertificateAppendixList",//接收一般处理程序返回来的json数据  
            columns: [[
                { field: 'CertificateId', title: '证书ID', width: 100, hidden: true },
               { field: 'DocumentName', title: '文件名称', width: 130 },
               { field: 'DocumentUrl', title: '文件路径', width: 150, hidden: true },
               { field: 'DocumentFormat', title: '文件格式', width: 120 },
               { field: 'CreateDate', title: '上传日期', width: 100 },
               { field: 'CreatePersonnel_n', title: '上传人员', width: 120 },
            ]],
            onLoadSuccess: function (data) {
                $('#annex_datagrid').datagrid('selectRow', line);
            },
            toolbar: "#certificate_annex_toolbar"
        });
    } else {
        $.messager.alert('提示', '请先选择一位人员进行操作！');
    }
    //添加证书附件
    $('#add_annex').unbind('click').bind('click', function () {
        add_annex(select_certificate);
    });
    //删除证书附件
    $("#del_files").unbind("click").bind("click", function () {
        del_files();
    });
    //阅读证书附件
    $('#view_annex').unbind('click').bind('click', function () {
        view_annex();
    });
};
//********************************************************************添加证书附件dialog
function add_annex(select_certificate) {
    $("#add_annex_dialog").dialog({
        width: 500,
        height: 250,
        modal: true,
        title: '添加附件',
        draggable: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            handler: function () {
                sumbit_annex(select_certificate);//确认添加证书
            }
        }, {
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#add_annex_dialog').dialog('close');//关闭添加证书弹窗
            }
        }]
    });
};
//********************************************************************提交添加证书附件
function sumbit_annex(select_certificate) {
    $('#add_annex_dialog').form('submit', {
        url: "/SystemSettings/AddCertificateFile",//接收一般处理程序返回来的json数据 
        onSubmit: function (param) {
            param.CertificateId = select_certificate.Id;//获取证书列表选中行的id传给后台
            return $(this).form('enableValidation').form('validate');//验证form表单是否输入数据
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);//把JSON字符串转为与之对应的JavaScript对象
                if (result.Success == true) {
                    $('#add_annex_dialog').dialog('close');//关闭添加证书附件弹窗
                    $('#add_annex_dialog').form('clear');//添加证书附件表单重置
                    $.messager.alert('提示', result.Message);
                    $('#annex_datagrid').datagrid('reload');//重新加载证书附件列表
                }
                else {
                    $.messager.alert('提示', result.Message);
                }
            }
        }
    });
};
//********************************************************************阅读证书附件
function view_annex() {
    var read_selected = $("#annex_datagrid").datagrid("getSelected")
    if (read_selected) {
        $("#ShowOffice").prop("href", WordUrlSpit[0] + '?id=' + read_selected.Id + '&Operation_Type=12&pageName=FileManagement' + WordUrlSpit[1]);
        document.getElementById('ShowOffice').click();
    }
    else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
};
//********************************************************************删除证书附件文件
function del_files() {
    var selectRow = $("#annex_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('删除提示 ', '确实要删除此文件吗？', function (r) {
            if (r) {
                $.ajax({
                    url: "/SystemSettings/DelFileManagement",
                    type: 'POST',
                    data: {
                        id: selectRow.Id,//获取选中行的ID传给后台
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $('#annex_datagrid').datagrid('reload');//重新加载文件列表
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        });
    } else {
        $.messager.alert('提示', '请先选择一份文件再进行操作！');
    };
};
//********************************************************************组管理******************************************************************
function groupInfo() {
    $('#group_dialog').dialog({
        width: 600,
        height: 500,
        modal: true,
        title: '组管理',
        draggable: true,
        buttons: [{
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#group_dialog').dialog('close');
            }
        }]
    });

    //组信息信息
    $('#group_management').tree({
        url: '/SystemSettings/GetGroupTree',
        method: 'post',
        required: true,
        title: '组管理',
        top: 0,
        fit: true,
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#group_management').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY

            });
        },
        onBeforeExpand: function (node, param) {
            $('#group_management').tree('options').url = "/SystemSettings/GetGroupTree?GroupParentId=" + node.id;
        },
        onSelect: function () {
            //view_technicians_info();
        }
    });
    //删除一个节点
    $('#tree_del').unbind('click').bind('click', function () {
        deleteInfo();
    });
    //删除一个节点
    $('#tree_del_').unbind('click').bind('click', function () {
        deleteInfo();
    });
    //编辑项目
    $('#tree_edit').unbind('click').bind('click', function () {
        var node = $('#group_management').tree('getSelected');
        if (node) {
            editInfo(node);
        } else {
            $.messager.alert('提示', '请选择要操作的内容！');
        }
    });
    //编辑项目
    $('#tree_edit_').unbind('click').bind('click', function () {
        var node = $('#group_management').tree('getSelected');
        if (node) {
            editInfo(node);
        } else {
            $.messager.alert('提示', '请选择要操作的内容！');
        }
    });
    //添加下级项目
    $('#tree_add_next').unbind('click').bind('click', function () {
        addInfo();
    });
    //添加下级项目
    $('#tree_add_next_').unbind('click').bind('click', function () {
        var node = $('#group_management').tree('getSelected');
        if (node) {
            addInfo();
        } else {
            $.messager.alert('提示', '请选择要操作的内容！');
        }

    });
}
//编辑节点
function editInfo(node) {
    //  var nodes = $('#department_info').tree('getSelected');
    $('#tree_add_dialog').dialog({
        title: '编辑组',
        width: 500,
        height: 400,
        modal: true,
        draggable: true,
        beforeSubmit: function (formData, jqForm, options) {//提交前的回调方法
            //return $("#tree_add_dialog").form('validate');
        },
        onOpen: function () {
            $.ajax({
                url: "/SystemSettings/LoadGroupInfo",
                type: 'POST',
                data: {
                    GroupId: node.id,
                },
                success: function (data) {
                    var obj = $.parseJSON(data);
                    // form数据回显
                    $('#tree_add_dialog').form('load', obj.Data);
                }
            });
        },
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                $('#tree_add_dialog').form('submit', {
                    url: "/SystemSettings/EditGroup",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.GroupId = node.id
                    },
                    success: function (data) {
                        var obj = $.parseJSON(data);
                        if (obj.Success == true) {
                            var node1 = $('#department_info').tree('getSelected');
                            if (node1) {
                                $('#department_info').tree('update', {
                                    target: node.target,
                                    text: $('#GroupName').textbox('getText')
                                });
                            }
                            $('#tree_add_dialog').dialog('close');
                            $.messager.alert('提示', obj.Message);
                        } else {
                            $.messager.alert('提示', obj.Message);
                        }
                    }
                });
                ////重置表单
                //$('#tree_add_dialog').form("reset");
            }

        }, {
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#tree_add_dialog').dialog('close');
            }
        }]


    });

}
//删除組节点
function deleteInfo() {
    var nodes = $('#department_info').tree('getSelected');

    if (nodes) {//id小于29的组都为必要组
        var isParent = $('#department_info').tree('getParent', nodes.target);
        if (isParent) {
            $.messager.confirm('提示',
                '是否要删除该组？',
                function(r) {
                    if (r) {
                        $.ajax({
                            url: "/SystemSettings/DelGroup",
                            type: 'POST',
                            data: {
                                GroupId: nodes.id,
                                GroupName: nodes.text
                            },
                            success: function(data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    $('#department_info').tree('remove', nodes.target);
                                    $.messager.alert('提示', result.Message);
                                } else {
                                    $.messager.alert('提示', result.Message);
                                }
                            }
                        });
                    }
                });
        } else {
            $.messager.alert('警告', '这是根节点，不可以删除！');
        }
    } else {
        $.messager.alert('提示', '请选择要操作的组！');
    }


}
//添加組项目
function addInfo() {
    var node_add = $('#department_info').tree('getSelected');
    var node_Parent = $('#department_info').tree('getParent', node_add.target);
    $('#tree_add_dialog').dialog({
        title: '添加子级',
        width: 500,
        height: 400,
        modal: true,
        draggable: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                $('#tree_add_dialog').form('submit', {
                    url: "/SystemSettings/AddGroup",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.GroupParentId = node_add.id
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //更新当前选中的节点
                            var node1 = $('#department_info').tree('getSelected');
                            if (node1) {
                                $('#department_info').tree('append', {
                                    parent: node1.target,
                                    data: [{
                                        id: result.Data.id,
                                        GroupId: result.Data.GroupId,
                                        GroupParentId: result.Data.GroupParentId,
                                        text: $("#GroupName").textbox("getText")
                                    }]
                                });
                            }
                            //选择当前添加节点
                            $('#tree_add_dialog').dialog('close');
                            $.messager.alert('提示', result.Message);
                        } else {
                            $.messager.alert('提示', result.Message);
                        }
                    }
                });
            }
        }, {
            text: '关闭',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#tree_add_dialog').dialog('close');

            }
        }]

    });
    $('#tree_add_dialog').form('reset');
};
