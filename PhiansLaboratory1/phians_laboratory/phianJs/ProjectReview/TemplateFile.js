var WordUrlSpit = new Array();
var word_link;
var line = 0;
var word_edit;//设置一个在线编辑全局变量
$(function () {
    word_edit = $("#reports_edit_1").attr("href");//获取office控件链接 URL
    WordUrlSpit_edit = word_edit.split("?");//以问号对获取的数组进行分割
    //******************************************************************//
    //////////////////////////检测证书模板///////////////////////////////
    //*****************************************************************//
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    word_link = $("#ShowOffice").attr("href");//获取a链接 URL
    WordUrlSpit = word_link.split("?");
    var searchValue = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    //搜索
    $('#search').combobox({
        data: [
                { 'value': 'FileNum', 'text': '文件编号' },
                { 'value': 'FileName', 'text': '文件名' },
                { 'value': 'AddPersonnel', 'text': '添加人' }
        ]
    });
    //等级下拉框
    $('#ReviewLevel').combobox({
        value: 2,
        data: [
              { 'value': '2', 'text': 'Ⅱ' },
              { 'value': '3', 'text': 'Ⅲ' }
        ],
    });

    $('#report').datagrid(
    {
        //  title: '体系文件',
        // iconCls: 'icon-add',         
        nowrap: false,
        striped: true,
        rownumbers: true,
        //singleSelect: true,
        //autoRowHeight: true,
        border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        ctrlSelect: true,
        pageSize: num,
        pageList: [num, num + 10, num + 20, num + 20],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        url: "/ProjectReview/LoadReportTemplate",//接收一般处理程序返回来的json数据         
        columns: [[
        { field: 'FileNum', title: '模板编号', width: 50 },
        { field: 'FileName', title: '模板名称', width: 150 },
        { field: 'FileType_n', title: '模板类型', width: 100 },
        { field: 'FileFormat', title: '模板格式', width: 30 },
        { field: 'ReviewLevel', title: '审核级别', width: 30 },
        { field: 'AddPersonnel', title: '添加人', width: 50 },
        { field: 'AddDate', title: '添加时间', width: 30 },
        { field: 'Remark', title: '附注', width: 150 },

        ]],
        queryParams: {
            search: searchValue,
            key: key,
        },
        onLoadSuccess: function (data) {
            $('#report').datagrid('selectRow', line);

        },
        sortOrder: 'asc',
        toolbar: "#report_toolbar"
    });
    //var data = report_management();
    //$('#report').datagrid('hideColumn', 'url')
    //$('#report').datagrid('loadData', data);


    $('#FileType').combobox({
        url: "/Common/LoadReportType",
        valueField: 'Value',
        textField: 'Text',
        required: true,
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    //编辑模板
    $('#report_read_2').unbind('click').bind('click', function () {
        report_edit_word();
    });
    //阅读模板
    $('#report_read_1').unbind('click').bind('click', function () {
        report_read();
    });
    //添加模板
    $('#addTemplate').unbind('click').bind('click', function () {
        addTemplate();
    });
    //删除模板
    $('#deleteTemplate').unbind('click').bind('click', function () {
        deleteTemplate();
    });
    //修改信息
    $('#editTemplate').unbind('click').bind('click', function () {
        editTemplate();
    });
    //下载模板
    $('#DownloadReport').unbind('click').bind('click', function () {
        DownloadReport();
    });
    //搜索
    $("#search_info").unbind("click").bind("click", function () {
        search();
    });
})

//搜索

function search() {
    var selectRow = $("#report").datagrid("getSelected");
    var searchValue = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    $('#report').datagrid({
                url: "/ProjectReview/LoadReportTemplate",//接收一般处理程序返回来的json数据    
                queryParams: {
                    search: searchValue,
                    key: key,

                },
                onLoadSuccess: function (data) {
                    $('#report').datagrid('selectRow', 0);
                }
            }).datagrid('resize');
}

var new_id_url_ = $("#read_doc").prop("href");
function report_read() {
    var selectRow = $("#report").datagrid("getSelected");
    if (selectRow) {

        let cookie_val = getCookie("UserCount");
        console.log(WordUrlSpit_edit[0])

        $("#ShowOffice").prop("href", WordUrlSpit[0] + '?id=' + selectRow.ID + '&OperateType=Template&UserCount_=' + cookie_val + WordUrlSpit[1]);
        document.getElementById('ShowOffice').click();
    }
    else {

        $.messager.alert('提示', '请选择要操作的行！');
    }
}

//添加模板 修改信息
function addTemplate() {
    

    $("#UploadFiles").css('display', 'block');
    $('#importFileForm').form("reset");
    $("#importFileForm").dialog({
        width: 400,
        height: 380,
        modal: true,
        title: '添加模板',
        border: false,
        buttons: [
            {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    saveAddTemplate();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importFileForm').dialog('close');
                }
        }]

    })
   

}
//点击确定添加模板
function saveAddTemplate() {
    var FileNumValue = $("#FileNum").textbox("getValue");
    var FileNameValue = $("#FileName").textbox("getValue");
    var RemarkValue = $("#Remark").textbox("getValue");
    $('#importFileForm').form('submit', {
        url: "/ProjectReview/ReportTemplateAdd",
        onSubmit: function (param) {
            //param.FileNum = FileNumValue;
            //param.FileName = FileNameValue;
            //param.Remark = RemarkValue;
            return $(this).form('enableValidation').form('validate');
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#report').datagrid('reload');
                $('#importFileForm').dialog('close');
                $.messager.alert('提示', result.Message);
            } else {
                $.messager.alert('提示', result.Message);
            }

        }
    });
}


function editTemplate() {
    var selectRow = $("#report").datagrid("getSelected");
    var rows = $("#report").datagrid("getSelections");
    $("#UploadFiles").css('display','none')
    if (selectRow) {
    $("#importFileForm").dialog({
        width: 400,
        height: 380,
        modal: true,
        title: '修改信息',
        border: false,
        buttons: [
            {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    saveEditTemplate()
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importFileForm').dialog('close');
                }
            }]
    });
        //点击修改 渲染数据到form表单
    $("#importFileForm").form('load', rows[0]);
    } else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
}

//点击修改文件修改
function saveEditTemplate() {
    var selectRow = $("#report").datagrid("getSelected");
    //form表单提交
    $('#importFileForm').form('submit', {
        url: "/ProjectReview/ReportTemplateEdit",
        onSubmit: function (param) {
            console.log(param);
            param.ID = selectRow.ID
            
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#report').datagrid('reload');
                $('#importFileForm').dialog('close');
                $.messager.alert('提示', result.Message);
            } else {
                $.messager.alert('提示', result.Message);
            }

        }
    });
  
}

//删除模板
function deleteTemplate() {
    var selected_report = $("#report").datagrid("getSelected");
    if (selected_report) {
        $.messager.confirm('删除任务提示', '您确认要删除选中模板文件吗', function (r) {
            if (r) {
                $.ajax({
                    url: "/ProjectReview/ReportTemplateDel",
                    dataType: "text",
                    type: 'POST',
                    data: {
                        ID: selected_report.ID,
                        FileName: selected_report.FileName
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#report').datagrid('reload');
                            $.messager.alert('提示', '删除信息成功');

                        }
                        else {
                            $.messager.alert('提示', result.Message);
                        }

                    }
                });
            }
        })

    } else {
        $.messager.alert('提示', '请选择报告');
    }
}
//获取cookie值
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

//编辑word文档

var newedit_id_url_ = $("#edit_word").prop("href");
function report_edit_word() {
    var selectRow = $("#report").datagrid("getSelected");
    if (selectRow) {
        //$.ajax({
        //    url: "OfficeOperate/WordEdit",
        //    type: 'POST',
        //    data: {
        //        id: selectRow.id,
        //        field_name: selectRow.field_name,
        //        field_fomat: selectRow.field_fomat,
        //        file_url: selectRow.file_url
        //    },
        //    success: function (data) {
        //        //alert(data);
        //        //if (data == 1) {
        //        //    $.messager.confirm('提示', '只允许阅读格式为 *.pdf,*.doc,*.docx 格式的文档！', function (r) { });
        //        //} else {
        //        //    var urls = new Array(); //定义一数组 
        //        //    urls = (data).split("，"); //字符分割 
        //        //    //alert(urls[1]);
        //        //    if (urls[1] == ".doc" || urls[1] == ".docx") {
        //        //        $("#edit_word").prop("href", newedit_id_url_ + "___hxw&url=" + urls[0]);
        //        //        document.getElementById('edit_word').click();

        //        //    }
        //        //    if (urls[1] == ".pdf") {


        //        //        var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + urls[0];
        //        //        window.parent.addTab("模板文件查看", url_);
        //        //    }
        //        //}
        //    }
        //});
        let cookie_val = getCookie("UserCount");
        console.log(WordUrlSpit_edit[0])
        $("#reports_edit_1").prop("href", WordUrlSpit_edit[0] + "?id=" + selectRow.ID + '&save_type=certificate_Template&OperateType=Template&UserCount_=' + cookie_val + WordUrlSpit_edit[1]);
        document.getElementById('reports_edit_1').click();
    }
    else {

        $.messager.alert('提示', '请选择要操作的行！');
    }
}

//下载模板
function DownloadReport() {
    var selected = $("#report").datagrid("getSelected");
    DownloadFile(selected.FileUrl, selected.FileName, selected.FileFormat);
}

//搜索证书文件
function search2() {

    var search2 = $('#search2').combobox('getText');
    var search3 = $('#search3').combobox('getText');
    switch (search2) {
        case "文件编号": search2 = "field_num"; break;
        case "文件名称": search2 = "field_name"; break;
        default: search2 = "";
    }

    $('#report').datagrid(
        {
            type: 'POST',
            dataType: "json",
            url: "Template_file.ashx?&cmd=report_search",//接收一般处理程序返回来的json数据                
            queryParams: {
                search: search2,
                search1: search3
            }

        });
    $('#search2').combobox({
        //url: 'combobox_data.json',
        //valueField: 'id',
        //textField: 'text'
        data: [
       { 'value': '0', 'text': '文件编号' },
       { 'value': '1', 'text': '文件名称' }
        ]
    });
    $('#search3').textbox({

        value: '请输入搜索内容 '
        // onfocus:"this.value=''"

    });

}

/*
*functionName:
*function:下载图片、文件
*Param: 文件路径：fileurl_ 
*Param: 文件名称：filename_ 
*Param: 文件格式：fileFormat_ 
*author:黄小文
*date:2018-11-09
*/
function DownloadFile(fileurl_, filename_, fileFormat_) {


    if (fileFormat_ == "" || fileFormat_ == null) {
        $.messager.alert('提示', '非文件无法下载');

        return;
    }

    var DataFormat = fileFormat_.toLowerCase();
    //ie 下载
    if (!!window.ActiveXObject || "ActiveXObject" in window) {//判断是否为IE
        // alert(1);
        //图片的保存方法
        if (DataFormat == ".png" || DataFormat == ".jpg" || DataFormat == ".jpeg") {
            if (window.navigator.msSaveBlob) {//IE10+方法           
                var canvas_ = document.createElement('canvas');
                var img_ = document.createElement('img');
                img_.onload = function (e) {
                    canvas_.width = img_.width;
                    canvas_.height = img_.height;
                    var context = canvas_.getContext('2d');
                    context.drawImage(img_, 0, 0, img_.width, img_.height);
                    window.navigator.msSaveBlob(canvas_.msToBlob(), filename_ + fileFormat_);///保存图片
                }
                img_.src = fileurl_;
            }
        }
        else {

            window.location = fileurl_;
        }
    }
        //谷歌等浏览器下载方法
    else {

        var $a = $("<a></a>").attr("href", fileurl_).attr("download", filename_ + fileFormat_);
        $a[0].click();


    }


}