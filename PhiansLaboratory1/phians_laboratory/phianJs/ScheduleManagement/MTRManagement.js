var line = 0;
$(function () {
    MTR_information_datagrid_init();//MTR信息列表加载
    //下拉框内容
    $("#ReportModified").combobox({
        value: true,
        data: [
           { 'value': true, 'text': 'Yes' },
           { 'value': false, 'text': 'No' }
        ]
    });
    $("#CNASLogo").combobox({
        value: true,
        data: [
           { 'value': true, 'text': 'Yes' },
           { 'value': false, 'text': 'No' }
        ]
    });
    //导出word
    $('#export_word').unbind('click').bind('click', function () {
        $("#export_word").prop("checked", "checked");
        $("#export_pdf").prop("checked", false);
    });
    //导出pdf
    $('#export_pdf').unbind('click').bind('click', function () {
        $("#export_pdf").prop("checked", "checked");
        $("#export_word").prop("checked", false);
    });
    //手动排程
    $("#ScheduleProject").unbind("click").bind("click", function () {
        ScheduleProjectform();
    });

});
/*
*functionName:ScheduleProjectform
*function：打开窗口手动排程
*Param: 
*author:程媛
*date:2018-09-17
*/
function ScheduleProjectform() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        var name = '文件查看';                            //网页名称，可为空; 
        var iWidth = screen.width - 20;                          //弹出窗口的宽度; 
        var iHeight = screen.availHeight - 100;                         //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = 10;
        //获得窗口的水平位置 
        var iLeft = 10;
        window.open("/ScheduleManagement/ScheduleProject?MTRNO=" + escape(selectRow.MTRNO) + "", name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }

};
//*******************************************MTR信息列表********************************************************************************************************************************************************************************************************
function MTR_information_datagrid_init() {
    //搜索信息的下拉框
    $('#key').combobox({
        panelHeight: 120,
        data: [
                { 'value': 'TM.MTRNO', 'text': 'MTRNO' },
                { 'value': 'MO.SampleNo', 'text': 'Sample No.' },
                { 'value': 'MO.SampleName', 'text': 'Sample Name' },
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
        remoteSort: false,
        url: "/ScheduleManagement/GetMTRMenagementList",
        columns: [[
       { field: 'MTRNO', title: 'MTR No.', sortable: true },//MTR单号
       { field: 'ProjectNo', title: 'Project No.' },//测试项目编号
       { field: 'Application', title: 'Application' },//测试样品的类型
       { field: 'CostCenter', title: 'Cost Center' },//项目所属成本中心
       { field: 'ProjectEng', title: 'Project Eng.' },//项目所属工程师
       { field: 'BU', title: 'Customer' },//客户
       { field: 'Purpose', title: 'Purpose' },//测试目的
       { field: 'FollowUp_n', title: 'Follow Up' },//跟进人
       { field: 'SampleNo', title: 'Sample No.' },//样品编号
       { field: 'SampleName', title: 'Sample Name' },//样品名称
       { field: 'SampleQty', title: 'Sample Qty' },//测试样品数量
      // { field: 'SamplePosition', title: 'Sample Position' },//样本位置
       //{
       //    field: 'Identification', title: 'Identification', formatter: function (value, row, index) {//标识
       //        if (value == true) {
       //            return "Normal";
       //        } else if (value == false) {
       //            return "Abnormal";
       //        }
       //    }
       //},
       {
           field: 'ReceivingDate', title: 'Receiving Date', width: 150, formatter: function (value, row, index) {//接收日期
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'PlanStartTiming', title: 'Plan Start Date', width: 150, formatter: function (value, row, index) {//计划开始时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },
       {
           field: 'EstimatedCompletionTiming', title: 'Expected Complete Date', formatter: function (value, row, index) {//期望完成时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },

       {
           field: 'MTRState', title: 'Status', formatter: function (value, row, index) {//MTR状态,
               switch (value) {
                   case 1: value = 'MTR评审'; break;
                   case 2: value = '样品接收'; break;
                   case 3: value = '测试记录'; break;
                   case 4: value = '报告编制'; break;
                   case 5: value = '报告审核'; break;
                   case 6: value = '报告审核退回'; break;
                   case 7: value = '报告签发'; break;
                   case 8: value = '报告签发退回'; break;
                   case 9: value = '报告完成'; break;
                   default: break;
               }
               return value;
           }
       },
       {
           field: 'OrdersReceived', title: 'Orders Received', formatter: function (value, row, index) {//订单接收时间
               if (value) {//格式化时间
                   if (value.length >= 10) {
                       value = value.substr(0, 10)
                       return value;
                   }
               }
           }
       },//
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
           field: 'CNASLogo', title: 'CNAS Logo', width: 100, formatter: function (value, row, index) {//
               if (value == true) {
                   return "Yes";
               } else if (value == false) {
                   return "No";
               }
           }
       },
       {
           field: 'remark', title: 'Remark', width: 150, formatter: function (value, row, index) {//说明
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
    //添加MTR信息
    $("#MTR_add").unbind("click").bind("click", function () {
        MTR_add();
    });
    //修改MTR信息
    $("#MTR_edit").unbind("click").bind("click", function () {
        MTR_edit();
    });
    //删除MTR信息
    $("#MTR_delete").unbind("click").bind("click", function () {
        MTR_delete();
    });
    //查看DVP信息
    $("#LookDVP").unbind("click").bind("click", function () {
        LookDVP();
    });
    //导出DVP信息
    $("#ExportDVP").unbind("click").bind("click", function () {
        ExportDVP();
    });
    //导入DVP信息
    $("#ImportDVP").unbind("click").bind("click", function () {
        ImportDVP();
    });
    //绘制流程图
    $("#Map_processes").unbind("click").bind("click", function () {
        Map_processes();
    });
    //搜索mtr信息
    $("#search").unbind("click").bind("click", function () {
        SearchMTR();
    });
    //搜索mtr信息
    $("#View_TaskRecord").unbind("click").bind("click", function () {
        LookTestRecord();
    });
    //查看报告
    $('#View_Report').unbind("click").bind("click", function () {
        look_item();
    });
    //提交样品接收
    $("#Submit_Sample_Accetion").unbind("click").bind("click", function () {
        Submit_Sample_Accetion();
    });
    
};
function Mot() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    return selectRow.MTRNO;
}
//****************导入DVP信息
function ImportDVP() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        line = $('#MTR_information_datagrid').datagrid("getRowIndex", selectRow);
        //前缀
        $('#Prefixion').combobox({
            url: "/Common/GetDictionaryList",
            valueField: 'Value',
            textField: 'Text',
            onBeforeLoad: function (param) {
                param.Parent_id = 'a620b59f-4659-4022-8b46-543108332070';

            },
            onSelect: function () {
                $("#Prefixion2").textbox("setText", $("#Prefixion").combobox("getText"));
                $("#suffix").textbox("setText", $("#Prefixion").combobox("getValue"));
            },
            filter: function (q, row) {
                var opts = $(this).combobox('options');
                return row[opts.textField].indexOf(q) >= 0;
            }
        });
        $('#importDVP_dialog').dialog({
            title: 'Import DVP',
            width: 500,
            height: 200,
            buttons: [{
                text: 'save',
                iconCls: 'icon-ok',
                handler: function () {
                    upload_file();
                }
            }, {
                text: 'cancel',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importDVP_dialog').dialog('close');
                }
            }]

        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };

    function upload_file() {
        $.messager.progress({
            text: '正在上传中...'
        });
        $('#importDVP_dialog').form('submit', {
            url: "/ScheduleManagement/ImportMTRProject",//接收一般处理程序返回来的json数据    
            onSubmit: function (param) {
                param.MTRNO = selectRow.MTRNO;
                param.Prefixion = $("#Prefixion").combobox("getText");
                param.suffix = $("#Prefixion").combobox("getValue");
                return $(this).form('enableValidation').form('validate');
            },
            onBeforeLoad: function () {
                $.messager.progress({
                    text: '正在上传中...'
                });
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $.messager.progress('close');
                        $("#importDVP_dialog").dialog('close');//关闭导入DVP弹窗
                        //$("#Signature_photo").attr("src", result.Data.Signature);
                        $.messager.alert('Tips', result.Message);
                    }
                    else if (result.Success == false) {
                        $.messager.progress('close');
                        $.messager.alert('Tips', result.Message);
                    }
                }
            }
        });
    };
    $('#importDVP_dialog').form('reset');//重置表单
};
//*************添加MTR信息
function MTR_add() {

    $("#FollowUp").combobox({
        url: "/ScheduleManagement/GetUserList",
        valueField: 'UserId',
        textField: 'UserName',
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });

    $("#MTRNO").textbox({
        readonly: false
    });
    $('#Add_MTR_dialog').dialog({
        title: 'Add MTR',
        width: 1100,
        height: 450,
        buttons: [{
            text: 'save',
            iconCls: 'icon-ok',
            handler: function () {
                Add_save();
            }
        }, {
            text: 'Close',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#Add_MTR_dialog').dialog('close');//关闭弹窗
            }
        }]
    });
    function Add_save() {
        //表单提交
        $('#Add_MTR_dialog').form('submit', {
            url: "/ScheduleManagement/AddMTRRegister",//接收一般处理程序返回来的json数据
            onSubmit: function (param) {
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $.messager.alert('Tips', result.Message);
                        $('#Add_MTR_dialog').dialog('close');
                        $('#MTR_information_datagrid').datagrid('reload');
                    } else if (result.Success == false) {
                        $.messager.alert('Tips', result.Message);

                    }
                }
            }
        });
    };
    $('#Add_MTR_dialog').form('reset');//重置表单
};

//******************修改MTR信息
function MTR_edit() {
    $("#FollowUp").combobox({
        url: "/ScheduleManagement/GetUserList",
        valueField: 'UserId',
        textField: 'UserName',
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    $("#MTRNO").textbox({
        readonly: true
    });
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        line = $('#MTR_information_datagrid').datagrid("getRowIndex", selectRow);
        $('#Add_MTR_dialog').dialog({
            title: 'Edit MTR',
            width: 1100,
            height: 450,
            draggable: true,
            modal: true,
            buttons: [{
                text: 'save',
                iconCls: 'icon-save',
                handler: function () {
                    Edit_save();
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Add_MTR_dialog').dialog('close');
                }
            }]
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };
    $('#Add_MTR_dialog').form('load', selectRow);//数据回显
    $("#ReportModified").combobox("setValue", selectRow.ReportModified);//设置报告修改状态
    $("#CNASLogo").combobox("setValue", selectRow.CNASLogo);//设置CNAS标志; 
};
//****************确认修改MTRNO
function Edit_save() {//form表单提交
    $('#Add_MTR_dialog').form('submit', {
        url: "/ScheduleManagement/EditMTRRegister",//接收一般处理程序返回来的json数据 
        onSubmit: function (param) {
            return $(this).form('enableValidation').form('validate');
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#Add_MTR_dialog').dialog('close');
                    $.messager.alert('Tips', result.Message);
                    $('#MTR_information_datagrid').datagrid('reload');
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        }
    });
};
//****************删除MTR信息
function MTR_delete() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        $.messager.confirm('Tips', 'Are you sure you want to delete the selected content?', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: "/ScheduleManagement/DelMTRRegister",//接收一般处理程序返回来的json数据     
                    data: { MTRNO: selectRow.MTRNO },
                    type: "POST",
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('Tips', result.Message);
                            $('#MTR_information_datagrid').datagrid('reload');
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
};
//*****************查看DVP信息
function LookDVP() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        var name = '文件查看';                            //网页名称，可为空; 
        var iWidth = screen.width - 20;                          //弹出窗口的宽度; 
        var iHeight = screen.availHeight - 20;                         //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = 10;
        //获得窗口的水平位置 
        var iLeft = 10;
        window.open("/ScheduleManagement/DVPInformationManager?MTRNO=" + escape(selectRow.MTRNO) + "", name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }


    //$('#iframe_window').html("<iframe src='../业务管理/DVP信息.html'></iframe>");

    //$('#iframe_window').window({
    //    fit: true,
    //    title:"DVP信息",
    //    modal: true
    //});
    //$('#iframe_window iframe').css("width", "100%");
    //$('#iframe_window iframe').css("height", "100%");
    //$('#iframe_window iframe').css("overflow", "hidden");

};
//*****************************************************************************查看主项目*****************************************************************
function look_item() {
    //搜索信息的下拉框
    $('#search_').combobox({
        panelHeight: 120,
        data: [
                { 'value': 'ProjectName', 'text': 'ProjectName' }
        ]
    });
    var selected_report = $("#MTR_information_datagrid").datagrid("getSelected");
    if (selected_report) {
        $('#report_item').dialog({
            width: 800,
            height: 400,
            fit: true,
            modal: true,
            title: 'Test Item',
            border: false,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#report_item').dialog('close');
                }
            }]
        });
        //检测报告信息
        $('#report_Item_datagrid').datagrid(
           {
               nowrap: false,
               striped: true,
               ctrlSelect: true,
               // singleSelect: true,
               border: false,
               fitColumns: true,
               fit: true,
               pagination: true,
               pageSize: 10,
               pageList: [10, 20, 30, 40],
               queryParams: {
                   MTRNO: selected_report.MTRNO,
                   search: $("#search_").combobox("getValue"),
                   key: $("#key_").textbox("getText")
               },
               pageNumber: 1,
               type: 'POST',
               dataType: "json",
               url: "/ReportManagement/GetTestTaskList",//接收一般处理程序返回来的json数据        
               columns: [[
               // { filed: 'id', title: 'id', checkbox: true, width: 80 },
               { field: 'MTRNO', title: 'MTRNO', sortable: 'true' },
               // { field: 'ProjectId', title: 'ProjectId' },
               { field: 'TestId', title: 'TestId' },
               { field: 'TestItem', title: 'TestItem' },
               { field: 'MainNum', title: 'MainNum' },
               //  { field: 'MainDescription', title: 'MainDescription' },
               //  { field: 'MainDescriptionUrl', title: 'MainDescriptionUrl' },
                //{ field: 'Method', title: 'Method' },
               //  { field: 'MethodUrl', title: 'MethodUrl' },
                { field: 'MotoNum', title: 'MotoNum' },
                { field: 'ProjectName', title: 'ProjectName' },
                { field: 'SampleQty', title: 'SampleQty' },
                { field: 'SampleNo', title: 'SampleNo' },
                { field: 'Operator', title: 'Operator' },
                   {
                       field: 'TestTaskState', title: 'TestTaskState', sortable: true, formatter: function (value, row, index) {//计划开始时间
                           switch (row.TestTaskState) {
                               case 0: value = "Test"; break;
                               case 2: value = "Tested"; break;
                               case 3: value = "Temporary Report"; break;
                               case 4: value = "Edited Report"; break;
                           }
                           return value;
                       }
                   },
                {
                    field: 'TestStartDate', title: 'TestStartDate', formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                {
                    field: 'TestEndDate', title: 'TestEndDate', formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                //{
                //    field: 'PlanStartDate', title: 'PlanStartDate', formatter: function (value, row, index) {//计划开始时间
                //        if (value) {//格式化时间
                //            if (value.length >= 10) {
                //                value = value.substr(0, 10)
                //                return value;
                //            }
                //        }
                //    }
                //},
                //{
                //    field: 'PlanEndDate', title: 'PlanEndDate', formatter: function (value, row, index) {//计划开始时间
                //        if (value) {//格式化时间
                //            if (value.length >= 10) {
                //                value = value.substr(0, 10)
                //                return value;
                //            }
                //        }
                //    }
                //},
                { field: 'Temperature', title: 'Temperature' },
                { field: 'Humidity', title: 'Humidity' },
                { field: 'ReportName', title: 'ReportName' },
                { field: 'TestResource', title: 'TestResource' },
                {
                    field: 'MachineTime', title: 'MachineTime', formatter: function (value, row, index) {//计划开始时间
                        if (value) {//格式化时间
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                { field: 'NonLife', title: 'NonLife' },
                { field: 'Total', title: 'Total' },
                { field: 'Quotation', title: 'Quotation' },
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
                }

               ]],
               rowStyler: function (index, row) {
                   switch (row.TestTaskState) {
                       case 0: return 'color:red ;'; break;
                       case 2: return 'color:black;'; break;
                       case 3: return 'color:blue;'; break;
                       case 4: return 'color:#32cd32;'; break;
                   }
               },
               onLoadSuccess: function (data) {
                   //默认选择行
                   $('#report_Item_datagrid').datagrid('selectRow', line);
               },
               onSelect: function () {
                   //提示
                   //$('#report_Item_datagrid').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
                   //view_taskinfo1();
               },
               sortName: 'TestTaskState',
               sortOrder: 'asc',
               toolbar: "#reports_toolbar1"
           });
        //搜索主项目
        $('#report_Item_search_').unbind("click").bind("click", function () {
            report_item_search();
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated');
    }
    //导出子报告
    $('#Export_Chirdren_Report').unbind("click").bind("click", function () {
        Export_Chirdren_Report("false");
    });
    //导出所有报告
    $('#Export_All_Report').unbind("click").bind("click", function () {
        Export_Chirdren_Report("true");
    });
  
}
//
/*
*functionName:View_Group
*function:函数功能 导出子报告
*Param 参数flag  true 导出所有 false 导出选中
*author:创建人 张慧敏
*date:时间
*/
//
function Export_Chirdren_Report(flag) {
    var selected_task = $("#report_Item_datagrid").datagrid("getSelected");
    var selected_task_rows = $("#report_Item_datagrid").datagrid("getSelections");
    if (selected_task) {
        var TaskIdsArr = [];
        for (var i = 0; i < selected_task_rows.length; i++) {
            TaskIdsArr.push(selected_task_rows[i].TaskId);
        }
        var TaskIds = TaskIdsArr.join(",");
        $.ajax({
            url: "/ReportManagement/ExportSubReport",
            type: 'POST',
            data: {
                MTRNO: selected_task.MTRNO,
                TaskIds: TaskIds,
                flag: flag,
                type: $("input[name='type']:checked").val()
            },
            success: function (data) {
                var obj = $.parseJSON(data);
                if (obj.Success == true) {
                    //addTabs({ id: "Export_All_Report", icon: "menu-icon fa fa-glass", title: "工作消息", url: obj.Message, close: true });
                    // window.location = obj.Message;

                    //不使用window.open，避免被拦截
                    var a = $("<a href='" + obj.Message + "' target='_blank'>Report</a>").get(0);
                    var e = document.createEvent('MouseEvents');
                    e.initEvent('click', true, true);
                    a.dispatchEvent(e);
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
//*****************查看测试记录信息信息
function LookTestRecord() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        var name = '测试记录查看';                            //网页名称，可为空; 
        var iWidth = screen.width - 20;                          //弹出窗口的宽度; 
        var iHeight = screen.availHeight - 20;                         //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = 10;
        //获得窗口的水平位置 
        var iLeft = 10;
        window.open("/ScheduleManagement/TestRecordManager", name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }


    //$('#iframe_window').html("<iframe src='../业务管理/DVP信息.html'></iframe>");

    //$('#iframe_window').window({
    //    fit: true,
    //    title:"DVP信息",
    //    modal: true
    //});
    //$('#iframe_window iframe').css("width", "100%");
    //$('#iframe_window iframe').css("height", "100%");
    //$('#iframe_window iframe').css("overflow", "hidden");

};
/*
*functionName:ExportDVP
*function:导出DVP信息
*Param: MTRNO
*author:程媛
*date:2018-05-10
*/
function ExportDVP() {
    var getRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行的MTR所有数据信息
    if (getRow) {
        $.ajax({
            url: "/ScheduleManagement/DVP_Export",
            type: 'POST',
            data: {
                TaskId: getRow.TaskId,
                MTRNO: getRow.MTRNO
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    window.location.href = result.Message;

                } else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };
};
//*********************搜索mtr信息
function SearchMTR() {
    $('#MTR_information_datagrid').datagrid({
        queryParams: {
            search: $("#key").combobox("getValue"),
            key: $("#key1").textbox("getText")
        },
        url: "/ScheduleManagement/GetMTRMenagementList",
    })
};
//******************初始化测试项目
function Map_processes() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    var _height = $(".tab-content").height();
    var tabs_width = screen.width;
    $('#cc').layout('panel', 'west').panel('resize', {
        width: 400,
        // height: _height
    });
    $('#cc').layout('panel', 'center').panel('resize', {
        width: tabs_width - 400,
        // height: _height
    });
    //每次打开时先清空流程图
    document.getElementById("map_iframe").src = "/ScheduleManagement/Testflow?MTRNO=" + selectRow.MTRNO;
    //$("#map_iframe").contents().find("#inputresult").val('');//将获取的文本值写入导入的文本框

    $('#test_layout').layout('resize');
    if (selectRow) {
        line = $('#MTR_information_datagrid').datagrid("getRowIndex", selectRow);

        $('#map_dialog').dialog({
            title: 'Map processes',
            width: 750,
            height: 500,
            fit: true,
            // href: "ScheduleManagement/Testflow",
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#map_dialog').dialog('close');
                }
            }]
        });

        $("#map_iframe").contents().find("#clear").click();

        // Processes_list(selectRow.MTRNO);    //回显 MTR单样品流程图

    }
    $('#Laboratory_tree').tree({
        url: "/ScheduleManagement/LoadDVPParProjectTree",
        method: 'post',
        required: true,
        queryParams: {
            MTRNO: selectRow.MTRNO
        },
        onContextMenu: function (e, node) {
            e.preventDefault();
            $('#Laboratory_tree').tree('select', node.target);
            $('#keyMenu').menu('show', {
                left: e.pageX,
                top: e.pageY
                //onClick: function (item) {
                //    alert(item.name);
                //    remove();
                //}
            });
        },
        //onBeforeExpand: function (node, param) {
        //    $('#Laboratory_tree').tree('options').url = "/ScheduleManagement/LoadDVPProjectTree?ParentId=" + node.id;
        //},
    });

    //追加流程名
    $("#add_to").unbind("click").bind("click", function () {
        AddTo();
    });
    //查看样品流转
    $("#View_Sample").unbind("click").bind("click", function () {
        View_Sample();
    });


}
//*******************项目回显历程图
function Processes_list(no) {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    // document.getElementById('map_iframe').contentWindow.clear();

    if (selectRow) {
        $.ajax({
            url: "/ScheduleManagement/GetMTRProcessDesign",
            type: 'POST',
            dataType: 'json',
            data: {
                MTRNO: selectRow.MTRNO
            },
            success: function (result) {

                //var result = $.parseJSON(data);
                if (result.success == true) {

                    $("#map_iframe").contents().find("#inputresult").val('');
                    if (result.total == 1) {

                        var json_result = result.rows[0].ProcessContent;//获取json对象
                        //  $("#map_iframe")[0].window.InputData();
                        $("#map_iframe").contents().find("#inputresult").val(json_result);//将获取的文本值写入导入的文本框
                        $("#map_iframe").contents().find("#MTR_no").val(no);//将获取的文本值写入子页面的的文本框

                        // document.getElementById('map_iframe').contentWindow.InputData;//调用子页面方法
                        //  $("#map_iframe")[0].contentWindow().InputData();

                        $("#map_iframe").contents().find("#export_rode").click();


                    } else {
                        $("#map_iframe").contents().find("#clear").click();
                    }
                }
            }
        });
    }
}
//**********************追加项目名到历程图
function AddTo() {
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        // $("#map_iframe")[0].contentWindow.Submit(node.text);
        document.getElementById('map_iframe').contentWindow.Submit(node.text);//调用子页面方法
        //  $("#map_iframe").contents().find("#ele_name").val(node.text);//获取子页面文本
    } else {
        $.messager.alert('Tips', 'Please select the tree tobe operated！');
    }
}
//**********************查看样品流转
function View_Sample() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    var node = $('#Laboratory_tree').tree('getSelected');
    if (node) {
        $('#sample_dialog').dialog({
            title: 'View Sample process',
            width: 800,
            height: 500,
            // fit: true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#sample_dialog').dialog('close');
                }
            }]
        });
    }

    //MotoNum_Range 点击编号范围显示
    if (node.SampleNo == "") {
        $("#MotoNum_Range").html('(无范围值)');
    } else {
        $("#MotoNum_Range").html('(' + node.SampleNo + ')');
    }

    $('#MotoNum').textbox({
        prompt: '1#~2#'
    })
    $('#sample_diagrid').datagrid({
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
        queryParams: {
            TaskId: node.id,
            SampleNo: node.SampleNo,
            MTRNO: selectRow.MTRNO
        },
        onClickCell: onClickCell,
        url: "/ScheduleManagement/GetMotorList",
        columns: [[
       { field: 'MotoNum', title: 'SampleNo', width: 100 },
        {
            field: 'Sortnum', title: 'Sortnum', width: 100
        }
        ]],
        onLoadSuccess: function (data) {
            $('#sample_diagrid').datagrid('selectRow', line);

        },
        toolbar: "#sample_information_toolbar"
    });
    //添加点击编号排序
    $("#sample_add").unbind("click").bind('click', function () {
        var node = $('#Laboratory_tree').tree('getSelected');
        $.ajax({
            url: "/ScheduleManagement/EditMotorSortnum",
            type: 'POST',
            dataType: 'json',
            data: {
                TaskId: node.id,
                MotorList: $("#MotoNum").textbox("getText"),
                SortNum: $("#Sort").textbox("getText")
            },
            success: function (data) {
                //  var result = $.parseJSON(data);
                if (data.Success == true) {
                    $.messager.alert('tips', data.Message);
                    $('#sample_diagrid').datagrid("reload");
                } else {
                    $.messager.alert('tips', data.Message);
                }

            }
        });
    });
}
//**********************提交样品接收dialog
function Submit_Sample_Accetion() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        $.messager.confirm('Tips', 'Are you sure Submit?', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: "/ScheduleManagement/SubmitMTRRegister",
                    type: 'POST',
                    data: {
                        Id: selectRow.id,
                        MTRNO: selectRow.MTRNO
                    },
                    beforeSend: function () {
                        $.messager.progress({
                            text: '正在提交中...'
                        });
                    },
                    success: function (data) {
                        $.messager.progress('close');
                        if (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $.messager.alert('Tips', result.Message);
                                $('#MTR_information_datagrid').datagrid('reload');//重新加载列表
                            } else if (result.Success == false) {
                                $.messager.alert('Tips', result.Message);
                            }
                        }
                    }
                });
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };
};