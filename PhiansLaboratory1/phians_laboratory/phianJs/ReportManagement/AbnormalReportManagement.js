var WordUrlSpit = new Array();//new一个新的数组
var word_link;//设置一个全局变量

//报告信息
$(function () {
    word_link = $("#WordRead").attr("href");//获取office控件链接 URL
    WordUrlSpit = word_link.split("?");//以问号对获取的数组进行分割

    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    //显示分页条数
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    //检测报告信息
    $('#Report_management').datagrid(
       {
           nowrap: false,
           striped: true,
           rownumbers: true,
           singleSelect: true,
           //autoRowHeight: true,
           border: false,
           fitColumns: true,
           fit: true,
           pagination: true,
           pageSize: num,
           pageList: [num, num + 10, num + 20, num + 30],
           pageNumber: 1,
           type: 'POST',
           dataType: "json",
           url: "/ReportManagement/LoadUnusualCertificateManagementList",//接收一般处理程序返回来的json数据        
           queryParams: {
               
           },
           columns: [[
                   { field: 'report_num', title: '报告编号', sortable: 'true',width:100 },
                   { field: 'report_name', title: '报告名称', width: 100 },
                   { field: 'clientele', title: '委托人', width: 100 },
                   { field: 'clientele_department', title: '用户(委托部门)', width: 100 },
                   { field: 'File_format', title: '证书url', hidden: 'true', width: 100 },

                   { field: 'error_remark', title: '申请信息', width: 100 },
                   { field: 'accept_personnel_n', title: '申请人', width: 100 },
                   {
                       field: 'accept_date', title: '申请时间', formatter: function (value, row, index) {
                           if (value) {
                               if (value.length >= 10) {
                                   value = value.substr(0, 10)
                                   return value;
                               }
                           }
                       }, width: 100
                   },
                    { field: 'review_personnel_n', title: '评审人', width: 100 },
                   {
                       field: 'review_date', title: '评审时间', formatter: function (value, row, index) {
                           if (value) {
                               if (value.length >= 10) {
                                   value = value.substr(0, 10)
                                   return value;
                               }
                           }
                       }, width: 100
                   },
                  { field: 'review_remarks', title: '评审说明', width: 100 },
                  { field: 'false_review_remarks', title: '退回评审说明', width: 100 },
                 { field: 'constitute_personnel_n', title: '报告编制人', width: 100 },
                 {
                     field: 'constitute_date', title: '编制时间', formatter: function (value, row, index) {
                         if (value) {
                             if (value.length >= 10) {
                                 value = value.substr(0, 10)
                                 return value;
                             }
                         }
                     }, width: 100
                 },
                  { field: 'review_personnel_word_n', title: '报告评审人', width: 100 },
                   {
                       field: 'review_personnel_word_date', title: '报告评审时间', formatter: function (value, row, index) {
                           if (value) {
                               if (value.length >= 10) {
                                   value = value.substr(0, 10)
                                   return value;
                               }
                           }
                       }, width: 100
                   },
                 {
                     field: 'accept_state', title: '处理状态', formatter: function (value, row, index) {
                         var return_value;
                         switch (value) {
                             case 0:
                                 return_value = "报废申请审核";
                                 break;
                             case 1:
                                 return_value = "异常申请审核";
                                 break;
                             case 2:
                                 return_value = "异常申请退回";
                                 break;
                             case 3:
                                 return_value = "异常编制";
                                 break;
                             case 4:
                                 return_value = "异常编制审核";
                                 break;
                             case 5:
                                 return_value = "异常编制审核退回";
                                 break;
                             case 6:
                                 return_value = "异常处理完成";
                                 break;
                             case 7:
                                 return_value = "报废申请审核退回";
                                 break;
                             case 8:
                                 return_value = "报废异常完成";
                                 break;
                         }

                         return return_value;

                     }, width: 100
                 },
                 { field: 'other_remarks', title: '附注', width: 100 }
           ]],
           onLoadSuccess: function (data) {
               //默认选择行
               $('#Report_management').datagrid('selectRow', 0);
           },
           sortOrder: 'asc',
           toolbar: "#reports_toolbar"
       });

    //搜索
    $('#search').combobox({
        value:'report_num',
        data: [
                { 'value': 'report_num', 'text': '报告编号' }
        ]
    });
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

    //预览报告
    $('#read_report').unbind('click').bind('click', function () {
        var selected_report = $("#Report_management").datagrid("getSelected");
        if (selected_report) {
            //$.ajax({
            //    url: "/ReportManagement/Preview_Report",
            //    type: 'POST',
            //    data: {
            //        id: select_report_ed.report_id
            //    },
            //    success: function (data) {
            //        var result = $.parseJSON(data);
            //        if (result.Success == true) {
            //            $.messager.alert('提示',result.Message );
            //        } else {

            //            var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + data;
            //            window.parent.addTab(select_report_ed.report_num + "报告文件查看", url_);
            //        }

            //    }
            //});
            let cookie_val = getCookie("UserCount");

            $("#WordRead").prop("href", WordUrlSpit[0] + '?id=' + selected_report.id + "&OperateType=ErrorReport&UserCount_=" + cookie_val + WordUrlSpit[1]);
            document.getElementById('WordRead').click();

        } else {
            $.messager.alert('提示', '请选择报告要操作的报告');
        }
    });

    //搜索
    $("#search_info").unbind('click').bind('click', function () {
        $('#Report_management').datagrid(
            {
                url: "/ReportManagement/LoadUnusualCertificateManagementList",//接收一般处理程序返回来的json数据    
                queryParams: {
                    search: $("#search").combobox('getValue'),
                    key: $("#search1").textbox('getText')
                },
                onLoadSuccess: function (data) {
                    $('#Report_management').datagrid('selectRow', 0);
                }
            }).datagrid('resize');
    });
    //查看申请原因
    $('#view_return_info').unbind('click').bind('click', function () {
        Back_report_dialog();
    });
});
//查看申请原因
function Back_report_dialog() {
    var selected_report = $("#Report_management").datagrid("getSelected");
    if (selected_report) {
        $('#Back_report_dialog').dialog({
            width: 420,
            height: 300,
            modal: true,
            title: '申请原因',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Back_report_dialog').dialog('close');
                }
            }]
        });
        $('#return_info').textbox('setText', selected_report.error_remark);
        $('#remarks').textbox('setText', selected_report.other_remarks);
    }

}

