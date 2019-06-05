var line = 0;
$(function () {
    MTR_information_datagrid();//MTR信息列表加载
});

//*******************************************MTR信息列表***********************************************************************************************************************************************************************************************************
function MTR_information_datagrid() {
    //搜索信息的下拉框
    $('#key').combobox({
        panelHeight: 120,
        data: [
                { 'value': 'MTRNO', 'text': 'MTRNO' }
        ]
    });
    $('#MTR_information_datagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: true,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        url: "/ScheduleManagement/GetMTRRegisterList",
        columns: [[
       { field: 'MTRNO', title: 'MTR No.' },//MTR单号
       {
           field: 'SampleReceivedTiming', title: 'Sample Received Date', formatter: function (value, row, index) {//计划开始时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       { field: 'WeekNo', title: 'Week No.' },//第几周
       { field: 'ProjectNo', title: 'Project No.' },//测试项目编号
       { field: 'Application', title: 'Application' },//测试样品的类型
       { field: 'CostCenter', title: 'Cost Center' },//项目所属成本中心
       { field: 'ProjectEng', title: 'Project Eng.' },//项目所属工程师
       { field: 'Purpose', title: 'Purpose' },//测试目的
       { field: 'TestingGroup', title: 'Testing Group' },//执行测试组别
       { field: 'TestItem', title: 'Test Item' },//测试项目
       { field: 'StandardNonStandard', title: 'Standard/Non- Standard' },//标准/非标准
       { field: 'SampleQty', title: 'Sample Qty' },//测试样品数量
       {
           field: 'PlanStartTiming', title: 'Plan Start Date', formatter: function (value, row, index) {//计划开始时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'ActualStartTiming', title: 'Actual Start Date', formatter: function (value, row, index) {//实际开始时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'EstimatedCompletionTiming', title: 'Estimated Completion Date', formatter: function (value, row, index) {//期望完成时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'CompletedTiming', title: 'Completed Date', formatter: function (value, row, index) {//实际完成时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'Status', title: 'Status', formatter: function (value, row, index) {//测试状态,
               if (value == 0) {
                   return "在用";
               } else if (value == 1) {
                   return "停用";
               }
           }
       },
       {
           field: 'MachineTime', title: 'Machine Date', width: 100, formatter: function (value, row, index) {//机器时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       { field: 'Tester', title: 'Tester' },//测试人员
       { field: 'OrdersReceived', title: 'Orders Received' },//订单接收时间
       {
           field: 'ReportModified', title: 'Report Modified(Y/N)', formatter: function (value, row, index) {//测试报告是否更改
               if (value == true) {
                   return "Yes";
               } else if (value == false) {
                   return "No";
               }
           }
       },
       {
           field: 'CNASLogo', title: 'CNAS Logo', formatter: function (value, row, index) {//
               if (value == true) {
                   return "Yes";
               } else if (value == false) {
                   return "No";
               }
           }
       },
       {
           field: 'remark', title: 'Remark', width: 100, formatter: function (value, row, index) {//说明
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
            $('#MTR_information_datagrid').datagrid('selectRow', line);

        },
        toolbar: "#MTR_information_toolbar"
    });
    //查看DVP信息
    $("#View_Schedule").unbind("click").bind("click", function () {
        ViewSchedule();
    });
    //搜索mtr信息
    $("#search").unbind("click").bind("click", function () {
        SearchMTR();
    });
  
};

//*********************s搜索mtr信息
function SearchMTR() {
    $('#MTR_information_datagrid').datagrid({
        queryParams: {
            search: $("#key").combobox("getValue"),
            key: $("#key1").textbox("getText")
        },
        url: "/ScheduleManagement/GetMTRRegisterList",
    })
};
//*****************查看DVP信息
function ViewSchedule() {

    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        $('#Schedule_dialog').dialog({
            title: 'View Schedule',
            width: 750,
            height: 500,
            fit:true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Schedule_dialog').dialog('close');//关闭弹窗
                }
            }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row this operated！');
    }
};


