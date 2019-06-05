var WordUrlSpit = new Array();
var word_link;
var line = 0;
$(function () {
    layout();//布局
    Procedures_tree();//树列表加载
    search();//下拉框信息加载

    word_link = $("#ShowOffice").attr("href");//获取a链接 URL

    WordUrlSpit = word_link.split("?");
    //WordUrlSpit[0]头部
    //WordUrlSpit[1]尾部

    //阅读文件
    $('#Environmental_management_read').unbind('click').bind('click', function () {
        Environmental_managementread();
    });
    //删除文件
    $('#Procedures_file_del').unbind('click').bind('click', function () {
        Procedures_file_del();
    });
    //导出表格
    $('#Procedures_file_export').unbind('click').bind('click', function () {
        Procedures_file_export();
    });
    //全部导出
    $('#all').unbind('click').bind('click', function () {
        s_all();
    });
    //选择导出
    $('#choose').unbind('click').bind('click', function () {
        s_choose();
    });
    //修改文件
    $('#Procedures_file_edit').unbind('click').bind('click', function () {
        Procedures_file_edit();
    });
    //增加文件
    $('#Procedures_file_add').unbind('click').bind('click', function () {
        Procedures_file_add();
    });
    //搜索文件
    $('#Procedures_file_search').unbind('click').bind('click', function () {
        Procedures_file_search();
    });
});

//布局
function layout() {
    var _height = $(".tab-content").height();
    var tabs_width = screen.width;
    //树部分的布局
    $('#Layout').layout('panel', 'west').panel('resize', {

    });
    //列表部分的布局
    $('#Layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 400,
    });

    $('#Layout').layout('resize');//
};
//文件类别树渲染
function Procedures_tree() {
    $('#Procedures_tree').tree({
        url: "/ManageQuality/LoadPageTree?id=1c359be8-b48e-44fc-a60b-c4e72e6f6e9f&Parent_id=8c20e217-5dc9-4b11-83bc-0325b47f7807",
        method: 'post',
        required: true,
        fit: true,
        top: 0,
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
                var n = $('#Procedures_tree').tree('find', data[0].id);
                //调用选中事件
                $('#Procedures_tree').tree('select', n.target);
            };
        },
        onBeforeExpand: function (node, param) {
            $('#Procedures_tree').tree('options').url = "/ManageQuality/LoadPageTree?&Parent_id=" + node.id;
        },
        onSelect: function () {
            treeDatagrid();
        }

    });
};
//搜索下拉框
function search() {
    $('#search').combobox({
        editable: false,
        data: [
           { 'value': 'FileNum', 'text': 'File Num' },
           { 'value': 'FileName', 'text': 'File Name' },
           { 'value': 'FileFormat', 'text': 'File Format' },
           { 'value': 'FilePersonnel', text: 'File Personnel'}
        ]
    });
    $('#search1').textbox({
        value: ''
    });
};
//搜索文件
function Procedures_file_search() {
    var file_type = $('#Procedures_tree').tree('getSelected');
    var search = $('#search').combobox('getValue');
    var search1 = $('#search1').textbox('getText');
    
    $('#Procedures_file_datagrid').datagrid(
        {
            type: 'POST',
            dataType: "json",
            url: "/ManageQuality/GetFileManagement",//接收一般处理程序返回来的json数据                
            queryParams: {
                search: search,
                key: search1,
                nodeText: file_type.id
            }
        }
   )
};
//增加文件
function Procedures_file_add() {
    $('#add_file').dialog({
        width: 650,
        height: 370,
        modal: true,
        title: 'Add File',
        draggable: true,
        buttons: [{
            text: 'Save',
            iconCls: 'icon-ok',
            id: 'add_file_save'
        }, {
            text: 'Cancel',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#add_file').dialog('close');
            }
        }]
    });
    var node = $('#Procedures_tree').tree('getSelected');//获取选中的树节点
    $('#add_file_save').unbind('click').bind('click', function () {
        $.messager.progress({
            text: '正在上传中...'
        });
        //form表单提交
        $('#add_file').form('submit', {
            url: "/ManageQuality/AddFileManagement",
            onSubmit: function (param) {
                param.FileTypeID = node.id;//获取选中树节点的id传给后台
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.progress('close');
                    $('#add_file').dialog('close');//关闭添加文件的弹窗
                    $.messager.alert('Tips', result.Message);
                    $('#Procedures_file_datagrid').datagrid('reload');//重新加载列表
                }
                else if (result.Success == false) {
                    $.messager.progress('close');
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    });
    $('#add_file').form('reset');//重置增加文件表单
    $('#FileType').textbox('setValue', node.text);//重新赋值文件类型的文本框
};
//修改文件
function Procedures_file_edit() {
    var selectRow = $("#Procedures_file_datagrid").datagrid("getSelected");//获取选中行
    var node = $('#Procedures_tree').tree('getSelected');//获取选中的树节点
    if (selectRow) {
        line = $('#Procedures_file_datagrid').datagrid("getRowIndex", selectRow);
        $('#edit_file').dialog({
            width: 630,
            height: 350,
            modal: true,
            title: 'Edit Type',
            draggable: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                id: 'edit_file_save'
            }, {
                text: 'Cancel',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#edit_file').dialog('close');
                }
            }]
        });
        var h_select_id = selectRow.id;//获取选中行的id
        var fileurl = selectRow.FileUrl;//获取选中行的FileUrl
        $('#edit_file').form('load', selectRow);//回显选中行的数据
        $('#FileNewName1').textbox('setText', selectRow.FileName);//回显选中行的fileName
        $('#FileType1').textbox('setValue', selectRow.FileType_n);//重新赋值文件类型文本框的值
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };

    $('#edit_file_save').unbind('click').bind('click', function () {
        //form表单提交
        $('#edit_file').form('submit', {
            url: "/ManageQuality/UpdateFileManagement",
            ajax: true,
            onSubmit: function (param) {
                param.id = h_select_id;//传给后台数据的id
                param.FileUrl = fileurl;//传给后台文件地址
                param.FileTypeID = node.id //获取选中树节点的id传给后台
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#edit_file').dialog('close');
                    $.messager.alert('Tips', result.Message);
                    $('#Procedures_file_datagrid').datagrid('reload');
                } else {
                    $.messager.alert('Tips', result.Message);

                }
            }
        });
    });
};
//---------------按钮功能实现--------------------//

//阅读文件
function Environmental_managementread() {
    var selectRow = $("#Procedures_file_datagrid").datagrid("getSelected");
    if (selectRow) {
        $("#ShowOffice").prop("href", WordUrlSpit[0] + '?id=' + selectRow.id + '&Operation_Type=5&pageName=FileManagement' + WordUrlSpit[1]);
        document.getElementById('ShowOffice').click();
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
//删除文件
function Procedures_file_del() {
    var selectRow = $("#Procedures_file_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('Tips', 'Are you sure you want to delete the selected task？', function (r) {
            //if (r) {
            //    var selectRow_1 = $("#Procedures_file_datagrid").datagrid('getSelections');
            //    var a = "";
            //    for (var i = 0; i < selectRow_1.length; i++) {
            //        if (i == 0) {
            //            a = selectRow_1[i].id;
            //        }
            //        if (i > 0) {
            //            a = a + "," + selectRow_1[i].id;
            //        }
            //    }
            if (r) {
                $.ajax({
                    url: "/ManageQuality/DelFileManagement",
                    type: 'POST',
                    data: {
                        id: selectRow.id,//获取选中行的id传给后台
                        FileName: selectRow.FileName,//获取选中行的FileName传给后台
                        FileUrl: selectRow.FileUrl//获取选中行的FileUrl传给后台
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('Tips', result.Message);
                            $('#Procedures_file_datagrid').datagrid('reload');
                        } else {
                            $.messager.alert('Tips', result.Message);

                        }
                    }
                });
            }
                
           // }
        });

    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
//导出报表
function Procedures_file_export() {
    var search = $('#search').combobox('getText');
    var search1 = $('#search1').combobox('getText');
    switch (search) {
        case "File Num": search = "FileNum"; break;
        case "File Type": search = "FileType"; break;
        case "File Name": search = "FileName"; break;
        case "File Format": search = "FileFormat"; break;
        case "File Remark": search = "FileRemark"; break;
        case "File Personnel": search = "FilePersonnel"; break;
        default: search = "";
    }
    var selectRow = $("#Procedures_file_datagrid").datagrid('getSelections');
    var a = "";
    if ($("#all").val() != '1') {
        var selectRow = $("#Procedures_file_datagrid").datagrid("getSelected");
        if (selectRow) {
            var selectRow_1 = $("#Procedures_file_datagrid").datagrid('getSelections');
            for (var i = 0; i < selectRow_1.length; i++) {
                if (i == 0) {
                    a = selectRow_1[i].id;
                }
                if (i > 0) {
                    a = a + "," + selectRow_1[i].id;
                }
            }
            $.ajax({
                url: "/ManageQuality/export",
                type: 'POST',
                data: {
                    search: search,
                    key: search1,
                    type: 1,
                    ids: a
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $.messager.alert('Tips', result.Message);
                        $('#Procedures_file_datagrid').datagrid('reload');
                    } else {
                        $.messager.alert('Tips', result.Message);
                    }
                }
            });

        }

        else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
        debugger;
    } else {
        $.ajax({
            url: "/ManageQuality/export",
            type: 'POST',
            data: {
                search: search,
                key: search1,
                type: 0,
                ids: a
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('Tips', result.Message);
                    $('#Procedures_file_datagrid').datagrid('reload');
                } else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    }
};
//全部导出
function s_all() {
    if ($("#all").is(":checked")) {
        $("#all").prop("checked", "checked");
        $("#choose").prop("checked", false);
        $("#all").val("1");
        $("#choose").val("0");
    } else {
        $("#choose").prop("checked", "checked");
        $("#all").prop("checked", false);
        $("#all").val("0");
        $("#choose").val("1");
    }
};
//选择导出
function s_choose() {
    if ($("#choose").is(":checked")) {
        $("#choose").prop("checked", "checked");
        $("#all").prop("checked", false);
        $("#all").val("0");
        $("#choose").val("1");
    } else {
        $("#all").prop("checked", "checked");
        $("#choose").prop("checked", false);
        $("#all").val("1");
        $("#choose").val("0");
    }
};
//树加载渲染
function treeDatagrid() {
    var node1 = $('#Procedures_tree').tree('getSelected');
    $('#Procedures_file_datagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        border: false,
        fit: true,
        pagination: true,
        fitColumns: true,
        pageSize: 30,
        pageList: [30, 60, 90, 120],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            nodeText: node1.id
        },
        url: "/ManageQuality/GetFileManagement",//接收一般处理程序返回来的json数据  
        columns: [[
            { field: 'FileNum', title: 'File Num', sortable: 'true', width: 100 },
            { field: 'FileType_n', title: 'File Type', width: 150 },
            { field: 'FileName', title: 'File Name', width: 250, sortable: 'true' },
            { field: 'FileFormat', title: 'File Format', width: 150 },
            { field: 'FileRemark', title: 'File Remark', width: 200 },
            { field: 'FileUserName', title: 'File User Name', width: 150 },
            {
                field: 'FileDate', title: 'File Date', width: 200, formatter: function (value, row, index) {
                    if (value) {//格式化时间
                        if (value.length > 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            {
                field: 'Remarks', title: 'Remarks', width: 150, formatter: function (value, row, index) {
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
            $('#Procedures_file_datagrid').datagrid('selectRow', line);//默认选中第一行
        },
        sortName: 'FileNum',
        sortOrder: 'asc',
        toolbar: "#Procedures_file_toolbar"
    })
    //下载文件
    $('#Download_files').unbind('click').bind('click', function () {
        Download_files();
    });
};
/*
*functionName:Download_files
*function:函数功能 下载
*Param id 
*author:程媛
*date:2018-05-03
*/
function Download_files() {
    var selectRow = $("#Procedures_file_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        $.ajax({
            url: "/ManageQuality/downloadFile",
            type: 'POST',
            data: {
                id: selectRow.id
            },
            success: function (data) {
                var obj = $.parseJSON(data);
                if (obj.Success == true) {
                    window.location = obj.Message;
                }
                else {
                    $.messager.alert('Tips', obj.Message);
                }
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }

};