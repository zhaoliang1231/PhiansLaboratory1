var line = 0;//定义列表的行
var word_link;
var WordUrlSpit = new Array();
$(function () {
    word_link = $("#ShowOffice").attr("href");//获取office控件链接 URL
    WordUrlSpit = word_link.split("?");
    //WordUrlSpit[0]头部
    //WordUrlSpit[1]尾部

    var tabs_width = screen.width;
    //资质证书列表的布局
    $('#Layout').layout('panel', 'west').panel('resize', {
        width: tabs_width - 600
    });
    //修改个人资料的布局
    $('#Layout').layout('panel', 'center').panel('resize', {
    });
    //重置布局
    $('#Layout').layout('resize');

    qualification_certificate()//证书信息列表
    personal_information();//个人资料初始化

    //性别下拉框
    $('#UserNsex').combobox({
        panelHeight: 50,
        editable: false,
        data: [
           { 'value': true, 'text': '女' },
           { 'value': false, 'text': '男' }
        ]
    });
});

//********************************************************************证书信息列表初始化********************************************************************
//********************************************************************加载证书信息列表
function qualification_certificate() {
    $('#qualification_certificate').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        fit: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/PersonalCenter/GetUserCertificate",
        columns: [[
           { field: 'CertificateNum', title: '证书编号', width: 100,sort:true },
           { field: 'CertificateName', title: '证书名字', width: 120 },
           { field: 'CertificateType_n', title: '证书类型', width: 100 },
           { field: 'IssuingUnit', title: '发证单位', width: 100 },
           {
               field: 'IssueDate', title: '发证时间', width: 100, formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           {
               field: 'ValidDate', title: '有效时间', width: 100, formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           { field: 'Profession', title: '专业', width: 100 },
           { field: 'Quarters', title: '岗位', width: 100 },
           { field: 'Grade', title: '等级', width: 50 },
           {
               field: 'CertificateSate', title: '证书状态', width: 100, formatter: function (value, row, index) {//样品标识
                   if (value == 0) {
                       return "Invalid";
                   } else if (value == 1) {
                       return "Effective";
                   }
               }
           },
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
           { field: 'CreatePersonnel_n', title: '创建', width: 120 },
           {
               field: 'remarks', title: '说明', width: 120, formatter: function (value, row, index) {
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
            $('#qualification_certificate').datagrid('selectRow', line);
        },
        rowStyler: function (index, row) {
            switch (row.CertificateSate) {
                case 0: return 'color:red;'; break;
            }
        },
        SortName:'CertificateNum',
        sortOrder: 'asc',
        toolbar: "#qualification_certificate_toolbar"
    });
    //查看证书附件
    $('#view_certificate_attachments').unbind('click').bind('click', function () {
        view_certificate_attachments();
    });
    ////到期预警
    //$('#due_warning').unbind('click').bind('click', function () {
    //    due_warning();
    //});
    ////过期查询
    //$('#overdue_query').unbind('click').bind('click', function () {
    //    overdue_query();
    //});
    ////显示全部
    //$('#show_all').unbind('click').bind('click', function () {
    //    show_all();
    //});
};
//********************************************************************查看证书附件初始化********************************************************************
//********************************************************************查看证书附件列表
function view_certificate_attachments() {
    var select_certificate = $("#qualification_certificate").datagrid("getSelected");//获取选中证书信息行
    if (select_certificate) {
        $('#certificate_annex_dialog').dialog({
            width: 800,
            height: 500,
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
               { field: 'CertificateId', title: 'CertificateId', width: 100, hidden: true },
               { field: 'DocumentName', title: '文件名称', width: 130 },
               { field: 'DocumentUrl', title: '文件地址', width: 150, hidden: true },
               { field: 'DocumentFormat', title: '文件格式', width: 120 },
               { field: 'CreateDate', title: '上传日期', width: 100 },
               { field: 'CreatePersonnel_n', title: '上传人', width: 120 }
            ]],
            onLoadSuccess: function (data) {
                $('#annex_datagrid').datagrid('selectRow', line);
            },
            toolbar: "#certificate_annex_toolbar"
        });
    } else {
        $.messager.alert('提示', '请选择您需要操作的行！');
    }
    //阅读证书附件
    $('#view_annex').unbind('click').bind('click', function () {
        view_annex();
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

//********************************************************************个人资料********************************************************************
function personal_information() {
    $.ajax({
        url: "/PersonalCenter/GetUser",
        type: 'POST',
        success: function (result) {
            if (result) {
                var obj = $.parseJSON(result);
                //回显数据
                console.log(obj)
                $('#department_people_info').form('reset');
                $("#Signature").attr("src", obj.Data.Portrait);
                $("#SignatureModification").attr("src", obj.Data.Signature);
                $("#department_people_info").form('load', obj.Data);
            }
        }
    });
    

    //保存签名
    $('#save_image').unbind('click').bind('click', function () {
        save_image();
    });
    //保存签名
    $('#edit_image').unbind('click').bind('click', function () {
        edit_image();
    });
    //修改资料
    $('#save_personnel_info').unbind('click').bind('click', function () {
        save_personnel_info();
    });
};
//********************************************************************保存头像********************************************************************
function save_image() {
    var Img_url = $('#Signature').attr("src");
    var Signature = $('#Url_image').filebox('getValue');//获取签名的地址
    if (Signature =="") {
        $.messager.alert('提示', '请选择照片');
    } else {
        $('#info_form').form('submit', {
            url: "/PersonalCenter/UploadUserPortrait",//接收一般处理程序返回来的json数据    
            onSubmit: function (param) {
                param.Portrait = Img_url
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $("#Signature").attr("src", result.Data.Portrait);//照片回显
                        $.messager.alert('提示', result.Message);
                        location.reload();
                    }
                    else if (result.Success == false) {
                        $.messager.alert('提示', result.Message);
                    }
                }
            }
        });
    }
    
};
//********************************************************************修改签名********************************************************************
function edit_image() {
    var Img_url = $('#SignatureModification').attr("src");
    var Modification_image = $('#Modification_image').filebox('getValue');//获取签名值
    if (Modification_image == "") {
        $.messager.alert('提示', '请选择照片');
    } else {
        $('#info_form1').form('submit', {
            url: "/PersonalCenter/UpdateUserImg",//接收一般处理程序返回来的json数据    
            onSubmit: function (param) {
                param.Signature = Img_url
                //param.files = Modification_image
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $("#SignatureModification").attr("src", result.Data.Signature);//照片回显
                        $.messager.alert('提示', result.Message);
                        location.reload();
                    }
                    else if (result.Success == false) {
                        $.messager.alert('提示', result.Message);
                    }
                }
            }
        });
    }
    
};
//********************************************************************修改资料********************************************************************
function save_personnel_info() {
    $('#department_people_info').form('submit', {
        url: "/PersonalCenter/UpdateUser",//接收一般处理程序返回来的json数据     
        ajax: true,
        success: function (data) {
            if (data) {
                var obj = $.parseJSON(data);
                if (obj.Success == true) {
                    $.messager.alert('提示', obj.Message);
                    $('#department_people_info').form('load')
                }
                else if (obj.Success == false) {
                    $.messager.alert('提示', obj.Message);
                }
            }
        }
    });
};
////到期预警
//function due_warning() {
//    $('#qualification_certificate').datagrid({
//        url: "",
//        dataType: "json",
//        type: 'POST'
//    });
//};
////过期查询
//function overdue_query() {
//    $('#qualification_certificate').datagrid({
//        url: "",
//        dataType: "json",
//        type: 'POST'
//    });
//};
////显示全部
//function show_all() {
//    $('#qualification_certificate').datagrid({
//        url: "",
//        dataType: "json",
//        type: 'POST'
//    });
//};