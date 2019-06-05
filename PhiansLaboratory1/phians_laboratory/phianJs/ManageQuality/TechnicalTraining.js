var WordUrlSpit = new Array();
var word_link;
$(function () {
    layout();//布局
    File_type();//树列表加载
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
    $('#Environmental_management_remove').unbind('click').bind('click', function () {
        h_delete();
    });

    //导出表格
    $('#Environmental_management_export').unbind('click').bind('click', function () {
        Environmental_managementexport();
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
    $('#Environmental_management_alter_file').unbind('click').bind('click', function () {
        Environmental_management_alterfile();
    });

    //增加文件
    $('#Environmental_management_add_file').unbind('click').bind('click', function () {
        Environmental_management_addfile();
    });

    //搜索文件
    $('#Environmental_management_search').unbind('click').bind('click', function () {
        search_file();
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
function File_type() {
    $('#File_type').tree({
        url: "/ManageQuality/LoadPageTree?id=52E00F9A-BBCD-4216-9CB8-0FEEA30F8663&Parent_id=8C20E217-5DC9-4B11-83BC-0325B47F7807",
        method: 'post',
        required: true,
        //title: '部门',
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
                var n = $('#File_type').tree('find', data[0].id);
                //调用选中事件
                $('#File_type').tree('select', n.target);
            };
        },
        onBeforeExpand: function (node, param) {
            $('#File_type').tree('options').url = "/ManageQuality/LoadPageTree?id=52E00F9A-BBCD-4216-9CB8-0FEEA30F8663&Parent_id=" + node.id;
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
           { 'value': '0', 'text': 'File Num' },
           { 'value': '2', 'text': 'File Name' },
           { 'value': '3', 'text': 'File Format' },
           { 'value': '4', 'text': 'File Remark' },
           { 'value': '5', 'text': 'File Personnel' }
           //{ 'value': '6', 'text': '评审人员' },
           //{ 'value': '7', 'text': '签发人员' }
        ]
    });
    $('#search1').textbox({
        value: ''
    });
};

//搜索文件
function search_file() {
    var file_type = $('#File_type').tree('getSelected').text;
    var search = $('#search').combobox('getText');
    var search1 = $('#search1').textbox('getText');
    switch (search) {
        case "File Num": search = "FileNum"; break;
        case "File Name": search = "FileName"; break;
        case "File Format": search = "FileFormat"; break;
        case "File Remark": search = "FileRemark"; break;
        case "File Personnel": search = "FilePersonnel"; break;
        default: search = "";
    }

    $('#Environmental_management').datagrid(
        {
            type: 'POST',
            dataType: "json",
            url: "/ManageQuality/GetFileManagement",//接收一般处理程序返回来的json数据                
            queryParams: {
                search: search,
                key: search1,
                nodeText: file_type
            }
        }
   )
};

//增加文件
function Environmental_management_addfile() {
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
    var node = $('#File_type').tree('getSelected');//获取选中的树节点
    $('#add_file_save').unbind('click').bind('click', function () {
        $.messager.progress({
            text: '正在上传中...'
        });
        //form表单提交
        $('#add_file').form('submit', {
            url: "/ManageQuality/AddFileManagement",
            onSubmit: function (param) {
                param.FileTypeID = node.id //获取选中树节点的id传给后台
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.progress('close');
                    $('#add_file').dialog('close');//关闭添加文件的弹窗
                    $.messager.alert('Tips', result.Message);
                    $('#Environmental_management').datagrid('reload');//重新加载文件列表
                }
                else if (result.Success == false) {
                    $.messager.progress('close');
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    });
    $('#add_file').form('reset');//重置增加文件表单
    $('#FileType').textbox('setValue', node.text);//设置文件类型的文本框的值

};

//修改文件
function Environmental_management_alterfile() {
    var selectRow = $("#Environmental_management").datagrid("getSelected");//获取选中行
    var node = $('#File_type').tree('getSelected');//获取选中的树节点
    if (selectRow) {
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
                    $('#Environmental_management').datagrid('reload');
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
    var selectRow = $("#Environmental_management").datagrid("getSelected");
    if (selectRow) {
        $("#ShowOffice").prop("href", WordUrlSpit[0] + '?id=' + selectRow.id + '&Operation_Type=readonly&pageName=FileManagement' + WordUrlSpit[1]);
        document.getElementById('ShowOffice').click();
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
//删除文件
function h_delete() {
    var selectRow = $("#Environmental_management").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('Tips', 'Are you sure you want to delete the selected task？', function () {
            //if (r) {
            //    var selectRow = $("#Environmental_management").datagrid('getSelections');
            //    var a = "";
            //    for (var i = 0; i < selectRow.length; i++) {
            //        if (i == 0) {
            //            a = selectRow[i].id;
            //        }
            //        if (i > 0) {
            //            a = a + "," + selectRow[i].id;
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
                            $('#Environmental_management').datagrid('reload');
                        } else {
                            $.messager.alert('Tips', result.Message);

                        }
                    }
                });

            }
         
            //}
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};


//导出报表
function Environmental_managementexport() {
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
    var selectRow = $("#Environmental_management").datagrid('getSelections');
    var a = "";
    if ($("#all").val() != '1') {
        var selectRow = $("#Environmental_management").datagrid("getSelected");
        if (selectRow) {
            var selectRow = $("#Environmental_management").datagrid('getSelections');
            for (var i = 0; i < selectRow.length; i++) {
                if (i == 0) {
                    a = selectRow[i].id;
                }
                if (i > 0) {
                    a = a + "," + selectRow[i].id;
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
                        $('#Environmental_management').datagrid('reload');
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
                    $('#Environmental_management').datagrid('reload');
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
    var node1 = $('#File_type').tree('getSelected');
    $('#Environmental_management').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        border: true,
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
        sortName: 'FileNum',
        sortOrder: 'asc',
        toolbar: "#Environmental_management_toolbar"
    });
    //下载文件
    $('#Download_files').unbind('click').bind('click', function () {
        Download_files();
    });
};
/*
*functionName:Download_files
*function:函数功能 下载
*Param 参数 
*author:创建人 张慧敏
*date:时间
*/
function Download_files() {
    var selectRow = $("#Environmental_management").datagrid("getSelected");//获取选中行
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

}