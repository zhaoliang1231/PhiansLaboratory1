var line = 0;//编辑操作选中行
var WordUrlSpit = new Array();
var WordUrlSpit1 = new Array();
var WordUrlSpit2 = new Array();
var ShowOffice2Spit = new Array();
var WordUrlSpit3 = new Array();
var WordUrlSpit4 = new Array();
$(function () {
    word_link = $("#ShowOffice_").attr("href");//获取a链接 URL
    word_link1 = $("#ShowOffice").attr("href");//获取a链接 URL
    word_link2 = $("#ShowOffice2").attr("href");//获取a链接 URL
    word_link3 = $("#ShowOffice1").attr("href");//获取a链接 URL
    word_link3 = $("#ShowOffice3").attr("href");//获取a链接 URL
    word_link4 = $("#ShowOffice4").attr("href");//获取a链接 URL

    WordUrlSpit = word_link.split("?");
    WordUrlSpit1 = word_link1.split("?");
    WordUrlSpit2 = word_link2.split("?");
    ShowOffice2Spit = word_link3.split("?");
    WordUrlSpit3 = word_link3.split("?");
    WordUrlSpit4 = word_link4.split("?");
    task_record_init();//任务列表
    $("#records_key").combobox({
        data: [
           { 'value': 'TaskId', 'text': 'Task Id' },
           { 'value': 'MotoNum', 'text': 'Moto Num' }
        ]
    });

   
});
//***********************************************************************测试记录列表初始化*******************************************
//**********************测试记录列表
function task_record_init() {
    // search_combobox();//搜索下拉框内容
    $('#task_record').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: false,
        //   fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        url: "",//接收一般处理程序返回来的json数据  
        columns: [[
       { field: 'TaskId', title: 'Task Id'},
       { field: 'ParentID', title: 'Parent ID', hidden: true },
       { field: 'MTRNO', title: 'MTRNO' },
          { field: 'ProjectName', title: 'ProjectName' },
        { field: 'ProjectId', title: 'ProjectId', hidden: true },
      // { field: 'MainNum', title: 'MainNum' },
       {
           field: 'MainDescription', title: 'MainDescription', width: 300, hidden: true, formatter: function (value, row, index) {
               return '<span  title=' + value + '>' + value + '</span>';
           }
       },
       {
           field: 'SubtDescription', title: 'SubtDescription', width: 300, hidden: true, formatter: function (value, row, index) {
               return '<span  title=' + value + '>' + value + '</span>';
           }
       },
     //  { field: 'SubNum', title: 'SubNum' },
      // { field: 'MotoNum', title: 'MotoNum' },

       { field: 'SampleQty', title: 'SampleQty' },
       { field: 'SampleNo', title: 'SampleNo' },
       { field: 'Operator', title: 'Operator' },
       {
           field: 'TestStartDate', title: 'TestStartDate', formatter: function (value, row, index) {//计划开始时间
               if (value) {//格式化时间
                   if (value == "1900-01-01 0:00:00") {
                       return "";
                   } else if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'TestEndDate', title: 'TestEndDate', formatter: function (value, row, index) {//计划开始时间
               if (value) {//格式化时间
                   if (value == "1900-01-01 0:00:00") {
                       return "";
                   } else if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'PlanStartDate', title: 'PlanStartDate', formatter: function (value, row, index) {//计划开始时间
               if (value) {//格式化时间
                   if (value == "1900-01-01 0:00:00") {
                       return "";
                   } else if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'PlanEndDate', title: 'PlanEndDate', formatter: function (value, row, index) {//计划开始时间
               if (value) {//格式化时间
                   if (value == "1900-01-01 0:00:00") {
                       return "";
                   } else if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       { field: 'Temperature', title: 'Temperature' },
       { field: 'Humidity', title: 'Humidity' },
       { field: 'ReportName', title: 'ReportName' },
       { field: 'TestResource', title: 'TestResource' },
       { field: 'MachineTime', title: 'MachineTime(h)' },
       { field: 'TestItem', title: 'TestItem' },
       { field: 'NonLife', title: 'NonLife' },
       { field: 'Total', title: 'Total(h)' },
       { field: 'Isparent', title: 'Isparent' },
       { field: 'TestResult', title: 'TestResult' },
       { field: 'activation', title: 'activation' },

       { field: 'SerialNum', title: 'SerialNum' },
       { field: 'Conculsion', title: 'Conculsion' },
        {
            field: 'TaskRemarks', title: 'TaskRemarks', width: 150, formatter: function (value, row, index) {
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
        },
        ]],
        onLoadSuccess: function (data) {
            $('#task_record').datagrid('selectRow', line);
        },
        sortOrder: 'asc',
        toolbar: "#task_record_toolbar"
    });

    $('#task_record').datagrid({ rowStyler: function (index, row) { if (row.TestTaskState == 2) { return 'color:red;'; } } });
    //搜索任务
    $('#search').unbind('click').bind('click', function () {
        search_task();
    });
    //开始测试
    $('#StartTesting').unbind('click').bind('click', function () {
        StartTesting();
    });
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#task_record').datagrid("loadData", json);
};

//*********************************************开始测试
function StartTesting() {
    //获取记录的显示title
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        //if (selectRow.TestTaskState == "0") {
        $('#View_TestTask_dialog').dialog({

            fit: true,
            modal: true,
            title: 'Start Testing',
            border: false,
            buttons: [
                {
                    text: 'View Upload Information',
                    iconCls: 'icon-Document',
                    id: "TestUploadInformation",
                    handler: function () {
                        // $('#View_TestTask_dialog').dialog('close');
                    }
                },
                {
                    text: 'Confirm Completion',
                    iconCls: 'icon-ok',
                    id: "ConfirmComplete",
                    handler: function () {
                        // $('#View_TestTask_dialog').dialog('close');
                    }
                },
                {
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#View_TestTask_dialog').dialog('close');
                    }
                }]
        });
        //***************************写入测试开始时间
        $.ajax({
            url: "/ScheduleManagement/WriteTestStartDate",//接收一般处理程序返回来的json数据     
            data: { TaskId: selectRow.TaskId, ParentID: selectRow.ParentID },
            type: "POST",
            async: false
        });


        $('#View_TestTask_datagrid').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            border: false,
            fitColumns: true,
            pagination: true,
            ctrlSelect: true,
            //  fit:true,
            height: 680,
            pageSize: 15,
            pageList: [15, 30, 45, 60],
            pageNumber: 1,
            type: 'POST',
            dataType: 'json',
            url: "/ScheduleManagement/GetTestTaskList",//接收一般处理程序返回来的json数据                
            queryParams: {
                search: 'TaskId',
                key: selectRow.TaskId
            },
            columns: [[
           { field: 'TaskId', title: 'Task Id', hidden: true },
           { field: 'ParentID', title: 'Parent ID', hidden: true },
           { field: 'MTRNO', title: 'MTRNO' },
              { field: 'ProjectName', title: 'ProjectName' },
            { field: 'ProjectId', title: 'ProjectId', hidden: true },
           {
               field: 'MainDescription', title: 'MainDescription', width: 300, hidden: true, formatter: function (value, row, index) {
                   return '<span  title=' + value + '>' + value + '</span>';
               }
           },
           {
               field: 'SubtDescription', title: 'SubtDescription', width: 300, hidden: true, formatter: function (value, row, index) {
                   return '<span  title=' + value + '>' + value + '</span>';
               }
           },
           { field: 'SampleQty', title: 'SampleQty' },
           { field: 'SampleNo', title: 'SampleNo' },
           { field: 'Operator', title: 'Operator' },
           {
               field: 'TestStartDate', title: 'TestStartDate', formatter: function (value, row, index) {//计划开始时间
                   if (value) {//格式化时间
                       if (value == "1900-01-01 0:00:00") {
                           return "";
                       } else if (value.length >= 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           {
               field: 'TestEndDate', title: 'TestEndDate', formatter: function (value, row, index) {//计划开始时间
                   if (value) {//格式化时间
                       if (value == "1900-01-01 0:00:00") {
                           return "";
                       } else if (value.length >= 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           {
               field: 'PlanStartDate', title: 'PlanStartDate', formatter: function (value, row, index) {//计划开始时间
                   if (value) {//格式化时间
                       if (value == "1900-01-01 0:00:00") {
                           return "";
                       } else if (value.length >= 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           {
               field: 'PlanEndDate', title: 'PlanEndDate', formatter: function (value, row, index) {//计划开始时间
                   if (value) {//格式化时间
                       if (value == "1900-01-01 0:00:00") {
                           return "";
                       } else if (value.length >= 10) {
                           value = value.substr(0, 10)
                           return value;
                       }
                   }
               }
           },
           { field: 'Temperature', title: 'Temperature' },
           { field: 'Humidity', title: 'Humidity' },
           { field: 'ReportName', title: 'ReportName' },
           { field: 'TestResource', title: 'TestResource' },
           { field: 'MachineTime', title: 'MachineTime(h)' },
           { field: 'TestItem', title: 'TestItem' },
           { field: 'NonLife', title: 'NonLife' },
           { field: 'Total', title: 'Total(h)' },
           { field: 'Isparent', title: 'Isparent' },
           { field: 'TestResult', title: 'TestResult' },
           { field: 'activation', title: 'activation' },
           { field: 'SerialNum', title: 'SerialNum' },
           { field: 'Conculsion', title: 'Conculsion' },
            {
                field: 'TaskRemarks', title: 'TaskRemarks', width: 150, formatter: function (value, row, index) {
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
            },
            ]],
            onLoadSuccess: function (data) {
                $('#View_TestTask_datagrid').datagrid('selectRow', line);
            },
            sortOrder: 'asc',
            toolbar: "#View_TestTask_toolbar"
        });
    } else {
        $.messager.alert("Tips", 'Please select the line to operate！')
    }
    //房间号选中
    $('#RoomNum').combobox({
        url: "/MonitoringManagement/GetRoomNum",
        valueField: 'Value',
        textField: 'Text',
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onSelect: function () {
            var RoomNum = $("#RoomNum").combobox("getValue");
            var TestStartDate = $("#TestStartDate").datetimebox("getValue");
            if (TestStartDate) {
                $.ajax({
                    url: "/MonitoringManagement/GetTemperatureFirstRecord",
                    type: 'POST',
                    data: {
                        RoomNum: RoomNum,//获取选中行的RoomNum传给后台
                        StartDatetime: TestStartDate
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            if (result.Data) {
                                $("#Temperature").textbox("setText", result.Data.Temperature);
                                $("#Temperature").textbox().focus();
                                $("#Humidity").textbox("setText", result.Data.humidity);
                                $("#Humidity").textbox().focus();
                            }
                        } else {
                            $.messager.alert('Tips', result.Message);

                        }
                    }
                });
            }

        }
    });
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    $('#info_Add').form("load", selectRow);
    $('#RoomNum').combobox("setValue", selectRow.RoomID);

    $('#Submit_Remark_dialog').form("load", selectRow);
    
    //编辑基本信息
    $("#information_save").unbind("click").bind("click", function () {
        information_save();
    })
    //点击查看测试记录
    test_record_look();
    //$("#test_record_look").unbind("click").bind("click", function () {
    //    test_record_look();
    //});
    ////查看上传图片
    //$("#View_Take_photo").unbind("click").bind("click", function () {
    //    View_Take_photo();
    //});
    ////查看上传图片
    View_Take_photo1();
    Monitor_List_Load();
    Test_List_Load();
    //$("#View_Take_photo1").unbind("click").bind("click", function () {

    //});
    //导入测试数据信息
    ViewTestDataFile_datagrid_init();
    //$("#ViewTestDataFile").unbind("click").bind("click", function () {
    //    ViewTestDataFile_datagrid_init();
    //});
    //完成子项目
    $("#ConfirmComplete").unbind("click").bind("click", function () {
        ConfirmComplete();
    });
    //查看测试设备
    TestEquipment_init();
    //$("#TestEquipment").unbind("click").bind("click", function () {
    //    TestEquipment_init();
    //});
    //上传记录模板文件
    $("#Upload_Record_Template").unbind("click").bind("click", function () {
        initrecord_template();

    });

    $("#TestUploadInformation").unbind("click").bind("click", function () {
        //上传记录信息查看
        View_TestInformation();

    });
    //////保存检验条件回显
    save_contation_View();
    //结论与备注保存
    $("#Submit_Remark").unbind("click").bind("click", function () {
        //上传记录信息查看
        Save_remark();

    });
    //查看基本信息
    $("#View_Test_information").unbind("click").bind("click", function () {
        //上传记录信息查看
        MTR_edit();

    });
};
/*
*functionName:MTR_edit
*function:查看基本信息
*Param search，key
*author:张慧敏
*date:2018-05-15
*/

function information_save() {
    //e.preventDefult();
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中行
    var roomID = $("#RoomNum").combobox("getValue");//获取选中行
    if (selectRow) {
        line = $('#task_record').datagrid("getRowIndex", selectRow);
        //  $('#info_Add').form('load', selectRow);//数据回显
        $.ajax({
            url: "/ScheduleManagement/WriteBaseInfo",//接收一般处理程序返回来的json数据     
            type: 'POST',
            data: {
                RoomID: roomID,
                TaskId: selectRow.TaskId,
                SampleQty: selectRow.SampleQty,
                SampleNo: selectRow.SampleNo,
                Operator: $("#Operator").textbox("getValue"),
                TestStartDate: $("#TestStartDate").datetimebox("getValue"),
                TestEndDate: $("#TestEndDate").datetimebox("getValue"),
                Temperature: $("#Temperature").textbox("getValue"),
                Humidity: $("#Humidity").textbox("getValue"),
                SetupRemark: $("#SetupRemark").textbox("getValue"),
                Conculsion: $("#Conculsion").textbox("getValue")
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('Tips', result.Message);
                    $('#task_record').datagrid("reload");
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
        //$('#info_Add').form('submit', {
        //    url: "/ScheduleManagement/WriteBaseInfo",//接收一般处理程序返回来的json数据     
        //    onSubmit: function (param) {
        //        param.TaskId = selectRow.TaskId;
        //        return $(this).form('enableValidation').form('validate');
        //    },
        //    success: function (data) {
        //        if (data) {
        //            var result = $.parseJSON(data);
        //            if (result.Success == true) {
        //                $.messager.alert('Tips', result.Message);
        //                $('#task_record').datagrid("reload");

        //                // $('#Submit_Remark_dialog').dialog('close');
        //            }
        //            else {
        //                $.messager.alert('Tips', result.Message);
        //            }
        //        }
        //    }
        //});
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };

};

/*
*functionName:search_records
*function:结论与备注保存
*Param search，key
*author:张慧敏
*date:2018-05-15
*/
function Save_remark() {
    //获取记录的显示title
    var selectedRow = $("#task_record").datagrid("getSelected");
    $('#Submit_Remark_dialog').form('submit', {
        url: "/ScheduleManagement/ConculsionTaskRemarks",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            param.TaskId = selectedRow.TaskId;
            return $(this).form('enableValidation').form('validate');
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('Tips', result.Message);
                    $('#task_record').datagrid("reload");
                    // $('#Submit_Remark_dialog').dialog('close');
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        }
    });
}
/*
 * 
 *functionName:save_contation_View()
 *function://////保存检验条件回显
 *Param 参数
 *author:张慧敏
 *date:2018/5/7
*/
function save_contation_View() {

    var selectedRow = $("#task_record").datagrid("getSelected")
    //获取文本值
    // var Method = document.getElementById("editor1").value;
    if (selectedRow) {
        getHtml(selectedRow.ParentID);

    }
    //传父id还是子id
    function getHtml(id) {
        $.ajax({
            url: "/ScheduleManagement/GetHtmlInfo",
            type: 'POST',
            // dataType: 'html',
            data: {
                TaskId: id,
                //   ProjectName: node.text,
                flag: "0",
                //   Method: Method

            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    $('#editor').val(result.MainDescription);
                    $('#editor1').val(result.Method);
                    //KindEditor.html('#editor', result.MainDescription);
                    //KindEditor.html('#editor1', result.Method);
                }

            }
        });
    }
}

//***************************************************************************************查看子项目测试记录
//*********************************************查看子项目测试记录
function test_record_look() {
    var selectedRow = $("#task_record").datagrid("getSelected");
    //获取记录的显示title
    var RecordRemark;
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中任务行
    var TaskId = selectRow.TaskId;
    $.ajax({
        url: "/ScheduleManagement/GetTestRecordRemarkList",//接收一般处理程序返回来的json数据     
        data: { TaskId: selectRow.TaskId },
        type: "POST",
        async: false,
        success: function (data) {
            RecordRemark = JSON.parse(data);
            if (data != 'null') {//判断是否有测试记录数据
                //****************************************加载测试记录列表
                test_data(TaskId, RecordRemark);
            }
            else {//返回没有数据的时候只显示工具栏和分页

                $('#test_records_datagrid').datagrid({
                    nowrap: false,
                    striped: true,
                    rownumbers: true,
                    url: "",//接收一般处理程序返回来的json数据  
                    border: true,
                    fitColumns: true,
                    height: 730,
                    pagination: true,
                    ctrlSelect: true,
                    pageSize: 15,
                    pageList: [15, 30, 45, 60],
                    pageNumber: 1,
                    toolbar: "#test_records_toolbar"
                });
                var json = {
                    "rows": [],
                    "total": 0,
                    "success": true
                };
                $('#test_records_datagrid').datagrid("loadData", json);
            }
        }
    });
    //修改测试记录
    $('#edit').unbind('click').bind('click', function () {
        edit_test_records();
    });
    //搜索测试记录
    $("#key").unbind("click").bind("click", function () {
        search_records();
    });
    //导入测试记录
    $("#import_records").unbind("click").bind("click", function () {
        import_records();
    });
};
/*
*functionName:search_records
*function:搜索测试记录
*Param search，key
*author:程媛
*date:2018-04-18
*/
function search_records() {
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中任务行
    var TaskId = selectRow.TaskId;
    var search = $('#records_key').combobox('getValue');
    var key = $('#records_key1').textbox('getText');
    $('#test_records_datagrid').datagrid(
        {
            type: 'POST',
            dataType: "json",
            url: "/ScheduleManagement/GetTestRecordList",//接收一般处理程序返回来的json数据                
            queryParams: {
                search: search,
                key: key,
                TaskId: selectRow.TaskId
            }
        }
   )
};
//获取父页面的文本框的值
function Value() {
   // var ContentType = $("#ContentType").combobox("getValue");
    var SortNum = $("#SortNum").textbox("getText");
    var Content = $("#Content").textbox("getText");
    var ValueJson = {};
  //  ValueJson.ContentType = ContentType;
    ValueJson.SortNum = SortNum;
    ValueJson.Content = Content;
    return ValueJson;
}
//********************************************3.2查看上传图片列表
function View_Take_photo1() {
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    $('#View_photo_records_datagrid1').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: false,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
      
        queryParams: {
            TaskId: selectRow.TaskId,
            ContentType: "1"
        },
        url: "/ScheduleManagement/ShowUploadFileList",//接收一般处理程序返回来的json数据  
        columns: [[
       
       {
           field: 'ContentType', title: 'ContentType', width: 100, formatter: function (value, row, index) {//标识
               if (value == 1) {
                   return "Set-up Data";
               } else if (value == 2) {
                   return "Monitor Data";
               } else if (value == 3) {
                   return "Test Photos";
               }
           }
       },
       { field: 'PicName', title: 'PicName', width: 100 },
      { field: 'DataFormat', title: 'DataFormat', width: 100 },
       { field: 'SortNum', title: 'SortNum', width: 100 },
        { field: 'Content', title: 'Content', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#View_photo_records_datagrid1').datagrid('selectRow', line);
        },
        sortOrder: 'asc',
        toolbar: "#View_photo_record_toolbar1"
    });
    //查看图片
    $("#View_photo_list1").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_datagrid1').datagrid('getSelected');//获取选中行
        if (selectRow_photo) {
            $('#look_photo_detail').dialog({
                width: 700,
                height: 500,
                modal: true,
                title: 'View Photo',
                draggable: true,
                buttons: [{
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#look_photo_detail').dialog('close');
                    }
                }]
            });
            // console.log(selectRow_photo.PicUr);
            $("#detail_img").prop("src", selectRow_photo.PicUrl);
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
    //上传文件/拍照
    $("#Take_photo").unbind("click").bind("click", function () {
        Take_photo("1");
    });
    //删除图片列表
    $("#View_photo_delete1").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_datagrid1').datagrid('getSelected');//获取选中行
        var selectRow_list = $('#task_record').datagrid('getSelected');//获取任务列表
        if (selectRow_photo) {
            $.messager.confirm('confirm', 'confirm delete？', function (r) {
                if (!r) {
                    return false;
                } else {
                    $.ajax({
                        url: '/ScheduleManagement/DelUploadFile',
                        type: 'POST',
                        data: {
                            id: selectRow_photo.id,
                            ContentType: selectRow_photo.ContentType,
                            ProjectName: selectRow_list.ProjectName
                        },
                        success: function (data) {
                            if (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //选择当前添加节点
                                    $.messager.alert('tips', result.Message);
                                    $('#View_photo_records_datagrid1').datagrid('reload');
                                    $('#View_photo_records_datagrid').datagrid('reload');

                                } else {
                                    $.messager.alert('tips', result.Message);
                                }
                            }

                        }
                    })
                }
            });
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
}

/*
*functionName:Monitor_List_Load
*function:3.3
*Param: MTRNO
*author:张慧敏
*date:2018-05-17
*/
function Monitor_List_Load() {
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    $('#View_photo_records_Monitor_datagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: false,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        //  height: 730,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        queryParams: {
            TaskId: selectRow.TaskId,
            ContentType: "2"
        },
        url: "/ScheduleManagement/ShowUploadFileList",//接收一般处理程序返回来的json数据  
        columns: [[
     //  { field: 'TaskId', title: 'Task Id',width:100 },
     { field: 'Content', title: 'Content', width: 150 },
     {
         field: 'ContentType', title: 'ContentType', width: 100, formatter: function (value, row, index) {//标识
             if (value == 1) {
                 return "Set-up Data";
             } else if (value == 2) {
                 return "Monitor Data";
             } else if (value == 3) {
                 return "Test Photos";
             } else if (value == 4) {
                 return "File";
             }
         }
     },
      { field: 'DataFormat', title: 'DataFormat', width: 100 },
       { field: 'PicName', title: 'PicName', width: 100 },
     //  { field: 'PicUrl', title: 'PicUrl', width: 100 },
       { field: 'SortNum', title: 'SortNum', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#View_photo_records_Monitor_datagrid').datagrid('selectRow', line);
        },
        sortOrder: 'asc',
        toolbar: "#View_photo_record_Monitor_toolbar"
    });
    //在线编辑报告数据
    $("#EditTestDataFile_").unbind("click").bind("click", function () {
        EditTestDataFile_();
    });
    //查看图片
    $("#View_photo_list_Monitor").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_Monitor_datagrid').datagrid('getSelected');//获取选中行
        if (selectRow_photo) {
            if (selectRow_photo.DataFormat == ".jpg" || selectRow_photo.DataFormat == ".png") {
                $('#look_photo_detail').dialog({
                    width: 700,
                    height: 500,
                    modal: true,
                    title: 'View Photo',
                    draggable: true,
                    buttons: [{
                        text: 'Close',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $('#look_photo_detail').dialog('close');
                        }
                    }]
                });
                // console.log(selectRow_photo.PicUr);
                $("#detail_img").prop("src", selectRow_photo.PicUrl);
            } else {
                $.messager.alert('Tips', 'This is a file and cannot view photos！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
    //上传文件/拍照
    $("#Take_photo_Monitor").unbind("click").bind("click", function () {
        Take_photo("2");
    });
    //删除图片列表
    $("#View_photo_delete_Monitor").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_Monitor_datagrid').datagrid('getSelected');//获取选中行
        var selectRow_list = $('#task_record').datagrid('getSelected');//获取任务列表
        if (selectRow_photo) {
            $.messager.confirm('confirm', 'confirm delete？', function (r) {
                if (!r) {
                    return false;
                } else {
                    $.ajax({
                        url: '/ScheduleManagement/DelUploadFile',
                        type: 'POST',
                        data: {
                            id: selectRow_photo.id,
                            ContentType: selectRow_photo.ContentType,
                            ProjectName: selectRow_list.ProjectName
                        },
                        success: function (data) {
                            if (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //选择当前添加节点
                                    $.messager.alert('tips', result.Message);
                                    $('#View_photo_records_Monitor_datagrid').datagrid('reload');
                                    $('#View_photo_records_datagrid').datagrid('reload');

                                } else {
                                    $.messager.alert('tips', result.Message);
                                }
                            }

                        }
                    })
                }
            });
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
}
/*
 * 
 *functionName:EditTestDataFile()
 *function:在线编辑报告数据
 *Param id，Operation_Type
 *author:程媛
 *date:2018/5/18
*/
function EditTestDataFile_() {
    var selected_Item = $("#View_photo_records_Monitor_datagrid").datagrid("getSelected");
    if (selected_Item) {
        if (selected_Item.DataFormat == ".xls"||selected_Item.DataFormat == ".csv") {
            $("#ShowOffice4").prop("href", WordUrlSpit4[0] + '?id=' + selected_Item.id + '&Operation_Type=16&pageName=FileManagement' + WordUrlSpit4[1]);
            document.getElementById('ShowOffice4').click();
        } else {
            $.messager.alert('Tips', 'This data cannot be edited online！');
        }
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
/*
*functionName:Test_List_Load
*function:3.4
*Param: MTRNO
*author:张慧敏
*date:2018-05-17
*/
function Test_List_Load() {
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    $('#View_photo_records_Test_datagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: false,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        //  height: 730,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        queryParams: {
            TaskId: selectRow.TaskId,
            ContentType: "3"
        },
        url: "/ScheduleManagement/ShowUploadFileList",//接收一般处理程序返回来的json数据  
        columns: [[
     //  { field: 'TaskId', title: 'Task Id',width:100 },
         { field: 'Content', title: 'Content', width: 150 },
         {
         field: 'ContentType', title: 'ContentType', width: 100, formatter: function (value, row, index) {//标识
             if (value == 1) {
                 return "Set-up Data";
             } else if (value == 2) {
                 return "Monitor Data";
             } else if (value == 3) {
                 return "Test Photos";
             } else if (value == 4) {
                 return "File";
             }
         }
     },
       { field: 'PicName', title: 'PicName', width: 100 },
       { field: 'DataFormat', title: 'DataFormat', width: 100 },
       { field: 'SortNum', title: 'SortNum', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#View_photo_records_Test_datagrid').datagrid('selectRow', line);
        },
        sortOrder: 'asc',
        toolbar: "#View_photo_record_Test_toolbar"
    });
    //查看图片
    $("#View_photo_list_Test").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_Test_datagrid').datagrid('getSelected');//获取选中行
        if (selectRow_photo) {
            $('#look_photo_detail').dialog({
                width: 700,
                height: 500,
                modal: true,
                title: 'View Photo',
                draggable: true,
                buttons: [{
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#look_photo_detail').dialog('close');
                    }
                }]
            });
            // console.log(selectRow_photo.PicUr);
            $("#detail_img").prop("src", selectRow_photo.PicUrl);
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
    //上传文件/拍照
    $("#Take_photo_Test").unbind("click").bind("click", function () {
        Take_photo("3");
    });
    //删除图片列表
    $("#View_photo_delete_Test").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_Test_datagrid').datagrid('getSelected');//获取选中行
        var selectRow_list = $('#task_record').datagrid('getSelected');//获取任务列表
        if (selectRow_photo) {
            $.messager.confirm('confirm', 'confirm delete？', function (r) {
                if (!r) {
                    return false;
                } else {
                    $.ajax({
                        url: '/ScheduleManagement/DelUploadFile',
                        type: 'POST',
                        data: {
                            id: selectRow_photo.id,
                            ContentType: selectRow_photo.ContentType,
                            ProjectName: selectRow_list.ProjectName
                        },
                        success: function (data) {
                            if (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //选择当前添加节点
                                    $.messager.alert('tips', result.Message);
                                    $('#View_photo_records_Test_datagrid').datagrid('reload');
                                    $('#View_photo_records_datagrid').datagrid('reload');

                                } else {
                                    $.messager.alert('tips', result.Message);
                                }
                            }

                        }
                    })
                }
            });
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
}
/*
*functionName:Take_photo
*function:上传文件dialog
*Param: flag 3.2,3.3,3.4
*author:张慧敏
*date:2018-05-17
*/
//*****************************************
function Take_photo(flag) {
    //重新打开时选中上传文件
    $("#u_file").prop("checked", "checked");
    $("#u_photo").prop("checked", false);
    $("#upload_file").css("display", "block");
    $("#radio_choose").css("display", "block");
    $("#upload_piture").css("display", "none");
    ////上传文件
    //等于“2”的时候就显示上传文件的内容，不等于“2”的时候就隐藏上传文件的内容
    if (flag != "2") {
        $('#u_file').click(function () {
            $("#u_file").prop("checked", "checked");
            $("#u_photo").prop("checked", false);
            $("#upload_file").css("display", "block");
            $("#radio_choose").css("display", "none");
            $("#upload_piture").css("display", "none");
        });
        //拍照
        $('#u_photo').click(function () {
            $("#radio_choose").css("display", "none");
            $("#u_file").prop("checked", false);
            $("#u_photo").prop("checked", "checked");
            $("#upload_file").css("display", "none");
            $("#upload_piture").css("display", "block");
            $("#WebCam_photo").prop("src", "/WebCam/TestRecord?TaskId=" + escape(selectRow.TaskId) + "&flag=" + flag + "");
        });
       
    } else {
        $('#u_file').click(function () {
            $("#u_file").prop("checked", "checked");
            $("#u_photo").prop("checked", false);
            $("#upload_file").css("display", "block");
            $("#radio_choose").css("display", "block");
            $("#upload_piture").css("display", "none");
        });
        //拍照
        $('#u_photo').click(function () {
            $("#radio_choose").css("display", "none");
            $("#u_file").prop("checked", false);
            $("#u_photo").prop("checked", "checked");
            $("#upload_file").css("display", "none");
            $("#upload_piture").css("display", "block");
            $("#WebCam_photo").prop("src", "/WebCam/TestRecord?TaskId=" + escape(selectRow.TaskId) + "&flag=" + flag + "");
        });
    };
    //切换名字内容
    $('#u_file1').click(function () {
        if ($("#u_file1").is(":checked")) {
            $("#name").html("File Name：");
            $("#Photo").html("File URL：");
        } else {
            $("#name").html("Photo Name：");
            $("#Photo").html("Photo：");
        }
    })
    if (flag != "2") {
        $("#name").html("Photo Name：");
        $("#Photo").html("Photo：");
        $("#radio_choose").css("display", "none");
    } else {
        $("#radio_choose").css("display", "block");
    }
 
    $("#SortNum").numberbox("setValue", "");
    $("#Content").textbox("setText", "");
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    if (selectRow) {
        if (selectRow.TestTaskState == 0) {
            $('#take_photo_dialog').dialog({
                width: 700,
                height: 500,
                modal: true,
                fit: true,
                title: 'Upload File',
                draggable: true,
                buttons: [{
                    text: 'close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#take_photo_dialog').dialog('close');
                    }
                }]
            });
            //View_Take_photo 显示查看列表
            View_Take_photo(flag);
        } else {
            $.messager.alert("Tips", "Test task has been completed and cannot be operated！")
        }
    } else {
        $.messager.alert('Tips', 'Please select the line to operate！');
    }

    //判断选中的是上传文件还是拍照
    var checVal = $("#radioSpan").find('input[type=checkbox]:checked').val();
    //判断选中的是上传文件
    if (checVal == "0") {
        //如果是上传文件0
        $("#save_image").unbind("click").bind("click", function () {
            save_image_photo(flag);
        });
    }

};
//**************************************保存图片文件
function save_image_photo(ContentType) {
    //if ($("#u_file1").is(":checked") && ContentType=="2") {
    //    ContentType = "4";
    //} else {
    //    ContentType = ContentType;
    //}
    if ($("#SortNum").numberbox("getText") != "") {

        //如果是上传文件0 /拍照 1
        var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
        //  var Signature = $('#upload_org_code_img').val();//获取签名的地址

        $('#info_form_photo').form('submit', {
            url: "/ScheduleManagement/UploadFile",//接收一般处理程序返回来的json数据     
            onSubmit: function (param) {
                param.TaskId = selectRow.TaskId;
                param.ContentType = ContentType;
                param.SortNum = $("#SortNum").textbox("getText");
                param.Content = $("#Content").textbox("getText");
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $.messager.alert('Tips', result.Message);
                        // $('#take_photo_dialog').dialog('close');
                        View_Take_photo(ContentType);//刷新列表
                        switch (ContentType) {
                            case "1": $('#View_photo_records_datagrid1').datagrid("reload"); break;
                            case "2": $('#View_photo_records_Monitor_datagrid').datagrid("reload"); break;
                            case "3": $('#View_photo_records_Test_datagrid').datagrid("reload"); break;
                            default: break;
                        }
                    }
                    else {
                        $.messager.alert('Tips', result.Message);
                    }
                }
            }
        });
        $('#info_form_photo').form("reset");
    } else {
        $.messager.alert('Tips', 'SortNum can not be empty！');
    };
};
//********************************************查看上传图片列表
function View_Take_photo(flag) {
    if (flag == 2) {
        $("#file_info").css("dispaly", "block");
    } else {
        $("#file_info").css("dispaly", "none");
    }
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    if (selectRow) {
        $('#View_photo_records').dialog({
            width: 700,
            height: 500,
            modal: true,
            title: 'Upload File',
            draggable: true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#View_photo_records').dialog('close');
                }
            }]
        });

    } else {
        $.messager.alert('Tips', 'Please select the row to operate！');
    };
    $('#View_photo_records_datagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: false,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        queryParams: {
            TaskId: selectRow.TaskId,
            ContentType: flag
        },
        url: "/ScheduleManagement/ShowUploadFileList",//接收一般处理程序返回来的json数据  
        columns: [[
      //  { field: 'TaskId', title: 'Task Id',width:100 },
       { field: 'PicName', title: 'PicName', width: 100 },
      //  { field: 'PicUrl', title: 'PicUrl', width: 100 },
       { field: 'SortNum', title: 'SortNum', width: 100 },
       {
           field: 'Content', title: 'Content', width: 150
       }
        ]],
        onLoadSuccess: function (data) {
            $('#View_photo_records_datagrid').datagrid('selectRow', line);
        },
        sortOrder: 'asc',
        toolbar: "#View_photo_record_toolbar"
    });
    //查看图片
    $("#View_photo_list").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_datagrid').datagrid('getSelected');//获取选中行
        if (selectRow_photo) {
            if (selectRow_photo.ContentType != 4) {
                $('#look_photo_detail').dialog({
                    width: 700,
                    height: 500,
                    modal: true,
                    title: 'View Photo',
                    draggable: true,
                    buttons: [{
                        text: 'Close',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $('#look_photo_detail').dialog('close');
                        }
                    }]
                });
                // console.log(selectRow_photo.PicUr);
                $("#detail_img").prop("src", selectRow_photo.PicUrl);
            } else {
                $.messager.alert('Tips', 'This is a file and cannot view photos！');
            }
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
    //删除图片列表
    $("#View_photo_delete").unbind("click").bind("click", function () {
        var selectRow_photo = $('#View_photo_records_datagrid').datagrid('getSelected');//获取选中行
        var selectRow_list = $('#task_record').datagrid('getSelected');//获取任务列表
        if (selectRow_photo) {
            $.messager.confirm('confirm', 'confirm delete？', function (r) {
                if (!r) {
                    return false;
                } else {
                    $.ajax({
                        url: '/ScheduleManagement/DelUploadFile',
                        type: 'POST',
                        data: {
                            id: selectRow_photo.id,
                            ContentType: selectRow_photo.ContentType,
                            ProjectName: selectRow_list.ProjectName
                        },
                        success: function (data) {
                            if (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    //选择当前添加节点
                                    $.messager.alert('tips', result.Message);
                                    $('#View_photo_records_datagrid').datagrid('reload');
                                    $('#View_photo_records_datagrid1').datagrid('reload');

                                } else {
                                    $.messager.alert('tips', result.Message);
                                }
                            }

                        }
                    })
                }
            });
        } else {
            $.messager.alert('Tips', 'Please select the line to operate！');
        }
    });
}


//***************************************************确认完成子项目
function ConfirmComplete() {
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        if (selectRow.TestTaskState == 0) {
            line = $('#task_record').datagrid("getRowIndex", selectRow);
            $.messager.confirm('Tips', 'Confirm submit?', function (r) {
                if (!r) {
                    return false;
                } else {
                    $.ajax({
                        url: "/ScheduleManagement/ConfirmCompletion",//接收一般处理程序返回来的json数据
                        data: {
                            TaskId: selectRow.TaskId,
                            ParentID: selectRow.ParentID,
                            MTRNO: selectRow.MTRNO
                        },
                        type: "POST",
                        success: function (data) {
                            if (data) {
                                var result = $.parseJSON(data);
                                if (result.Success == true) {
                                    $.messager.alert('Tips', result.Message);
                                    $('#task_record').datagrid("reload");
                                }
                                else {
                                    $.messager.alert('Tips', result.Message);
                                }
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert("Tips", "Test task has been completed and cannot be operated！")
        }
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };
}
//*********************************************记录搜索下拉框
function search_combobox() {
    $('#task_search').combobox({
        data: [
           { 'value': 'TaskId', 'text': 'TaskId' },
           { 'value': 'MotoNum', 'text': 'MotoNum' },
           { 'value': 'SampleNo', 'text': 'SampleNo' },
           { 'value': 'TestItem', 'text': 'TestItem' },
           { 'value': 'SubNum', 'text': 'SubNum' },
        ]
    });
};
//***********************************任务列表搜索
function search_task() {
    var key = $('#task_search1').textbox('getText');
    if (key != "") {
        $('#task_record').datagrid(
          {
              type: 'POST',
              dataType: "json",
              url: "/ScheduleManagement/GetTestTaskList",//接收一般处理程序返回来的json数据                
              queryParams: {
                  search: 'SampleNo',
                  key: key,
              }
          });
    } else {
        //清空列表
        var json = {
            "rows": [],
            "total": 0,
            "success": true
        };
        $('#task_record').datagrid("loadData", json);
    }

};
//*********************************************查看测试设备
function TestEquipment_init() {
    //$('#View_TestEquipment_dialog').dialog({
    //    width: 800,
    //    height: 400,
    //    fit: true,
    //    modal: true,
    //    title: 'Test Equipment',
    //    border: false,
    //    buttons: [{
    //        text: 'Close',
    //        iconCls: 'icon-cancel',
    //        handler: function () {
    //            $('#View_TestEquipment_dialog').dialog('close');
    //        }
    //    }]
    //});
    //获取记录的显示title
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        $('#View_TestEquipment_datagrid').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            border: true,
            fitColumns: true,
            height: 680,
            pagination: true,
            ctrlSelect: true,
            pageSize: 15,
            pageList: [15, 30, 45, 60],
            pageNumber: 1,
            queryParams: {
                TaskId: selectRow.TaskId
            },
            url: "/ScheduleManagement/GetTestEquipment",//接收一般处理程序返回来的json数据     
            columns: [[
                { field: 'TaskNum', title: 'TaskNum', width: 100, hidden: true },
                { field: 'EquipmentCode', title: 'EquipmentCode', width: 100 },
                {
                    field: 'EquipmentName', title: 'EquipmentName', width: 150
                }
            ]],
            onLoadSuccess: function (data) {
                $('#View_TestEquipment_datagrid').datagrid('selectRow', 0);
            },
            toolbar: "#View_TestEquipment_toolbar"
        });
    }
    //扫码
    $("#Scan").unbind("click").bind("click", function () {
        Scan();
    });
};
// 回车键事件 
// 绑定键盘按下事件  
$(document).keypress(function (e) {
    // 回车键事件  
    if (e.which == 13) {
        Scan_View();
    }
});
//****************************************扫码
function Scan() {
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    if (selectRow) {
        if (selectRow.TestTaskState == 0) {
            $('#Scan_dialog').dialog({
                width: 700,
                height: 500,
                modal: true,
                title: 'Scan',
                border: false,
                buttons: [
                    {
                        text: 'Save',
                        iconCls: 'icon-ok',
                        handler: function () {
                            Scan_Submit();//扫码提交
                        }
                    },
                    {
                        text: 'Cancel',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $('#Scan_dialog').dialog('close');
                        }
                    }]
            });

        } else {
            $.messager.alert("Tips", "Test task has been completed and cannot be operated！")
        }
    } else {
        $.messager.alert('Tips', 'Please select the row to operate！');
    }
    //回显设备信息
    $("#Scan_View").unbind("click").bind("click", function () {
        Scan_View();
    });
    //删除设备信息
    $("#Scan_delete").unbind("click").bind("click", function () {
        var selectRow_View_TestEquipment_datagrid = $('#View_TestEquipment_datagrid').datagrid('getSelected');//获取选中行
        if (selectRow_View_TestEquipment_datagrid) {
            $.messager.confirm('Tips', 'Confirm delete？', function (r) {
                if (r) {
                    $.ajax({
                        url: "/ScheduleManagement/DelTestEquipment",
                        type: 'POST',
                        data: {
                            id: selectRow_View_TestEquipment_datagrid.id,
                            TaskNum: selectRow_View_TestEquipment_datagrid.TaskNum,//获取选中行的TaskId传给后台
                            EquipmentName: selectRow_View_TestEquipment_datagrid.EquipmentName
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $.messager.alert('Tips', result.Message);
                                $('#View_TestEquipment_datagrid').datagrid('reload');
                            } else {
                                $.messager.alert('Tips', result.Message);

                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Tips', 'Please select the row to operate！');
        }
    });
};
//回显设备信息
function Scan_View() {
    var EquipmentCode = $("#EquipmentCode").textbox("getText");
    var selectRow_View_TestEquipment_datagrid = $('#View_TestEquipment_datagrid').datagrid('getSelected');//获取选中行
    if (EquipmentCode) {
        $.ajax({
            url: "/ScheduleManagement/GetEquipmentInfo",
            type: 'POST',
            data: {
                EquipmentCode: $("#EquipmentCode").textbox("getText")
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $("#Scan_dialog").form("load", result.Data);
                    switch (result.Data.StatusFlag) {
                        case 1: $("#StatusFlag").textbox("setText", "使用中"); break;
                        case 2: $("#StatusFlag").textbox("setText", "维修中"); break;
                        case 3: $("#StatusFlag").textbox("setText", "报废"); break;
                        case 4: $("#StatusFlag").textbox("setText", "送检"); break;
                        default: break;
                    }

                    var str_data = result.Data.CalibrationDate;//获取时间截取
                    if (str_data) {//格式化时间
                        if (str_data.length >= 10) {
                            str_data = str_data.substr(0, 10);
                            $("#CalibrationDate").textbox("setText", str_data);
                        }
                    }


                } else {
                    $.messager.alert('Tips', result.Message);

                }
            }
        });
    } else {
        $.messager.alert('Tips', 'EquipmentCode is null！');
    }
};

//扫码提交
function Scan_Submit() {
    //获取记录的显示title
    var selectRow = $('#task_record').datagrid('getSelected');//获取选中行
    // $('#Scan_dialog').form("reset");
    $('#Scan_dialog').form('submit', {
        url: "/ScheduleManagement/ChooseTestEquipment",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            param.TaskId = selectRow.TaskId;
            return $(this).form('enableValidation').form('validate');
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('Tips', result.Message);
                    $('#View_TestEquipment_datagrid').datagrid("reload");
                    // $('#Scan_dialog').dialog('close');
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        }
    });
}
//***************************************************************************************查看记录数据测试记录
//****************************************加载测试记录列表
function test_data(TaskId, RecordRemark) {
    // var selectRow = $("#task_record").datagrid("getSelected");//获取选中任务行
    //alert(TaskId);
    // console.log(RecordRemark['DataRemark1']);
    //检测记录信息
    $('#test_records_datagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: true,
        fitColumns: true,
        height: 730,
        pagination: true,
        ctrlSelect: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        queryParams: {
            TaskId: TaskId
        },
        type: 'POST',
        dataType: 'json',
        url: "/ScheduleManagement/GetTestRecordList",//接收一般处理程序返回来的json数据  
        columns: [[
          { field: 'TaskId', title: 'TaskId', sortable: 'true' },
          { field: 'MotoNum', title: 'MotoNum' },
          { field: 'Data1', title: RecordRemark['DataRemark1'], hidden: RecordRemark['DataRemark1'] == null ? true : false },
          { field: 'Data2', title: RecordRemark['DataRemark2'], hidden: RecordRemark['DataRemark2'] == null ? true : false },
          { field: 'Data3', title: RecordRemark['DataRemark3'], hidden: RecordRemark['DataRemark3'] == null ? true : false },
          { field: 'Data4', title: RecordRemark['DataRemark4'], hidden: RecordRemark['DataRemark4'] == null ? true : false },
          { field: 'Data5', title: RecordRemark['DataRemark5'], hidden: RecordRemark['DataRemark5'] == null ? true : false },
          { field: 'Data6', title: RecordRemark['DataRemark6'], hidden: RecordRemark['DataRemark6'] == null ? true : false },
          { field: 'Data7', title: RecordRemark['DataRemark7'], hidden: RecordRemark['DataRemark7'] == null ? true : false },
          { field: 'Data8', title: RecordRemark['DataRemark8'], hidden: RecordRemark['DataRemark8'] == null ? true : false },
          { field: 'Data9', title: RecordRemark['DataRemark9'], hidden: RecordRemark['DataRemark9'] == null ? true : false },
          { field: 'Data10', title: RecordRemark['DataRemark10'], hidden: RecordRemark['DataRemark10'] == null ? true : false },
          { field: 'Data11', title: RecordRemark['DataRemark11'], hidden: RecordRemark['DataRemark11'] == null ? true : false },
          { field: 'Data12', title: RecordRemark['DataRemark12'], hidden: RecordRemark['DataRemark12'] == null ? true : false },
          { field: 'Data13', title: RecordRemark['DataRemark13'], hidden: RecordRemark['DataRemark13'] == null ? true : false },
          { field: 'Data14', title: RecordRemark['DataRemark14'], hidden: RecordRemark['DataRemark14'] == null ? true : false },
          { field: 'Data15', title: RecordRemark['DataRemark15'], hidden: RecordRemark['DataRemark15'] == null ? true : false },
          { field: 'Data16', title: RecordRemark['DataRemark16'], hidden: RecordRemark['DataRemark16'] == null ? true : false },
          { field: 'Data17', title: RecordRemark['DataRemark17'], hidden: RecordRemark['DataRemark17'] == null ? true : false },
          { field: 'Data18', title: RecordRemark['DataRemark18'], hidden: RecordRemark['DataRemark18'] == null ? true : false },
          { field: 'Data19', title: RecordRemark['DataRemark19'], hidden: RecordRemark['DataRemark19'] == null ? true : false },
          { field: 'Data20', title: RecordRemark['DataRemark20'], hidden: RecordRemark['DataRemark20'] == null ? true : false },
          { field: 'Data21', title: RecordRemark['DataRemark21'], hidden: RecordRemark['DataRemark21'] == null ? true : false },
          { field: 'Data22', title: RecordRemark['DataRemark22'], hidden: RecordRemark['DataRemark22'] == null ? true : false },
          { field: 'Data23', title: RecordRemark['DataRemark23'], hidden: RecordRemark['DataRemark23'] == null ? true : false },
          { field: 'Data24', title: RecordRemark['DataRemark24'], hidden: RecordRemark['DataRemark24'] == null ? true : false },
          { field: 'Data25', title: RecordRemark['DataRemark25'], hidden: RecordRemark['DataRemark25'] == null ? true : false },
          { field: 'Data26', title: RecordRemark['DataRemark26'], hidden: RecordRemark['DataRemark26'] == null ? true : false },
          { field: 'Data27', title: RecordRemark['DataRemark27'], hidden: RecordRemark['DataRemark27'] == null ? true : false },
          { field: 'Data28', title: RecordRemark['DataRemark28'], hidden: RecordRemark['DataRemark28'] == null ? true : false },
          { field: 'Data29', title: RecordRemark['DataRemark29'], hidden: RecordRemark['DataRemark29'] == null ? true : false },
          { field: 'Data30', title: RecordRemark['DataRemark30'], hidden: RecordRemark['DataRemark30'] == null ? true : false },
          { field: 'Data31', title: RecordRemark['DataRemark31'], hidden: RecordRemark['DataRemark31'] == null ? true : false },
          { field: 'Data32', title: RecordRemark['DataRemark32'], hidden: RecordRemark['DataRemark32'] == null ? true : false },
          { field: 'Data33', title: RecordRemark['DataRemark33'], hidden: RecordRemark['DataRemark33'] == null ? true : false },
          { field: 'Data34', title: RecordRemark['DataRemark34'], hidden: RecordRemark['DataRemark34'] == null ? true : false },
          { field: 'Data35', title: RecordRemark['DataRemark35'], hidden: RecordRemark['DataRemark35'] == null ? true : false },
          { field: 'Data36', title: RecordRemark['DataRemark36'], hidden: RecordRemark['DataRemark36'] == null ? true : false },
          { field: 'Data37', title: RecordRemark['DataRemark37'], hidden: RecordRemark['DataRemark37'] == null ? true : false },
          { field: 'Data38', title: RecordRemark['DataRemark38'], hidden: RecordRemark['DataRemark38'] == null ? true : false },
          { field: 'Data39', title: RecordRemark['DataRemark39'], hidden: RecordRemark['DataRemark39'] == null ? true : false },
          { field: 'Data40', title: RecordRemark['DataRemark40'], hidden: RecordRemark['DataRemark40'] == null ? true : false },
          { field: 'Data41', title: RecordRemark['DataRemark41'], hidden: RecordRemark['DataRemark41'] == null ? true : false },
          { field: 'Data42', title: RecordRemark['DataRemark42'], hidden: RecordRemark['DataRemark42'] == null ? true : false },
          { field: 'Data43', title: RecordRemark['DataRemark43'], hidden: RecordRemark['DataRemark43'] == null ? true : false },
          { field: 'Data44', title: RecordRemark['DataRemark44'], hidden: RecordRemark['DataRemark44'] == null ? true : false },
          { field: 'Data45', title: RecordRemark['DataRemark45'], hidden: RecordRemark['DataRemark45'] == null ? true : false },
          { field: 'Data46', title: RecordRemark['DataRemark46'], hidden: RecordRemark['DataRemark46'] == null ? true : false },
          { field: 'Data47', title: RecordRemark['DataRemark47'], hidden: RecordRemark['DataRemark47'] == null ? true : false },
          { field: 'Data48', title: RecordRemark['DataRemark48'], hidden: RecordRemark['DataRemark48'] == null ? true : false },
          { field: 'Data49', title: RecordRemark['DataRemark49'], hidden: RecordRemark['DataRemark49'] == null ? true : false },
          { field: 'Data50', title: RecordRemark['DataRemark50'], hidden: RecordRemark['DataRemark50'] == null ? true : false },
          { field: 'Data51', title: RecordRemark['DataRemark51'], hidden: RecordRemark['DataRemark51'] == null ? true : false },
          { field: 'Data52', title: RecordRemark['DataRemark52'], hidden: RecordRemark['DataRemark52'] == null ? true : false },
          { field: 'Data53', title: RecordRemark['DataRemark53'], hidden: RecordRemark['DataRemark53'] == null ? true : false },
          { field: 'Data54', title: RecordRemark['DataRemark54'], hidden: RecordRemark['DataRemark54'] == null ? true : false },
          { field: 'Data55', title: RecordRemark['DataRemark55'], hidden: RecordRemark['DataRemark55'] == null ? true : false },
          { field: 'Data56', title: RecordRemark['DataRemark56'], hidden: RecordRemark['DataRemark56'] == null ? true : false },
          { field: 'Data57', title: RecordRemark['DataRemark57'], hidden: RecordRemark['DataRemark57'] == null ? true : false },
          { field: 'Data58', title: RecordRemark['DataRemark58'], hidden: RecordRemark['DataRemark58'] == null ? true : false },
          { field: 'Data59', title: RecordRemark['DataRemark59'], hidden: RecordRemark['DataRemark59'] == null ? true : false },
          { field: 'Data60', title: RecordRemark['DataRemark60'], hidden: RecordRemark['DataRemark60'] == null ? true : false },
          { field: 'Data61', title: RecordRemark['DataRemark61'], hidden: RecordRemark['DataRemark61'] == null ? true : false },
          { field: 'Data62', title: RecordRemark['DataRemark62'], hidden: RecordRemark['DataRemark62'] == null ? true : false },
          { field: 'Data63', title: RecordRemark['DataRemark63'], hidden: RecordRemark['DataRemark63'] == null ? true : false },
          { field: 'Data64', title: RecordRemark['DataRemark64'], hidden: RecordRemark['DataRemark64'] == null ? true : false }
        ]],
        onLoadSuccess: function (data) {
            $('#test_records_datagrid').datagrid('selectRow', 0);
        },
        sortOrder: 'asc',
        toolbar: "#test_records_toolbar"
    });
};
//**************************************修改测试记录
function edit_test_records() {
    var selectRow = $("#test_records_datagrid").datagrid("getSelected");//获取选中行测试记录行
    var task_record_select = $("#task_record").datagrid("getSelected")
    if (selectRow) {
        if (task_record_select.TestTaskState == 0) {
            line = $('#test_records_datagrid').datagrid("getRowIndex", selectRow);
            $('#edit_test_records').dialog({
                width: 750,
                height: 500,
                modal: true,
                title: 'Edit Test Records',
                draggable: true,
                buttons: [{
                    text: 'Save',
                    iconCls: 'icon-ok',
                    id: 'save_edit'
                }, {
                    text: 'Cancel',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#edit_test_records').dialog('close');
                    }
                }]
            });
            //获取记录的显示title
            var RecordRemark;
            var selectRow_1 = $("#task_record").datagrid("getSelected");//获取选中任务行
            $.ajax({
                url: "/ScheduleManagement/GetTestRecordRemarkList",//接收一般处理程序返回来的json数据     
                data: { TaskId: selectRow_1.TaskId },
                type: "POST",
                async: false,
                success: function (data) {
                    RecordRemark = JSON.parse(data);//将获取出来的title json对象转化成数组
                }
            });

            var select = $('#test_records_datagrid').datagrid('getSelected'); //获取选中的测试数据  
            var str1 = "";//定义一个空的变量，去拼接form里面的样式和数据
            //把固定的TaskId和MotoNum拼接到form里面
            var str = '<div style="float:left;width:50%;margin-top:20px;"><span style="text-align:right;display:inline-block;width:120px;">TaskId：</span><label><input id="TaskId" name="TaskId" value="' + select.TaskId + '" name="' + select.TaskId + '" readonly="readonly"  style="width: 180px; height: 23px;border:1px solid #ccc;border-radius:5px;"/></label></div><div style="float:left;width:50%;margin-top:20px"><span style="text-align:right;display:inline-block;width:120px;">MotoNum：</span><label><input name="' + select.MotoNum + '" value="' + select.MotoNum + '"  style="width: 180px; height: 23px;border:1px solid #ccc;border-radius:5px;" /></label></div>';

            //循环遍历出回显的form表单的值
            for (var i = 1; i <= 64; i++) {
                if (RecordRemark['DataRemark' + i] != null) {
                    str1 += '<div style="float:left;width:50%;margin-top:10px"><span style="text-align:right;display:inline-block;width:120px;">' + RecordRemark['DataRemark' + i] + '：</span><label><input id="' + RecordRemark['DataRemark' + i] + '" value="' + select['Data' + i] + '"  name="' + RecordRemark['DataRemark' + i] + '"  style="width: 180px; height: 23px;border:1px solid #ccc;border-radius:5px;" /></label></div>'

                } else if (RecordRemark['DataRemark' + i] == "") {
                    str1 += '<div style="float:left;width:50%;display:none"><span style="text-align:right;width:120px;">' + RecordRemark['DataRemark' + i] + '：</span><label><input id="' + RecordRemark['DataRemark' + i] + '" value="' + select['Data' + i] + '"  name="' + RecordRemark['DataRemark' + i] + '"  style="width: 150px; height: 23px;border:1px solid #ccc;border-radius:5px;" /></label></div>'

                }
            };
            $("#edit_test_records").html(str + str1);//把数据和样式添加到form表单中
            $("#save_edit").unbind('click').bind('click', function () {
                //form表单提交
                $('#edit_test_records').form('submit', {
                    url: "/ScheduleManagement/EditTestRecord",//接收一般处理程序返回来的json数据     
                    onSubmit: function (param) {
                        param.id = selectRow_1.TaskId  //获取选中行的TaskId传给后台
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#test_records_datagrid').datagrid('reload');//重新加载测试记录的列表
                            $('#edit_test_records').dialog('close');//关闭修改测试记录的弹窗
                            $.messager.alert('Tips', result.Message);

                        } else {
                            $.messager.alert('Tips', result.Message);
                        }
                    }
                });

            })
        } else {
            $.messager.alert("Tips", "Test task has been completed and cannot be operated！")
        }
    } else {
        $.messager.alert('Tips', 'Please select the task you want to operate！');
    }
};
//********************************************导入测试记录
function import_records() {
    var task_record_select = $("#task_record").datagrid("getSelected")
    if (task_record_select.TestTaskState == 0) {
        $('#import_file').dialog({
            width: 500,
            height: 360,
            modal: true,
            title: 'Import Data',
            draggable: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                id: 'import_file_save'
            }, {
                text: 'Cancel',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#import_file').dialog('close');//关闭导入测试记录弹窗
                }
            }]
        });
    } else {
        $.messager.alert("Tips", "Test task has been completed and cannot be operated！")
    };

    $('#import_file_save').unbind('click').bind('click', function () {
        var get_row = $("#task_record").datagrid("getSelected");//获取选中行
        //form表单提交
        $('#import_file').form('submit', {
            url: "/ScheduleManagement/ImportDate",
            onSubmit: function (param) {
                param.TaskId = get_row.TaskId;
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#import_file').dialog('close');
                    $('#test_records_datagrid').datagrid('reload')
                    $.messager.alert('Tips', result.Message);
                }
                else if (result.Success == false) {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    });
    //导入文件表单重置
    $('#import_file').form('reset');
};
/*
 * 
 *functionName:UploadTestDataFile_datagrid_init()
 *function:查看上传信息列表
 *Param 参数
 *author:张慧敏
 *date:2018/4/23
*/
function ViewTestDataFile_datagrid_init() {
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中行
    //$('#UploadTestDataFile_dialog').dialog({
    //    title: 'ViewUploadTestDataFile',
    //    width: 800,
    //    height: 500,
    //    buttons: [{
    //        text: 'Close',
    //        iconCls: 'icon-cancel',
    //        handler: function () {
    //            $('#UploadTestDataFile_dialog').dialog('close');
    //        }
    //    }]

    //});
    $('#UploadTestDataFile_datagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        border: false,
        fit: true,
        height: 780,
        pagination: true,
        fitColumns: true,
        pageSize: 30,
        pageList: [30, 60, 90, 120],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        queryParams: {
            TaskId: selectRow.TaskId
        },
        url: "/ScheduleManagement/GetTestDataFileList",//接收一般处理程序返回来的json数据   
        columns: [[
           {
               title: 'DataType', field: 'DataType', width: 150, formatter: function (value, row, index) {
                   switch (value) {
                       case 1: value = "记录数据"; break;
                       case 2: value = "报告数据"; break;
                       case 3: value = "原始数据"; break;
                       default: break;
                   }
                   return value;
               }
           },
           { title: 'DataFileName', field: 'DataFileName', width: 150 },
           { title: 'DataFormat', field: 'DataFormat', width: 150 },
           { title: 'TaskId', field: 'TaskId', hidden: true },
           { title: 'DataUrl', field: 'DataUrl', hidden: true }
        ]],
        onLoadSuccess: function (node, data) {
            $('#UploadTestDataFile_datagrid').datagrid('selectRow', 0);//默认选中第一行
        },
        sortOrder: 'asc',
        toolbar: "#UploadTestDataFile_datagrid_toolbar"
    });
    //上传测试记录文件
    $("#UploadTestDataFile_upload").unbind("click").bind("click", function () {
        UploadTestDataFile();
    });
    //下载测试记录文件
    $("#UploadTestDataFile_download").unbind("click").bind("click", function () {
        DownloadTestDataFile();
    });
    //在线编辑报告数据
    $("#EditTestDataFile").unbind("click").bind("click", function () {
        EditTestDataFile();
    });
    //删除测试记录文件
    $("#Delete_UploadData").unbind("click").bind("click", function () {
        DeleteUploadData();
    });
}
/*
 * 
 *functionName:DeleteUploadData()
 *function:删除测试记录文件
 *Param 参数
 *author:张慧敏
 *date:2018/5/15
*/

function DeleteUploadData() {
    var selectRow = $("#UploadTestDataFile_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('Tips', 'Confirm delete？', function (r) {
            if (r) {
                $.ajax({
                    url: "/ScheduleManagement/DelTestDataFile",
                    type: 'POST',
                    data: {
                        id: selectRow.Id,
                        TaskId: selectRow.TaskId,//获取选中行的TaskId传给后台
                        DataType: selectRow.DataType
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('Tips', result.Message);
                            $('#UploadTestDataFile_datagrid').datagrid('reload');
                        } else {
                            $.messager.alert('Tips', result.Message);

                        }
                    }
                });
            }
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
}
/*
 * 
 *functionName:UploadTestDataFile()
 *function:上传信息列表
 *Param 参数
 *author:张慧敏
 *date:2018/4/23
*/
function UploadTestDataFile() {
    //DataType
    $("#DataType").combobox({
        data: [
           { 'value': "1", 'text': '记录数据' },
           { 'value': "2", 'text': '报告数据' }
        ]
    });
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中行
    if (selectRow) {
        if (selectRow.TestTaskState == 0) {
            line = $('#task_record').datagrid("getRowIndex", selectRow);
            initrecord_template();//加载记录模板
            $('#UploadTestDataFile_form').dialog({
                title: 'UploadTestDataFile',
                width: 700,
                height: 550,
                buttons: [{
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#UploadTestDataFile_form').dialog('close');
                    }
                }]

            });
        } else {
            $.messager.alert("Tips", "Test task has been completed and cannot be operated！");
        };
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    //查看上传记录模板文件
    $("#upload_file_save").unbind("click").bind("click", function () {
        upload_file();
    });
    // 上传
    function upload_file() {
        var selectRow = $("#task_record").datagrid("getSelected");//获取选中行
        $('#UploadTestDataFile_form').form('submit', {
            url: "/ScheduleManagement/UploadTestDataFile",//接收一般处理程序返回来的json数据    
            onSubmit: function (param) {
                param.TaskId = selectRow.TaskId
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $("#UploadTestDataFile_form").dialog('close');//关闭导入DVP弹窗
                        $("#UploadTestDataFile_datagrid").datagrid("reload");
                        $.messager.alert('Tips', result.Message);
                    }
                    else if (result.Success == false) {
                        $.messager.alert('Tips', result.Message);
                    }
                }
            }
        });
    };
    $('#UploadTestDataFile_form').form('reset');//重置表单
}
/*
 * 
 *functionName:UploadTestDataFile()
 *function:下载信息列表
 *Param 参数
 *author:张慧敏
 *date:2018/4/23
*/
function DownloadTestDataFile() {
    var selectRow = $("#UploadTestDataFile_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        $.ajax({
            url: "/ScheduleManagement/DownloadTestDataFile",//接收一般处理程序返回来的json数据     
            data: {
                Id: selectRow.Id
            },
            type: "POST",
            async: false,
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    window.location = result.Message;
                }
                else if (result.Success == false) {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });

    } else {
        $.messager.alert('Tips', 'Please select the row');
    }
};
/*
 * 
 *functionName:EditTestDataFile()
 *function:在线编辑报告数据
 *Param id，Operation_Type
 *author:程媛
 *date:2018/5/18
*/
function EditTestDataFile() {
    var selected_Item = $("#UploadTestDataFile_datagrid").datagrid("getSelected");
    if (selected_Item) {
        if (selected_Item.DataType == 2) {
            $("#ShowOffice3").prop("href", WordUrlSpit3[0] + '?id=' + selected_Item.Id + '&Operation_Type=15&pageName=FileManagement' + WordUrlSpit3[1]);
            document.getElementById('ShowOffice3').click();
        } else {
            $.messager.alert('Tips', 'This data cannot be edited online！');
        }
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
/*
 * 
 *functionName:View_TestInformation()
 *function:查看上传记录信息
 *Param 参数
 *author:张慧敏
 *date:2018/4/16
*/
function View_TestInformation() {

    //获取记录的显示title
    var selectRow = $("#task_record").datagrid("getSelected");//获取选中任务行
    if (selectRow) {
        $("#ShowOffice_").prop("href", WordUrlSpit[0] + '?ParentID=' + selectRow.ParentID + '&TaskId=' + selectRow.TaskId + WordUrlSpit[1]);
        document.getElementById('ShowOffice_').click();

    }
}
/*
 * 上次记录模板
 *functionName:initrecord_template()
 *function:上传记录模板文件
 *Param 参数
 *author:张慧敏
 *date:2018/4/13
*/
function initrecord_template() {
    var selected_report = $("#task_record").datagrid("getSelected");
    if (selected_report) {
        //if (selected_report.TestTaskState == "0") {
        line = $('#task_record').datagrid("getRowIndex", selected_report);
        // var type = $('#Template_type').combobox('getText');
        //详细位置---领取信息dialog
        $('#recordTemplate').combobox({
            url: "/ScheduleManagement/GetAccreditTempList",//接收一般处理程序返回来的json数据      
            valueField: 'Value',
            textField: 'Text',
            //  required: true,
            queryParams: {
                ProjectId: selected_report.ProjectId
            },
            //editable: false,
            //本地联系人数据模糊索引
            filter: function (q, row) {
                var opts = $(this).combobox('options');
                return row[opts.textField].indexOf(q) >= 0;
            }
        });
        //$('#record_template_form').form("reset");
        //$('#record_template_form').dialog({
        //    width: 680,
        //    height: 400,
        //    modal: true,
        //    title: 'Upload Record Template',
        //    border: false,
        //    buttons: [
        //        {
        //            text: 'Close',
        //            iconCls: 'icon-cancel',
        //            handler: function () {
        //                $('#record_template_form').dialog('close');
        //            }
        //        }]
        //});
    } else {
        $.messager.alert("Tips", "Test task has been completed and cannot be operated！")
    };

    //} else {
    //    $.messager.alert('Tips', 'Please select the row to be operated');
    //}
    //查看上传记录模板文件
    $("#view_record_template_temp").unbind("click").bind("click", function () {
        view_record_template_temp();
    });
    //载入上传记录模板文件
    $('#load_record_template').unbind('click').bind('click', function () {
        load_record_template();
    });
    //删除上传记录模板文件
    $("#delete_record_template").unbind("click").bind("click", function () {
        delete_record_template();
    });
}
/*
 * 查看上传记录模板文件
 *functionName:view_record_template_temp()
 *function:查看上传记录模板文件
 *Param 参数
 *author:张慧敏
 *date:2018/4/13
*/
function view_record_template_temp() {
    var TemplateId = $("#recordTemplate").combobox("getValue");//模板id
    if (TemplateId) {
        $("#ShowOffice").prop("href", WordUrlSpit1[0] + '?id=' + TemplateId + '&Operation_Type=6' + WordUrlSpit1[1]);
        document.getElementById('ShowOffice').click();
    }
    else {
        $.messager.alert('Tips', 'Please select the recordTemplate');
    }
};
/*
 * 载入上传记录模板文件
 *functionName:load_record_template()
 *function:载入上传记录模板文件
 *Param 参数
 *author:张慧敏
 *date:2018/4/13
*/
function load_record_template() {
    var selectRow_report = $("#task_record").datagrid("getSelected");
    var id = $("#recordTemplate").combobox("getValue");
    if (id != "") {
        $.ajax({
            url: "/ScheduleManagement/addTestRecordreport",
            dataType: "text",
            type: 'POST',
            data: {
                Templateid: id,
                DataFileName: $("#recordTemplate").combobox("getText"),
                //SampleReceivedTiming: selectRow_report.SampleReceivedTiming,
                //ReportModified: selectRow_report.ReportModified,
                TaskId: selectRow_report.TaskId,
                DataType: 2
                //id: id
            },
            success: function (data) {
                if (data) {
                    var obj = $.parseJSON(data);
                    if (obj.Success == true) {
                        // $("#Edit_online_report").click();
                        //$.messager.alert('Tips', obj.Message);

                        edit_Record_Template(obj.Message);//显示可以编辑word
                        //   $.messager.alert('Tips', obj.Message);
                    }
                    else if (obj.Success == false) {
                        $.messager.alert('Tips', obj.Message);
                    }
                }
            }
        });
        //模板没选择操作
    } else if (id == "") {
        $.messager.alert('tips', 'please choose the templete！');

    }
    try {
        $.messager.progress('close');
    }
    catch (r) { }

};
/*
 * 删除上传记录模板文件
 *functionName:load_record_template()
 *function:删除上传记录模板文件
 *Param 参数
 *author:张慧敏
 *date:2018/4/13
*/
function delete_record_template() {
    var selectRow = $("#task_record").datagrid("getSelected");
    var SubReportUrl = $("#recordTemplate").combobox("getValue");//获取combobox的value值
    if (SubReportUrl != "") {
        $.messager.confirm('Tips', 'Are you sure you want to delete this report？', function (r) {
            if (r) {
                $.ajax({
                    url: "/ScheduleManagement/DelReport",
                    type: 'POST',
                    data: {
                        TaskId: selectRow.id,//获取选中行的TaskId传给后台
                        SubReportUrl: SubReportUrl//获取下拉框的文件地址传给后台
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('Tips', result.Message);
                            $('#task_record').datagrid('reload');
                            $('#View_TestTask_datagrid').datagrid('reload');
                        } else {
                            $.messager.alert('Tips', result.Message);

                        }
                    }
                });
            }
        });
    } else {
        $.messager.alert('tips', 'please choose the templete report！');
    }
};
/*
 * 编辑上传记录模板文件
 *functionName:edit_Record_Template()
 *function:编辑上传记录模板文件
 *Param 参数
 *author:张慧敏
 *date:2018/4/13
*/
function edit_Record_Template(returnid) {
    var selected_Item = $("#task_record").datagrid("getSelected");
    //var id = $("#recordTemplate").combobox("getValue")
    if (selected_Item) {
        $('#UploadTestDataFile_datagrid').datagrid('reload');
        $("#ShowOffice1").prop("href", ShowOffice2Spit[0] + '?id=' + returnid + '&Operation_Type=15' + ShowOffice2Spit[1]);
        document.getElementById('ShowOffice1').click();
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};


//鼠标经过datagrid显示提示
//$(function () {
//    //验证数字框
//    $.extend($.fn.validatebox.defaults.rules, {
//        equals: {
//            validator: function (value, param) {
//                return value == $(param[0]).val();
//            },
//            message: 'Field do not match.'
//        },
//        onlyNum: {
//            validator: function (value, param) {
//                var reg = /^\d+$/g;
//                return reg.test(value);
//            },
//            message: '该项只能输入数字！'
//        }
//    });

//    $.extend($.fn.datagrid.methods, {
//        doCellTip1: function (jq, params) {
//            function showTip(showParams, td, e, dg) {
//                //无文本，不提示。
//                //if ($(td).text() == "") return;
//                params = params || {};
//                var options = dg.data('datagrid');
//                //var styler = 'style="';
//                //if (showParams.width) {
//                //    styler = styler + "width:" + showParams.width + ";";
//                //}
//                //if (showParams.maxWidth) {
//                //    styler = styler + "max-width:" + showParams.maxWidth + ";";
//                //}
//                //if (showParams.minWidth) {
//                //    styler = styler + "min-width:" + showParams.minWidth + ";";
//                //}
//                //styler = styler + '"';
//                showParams.content = '<div class="tipcontent" style="width:100px;">红色：加急委托</div>';
//                $(td).tooltip({
//                    content: showParams.content,
//                    trackMouse: true,
//                    position: params.position,
//                    onHide: function () {
//                        $(this).tooltip('destroy');
//                    },
//                    onShow: function () {
//                        var tip = $(this).tooltip('tip');
//                        if (showParams.tipStyler) {
//                            tip.css(showParams.tipStyler);
//                        }
//                        if (showParams.contentStyler) {
//                            tip.find('div.tipcontent').css(showParams.contentStyler);
//                        }
//                    }
//                }).tooltip('show');
//            };
//            return jq.each(function () {
//                var grid = $(this);
//                var options = $(this).data('datagrid');
//                if (!options.tooltip) {
//                    var panel = grid.datagrid('getPanel').panel('panel');
//                    panel.find('.datagrid-body').each(function () {
//                        var delegateEle = $(this).find('> div.datagrid-body-inner').length ? $(this).find('> div.datagrid-body-inner')[0] : this;
//                        $(delegateEle).undelegate('td', 'mouseover').undelegate('td', 'mouseout').undelegate('td', 'mousemove').delegate('td[field]', {
//                            'mouseover': function (e) {
//                                //if($(this).attr('field')===undefined) return;
//                                var that = this;
//                                var setField = null;
//                                if (params.specialShowFields && params.specialShowFields.sort) {
//                                    for (var i = 0; i < params.specialShowFields.length; i++) {
//                                        if (params.specialShowFields[i].field == $(this).attr('field')) {
//                                            setField = params.specialShowFields[i];
//                                        }
//                                    }
//                                }
//                                if (setField == null) {
//                                    options.factContent = $(this).find('>div').clone().css({ 'margin-left': '-5000px', 'width': 'auto', 'display': 'inline', 'position': 'absolute' }).appendTo('body');
//                                    var factContentWidth = options.factContent.width();
//                                    params.content = $(this).text();
//                                    if (params.onlyShowInterrupt) {
//                                        if (factContentWidth > $(this).width()) {
//                                            showTip(params, this, e, grid);
//                                        }
//                                    } else {
//                                        showTip(params, this, e, grid);
//                                    }
//                                } else {
//                                    panel.find('.datagrid-body').each(function () {
//                                        var trs = $(this).find('tr[datagrid-row-index="' + $(that).parent().attr('datagrid-row-index') + '"]');
//                                        trs.each(function () {
//                                            var td = $(this).find('> td[field="' + setField.showField + '"]');
//                                            if (td.length) {
//                                                params.content = td.text();
//                                            }
//                                        });
//                                    });
//                                    showTip(params, this, e, grid);
//                                }
//                            },
//                            'mouseout': function (e) {
//                                if (options.factContent) {
//                                    options.factContent.remove();
//                                    options.factContent = null;
//                                }
//                            }
//                        });
//                    });
//                }
//            });
//        }
//    });
//});