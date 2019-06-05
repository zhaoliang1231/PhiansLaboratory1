
//页面权限
$(function () {
    // var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight);
    var _height = $(".tab-content").height();
    var tabs_width = screen.width;
    $('#test_layout').layout('panel', 'west').panel('resize', {
        //  width: 310,
        // height: _height
    });
    $('#test_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 600,
        // height: _height
    });
    $('#cc').layout('panel', 'north').panel('resize', {
        width:800,
        height: _height
    });
    //$('#test_layout').layout('panel', 'south').panel('resize', { height: 30 });
    $('#test_layout').layout('resize');
    $('#cc').layout('resize');
   
    //*************************************************************************项目选择
    $('#system_choose').combobox({
        url: "/SystemSettings/GetSystemList",
        valueField: 'Value',
        textField: 'Text',
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },

        onLoadSuccess: function (data) {
            //默认选中第一个
            $('#system_choose').combobox("setValue", data[0].Value);
            //权限列表
            ModuleName_load(data[0].Value);
            //获取右边选中行树
            var node_on_select = $('#department_info').tree('getSelected');
            //判断是否是首次加载
            if (node_on_select) {
                var node_on = $('#department_info').tree('getParent', node_on_select.target);
                //如果选中的节点不等于空，就调用回显
                if (node_on) {
                    //回显已有权限
                    AuthorizedPage(false);
                }
            }
        },
        onSelect: function (data) {
            //权限列表
            ModuleName_load(data.Value);
            //判断是否是首次加载
            //获取右边选中行树
            var node_on_select = $('#department_info').tree('getSelected');
            if (node_on_select) {
                var node_on = $('#department_info').tree('getParent', node_on_select.target);
                //如果选中的节点不等于空，就调用回显
                if (node_on) {
                    //回显已有权限
                    AuthorizedPage(false);
                }
            }

        }
    });
    //*********************************************************初始化树
    tree_load();
    //*********************************************************权限列表初始化
    ModuleName_load();
    $('#authGroup').click(function () {
        $("#authGroup").prop("checked", "checked");
        $("#authPersonal").prop("checked", false);
        $("#auth").val(false);
    });
    //拍照
    $('#authPersonal').click(function () {
        $("#authPersonal").prop("checked", "checked");
        $("#authGroup").prop("checked", false);
        $("#auth").val(true);
    });
});
//***************************************************************初始化树
function tree_load() {
    //部门信息
    $('#department_info').tree({
        url: "/SystemSettings/GetAuthGroupTree",
        method: 'post',
        required: true,
        //  title: '部门',
        fit: true,
        top: 0,
        onBeforeExpand: function (node, param) {
            $('#department_info').tree('options').url = "/SystemSettings/GetAuthGroupTree?GroupParentId=" + node.id;
        },
        onSelect: function () {
            var node_on = $('#department_info').tree('getSelected');
            //判断是否选中的组管理
            if (node_on.id != "8cff8e9f-f539-41c9-80ce-06a97f481391") {
                //每次加载前清空
                $("input:checked").each(function () {
                    $(this).prop("checked", false);
                });
                $("#authGroup").prop("checked", "checked");
                AuthorizedPage(false);
            }
            personDatagrid();
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
        },

    });
}
//********************************************************************人员详细信息列表初始化********************************************************************
//********************************************************************人员详细信息列表
function personDatagrid() {
    var node = $('#department_info').tree('getSelected');//获取选中树节点的信息
    
    $('#personDatagrid').datagrid(
    {
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
        url: "/SystemSettings/GetgroupPersonnelList",//接收一般处理程序返回来的json数据  
        queryParams: {
            GroupId: node.id//获取树节点的id传给后台
        },
        columns: [[
           { field: 'UserCount', title: 'UserCount', width: 100, sortable: 'true' },
           { field: 'UserName', title: 'UserName', width: 100, sortable: 'true' },
           {
               field: 'UserNsex', title: 'UserNsex', width: 100, formatter: function (value, row, index) {
                   if (value == true) {
                       return "man";
                   }
                   if (value == false) {
                       return "women";
                   }
               }
           }
        ]],
        sortName: 'UserCount',
        sortOrder: 'asc',
        onSelect: function (data) {
           
            var node_on = $('#department_info').tree('getSelected');
           
            //判断是否选中的组管理
            if (node_on.id != "8cff8e9f-f539-41c9-80ce-06a97f481391") {
                //每次加载前清空
                $("input:checked").each(function () {
                    $(this).prop("checked", false);
                });
                var AuthFlag = $("#auth").val();
                if (AuthFlag == "true") {
                    $("#authPersonal").prop("checked", "checked");
                    $("#authGroup").prop("checked", false);
                    $("#auth").val("true");
                } else {
                    $("#authGroup").prop("checked", "checked");
                    $("#authPersonal").prop("checked", false);
                    $("#auth").val("false");
                }
                AuthorizedPage(true);
            }
        }
        //toolbar: department_people_toolbar
    });
 
};
//****************************************************************权限页面回显
function AuthorizedPage(flag) {
    var node_on = $('#department_info').tree('getSelected');
    var selectRow = $('#personDatagrid').datagrid('getSelected');
    //获取载入的数据getData 获取每个父节点有几个子节点
    var nodes_data = $("#test").treegrid("getData");
    var userId="";
    if (selectRow) {
        userId = selectRow.UserId;
    }
    if (node_on) {
        //权限回显
        $.ajax({
            type: 'POST',
            dataType: "json",
            url: "/SystemSettings/ShowAuthorizedPage",
            data: {
                UserId: userId,
                GroupId: node_on.id,
                PageId: $('#system_choose').combobox("getValue"),  //获取项目pageiD
                flag: flag       //判断是组授权还是人员授权
            },
            success: function (data) {
                if (data) {
                    var PageIdArr = [];//PageId集合
                    var idArr = [];//Id集合
                    var idParentArr = [];//IdParent集合
                    for (var i = 0; i < data.length; i++) {
                        PageIdArr.push(data[i].PageId);
                        idParentArr.push(data[i].id);
                        //获取子节点的选中id
                        var nodes = $("#test").treegrid("getChildren", data[i].PageId);
                        if (nodes.length > 0) {
                            for (var g = 0; g < data[i].children.length; g++) {
                                idArr.push(data[i].children[g].id);

                            }
                        }

                        //判断儿子的是否全选 如果全选将父亲也打对勾
                        //if (nodes.length > 0) {
                        //    var sons = nodes;
                        //    if (sons.length != 0) {
                        //        for (j = 0; j < sons.length; j++) {
                        //            idArr.push(sons[j].id);
                        //            PageIdArr.push(sons[j].PageId)
                        //        }
                        //    }
                        //}
                    }
                    for (var i = 0; i < idArr.length; i++) {
                        $(('#check_' + idArr[i])).prop("checked", true);
                    }
                }
            }
        });
    }
}
//*****************************************************************权限列表
function ModuleName_load(pageId) {
    //获取项目pageiD
    // var pageId = $('#system_choose').combobox("getValue");
    //权限列表
    $('#test').treegrid({
        url: "/SystemSettings/LoadPageTree",
        idField: 'PageId',
        treeField: 'ModuleName',
        fit: true,
        fitColumns: true,
        queryParams: {
            PageId: pageId
        },
        dataType: "json",
        columns: [[
             {
                 field: 'ModuleName', title: '模块', width: 100, formatter: function (value, row, index) {
                     return "<input type='checkbox' disabled value=" + row.ModuleName + "," + row.PageId + " id='check_" + row.id + "'/>" + value;
                 }
             },

              { field: 'PageId', title: 'PageId', hidden: true },
             { field: 'IconCls', title: 'IconCls', hidden: true },
             {
                 field: 'NodeType', title: '类型', width: 80, formatter: function (value, row, index) {
                     if (value == "1") {
                         return "ModuleName";
                     } else if (value == "2") {
                         return "page";
                     }
                 }
             },
             { field: 'SortNum', title: '排序' },
             //{
             //    field: 'PermissionFlag', width: 80, title: 'PermissionFlag'
             //},
             //{ field: 'SortNum', title: 'SortNum' },
               //{
               //    field: 'Remarks', title: 'Remarks', width: 150, formatter: function (value, row, index) {
               //        if (value) {
               //            if (value.length > 30) {
               //                var result = value.replace(" ", "");//去空
               //                var value1 = result.substr(0, 30);
               //                return '<span  title=' + value + '>' + value1 + "......" + '</span>';
               //            } else {
               //                return '<span  title=' + value + '>' + value + '</span>';
               //            }
               //        }
               //    }
               //},
             {
                 field: 'id', title: '操作', width: 80, formatter: function (value, row, index) {
                     var nodes = $("#test").treegrid("getParent", row.PageId);
                     if (nodes) {
                         return '<a href="#" id="btn_' + row.id + '"  style="text-align:center;border-radius:8px;width:80px;background:#2c7659;display:inline-block;height:26px;color:#fff;line-height:26px;list-style:none;text-decoration:none" class="easyui-linkbutton">查看授权详情</a>';
                     }

                 }
             }
        ]],
        //onLoadSuccess: function (row, data) {
        //    $('#test').treegrid('doCellTip1', { delay: 1000 });

        //},
        rowStyler: function (row, index) {
            if (row.NodeType == 1) {
                return 'color:blue;font-weight:bold';
            } else if (row.NodeType == 2) {
                return 'color:black;';
            }
        },
        onClickCell: function (field, row) {
            //点击选中checkbox
            if (field == "ModuleName") {
                // $('#test').treegrid('selectRow', 0);
                var node_on = $('#department_info').tree('getSelected');
                if (node_on) {
                    //获取选中的模块
                    //判断是否之前被选中
                    choose(row);

                } else {
                    $(('#check_' + row.id)).prop("checked", false);
                    $.messager.alert("提示", "请先选择组！");
                }
            }
            //查看权限列表  判断是否是父节点，只有子节点的时候才有button
            var nodes = $("#test").treegrid("getChildren", row.PageId);
            if (nodes.length == 0 && field == "id") {
                detailBtn(row.id, row.PageId, row.ModuleName);
            }

        },

        toolbar: test_toolbar
    });
};

//****************************************************************选中checkbox
function choose(row) {
    var flag2;//判断当前checkbox是否是已经选中的标识
    if ($(('#check_' + row.id)).is(':checked')) {
        flag2 = false;//之前被选中 取消授权
    } else {
        flag2 = true;//之前没被选中,授权
    }
    show(row.PageId, row.id, flag2);
    //获取选中的模块
    getSelected(row.PageId, row.ModuleName, row.id, flag2);
}
//*********************************************************显示复选框全选
function show(checkid, id, flag2) {
    var s = '#check_' + id;
    /*选子节点*/
    var nodes = $("#test").treegrid("getChildren", checkid);
    for (i = 0; i < nodes.length; i++) {
        //  alert()
        $(('#check_' + nodes[i].id))[0].checked = $(s)[0].checked;
    }
    //选上级节点
    if ($(('#check_' + id)).is(':checked')) {
        var parent = $("#test").treegrid("getParent", checkid);
        /*选子节点*/
        var nodes = $("#test").treegrid("getChildren", checkid);
        if (nodes.length > 0) {
            var sons = nodes;
            if (sons.length != 0) {
                for (j = 0; j < sons.length; j++) {
                    $(('#check_' + sons[j].id)).prop("checked", false);
                }
            }

        }

    } else {
        var parent = $("#test").treegrid("getParent", checkid);
        /*选子节点*/
        var nodes = $("#test").treegrid("getChildren", checkid);
        if (nodes.length > 0) {
            var sons = nodes;
            if (sons.length != 0) {
                for (j = 0; j < sons.length; j++) {
                    $(('#check_' + sons[j].id)).prop("checked", true);
                }
            }

        }

    }
}
//********************************************************获取取消的结点
//获取选中的结点
function getSelected(PageId, ModuleName, id, flag2) {
    var node_on = $('#department_info').tree('getSelected');
    var $check_id = $("check_" + id);
    //获取Id
    var idList = [];
    //获取模块名
    var ModuleNameArr = [];
    //获取模块PageId
    var PageIdArr = [];
    //
    //获取选中的所有
    //$("input:checked").each(function () {
    //    var id = $(this).attr("id");
    //    var id1 = $("#" + id).val();
    //    if (id.indexOf('check_type') == -1 && id.indexOf("check_") > -1)
    //        idList.push(id1);

    //});
    //追加进去点击的id
    idList.push(id);
    //获取当前选中的PageId和模板名
    ModuleNameArr.push(ModuleName);
    PageIdArr.push(PageId);
    //获取子节点的选中id
    var nodes = $("#test").treegrid("getChildren", PageId);
    if (nodes.length > 0) {
        var sons = nodes;
        if (sons.length != 0) {
            for (j = 0; j < sons.length; j++) {
                idList.push(sons[j].id);
                ModuleNameArr.push(sons[j].ModuleName);
                PageIdArr.push(sons[j].PageId)
            }
        }

    }
    //id转换成字符串传递给后台
    idListStr = idList.join(",");
    //将模块名和模块PageId转换成字符串传递给后台
    ModuleNameArrStr = ModuleNameArr.join(",");
    //console.log(ModuleNameArrStr);
    PageIdArrStr = PageIdArr.join(",");
    var UserIdArr=[];
    var UserIds;
    var selectRow = $('#personDatagrid').datagrid('getSelected');//获取选中行
    var authFlag = $("#auth").val();
    if (authFlag == "true") {
        if (selectRow) {
            var selectRows = $('#personDatagrid').datagrid('getSelections');//获取选中行
            for (var i = 0; i < selectRows.length; i++) {
                UserIdArr.push(selectRows[i].UserId);
                UserIds = UserIdArr.join(",");
            }
        } else {
            $.messager.alert('Tips', '请选择人员！');
            return;
        }
    }
    //获取組节点
    $.ajax({
        type: 'POST',
        dataType: "json",
        url: "/SystemSettings/GroupAuthority",
        data: {
            flag:$("#auth").val(),
            flag2: flag2,//是授权还是取消
            flag3: "true",
            UserId: UserIds,
            GroupId: node_on.id,
            ids: idListStr,
            PageIds: PageIdArrStr,
            GroupName: node_on.text,
            ModuleNames: ModuleNameArrStr,
            SystemId: $('#system_choose').combobox("getValue")
        },
        success: function (data) {
            if (data.Success == true) {
                if ($(('#check_' + id)).is(':checked')) {
                    $('#check_' + id).prop("checked", false);
                } else {
                    $('#check_' + id).prop("checked", true);
                }
            }
            // var result = $.parseJSON(data);
            if (data.Success == false) {
                $.messager.alert('提示', data.Message);
                // $("#test").treegrid("reload");
            }
        }
    });


}
//获取选中点 条数
var totalNum = 0;
//****************/查看详细权限
function detailBtn(id, pageId, ModuleName) {
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    var node_on = $('#department_info').tree('getSelected');
    if ($(('#check_' + id)).is(':checked')) {
        $('#btn_dialog').dialog({
            width: 700,
            height: 500,
            top: 40,
            modal: true,
            title: '查看授权详情',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#btn_dialog').dialog('close');
                }
            }]
        });
        $('#check_btn').datagrid({
            nowrap: false,
            striped: true,
            //rownumbers: true,
            ctrlSelect: true,
            //fitColumns: true,
            fit: true,
            pagination: true,
            pageSize: num,
            pageList: [num, num + 10, num + 20, num + 20],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            url: "/SystemSettings/GetButtonAuthorityList",
            queryParams: {
                PageId: pageId,
                flag: "false", //标识 组授权信息为false  人员授权信息true   (这里只做组授权信息)
                Id: $('#department_info').tree('getSelected').id //Id可以是组的id 也可能是人员的id 根据 flag决定 这里写组信息的id
            },
            columns: [[
             { field: 'ModuleName', title: '模块', width: 120 },
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
            },
             {
                 field: 'IdFlag', title: '是否授权', width: 110, formatter: function (value, row, index) {
                     if (value == 0) {
                         value = "未经授权的";
                     } else if (value == 1) {
                         value = "经授权的";
                     }
                     return '<span style="font-weight:bold">' + value + '</span>';
                 }
             },
            {
                field: 'NodeType', title: '类型', width: 100, formatter: function (value, row, index) {
                    if (value == "3") {
                        return "按钮";
                    }
                }
            },
            { field: 'SortNum', title: '排序', width: 100 },
            //{ field: 'SortNum', title: 'SortNum', width: 100 },
             //{ field: 'U_url', title: 'U_url', width: 100 },
            { field: 'IconCls', title: 'IconCls', hidden: true },
             {
                 field: 'PermissionFlag', title: '是否批准', width: 100, formatter: function (value, row, index) {
                     if (value == false) {
                         return "否";
                     }else if (value == true) {
                         return "是";
                     }
                 }
             }

            ]],
            onDblClickRow: function (index, row) {
                // 判断是要授权还是已经授权
                var flag_auth;
                if (row.IdFlag == 0) {
                    flag_auth = "true";//双击授权
                } else {
                    flag_auth = "false";//双击取消授权
                }
                //btn授权
                //授权
                $.ajax({
                    type: 'POST',
                    dataType: "json",
                    url: "/SystemSettings/GroupAuthority",
                    data: {
                        flag2: flag_auth,//是授权还是取消
                        flag3: "false",
                        ids: id,
                        GroupName: node_on.text,
                        GroupId: node_on.id,
                        PageIds: row.PageId,
                        ButtonNames: row.ModuleName,
                        ModuleNames: ModuleName,
                        SystemId: $('#system_choose').combobox("getValue")
                    },
                    success: function (data) {
                        if (data.Success == true) {
                            $('#check_btn').datagrid("reload");
                        } else {
                            $.messager.alert('提示', data.Message);
                        }
                    }
                });
            },
            rowStyler: function (index, row) {
                if (row.IdFlag == 1) {
                    return 'color:black;';
                } else if (row.IdFlag == 0) {
                    return 'color:red;';
                }
            },
            onLoadSuccess: function () {
                //    //提示
                //    $('#check_btn').datagrid('doCellTip', { delay: 1000 });
                //},
               
            },
            toolbar: btn_toolbar
        });


    } else {
        $.messager.alert("提示", "请先分配页面权限！")
    }
    //全部授权
    $("#Btn_Auth_All").unbind("click").bind("click", function () {
        Btn_Auth(id, 'true', ModuleName)
    })
    //全部取消授权
    $("#Btn_UnAuth_All").unbind("click").bind("click", function () {
        Btn_Auth(id, 'false', ModuleName)
    })
}

//****************************************button授权
//flag判断是授权还是取消授权
//id 为节点树的page 行id
//ModuleName 主模块名
function Btn_Auth(id, flag, ModuleName) {
    var node_on = $('#department_info').tree('getSelected');
    //获取btn所有行
    var RowssData = $("#check_btn").datagrid("getData");//获取选中人员信息行
    debugger;
    var PageIdArr = [];
    var ModuleNameArr = [];
    for (var i = 0; i < RowssData.rows.length; i++) {
        PageIdArr.push(RowssData.rows[i].PageId);
        ModuleNameArr.push(RowssData.rows[i].ModuleName);
    }
    PageIds = PageIdArr.join(",");
    ModuleNames = ModuleNameArr.join(",");
    //授权
    $.ajax({
        type: 'POST',
        dataType: "json",
        url: "/SystemSettings/GroupAuthority",
        data: {
            flag2: flag,//是授权还是取消
            flag3: false,
            ids: id,
            GroupName: node_on.text,
            GroupId: node_on.id,
            PageIds: PageIds,
            ButtonNames: ModuleNames,
            ModuleNames: ModuleName,
            SystemId: $('#system_choose').combobox("getValue")
        },
        success: function (data) {
            if (data.Success == true) {
                $('#check_btn').datagrid("reload");
            } else {
                $.messager.alert('提示', data.Message);
            }
        }
    });
}

//*********************************************************鼠标经过datagrid显示提示
$(function () {
    $.extend($.fn.datagrid.methods, {
        doCellTip: function (jq, params) {
            function showTip(showParams, td, e, dg) {
                params = params || {};
                var options = dg.data('datagrid');
                showParams.content = '<div class="tipcontent" style="width:350px;font-weight:bold">黑色：已授权；红色：未授权；（双击可进行授权或取消授权.）</div>';
                $(td).tooltip({
                    content: showParams.content,
                    trackMouse: true,
                    position: params.position,
                    onHide: function () {
                        $(this).tooltip('destroy');
                    },
                    onShow: function () {
                        var tip = $(this).tooltip('tip');
                        if (showParams.tipStyler) {
                            tip.css(showParams.tipStyler);
                        }
                        if (showParams.contentStyler) {
                            tip.find('div.tipcontent').css(showParams.contentStyler);
                        }
                    }
                }).tooltip('show');
            };
            return jq.each(function () {
                var grid = $(this);
                var options = $(this).data('datagrid');
                if (!options.tooltip) {
                    var panel = grid.datagrid('getPanel').panel('panel');
                    panel.find('.datagrid-body').each(function () {
                        var delegateEle = $(this).find('> div.datagrid-body-inner').length ? $(this).find('> div.datagrid-body-inner')[0] : this;
                        $(delegateEle).undelegate('td', 'mouseover').undelegate('td', 'mouseout').undelegate('td', 'mousemove').delegate('td[field]', {
                            'mouseover': function (e) {
                                var that = this;
                                var setField = null;
                                if (params.specialShowFields && params.specialShowFields.sort) {
                                    for (var i = 0; i < params.specialShowFields.length; i++) {
                                        if (params.specialShowFields[i].field == $(this).attr('field')) {
                                            setField = params.specialShowFields[i];
                                        }
                                    }
                                }
                                if (setField == null) {
                                    options.factContent = $(this).find('>div').clone().css({ 'margin-left': '-5000px', 'width': 'auto', 'display': 'inline', 'position': 'absolute' }).appendTo('body');
                                    var factContentWidth = options.factContent.width();
                                    params.content = $(this).text();
                                    if (params.onlyShowInterrupt) {
                                        if (factContentWidth > $(this).width()) {
                                            showTip(params, this, e, grid);
                                        }
                                    } else {
                                        showTip(params, this, e, grid);
                                    }
                                } else {
                                    panel.find('.datagrid-body').each(function () {
                                        var trs = $(this).find('tr[datagrid-row-index="' + $(that).parent().attr('datagrid-row-index') + '"]');
                                        trs.each(function () {
                                            var td = $(this).find('> td[field="' + setField.showField + '"]');
                                            if (td.length) {
                                                params.content = td.text();
                                            }
                                        });
                                    });
                                    showTip(params, this, e, grid);
                                }
                            },
                            'mouseout': function (e) {
                                if (options.factContent) {
                                    options.factContent.remove();
                                    options.factContent = null;
                                }
                            }
                        });
                    });
                }
            });
        },
        doCellTip1: function (jq, params) {
            function showTip(showParams, td, e, dg) {
                params = params || {};
                var options = dg.data('datagrid');
                showParams.content = '<div class="tipcontent" style="width:110px;font-weight:bold;color:#f00">单击ModuleName单元格进行授权操作</div>';
                $(td).tooltip({
                    content: showParams.content,
                    trackMouse: true,
                    position: params.position,
                    onHide: function () {
                        $(this).tooltip('destroy');
                    },
                    onShow: function () {
                        var tip = $(this).tooltip('tip');
                        if (showParams.tipStyler) {
                            tip.css(showParams.tipStyler);
                        }
                        if (showParams.contentStyler) {
                            tip.find('div.tipcontent').css(showParams.contentStyler);
                        }
                    }
                }).tooltip('show');
            };
            return jq.each(function () {
                var grid = $(this);
                var options = $(this).data('datagrid');
                if (!options.tooltip) {
                    var panel = grid.datagrid('getPanel').panel('panel');
                    panel.find('.datagrid-body').each(function () {
                        var delegateEle = $(this).find('> div.datagrid-body-inner').length ? $(this).find('> div.datagrid-body-inner')[0] : this;
                        $(delegateEle).undelegate('td', 'mouseover').undelegate('td', 'mouseout').undelegate('td', 'mousemove').delegate('td[field]', {
                            'mouseover': function (e) {
                                var that = this;
                                var setField = null;
                                if (params.specialShowFields && params.specialShowFields.sort) {
                                    for (var i = 0; i < params.specialShowFields.length; i++) {
                                        if (params.specialShowFields[i].field == $(this).attr('field')) {
                                            setField = params.specialShowFields[i];
                                        }
                                    }
                                }
                                if (setField == null) {
                                    options.factContent = $(this).find('>div').clone().css({ 'margin-left': '-5000px', 'width': 'auto', 'display': 'inline', 'position': 'absolute' }).appendTo('body');
                                    var factContentWidth = options.factContent.width();
                                    params.content = $(this).text();
                                    if (params.onlyShowInterrupt) {
                                        if (factContentWidth > $(this).width()) {
                                            showTip(params, this, e, grid);
                                        }
                                    } else {
                                        showTip(params, this, e, grid);
                                    }
                                } else {
                                    panel.find('.datagrid-body').each(function () {
                                        var trs = $(this).find('tr[datagrid-row-index="' + $(that).parent().attr('datagrid-row-index') + '"]');
                                        trs.each(function () {
                                            var td = $(this).find('> td[field="' + setField.showField + '"]');
                                            if (td.length) {
                                                params.content = td.text();
                                            }
                                        });
                                    });
                                    showTip(params, this, e, grid);
                                }
                            },
                            'mouseout': function (e) {
                                if (options.factContent) {
                                    options.factContent.remove();
                                    options.factContent = null;
                                }
                            }
                        });
                    });
                }
            });
        }
    });
});
