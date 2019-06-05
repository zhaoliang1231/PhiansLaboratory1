var line = 0;//初始化
$(function () {
    sample_reception_datagrid_init();//列表初始化加载
});
//***********************************************************************样品接收列表初始化***************************************
function sample_reception_datagrid_init() {
    $('#sample_reception_datagrid').datagrid({
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
        url: '/SampleManagement/GetSampleCollectionList',
        queryParams: {
            key: $('#MTR_value').textbox('getText')
        },
        columns: [[
            { field: 'MTRNO', title: 'MTR NO.', width:100,sortable: 'true' },//订单号
            { field: 'SampleName', title: 'SampleName', width: 100, sortable: 'true' },//样品名称
            //{ field: 'SamplePosition', title: 'Sample Position',width:100 },//样板位置
            {
                field: 'TestLoad', title: 'Test Load', width: 100, formatter: function (value, row, index) {//测试负载
                    if (value == true) {
                        return "Attach Load";
                    } else if (value == false) {
                        return "No Attach Load";
                    }
                }
            },
            { field: 'FollowUp_n', title: 'Follow Person', width: 100 },//跟进人
            { field: 'SampleQty', title: 'Sample Qty', width: 80 },//样品数量
            //{ field: 'SampleDetails', title: 'Sample Details', width: 100 },//样品详情
            //{ field: 'SampleDescription', title: 'Sample Description', width: 130 },//样品描述
            //{
            //    field: 'Identification', title: 'Identification', formatter: function (value, row, index) {//样品标识
            //        if (value == true) {
            //            return "Normal";
            //        } else if (value == false) {
            //            return "Abnormal";
            //        }
            //    }
            //},
            //{
            //    field: 'Appearance', title: 'Appearance', formatter: function (value, row, index) {//样品外观
            //        if (value == true) {
            //            return "Normal";
            //        } else if (value == false) {
            //            return "Abnormal";
            //        }
            //    }
            //},
            //{
            //    field: 'OthersStatus', title: 'OthersStatus', formatter: function (value, row, index) {//其他状态
            //        if (value == true) {
            //            return "Normal";
            //        } else if (value == false) {
            //            return "Abnormal";
            //        }
            //    }
            //},
            {
                field: 'ReceivingDate', title: 'Receiving Date', width: 100, formatter: function (value, row, index) {//接收日期
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            { field: 'Reviewer_n', title: 'Reviewer', width: 100 },//样品接收人
            { field: 'ReceivingSignature_n', title: 'Receiving Signature', width: 120 },//接收人签名
            {
                field: 'Retrieval', title: 'Retrieval（Y/N）', width: 100, formatter: function (value, row, index) {//是否取回
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            { field: 'Retriever_n', title: 'Retriever', width: 100 },//取回人
            { field: 'ContactMethod', title: 'ContactMethod', width: 100 },//联系方式
            {
                field: 'RetrievalDate', title: 'Retrieval Date', width: 100, formatter: function (value, row, index) {//取回日期
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            {
                field: 'Status', title: 'Status', width: 50, formatter: function (value, row, index) {//状态
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
                field: 'Remarks', title: 'Remarks', width: 100, formatter: function (value, row, index) {
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
            $('#sample_reception_datagrid').datagrid('selectRow', line);
        },
        toolbar: "#sample_reception_toolbar"//工具栏
    });
    //搜索
    $('#search').unbind("click").bind("click", function () {
        search();
    });
    //查看样品信息
    $('#read_sample').unbind("click").bind("click", function () {
        read_sample();
    });
    //样品信息
    $('#Motor').unbind("click").bind("click", function () {
        Sample_init();
    });
    //打印全部的样品接收条码
    $('#print').unbind("click").bind("click", function () {
        printQR();
    });
    //拍照
    $("#Take_photo").unbind("click").bind("click", function () {
        Take_photo();
    });
    //确认接收
    $("#receive_sample").unbind("click").bind("click", function () {
        receive_sample();
    });
};
//************************************打印全部样品接收条码
function printQR() {
    $.ajax({
        url: "/SampleManagement/GetSampleAllList",//接收一般处理程序返回来的json数据     
        type: 'POST',
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                //for (var i = 0; i < result.rows.length; i++) {
                LODOP = getLodop();
                //LODOP.ADD_PRINT_BARCODE(10, 20, 174, 161, "QRCode", result.MTRNO + " ");//上边距，左边距，宽度，高度
                //LODOP.ADD_PRINT_TEXT(165, 35, 350, 20, result.rows[i].MTRNO + "   ");

                LODOP.ADD_PRINT_BARCODE(7, 9, 75, 70, "QRCode", result.SampleNo);
                LODOP.ADD_PRINT_TEXT(19, 85, 100, 30, "JE Lab");
                LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
                LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
                LODOP.ADD_PRINT_TEXT(76, 8, 198, 20, result.SampleNo);
                LODOP.PRINT();
                //}
            }
        }
    });
};
//***************************************搜索
function search() {
    var key = $('#MTR_value').textbox('getText');
    $('#sample_reception_datagrid').datagrid({
        type: 'POST',
        dataType: "json",
        url: '/SampleManagement/GetSampleCollectionList',//接收一般处理程序返回来的json数据                
        queryParams: {
            key: key
        }
    });
};
//**********************************查看样品信息
function read_sample() {
    ////下拉框内容
    //$("#Retrieval").combobox({
    //    panelHeight: 50,
    //    data: [
    //       { 'value': true, 'text': 'Yes' },
    //       { 'value': false, 'text': 'No' }
    //    ]
    //});
    //$("#TestLoad").combobox({
    //    panelHeight: 50,
    //    data: [
    //       { 'value': true, 'text': 'Attach Load' },
    //       { 'value': false, 'text': 'No Attach Load' }
    //    ]
    //});
    //$("#Identification").combobox({
    //    panelHeight: 50,
    //    data: [
    //       { 'value': true, 'text': 'Normal' },
    //       { 'value': false, 'text': 'Abnormal' }
    //    ]
    //});
    //$("#Appearance").combobox({
    //    panelHeight: 50,
    //    data: [
    //       { 'value': true, 'text': 'Normal' },
    //       { 'value': false, 'text': 'Abnormal' }
    //    ]
    //});
    //$("#OthersStatus").combobox({
    //    panelHeight: 50,
    //    data: [
    //       { 'value': true, 'text': 'Normal' },
    //       { 'value': false, 'text': 'Abnormal' }
    //    ]
    //});
    $("#Status").combobox({
        panelHeight: 50,
        data: [
           { 'value': 0, 'text': '初始' },
           { 'value': 1, 'text': '在库' },
           { 'value': 2, 'text': '出库' }
        ]
    });
    var selectRow = $('#sample_reception_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        line = $('#sample_reception_datagrid').datagrid("getRowIndex", selectRow);
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
//***********************************样品列表
function Sample_init() {
    var selectRow = $('#sample_reception_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        $('#datagrid_dialog').dialog({
            width: 800,
            height: 500,
            modal: true,
            title: 'Motor Info',
            fit: true,
            draggable: true,
            buttons: [{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#datagrid_dialog').dialog('close');
                }
            }]
        })
    } else {
        $.messager.alert('tip', 'Please select the line to operate！')
    };

    //电机记录
    $('#Motor_datagrid').datagrid(
       {
           nowrap: false,
           striped: true,
           rownumbers: true,
           ctrlSelect: true,
           //checkbox: true,
           border: true,
           resizable: false,
           remoteSort: false,
           pagination: true,
           pageSize: 5,
           fit: true,
           pageList: [5, 30, 45, 60],
           fitColumns: true,
           pageNumber: 1,
           url: '/SampleManagement/GetMotorList',
           type: 'POST',
           dataType: "json",
           queryParams: { MTRNO: selectRow.MTRNO },//传MTRNO给后台
           columns: [[
            { field: 'MTRNO', title: 'MTR NO.', width: 100 },
            { field: 'MotoNum', title: 'Motor Num', sortable: 'true', width: 100 },//电机编号
            { field: 'SortNum', title: 'Sort Num', width: 100, hidden: true },//电机序号
            { field: 'AddressId_n', title: 'Address', width: 100 },//电机存放地址
            {
                field: 'Status', title: 'Status', width: 50, formatter: function (value, row, index) {//状态
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
            { field: 'SamplePosition', title: 'Sample Position', width: 150, hidden: true },//样板位置
            {
                field: 'Scrap', title: 'Scrap', width: 50, formatter: function (value, row, index) {
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },//是否失效
            {
                field: 'ScrapDate', title: 'Scrap Date', width: 100, formatter: function (value, row, index) {//失效日期
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
           rowStyler: function (index, row) {
               if (row.Status == 3) {
                   return 'color:red;';
               }
           },
           sortOrder: 'asc',
           toolbar: Motor_datagrid_toolbar
       });
    //添加电机信息
    $('#AddRecord').unbind("click").bind("click", function () {
        AddRecord();
    });
    //存放位置
    $('#Position').unbind("click").bind("click", function () {
        Position();
    });
    //生成电机
    $("#generate_samples").unbind("click").bind("click", function () {
        generate_samples();
    });
    //作废电机
    $("#del").unbind("click").bind("click", function () {
        del_all();
    });
    //打印选中条码
    $('#print1').unbind("click").bind("click", function () {
        printSlectQrcode();
    });
    //打印预览
    $('#print2').unbind("click").bind("click", function () {
        PREVIEW_();
    });
    //打印全部
    $('#print3').unbind("click").bind("click", function () {
        printAllQrcode();
    });

};
//**************************************拍照dialog
function Take_photo() {
    var selectRow = $('#sample_reception_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        $('#take_photo_dialog').dialog({
            width: 1020,
            height: 600,
            modal: true,
            title: 'Photo Information',
            draggable: true,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#take_photo_dialog').dialog('close');
                }
            }]
        });
        $('#take_photo_datagrid').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            ctrlSelect: true,
            border: false,
            resizable: false,
            pagination: true,
            pageSize: 15,
            fitColumns: true,
            fit: true,
            pageList: [15, 30, 40, 60],
            pageNumber: 1,
            type: 'POST',
            queryParams: {
                MTRNO: selectRow.MTRNO
            },
            url: '/SampleManagement/GetSampleImgList',
            dataType: "json",
            columns: [[
             { field: 'MTRNO', title: 'MTR NO.', sortable: 'true', width: 100 },
             { field: 'PictureUrl', title: 'PictureUrl', width: 100, hidden: true },
             { field: 'PictureName', title: 'PictureName', width: 100 },
             { field: 'sort', title: 'sort', width: 50 }
            ]],
            onLoadSuccess: function (data) {
                $('#take_photo_datagrid').datagrid('selectRow', 0);
            },
            toolbar: take_photo_datagrid_toolbar
        });
    } else {
        $.messager.alert('Tips', 'Please select the line to operate！');
    }
    $("#WebCam_photo").prop("src", "/WebCam/Index?MTRNO=" + escape(selectRow.MTRNO) + "");

    //查看图片
    $('#take_photo_View').unbind("click").bind("click", function () {
        take_photo_View();
    });
    //删除照片
    $('#take_photo_delete').unbind("click").bind("click", function () {
        take_photo_delete();
    });

};
//***************************************修改电机信息
function AddRecord() {
    //下拉框内容
    $("#Scrap").combobox({
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
            title: 'Sample Info',
            draggable: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                handler: function () {
                    Motor_save();
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
    $("#Scrap").combobox("setValue", selectRow.Scrap);

    function Motor_save() {//form表单提交
        var Scrap = $("#Scrap").combobox("getValue")
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
};
//*****************************************存放位置加载
function Position() {
    var selectRow = $('#Motor_datagrid').datagrid('getSelected');//获取选中行
    var selected_motor = $("#Motor_datagrid").datagrid("getData");//获取电机列表所有的数据
    if (selected_motor.rows.length != 0) {//判断电机列表没有数据的时候不能打开存放位置列表
        $('#position_dialog').dialog({
            width: 700,
            height: 450,
            modal: true,
            title: 'Position',
            draggable: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                handler: function () {
                    place_save(selectRow);//确认保存初始化
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#position_dialog').dialog('close');//关闭存放位置弹窗
                    $("#times").numberbox("setValue", "");
                }
            }]
        });
        area();//位置下拉框
    } else {
        $.messager.alert('Tips', "No data here, you can't ues the address！");
    };
};
//地点下拉框
function area() {
    var sample_row = $('#sample_reception_datagrid').datagrid('getSelected');//获取选中行
    $('#area').combobox({
        url: "/SampleManagement/GetWarehouseList",
        valueField: 'NodeId',
        textField: 'NodeName',
        queryParams: {
            NodeParent: '145BAA3A-EBF6-453E-8E21-379565488CF7'
        },

        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onLoadSuccess: function (data) {
            $('#area').combobox('setValue', data[0].NodeName);
            var NodeId = data[0].NodeId
            position_datagrid(sample_row, NodeId);
        },
        //2级
        onSelect: function (record) {
            var NodeId = record.NodeId;
            position_datagrid(sample_row, NodeId);
        }
    })
};
//加载仓库位置信息
function position_datagrid(sample_row, NodeId) {
    $('#position_datagrid').datagrid(
       {
           border: true,
           nowrap: false,
           striped: true,
           checkbox: true,
           singleselect: false,
           fit: true,
           fitColumns: true,
           rownumbers: true,
           remoteSort: false,
           pagination: true,
           pageSize: 15,
           pageList: [15, 30, 45, 60],
           pageNumber: 1,
           queryParams: {
               MTRNO: sample_row.MTRNO,
               NodeParent: NodeId
           },
           url: '/SampleManagement/GetWarehouseData',
           type: 'POST',
           dataType: "json",
           columns: [[
            { field: 'NodeName2', title: 'Shelves', width: 100 },//货架
            { field: 'NodeName', title: 'Drawer', sortable: 'true', width: 100 },//抽屉
            { field: 'NodeUrl', title: 'Address', width: 100 },//地址
            {
                field: 'WarehousingState', title: 'WarehousingState', width: 100, formatter: function (value, row, index) {//状态
                    if (value == 0) {
                        return "未占";
                    } else if (value == 1) {
                        return "占用";
                    } else if (value == 2) {
                        return "停用";
                    }
                }
            },
            //{
            //    field: 'Remarks', title: 'Remarks', width: 150, formatter: function (value, row, index) {//说明
            //        if (value) {
            //            if (value.length > 30) {
            //                var result = value.replace(" ", "");//去空
            //                var value1 = result.substr(0, 30);
            //                return '<span  title=' + value + '>' + value1 + "......" + '</span>';
            //            } else {
            //                return '<span  title=' + value + '>' + value + '</span>';
            //            }
            //        }

            //    }
            //}
           ]],
           onLoadSuccess: function (data) {
               $('#position_datagrid').datagrid('selectRow', line);
           },
           onClickRow: function (row) {
               var rows = $('#position_datagrid').datagrid('getSelections');
               $("#times").numberbox("setValue", rows.length)
           },
           onSelect: function () {
               var rowss = $("#position_datagrid").datagrid("getSelected");
               if (rowss) {
                   $("#times").numberbox("setValue", "1");
               } else {
                   $("#times").numberbox("setValue", "0");
               }
           },
           sortOrder: 'asc',
           toolbar: position_datagrid_toolbar
       });
};
//保存位置
function place_save(selectRow) {
    var rowss = $("#position_datagrid").datagrid("getRows");
    var motorDatagrid = $("#Motor_datagrid").datagrid("getData");//获取点击信息列表的所有数据
    var motorDatagridTotal = motorDatagrid.total;
    //表单提交
    if ($("#times").numberbox("getText") != "0" && rowss.length != 0) {
        if (motorDatagridTotal >= $("#times").numberbox("getText")) {
            $.messager.confirm('Tips',
                'Are you sure you want to save in these locations？',
                function(r) {
                    if (r) {
                        var selectRow = $("#sample_reception_datagrid").datagrid("getSelected"); //获取样品接收列表选中行的数据
                        var rows = $("#position_datagrid").datagrid("getSelections"); //获取存放位置行的信息
                        for (var i = 0; i < rows.length; i++) {
                            if (i == 0) {
                                ids = rows[i].NodeId;
                            }
                            if (i > 0) {
                                ids = ids + "," + rows[i].NodeId;
                            }
                        };

                        $.ajax({
                            url: "/SampleManagement/EditWarehouseListState", //接收一般处理程序返回来的json数据
                            type: "POST",
                            data: {
                                //传送额外的值给后台
                                Warehousing: ids, //选中行货架的NodeId传给后台
                                MTRNO: selectRow.MTRNO, //MTRNO传给后台
                                Copies: $("#times").numberbox("getValue")
                            },
                            success: function(data) {
                                if (data) {
                                    var result = $.parseJSON(data);
                                    if (result.Success == true) {
                                        $.messager.alert('Tips', result.Message);
                                        $('#position_dialog').dialog('close'); //关闭弹窗
                                        $('#Motor_datagrid').datagrid('reload'); //重新加载列表
                                    } else if (result.Success == false) {
                                        $.messager.alert('Tips', result.Message);
                                    }
                                }
                            }
                        });
                    }
                });
        } else {
            $.messager.alert('Tips', "Selected extra location, please select the correct quantity！");
        }
    } else {
        $.messager.alert('Tips', "Please select the location！");
    }
};
//**********************************************生成样品
function generate_samples() {
    var selectRow = $("#sample_reception_datagrid").datagrid("getSelected");//获取样品接收列表选中行的数据
    //if ($("#Motor_datagrid").datagrid("getData").total == 0) {//判断Motor列表是否有数据
    $('#generate_samples_dailog').dialog({
        width: 500,
        height: 200,
        modal: true,
        title: 'Sample Info',
        draggable: true,
        buttons: [{
            text: 'Save',
            iconCls: 'icon-ok',
            handler: function () {
                Sample_save();
            }
        }, {
            text: 'Cancel',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#generate_samples_dailog').dialog('close');//关闭
            }
        }]
    });
    $('#SampleNum').textbox({ prompt: "只能输入'1#~20#'的格式！" });//给Motor Num设置占位符
    function Sample_save() {
        $('#generate_samples_dailog').form('submit', {
            url: "/SampleManagement/CreateSample",//接收一般处理程序返回来的json数据
            onSubmit: function (param) {
                param.MTRNO = selectRow.MTRNO//MTRNO传给后台
                return $(this).form('enableValidation').form('validate');
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $.messager.alert('Tips', result.Message);
                        $('#generate_samples_dailog').dialog('close');//关闭生成样品弹窗
                        $('#Motor_datagrid').datagrid('reload');//重新加载列表
                    } else if (result.Success == false) {
                        $.messager.alert('Tips', result.Message);
                    }
                }
            }
        });
    };
    $('#generate_samples_dailog').form('reset');//重置生成样品表单
    //} else {
    //    $.messager.alert('Tips', "The list has sample data and cannot reproduce samples！");
    //}

};


//************************************************删除的样品
function del_all() {
    var selectRowss = $("#Motor_datagrid").datagrid("getSelected");
    if (selectRowss) {
        if (selectRowss.Status == "1") {
            $.messager.confirm('Tips', 'Are you sure you want to invalid this Motor Num？', function (r) {
                if (!r) {
                    return false;
                } else {
                    $.ajax({
                        url: "/SampleManagement/DelMotorList",
                        type: 'POST',
                        data: {
                            ids: selectRowss.id
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $('#Motor_datagrid').datagrid('reload');
                                $.messager.alert('tips', result.Message);
                            } else {
                                $.messager.alert('tips', result.Message);
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('tips', 'The sample is borrowed and cannot be operate！');
        }
    }
    else {

        $.messager.alert('tips', 'Please select the line to operate！');
    }
};
//********************************************打印全部样品条码
function printAllQrcode() {
    var selectRow = $('#sample_reception_datagrid').datagrid('getSelected');
    if (selectRow) {
        $.ajax({
            url: "/Samplemanagement/GetMotorNumList",//接收一般处理程序返回来的json数据     
            type: 'POST',
            data: {
                MTRNO: selectRow.MTRNO
            },
            success: function (data) {
                var result = $.parseJSON(data);
                for (var i = 0; i < result.length; i++) {
                    LODOP = getLodop();


                    LODOP.ADD_PRINT_BARCODE(2, 8, 46, 46, "QRCode", result[i].MotoNum);
                    LODOP.ADD_PRINT_TEXT(19, 60, 70, 30, "JE Lab");
                    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
                    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
                    LODOP.ADD_PRINT_TEXT(45, 8, 80, 20, result[i].MotoNum);

                    //LODOP.ADD_PRINT_BARCODE(10, 20, 174, 161, "QRCode", result[i].MTRNO + " " + result[i].MotoNum);//上边距，左边距，宽度，高度
                    //LODOP.ADD_PRINT_TEXT(165, 35, 350, 20, result[i].MTRNO + "   " + result[i].MotoNum);
                    LODOP.PRINT();
                }

            }
        });
    }
}
//**********************************************打印选中样品条码
function printSlectQrcode() {
    var selectRow = $('#Motor_datagrid').datagrid('getSelections');
    for (var i = 0; i < selectRow.length; i++) {
        LODOP = getLodop();

        LODOP.ADD_PRINT_BARCODE(2, 8, 46, 46, "QRCode", selectRow[i].MotoNum);
        LODOP.ADD_PRINT_TEXT(19, 60, 70, 30, "JE Lab");
        LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
        LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
        LODOP.ADD_PRINT_TEXT(45, 8, 80, 20, selectRow[i].MotoNum);
        //LODOP.ADD_PRINT_BARCODE(10, 20, 174, 161, "QRCode",  selectRow[i].MotoNum);//上边距，左边距，宽度，高度
        //LODOP.ADD_PRINT_TEXT(165, 35, 350, 20, selectRow[i].MotoNum);
        LODOP.PRINT();
    }
}
//**************************************************打印预览
function PREVIEW_() {
    var selectRow = $('#Motor_datagrid').datagrid('getSelected');
    LODOP = getLodop();

    LODOP.ADD_PRINT_BARCODE(2, 8, 46, 46, "QRCode", selectRow.MotoNum);
    LODOP.ADD_PRINT_TEXT(19, 60, 70, 30, "JE Lab");
    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
    LODOP.SET_PRINT_STYLEA(0, "Bold", 1);
    LODOP.ADD_PRINT_TEXT(45, 8, 80, 20, selectRow.MotoNum);


    LODOP.SET_PREVIEW_WINDOW(1, 1, 0, 600, 400, "二维码打印预览")

    //LODOP.ADD_PRINT_BARCODE(10, 200, 174, 161, "QRCode", selectRow.MTRNO + " " + selectRow.MotoNum);//上边距，左边距，宽度，高度
    //LODOP.ADD_PRINT_TEXT(165, 235, 350, 20, selectRow.MTRNO + "   " + selectRow.MotoNum);
    //LODOP.SET_PREVIEW_WINDOW(1, 1, 0, 600, 400, "二维码打印预览")
    LODOP.PREVIEW();//打印预览
    //LODOP.PRINT();


}


//刷新列表
function take_photo_datagrid_refresh() {
    $('#take_photo_datagrid').datagrid("reload");
}
//*****************************************删除一个照片
function take_photo_delete() {
    var selectRow = $('#take_photo_datagrid').datagrid('getSelected');
    var rowss = $('#take_photo_datagrid').datagrid('getSelections');

    if (selectRow) {//id小于29的组都为必要组
        var id1 = [];
        var PictureUrl1 = [];
        var PictureNamel = [];
        for (var i = 0; i < rowss.length; i++) {
            id1.push(rowss[i].id);
            PictureUrl1.push(rowss[i].PictureUrl);
            PictureNamel.push(rowss[i].PictureName);
        }
        var ids = id1.join(",");
        var PictureUrls = PictureUrl1.join(",");
        var PictureNamels = PictureNamel.join(",");
        $.messager.confirm('Tips', 'Are you sure you want to delete the selected photo?', function (r) {
            if (!r) {
                return false;
            } else {
                $.ajax({
                    url: '/SampleManagement/Del_SampleImg',
                    type: 'POST',
                    data: {
                        ids: ids,
                        PictureUrls: PictureUrls,
                        PictureNames: PictureNamels
                    },
                    success: function (data) {
                        if (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $.messager.alert('Tips', result.Message);
                                $('#take_photo_datagrid').datagrid('reload');//重新加载照片列表
                            } else {
                                $.messager.alert('Tips', result.Message);
                            }
                        }

                    }
                })
            }
        });
    }
};
//*********************************************拍照添加
function take_photo_View() {
    var selectRow = $('#take_photo_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        $('#take_photo_view_dialog').dialog({
            width: 1000,
            height: 500,
            modal: true,
            title: 'Photo',
            draggable: true,
            buttons: [{
                text: 'close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#take_photo_view_dialog').dialog('close');
                }
            }]
        });
        $("#take_photo_view_look").prop("src", selectRow.PictureUrl);
    } else {
        $.messager.alert('Tips', 'Please select the line to operate！');
    }
};
/*
 *functionName:load_personal()
 *function:加载人员下拉框
 *Param 
 *author:程媛
 *date:2018/4/23
*/
function load_personal() {
    $('#Group').combobox({
        url: "/Common/LoadGroupCombobox",
        valueField: 'Value',
        textField: 'Text',
        required: true,
        onLoadSuccess: function (data) {
            $('#Group').combobox('setValue', data[0].Value);
            var GroupId = $('#Group').combobox('getValue');
            //详细位置---领取信息dialog
            $('#ApprovedBy').combobox({
                url: "/Common/LoadPersonnelCombobox",
                valueField: 'Value',
                textField: 'Text',
                required: true,
                //editable: false,
                queryParams: {
                    "GroupId": GroupId
                },
                onLoadSuccess: function (data) {
                    $('#ApprovedBy').combobox('setValue', data[0].Value);
                },

                //本地联系人数据模糊索引
                filter: function (q, row) {
                    var opts = $(this).combobox('options');
                    return row[opts.textField].indexOf(q) >= 0;
                }
            })
        },

        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onSelect: function () {
            var GroupId = $('#Group').combobox('getValue');
            //详细位置---领取信息dialog
            $('#ApprovedBy').combobox({
                url: "/Common/LoadPersonnelCombobox",
                valueField: 'Value',
                textField: 'Text',
                required: true,
                //editable: false,
                queryParams: {
                    "GroupId": GroupId
                },
                //本地联系人数据模糊索引
                filter: function (q, row) {
                    var opts = $(this).combobox('options');
                    return row[opts.textField].indexOf(q) >= 0;
                }
            })
        }
    });
}
//选择接收样品人员信息弹窗
function receive_sample() {
    var selectRow = $("#sample_reception_datagrid").datagrid("getSelected");
    //加载样品接收人下拉框
    load_personal();
    if (selectRow) {
        $("#submitSampleReceive").dialog({
            width: 800,
            height: 450,
            modal: true,
            title: 'Receive',
            draggable: true,
            buttons: [{
                text: 'Sumbit',
                iconCls: 'icon-ok',
                handler: function () {
                    submitSampleReceive(selectRow);
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#submitSampleReceive').dialog('close');
                }
            }]
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    $('#submitSampleReceive').form('load', selectRow);//数据回显
};
//提交样品接收
function submitSampleReceive(selectRow) {
    $('#submitSampleReceive').form('submit', {
        url: "/SampleManagement/EditSampleStatus",
        onSubmit: function (param) {
            param.MTRNO = selectRow.MTRNO,//选中的MTRNO单号传给后台
            param.FollowUp = selectRow.FollowUp,//选中的MTRNO单号传给后台
            param.SendedPerson = $("#ApprovedBy").combobox("getValue")//接收人的id传给后台
            return $(this).form('enableValidation').form('validate');//验证表单
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#submitSampleReceive').dialog('close');
                    $.messager.alert('Tip', result.Message);
                    $('#sample_reception_datagrid').datagrid('reload');
                }
                else if (result.Success == false) {
                    $.messager.alert('Tip', result.Message);
                }
            }
        }
    })
};

