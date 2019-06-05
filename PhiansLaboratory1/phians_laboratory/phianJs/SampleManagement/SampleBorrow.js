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
        columns: [[
            { field: 'MTRNO', title: 'MTR NO.', sortable: 'true', width: 100, align: 'center' },
            { field: 'SampleCode', title: 'Sample Code', width: 100, align: 'center' },//样品编号
             { field: 'SamplePosition', title: 'Address', width: 100, },//取出签名
            { field: 'TakeOutSignatureName', title: 'TakeOut Signature', width: 150 ,hidden:true },//取出签名
            {
                field: 'TakeOutDate', title: 'TakeOut Date', width: 150, formatter: function (value, row, index) {//取出日期

                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                },hidden:true
            },
            { field: 'ReturnSignatureName', title: 'Return Signature', width: 150, hidden: true },//返回签名
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
            }, {
                field: 'Remark', title: 'Remark', align: 'center', width: 150, formatter: function (value, row, index) {
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
        var SampleCode = $("#sample_text").textbox("getValue");
        if (SampleCode != "") {
            append_sample();
        } else {
            $.messager.alert('Tips', "Sample number cannot be empty！");
        }
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
*functionName:提交所有归还样品
*function:sumbit_sample
*Param: dataGridData
*author:程媛
*date:2018-05-17
*/
function sumbit_sample() {
    var rowss = $("#Sample_datagrid").datagrid("getData");
    if (rowss.total != 0) {
        $.ajax({
            url: '/SampleManagement/SampleBorrowsave',
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
                    
                    $.messager.alert('Tip', "Operate Success!");
                } else if (data.Success == false) {
                    $.messager.alert('Tip', data.Message);
                }
            }
        });
    } else {
        $.messager.alert('Tip', "The has not sample to submit！");
    }
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
                        id = parent.id;break;
                        defalut: break;
                }
                $('#mtr_datagrid').datagrid({
                    queryParams: {
                        MTRNO: $("#mtr_text").textbox("getText"),
                        TaskID: id,
                        type:"1"
                    },
                    url: '/SampleManagement/GetTaskMotorList'
                });
            }
           
        }
    });
}
//加载列表
function mtr_datagrid() {
    $("#mtr_datagrid").datagrid({
        nowrap: false,
        striped: true,
        singleSelect: true,
        border: false,
       // fitColumns: true,
        fit: true,
        pagination: false,
       // pageSize: 40,
       // pageNumber: 1,
        type: 'POST',
        dataType: 'json',
        url: "",
        columns: [[
            { field: 'MTRNO', title: 'MTR NO.', sortable: 'true' },
            { field: 'MotoNum', title: 'Sample Code' },//样品编号
             { field: 'AddressId_n', title: 'Address', width: 100 },//样品位置
            
            { field: 'TakeOutSignatureName', title: 'TakeOut Signature', hidden: true },//取出签名
              { field: 'SamplePosition', title: 'SamplePosition', hidden: true },//取出签名
            {
                field: 'TakeOutDate', title: 'TakeOut Date', width: 150, formatter: function (value, row, index) {//取出日期

                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
                , hidden: true
                
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
*date:2018-05-1
*/
function save_choose_mrt() {
   // var SampleCode = $("#sample_text").textbox("getText");
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
                        ReturnSignatureName: rowss[i].ReturnSignatureName,
                        ReturnDate: rowss[i].ReturnDate,
                        Remark: rowss[i].Remark,
                        Status: rowss[i].Status,
                        SamplePosition: rowss[i].AddressId_n
                    });
                }
            }
        }
      //  $("#mtr_text").textbox("setValue", "");
        $("#mtr_text").textbox('textbox').focus();//聚焦样品文本框
    } else {
        $.messager.alert('Tip', "Please select the row to be operated！");
    };
    
};

/*
*functionName:追加样品信息
*function:append_sample
*Param: SampleCode
*author:程媛
*date:2018-05-16
*/
function append_sample() {
    var SampleCode = $("#sample_text").textbox("getText");
    var SampleRemark = $("#sample_remark").textbox("getText");
    $.ajax({
        url: '/SampleManagement/GetMotorInfoBorrow',
        type: 'POST',
        dataType: "json",
        data: {
            SampleCode: SampleCode,
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
                            ReturnSignatureName: data.Data.ReturnSignatureName,
                            ReturnDate: data.Data.ReturnDate,
                            Remark: SampleRemark,
                            Status: data.Data.Status,
                            SamplePosition: data.Data.SamplePosition,
                        });
                        $.messager.alert('Tip', "Operate Success!");
                    } else {
                        $.messager.alert('Tip', "The SampleNo already exists!");
                    };
                    $("#sample_text").textbox("setValue", "");
                    $("#sample_text").textbox('textbox').focus();//聚焦样品文本框
                }
                else if (data.Success == false) {
                    $.messager.alert('Tip', data.Message);
                }
            }
        }
    });
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
                $.messager.alert('Tip', "Operate Success!");
            }
        });
    }
    else {
        $.messager.alert('Tips', 'Please select the row to be operated！');
    }
};