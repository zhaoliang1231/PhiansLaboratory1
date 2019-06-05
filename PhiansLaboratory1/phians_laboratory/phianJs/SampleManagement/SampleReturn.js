var line = 0;//初始化行
var ArrList = [];//定义一个空的数组
$(function () {
    Init_Sample_circulation();//样品列表树表格初始化
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
            { field: 'SampleCode', title: 'Sample Code', width: 150 },//样品编号
            //{ field: 'MotoNum', title: 'Motor Num' },//电机编号
              { field: 'SamplePosition', title: 'Address', width: 100 },//样品位置
            { field: 'TakeOutSignatureName', title: 'TakeOut Signature', hidden: true },//取出签名
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
            { field: 'ReturnSignatureName', title: 'Return Signature', hidden: true },//返回签名
            {
                field: 'ReturnDate', title: 'Return Date', width: 150, formatter: function (value, row, index) {//返回日期
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }, hidden: true
            },
            {
                field: 'Status', title: 'Status', width: 100, formatter: function (value, row, index) {//状态
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
            }
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
    //根据MTRNO追加样品信息
    $("#append_mtr").unbind("click").bind("click", function () {
        append_mtr();
    });
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
*functionName:根据MTRNO追加样品信息
*function:append_mtr
*Param: append_mtr
*author:程媛
*date:2018-05-16
*/
function append_mtr() {
    mtr_datagrid();//初始化列表
    $("#mtr_dialog").dialog({
        width: 1200,
        height: 600,
        modal: true,
        title: 'Choose MTRNO',
        draggable: true,
        buttons: [{
            text: 'Save',
            iconCls: 'icon-ok',
            handler: function () {
                save_choose_mrt();
            }
        }, {
            text: 'Close',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#mtr_dialog').dialog('close');//关闭弹窗
            }
        }]
    });
};
//加载列表
function mtr_datagrid() {
    $("#mtr_datagrid").datagrid({
        nowrap: false,
        striped: true,
        singleSelect: true,
        border: false,
        // fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        url: "",
        columns: [[
            { field: 'MTRNO', title: 'MTR NO.', sortable: 'true' },
            { field: 'MotoNum', title: 'Sample Code' },//样品编号
              { field: 'AddressId_n', title: 'Address', width: 100 },//样品位置
            { field: 'TakeOutSignatureName', title: 'TakeOut Signature', hidden: true },//取出签名
            {
                field: 'TakeOutDate', title: 'TakeOut Date', width: 150, formatter: function (value, row, index) {//取出日期

                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }, hidden: true

            },
            { field: 'ReturnSignatureName', title: 'Return Signature', hidden: true },//返回签名
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
            }
        ]],
        onLoadSuccess: function (data) {
            $('#mtr_datagrid').datagrid('selectRow', line);
        },
        toolbar: "#mtr_datagrid_toolbar"//工具栏
    });
    //初始化页数记录
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#mtr_datagrid').datagrid("loadData", json);

    $("#search_mtr").unbind("click").bind("click", function () {
        search_mtr();
    });
};
/*
*functionName:搜索信息
*function:search_mtr
*Param: 
*author:张慧敏
*date:2018-05-18
*/
function search_mtr() {
    var MTRNO = $("#mtr_text").textbox("getText");
    //测试项目树初始化
    load_tree(MTRNO);
};
/*
*functionName:确认追加信息
*function:save_choose_mrt
*Param: 
*author:张慧敏
*date:2018-05-18
*/
function save_choose_mrt() {
    //  var SampleCode = $("#sample_text").textbox("getText");
    var selected = $("#mtr_datagrid").datagrid("getSelected");
    if (selected) {
        var rowss = $("#mtr_datagrid").datagrid("getRows");//获取所有的行
        for (var i = 0; i < rowss.length; i++) {
            var type = $.inArray(rowss[i].SampleCode, ArrList);
            ArrList.push(rowss[i].SampleCode);
            if (type == -1) {
                for (i = 0; i < rowss.length; i++) {
                    $('#Sample_datagrid').datagrid('appendRow', {
                        MTRNO: rowss[i].MTRNO,
                        SampleCode: rowss[i].MotoNum,
                        TakeOutDate: rowss[i].TakeOutDate,
                        ReturnSignatureName: rowss[i].ReturnSignatureName,
                        ReturnDate: rowss[i].ReturnDate,
                        Remark: rowss[i].Remark,
                        Status: rowss[i].Status,
                        SamplePosition: rowss[i].AddressId_n
                        // SamplePosition: rowss[i].SamplePosition
                    });
                }
            }
        }
        // $("#mtr_text").textbox("setValue", "");
        $("#mtr_text").textbox('textbox').focus();//聚焦样品文本框
    } else {
        $.messager.alert('Tip', "Please select the row to be operated！");
    };

};
/*
*functionName:load_tree
*function:测试项目树初始化
*Param: 
*author:张慧敏
*date:2018-05-18
*/
//初始化测试项目树
function load_tree(MTRNO) {
    $('#Laboratory_tree').tree({
        url: "/ScheduleManagement/LoadDVPProjectTree",
        method: 'post',
        required: true,
        queryParams: {
            MTRNO: MTRNO
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
        onSelect: function () {
            var node_add = $('#Laboratory_tree').tree('getSelected');
            var id;//如果选中行是子项目传父id的值
            if (node_add) {
                //获取父节点的id
                var parent = $('#Laboratory_tree').tree('getParent', node_add.target);
                switch (node_add.Isparent) {
                    case "True":
                        id = node_add.id; break;
                    case "False":
                        id = parent.id; break;
                        defalut: break;
                }
                $('#mtr_datagrid').datagrid({
                    queryParams: {
                        MTRNO: $("#mtr_text").textbox("getText"),
                        TaskID: id,
                        type: "2"
                    },
                    url: '/SampleManagement/GetTaskMotorList'
                });
            }
        }
    });
}
//提交所有归还样品
function sumbit_sample() {
    var rowss = $("#Sample_datagrid").datagrid("getData");
    if (rowss.total != 0) {
        $.ajax({
            url: '/SampleManagement/SampleReturnsave',
            type: 'POST',
            dataType: "json",
            data: {
                dataGridData: JSON.stringify(rowss)
            },
            success: function (data) {
                if (data.Success == true) {
                    var json = {
                        "rows": [],
                        "total": 0,
                        "success": true
                    };
                    $('#Sample_datagrid').datagrid("loadData", json);
                    ArrList = [];
                    $('#search_text').textbox("setValue", "");
                } else if (data.Success == false) {
                    $.messager.alert('Tip', data.Message);
                }
            }
        });
    } else {
        $.messager.alert('Tip', "The has not sample to sumbit！");
    }
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
                            TakeOutSignatureName: data.Data.TakeOutSignatureName,
                            TakeOutDate: data.Data.TakeOutDate,
                            ReturnSignatureName: data.Data.ReturnSignatureName,
                            ReturnDate: data.Data.ReturnDate,
                            SamplePosition: data.Data.AddressId_n,
                            Remark: data.Data.Remark,
                            Status: data.Data.Status
                        });
                        $.messager.alert('Tip', "Operate Success!");
                    } else {
                        $.messager.alert('Tip', "The SampleNo already exists!");

                    };
                    $("#search_text").textbox("setValue", "");
                    $("#search_text").textbox('textbox').focus();//聚焦样品文本框
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
                    save_edit(selected);
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
        $("#Status").textbox("setValue", selected.Status);
        $("#Remark").textbox("setValue", selected.Remark);
    } else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }

};
/*
*functionName:确认修改样品
*function:save_edit
*Param: 
*author:程媛
*date:2018-05-16
*/
function save_edit(selected) {
    var Status = $("#Status").textbox("getText");
    var Remark = $("#Remark").textbox("getText");
    $('#Sample_datagrid').datagrid('updateRow', {
        index: $("#Sample_datagrid").datagrid("getRowIndex"),
        row: {
            MTRNO: selected.MTRNO,
            SampleCode: selected.SampleCode,
            MotoNum: selected.MotoNum,
            SampleQty: selected.SampleQty,
            Identification: selected.Identification,
            Appearance: selected.Appearance,
            OthersStatus: selected.OthersStatus,
            TakeOutSignatureName: selected.TakeOutSignatureName,
            TakeOutDate: selected.TakeOutDate,
            ReturnSignatureName: selected.ReturnSignatureName,
            ReturnDate: selected.ReturnDate,
            Remark: Remark,
            Status: Status
        }
    });
    $.messager.alert('Tip', "Operate Success!");
    $('#edit_dialog').dialog('close');//关闭弹窗
};
/*
*functionName:删除样品信息
*function:del_sample
*Param: SampleCode
*author:程媛
*date:2018-05-16
Test005-2#
*/
function del_sample() {
    var selectRow = $("#Sample_datagrid").datagrid("getSelected");
    if (selectRow) {
        $.messager.confirm('Tips', 'Are you sure you want to delete the line？', function (r) {
            if (r) {
                var rowIndex = $('#Sample_datagrid').datagrid('getRowIndex', selectRow);
                $('#Sample_datagrid').datagrid('deleteRow', rowIndex);
                ArrList.splice(rowIndex);
            }
        });
        $.messager.alert('Tip', "Operate Success!");
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};