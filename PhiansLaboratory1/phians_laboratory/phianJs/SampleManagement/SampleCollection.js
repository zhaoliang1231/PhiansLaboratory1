$(function () {
    sample_collection_datagrid();//列表初始化加载
    receive_personnel();//下拉框初始化
    //搜索
    $('#search').unbind("click").bind("click", function () {
        search();
    });
    ////查看样品信息
    //$('#read_sample').unbind("click").bind("click", function () {
    //    read_sample();
    //});
    //电机信息
    $('#Motor').unbind("click").bind("click", function () {
        Motor();
    });
    ////添加检定
    //$('#AddRecord').unbind("click").bind("click", function () {
    //    AddRecord();
    //});
    ////添加检定
    //$('#submit').unbind("click").bind("click", function () {
    //    submit();
    //});
    ////拍照
    //$("#Take_photo").unbind("click").bind("click", function () {
    //    Take_photo();
    //});
    ////查看图片
    //$('#take_photo_View').unbind("click").bind("click", function () {
    //    take_photo_View();
    //});
    ////删除照片
    //$('#take_photo_delete').unbind("click").bind("click", function () {
    //    take_photo_delete();
    //});
    //位置
    $('#place').unbind("click").bind("click", function () {
        place();
    });
    //确认接收
    $("#receive").unbind("click").bind("click", function () {
        receive();
    });
    ////打印
    //$('print').unbind("click").bind("click", function () {
    //    print();
    //});
});
var line = 0;//初始化
//样品接收人下拉框
function receive_personnel() {
    $('#receive_personnel').combobox({
        url: "/ScheduleManagement/GetUserList",
        valueField: 'UserId',
        textField: 'UserName',
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
};
////地点下拉框
//function place() {
//    var selectRow = $('#Motor_datagrid').datagrid('getSelected');//获取选中行
//    if (selectRow) {
//        $('#photo_dialog').dialog({
//            width: 400,
//            height: 250,
//            modal: true,
//            title: 'Place',
//            draggable: true,
//            buttons: [{
//                text: 'Submit',
//                iconCls: 'icon-ok',
//                handler: function () {
//                    place_save();
//                }
//            }, {
//                text: 'Cancel',
//                iconCls: 'icon-cancel',
//                handler: function () {
//                    $('#photo_dialog').dialog('close');
//                }
//            }]
//        }).dialog('close');
//        $('#photo_dialog').dialog('open');
//    }
//    else {
//        $.messager.alert('提示', 'Please  select  the  line to  operate！');
//    };
//    function place_save() {
//        //地址不能为空
//        if ($('#position').combobox('getValue') == "") {
//            $.messager.alert('提示', '地址不能为空！'); return;
//        };
//        //库房不能为空
//        if ($('#treasury').combobox('getValue') == "") {
//            $.messager.alert('提示', '库房不能为空！'); return;
//        };
//        //货架不能为空
//        if ($('#shelves').combobox('getValue') == "") {
//            $.messager.alert('提示', '货架不能为空！'); return;
//        };
//        //表单提交
//        $('#photo_dialog').form('submit', {
//            url: "/SampleManagement/",//接收一般处理程序返回来的json数据  
//            success: function (data) {
//                if (data) {
//                    var result = $.parseJSON(data);
//                    if (result.Success == true) {
//                        $.messager.alert('提示', result.Message);
//                        $('#photo_dialog').dialog('close');
//                        $('#Motor_datagrid').datagrid('reload');
//                    } else if (result.Success == false) {
//                        $.messager.alert('提示', result.Message);

//                    }
//                }
//            }
//        });
//    };
//    $('#photo_dialog').form('reset');//重置表单
//};
//样品接收列表
function sample_collection_datagrid() {
    $('#sample_collection_datagrid').datagrid({
        border: true,
        nowrap: false,
        striped: true,
        ctrlSelect: true,
        fit: true,
       // fitColumns: true,
        rownumbers: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: '/SampleManagement/GetSampleReceptionList',
        columns: [[
            { field: 'MTRNO', title: 'MTR NO.', sortable: 'true' },//订单号
            { field: 'SamplePosition', title: 'Address' ,width:100},//样板位置
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
            { field: 'SampleDetails', title: 'Sample Details', width: 100 },//样品详情
            { field: 'SampleDescription', title: 'Sample Description', width: 100 },//样品描述
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
                field: 'ReceivingDate', title: 'Receiving Date', width: 150, formatter: function (value, row, index) {//接收日期
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            { field: 'Reviewer_n', title: 'Reviewer' },//样品接收人
            { field: 'ReceivingSignature_n', title: 'Receiving Signature' },//接收人签名
            {
                field: 'TakeOutDate', title: 'TakeOut Date', width: 150, formatter: function (value, row, index) {//取出日期

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
                field: 'RetrievalDate', title: 'Retrieval Date', width: 150, formatter: function (value, row, index) {//取回日期
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
            $('#sample_collection_datagrid').datagrid('selectRow', line);
        },
        toolbar: sample_collection_toolbar//工具栏
    });
};
//搜索
function search() {
    var key = $('#MTR_value').textbox('getText');
    $('#sample_collection_datagrid').datagrid({
        type: 'POST',
        dataType: "json",
        url: '/SampleManagement/GetSampleReceptionList',//接收一般处理程序返回来的json数据                
        queryParams: {
            key: key
        }
    });
};
//修改样品信息
//function read_sample() {
//    //下拉框内容
//    $("#Retrieval").combobox({
//        panelHeight: 50,
//        data: [
//           { 'value': true, 'text': 'Yes' },
//           { 'value': false, 'text': 'No' }
//        ]
//    });
//    $("#Appearance").combobox({
//        panelHeight: 50,
//        data: [
//           { 'value': true, 'text': 'Yes' },
//           { 'value': false, 'text': 'No' }
//        ]
//    });
//    $("#Identification").combobox({
//        panelHeight: 50,
//        data: [
//           { 'value': true, 'text': 'Yes' },
//           { 'value': false, 'text': 'No' }
//        ]
//    });
//    $("#OthersStatus").combobox({
//        panelHeight: 50,
//        data: [
//           { 'value': true, 'text': 'Yes' },
//           { 'value': false, 'text': 'No' }
//        ]
//    });
//    var selectRow = $('#sample_collection_datagrid').datagrid('getSelected');//获取选中行
//    if (selectRow) {
//        line = $('#sample_collection_datagrid').datagrid("getRowIndex", selectRow);
//        $('#Sample_dialog').dialog({
//            width: 700,
//            height: 500,
//            modal: true,
//            title: 'Sample Info.',
//            draggable: true,
//            buttons: [{
//                text: 'Save',
//                iconCls: 'icon-ok',
//                handler: function () {
//                    Edit_save();
//                }
//            }, {
//                text: 'Close',
//                iconCls: 'icon-cancel',
//                handler: function () {
//                    $('#Sample_dialog').dialog('close');
//                }
//            }]
//        }).dialog('close');
//        $('#Sample_dialog').dialog('open');
//    } else {
//        $.messager.alert('提示', '请选择要操作的行！');
//    };

//    $('#Sample_dialog').form('load', selectRow);//数据回显
//    $('#Retrieval').form('load', selectRow.Retrieval);//数据回显
//    $('#Appearance').form('load', selectRow.Appearance);//数据回显
//    $('#Identification').form('load', selectRow.Identification);//数据回显
//    $('#OthersStatus').form('load', selectRow.OthersStatus);//数据回显
//    function Edit_save() {//form表单提交
//        $('#Sample_dialog').form('submit', {
//            url: "/SampleManagement/EditSample",//接收一般处理程序返回来的json数据 
//            onSubmit: function (param) {
//                param.ids = selectRow.id
//            },
//            success: function (data) {
//                if (data) {
//                    var result = $.parseJSON(data);
//                    if (result.Success == true) {
//                        $('#Sample_dialog').dialog('close');
//                        $.messager.alert('提示', result.Message);
//                        $('#sample_reception_datagrid').datagrid('reload');
//                    }
//                    else {
//                        $.messager.alert('提示', result.Message);
//                    }
//                }
//            }
//        });
//    };
//};
//电机列表
function Motor() {
    var selectRow = $('#sample_collection_datagrid').datagrid('getSelected');//获取选中行
    if (selectRow) {
        $('#datagrid_dialog').dialog({
            width: 800,
            height: 500,
            modal: true,
            title: 'Motor Info',
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
        $.messager.alert('tip', 'Please select the line to operate！');
    };

    //电机记录
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
           url: '/SampleManagement/GetMotorList',
           type: 'POST',
           dataType: "json",
           queryParams: { MTRNO: selectRow.MTRNO },//传MTRNO给后台
           columns: [[
            //{ field: 'ck', checkbox: 'true' },
            { field: 'MTRNO', title: 'MTR NO.', sortable: 'true', width: 100 },
            { field: 'MotoNum', title: 'Motor Num', width: 100 },//电机编号
            { field: 'AddressId_n', title: 'Address', width: 100 },//样板位置
            { field: 'SortNum', title: 'Sort Num', width: 100 ,hidden:true},//电机序号
            {
                field: 'Status', title: 'Status', width: 70, formatter: function (value, row, index) {//状态
                    if (value == 0) {
                        return "初始";
                    }
                    else if (value == 1) {
                        return "在库";
                    } else if (value == 2) {
                        return "出库";
                    } else if (value == 3) {
                        return "失效";
                    }
                }
            },
            {
                field: 'Scrap', title: 'Scrap', width: 70, formatter: function (value, row, index) {
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },//是否失效
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
           //toolbar: Motor_datagrid_toolbar
       });
};
//修改电机信息
//function AddRecord() {
//    //下拉框内容
//    $("#Scrap").combobox({
//        panelHeight: 50,
//        data: [
//           { 'value': true, 'text': 'Yes' },
//           { 'value': false, 'text': 'No' }
//        ]
//    });
//    var selectRow = $('#Motor_datagrid').datagrid('getSelected');//获取选中行
//    if (selectRow) {
//        line = $('#Motor_datagrid').datagrid("getRowIndex", selectRow);
//        $('#Motor_dialog').dialog({
//            width: 650,
//            height: 400,
//            modal: true,
//            title: 'Motor Info',
//            draggable: true,
//            buttons: [{
//                text: 'Save',
//                iconCls: 'icon-ok',
//                handler: function () {
//                    Motor_save();
//                }
//            }, {
//                text: 'Close',
//                iconCls: 'icon-cancel',
//                handler: function () {
//                    $('#Motor_dialog').dialog('close');
//                }
//            }]
//        })
//    }
//    else {
//        $.messager.alert('提示', 'Please select the line to operate！');
//    };

//    $('#Motor_dialog').form('load', selectRow);//数据回显
//    $("#Scrap").combobox("setValue", selectRow.Scrap);

//    function Motor_save() {//form表单提交
//        $('#Motor_dialog').form('submit', {
//            url: "/SampleManagement/EditMotor",//接收一般处理程序返回来的json数据 
//            onSubmit: function (param) {
//                param.ids = selectRow.id
//                param.Scrap = selectRow.Scrap
//            },
//            success: function (data) {
//                if (data) {
//                    var result = $.parseJSON(data);
//                    if (result.Success == true) {
//                        $('#Motor_dialog').dialog('close');
//                        $.messager.alert('提示', result.Message);
//                        $('#Motor_datagrid').datagrid('reload');
//                    }
//                    else {
//                        $.messager.alert('提示', result.Message);
//                    }
//                }
//            }
//        });
//    };
//};
//拍照dialog
//function Take_photo() {
//    var selectRow = $('#sample_reception_datagrid').datagrid('getSelected');//获取选中行
//    if (selectRow) {
//        $('#take_photo_dialog').dialog({
//            width: 1020,
//            height: 600,
//            modal: true,
//            title: 'Photo Information',
//            draggable: true,
//            buttons: [{
//                text: 'close',
//                iconCls: 'icon-cancel',
//                handler: function () {
//                    $('#take_photo_dialog').dialog('close');
//                }
//            }]
//        });
//        $('#take_photo_datagrid').datagrid({
//            nowrap: false,
//            striped: true,
//            rownumbers: true,
//            ctrlSelect: true,
//            border: false,
//            resizable: false,
//            pagination: true,
//            pageSize: 15,
//            fitColumns: true,
//            fit: true,
//            pageList: [15, 30, 40, 60],
//            pageNumber: 1,
//            type: 'POST',
//            queryParams: {
//                MTRNO: selectRow.MTRNO
//            },
//            url: '/SampleManagement/GetSampleImgList',
//            dataType: "json",
//            columns: [[
//             { field: 'MTRNO', title: 'MRT NO.', sortable: 'true', width: 100 },
//             { field: 'PictureUrl', title: 'PictureUrl', width: 100, hidden: true },
//             { field: 'PictureName', title: 'PictureName', width: 100 },
//             { field: 'sort', title: 'sort', width: 50 }
//            ]],
//            onLoadSuccess: function (data) {
//                $('#take_photo_datagrid').datagrid('selectRow', 0);
//            },
//            toolbar: take_photo_datagrid_toolbar
//        });
//    } else {
//        $.messager.alert('提示', 'Please select the line to operate！');
//    }
//    $("#WebCam_photo").prop("src", "/WebCam/Index?MTRNO=" + escape(selectRow.MTRNO) + "");
//};
//刷新列表
//function take_photo_datagrid_refresh() {
//    $('#take_photo_datagrid').datagrid("reload");
//}
////删除一个照片
//function take_photo_delete() {
//    var selectRow = $('#take_photo_datagrid').datagrid('getSelected');
//    var rowss = $('#take_photo_datagrid').datagrid('getSelections');

//    if (selectRow) {//id小于29的组都为必要组
//        var id1 = [];
//        var PictureUrl1 = [];
//        var PictureNamel = [];
//        for (var i = 0; i < rowss.length; i++) {
//            id1.push(rowss[i].id);
//            PictureUrl1.push(rowss[i].PictureUrl);
//            PictureNamel.push(rowss[i].PictureName);
//        }
//        var ids = id1.join(",");
//        var PictureUrls = PictureUrl1.join(",");
//        var PictureNamels = PictureNamel.join(",");
//        $.messager.confirm('删除提示', '您确认要删除选中该照片吗', function (r) {
//            if (!r) {
//                return false;
//            } else {
//                $.ajax({
//                    url: '/SampleManagement/Del_SampleImg',
//                    type: 'POST',
//                    data: {
//                        ids: ids,
//                        PictureUrls: PictureUrls,
//                        PictureNames: PictureNamels
//                    },
//                    success: function (data) {
//                        if (data) {
//                            var result = $.parseJSON(data);
//                            if (result.Success == true) {
//                                $.messager.alert('提示', '删除信息成功');
//                                $('#take_photo_datagrid').datagrid('reload');
//                            } else {
//                                $.messager.alert('提示', '删除信息失败');
//                            }
//                        }

//                    }
//                })
//            }
//        });
//    }
//};
////拍照添加
//function take_photo_View() {
//    var selectRow = $('#take_photo_datagrid').datagrid('getSelected');//获取选中行
//    if (selectRow) {
//        $('#take_photo_view_dialog').dialog({
//            width: 1000,
//            height: 500,
//            modal: true,
//            title: 'Photo',
//            draggable: true,
//            buttons: [{
//                text: 'close',
//                iconCls: 'icon-cancel',
//                handler: function () {
//                    $('#take_photo_view_dialog').dialog('close');
//                }
//            }]
//        });
//        $("#take_photo_view_look").prop("src", selectRow.PictureUrl);
//    } else {
//        $.messager.alert('提示', 'Please select the line to operate！');
//    }

//}
//BU领取样品
function receive() {
    //接收人不能为空
    if ($('#receive_personnel').combobox('getValue') == "") {
        $.messager.alert('Tips', 'Sended Person can not be empty'); return;
    };
    var Row = $("#sample_collection_datagrid").datagrid("getSelected");//获取选中行的数据
    var userid = $("#receive_personnel").combobox("getValue");//获取接收人的id
    if (Row) {
        $('#Vertify_dialog').dialog({
            title: 'Vertify Code',
            width: 500,
            height: 200,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                handler: function () {
                    savereceive();
                }
            },{
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Vertify_dialog').dialog('close');
                }
            }]

        });
        $("#Vertify_Code").textbox("setText", '');
       
    } else {
        $.messager.alert("Tips", "Please select the sample you want to receive！")
    };
};

//BU领取样品
function savereceive() {
    var Row = $("#sample_collection_datagrid").datagrid("getSelected");//获取选中行的数据
    var userid = $("#receive_personnel").combobox("getValue");//获取接收人的id
    $.ajax({
        url: "/SampleManagement/EditSampleReceptionStatus",
        type: 'POST',
        data: {
            MTRNO: Row.MTRNO,//选中的MTRNO单号传给后台
            FollowUp: userid,//领取人的id传给后台
            VertifyCode: $("#Vertify_Code").textbox("getText")
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $.messager.alert('Tips', result.Message);
                    $('#Vertify_dialog').dialog('close');
                    $("#sample_collection_datagrid").datagrid("reload");//重新加载样品领取的列表
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        }
    });

}