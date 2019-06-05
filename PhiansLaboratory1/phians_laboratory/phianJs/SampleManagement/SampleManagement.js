var line = 0;//初始化行
////******************************************************点击事件以及样品管理列表初始化**********************************//
$(function () {
    sample_management_datagrid();//样品管理列表初始化
    //搜索样品管理列表
    $('#Search').unbind("click").bind("click", function () {
        Searc_sample();
    });
    //修改样品
    $('#editSample').unbind("click").bind("click", function () {
        editSample();
    });
    //查看样品记录
    $('#view_record').unbind("click").bind("click", function () {
        view_record();
    });
    //查看样品信息
    $('#read_sample').unbind("click").bind("click", function () {
        read_sample();
    });
    //查看电机信息
    $('#Motor').unbind("click").bind("click", function () {
        Motor_info();
    });
    //作废电机信息
    $('#Scrap').unbind("click").bind("click", function () {
        Scrap_motor();
    });
    //选择打印电机二维码
    $('#print').unbind("click").bind("click", function () {
        printQrcode();
    });
    //导出流程卡信息
    $("#export_Sample").unbind("click").bind("click", function () {
        export_Sample();
    });
});
/*
*functionName:editSample
*function:修改样品
*author:张慧敏
*date:2018-10-15
*/
function editSample() {
    
}

//**********************************************打印选择电机信息条码
/*
*functionName:printQrcode
*function:打印选择电机信息条码
*Param MTRNO，MotoNum
*author:程媛
*date:2018-04-23
*/
function printQrcode() {
    var selectRow = $('#Motor_datagrid').datagrid('getSelections');
    for (var i = 0; i < selectRow.length; i++) {
        LODOP = getLodop();
        LODOP.ADD_PRINT_BARCODE(2, 8, 46, 46, "QRCode", selectRow[i].MotoNum);
        LODOP.ADD_PRINT_TEXT(19, 60, 70, 30, "JE Lab");
        LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
        LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
        LODOP.ADD_PRINT_TEXT(45, 8, 80, 20, selectRow[i].MotoNum);
        LODOP.PRINT();
    }
};
//******************************************************样品管理列表******************************************************//
function sample_management_datagrid() {
    $('#sample_management_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        fit: true,
        fitColumns: true,
        rownumbers: true,
        remoteSort: false,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/SampleManagement/GetSampleList",
        columns: [[
            { field: 'MTRNO', title: 'MTR NO.', sortable: 'true' },//订单号
            { field: 'SamplePosition', title: 'Address', width: 100 },//样板位置
            {
                field: 'TestLoad', title: 'Test Load', formatter: function (value, row, index) {//测试负载
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            { field: 'FollowUp_n', title: 'Follow Person' },//跟进人
            { field: 'SampleName', title: 'SampleName' },//样品名称
            { field: 'SampleQty', title: 'Sample Qty' },//样品数量
            { field: 'SampleDetails', title: 'Sample Details' },//样品详情
            { field: 'SampleDescription', title: 'Sample Description' },//样品描述
            {
                field: 'Identification', title: 'Identification', formatter: function (value, row, index) {//样品标识
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            {
                field: 'Appearance', title: 'Appearance', formatter: function (value, row, index) {//样品外观
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            {
                field: 'OthersStatus', title: 'OthersStatus', formatter: function (value, row, index) {//其他状态
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            {
                field: 'ReceivingDate', title: 'Receiving Date', formatter: function (value, row, index) {//接收日期
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            { field: 'Reviewer_n', title: 'Reviewer', hidden: true },//样品接收人
            { field: 'ReceivingSignature_n', title: 'Receiving Signature' },//接收人签名
              {
                  field: 'SampleStatus', title: 'Sample Status', formatter: function (value, row, index) {//状态
                      if (value == 0) {
                          return "待接收";
                      } else if (value == 1) {
                          return "已接收";
                      } else if (value == 2) {
                          return "已领取";
                      }
                  }
              },//接收人签名

            {
                field: 'TakeOutDate', title: 'TakeOut Date', formatter: function (value, row, index) {//取出日期

                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            { field: 'TakeOutSignature_n', title: 'TakeOut Signature' },//取出签名
            {
                field: 'Retrieval', title: 'Retrieval（Y/N）', formatter: function (value, row, index) {//是否取回
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            { field: 'Retriever_n', title: 'Retriever' },//取回人
            { field: 'ContactMethod', title: 'ContactMethod' },//联系方式
            {
                field: 'RetrievalDate', title: 'Retrieval Date', formatter: function (value, row, index) {//取回日期
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            {
                field: 'Status', title: 'Status', formatter: function (value, row, index) {//状态
                    if (value == 0) {
                        return "在用";
                    } else if (value == 1) {
                        return "停用";
                    } else if (value == 2) {
                        return "作废";
                    }
                }
            },
            {
                field: 'Remarks', title: 'Remarks', formatter: function (value, row, index) {
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
            $('#sample_management_datagrid').datagrid('selectRow', line);
        },
        toolbar: sample_management_toolbar
    });
};
//******************************************************搜索样品管理列表**************************************************//
function Searc_sample() {
    var key = $('#key').textbox('getText');//获取搜索文本框的值
    $('#sample_management_datagrid').datagrid({
        type: 'POST',
        dataType: "json",
        url: "/SampleManagement/GetSampleList",
        queryParams: {
            key: key//获取搜索文本框的值传给后台
        }
    });
};

//**********************************查看样品记录
function view_record() {
    var selectRow = $('#sample_management_datagrid').datagrid('getSelected');//获取选中行的样品信息
    if (selectRow) {
        $('#record_dialog').dialog({
            width: 800,
            height: 500,
            modal: true,
            title: 'Sample Record',
            draggable: true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#record_dialog').dialog('close');//关闭电机信息dialog
                }
            }]
        });
    } else {
        $.messager.alert('tip', 'Please select the line to operate！');
    };
    sample_record_datagrid(selectRow); //电机记录列表初始化
    //搜索Motor信息
    $("#Search_record").unbind("click").bind("click", function () {
        Search_record(selectRow);
    });
    //搜索下拉框的信息
    $("#key1").combobox({
        data: [
            { 'value': 'SampleCode', 'text': 'SampleCode' },
            { 'value': 'MotoNum', 'text': 'MotoNum' }
        ]
    });
};
function sample_record_datagrid(selectRow) {
    $('#sample_record_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        fit: true,
        fitColumns: true,
        rownumbers: true,
        remoteSort: false,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: { MTRNO: selectRow.MTRNO }, //传MTRNO给后台
        url: "/SampleManagement/GetSampleRecordList",
        columns: [
            [
                { field: 'MTRNO', title: 'MTR NO.', sortable: 'true' }, //订单号
                { field: 'SampleCode', title: 'SampleCode' }, //样品编号
                { field: 'MotoNum', title: 'MotoNum' }, //电机编号
                { field: 'SampleQty', title: 'Sample Qty' }, //样品数量
                //{
                //    field: 'Identification',
                //    title: 'Identification',
                //    formatter: function (value, row, index) { //样品标识
                //        if (value == true) {
                //            return "Yes";
                //        } else if (value == false) {
                //            return "No";
                //        }
                //    }
                //},
                //{
                //    field: 'Appearance',
                //    title: 'Appearance',
                //    formatter: function (value, row, index) { //样品外观
                //        if (value == true) {
                //            return "Yes";
                //        } else if (value == false) {
                //            return "No";
                //        }
                //    }
                //},
                //{
                //    field: 'OthersStatus',
                //    title: 'OthersStatus',
                //    formatter: function (value, row, index) { //其他状态
                //        if (value == true) {
                //            return "Yes";
                //        } else if (value == false) {
                //            return "No";
                //        }
                //    }
                //},
                { field: 'OutSignaTureName', title: 'OutSignaTure' },
                {
                    field: 'TakeOutDate',
                    title: 'TakeOut Date',
                    formatter: function (value, row, index) { //取出日期

                        if (value) {
                            if (value.length >= 10) {
                                value = value.substr(0, 10);
                                return value;
                            }
                        }
                    }
                },
                { field: 'ReturnSignaTureName', title: 'ReturnSignaTure' }, //取回人
                {
                    field: 'ReturnDate',
                    title: 'Retrieval Date',
                    formatter: function (value, row, index) { //取回日期
                        if (value) {
                            if (value.length >= 10) {
                                value = value.substr(0, 10);
                                return value;
                            }
                        }
                    }
                },
                {
                    field: 'Status',
                    title: 'Status',
                    formatter: function (value, row, index) { //状态
                        if (value == 0) {
                            return "初始";
                        } else if (value == 1) {
                            return "在库";
                        } else if (value == 2) {
                            return "出库";
                        } else if (value == 3) {
                            return "失效";
                        }
                    }
                },
                {
                    field: 'Remark',
                    title: 'Remark',
                    formatter: function (value, row, index) {
                        if (value) {
                            if (value.length > 30) {
                                var result = value.replace(" ", ""); //去空
                                var value1 = result.substr(0, 30);
                                return '<span  title=' + value + '>' + value1 + "......" + '</span>';
                            } else {
                                return '<span  title=' + value + '>' + value + '</span>';
                            }
                        }
                    }
                },
                {
                    field: 'RecordSate',
                    title: 'RecordSate',
                    formatter: function (value, row, index) { //状态
                        if (value == 0) {
                            return "借出";
                        } else if (value == 1) {
                            return "归还";
                        };
                    }
                },
            ]
        ],
        onLoadSuccess: function (data) {
            $('#sample_record_datagrid').datagrid('selectRow', line);
        },
        toolbar: sample_record_toolbar
    });
};
//******************************************************搜索样品记录列表**************************************************//
function Search_record() {
    var key = $('#key2').textbox('getText');//获取搜索文本框的值
    var search = $("#key1").combobox("getValue");
    $('#sample_record_datagrid').datagrid({
        type: 'POST',
        dataType: "json",
        url: "/SampleManagement/GetSampleRecordList",
        queryParams: {
            search: search,//获取搜索文本框的值传给后台
            key: key//获取搜索文本框的值传给后台
        }
    });
};
//*********************************修改样品信息
function editSample() {
    //下拉框内容
    $("#Retrieval").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Yes' },
           { 'value': false, 'text': 'No' }
        ]
    });
    $("#TestLoad").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Attach Load' },
           { 'value': false, 'text': 'No Attach Load' }
        ]
    });
    $("#Identification").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Normal' },
           { 'value': false, 'text': 'Abnormal' }
        ]
    });
    $("#Appearance").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Normal' },
           { 'value': false, 'text': 'Abnormal' }
        ]
    });
    $("#OthersStatus").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Normal' },
           { 'value': false, 'text': 'Abnormal' }
        ]
    });
    $("#Status").combobox({
        panelHeight: 50,
        data: [
           { 'value': 0, 'text': '在用' },
           { 'value': 1, 'text': '停用' },
           { 'value': 2, 'text': '作废' }
        ]
    });
    var selectRow = $('#sample_management_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        $('#Sample_dialog').dialog({
            width: 750,
            height: 500,
            modal: true,
            title: 'Sample Info.',
            draggable: true,
            buttons: [
                {
                    text: 'save',
                    iconCls: 'icon-ok',
                    handler: function () {
                        $('#Sample_dialog').form('submit', {
                            url: "/SampleManagement/EditMotortbaseinfo",//接收一般处理程序返回来的json数据 
                            onSubmit: function (param) {
                                param.id = selectRow.id;
                                param.MTRNO = selectRow.MTRNO;
                            },
                            success: function (data) {
                                if (data) {
                                    var result = $.parseJSON(data);
                                    if (result.Success == true) {
                                        $('#Sample_dialog').dialog('close');
                                        $.messager.alert('Tips', result.Message);
                                        $('#sample_management_datagrid').datagrid('reload');
                                    }
                                    else {
                                        $.messager.alert('Tips', result.Message);
                                    }
                                }
                            }
                        });
                    }
                }
                ,{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Sample_dialog').dialog('close');
                }
            }]
        })
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };
    $('#Sample_dialog').form('load', selectRow);//数据回显
    $('#Retrieval').combobox('setValue', selectRow.Retrieval);//数据回显
    $('#Appearance').combobox('setValue', selectRow.Appearance);//数据回显
    $('#Identification').combobox('setValue', selectRow.Identification);//数据回显
    $('#OthersStatus').combobox('setValue', selectRow.OthersStatus);//数据回显
    $('#TestLoad').combobox('setValue', selectRow.TestLoad);//数据回显
    $('#Status').combobox('setValue', selectRow.Status);//数据回显
};
//**********************************查看样品信息
function read_sample() {
    //下拉框内容
    $("#Retrieval").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Yes' },
           { 'value': false, 'text': 'No' }
        ]
    });
    $("#TestLoad").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Attach Load' },
           { 'value': false, 'text': 'No Attach Load' }
        ]
    });
    $("#Identification").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Normal' },
           { 'value': false, 'text': 'Abnormal' }
        ]
    });
    $("#Appearance").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Normal' },
           { 'value': false, 'text': 'Abnormal' }
        ]
    });
    $("#OthersStatus").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Normal' },
           { 'value': false, 'text': 'Abnormal' }
        ]
    });
    $("#Status").combobox({
        panelHeight: 50,
        data: [
           { 'value': 0, 'text': '在用' },
           { 'value': 1, 'text': '停用' },
           { 'value': 2, 'text': '作废' }
        ]
    });
    var selectRow = $('#sample_management_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        $('#Sample_dialog').dialog({
            width: 750,
            height: 500,
            modal: true,
            title: 'Sample Info.',
            draggable: true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Sample_dialog').dialog('close');
                }
            }]
        })
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    };
    $('#Sample_dialog').form('load', selectRow);//数据回显
    $('#Retrieval').combobox('setValue', selectRow.Retrieval);//数据回显
    $('#Appearance').combobox('setValue', selectRow.Appearance);//数据回显
    $('#Identification').combobox('setValue', selectRow.Identification);//数据回显
    $('#OthersStatus').combobox('setValue', selectRow.OthersStatus);//数据回显
    $('#TestLoad').combobox('setValue', selectRow.TestLoad);//数据回显
    $('#Status').combobox('setValue', selectRow.Status);//数据回显
};
//******************************************************电机信息dialog****************************************************//
function Motor_info() {
    var selectRow = $('#sample_management_datagrid').datagrid('getSelected');//获取选中行的样品信息
    if (selectRow) {
        $('#datagrid_dialog').dialog({
            width: 800,
            height: 500,
            modal: true,
            title: 'Motor Info',
            draggable: true,
            buttons: [
                {
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#datagrid_dialog').dialog('close'); //关闭电机信息dialog
                    }
                }
            ]
        });
    } else {
        $.messager.alert('tip', 'Please select the line to operate！')
    };
    Motor_datagrid(selectRow); //电机记录列表初始化
    //搜索Motor信息
    $("#search_motor").unbind("click").bind("click", function () {
        search_motor(selectRow);
    });
    //搜索下拉框的信息
    $("#search_combox").combobox({
        panelHeight: 120,
        data: [
                { 'value': 'MotoNum', 'text': 'Motor Num' },
                { 'value': 'Status', 'text': 'Status' },
                { 'value': 'Scrap', 'text': 'Scrap' },
        ]
    });
};
//******************************************************电机记录列表
function Motor_datagrid(selectRow) {
    $('#Motor_datagrid').datagrid(
           {
               nowrap: false,
               striped: true,
               rownumbers: true,
               ctrlSelect: true,
               checkbox: true,
               border: true,
               resizable: false,
               pagination: true,
               pageSize: 15,
               fit: true,
               pageList: [15, 30, 45, 60],
               pageNumber: 1,
               url: '/SampleManagement/GetSample_MotorList',
               type: 'POST',
               dataType: "json",
               queryParams: { MTRNO: selectRow.MTRNO },//传MTRNO给后台
               columns: [[
                { field: 'MTRNO', title: 'MTR NO.', sortable: 'true', width: 100 },
                { field: 'MotoNum', title: 'Motor Num', width: 100 },//电机编号
                { field: 'AddressId_n', title: 'Address', width: 100 },//样板位置
                { field: 'SortNum', title: 'Sort Num', width: 100, hidden: true },//电机序号
                {
                    field: 'Status', title: 'Status', width: 70, formatter: function (value, row, index) {//状态
                        if (value == 0) {
                            return "初始";
                        } else if (value == 1) {
                            return "在库";
                        } else if (value == 2) {
                            return "出库";
                        } else if (value == 3) {
                            return "失效";
                        }
                    }
                },
                { field: 'SamplePosition', title: 'Address', width: 150, hidden: true },//样板位置
                {
                    field: 'Scrap', title: 'Scrap', width: 70, formatter: function (value, row, index) {//是否失效
                        if (value == true) {
                            return "Yes";
                        } else if (value == false) {
                            return "No";
                        }
                    }
                },
                {
                    field: 'ScrapDate', title: 'Scrap Date', width: 110, formatter: function (value, row, index) {//失效日期
                        if (value) {
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                {
                    field: 'ScrapRemarks', title: 'Scrap Remarks', width: 150, formatter: function (value, row, index) {
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
                {
                    field: 'Remarks', title: 'Remarks', width: 150, formatter: function (value, row, index) {
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
                   $('#Motor_datagrid').datagrid('selectRow', line);
               },
               sortOrder: 'asc',
               toolbar: "#Motor_datagrid_toolbar"
           });
    $("#edit_motor").unbind("click").bind("click", function () {
        edit_motor();
    });
};
//******************************************************搜索电机信息列表
function search_motor(selectRow) {
    var search = $('#search_combox').combobox('getValue');//获取搜索文本框的值
    var key = $('#key_text').textbox('getText');//获取搜索文本框的值
    $('#Motor_datagrid').datagrid({
        type: 'POST',
        dataType: "json",
        url: "/SampleManagement/GetSample_MotorList",
        queryParams: {
            MTRNO: selectRow.MTRNO,
            search: search,//获取搜索下拉框的值传给后台
            key: key//获取搜索文本框的值传给后台
        }
    });
};
/*
*functionName:修改电机信息form
*function:edit_motor
*Param: 
*author:程媛
*date:2018-05-17
*/
function edit_motor() {
    //下拉框内容
    $("#Scrap_statu").combobox({
        panelHeight: 50,
        data: [
           { 'value': true, 'text': 'Yes' },
           { 'value': false, 'text': 'No' }
        ]
    });
    var selectRow = $('#Motor_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        line = $('#Motor_datagrid').datagrid("getRowIndex", selectRow);
        $('#Motor_dialog').dialog({
            width: 650,
            height: 400,
            modal: true,
            title: 'Edit Motor',
            draggable: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                handler: function () {
                    Motor_save(selectRow);
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Motor_dialog').dialog('close');
                }
            }]
        })
    }
    else {
        $.messager.alert('Tips', 'Please select the line to operate！');
    };

    $('#Motor_dialog').form('load', selectRow);//数据回显
    $("#Scrap_statu").combobox("setValue", selectRow.Scrap);

};
/*
*functionName:确认修改电机信息
*function:Motor_save
*Param: ids，Scrap
*author:程媛
*date:2018-05-03
*/
function Motor_save(selectRow) {//form表单提交
    var Scrap = $("#Scrap_statu").combobox("getValue")
    $('#Motor_dialog').form('submit', {
        url: "/SampleManagement/EditMotor",//接收一般处理程序返回来的json数据 
        onSubmit: function (param) {
            param.ids = selectRow.id
            param.Scrap = Scrap
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#Motor_dialog').dialog('close');
                    $.messager.alert('Tips', result.Message);
                    $('#Motor_datagrid').datagrid('reload');
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        }
    });
};
/*
*functionName:样品失效dialog
*function:Scrap_motor
*Param: 
*author:程媛
*date:2018-05-03
*/
function Scrap_motor() {
    var selectRowss = $("#Motor_datagrid").datagrid("getSelected");
    if (selectRowss) {
        if (selectRowss.Scrap != true) {
            line = $('#Motor_datagrid').datagrid("getRowIndex", selectRowss);
            $('#scrap_dialog').dialog({
                width: 500,
                height: 300,
                modal: true,
                title: 'Scrap',
                draggable: true,
                buttons: [{
                    text: 'Save',
                    iconCls: 'icon-ok',
                    handler: function () {
                        scrap_save(selectRowss);
                    }
                }, {
                    text: 'Close',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#scrap_dialog').dialog('close');//关闭弹窗
                    }
                }]
            });
        } else {
            $.messager.alert("Tips", "The line is already in an scrap state, can not be operate！");
        }
    } else {
        $.messager.alert("Tips", "Please select the line to operate！");
    }
    $('#scrap_dialog').form("reset");//重置失效申请表单
};
/*
*functionName:失效申请
*function:scrap_save
*Param: id
*author:程媛
*date:2018-05-03
*/
function scrap_save(selectRowss) {
    $('#scrap_dialog').form('submit', {
        url: "/SampleManagement/DelMotorList",
        onSubmit: function (param) {
            param.ids = selectRowss.id;
            param.MTRNO = selectRowss.MTRNO;
            param.MotoNum = selectRowss.MotoNum;
            return $(this).form('enableValidation').form('validate');
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $.messager.alert('Tips', result.Message);
                $('#scrap_dialog').dialog('close');
                $('#Motor_datagrid').datagrid('reload');
            } else {
                $.messager.alert('Tips', result.Message);
            }
        }
    });
};

//******************************************************导出样品流程卡
function export_Sample() {
    var rows = $('#sample_management_datagrid').datagrid('getSelected');
    if (rows) {
        $.ajax({
            url: "/Samplemanagement/exportSample",
            type: 'POST',
            data: {
                MTRNO: rows.MTRNO
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
}
