var line = 0;
var tabs_width = screen.width;
//iframe可用高度
var _height = screen.availHeight;
var num = parseInt(_height / 25);
var flag = 0;
//  alert(_height)
$(function () {

    $('#cc').layout('panel', 'west').panel('resize', {
        width: 400,
        height: _height - 150

    });
    $('#cc').layout('panel', 'center').panel('resize', {

        height: _height - 150,
        width: tabs_width - 400,
    });
    $('#cc').layout('resize');
    //NodeType 节点类型
    $("#NodeType").combobox({
        data: [
           { 'value': '0', 'text': '系统' },
           { 'value': '1', 'text': '模块' },















































































           { 'value': '2', 'text': 'page' },
             { 'value': '3', 'text': 'button' }
        ]
    })
    //初始化树
    load_tree();

    //是否需要详细配置 1
    $('#PermissionFlag1').unbind('click').bind('click', function () {
        $("#PermissionFlag1").prop("checked", "checked");
        $("#PermissionFlag2").prop("checked", false);
        $("#PermissionFlag").val("True");
    });
    //是否需要详细配置 0
    $('#PermissionFlag2').unbind('click').bind('click', function () {
        $("#PermissionFlag2").prop("checked", "checked");
        $("#PermissionFlag1").prop("checked", false);
        $("#PermissionFlag").val("False");
    });
});

//初始化树
function load_tree() {
    $('#tree').tree({
        url: "/FunctionalModule/LoadPage",
        method: 'post',
        required: true,
        height: 700,
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#tree').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY
                //onClick: function (item) {
                //    alert(item.name);
                //    remove();
                //}
            });
        },
        onBeforeExpand: function (node, param) {
            $('#tree').tree('options').url = "/FunctionalModule/LoadPage?ParentId=" + node.id;
        },
        onAfterEdit: function () {
            var node = $('#tree').tree('getSelected');
            //$.ajax({
            //    url: "_info.aspx?cmd=treeedit",
            //    type: 'POST',
            //    data: {
            //        ids: node.id,
            //        Project_name: node.text
            //    },
            //    success: function (data) {
            //        if (data == 'T') {
            //            $('#show_info').datagrid('reload');
            //            $.messager.alert('提示', '编辑信息成功');

            //        }
            //    }
            //});
        }, onSelect: function () {
            //var node = $('#tree').tree('getSelected');
            //渲染右边列表
            treeDatagrid();
        }

    });

    //编辑项目
    $('#tree_edit').unbind("click").bind("click", function () {
        var node = $('#tree').tree('getSelected');
        if (node) {
            //node.text = '修改';  //-->txt-->DB
            //$('#tree').tree('beginEdit', node.target);
            editInfo(node);
        }
    });
    //添加下级项目
    /*
    *functionName:treeDatagrid
    *function:初始化datagrid树
    *Param: 参数
    *author:创建人
    *date:时间
    */
    $('#tree_add_next').unbind('click').bind('click', function () {
        tree_add_next();
    });
    //删除树
    $('#tree_del').unbind("click").bind("click", function () {
        deleteInfo()
    });
    //搜索树内容
    $('#search_tree_click').unbind('click').bind('click', function () {
        search_tree2();
    });
    //搜索树内容
    $('#test').unbind('click').bind('click', function () {
        seach_ree();

    });

}
//编辑节点
function editInfo(node) {
    $('#tree_add_dialog').dialog({
        title: '修改页面信息',
        width: 400,
        height: 300,
        left: 50,
        top: 100,
        beforeSubmit: function (formData, jqForm, options) {//提交前的回调方法
            //return $("#tree_add_dialog").form('validate');
            console.log($('#ModuleName').textbox('getText'))
        },
        onOpen: function () {
            $.ajax({
                url: "/FunctionalModule/LoadPageInfo",
                type: 'POST',
                data: {
                    PageId: node.id,
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
                    url: "/FunctionalModule/EditPage",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.PageId = node.id
                    },
                    success: function (data) {
                        var obj = $.parseJSON(data);
                        if (obj.Success == true) {
                            var node1 = $('#tree').tree('getSelected');
                            if (node1) {
                                $('#tree').tree('update', {
                                    target: node.target,
                                    text: $('#ModuleName').textbox('getText')
                                });
                            }
                            $('#tree_add_dialog').dialog('close');
                            $.messager.alert('提示', '修改信息成功');
                        } else {
                            $.messager.alert('提示', '添加信息失败');
                        }
                    }
                });
                ////重置表单
                //$('#tree_add_dialog').form("reset");
            }

        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#tree_add_dialog').dialog('close');
            }
        }]


    });

}

//添加树同级项目
//function tree_add() {
//    var node_add = $('#tree').tree('getSelected');
//    var node_Parent = $('#tree').tree('getParent', node_add.target);
//    $('#tree_add_dialog').dialog({
//        title: '添加同级项目',
//        width: 400,
//        height: 300,
//        left: 50,
//        top: 100,
//        buttons: [{
//            text: '保存',
//            iconCls: 'icon-save',
//            handler: function () {
//                $('#tree_add_dialog').form('submit', {
//                    url: "/FunctionalModule/LoadPage",//接收一般处理程序返回来的json数据     
//                    onSubmit: function (param) {
//                        param.PageId = node_add.id
//                    },
//                    success: function (data) {
//                        if (data == 'T') {
//                            $('#tree_add_dialog').dialog('close');
//                            var node = $('#tree').tree('find', data);
//                            $('#tree').tree('select', node.target);
//                            $.messager.alert('提示', '添加信息成功');
//                        } else {
//                            $.messager.alert('提示', '添加信息失败');
//                        }
//                    }
//                });
//            }

//        }, {
//            text: '取消',
//            iconCls: 'icon-cancel',
//            handler: function () {
//                $('#tree_add_dialog').dialog('close');
//            }
//        }]
//    });
//}

//列表树详细信息

/*
*functionName:treeDatagrid
*function:初始化datagrid树
*Param: 参数
*author:创建人
*date:时间
*/
function treeDatagrid() {
    var node = $('#tree').tree('getSelected');
    $('#show_info').datagrid(
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
          pagination: true,
          pageSize: 10,
          pageList: [10, 20, 30, 40],
          pageNumber: 1,
          type: 'POST',
          dataType: "json",
          queryParams: {
              PageId: node.id
          },
          url: "/FunctionalModule/LoadModuleInfo",//接收一般处理程序返回来的json数据  
          columns: [[
             { field: 'ModuleName', title: '页面名称' },
             { field: 'IconCls', title: '图标' },
             { field: 'NodeType', title: '节点类型' },
             { field: 'SortNum', title: '排序' },
             { field: 'U_url', title: '链接' },
             {
                 field: 'PermissionFlag', title: '是否需要详细配置', formatter: function (value, row, index) {
                     if (value == "false") {
                         return "否";
                     } else {
                         return "是";
                     }
                 }
             },
             { field: 'SortNum', title: '排序号' },
             {
                 field: 'remarks', title: '附注', formatter: function (value, row, index) {
                     if (value != null & value != "") {
                         var result = value.replace(" ", "");
                         return '<span  class="class-with-tooltip"  title= ' + (result) + '>' + result + '</span>'
                     }
                 }
             }
          ]],
          onLoadSuccess: function (data) {
              $('#show_info').datagrid('selectRow', 0);
          }
          // toolbar: department_people_toolbar
      });

}
//添加树下级项目
function tree_add_next() {
    var node_add = $('#tree').tree('getSelected');
    var node_Parent = $('#tree').tree('getParent', node_add.target);
    $('#tree_add_dialog').dialog({
        title: '添加下级项目',
        width: 400,
        height: 300,
        left: 50,
        top: 100,
        buttons: [{
            text: '保存',
            iconCls: 'icon-save',
            handler: function () {
                $('#tree_add_dialog').form('submit', {
                    url: "/FunctionalModule/AddPage",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.PageId = node_add.id
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //更新当前选中的节点
                            var node1 = $('#tree').tree('getSelected');

                            if (node1) {
                                $('#tree').tree('append', {
                                    parent: node1.target,

                                    data: [{
                                        id: result.Resultdata,
                                        text: $("#ModuleName").textbox("getText")
                                    }]
                                });
                            }
                            //选择当前添加节点
                            var node = $('#tree').tree('find', result.Resultdata);
                            $('#tree').tree('select', node.target);
                            // $('#tree_add_dialog').dialog('close');

                            $.messager.alert('提示', '添加信息成功');
                        } else {
                            $.messager.alert('提示', '添加信息失败');
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

    });
    $('#tree_add_dialog').form('reset');

}
//shanc    //删除一个节点
function deleteInfo() {
    var nodes = $('#tree').tree('getSelected');
    var root = $('#tree').tree('getRoot');
    //var str = "";
    //var parentAll = "";
    //parentAll = nodes.text;
    //parentAll = parentAll.replace(/\[[^\)]*\]/g, ""); //获得所需的节点文本
    //var flag = ",";
    //var parent = $('#tree').tree('getParent', nodes.target); //获取选中节点的父节点
    //for (i = 0; i < 6; i++) { //可以视树的层级合理设置I
    //    if (parent != null) {
    //        parentAll = flag.concat(parentAll);
    //        str = (parent.text).replace(/\[[^\)]*\]/g, "");
    //        parentAll = (str).concat(parentAll);
    //        var parent = $('#tree').tree('getParent', parent.target);
    //    }
    //}
    if (nodes) {//id小于29的组都为必要组
        $.messager.confirm('删除提示', '您确认要删除选中节点吗', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: '/FunctionalModule/DelPage',
                    type: 'POST',
                    data: {
                        PageId: nodes.id
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            //选择当前添加节点
                            $('#tree').tree('remove', nodes.target);
                            $.messager.alert('提示', '删除信息成功');
                        } else {
                            $.messager.alert('提示', '删除信息失败');
                        }
                    }
                })
            }
        });


    }


    //搜索树内容
    //function search_tree(count_id) {

    //    var flag = 0;
    //    var search = $("#search_tree").textbox('getText')
    //    $('#tree').tree({
    //        url: "_info.aspx?cmd=loadtree_id&search=" + count_id,
    //        method: 'post',
    //        required: true,
    //        height: 700,
    //        onContextMenu: function (e, node) {
    //            e.preventDefault();
    //            $('#tree').tree('select', node.target);
    //            $('#keyMenu').menu('show', {
    //                left: e.pageX,
    //                top: e.pageY

    //            });
    //        }, onBeforeExpand: function (node, param) {
    //            $('#tree').tree('options').url = "_info.aspx?cmd=loadtree&Parent_id=" + node.id;
    //        },
    //        onAfterEdit: function () {

    //            var node = $('#tree').tree('getSelected');
    //            $.ajax({
    //                url: "_info.aspx?cmd=treeedit",
    //                type: 'POST',
    //                data: {
    //                    ids: node.id,
    //                    Project_name: node.text
    //                },
    //                success: function (data) {
    //                    if (data == 'T') {
    //                        $('#show_info').datagrid('reload');
    //                        $.messager.alert('提示', '编辑信息成功');

    //                    }
    //                }
    //            });
    //        }, onSelect: function () {
    //            flag = 1;
    //            var node = $('#tree').tree('getSelected');
    //            $('#show_info').datagrid({
    //                type: 'POST',
    //                dataType: "json",
    //                url: "_info.aspx?&cmd=show_info",//接收一般处理程序返回来的json数据
    //                queryParams: {
    //                    search: $("#search_com").combobox('getValue'),
    //                    search1: $("#search").textbox('getText'),
    //                    Parent_id: node.id
    //                }
    //            });

    //        }, rowStyler: function (index, row) {
    //            if (row.listprice > 50) {
    //                return 'background-color:pink;color:blue;font-weight:bold;';
    //            }
    //        },
    //        onLoadSuccess: function (data) {
    //            //在查询的时候起作用
    //            if (flag == 0) {
    //                //选中查询到结果
    //                var node1 = $('#tree').tree('find', count_id);
    //                $('#tree').tree('select', node1.target);
    //            }
    //        }
    //    });

    //}
    //搜索树内容
    function search_tree2() {

        var search = $("#search_tree").textbox('getText')
        $('#tree').tree({
            url: "_info.aspx?cmd=loadtree_s&search=" + search,
            method: 'post',
            required: true,
            height: 700,
            onContextMenu: function (e, node) {
                e.preventDefault();
                $('#tree').tree('select', node.target);
                $('#keyMenu').menu('show', {
                    left: e.pageX,
                    top: e.pageY

                });
            }, onBeforeExpand: function (node, param) {
                $('#tree').tree('options').url = "_info.aspx?cmd=loadtree&Parent_id=" + node.id;
            },
            onAfterEdit: function () {

                var node = $('#tree').tree('getSelected');
                $.ajax({
                    url: "_info.aspx?cmd=treeedit",
                    type: 'POST',
                    data: {
                        ids: node.id,
                        Project_name: node.text
                    },
                    success: function (data) {
                        if (data == 'T') {
                            $('#show_info').datagrid('reload');
                            $.messager.alert('提示', '编辑信息成功');

                        }
                    }
                });
            }, onSelect: function () {
                var node = $('#tree').tree('getSelected');

            }, rowStyler: function (index, row) {
                if (row.listprice > 50) {
                    return 'background-color:pink;color:blue;font-weight:bold;';
                }
            }

        });

    }

    var frist_ = 0;
    var send_cout = 0;
    var count_id = "";
    var count_ = 0;
    function seach_ree() {
        frist_ = 0;
        send_cout = 0;
        count_id = "";
        count_ = 0;
        search = $("#search_tree").textbox('getText');
        if (search == "") {
            $.messager.alert('提示', '请输入查询内容');
            return;
        }
        $.ajax({
            url: "_info.aspx?cmd=check_info",
            type: 'POST',
            data: {
                search: search
            },
            success: function (data) {

                if (data != "F") {

                    if (data.length > 0) {
                        count_id = data.split(',');
                        count_ = count_id.length
                    }
                    $('#show_count').dialog({
                        title: '查询结果',
                        width: 450,
                        height: 150,
                        left: 400,
                        top: 100,
                        buttons: [{
                            text: '关闭',
                            iconCls: 'icon-cancel',
                            handler: function () {
                                $('#show_count').dialog('close');
                            }
                        }]

                    });

                    $("#count_").html(count_);
                }

                else {
                    $.messager.alert('提示', '失败');
                }

            }

        });

        $('#up_').unbind('click').bind('click', function () {
            find_(1);

        });

        $('#next').unbind('click').bind('click', function () {

            find_(0);

        });
    }

    function find_(nn) {

        //首次点击
        if (frist_ == 0 && count_ != 0) {
            search_tree(count_id[0]);
            $("#count_1").html("1");
            frist_ = 1;
            send_cout = 0;
        }
        else if (count_ == 1 && frist_ != 0) {

            search_tree(count_id[0]);
            $("#count_1").html("1");
            send_cout = 0;

        } else if (count_ >= 2) {

            switch (nn) {
                //下一条结果
                case 0:

                    if (send_cout + 1 < count_) {
                        send_cout = send_cout + 1;
                        search_tree(count_id[send_cout]);
                    }

                    break;
                    //上一条结果
                case 1:
                    if (send_cout - 1 >= 0) {
                        send_cout = send_cout - 1;
                        search_tree(count_id[send_cout]);
                    }
                    // $("#count_1").html(send_cout -1);
                    break;
            }

        }

    }
    //初始化树详细内容
    function load_detail_info() {
        $('#search_com').combobox({
            //value:0,
            panelHeight: 80,
            data: [
                { 'value': 'content_name', 'text': '名称' },
                { 'value': 'content', 'text': '数据' },
                { 'value': 'content_type', 'text': '类型' }

            ]
        });
        $('#show_info').datagrid({
            border: false,
            resizeHandle: 'both',

            nowrap: false,//如果为true，则在同一行中显示数据。设置为true可以提高加载性能。
            striped: true,//是否显示斑马线效果。
            rownumbers: true,//如果为true，则显示一个行号列。
            singleSelect: true,//如果为true，则只允许选择一行。
            //autoRowHeight: true,
            fit: true,
            fitColumns: true, //真正的自动展开/收缩列的大小，以适应网格的宽度，防止水平滚动。
            pagination: true,//如果为true，则在DataGrid控件底部显示分页工具栏。
            pageSize: num,
            pageList: [num, num + 10, num + 20, num + 20],
            pageNumber: 1, //在设置分页属性的时候初始化页码。
            type: 'POST',
            dataType: "json",
            // url: "_info.aspx?&cmd=show_info",//接收一般处理程序返回来的json数据
            columns: [[
                  { title: 'id', field: 'id', hidden: true },
                  { title: '名称', field: 'content_name' },
                  //去掉类型的展示 -- 2017-9-25
                  //{ title: '类型', field: 'content_type' },
                  { title: '数据内容', field: 'content' },
                  {
                      title: '附件', field: 'fileExtension', formatter: function (value, row, index) {
                          //                          if (value.length > 0) {
                          //                              alert(value);
                          //                          }

                          //把获取到的字符串 转换为小写
                          //type 文件后缀
                          var type = value.toLowerCase();
                          //文本内容
                          if (type == '.txt') {
                              value = '<image src="../image/text_48px.png" />';

                          } else if (type == '.doc' || type == '.xls' || type == '.docx' || type == '.xlsx' || type == '.ppt' || type == '.pptx') { //office文件
                              value = '<image src="../image/office_32px.png" />';

                          } else if (type == '.jpg' || type == '.png' || type == '.jpeg') { //图片
                              value = '<image src="../image/jpg_1.png" />';

                          } else if (type == '.pdf') {
                              value = '<image src="../image/pdf_1.png" />';
                          }
                          return value;
                      }
                  }

            ]],
            queryParams: {
                search: $("#search_com").combobox('getValue'),
                search1: $("#search").textbox('getText')

            },
            onLoadSuccess: function (data) {
                //默认选择行
                //$('#show_info').datagrid('selectRow', 0);
                try {
                    $('#show_info').datagrid('selectRow', line);
                } catch (e) {

                }

            },
            onSelect: function () {
                var getSelected = $('#show_info').datagrid('getSelected');

                if (getSelected) {
                    var file_url_ = getSelected.file_url;
                    if (file_url_.length > 0) {
                        $('#detail_read').show();
                    } else {
                        $('#detail_read').hide();
                    }
                }
                // $('#tree').tree('expandAll', node.target);

            },
            onDblClickRow: function (index, row) {

                var getSelected = $('#show_info').datagrid('getSelected');

                /*
                 -- 2017-9-25
                if (getSelected.content_type == 'office文件' || getSelected.content_type == 'pdf文件') {
                    //如果是非图片则展示为HTML
                    //判断是否没有上传文件
                    if (getSelected.file_url != null && getSelected.file_url != "") {
                        attachment_html(getSelected);
                    }
                }
                */


                //如果是 office文件或pdf文件，则将其转换成Html
                //如果是图片 则显示图片
                //文件后缀
                var fileExtension = getSelected.fileExtension.toLowerCase();

                if (fileExtension == '.doc' || fileExtension == '.xls' || fileExtension == '.docx' || fileExtension == '.xlsx' || fileExtension == '.ppt' || fileExtension == '.pptx' || fileExtension == '.pdf') {
                    attachment_html(getSelected);
                } else if (fileExtension == ".pdf" || fileExtension == "jpeg" || fileExtension == '.jpg') {
                    //显示对话框  展示图片
                    $('#show_pic').dialog({

                        title: '展示',
                        width: 300,
                        height: 300,
                        resizable: true,
                        closed: false,

                        fit: true,
                        cache: false

                    });

                    $("#img").attr('src', getSelected.file_url);
                }

            },

            toolbar: "#info_toolbar"

        });
        //搜索内容
        $("#_search").click(function () {
            var node_add = $('#tree').tree('getSelected');
            if (node_add) {
                $('#show_info').datagrid(
               {
                   type: 'POST',
                   dataType: "json",
                   url: "_info.aspx?&cmd=show_info",//接收一般处理程序返回来的json数据
                   queryParams: {
                       search: $("#search_com").combobox('getValue'),
                       search1: $("#search").textbox('getText'),
                       Parent_id: node_add.id
                   }
               });
            }
            else {

                $.messager.alert('提示', '请选择树的节点');

            }
        });
        //添加树详细内容
        $('#detail_add').unbind('click').bind('click', function () {
            var node_add = $('#tree').tree('getSelected');
            if (node_add) {

                detail_info_add(node_add);

            }
            else {
                $.messager.alert('提示', '请选择BOM树');
            }
        });
        //编辑树详细内容
        $('#detail_edit').unbind('click').bind('click', function () {
            var getSelected = $('#show_info').datagrid('getSelected');
            if (getSelected) {
                line = $('#show_info').datagrid("getRowIndex", getSelected);
                detail_info_edit(getSelected);

            }
            else {
                $.messager.alert('提示', '请选择BOM树');
            }
        });

        //删除树详细内容
        $('#detail_remove').unbind('click').bind('click', function () {
            var getSelected = $('#show_info').datagrid('getSelected');
            if (getSelected) {

                detail_info_delete(getSelected);

            }
            else {
                $.messager.alert('提示', '请选择行');
            }
        });
        //查看附件
        $('#detail_read').unbind('click').bind('click', function () {
            var getSelected = $('#show_info').datagrid('getSelected');
            //window.location = getSelected.file_url;
            window.open(getSelected.file_url);
        });
    }

    //添加树详细内容
    function detail_info_add(node_add) {
        $('#content_type').combobox({
            //value:0,
            panelHeight: 120,
            data: [
                { 'value': '文本内容', 'text': '文本内容' },
                { 'value': '图片', 'text': '图片' },
                { 'value': '图纸', 'text': '图纸' },
                { 'value': 'office文件', 'text': 'office文件' },
                { 'value': 'pdf文件', 'text': 'pdf文件' }

            ]
        });

        $('#info_form').dialog({
            title: '添加BOM详细内容',
            width: 450,
            height: 400,

            top: 100,
            buttons: [{
                text: '保存',
                iconCls: 'icon-save',
                handler: function () { save(node_add); }

            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#info_form').dialog('close');
                }
            }]

        });

        function save() {

            //--不显示上传文件类型
            // var content_type1 = $('#content_type').combobox('getText');

            $('#info_form').form('submit', {

                url: "_info.aspx",//接收一般处理程序返回来的json数据
                dataType: 'text',
                contentType: 'application/json',
                onSubmit: function (param) {

                    param.cmd = 'detail_add';

                    //取消类型
                    // param.content_type1 = content_type1;
                    param.info_id = node_add.id;

                },
                success: function (data) {
                    if (data == 'T') {
                        $('#info_form').dialog('close');
                        line = 0;
                        $('#show_info').datagrid('reload');
                        $.messager.alert('提示', '添加信息成功');

                    }
                    else {

                        $.messager.alert('提示', '添加信息失败');
                    }

                }
            });

        }
    }
    //编辑树详细内容
    function detail_info_edit(getSelected) {

        $('#content_type').combobox({
            //value:0,
            panelHeight: 120,
            data: [
                { 'value': '文本内容', 'text': '文本内容' },
                { 'value': '图片', 'text': '图片' },
                { 'value': '图纸', 'text': '图纸' },
                { 'value': 'office文件', 'text': 'office文件' },
                { 'value': 'pdf文件', 'text': 'pdf文件' }

            ]
        });

        $('#info_form').dialog({
            title: '添加BOM详细内容',
            width: 450,
            height: 400,

            top: 100,
            buttons: [{
                text: '保存',
                iconCls: 'icon-save',
                handler: function () { save(getSelected); }

            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#info_form').dialog('close');
                }
            }]

        });
        var rowss = $('#show_info').datagrid('getSelections'); //获取选中数据

        $('#info_form').form('load', rowss[0]);
        //  $('#content_type').combobox('select', rowss[0].content_type);

        function save(getSelected) {

            //var content_type1 = $('#content_type').combobox('getText');


            $('#info_form').form('submit', {

                url: "_info.aspx",//接收一般处理程序返回来的json数据
                dataType: 'text',
                onSubmit: function (param) {
                    param.cmd = 'detail_edit';
                    // param.content_type1 = content_type1;
                    param.id = getSelected.id;
                    param.old_url = getSelected.file_url;


                },
                success: function (data) {
                    if (data == 'T') {
                        $('#info_form').dialog('close');
                        $('#show_info').datagrid('reload');
                        $.messager.alert('提示', '修改信息成功');

                    }
                    else {

                        $.messager.alert('提示', '修改信息失败');
                    }

                }
            });
        }

    }
    //删除树详细内容
    function detail_info_delete(getSelected) {

        $.ajax({
            url: "_info.aspx?cmd=detail_remove",
            type: 'POST',
            data: {
                ids: getSelected.id,
                old_url: getSelected.file_url
            },
            success: function (data) {

                if (data == "T") {
                    $.messager.alert('提示', '删除信息成功');
                    line = 0;
                    $('#show_info').datagrid('reload');
                }
                if (data == '0') {
                    $.messager.alert('提示', '无法删除，可能内容不存在');

                }
                else if (data != '0' && data != 'T') {
                    $.messager.alert('提示', '删除信息失败');
                }

            }

        });
    }

    //将获取到的附件转换成HTML
    function attachment_html(getSelected) {

        $.ajax({
            url: "_info.aspx?cmd=attachment_html",
            type: 'POST',
            data: {
                new_url: getSelected.file_url
            },
            success: function (data) {

                window.open(data);
            }

        });
    }
}