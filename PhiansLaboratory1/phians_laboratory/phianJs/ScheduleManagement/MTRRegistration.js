var line = 0;
var ArrList = [];//定义一个空的数组
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

});

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
        singleSelect: true,
        fit: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        remoteSort: false,
        url: "/ScheduleManagement/GetMTRRegisterList",
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
    //导入DVP信息
    $("#ImportDVP").unbind("click").bind("click", function () {
        ImportDVP();
    });
    //导出DVP信息
    $("#ExportDVP").unbind("click").bind("click", function () {
        ExportDVP();
    });
    //拷贝MTR信息
    $("#copyMTR").unbind("click").bind("click", function () {
        copyMTR();
    });
    //绘制流程图
    $("#Map_processes").unbind("click").bind("click", function () {
        Map_processes();
    });
    //搜索mtr信息
    $("#search").unbind("click").bind("click", function () {
        SearchMTR();
    });
    //提交样品接收
    $("#Submit_Sample_Accetion").unbind("click").bind("click", function () {
        Submit_Sample_Accetion();
    });
    //提交样品接收
    $("#ScheduleProject").unbind("click").bind("click", function () {
        ScheduleProjectform();
    });

};
function Mot() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    return selectRow.MTRNO;
}
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
        modal: true,
        draggable: true,
        buttons: [
            {
                text: 'save',
                iconCls: 'icon-ok',
                handler: function () {
                    Add_save();
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Add_MTR_dialog').dialog('close'); //关闭弹窗
                }
            }
        ]
    });
    $('#Add_MTR_dialog').form('reset');//重置表单
};
//确认添加MTR表单
function Add_save() {
    var mtrno = $("#MTRNO").textbox('getText');
    if (mtrno.indexOf('\\') >= 0 || mtrno.indexOf('/') >= 0) {
        $.messager.alert("Tips", "MTR No中存在非法字符'/'或者'\\'！");
    } else {
        //mtrno = mtrno.replace(/[\\\/g]/, "");
        $('#Add_MTR_dialog').form('submit', {
            url: "/ScheduleManagement/AddMTRRegister", //接收一般处理程序返回来的json数据
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
            modal: true,
            draggable: true,
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
        window.open("/ScheduleManagement/DVPInformation?MTRNO=" + escape(selectRow.MTRNO) + "", name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
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
//****************导入DVP信息
function ImportDVP() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        line = $('#MTR_information_datagrid').datagrid("getRowIndex", selectRow);
        //前缀
        $('#Prefixion').combobox({
            url: "/Common/GetDictionaryList_id",
            valueField: 'Value',
            textField: 'Text',
            onBeforeLoad: function (param) {
                param.Parent_id = 'a620b59f-4659-4022-8b46-543108332070';

            },
            onSelect: function () {
                $.ajax({
                    url: "/Common/GetDicitionaryContent",
                    type: 'POST',
                    // dataType: 'json',
                    data: {
                        id: $("#Prefixion").combobox("getValue")
                    },
                    success: function (result) {
                        var result1 = $.parseJSON(result);


                        if (result1.Success == true) {
                            // alert(result1.Data.Project_value);
                            $("#Prefixion2").textbox("setText", result1.Data.Project_name);//前缀
                            $("#suffix").textbox("setText", result1.Data.Project_value);//后缀

                        } else {
                            $.messager.alert('Tips', result.Message);
                        }
                    }
                });
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
            modal: true,
            draggable: true,
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
                param.Prefixion = $("#Prefixion2").textbox("getText");
                param.suffix = $("#suffix").textbox("getText");
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
//*********************拷贝MTR
function copyMTR() {
    var selectRowMtr = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRowMtr) {
        $('#copyMTR_dialog').dialog({
            title: 'Copy MTR',
            width: 1250,
            height: 750,
            modal: true,
            draggable: true,
            // fit: true,
            buttons: [
                {
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#copyMTR_dialog').dialog('close');
                    }
                }
            ]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    $('#copyMTR_diagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: true,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        pageNumber: 0,
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
            $('#copyMTR_diagrid').datagrid('selectRow', 0);
        },
        sortName: '',
        sortOrder: 'asc',
        toolbar: "#copyMTR_diagrid_toolbar"
    });
    $("#chooseType").combobox({
        value: '1',
        data: [
            { 'value': '0', 'text': '选择拷贝' },
            { 'value': '1', 'text': '全部拷贝' }
        ]
    });
    $("#keyMTR").combobox({
        value: 'TM.MTRNO',
        data: [
            { 'value': 'TM.MTRNO', 'text': 'MTRNO' },
            { 'value': 'MO.SampleNo', 'text': 'Sample No.' },
            { 'value': 'MO.SampleName', 'text': 'Sample Name' }
        ]
    });
    //搜索MTR信息
    $("#searchMTR").unbind("click").bind("click", function () {
        searchAllMTR();
    });
    $("#copy").unbind("click").bind("click", function () {
        if ($("#copyMTR_diagrid").datagrid("getSelected")) {
            if ($("#chooseType").combobox("getValue") === '1') {
                copyAll();
            } else if ($("#chooseType").combobox("getValue") === '0') {
                copyChoose();
            }
        } else {
            $.messager.alert('Tips', 'Please select the row to be operated！');
        }
    });
    $("#ViewDVP").unbind("click").bind("click", function () {
        viewDVP();
    });
};
function viewDVP() {
    var selectRow = $("#copyMTR_diagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        var name = '文件查看';                            //网页名称，可为空; 
        var iWidth = screen.width - 20;                          //弹出窗口的宽度; 
        var iHeight = screen.availHeight - 20;                         //弹出窗口的高度; 
        //获得窗口的垂直位置 
        var iTop = 10;
        //获得窗口的水平位置 
        var iLeft = 10;
        window.open("/ScheduleManagement/ViewDVP?MTRNO=" + escape(selectRow.MTRNO) + "", name, 'height=' + iHeight + ',,innerHeight=' + iHeight + ',width=' + iWidth + ',innerWidth=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',status=no,toolbar=no,menubar=no,location=no,resizable=no,scrollbars=0,titlebar=no');
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }

}
//搜索MTR信息
function searchAllMTR() {
    $('#copyMTR_diagrid').datagrid({
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        queryParams: {
            search: $("#keyMTR").combobox("getValue"),
            key: $("#keyMTR1").textbox("getText")
        },
        url: "/ScheduleManagement/GetAllMTRList"
    });

};
//拷贝全部
function copyAll() {
    var selectRow = $("#copyMTR_diagrid").datagrid("getSelected");//获取选中行
    var selectRowMtr = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        $.ajax({
            url: "/ScheduleManagement/CopyMTRProject",
            type: 'POST',
            dataType: 'json',
            data: {
                MTRNO: selectRowMtr.MTRNO,
                NewMTRNO: selectRow.MTRNO
            },
            success: function (result) {
                if (result.Success == true) {
                    $.messager.alert('Tips', result.Message);
                    $("#copyMTR_dialog").dialog('close');
                } else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    } else {
        $.messager.alert('Tips', "Please select the location！");
    };
};
//选择拷贝
function copyChoose() {
    var selectRowMtr = $("#copyMTR_diagrid").datagrid("getSelected");//获取选中行
    if (selectRowMtr) {
        $('#chooseMTR_dialog').dialog({
            title: 'Choose MTR',
            width: 1200,
            height: 680,
            modal: true,
            draggable: true,
            buttons: [
                {
                    text: 'Save',
                    iconCls: 'icon-ok',
                    handler: function () {
                        openImportDVP();
                    }
                }, {
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#chooseMTR_dialog').dialog('close');
                    }
                }]
        });
        choosemtr_authorize_Add();
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
function openImportDVP() {

    $('#importDVP_dialog1').dialog({
        title: 'Choose',
        width: 500,
        height: 200,
        modal: true,
        draggable: true,
        buttons: [{
            text: 'Submit',
            iconCls: 'icon-ok',
            handler: function () {
                saveChooseMTR();
            }
        }, {
            text: 'cancel',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#importDVP_dialog1').dialog('close');
            }
        }]

    });
    //前缀
    $('#Prefixion1').combobox({
        url: "/Common/GetDictionaryList_id",
        valueField: 'Value',
        textField: 'Text',
        onBeforeLoad: function (param) {
            param.Parent_id = 'a620b59f-4659-4022-8b46-543108332070';

        },
        onSelect: function () {
            $.ajax({
                url: "/Common/GetDicitionaryContent",
                type: 'POST',
                // dataType: 'json',
                data: {
                    id: $("#Prefixion1").combobox("getValue")
                },
                success: function (result) {
                    var result1 = $.parseJSON(result);
                    if (result1.Success == true) {
                        // alert(result1.Data.Project_value);
                        $("#Prefixion3").textbox("setText", result1.Data.Project_value);
                        $("#suffix1").textbox("setText", result1.Data.Project_name);

                    } else {
                        $.messager.alert('Tips', result.Message);
                    }
                }
            });
        },
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    $('#importDVP_dialog1').form('reset');//重置表单
}
function choosemtr_authorize_Add() {
    var selectRowMtr = $("#copyMTR_diagrid").datagrid("getSelected");//获取选中行
    $("#Consumables_search1").combobox({
        data: [
                 { 'value': 'PONum', 'text': 'PONum' },
                 { 'value': 'ConsumablesName', 'text': 'ConsumablesName' }
        ]
    });
    $('#choosemtr_diagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        title: "Project Info",
        singleSelect: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        queryParams: { MTRNO: selectRowMtr.MTRNO },
        url: '/ScheduleManagement/GetMTRMainTaskList',
        dataType: "json",
        columns: [[
           { field: 'TaskId', title: 'Task Id', hidden: true },
           { field: 'ParentID', title: 'Parent ID', hidden: true },
           { field: 'MTRNO', title: 'MTRNO' },
           { field: 'ProjectNo', title: 'ProjectNo' },
           { field: 'ProjectName', title: 'ProjectName' },
            { field: 'ProjectId', title: 'ProjectId', hidden: true },
           //{
           //    field: 'MainDescription', title: 'MainDescription', width: 300, hidden: true, formatter: function (value, row, index) {
           //        return '<span  title=' + value + '>' + value + '</span>';
           //    }
           //},
           //{
           //    field: 'SubtDescription', title: 'SubtDescription', width: 300, hidden: true, formatter: function (value, row, index) {
           //        return '<span  title=' + value + '>' + value + '</span>';
           //    }
           //},
           { field: 'SampleName', title: 'SampleName' },
            { field: 'SampleNo', title: 'Sample No.' },//样品编号
           { field: 'SampleQty', title: 'SampleQty' },
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
           { field: 'Temperature', title: 'Temperature' },
           { field: 'Humidity', title: 'Humidity' },
           { field: 'MachineTime', title: 'MachineTime(h)' },
           { field: 'Total', title: 'Total(h)' },
           { field: 'Isparent', title: 'Isparent' },
           { field: 'TestResult', title: 'TestResult' },
           { field: 'activation', title: 'activation' },
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
        onDblClickRow: function (index, row) {
            $('#choosemtr_diagrid').click();
        },
        onLoadSuccess: function (data) {
            $('#choosemtr_diagrid').datagrid('selectRow', 0);

        },
        onSelect: function (index, row) {
            $("#SampleNoInfo").textbox('setValue', row.SampleNo);
            $("#SampleQtyInfo").textbox('setValue', row.SampleQty);
        }
    });
    ////定义pagination加载内容
    //var p_111 = $('#choosemtr_diagrid').datagrid('getPager');
    //(p_111).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    //
    $('#choosemtr_authorize').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        // rownumbers: true,
        title: "Authorize Project Info",
        singleSelect: true,
        //autoRowHeight: true,
        // fit: true,
        fitColumns: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
        },
        columns: [[
           { field: 'TaskId', title: 'Task Id', hidden: true },
           { field: 'ParentID', title: 'Parent ID', hidden: true },
           { field: 'MTRNO', title: 'MTRNO' },
           { field: 'ProjectNo', title: 'ProjectNo' },
           { field: 'ProjectName', title: 'ProjectName' },
            { field: 'ProjectId', title: 'ProjectId', hidden: true },
           //{
           //    field: 'MainDescription', title: 'MainDescription', width: 300, hidden: true, formatter: function (value, row, index) {
           //        return '<span  title=' + value + '>' + value + '</span>';
           //    }
           //},
           //{
           //    field: 'SubtDescription', title: 'SubtDescription', width: 300, hidden: true, formatter: function (value, row, index) {
           //        return '<span  title=' + value + '>' + value + '</span>';
           //    }
           //},
           { field: 'SampleName', title: 'SampleName' },
            { field: 'SampleNo', title: 'Sample No.' },//样品编号
           { field: 'SampleQty', title: 'SampleQty' },
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
                field: 'TestEndDate', title: 'TestEndDate', formatter: function (value, row, index) { //计划开始时间
                    if (value) { //格式化时间
                        if (value == "1900-01-01 0:00:00") {
                            return "";
                        } else if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            }
        ]],
        toolbar: "#choosemtr_authorize_toolbar"
    });
    ////定义pagination加载内容
    //var p_211 = $('#choosemtr_authorize').datagrid('getPager');
    //(p_211).pagination({
    //    layout: ['first', 'prev', 'last', 'next']

    //});
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#choosemtr_authorize').datagrid("loadData", json);
    //添加已授权MTR
    $('#addChooseMtrDatagrid').unbind("click").bind("click", function () {
        addChooseMtrDatagrid();
    });
    //删除已授权MTR
    $('#removeChooseMtrDatagrid').unbind("click").bind("click", function () {
        removeChooseMtrDatagrid();
    });
    //修改已授权MTR
    $('#editChooseMtrDatagrid').unbind("click").bind("click", function () {
        editChooseMtrDatagrid();
    });
};
//addMTR项目
function addChooseMtrDatagrid() {
    var selectRowChooseMtrDatagrid = $('#choosemtr_diagrid').datagrid('getSelected');
    console.log(selectRowChooseMtrDatagrid);
    if (selectRowChooseMtrDatagrid) {
        var type = $.inArray(selectRowChooseMtrDatagrid, ArrList);
        ArrList.push(selectRowChooseMtrDatagrid);
        var sampleNo = $("#SampleNoInfo").textbox("getText");
        var sampleQty = $("#SampleQtyInfo").textbox("getText");
        if (type == -1) {
            debugger;
            $('#choosemtr_authorize').datagrid('appendRow', {
                TaskId: selectRowChooseMtrDatagrid.TaskId,
                MTRNO: selectRowChooseMtrDatagrid.MTRNO,
                ProjectNo: selectRowChooseMtrDatagrid.ProjectNo,
                ProjectName: selectRowChooseMtrDatagrid.ProjectName,
                SampleName: selectRowChooseMtrDatagrid.SampleName,
                SampleNo:sampleNo,
                SampleQty:sampleQty
            });
        }
        $("#choosemtr_authorize").datagrid("selectRow", 0);
    } else {
        $.messager.alert('Tip', "Please select the row to be operated！");
    };
   // saveEdit1(selectRowChooseMtrDatagrid)
};
//保存修改已经选择MTR项目
function saveEdit1(selectRowChooseMtrDatagrid) {
    var sampleNo = $("#SampleNoInfo").textbox("getText");
    var sampleQty = $("#SampleQtyInfo").textbox("getText");
    $('#choosemtr_authorize').datagrid('updateRow', {
        index: $("#choosemtr_authorize").datagrid("getRowIndex"),
        row: {
            MTRNO: selectRowChooseMtrDatagrid.MTRNO,
            ProjectNo: selectRowChooseMtrDatagrid.ProjectNo,
            ProjectName: selectRowChooseMtrDatagrid.ProjectName,
            SampleName: selectRowChooseMtrDatagrid.SampleName,
            SampleQty: sampleQty,
            SampleNo: sampleNo
        }
    });
};
//删除已经选择MTR项目
function removeChooseMtrDatagrid() {
    var selectRow = $("#choosemtr_authorize").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('Tips', 'Are you sure you want to delete the line？', function (r) {
            if (r) {
                var rowIndex = $('#choosemtr_authorize').datagrid('getRowIndex', selectRow);
                $('#choosemtr_authorize').datagrid('deleteRow', rowIndex);
                ArrList.splice(rowIndex);
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
//修改已授权MTR
function editChooseMtrDatagrid() {
    var selected = $("#choosemtr_authorize").datagrid("getSelected");//获取选中行
    if (selected) {
        $("#edit_dialog").dialog({
            width: 500,
            height: 300,
            modal: true,
            title: 'Edit1',
            draggable: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                handler: function () {
                    saveEdit(selected);
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#edit_dialog').dialog('close');//关闭弹窗
                }
            }]
        });
        $('#Sample_dialog').form('load', selected);//数据回显
        $("#SampleNo1").textbox("setValue", selected.SampleNo);
        $("#SampleQty1").textbox("setValue", selected.SampleQty);
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
//保存修改MTR信息
function saveEdit(selected) {
    var sampleNo = $("#SampleNo1").textbox("getText");
    var sampleQty = $("#SampleQty1").textbox("getText");
    debugger;
    $('#choosemtr_authorize').datagrid('updateRow', {
        index: $("#choosemtr_authorize").datagrid("getRowIndex", selected),
        row: {
            MTRNO: selected.MTRNO,
            ProjectNo: selected.ProjectNo,
            ProjectName: selected.ProjectName,
            SampleName: selected.SampleName,
            SampleQty: sampleQty,
            SampleNo: sampleNo
        }
    });
    $.messager.alert('Tip', "Operate Success!");
    $('#SampleNo1').textbox('setText', '');//置空
    $('#SampleQty1').textbox('setText', '');//置空
    $('#edit_dialog').dialog('close');//关闭弹窗
};
//确认选择拷贝的项目
function saveChooseMTR() {
    var selectRowMtr = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    var rowss = $("#choosemtr_authorize").datagrid("getData");
    var selectNewRowMtr = $("#choosemtr_authorize").datagrid("getSelected");
    if (rowss.total != 0) {
        $.ajax({
            url: '/ScheduleManagement/ChooseCopyMTRProject',
            type: 'POST',
            dataType: "json",
            data: {
                MTRNO: selectRowMtr.MTRNO,
                NewMTRNO: selectNewRowMtr.MTRNO,
                Prefixion: $("#Prefixion3").textbox("getText"),
                suffix: $("#suffix1").textbox("getText"),
                dataGridData: JSON.stringify(rowss)
            },
            success: function (data) {
                if (data.Success == true) {
                    var json = {
                        "rows": [],
                        "total": 0,
                        "success": true
                    };
                    $.messager.alert('Tip', data.Message);
                    $('#choosemtr_authorize').datagrid("loadData", json);
                    $('#chooseMTR_dialog').dialog("close");
                    ArrList = [];
                } else if (data.Success == false) {
                    $.messager.alert('Tip', data.Message);
                }
            }
        });
    } else {
        $.messager.alert('Tip', "The has not data to sumbit！");
    }
};

//*********************搜索mtr信息
function SearchMTR() {
    $('#MTR_information_datagrid').datagrid({
        queryParams: {
            search: $("#key").combobox("getValue"),
            key: $("#key1").textbox("getText")
        },
        url: "/ScheduleManagement/GetMTRRegisterList",
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
        success: function (data) {

        }
        //onBeforeExpand: function (node, param) {
        //    $('#Laboratory_tree').tree('options').url = "/ScheduleManagement/LoadDVPProjectTree?ParentId=" + node.id;
        //},
    });

    //追加流程名
    $("#add_to").unbind("click").bind("click", function () {
        AddTo();
    });
    //追加所有流程名
    $("#add_Append").unbind("click").bind("click", function () {
        addAppend();
    });
    //查看样品流转
    $("#View_Sample").unbind("click").bind("click", function () {
        View_Sample();
    });


}
//*******************追加所有流程名
function addAppend() {
    document.getElementById('map_iframe').contentWindow.InputDataAll();//调用子页面方法
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
        document.getElementById('map_iframe').contentWindow.Submit(node.text, node.id);//调用子页面方法
        //  $("#map_iframe").contents().find("#ele_name").val(node.text);//获取子页面文本
    } else {
        $.messager.alert('Tips', 'Please select the tree tobe operated！');
    }
}
//*******************保存项目流程图
function Save_process() {
    var selectRow = $("#MTR_information_datagrid").datagrid("getSelected");//获取选中行
    document.getElementById('map_iframe').contentWindow.Export();//调用子页面方法
    var textValue = $("#map_iframe").contents().find("#result").val();//获取子页面导出页面json对象
    document.getElementById('map_iframe').contentWindow.Export2();//调用子页面方法
    var textValue2 = $("#map_iframe").contents().find("#result2").val();//获取子页面导出页面json对象
    $.ajax({
        url: "/ScheduleManagement/SaveMTRProcessDesign",
        type: 'POST',
        dataType: 'json',
        data: {
            MTRNO: selectRow.MTRNO,
            nodeinfo: textValue,
            nodeinfoID: textValue2
        },
        success: function (data) {
            //  var result = $.parseJSON(data);
            if (data.Success == true) {
                $.messager.alert('tips', data.Message);
            } else {
                $.messager.alert('tips', data.Message);
            }

        }
    });
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
var editIndex = undefined;
var line1 = 0;
//存取编辑行的field
var field_str;
//性能参数编辑 field编辑的那个字段
function endEditing() {
    var node_on1 = $('#Laboratory_tree').tree('getSelected');//项目树
    if (editIndex == undefined) { return true }
    if ($('#sample_diagrid').datagrid('validateRow', editIndex)) {
        //var row = $('#sample_diagrid').datagrid("getSelections");
        //var row1 = $("#sample_diagrid").datagrid("getSelected");
        ////是否自动判断
        //var AutoFlag = row1.AutoFlag;
        ////判断值得范围
        //var RangeFlag_ = row1.RangeFlag;
        ////获取参照A 和B的值
        //var RequiredValue1_ = row1.RequiredValue1;
        //var RequiredValue2_ = row1.RequiredValue2;
        //$.ajax({
        //  //  url: "/ScheduleManagement/EditProjParameterValue",
        //    type: 'POST',
        //    data: {
        //        // flag:field,//编辑的是哪个字段
        //        id: row1.id,
        //        Value: value,
        //        TaskId: node_on1.id,//项目id
        //        Parameter: row1.Parameter,
        //        ProjectName: node_on1.text,//项目名称
        //        RangeFlag: row1.RangeFlag,
        //        RequiredValue1: row1.RequiredValue1,
        //        RequiredValue2: row1.RequiredValue2,
        //        AutoFlag: row1.AutoFlag,
        //        remarks: row1.remarks
        //    },
        //    success: function (data) {
        //        var result = $.parseJSON(data);
        //        if (result.Success == true) {
        //            //刷新权限
        //            $('#sample_diagrid').datagrid('reload');
        //        } else {
        //            $.messager.alert('tips', result.Message);
        //        }
        //    }
        //});
        $('#sample_diagrid').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
//点击编辑行的时间 index行号  field字段
function onClickCell(index, field) {
    line2 = index;
    if (endEditing(field)) {
        $('#sample_diagrid').datagrid('selectRow', index)
                .datagrid('editCell', { index: index, field: field });

        line1 = index;
        editIndex = index;
    }

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

/*
*functionName:ScheduleProjectform
*function：打开窗口手动排程
*Param: 
*author:黄小文
*date:2018-09-14
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