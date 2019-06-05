var line = 0;//初始化行
var ArrList = [];//定义一个空的数组
$(function () {
    Init_Sample_circulation();//样品列表树表格初始化
    //归还
    $('#return_checked').unbind('click').bind('click', function () {
        $("#return_checked").prop("checked", "checked");
        $("#borrow_checked").prop("checked", false);
        Init_Sample_circulation();
    });
    //借用
    $('#borrow_checked').unbind('click').bind('click', function () {
        $("#borrow_checked").prop("checked", "checked");
        $("#return_checked").prop("checked", false);
        Init_Sample_circulation();
    });
});
//*******************************************************样品列表树表格加载****************************************************************//
function Init_Sample_circulation() {
    $("#Sample_datagrid").datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        border: true,
        resizable: false,
        pagination: false,
        fitColumns: true,
        fit: true,
        //pageList: [20, 40, 60, 80],
        //pageSize: 20,
        //pageNumber: 1,
        columns: [[
            { field: 'MTRNO', title: 'MTR NO.', sortable: 'true', width: 100 },
            { field: 'SampleCode', title: 'Sample Code', width: 100 },//样品编号
            { field: 'MotoNum', title: 'Motor Num', width: 100 },//电机编号
            { field: 'SampleQty', title: 'Sort Num', width: 100, hidden: true },//电机序号
            {
                field: 'Identification', title: 'Identification', formatter: function (value, row, index) {//样品标识
                    if (value == true) {
                        return "Normal";
                    } else if (value == false) {
                        return "Abnormal";
                    }
                }
            },
            {
                field: 'Appearance', title: 'Appearance', formatter: function (value, row, index) {//样品标识
                    if (value == true) {
                        return "Normal";
                    } else if (value == false) {
                        return "Abnormal";
                    }
                }
            },
            {
                field: 'OthersStatus', title: 'OthersStatus', formatter: function (value, row, index) {//其他状态
                    if (value == true) {
                        return "Normal";
                    } else if (value == false) {
                        return "Abnormal";
                    }
                }
            },
            { field: 'TakeOutSignatureName', title: 'TakeOut Signature' },//取出签名
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
            { field: 'ReturnSignatureName', title: 'Return Signature' },//返回签名
            {
                field: 'ReturnDate', title: 'Return Date', width: 150, formatter: function (value, row, index) {//返回日期
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            {
                field: 'Remark', title: 'Remark', width: 150, formatter: function (value, row, index) {
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
            { field: 'Status', title: 'Status', width: 70 }
        ]],
        onLoadSuccess: function (data) {
            $('#Sample_datagrid').datagrid('selectRow', line);
        },
        toolbar: "#Sample_circulation_toolbar"//工具栏
    });
    //初始化页数记录
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#Sample_datagrid').datagrid("loadData", json);

    //追加样品信息
    $("#append_sample").unbind("click").bind("click", function () {
        var SampleNo = $("#search_text").textbox("getValue");

        if (SampleNo != "") {
            append_sample();
        } else {
            $.messager.alert('Tips', "Sample number cannot be empty！");
        }
    });
    //修改样品信息
    $("#edit_sample").unbind("click").bind("click", function () {
        edit_sample();
    });
    //删除样品信息
    $("#del_sample").unbind("click").bind("click", function () {
        del_sample();
    });
    //提交样品信息
    $("#sumbit_sample").unbind("click").bind("click", function () {
        sumbit_sample();
    });
};
/*
*functionName:追加样品信息
*function:append_sample
*Param: SampleCode
*author:程媛
*date:2018-05-16
*/
function append_sample() {
    var SampleCode = $("#search_text").textbox("getValue");
    $.ajax({
        url: '/SampleManagement/GetMotorInfoIN',
        type: 'POST',
        dataType: "json",
        data: {
            SampleCode: SampleCode
        },
        success: function (data) {
            if (data) {
                if (data.Success == true) {
                    var rowss = $("#Sample_datagrid").datagrid("getRows");//获取所有的行
                    for (var i = 0; i < rowss.length; i++) {
                        ArrList.push(rowss[i].SampleCode);
                    }
                    var type = $.inArray(SampleCode, ArrList);
                    if (type == -1) {
                        $('#Sample_datagrid').datagrid('appendRow', {
                            MTRNO: data.Data.MTRNO,
                            SampleCode: data.Data.SampleCode,
                            MotoNum: data.Data.MotoNum,
                            SampleQty: data.Data.SampleQty,
                            Identification: data.Data.Identification,
                            TakeOutSignatureName: data.Data.TakeOutSignatureName,
                            TakeOutDate: data.Data.TakeOutDate,
                            ReturnSignatureName: data.Data.ReturnSignatureName,
                            ReturnDate: data.Data.ReturnDate,
                            Remark: data.Data.Remark,
                            Status: data.Data.Status
                        });
                    } else {
                        $.messager.alert('Tip', "The SampleNo already exists!");
                        
                    }
                    console.log(ArrList);
                    console.log($.inArray(SampleCode, ArrList));
                }
                else if (data.Success == false) {
                    $.messager.alert('Tip', data.Message);
                }
            }
        }
    });
};
/*
*functionName:修改样品dialog
*function:edit_sample
*Param: 
*author:程媛
*date:2018-05-16
*/
function edit_sample() {
    var selected = $("#Sample_datagrid").datagrid("getSelected");//获取选中行
    if (selected) {
        $("#edit_dialog").dialog({
            width: 500,
            height: 300,
            modal: true,
            title: 'Edit',
            draggable: true,
            buttons: [{
                text: 'Save',
                iconCls: 'icon-ok',
                handler: function () {
                    save_edit();
                }
            }, {
                text: 'Close',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#edit_dialog').dialog('close');//关闭弹窗
                }
            }]
        });
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
    $('#Sample_dialog').form('load', selected);//数据回显
};
/*
*functionName:确认修改样品
*function:save_edit
*Param: 
*author:程媛
*date:2018-05-16
*/
function save_edit() {
    $('#edit_dialog').form('submit', {
        //url: "/SampleManagement/EditSampleStatus",
        onSubmit: function (param) {
            //param.MTRNO = selectRow.MTRNO,//选中的MTRNO单号传给后台
            //param.SendedPerson = $("#ApprovedBy").combobox("getValue")//接收人的id传给后台
            return $(this).form('enableValidation').form('validate');//验证表单
        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#edit_dialog').dialog('close');
                    $.messager.alert('Tip', result.Message);
                    //$('#sample_reception_datagrid').datagrid('reload');
                }
                else if (result.Success == false) {
                    $.messager.alert('Tip', result.Message);
                }
            }
        }
    })
};
/*
*functionName:删除样品信息
*function:del_sample
*Param: SampleCode
*author:程媛
*date:2018-05-16
*/
function del_sample() {
    var selectRow = $("#Sample_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('Tips', 'Are you sure you want to delete the line？', function (r) {
            if (r) {
                $.ajax({
                    // url: "/SampleManagement/DelFileManagement",
                    type: 'POST',
                    data: {
                        SampleCode: selectRow.SampleCode
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('Tips', result.Message);
                            //$('#Sample_datagrid').datagrid('reload');
                        } else {
                            $.messager.alert('Tips', result.Message);
                        }
                    }
                });
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};
////********************************************************样品借出
//function ee() {
//    var checkedItems = $('#Motor_datagrid').datagrid('getChecked');//获取checkbox为true的行的值
//    var tmp = [];//定义一个空数组
//    $.each(checkedItems, function (index, item) {//遍历获取的信息
//        tmp.push(item.MotoNum);
//    });
//    var Motor = tmp.join(',');//把数组转成以逗号分隔的字符串
//    var MTRNO = $("#search_text").textbox("getValue");//获取MTRNO的value值
//    var selectedRow = $("#Sample_datagrid").datagrid('getSelected')
//    if (selectedRow) {
//        $.messager.confirm("Tips", "Are you sure you want to receive?", function (r) {
//            if (!r) {
//                return false;
//            } else {
//                $.ajax({
//                    url: '/SampleManagement/EditSampleCollectionSingle',
//                    type: 'POST',
//                    data: {
//                        Motor: Motor,//传电机编号给后台
//                        MTRNO: MTRNO//传MTRNO单号给后台
//                    },
//                    success: function (data) {
//                        var result = $.parseJSON(data);
//                        if (result.Success == true) {
//                            $.messager.alert('Tips', result.Message);
//                            $('#Motor_datagrid').datagrid('reload');//重新加载Motor列表
//                        } else {
//                            $.messager.alert('Tips', result.Message);
//                        }
//                    }
//                });
//            }
//        });
//    } else {
//        $.messager.alert("Tips", "Please select the row to operate！")
//    }
//};
////********************************************************样品归还//
//function qq() {
//    var checkedItems = $('#Motor_datagrid').datagrid('getChecked');//获取checkbox为true的行的值
//    var tmp = [];//定义一个空数组
//    $.each(checkedItems, function (index, item) {//遍历获取的信息
//        tmp.push(item.MotoNum);
//    });
//    var Motor = tmp.join(',');//把数组转成以逗号分隔的字符串
//    var MTRNO = $("#search_text").textbox("getValue");//获取MTRNO的value值
//    var selectedRow = $("#Motor_datagrid").datagrid('getSelected')
//    if (selectedRow) {
//        $.messager.confirm("Tips", "Are you sure you want to collect?", function (r) {
//            if (!r) {
//                return false;
//            } else {
//                $.ajax({
//                    url: '/SampleManagement/EditSampleReceiveSingle',
//                    type: 'POST',
//                    data: {
//                        Motor: Motor,//传电机编号给后台
//                        MTRNO: MTRNO//传MTRNO单号给后台
//                    },
//                    success: function (data) {
//                        var result = $.parseJSON(data);
//                        if (result.Success == true) {
//                            $.messager.alert('Tips', result.Message);
//                            $('#Motor_datagrid').datagrid('reload');//重新加载Motor列表
//                        } else {
//                            $.messager.alert('Tips', result.Message);
//                        }
//                    }
//                });
//            }
//        });
//    } else {
//        $.messager.alert("Tips", "Please select the row to operate！")
//    }
//};