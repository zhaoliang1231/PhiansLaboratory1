
//加载信息
$(function () {

    //搜索
    $('#search').combobox({
        data: [
                { 'value': 'contract_num', 'text': '委托编号' },
                { 'value': 'Certificate_num', 'text': '报告编号' }
        ]
    });

});

//检测报告编制-报告信息
$(function () {
    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    //显示分页条数
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    //管理处理/已经处理的
    management_all()
    //加载列表
    load_list();

    //搜索
    $("#search_info").unbind("click").bind("click", function () {
        search();
    });

    //提交审核报告
    $('#Submit_report').unbind('click').bind('click', function () {
        submit_report();
    });

    //检测报告在线编辑
    $('#Edit_online_report').unbind('click').bind('click', function () {
        edit_online();
    });
    //查看申请原因
    $('#view_return_info').unbind('click').bind('click', function () {
        Back_report_dialog();
    });
    //预览报告
    $('#Preview_Report').unbind('click').bind('click', function () {
        //判断是待审核还是历史
        var flag = $('#management_all').combobox("getValue");
        if (flag == "0") {
            view_report();

        } else {
            view_report1();
        }
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
    $('#report_edit_task').datagrid(
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
           url: "LosslessReport_EditApply_Edit.aspx?&cmd=load_list",//接收一般处理程序返回来的json数据        
           columns: [[
              { field: 'report_num', title: '报告编号', sortable: 'true' },
              { field: 'report_name', title: '报告名称' },
              { field: 'clientele', title: '委托人' },
              { field: 'clientele_department', title: '用户(委托部门)' },
              { field: 'File_format', title: '证书url',hidden:'true' },
           
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
            { field: 'constitute_personnel_n', title: '报告编制人' },
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
             { field: 'review_personnel_word_n', title: '报告评审人' },
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
               $('#report_edit_task').datagrid('selectRow', 0);
           },
           onSelect: function () {
               //提示
               //  $('#report_edit_task').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
               //view_taskinfo1();
           },
           rowStyler: function (index, row) {
           },
           sortOrder: 'asc',
           toolbar: reports_toolbar
       });
}

//搜索

function search() {
    var search = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    var history_flag = $('#management_all').combobox('getValue');
    $('#report_edit_task').datagrid(
            {
                url: "LosslessReport_EditApply_Edit.ashx?&cmd=load_list",//接收一般处理程序返回来的json数据    
                queryParams: {
                    search: search,
                    key: key,
                    history_flag: history_flag

                },
                onLoadSuccess: function (data) {
                    $('#report_edit_task').datagrid('selectRow', 0);
                }
            }).datagrid('resize');
}



//报告在线编辑
var new_url= $("#reports_edit_1").prop("href");
function edit_online() {
    var read_doc_times = 0;
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        $.ajax({
            url: "LosslessReport_EditApply_Edit.aspx?&cmd=get_report_url",
            dataType: "text",
            type: 'POST',
            data: {
                report_id: selected_report.report_id,
            },
            success: function (data) {
                if (data == 'F') {
                    $.messager.alert('提示', '报告不存在，请先载入报告！');
                } else {
                  
                    $("#reports_edit_1").prop("href", new_url + "___hxw&url=" + data);
                    document.getElementById('reports_edit_1').click();
                }
            }
        });

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}

//预览报告
var new_url1 = $("#read_doc").prop("href");
function view_report() {
    var read_doc_times = 0;
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        $.ajax({
            url: "LosslessReport_EditApply_Edit.aspx?cmd=get_report_url",
            dataType: "text",
            type: 'POST',
            data: {
                report_id: selected_report.report_id,
            },
            success: function (data) {
                if (data == 'F') {
                    $.messager.alert('提示', '报告不存在，请退回报告编制！');
                } else {

                    $("#read_doc").prop("href", new_url1 + "___hxw&url=" + data);
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
    var selected_report = $("#report_edit_task").datagrid("getSelected");
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
//查看申请原因
function Back_report_dialog() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
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
              { 'value': '0', 'text': '待编制' },
              { 'value': '1', 'text': '历史编制' }
        ],
        onSelect: function () {

            //历史编制  1    未提交 0

            load_list();
        }
    });

}


//提交报告审核
function submit_report() {
    $('#group').combobox({
        url: "LosslessReport_Edit.ashx?&cmd=load_professional_department",
        valueField: 'id',
        textField: 'Department_name',
        onSelect: function () {
            var User_department = $('#group').combobox('getValue');
            $('#review_personnel').combobox({
                url: "LosslessReport_Edit.ashx?&cmd=load_responsible_people",
                valueField: 'User_count',
                textField: 'User_name',
                queryParams: {
                    "User_department": User_department
                }
            });
        },
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    var selectRow =    $("#report_edit_task").datagrid("getSelected");
    if (selectRow) {
        $('#importFileForm1').dialog({
            width: 500,
            height: 120,
            modal: true,
            title: '提交报告审核',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    save_();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importFileForm1').dialog('close');
                }
            }]
        });
      function save_() {
                  
            $('#importFileForm1').form('submit', {
                url: "LosslessReport_EditApply_Edit.aspx",
                onSubmit: function (param) {
                    param.cmd = 'submit_audit';
                    param.id = selectRow.id;
                    param.report_num = selectRow.report_num;
                 
                },
                success: function (data) {
                    if (data == 'T') {
                    
                        $('#importFileForm1').dialog('close');
                        $('#report_edit_task').datagrid('reload');
                        $.messager.alert('提示', '修改报告成功');
                       
                    }
                    else if (data == "F") {
                        $.messager.alert('提示', '修改报告失败！');
                    }
                    else {
                        $.messager.alert('提示', data);
                    }
                }
            });

        }
    } else {
        $.messager.alert('提示', "请选择你要操作的报告");
    }
}


