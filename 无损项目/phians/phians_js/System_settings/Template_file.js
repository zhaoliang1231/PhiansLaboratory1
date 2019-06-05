var line = 0;
$(function () {

    //******************************************************************//
    //////////////////////////检测证书模板///////////////////////////////
    //*****************************************************************//
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);

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
        url: "Template_file.ashx?&cmd=report_load",//接收一般处理程序返回来的json数据         
        columns: [[
        { field: 'field_name', title: '文件名称' },
        { field: 'field_fomat', title: '文件格式' },
        { field: 'url', title: '路径' },

        ]],
        
        onLoadSuccess: function (data) {
            $('#report').datagrid('selectRow', line);

        },
        sortOrder: 'asc',
        toolbar: "#report_toolbar"
    });
    var data = report_management();
    $('#report').datagrid('hideColumn', 'url')
    $('#report').datagrid('loadData', data);



    //编辑模板
    $('#report_read_2').unbind('click').bind('click', function () {
        report_edit_word();
    });
    //阅读模板
    $('#report_read_1').unbind('click').bind('click', function () {
        report_read();
    });
    //阅读模板
    $('#DownloadReport').unbind('click').bind('click', function () {
        DownloadReport();
    });

})
var new_id_url_ = $("#read_doc").prop("href");
function report_read() {
    var selectRow = $("#report").datagrid("getSelected");
    if (selectRow) {
        $.ajax({
            url: "Template_file.ashx?&cmd=report_read",
            type: 'POST',
            data: {
                id: selectRow.id,
                field_name: selectRow.field_name,
                field_fomat: selectRow.field_fomat,
                file_url: selectRow.file_url
            },
            success: function (data) {
                //alert(data);
                if (data == 1) {
                    $.messager.confirm('提示', '只允许阅读格式为 *.pdf,*.doc,*.docx 格式的文档！', function (r) { });
                } else {
                    var urls = new Array(); //定义一数组 
                    urls = (data).split("，"); //字符分割 
                    //alert(urls[1]);
                    if (urls[1] == ".doc" || urls[1] == ".docx") {
                        $("#read_doc").prop("href", new_id_url_ + "___hxw&url=" + urls[0]);
                        document.getElementById('read_doc').click();

                    }
                    if (urls[1] == ".pdf") {


                        var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + urls[0];
                        window.parent.addTab("模板文件查看", url_);
                    }
                }
            }
        });
    }
    else {

        $.messager.alert('提示', '请选择要操作的行！');
    }
}
//编辑word文档

var newedit_id_url_ = $("#edit_word").prop("href");
function report_edit_word() {
    var selectRow = $("#report").datagrid("getSelected");
    if (selectRow) {
        $.ajax({
            url: "Template_file.ashx?&cmd=report_Edit",
            type: 'POST',
            data: {
                id: selectRow.id,
                field_name: selectRow.field_name,
                field_fomat: selectRow.field_fomat,
                file_url: selectRow.file_url
            },
            success: function (data) {
                //alert(data);
                if (data == 1) {
                    $.messager.confirm('提示', '只允许阅读格式为 *.pdf,*.doc,*.docx 格式的文档！', function (r) { });
                } else {
                    var urls = new Array(); //定义一数组 
                    urls = (data).split("，"); //字符分割 
                    //alert(urls[1]);
                    if (urls[1] == ".doc" || urls[1] == ".docx") {
                        $("#edit_word").prop("href", newedit_id_url_ + "___hxw&url=" + urls[0]);
                        document.getElementById('edit_word').click();

                    }
                    if (urls[1] == ".pdf") {


                        var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + urls[0];
                        window.parent.addTab("模板文件查看", url_);
                    }
                }
            }
        });
    }
    else {

        $.messager.alert('提示', '请选择要操作的行！');
    }
}

//下载模板
function DownloadReport() {
    var selected = $("#report").datagrid("getSelected");
    alert(selected.url);
    window.location = selected.url;
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

function report_management() {
    var data = {
        "total": 2,
        "rows": [
            {
                "field_name": "热处理模板3",
                "field_fomat": ".docx",
                "url": "/upload_Folder/Lossless_report/1.docx"
            },
            {
                "field_name": "目视报告模板_VT_63",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/2.doc"
            },
            {
                "field_name": "DT模板空白_5版",
                "field_fomat": ".docx",
                "url": "/upload_Folder/Lossless_report/3.docx"
            },
            {
                "field_name": "ECT-涡流检验报告模板RPV",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/4.doc"
            },
            {
                "field_name": "ECT-涡流检验报告模板SG",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/5.doc"
            },
            {
                "field_name": "LT-氦检漏报告模板_4版123",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/6.doc"
            },
            {
                "field_name": "MT-磁轭法和触头法磁粉检验报告3版123",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/7.doc"
            },
            {
                "field_name": "MT-磁粉检验报告床式",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/8.doc"
            },
            {
                "field_name": "UT-超声波测厚报告",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/10.doc"
            },
            {
                "field_name": "UT-超声波检验报告1-正页",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/11.doc"
            },
            {
                "field_name": "水压试验报告模板21",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/12.doc"
            },
            {
                "field_name": "自动超声波检验报告1-检验报告",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/13.doc"
            },
            {
                "field_name": "PT-液体渗透检验报告",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/26.doc"
            },
            {
                "field_name": "RT-射线检验报告1",
                "field_fomat": ".doc",
                "url": "/upload_Folder/Lossless_report/27.doc"
            }
        ]
    };
    return data;
}