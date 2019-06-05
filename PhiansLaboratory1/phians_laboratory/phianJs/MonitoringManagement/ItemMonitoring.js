var line = 0;//初始化
$(function () {
    var _height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 125;
    //  alert(_height);
    // $("#cc").css("height", _height);
    $("#north").css("height", _height - 100);
    $("#south").css("height", "100px");
    ItemMonitoring_datagrid_init();//任务监控初始化
    search_combobox();//搜索下拉框信息初始化

});
//*************************************************************************任务监控初始化***********************************************************************
function ItemMonitoring_datagrid_init() {
    $('#ItemMonitoring_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        fit: true,
        rownumbers: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: '/MonitoringManagement/GetMTRRegisterList',
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
       {
           field: 'MTRState', title: 'MTRState', formatter: function (value, row, index) {//计划开始时间
               switch (value) {
                   case 1: value = "MTR Review"; break;
                   case 2: value = "Sample Recept"; break;
                   case 3: value = "Test Record"; break;
                   case 4: value = "Edit Report"; break;
                   case 5: value = "Review Report"; break;
                   case 6: value = "Review Report Return"; break;
                   case 7: value = "Issue Report"; break;
                   case 8: value = "Issue Report Return"; break;
                   case 9: value = "Report Finish"; break;
                   default: break;
               }
               return value;
           }
       },
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
           field: 'MachineTime', title: 'Machine Date', formatter: function (value, row, index) {//期望完成时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },//机器时间
       //{ field: 'Tester', title: 'Tester' },//测试人员
       { field: 'OrdersReceived', title: 'Orders Received' },//订单接收时间
       //{
       //    field: 'ReportModified', title: 'Report Modified(Y/N)', formatter: function (value, row, index) {//测试报告是否更改
       //        if (value == true) {
       //            return "Yes";
       //        } else if (value == false) {
       //            return "No";
       //        }
       //    }
       //},
       //{
       //    field: 'CNASLogo', title: 'CNAS Logo', formatter: function (value, row, index) {//
       //        if (value == true) {
       //            return "Yes";
       //        } else if (value == false) {
       //            return "No";
       //        }
       //    }
       //},
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
            $('#ItemMonitoring_datagrid').datagrid('selectRow', line);//默认选中第一行
        },
        onSelect: function () {
            $(".ystep4").html("");
            step();//进度条
        },
        toolbar: formlate_datagrid_toolbar//工具栏
    });
    //搜索
    $("#Monitoring_search").unbind("click").bind("click", function () {
        Monitoring_search();
    });
    //查看MTR流程记录
    $("#Item_process_record").unbind("click").bind("click", function () {
        MTR_Process_datagrid_init();
    });
    //查看主项目
    $("#ItemMonitoring_Item").unbind("click").bind("click", function () {
        Main_Item_datagrid_init();
    });
};
//****************************************获取mtrJ监控搜索下拉框的值
function search_combobox() {
    $('#search').combobox({
        panelHeight: 120,
        data: [
           { 'value': 'MTRNO', 'text': 'MTRNO' }
        ]
    });
};
//**********************************************mtrJ监控搜索
function Monitoring_search() {
    var search = $('#search').combobox('getValue');//获取下拉框的值
    var key = $('#key').textbox('getText');//获取文本框的值
    $('#ItemMonitoring_datagrid').datagrid(
        {
            type: 'POST',
            dataType: "json",
            url: '/MonitoringManagement/GetMTRRegisterList',
            queryParams: {
                search: search,//下拉框的值传给后台
                key: key,//文本框的值传给后台
            }
        }
   )
};
//******************************************************************************查看MTR流程记录********************************************************
function MTR_Process_datagrid_init() {
    //获取记录的显示title
    var selectRow = $("#ItemMonitoring_datagrid").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        $('#MTR_Process_dialog').dialog({
            width: 800,
            height: 400,
            fit: true,
            modal: true,
            title: 'Process Record',
            border: false,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#MTR_Process_dialog').dialog('close');
                }
            }]
        });
        $('#MTR_Process_datagrid').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            border: true,
            fitColumns: true,
            fit: true,
            pagination: true,
            ctrlSelect: true,
            pageSize: 15,
            pageList: [15, 30, 45, 60],
            pageNumber: 1,
            queryParams: {
                MTRNO: selectRow.MTRNO
            },
            url: "/MonitoringManagement/GetProcessRecordList",//接收一般处理程序返回来的json数据     
            columns: [[
                { field: 'MTRNo', title: 'MTRNO', width: 100 },
                { field: 'Operator_n', title: 'Operator', width: 100 },
                 {
                     field: 'OperateDate', title: 'OperateDate', width: 100
                 },

                { field: 'NodeResult', title: 'NodeResult', width: 100 },
                {
                    field: 'NodeId', title: 'NodeId', width: 100, formatter: function (value, row, index) {//计划开始时间
                        switch (value) {
                            case 1: value = "MTR Review"; break;
                            case 2: value = "Sample Recept"; break;
                            case 3: value = "Test Record"; break;
                            case 4: value = "Edit Report"; break;
                            case 5: value = "Review Report"; break;
                            case 6: value = "Review Report Return"; break;
                            case 7: value = "Issue Report"; break;
                            case 8: value = "Issue Report Return"; break;
                            case 9: value = "Report Finish"; break;
                            default: break;
                        }
                        return value;
                    }
                },

                {
                    field: 'Remark', title: 'Remark', width: 100, formatter: function (value, row, index) {//说明
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
                //{ field: 'TestResult', title: 'TestResult', width: 100 },
                // { field: 'activation', title: 'activation', width: 150 },
                //{ field: 'TaskRemarks', title: 'TaskRemarks', width: 100 }
                //{ field: 'SerialNum', title: 'SerialNum', width: 150 },
            ]],
            onLoadSuccess: function (data) {
                $('#MTR_Process_datagrid').datagrid('selectRow', 0);
            },
            toolbar: "#MTR_Process_toolbar"
        });
    }

    //搜索主项目下拉框初始化
    search_process_combobox();
    //主项目搜索
    $("#MTR_Process_search").unbind("click").bind("click", function () {
        MTR_Process_search();
    });
    //查看退回信息
    $("#MTR_Process_view").unbind("click").bind("click", function () {
        MTR_Process_view();
    });
}
//****************************************获取流程记录搜索下拉框的值
function search_process_combobox() {
    $('#search_MTR_Process').combobox({
        panelHeight: 120,
        data: [
           { 'value': 'Operator', 'text': 'Operator' }
        ]
    });
};
//*****************************************mtr流程记录搜索
function MTR_Process_search() {
    var selectRow = $("#ItemMonitoring_datagrid").datagrid("getSelected");//获取选中任务行
    var search = $('#search_MTR_Process').combobox('getValue');//获取下拉框的值
    var key = $('#key_MTR_Process').textbox('getText');//获取文本框的值
    $('#MTR_Process_datagrid').datagrid(
        {

            url: "/MonitoringManagement/GetProcessRecordList",//接收一般处理程序返回来的json数据  
            queryParams: {
                search: search,//下拉框的值传给后台
                key: key,//文本框的值传给后台
                MTRNO: selectRow.MTRNO
            }
        }
   )
};
//查看退回信息
function MTR_Process_view() {
    var selectRow = $("#MTR_Process_datagrid").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        var rowss = $('#MTR_Process_datagrid').datagrid('getSelections');
        $("#return_info_dialog").dialog({
            width: 450,
            height: 350,
            modal: true,
            title: 'View',
            border: false,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#return_info_dialog').dialog('close');
                }
            }]
        });
        $('#return_info_dialog').form('load', rowss[0]);
        var NodeId;
        switch (rowss[0].NodeId) {
            case 1: NodeId = "MTR Review"; break;
            case 2: NodeId = "Sample Recept"; break;
            case 3: NodeId = "Test Record"; break;
            case 4: NodeId = "Edit Report"; break;
            case 5: NodeId = "Review Report"; break;
            case 6: NodeId = "Review Report Return"; break;
            case 7: NodeId = "Issue Report"; break;
            case 8: NodeId = "Issue Report Return"; break;
            case 9: NodeId = "Report Finish"; break;
            default: break;

        }
        $("#NodeId").textbox("setText", NodeId);
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
//***************************************************************************** 查看主项目***************************************************
function Main_Item_datagrid_init() {
    //获取记录的显示title
    var selectRow = $("#ItemMonitoring_datagrid").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        $('#Item_Main_dialog').dialog({
            width: 800,
            height: 400,
            fit: true,
            modal: true,
            title: 'Main Item',
            border: false,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Item_Main_dialog').dialog('close');
                }
            }]
        });
        $('#Item_Main_datagrid').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            border: true,
            fitColumns: true,
            fit: true,
            pagination: true,
            ctrlSelect: true,
            pageSize: 15,
            pageList: [15, 30, 45, 60],
            pageNumber: 1,
            queryParams: {
                MTRNO: selectRow.MTRNO
            },
            url: "/MonitoringManagement/GetTestTaskList",//接收一般处理程序返回来的json数据     
            columns: [[
                { field: 'MTRNO', title: 'MTRNO', width: 100 },
                //{ field: 'ProjectId', title: 'ProjectId', width: 100 },
                { field: 'TestItem', title: 'TestItem', width: 150 },
                { field: 'MainNum', title: 'MainNum', width: 150 },
               // { field: 'Method', title: 'Method', width: 100 },
                { field: 'MotoNum', title: 'MotoNum', width: 100 },
                { field: 'ProjectName', title: 'ProjectName', width: 150 },
                { field: 'SampleQty', title: 'SampleQty', width: 150 },
                { field: 'SampleNo', title: 'SampleNo', width: 100 },
                 {
                     field: 'TestStartDate', title: 'TestStartDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                         if (value) {//格式化时间
                             if (value.length >= 10) {
                                 value = value.substr(0, 10)
                                 return value;
                             }
                         }
                     }
                 },
                {
                    field: 'TestEndDate', title: 'TestEndDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                {
                    field: 'PlanStartDate', title: 'PlanStartDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                {
                    field: 'PlanEndDate', title: 'PlanEndDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                //{ field: 'Temperature', title: 'Temperature', width: 100 },
                //{ field: 'Humidity', title: 'Humidity', width: 100 },
                // { field: 'ReportName', title: 'ReportName', width: 150 },
                //{ field: 'TestResource', title: 'TestResource', width: 150 },

                //{ field: 'MachineTime', title: 'MachineTime', width: 100 },
                //{ field: 'NonLife', title: 'NonLife', width: 100 },
                // { field: 'Total', title: 'Total', width: 150 },
                //{ field: 'Quotation', title: 'Quotation', width: 150 },
                { field: 'Isparent', title: 'Isparent', width: 100 },
                {
                    field: 'TestTaskState', title: 'TestTaskState', width: 100, formatter: function (value, row, index) {//计划开始时间
                        switch (value) {
                            case 0: value = "Test"; break;
                            case 2: value = "Tested"; break;
                            case 3: value = "Temporary Report"; break;
                            case 4: value = "Edited Report"; break;
                            default: break;
                        }
                        return value;
                    }
                },

                {
                    field: 'MainDescription', title: 'MainDescription', width: 100, formatter: function (value, row, index) {//说明
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
                //{ field: 'TestResult', title: 'TestResult', width: 100 },
                // { field: 'activation', title: 'activation', width: 150 },
                //{ field: 'TaskRemarks', title: 'TaskRemarks', width: 100 }
                //{ field: 'SerialNum', title: 'SerialNum', width: 150 },
            ]],
            onLoadSuccess: function (data) {
                $('#Item_Main_datagrid').datagrid('selectRow', 0);
            },
            toolbar: "#Item_Main_toolbar"
        });
    }
    //查看子项目
    $("#View_Chirdren_Item").unbind("click").bind("click", function () {
        Chirdren_Item_datagrid_init();
    });
    //搜索主项目下拉框初始化
    search_main_combobox();
    //主项目搜索
    $("#Item_Main_search").unbind("click").bind("click", function () {
        main_Item_search();
    });
}
//****************************************获取组项目搜索下拉框的值
function search_main_combobox() {
    $('#search_Main').combobox({
        panelHeight: 120,
        data: [
           { 'value': 'ProjectName', 'text': 'ProjectName' }
        ]
    });
};
//*****************************************主项目搜索
function main_Item_search() {
    var selectRow = $("#ItemMonitoring_datagrid").datagrid("getSelected");//获取选中任务行
    var search = $('#search_Main').combobox('getText');//获取下拉框的值
    var key = $('#key_Main').combobox('getText');//获取文本框的值
    $('#Item_Main_datagrid').datagrid(
        {
            type: 'POST',
            dataType: "json",
            url: "/MonitoringManagement/GetTestTaskList",//接收一般处理程序返回来的json数据   
            queryParams: {
                search: search,//下拉框的值传给后台
                key: key,//文本框的值传给后台
                MTRNO: selectRow.MTRNO
            }
        }
   )
};
//**************************************************************************************查看子项目项目*********************************************
function Chirdren_Item_datagrid_init() {
    //获取记录的显示title
    var selectRow = $("#Item_Main_datagrid").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        $('#Item_Chirdren_dialog').dialog({
            width: 800,
            height: 400,
            fit: true,
            modal: true,
            title: 'Main Item',
            border: false,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Item_Chirdren_dialog').dialog('close');
                }
            }]
        });
        $('#Item_Chirdren_datagrid').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            border: true,
            fitColumns: true,
            fit: true,
            pagination: true,
            ctrlSelect: true,
            pageSize: 15,
            pageList: [15, 30, 45, 60],
            pageNumber: 1,
            queryParams: {
                //    MTRNO: selectRow.MTRNO
            },
            //   url: "/MonitoringManagement/GetTestTaskList",//接收一般处理程序返回来的json数据     
            columns: [[
                { field: 'MTRNO', title: 'MTRNO', width: 100 },
                { field: 'ProjectId', title: 'ProjectId', width: 100 },
                { field: 'TestItem', title: 'TestItem', width: 150 },
                { field: 'SubNum', title: 'SubNum', width: 150 },
                { field: 'Method', title: 'Method', width: 100 },
                { field: 'MotoNum', title: 'MotoNum', width: 100 },
                { field: 'ProjectName', title: 'ProjectName', width: 150 },
                { field: 'SampleQty', title: 'SampleQty', width: 150 },
                { field: 'SampleNo', title: 'SampleNo', width: 100 },
                 {
                     field: 'TestStartDate', title: 'TestStartDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                         if (value) {//格式化时间
                             if (value.length >= 10) {
                                 value = value.substr(0, 10)
                                 return value;
                             }
                         }
                     }
                 },
                {
                    field: 'TestEndDate', title: 'TestEndDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                {
                    field: 'PlanStartDate', title: 'PlanStartDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                {
                    field: 'PlanEndDate', title: 'PlanEndDate', width: 100, formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                //{ field: 'Temperature', title: 'Temperature', width: 100 },
                //{ field: 'Humidity', title: 'Humidity', width: 100 },
                // { field: 'ReportName', title: 'ReportName', width: 150 },
                //{ field: 'TestResource', title: 'TestResource', width: 150 },

                //{ field: 'MachineTime', title: 'MachineTime', width: 100 },
                //{ field: 'NonLife', title: 'NonLife', width: 100 },
                // { field: 'Total', title: 'Total', width: 150 },
                //{ field: 'Quotation', title: 'Quotation', width: 150 },
                { field: 'Isparent', title: 'Isparent', width: 100 },
                {
                    field: 'TestTaskState', title: 'TestTaskState', width: 100, formatter: function (value, row, index) {//计划开始时间
                        switch (value) {
                            case 0: value = "To be tested"; break;
                            case 2: value = "Tested"; break;
                            case 3: value = "Temporary Report"; break;
                            case 4: value = "Edited Report"; break;
                            default: break;
                        }
                        return value;
                    }
                },

                {
                    field: 'SubtDescription', title: 'SubtDescription', width: 100, formatter: function (value, row, index) {//说明
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
                //{ field: 'TestResult', title: 'TestResult', width: 100 },
                // { field: 'activation', title: 'activation', width: 150 },
                //{ field: 'TaskRemarks', title: 'TaskRemarks', width: 100 }
                //{ field: 'SerialNum', title: 'SerialNum', width: 150 },
            ]],
            onLoadSuccess: function (data) {
                $('#Item_Chirdren_datagrid').datagrid('selectRow', 0);
            },
            toolbar: "#Item_Chirdren_toolbar"
        });
    }
    //搜索子项目下拉框初始化
    search_Chirdren_combobox();
    //搜索
    $("#Item_Chirdren_search").unbind("click").bind("click", function () {
        Item_Chirdren_search();
    });
}
//****************************************获取搜索子项目下拉框的值
function search_Chirdren_combobox() {
    $('#search_Chirdren').combobox({
        panelHeight: 120,
        data: [
           { 'value': 'ProjectName', 'text': 'ProjectName' }
        ]
    });
};
//****************************************子项目搜索
function Item_Chirdren_search() {
    var selectRow = $("#ItemMonitoring_datagrid").datagrid("getSelected");//获取选中任务行
    var search = $('#search_Main').combobox('getValue');//获取下拉框的值
    var key = $('#key_Main').combobox('getText');//获取文本框的值
    $('#Item_Main_datagrid').datagrid(
        {
            type: 'POST',
            dataType: "json",
            // url: "/MonitoringManagement/GetTestTaskList",//接收一般处理程序返回来的json数据   
            queryParams: {
                search: search,//下拉框的值传给后台
                key: key,//文本框的值传给后台
                MTRNO: selectRow.MTRNO
            }
        }
   )
};
/*
*functionName:
*function:进度条
*Param: 
*author:张慧敏
*date:2018-05-26
*/
function step() {
    var selectRow_step = 0;
    var selectRow = $('#ItemMonitoring_datagrid').datagrid("getSelected");
    switch (selectRow.MTRState) {
        case 6:
            $(".ystep4").loadStep({
                size: "large",
                color: "blue",
                steps: [{
                    title: "MTR评审",
                }, {
                    title: "样品接收",
                }, {
                    title: "测试记录",
                }, {
                    title: "报告审核退回",
                }, {
                    title: "报告审核",
                }, {
                    title: "报告签发",
                }, {
                    title: "报告完成",
                }
                ]
            }); break;
        case 8:
            $(".ystep4").loadStep({
                size: "large",
                color: "blue",
                steps: [{
                    title: "MTR评审",
                }, {
                    title: "样品接收",
                }, {
                    title: "测试记录",
                }, {
                    title: "报告签发退回",
                }, {
                    title: "报告审核",
                }, {
                    title: "报告签发",
                }, {
                    title: "报告完成",
                }
                ]
            }); break;
        default:
            $(".ystep4").loadStep({
                size: "large",
                color: "blue",
                steps: [{
                    title: "MTR评审",
                }, {
                    title: "样品接收",
                }, {
                    title: "测试记录",
                }, {
                    title: "报告编制",
                }, {
                    title: "报告审核",
                }, {
                    title: "报告签发",
                }, {
                    title: "报告完成",
                }
                ]
            }); break;
    }
    if (selectRow.MTRState == 6) {
        selectRow_step = 5;
    }else if (selectRow.MTRState == 7) {
        selectRow_step = 6;
    } else if (selectRow.MTRState == 8) {
        selectRow_step = 6;
    } else if (selectRow.MTRState == 9) {
        selectRow_step = 7;
    } else {
        selectRow_step = selectRow.MTRState;
    }
   // alert(selectRow_step);
    $(".ystep4").setStep(selectRow_step);
}