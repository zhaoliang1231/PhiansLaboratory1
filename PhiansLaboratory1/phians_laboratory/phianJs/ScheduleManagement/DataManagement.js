var line = 0;
$(function () {
    InitMTRInformationDatagrid();
});
//*******************************************MTR信息列表********************************************************************************************************************************************************************************************************
//程媛 2018-10-25
function InitMTRInformationDatagrid() {
    //搜索信息的下拉框
    $('#key').combobox({
        panelHeight: 120,
        data: [
                { 'value': 'TM.MTRNO', 'text': 'MTRNO' },
                { 'value': 'MO.SampleNo', 'text': 'Sample No.' },
                { 'value': 'MO.SampleName', 'text': 'Sample Name' },
        ]
    });
    $('#MTRInformationDatagrid').datagrid({
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
        url: "/ScheduleManagement/GetDataMenagementList",
        queryParams: {
            search: $("#key").combobox("getValue"),
            key: $("#key1").textbox("getText")
        },
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
            $('#MTRInformationDatagrid').datagrid('selectRow', line);

        },
        toolbar: "#MTRInformationToolbar"
    });
    //修改MTR信息
    $("#MTREdit").unbind("click").bind("click", function () {
        MTREdit();
    });
    //搜索MTR信息
    $("#search").unbind("click").bind("click", function () {
        SearchMTR();
    });
};
//*********************搜索mtr信息
//程媛 2018-10-25
function SearchMTR() {
    $('#MTRInformationDatagrid').datagrid({
        queryParams: {
            search: $("#key").combobox("getValue"),
            key: $("#key1").textbox("getText")
        },
        url: "/ScheduleManagement/GetDataMenagementList "
    });
};
//*********************布局
//程媛 2018-10-25
function layout() {
    //var _height = $(".tab-content").height();
    var tabsWidth = screen.width;
    var westWidth = tabsWidth * 0.3;
    var eastWidth = tabsWidth - westWidth;

    //树部分的布局
    $('#Layout').layout('panel', 'west').panel('resize', {
        width: westWidth
    });
    //树部分的布局
    $('#Layout').layout('panel', 'east').panel('resize', {
        width: eastWidth
    });
    $('#Layout').layout('resize');//自适应
    $("#tt").tabs({ tabPosition: 'top' });
};
//*********************编辑mtr信息
//程媛 2018-10-25
function MTREdit() {
    var selectRow = $("#MTRInformationDatagrid").datagrid("getSelected");//获取选中行
    if (selectRow) {
        line = $('#MTRInformationDatagrid').datagrid("getRowIndex", selectRow);
        $('#EditMTRDialog').dialog({
            title: 'Edit MTR',
            modal: true,
            draggable: true,
            fit: true,
            buttons: [
                {
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#EditMTRDialog').dialog('close');
                    }
                }
            ]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    layout();
    File_type(selectRow);

};
//文件类别树渲染
function File_type(selectRow) {
    $('#tree').tree({
        url: "/ScheduleManagement/LoadDataMenagementTree",
        queryParams: { MTRNO: selectRow.MTRNO },
        method: 'post',
        required: true,
        border: true,
        //  fit: true,
        top: 0,
        onLoadSuccess: function (node, data) {
            //树菜单展开
            var t = $(this);
            if (data) {
                $(data).each(function (index, d) {
                    if (this.state == 'closed') {
                        t.tree('expandAll');
                    }
                })
            };
            //默认选中第一个节点
            if (data.length > 0) {
                //找到第一个元素
                var n = $('#tree').tree('find', data[0].id);
                //调用选中事件
                $('#tree').tree('select', n.target);
            };
        },
        onBeforeExpand: function (node, param) {
            $('#tree').tree('options').url = "/ScheduleManagement/LoadDataMenagementTree?&ParentID=" + node.ProjectId;
        },
        onSelect: function () {
            infoAddEcho();
            initTestEquipment();
        }
    });
};
//基本信息数据回显
function infoAddEcho() {
    console.log($('#tree').tree("getSelected"));
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
            var roomNum = $("#RoomNum").combobox("getValue");
            var testStartDate = $("#TestStartDate").datetimebox("getValue");
            if (testStartDate) {
                $.ajax({
                    url: "/MonitoringManagement/GetTemperatureFirstRecord",
                    type: 'POST',
                    data: {
                        RoomNum: roomNum,//获取选中行的RoomNum传给后台
                        StartDatetime: testStartDate
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
    var selectRow = $('#tree').tree("getSelected")//获取选中行
    $('#info_Add').form("load", selectRow);
    $('#RoomNum').combobox("setValue", selectRow.RoomID);

    //$('#Submit_Remark_dialog').form("load", selectRow);
    //编辑基本信息
    $("#information_save").unbind("click").bind("click", function () {
        information_save();
    });
}

/*
*functionName:information_save
*function:保存基本信息的修改
*Param
*author:程媛
*date:2018-10-26
*/
function information_save() {
    //e.preventDefult();
    var selectRow = $("#tree").tree("getSelected");//获取选中行
    var roomId = $("#RoomNum").combobox("getValue");//获取选中房间号
    if (selectRow) {
        //  $('#info_Add').form('load', selectRow);//数据回显
        $.ajax({
            url: "/ScheduleManagement/EditDataMenagementBaseInfo",//接收一般处理程序返回来的json数据     
            type: 'POST',
            data: {
                RoomID: roomId,
                TaskId: selectRow.id,
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
                    //$('#task_record').datagrid("reload");
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };

};

/*
*functionName:initTestEquipment
*function:查看检测设备数据
*Param
*author:程媛
*date:2018-10-26
*/
function initTestEquipment() {
    var selectRow = $("#tree").tree("getSelected");//获取选中任务行
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
                TaskId: selectRow.id
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

