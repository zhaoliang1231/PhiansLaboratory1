

//加载信息
$(function () {
    //搜索
    $('#search').combobox({
        data: [
                { 'value': 'contract_num', 'text': '委托单号' },
                { 'value': 'Certificate_num', 'text': '报告编号' }
        ]
    })

});

$(function () {
    //管理待处理/已处理
    management_all();
    //加载列表
    load_list();

    //搜索
    $("#search_info").unbind('click').bind('click', function () {
        search();
    });
    //预览报告
    $('#Preview_Report').unbind('click').bind('click', function () {

        //判断是待审核还是历史
        var flag=$('#management_all').combobox("getValue");
        if (flag == "0") {
            view_report();

        } else {
            view_report1();
        }
      
    });

    //退回报告编制
    $('#Back_report').unbind('click').bind('click', function () {
        back_report();
    });

    //通过
    $('#Submit_report').unbind('click').bind('click', function () {
        submit_report();
    });
    //查看申请原因
    $('#view_return_info').unbind('click').bind('click', function () {
        Back_report_dialog();
    });

});

//加载列表
function load_list() {
    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    //显示分页条数
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    var search = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    var history_flag = $('#management_all').combobox('getValue');
    //按钮显示隐藏
    if (history_flag == "0") {
        $(".link_button").css("display", "block");
    } else {
        $(".link_button").css("display", "none");
    }
    //检测报告信息
    $('#report_review_task').datagrid(
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
           url: "LosslessReport_Apply_Audit.aspx?&cmd=load_list",//接收一般处理程序返回来的json数据        
           columns: [[
             { field: 'report_num', title: '报告编号'},
             { field: 'report_name', title: '报告名称' },
             { field: 'clientele', title: '委托人' },
             { field: 'clientele_department', title: '用户(委托部门)' },
             { field: 'File_format', title: '证书url', hidden: 'true' },
             { field: 'error_remark', title: '申请信息' },        
             { field: 'accept_personnel_n', title: '申请人' },
             {
                 field: 'accept_date', title: '申请时间', formatter: function (value, row, index) {
                     if (value) {
                         if (value.length > 10) {
                             value = value.substr(0, 10)
                             return value;
                         }
                     }
                 }
             },
              { field: 'review_personnel_n', title: '评审人' },
             {
                 field: 'review_date', title: '评审时间', formatter: function (value, row, index) {
                     if (value) {
                         if (value.length > 10) {
                             value = value.substr(0, 10)
                             return value;
                         }
                     }
                 }
             },
            { field: 'review_remarks', title: '评审说明' },
            { field: 'false_review_remarks', title: '退回评审说明' },
           { field: 'constitute_personnel', title: '报告编制人' },
           {
               field: 'constitute_date', title: '编制时间', formatter: function (value, row, index) {
                   if (value) {
                       if (value.length > 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
            { field: 'review_personnel_word', title: '报告评审人' },
             {
                 field: 'review_personnel_word_date', title: '报告评审时间', formatter: function (value, row, index) {
                     if (value) {
                         if (value.length > 10) {
                             value = value.substr(0, 10)
                             return value;
                         }
                     }
                 }
             },
           {
               field: 'accept_state', title: '处理状态', formatter: function (value, row, index) {
                   var return_value;
                 
                   switch (value) {
                       case "0":
                           return_value = "报废申请审核";
                           break;
                       case "1":
                           return_value = "异常申请审核";
                           break;
                       case "2":
                           return_value = "异常申请退回";
                           break;
                       case "3":
                           return_value = "异常编制";
                           break;
                       case "4":
                           return_value = "异常编制审核";
                           break;
                       case "5":
                           return_value = "异常编制审核退回";
                           break;
                       case "6":
                           return_value = "异常编制完成";
                           break;
                       case "7":
                           return_value = "报废申请审核退回";
                           break;
                       case "8":
                               return_value = "报废完成";
                               break;
                   }

                   return return_value;


               }
           },
           { field: 'other_remarks', title: '附注' }
           ]],
           queryParams: {
               search: search,
               key: key,
               history_flag: history_flag

           },
           onLoadSuccess: function (data) {
               //默认选择行
               $('#report_review_task').datagrid('selectRow', 0);
           },
           onSelect: function () {
               //提示
               //$('#report_review_task').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
               //view_taskinfo1();
           },
           //rowStyler: function (index, row) {
           //    if (row.return_flag == "False") {
           //        //return 'background-color:pink;color:blue;font-weight:bold;';
           //        return 'color:black;'; //green
           //    }
           //    if (row.return_flag == "True") {
           //        //return 'background-color:pink;color:blue;font-weight:bold;';
           //        return 'color:red;';
           //    }
           //},
           sortOrder: 'asc',
           toolbar: "#reports_toolbar"
       });
}

//搜索
function search() {
    var search = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    var history_flag = $('#management_all').combobox('getValue');
    $('#report_review_task').datagrid(
    {
        url: "LosslessReport_Apply_Audit.aspx?&cmd=load_list",//接收一般处理程序返回来的json数据    
        queryParams: {
            search: search,
            key: key,
            history_flag: history_flag

        },
        onLoadSuccess: function (data) {
            $('#report_review_task').datagrid('selectRow', 0);
        }
    }).datagrid('resize');
}
//预览报告
var new_url = $("#read_doc").prop("href");
function view_report() {
    var read_doc_times = 0;
    var selected_report = $("#report_review_task").datagrid("getSelected");
    if (selected_report) {
        $.ajax({
            url: "LosslessReport_Apply_Audit.aspx?&cmd=check_url",
            dataType: "text",
            type: 'POST',
            data: {
                File_url: selected_report.File_url,
            },
            success: function (data) {
                if (data == 'F') {
                    $.messager.alert('提示', '报告不存在，请退回报告编制！');
                } else {
                  
                    $("#read_doc").prop("href", new_url + "___hxw&url=" + data);
                    document.getElementById('read_doc').click();
                }
            }
        });

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}
//预览报告pdf
function view_report1() {
    var selected_report = $("#report_review_task").datagrid("getSelected");
    if (selected_report) {
        $.ajax({
            url: "LosslessReport_Management.ashx?&cmd=Preview_Report",
            type: 'POST',
            data: {
                id: selected_report.report_id
            },
            success: function (data) {
                if (data == 'F') {
                    $.messager.alert('提示', '预览错误！');
                } else {

                    var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + data;
                    window.parent.addTab(selected_report.report_num + "报告文件查看", url_);
                }

            }
        });
    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');
    }
}

//退回申请审核
function back_report() {
    var selected_report = $("#report_review_task").datagrid("getSelected");
    if (selected_report) {

        $('#Back_report_dialog').dialog({
            width: 420,
            height: 300,
            modal: true,
            title: '退回申请审核',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    submit_audit(selected_report);
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Back_report_dialog').dialog('close');
                }
            }]
        });

       
    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }

    function submit_audit(selected_report) {
        var flag = "";
        if (selected_report.accept_state == "0") {
            flag = "Scrap"
        }
        $('#Back_report_dialog').form('submit', {
            url: "LosslessReport_Apply_Audit.aspx",//接收一般处理程序返回来的json数据     
            onSubmit: function (param) {
                param.cmd = 'audit';
                param.flag = flag;
                param.id = selected_report.id;
                param.report_id = selected_report.report_id;
                param.report_num = selected_report.report_num;
                param.accept_personnel = selected_report.accept_personnel;
                param.constitute_personnel = selected_report.constitute_personnel;
                param.type = "0";
            },
            success: function (data) {
                if (data == 'T') {
                    $('#reports_review_dialog').dialog('close');
                    $('#Back_report_dialog').dialog('close');
                    $.messager.alert('提示', '退回成功！');
                    $('#report_review_task').datagrid('reload');
                }
                else {
                    $.messager.alert('提示', '退回失败！');

                }
            }
        });
    }
      
}

//通过
function submit_report() {
    var selected_report = $("#report_review_task").datagrid("getSelected");
    var flag = "";
    if (selected_report.accept_state == "0") {
        flag = "Scrap"
    }
    if (selected_report) {
        $.messager.confirm('提交提示', '是否通过审核', function (r) {
            if (!r) {
                return;
            }
            $.ajax({
                url: "LosslessReport_Apply_Audit.aspx?&cmd=audit",
                type: 'POST',
                data: {
                    //reports_num: select_report_ed.reports_num,
                    flag: flag,
                    report_num: selected_report.report_num,
                    report_id : selected_report.report_id,
                    accept_personnel : selected_report.accept_personnel,
                    constitute_personnel : selected_report.constitute_personnel,
                    id: selected_report.id,
                    type: "1",
                 
                },
                success: function (data) {
                    if (data == "T") {
                       $('#reports_review_dialog').dialog('close');
                        $.messager.alert('提示', '提交成功！');
                        $('#report_review_task').datagrid('reload');

                    }S

                }
            });

        });

    } else {
        $.messager.alert('提示', '请选择要操作的报告！');
    }
}
//查看申请原因
function Back_report_dialog() {
    var selected_report = $("#report_review_task").datagrid("getSelected");
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

//管理待处理/已处理
function management_all() {

    $('#management_all').combobox({
        value: 0,
        data: [
              { 'value': '0', 'text': '待审核' },
              { 'value': '1', 'text': '历史审核' }
        ],
        onSelect: function () {

            //历史编制  1    未提交 0

            load_list();
        }
    });

}
