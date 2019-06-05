/// <reference path="E:\计量理化\理化项目\phians\mainform/Lossless_report/LookPicture.html" />
/// <reference path="E:\计量理化\理化项目\phians\mainform/Lossless_report/LookPicture.html" />
var line = 0;
var WordUrlSpit = new Array();
var WordUrlSpit1 = new Array();
var WordUrlSpit_Read = new Array();
var word_link;
//加载信息
$(function () {
    word_link = $("#reports_edit_1").attr("href");//获取a链接 URL
    WordUrlSpit = word_link.split("?");

    word_link1 = $("#WordRevision").attr("href");//获取office控件链接 URL
    WordUrlSpit1 = word_link1.split("?");

    word_link2 = $("#WordRead").attr("href");//获取office控件链接 URL
    WordUrlSpit_Read = word_link2.split("?");
    //搜索
    $('#ReportType').combobox({
        value: '产品理化试验报告',
        data: [
                { 'value': '产品理化试验报告', 'text': '产品制造检验报告' },
                { 'value': '工艺评定理化试验报告', 'text': '工艺评定理化试验报告' },
                { 'value': '工艺试验检验报告', 'text': '工艺试验检验报告' },
                { 'value': '焊材验收检验报告', 'text': '焊材验收检验报告' },
                { 'value': '原材料验收检验报告', 'text': '原材料验收检验报告' },
                { 'value': '焊工考试检验报告', 'text': '焊工考试检验报告' },
                { 'value': '工艺临时更改通知单检验报告', 'text': '工艺临时更改通知单检验报告' }
        ]
    });

    $('#TemplateChoose').combobox({
        url: "/ReportManagement/LoadTemplateCombobox",
        valueField: 'Value',
        textField: 'Text',
        required: true,
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });

    //搜索
    $('#search').combobox({
        value: 'report_num',
        data: [
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                //{ 'value': '', 'text': '报告类型' },
                { 'value': 'Project_name', 'text': '产品（项目名称）' },
                //{ 'value': '', 'text': '零件' },
                { 'value': 'Subassembly_name', 'text': '部件' },
                { 'value': 'clientele_department', 'text': '委托部门' },
                { 'value': 'contract_num', 'text': '委托编号' }
        ]
    });
});

//检测报告编制-报告信息
$(function () {
    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    //显示分页条数
    var _height1 = window.screen.height - 400;;
    var num = parseInt(_height1 / 25);
    //检测报告信息
    //待处理和已处理切换
    management_all();
    //加载列表
    load_list();

    //搜索
    $("#search_info").unbind("click").bind("click", function () {
        search();
    });

    ////预览报告模板
    //$('#view_word_temp').unbind('click').bind('click', function () {
    //    view_report_model();
    //});
   
    //查看附件
    $('#read_report_info').unbind('click').bind('click', function () {
        read_report_info();
    });
    //复制报告
    $('#copy_info').unbind('click').bind('click', function () {
        copy_info();
    });

    //写入报告信息
    $('#edit_info').unbind("click").bind("click", function () {
        edit_info();
    });
    //写入检测数据
    $('#WriteTestData').unbind("click").bind("click", function () {
        WriteTestData();
    });
    //修改报告信息
    $('#edit_report_info').unbind("click").bind("click", function () {
        edit_report_info();
    });
    //删除信息
    $('#DataDel').unbind("click").bind("click", function () {
        DataDel();
    });
    //下载报告
    $('#download_info').unbind('click').bind('click', function () {
        download_info();
    });
    //载入报告
    $('#load_Report').unbind('click').bind('click', function () {
        load_report_model();
    });
    //检测报告在线编辑
    $('#Edit_online_report').unbind('click').bind('click', function () {
        edit_online();
    });
    //附件添加到报告
    $('#AddAccessory').unbind('click').bind('click', function () {
        AddAccessory();
    });
    //删除报告
    $('#Delete_certificate_file').unbind('click').bind('click', function () {
        delete_report_file();
    });

    //提交审核报告
    $('#submit_review_Report').unbind('click').bind('click', function () {
        submit_report();
    });
    //添加附件信息
    $('#add_read').unbind('click').bind('click', function () {
        add_read();
    });
    //删除附件信息
    $('#del_read').unbind('click').bind('click', function () {
        del_read();
    });
    //查看附件信息
    $('#read_read').unbind('click').bind('click', function () {
        //判断是否是图片
        var getSelected = $('#read_table').datagrid('getSelected');
        if (getSelected) {
            if (getSelected.accessory_format == ".jpg" || getSelected.accessory_format == ".jpeg" || getSelected.accessory_format == ".GPEG" || getSelected.accessory_format == ".PNG" || getSelected.accessory_format == ".png") {
                read_pic();
            } else if (getSelected.accessory_format == ".pdf") {
                view_pdf();
            } else if (getSelected.accessory_format == ".zip") {
                $.messager.alert('提示', 'zip压缩文件只能下载不能查看');
            } else {
                $.messager.alert('提示', '不是png,jpg或是pdf格式的不能打开');
            }
        } else {
            $.messager.alert('提示', '请选择你要操作得行');
        }

        //判断是否是文件
    });
    //下载附件信息
    $('#download_read').unbind('click').bind('click', function () {
        download_read();
    });
    //批量下载附件信息
    $('#downloads_read').unbind('click').bind('click', function () {
        downloads_read();
    });

    //查看退回信息
    $('#read_return_info').unbind('click').bind('click', function () {
        back_report();
    });
    //查看退回原因
    $('#return_reason').unbind('click').bind('click', function () {
        write_method_equipment();
    });
    //产生已提交的记录文档
    $('#read_document').unbind('click').bind('click', function () {
        readRecord();
    });
    //历史编制预览报告
    $('#Preview_Report').unbind('click').bind('click', function () {
        read_report1();
    });

    //需要复制报告
    $('#copy_flag1').unbind('click').bind('click', function () {
        $("#copy_flag1").prop("checked", "checked");
        $("#copy_flag2").prop("checked", false);
        $("#copy_flag").val("1");
    });
    //不需要复制报告
    $('#copy_flag2').unbind('click').bind('click', function () {
        $("#copy_flag2").prop("checked", "checked");
        $("#copy_flag1").prop("checked", false);
        $("#copy_flag").val("0");
    });
    //历史审核退回编制
    $('#returnHistoryReport').unbind('click').bind('click', function () {
        returnHistoryReport();
    });

});
//历史审核退回审核
function returnHistoryReport() {
    var select = $('#report_edit_task').datagrid("getSelected");
    if (select) {
        $.messager.confirm('提示', '您确认要退回编制吗', function (r) {
            if (r) {
                $.ajax({
                    url: "/ReportManagement/TakeBackEditReport",
                    type: 'POST',
                    data: {
                        id: select.id,
                        Inspection_personnel: select.Inspection_personnel
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $('#report_edit_task').datagrid('reload');

                        } else {
                            $.messager.alert('提示', result.Message);
                        }

                    }
                });
            }
        });
    } else {
        $.messager.alert('提示', "请选择操作行！");
    }

}
function load_list() {
    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    //显示分页条数
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    var search = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    var history_flag = $('#management_all').combobox('getValue');
    //按钮显示隐藏
    //按钮显示隐藏
    if (history_flag == "0") {
        $(".link_button").css("display", "block");
        $(".history_link_button").css("display", "none");

    } else {
        $(".link_button").css("display", "none");
        $(".history_link_button").css("display", "block");
    }
    $.ajax({
        url: "/ReportManagement/loadPageShowSetting",
        type: 'POST',
        data: {
            PageId: '101',
        },
        success: function (result) {
            if (result) {
                // console.log(result);
                $('#report_edit_task').datagrid(
                    {
                        nowrap: false,
                        striped: true,
                        //rownumbers: true,
                        singleSelect: true,
                        //autoRowHeight: true,
                        border: false,
                        fitColumns: true,
                        rownumbers: true,
                        fit: true,
                        pagination: true,
                        pageSize: num,
                        pageList: [num, num + 10, num + 20, num + 30],
                        pageNumber: 1,
                        type: 'POST',
                        dataType: "json",
                        url: "/ReportManagement/LoadReportEditList", //接收一般处理程序返回来的json数据        
                        columns: [[
                            { field: 'id', title: '序号', hidden: false, sortable: false, width: 100 },
                            { field: 'report_num', title: '总报告编号', hidden: true, sortable: false, width: 100 },
                            { field: 'report_name', title: '报告名称', hidden: true, sortable: false, width: 100 },
                            { field: 'clientele_department', title: '用户(委托部门)', hidden: false, sortable: false, width: 100 },
                            { field: 'clientele', title: '委托人', hidden: false, sortable: false, width: 100 },
                            { field: 'application_num', title: '订单号', hidden: false, sortable: false, width: 100 },
                            { field: 'Project_name', title: '项目名称', hidden: false, sortable: false, width: 100 },
                            { field: 'Subassembly_name', title: '部件名称', hidden: false, sortable: false, width: 100 },
                            { field: 'Material', title: '材质', hidden: false, sortable: false, width: 100 },
                            { field: 'Type_', title: '规    格', hidden: false, sortable: false, width: 100 },
                            { field: 'Chamfer_type', title: '坡口型式', hidden: false, sortable: false, width: 100 },
                            { field: 'Drawing_num', title: '图号', hidden: false, sortable: false, width: 100 },
                            { field: 'Procedure_', title: '检验规程', hidden: false, sortable: false, width: 100 },
                            { field: 'Inspection_context', title: '检验内容', hidden: false, sortable: false, width: 100 },
                            { field: 'Inspection_opportunity', title: '检验时机', hidden: false, sortable: false, width: 100 },
                            { field: 'circulation_NO', title: '流转卡号', hidden: true, sortable: false, width: 100 },
                            { field: 'procedure_NO', title: '工序号', hidden: false, sortable: false, width: 100 },
                            { field: 'apparent_condition', title: '表面状态', hidden: false, sortable: false, width: 100 },
                            { field: 'manufacturing_process', title: '制造工艺', hidden: false, sortable: false, width: 100 },
                            { field: 'Batch_Num', title: '批次号', hidden: false, sortable: false, width: 100 },
                            { field: 'Inspection_NO', title: '检验编号', hidden: false, sortable: false, width: 100 },
                            {
                                field: 'Inspection_date', title: '检验日期', hidden: true, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length >= 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }, width: 100
                            },
                            {
                                field: 'Inspection_personnel_date', title: '检验人签字时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length >= 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }, width: 100
                            },
                            { field: 'Inspection_personnel_n', title: '检验人', hidden: false, sortable: false, width: 100 },
                            { field: 'level_Inspection', title: '检验级别', hidden: false, sortable: false, width: 100 },
                            { field: 'Audit_personnel_n', title: '审核人员', hidden: false, sortable: false, width: 100 },
                            { field: 'Audit_date', title: '审核时间', hidden: false, sortable: false, width: 100 },
                            { field: 'return_info', title: '退回原因', hidden: false, sortable: false, width: 100 },
                            { field: 'level_Audit', title: '审核级别', hidden: false, sortable: false, width: 100 },
                            { field: 'issue_personnel_n', title: '签发人员', hidden: false, sortable: false, width: 100 },
                            { field: 'issue_date', title: '签发时间', hidden: false, sortable: false, width: 100 },
                            { field: 'laboratorian_', title: '授权检验师', hidden: false, sortable: false, width: 100 },
                            {
                                field: 'laboratorian_date', title: '授权检验师时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length >= 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }, width: 100
                            },
                            { field: 'figure', title: '附图', hidden: false, sortable: false, width: 100 },
                            { field: 'disable_report_num', title: '不符合项报告号', hidden: false, sortable: false, width: 100 },
                            { field: 'report_format', title: '报告格式', hidden: false, sortable: false, width: 100 },
                            {
                                field: 'Inspection_result', title: '检验结果', hidden: false, sortable: false, width: 100,
                                formatter: function (value, row, index) {
                                    if (value == '1') { return "合格" } if (value == '0') { return "不合格" }
                                }
                            },
                            {
                                field: 'state_', title: '状态', hidden: 'true', sortable: 'false', width: 100,
                                formatter: function (value, row, index) {
                                    if (value == "1") { return "编辑"; } if (value == "2") { return "审核"; } if (value == "3") { return "签发"; } if (value == "4") { return "完成"; }
                                }
                            },
                            { field: 'Tubes_Size', title: '管子规格', hidden: false, sortable: false, width: 100 },
                            { field: 'Tubes_num', title: '管子数量', hidden: false, sortable: false, width: 100 },
                            { field: 'welding_method', title: '焊接方法', hidden: false, sortable: false, width: 100 },
                            { field: 'Job_num', title: '工号', hidden: true, sortable: false, width: 100 },
                            { field: 'heat_treatment', title: '热处理设备', hidden: false, sortable: false, width: 100 },
                            { field: 'Work_instruction', title: '作业指导书', hidden: false, sortable: false, width: 100 },
                            { field: 'ReportCreationTime', title: '报告创建时间', hidden: false, sortable: false, width: 100 },
                            { field: 'remarks', title: '备注', hidden: true, sortable: false, width: 100 },

                        ]],
                        queryParams: {
                            search: search,
                            key: key,
                            history_flag: history_flag
                        },
                        onBeforeLoad: function () {
                            var hidden = $.parseJSON(result);
                            var hiddenJson = $.parseJSON(hidden.Message);
                            for (var i = 0; i < hiddenJson.length; i++) {
                                try {
                                    console.log(hiddenJson[i].hidden);
                                    if (hiddenJson[i].hidden) {
                                        $('#report_edit_task').datagrid('hideColumn', hiddenJson[i].fieldname);
                                    } else {
                                        $('#report_edit_task').datagrid('showColumn', hiddenJson[i].fieldname);
                                    }
                                } catch (e) {

                                }

                            }
                        },
                        onLoadSuccess: function (data) {

                            //默认选择行
                            $('#report_edit_task').datagrid('selectRow', line);
                            var selectRow = $("#report_edit_task").datagrid("getSelected");
                            if (!selectRow) {
                                $('#report_edit_task').datagrid('selectRow', 0);
                            }
                        },
                        onSelect: function (rowIndex, rowData) {
                         
                        },
                        rowStyler: function (index, row) {
                            if (row.return_flag == "1") { return 'background-color:#ff6347;'; }
                           
                           
                        },
                        sortOrder: 'asc',
                        toolbar: reports_toolbar
                    });
            }
            else {
                //  $.messager.alert('提示', data);
            }

        }

    });


}
//预览历史编制报告只读模式打开
function read_report1() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {

        let cookie_val = getCookie("UserCount");

        $("#WordRead").prop("href", WordUrlSpit_Read[0] + "?id=" + selected_report.id + "&OperateType=Report&UserCount_=" + cookie_val + WordUrlSpit_Read[1]);
        document.getElementById('WordRead').click();

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');
    }
}
//搜索
function search() {
    $('#report_edit_task').datagrid(
        {
            url: "/ReportManagement/LoadReportEditList",//接收一般处理程序返回来的json数据    
            queryParams: {
                search: $("#search").combobox('getValue'),
                key: $("#search1").textbox('getText'),
                history_flag: $('#management_all').combobox('getValue')
            },
            onLoadSuccess: function (data) {
                $('#report_edit_task').datagrid('selectRow', 0);
            }
        }).datagrid('resize');
}
//下载报告
function download_info() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");

    if (selected_report) {

        DownloadFile(selected_report.report_url, selected_report.report_num, ".doc");
    } else {
        $.messager.alert('提示', '请选择操作行！');
    }
}
//查看原因
function write_method_equipment() {
    //显示分页条数
    var _height1 = window.screen.height - 400;;
    var num = parseInt(_height1 / 25);
    var selectRow = $('#report_edit_task').datagrid('getSelected');
    if (selectRow) {
        $('#Standard_equipment_info').dialog({
            width: 800,
            height: 550,
            modal: true,
            title: '报告退回原因',
            draggable: true,
            buttons: [{
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Standard_equipment_info').dialog('close');
                }
            }]
        });
        $('#Standard_equipment_info').form('reset');
        $("#return_info1").textbox("setText", selectRow.return_info);
        //已邮报告原因设备
        $('#Standard_verification1').datagrid({
            nowrap: false,
            striped: true,
            //rownumbers: true,
            singleSelect: true,
            border: false,
            fitColumns: true,
            // fit: true,
            pagination: true,
            height: 400,
            pageSize: num,
            pageList: [num, num + 10, num + 20],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            queryParams: {
                id: selectRow.id
            },
            url: "/ReportManagement/LoadErrorInfo",//接收一般处理程序返回来的json数据 
            columns: [[
                { field: 'error_remarks', title: '错误信息', width: 100 },
               { field: 'addpersonnel_n', title: '发现人', width: 100 },
                { field: 'add_date', title: '发现时间', width: 100,formatter: function (value, row, index) {
                    if (value) {
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                } },
                  { field: 'ReturnNode', title: '返回环节', width: 100 }
            ]],
            onLoadSuccess: function (data) {
                $('#Standard_verification1').datagrid('selectRow', 0);
            }
        });
    } else {
        $.messager.alert('提示', '请选择你要操作得行');
    }
}
//查看记录文档
function readRecord() {
    var selectRow = $('#report_edit_task').datagrid('getSelected');
    if (selectRow) {
        $('#ReadRecord_info').dialog({
            width: 800,
            height: 500,
            modal: true,
            title: '记录文档',
            draggable: true,
            buttons: [{
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#ReadRecord_info').dialog('close');
                }
            }]
        });
        //已邮报告原因设备
        $('#ReadRecord').datagrid({
            nowrap: false,
            striped: true,
            //rownumbers: true,
            singleSelect: true,
            border: false,
            fitColumns: true,
            fit: true,
            pagination: true,
            pageSize: 10,
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            queryParams: {
                id: selectRow.id
            },
            url: "/ReportManagement/ReadRecord",//接收一般处理程序返回来的json数据 
            columns: [[
                { field: 'report_id', title: '报告id', width: 100 },
               { field: 'addpersonnel_n', title: '修改人', width: 100 },
                {
                    field: 'add_date', title: '修改时间', width: 100, formatter: function (value, row, index) {
                        if (value) {
                            if (value.length >= 10) {
                                value = value.substr(0, 10)
                                return value;
                            }
                        }
                    }
                },
                  { field: 'ReturnNode', title: '返回环节', width: 100 }
            ]],
            onLoadSuccess: function (data) {
                $('#ReadRecord').datagrid('selectRow', 0);
            },
            toolbar: ReadRecord_toolbar
        });
    } else {
        $.messager.alert('提示', '请选择你要操作得行');
    }
    //记录文档报告查看
    $('#ReadRecord_Preview_Report').unbind('click').bind('click', function () {
        view_report1();
    });
}
//查看报告记录痕迹只读模式
function view_report1() {
    var selected_report = $("#ReadRecord").datagrid("getSelected");
    if (selected_report) {
        let cookie_val = getCookie("UserCount");
        $("#WordRevision").prop("href", WordUrlSpit1[0] + "?id=" + selected_report.id + "&OperateType=Report&UserCount_=" + cookie_val + WordUrlSpit1[1]);
        document.getElementById('WordRevision').click();

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');
    }
}





//退回报告
function back_report() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    if (selectRow) {
        line = $('#report_edit_task').datagrid("getRowIndex", selectRow);
        $('#Back_report_dialog').form("reset");
        $('#return_accessory').datagrid(
        {
            nowrap: false,
            striped: true,
            rownumbers: true,
            singleSelect: true,
            autoRowHeight: true,
            border: false,
            //fitColumns: true,
            width: 700,
            height: 400,
            pagination: true,
            pageSize: 10,
            pageList: [5, 10, 15, 20],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            queryParams: {
                report_id: selectRow.id
            },
            url: "/ReportManagement/load_accessory_list",//接收一般处理程序返回来的json数据        
            columns: [[
             //{ field: 'id', title: 'id', sortable: 'true' },
             //{ field: 'report_id', title: '报告id', sortable: 'true' },
             { field: 'Picture_name', title: '图片名称' },
             { field: 'Picture_format', title: '图片格式' },
             //{ field: 'Picture_url', title: '图片url' },
             { field: 'Add_personnel_n', title: '操作人员' },
             {
                 field: 'Add_time', title: '操作时间', formatter: function (value, row, index) {
                     if (value) {
                         if (value.length > 10) {
                             value = value.substr(0, 10)
                             return value;
                         }
                     }
                 }
             },
             { field: 'return_type', title: '返回类型' },
             { field: 'remarks', title: '附注' }
            ]],
            onLoadSuccess: function (data) {
                $('#return_accessory').datagrid('selectRow', 0);
            },
            sortOrder: 'asc',
            toolbar: "#return_accessory_toolbar"
        });

        $('#Back_report_dialog').dialog({
            width: 720,
            height: 475,
            modal: true,
            title: '退回信息',
            draggable: true,
            buttons: [{
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Back_report_dialog').dialog('close');
                }
            }]
        });

        //查看图片
        //$('#read_read').unbind('click').bind('click', function () {

        //    var getSelected = $('#return_accessory').datagrid('getSelected');
        //    if (getSelected) {
        //        var rowss = $('#return_accessory').datagrid('getSelections');
        //        $('#read_read').attr("target", "_blank");
        //        $('#read_read').attr("href", "/mainform/Lossless_report/LookPicture.html?src='" + rowss[0].Picture_url + "'");
        //        $("#Picture_img").attr("src", "../.." + rowss[0].Picture_url);
        //        $('#Picture_form').dialog({
        //            width: 400,
        //            height: 300,
        //            top: 40,
        //            modal: true,
        //            title: '图片',
        //            draggable: true,
        //            buttons: [{
        //                text: '关闭',
        //                iconCls: 'icon-cancel',
        //                handler: function () {
        //                    $('#Picture_form').dialog('close');
        //                }
        //            }]
        //        });
        //    } else {
        //        $.messager.alert('提示', '请选择要操作的行！');
        //    }

        //})

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}

//查看附件
function read_report_info() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    if (selectRow) {
        $('#read_dialog').dialog({
            width: 800,
            height: 475,
            modal: true,
            title: '附件信息',
            draggable: true,
            buttons: [{
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#read_dialog').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('提示', '请选择要操作的行！');
    }

    //<%--附件信息datagrid--%>
    $('#read_table').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        autoRowHeight: true,
        fitColumns: true,
        fit: true,
        border: false,
        fit: true,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 15, 20],
        pageNumber: 1,
        type: 'POST',
        queryParams: {
            report_id: selectRow.id
        },
        dataType: "json",
        url: "/ReportManagement/load_accessory",//接收一般处理程序返回来的json数据        
        columns: [[
       { field: 'report_id', title: '报告id', sortable: 'true', width: 100 },
       { field: 'accessory_name', title: '附件名称', width: 100 },
       { field: 'accessory_format', title: '文件格式', width: 100 },
       { field: 'add_personnel', title: '添加人', width: 100 },
       { field: 'add_date', title: '添加时间', width: 100 },
       { field: 'remarks  ', title: '说明', width: 100 }
        ]],
        onLoadSuccess: function (data) {
            $('#read_table').datagrid('selectRow', 0);

        },
        onSelect: function () {
            //提示
            //$('#test_records_Sample_table').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
        },
        rowStyler: function (index, row) {

        },
        toolbar: "#read_toolbar"
    });


}

//添加附件信息
function add_read() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    $('#add_read_dialog').dialog({
        title: '添加附件信息',
        width: 500,
        height: 300,
        top: 100,
        buttons: [{
            text: '提交',
            iconCls: 'icon-ok',
            handler: function () {
                save(selectRow);
            }

        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#add_read_dialog').dialog('close');
            }
        }]

    });
}
function save(node_add) {
    $('#add_read_dialog').form('submit', {
        url: "/ReportManagement/upload_accessory",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            param.report_id = node_add.id;
            param.report_num = node_add.report_num;
        },

        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#add_read_dialog').dialog('close');
                $('#read_table').datagrid('reload');
                $.messager.alert('提示', result.Message);

            }
            else {
                $.messager.alert('提示', result.Message);
            }
        }
    });

}
//查看图片
function read_pic() {
    var getSelected = $('#read_table').datagrid('getSelected');
    if (getSelected) {
        var rowss = $('#read_table').datagrid('getSelections');
        $("#Picture_img").attr("src", "../.." + rowss[0].accessory_url);
        $('#Picture_form').dialog({
            width: 600,
            height: 600,
            top: 40,
            modal: true,
            title: '图片',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Picture_form').dialog('close');
                }
            }]
        });
    } else {
        $.messager.alert('提示', '请选择要操作的行！');
    }

}
//预览附件pdf
function view_pdf() {
    var selected_report = $("#read_table").datagrid("getSelected");
    if (selected_report) {
        var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + selected_report.accessory_url;
        window.parent.addTab(selected_report.report_num + "报告文件查看", url_);
    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');
    }
}
//下载附件
function download_read() {
    var selected_report = $("#read_table").datagrid("getSelected");
    if (selected_report) {
        DownloadFile(selected_report.accessory_url, selected_report.accessory_name, selected_report.accessory_format);
    } else {
        $.messager.alert('提示', '请选择操作行！');
    }
}
//批量下载附件

function downloads_read() {
    var selected_report = $("#read_table").datagrid("getSelected");
    var rowss = $("#read_table").datagrid("getSelections");

    if (selected_report) {
        var id1 = [];
        for (var i = 0; i < rowss.length; i++) {
            id1.push(rowss[i].id);
        }
        var ids = id1.join(",");
        $.ajax({
            url: "/ReportManagement/downloadAccessory",
            dataType: "text",
            type: 'POST',
            data: {
                ids: ids,
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    window.location.href = data;
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    } else {
        $.messager.alert('提示', '请选择操作行！');
    }
}
//删除附件信息
function del_read() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    var selected_report = $("#read_table").datagrid("getSelected");
    if (selected_report) {
        $.messager.confirm('删除任务提示', '您确认要删除选中信息吗', function (r) {
            if (r) {
                $.ajax({
                    url: "/ReportManagement/DelAccessory",
                    type: 'POST',
                    data: {
                        report_num: selectRow.report_num,
                        report_id: selectRow.id,
                        id: selected_report.id
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#read_table').datagrid('reload');
                            $.messager.alert('Tips', result.Message);
                        }
                        else {
                            $.messager.alert('Tips', result.Message);
                        }
                    }
                });
            }
        })

    } else {
        $.messager.alert('提示', '请选择报告');
    }

}
//===================================================
//添加报告
function edit_info() {
    $("#TemplateChoose1").combobox({ "disabled": false });
    $('#add_reports_info').form("reset");
    $("#copy_add").css('display', 'none');
    $("#TemplateChoose1").combobox("select", "ECT-涡流检验报告模板RPV")
    //$('#add_reports_info').form("reset");
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    $("#copy_add").css("display", "block");
    //if (selectRow) {
    //line = $('#report_edit_task').datagrid("getRowIndex", selectRow);

   


    $('#add_reports_info').dialog({
        width: 670,
        height: 600,
        modal: true,
        title: '添加报告',
        border: false,
        buttons: [{
            text: '获取规程和订单信息',
            iconCls: 'icon-ok',
            handler: function () {
                Method_info();
                order_information();
            }
        },
            //{
            //    text: '订单信息',
            //    iconCls: 'icon-ok',
            //    handler: function () {
            //        order_information();
            //    }
            //},
            {
                text: '预览报告模板',
                iconCls: 'icon-ok',
                handler: function () {
                    view_word_temp();
                }
            }, {
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    editInfoClick();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#add_reports_info').dialog('close');
                }
            }]
    });
    $("#add_reports_info").form("reset");
    $(".test :text").attr("disabled", false);
    //获取mess
    $("#process").unbind("click").bind("click", function () {
        if ($('#circulation_NO').textbox('getText') == "") {
            $.messager.alert('提示', '流转卡号不能为空');
            return;
        }
        if ($('#procedure_NO').textbox('getText') == "") {
            $.messager.alert('提示', '工序号不能为空');
            return;
        }
        $.ajax({
            //  url: "/ReportManagement/AddReportBaseInfo",        
            type: 'POST',
            data: {
                circulation_NO: $('#circulation_NO').textbox('getText'),
                procedure_NO: $('#procedure_NO').textbox('getText')
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    var result = data.split("&");
                    $('#application_num').textbox('setText', result[0]);

                    $('#Subassembly_name').textbox('setText', result[1]);
                    $('#Inspection_context').textbox('setText', result[2]);
                }
                else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        });
    });
    //工号失去焦点的时候调取订单信息接口
    //$('#Job_num').textbox({
    //    onChange: function (value) {
    //        var Job_num = $('#Job_num').textbox('getText');
    //        if (Job_num != "") {
    //            $.ajax({
    //                url: "/ReportManagement/GetOrderNUM",
    //                dataType: "text",
    //                type: 'POST',
    //                data: {
    //                    OrderNUM: $('#Job_num').textbox('getText')
    //                },
    //                success: function (data) {
    //                    var result = $.parseJSON(data);
    //                    if (result.Success == true) {
    //                        $('#application_num').textbox('setText', data);
    //                        $('#application_num').textbox().focus();
    //                    }
    //                    else {
    //                        $.messager.alert('Tips', result.Message);
    //                    }


    //                }
    //            });
    //        }
    //    }
    //});

    //获取报告编号
    $("#ReportCodingLogic").unbind("click").bind("click", function () {
        $.ajax({
            url: "/ReportManagement/ReportCodingLogic",
            dataType: "text",
            type: 'POST',
            data: {
                ReportType: $('#ReportType').combobox('getValue'),
                circulation_NO: $('#circulation_NO').textbox('getText'),
                procedure_NO: $('#procedure_NO').textbox('getText')
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    $('#report_num').textbox('setText', result.Message);
                    $('#report_num').textbox().focus();
                } else {
                    $.messager.alert('提示', result.Message);
                }
            }
        });
    });

    //获取订单号信息
    function order_information() {
        if ($('#Job_num').textbox('getText') == "") {
            $.messager.alert('提示', '工号不能为空');
            return;
        }
        $.ajax({
            url: "/ReportManagement/GetOrderNUM",
            dataType: "json",
            type: 'POST',
            data: {
                OrderNUM: $('#Job_num').textbox('getText')
            },
            success: function (data) {
                if (data.Success == true) {
                    $('#application_num').textbox('setText', data.Message);
                    $('#application_num').textbox().focus();
                } else {
                    $.messager.alert('提示', data.Message);
                }
               

            }
        });
    }

    //获取规程号
    function Method_info() {
        var circulation_NO = $('#circulation_NO').textbox('getText');
        var procedure_NO = $('#procedure_NO').textbox('getText');

        if (circulation_NO == "") { return $.messager.alert('提示', '流程卡号不能为空！') }
        if (procedure_NO == "") { return $.messager.alert('提示', '工序号不能为空！') }

        $.ajax({
            url: "/ReportManagement/Getmethod",
            dataType: "json",
            type: 'POST',
            data: {
                circulation_NO: circulation_NO,
                procedure_NO: procedure_NO
            },
            success: function (data) {
                if (data.Success == true) {
                    $('#Procedure_').textbox('setText', data.Message);
                    $('#Procedure_').textbox().focus();
                 
                }
                else {
                    $.messager.alert('提示', data.Message);
                }

            }
        });

    }

    //获取MES信息
    $("#ReadDatagrid").unbind("click").bind("click", function () {
        $('#ReadDatagrid_dialog').dialog({
            width: 800,
            height: 475,
            modal: true,
            title: '获取MES信息',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    var row = $("#ReadDatagrid_info").datagrid("getSelected");
                    if (row) {

                        var context = (row.WORKID).split('-');
                        if (context.length == 4) {

                            $('#circulation_NO').textbox('setText', context[0] + "-" + context[1] + "-" + context[2]);
                            $('#circulation_NO').textbox().focus();
                            $('#procedure_NO').textbox('setText', context[3]);
                            $('#procedure_NO').textbox().focus();
                        }
                        //  $('#circulation_NO').textbox('setText', row.circulation_NO);
                        // $('#procedure_NO').textbox('setText', row.procedure_NO);
                        //$('#application_num').textbox('setText', row.APPLICATION_NUM);
                        //$('#application_num').textbox().focus();
                        $('#Subassembly_name').textbox('setText', row.SUBASSEMBLY_NAME);
                        $('#Subassembly_name').textbox().focus();
                        $('#Inspection_context').textbox('setText', row.INSPECTION_CONTEXT);
                        $('#Inspection_context').textbox().focus();
                        $('#Drawing_num').textbox('setText', row.DRAWING_NUM);
                        $('#Drawing_num').textbox().focus();
                        $('#ReadDatagrid_dialog').dialog('close');
                    }
                    else {


                        $.messager.alert("提示", "请选择行");
                    }

                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#ReadDatagrid_dialog').dialog('close');
                }
            }]
        });

        //<%--附件信息datagrid--%>
        $('#ReadDatagrid_info').datagrid({
            nowrap: false,
            striped: true,
            rownumbers: true,
            singleSelect: true,
            autoRowHeight: true,
            border: false,
            //fitColumns: true,
            fit: true,

            pagination: true,
            pageSize: 10,
            pageList: [5, 10, 15, 20],
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            url: "/ReportManagement/GET_MESlist",//接收一般处理程序返回来的json数据        
            columns: [[
           { field: 'WORKID', title: '流转卡号/ST+工序号', sortable: 'true' },
           //{ field: 'procedure_NO', title: '工序号' },
           { field: 'DRAWING_NUM', title: '图号' },
           { field: 'APPLICATION_NUM', title: '订单号' },
           { field: 'SUBASSEMBLY_NAME', title: '部件名称' },
           { field: 'INSPECTION_CONTEXT', title: '检验内容' }
            ]],
            onLoadSuccess: function (data) {
                if (data.success == false) {
                    $.messager.alert('提示', data.message);
                } else {
                    $('#ReadDatagrid_info').datagrid('selectRow', 0);
                }
            },
            onLoadError:function () {
                $.messager.alert('提示', '数据加载失败！');
            },
          
            toolbar: "#ReadDatagrid_toolbar"
        });
    });

    //搜索
    $('#searchs').combobox({
        value: 'circulation_NO',
        data: [
                { 'value': 'circulation_NO', 'text': '流转卡号/ST/工序号' },

        ]
    });
    $('#key').textbox({
        prompt: "请输入检索信息"
    });


    //搜索按钮
    $("#_search").unbind("click").bind("click", function () {
        $('#ReadDatagrid_info').datagrid(
            {
                url: "/ReportManagement/GET_MESlist",//接收一般处理程序返回来的json数据    
                queryParams: {
                    search: $("#searchs").combobox('getValue'),
                    key: $("#key").textbox('getText')
                },
                onLoadSuccess: function (data) {
                    $('#ReadDatagrid_info').datagrid('selectRow', 0);
                }
            }).datagrid('resize');

    })

}
//获取当前时间，格式YYYY-MM-DD
function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = year + seperator1 + month + seperator1 + strDate;
    return currentdate;
}
//复制报告
function copy_info() {
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    $('#copy_report_dialog').dialog({
        width: 750,
        height: 500,
        modal: true,
        title: '复制报告',
        border: false,
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                copy();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#copy_report_dialog').dialog('close');
            }
        }]
    });
    var search = $("#Copy_search1").combobox('getValue');
    var key = $("#Copy_key1").textbox('getText');
    var search1 = $("#Copy_search").combobox('getValue');
    var key1 = $("#Copy_key").textbox('getText');
    $("#copy_report_list").datagrid({
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        border: false,
        fitColumns: true,
        singleSelect: true,
        fit: true,
        pagination: true,
        pageSize: 10,
        pageList: [10, 20, 30, 40],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        checkOnSelect: false, //selectOnCheck: false,
        url: "/ReportManagement/loadReportCopy",//接收一般处理程序返回来的json数据        
        columns: [[
           { field: 'id', checkbox: "true" },
         { field: 'report_num', title: '总报告编号', sortable: 'true' },
         { field: 'report_name', title: '报告类别', sortable: 'true' },
         { field: 'application_num', title: '订单号', sortable: 'true' },
         { field: 'circulation_NO', title: '流转卡号' },
         { field: 'procedure_NO', title: '工序号' },
         { field: 'Job_num', title: '工号' },
         { field: 'clientele', title: '委托人' },
         { field: 'clientele_department', title: '委托部门' },
         { field: 'Project_name', title: '项目名称' },
         { field: 'Subassembly_name', title: '部件名称' },
         { field: 'Type_', title: '规格' },
         { field: 'Chamfer_type', title: '坡口型式' },
         {
             field: 'state_', title: '状态', formatter: function (value, row, index) {

                 switch (value) {
                     case '1': return "编辑";
                     case '2': return "审核";
                     case '3': return "签发";
                     case '4': return "完成";
                     default:

                 }

             }
         }
        ]],
        queryParams: {
            search: search,
            key: key,
            search1: search1,
            key1: key1,
        },
        onLoadSuccess: function (data) {
            //默认选择行
            $('#report_edit_task').datagrid('selectRow', 0);
        },
        onSelect: function () {
        },
        sortOrder: 'asc',
        toolbar: "#copy_report_list_toolbar"

    });
    function copy() {
        $("#copy_flag1").prop("checked", "checked")
        $("#copy_flag2").prop("checked", false);
        $("#copy_flag").val("1");
        var selectRow_num = $("#copy_report_list").datagrid("getSelected");
        var selected_report = $("#report_edit_task").datagrid("getSelected");
        if (selectRow_num) {
            $.ajax({
                url: "/ReportManagement/reportcopyshow",
                type: 'POST',
                data: {
                    report_num: selectRow_num.report_num
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    $("#copy_id").val(result.rows[0].id);
                    $("#add_reports_info").form('load', result.rows[0]);
                    $("#TemplateChoose1").combobox("select", selectRow_num.tm_id);
                    $("#Inspection_result").combobox("select", selectRow_num.Inspection_result);
                    $("#report_num").textbox("setText", "");
                    //检验日期回显
                    var NowFormatDate = getNowFormatDate();
                    $("#Inspection_date").datebox('setValue', NowFormatDate);
;                    $('#copy_report_dialog').dialog('close');
                }
            });
        } else {
            $.messager.alert('提示', '请选择要复制得报告');
        }
    }
    //搜索
    $('#Copy_search').combobox({
        data: [
               { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告名称' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                //{ 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' }
        ]
    });
    $('#Copy_key').textbox({
        prompt: "请输入检索信息"
    });
    $('#Copy_search1').combobox({
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告名称' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                //{ 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' }
        ]
    });
    $('#Copy_key1').textbox({
        prompt: "请输入检索信息"
    });
    //搜索按钮
    $("#_search2").unbind("click").bind("click", function () {
        $('#copy_report_list').datagrid(
            {
                url: "/ReportManagement/loadReportCopy",//接收一般处理程序返回来的json数据    
                queryParams: {
                    search: $("#Copy_search").combobox('getValue'),
                    key: $("#Copy_key").textbox('getText'),
                    search1: $("#Copy_search1").combobox('getValue'),
                    key1: $("#Copy_key1").textbox('getText')
                },
                onLoadSuccess: function (data) {
                    $('#copy_report_list').datagrid('selectRow', 0);
                }
            }).datagrid('resize');

    })
}

//确定修改
function editInfoClick() {
    //alert($("#copy_id").val());
    var selectRow = $("#report_edit_task").datagrid("getSelected");
    var tm_id = $("#TemplateChoose1").combobox("getValue");
    //var temp = TemplateChoose.split('.');
    //var report_format = "." + temp[1];
    //var tm_id = temp[0];
    var Template = $("#TemplateChoose1").combobox("getText");
    //var url = "/upload_Folder/Lossless_report/" + TemplateChoose;//+ ".docx";
    //form表单提交
    //if ($('#report_num').textbox('getText') == "") {
    //    $.messager.alert('提示', '报告编号不能为空');
    //    return;
    //}
    if ($('#TemplateChoose1').combobox('getValue') == "" || $('#TemplateChoose1').combobox('getValue') == $('#TemplateChoose1').combobox('getText')) {
        $.messager.alert('提示', '请选择有效报告模板');
        return;
    }
    //if ($('#Inspection_result').combobox('getValue') == "" || $('#Inspection_result').combobox('getValue') == $('#Inspection_result').combobox('getText')) {
    //    $.messager.alert('提示', '请选择有效检验结果');
    //    return;
    //}
    //if ($('#Inspection_date').textbox('getText') == "") {
    //    $.messager.alert('提示', '检验日期不能为空');
    //    return;
    //}


    $('#add_reports_info').form('submit', {
        url: "/ReportManagement/AddReportBaseInfo",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            //param.report_format = report_format;
            //param.report_name = Template;
            //param.report_url = url;
            param.id = $("#copy_id").val();
            param.tm_id = tm_id;
            return $(this).form('enableValidation').form('validate');
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#report_edit_task').datagrid('reload');
                $('#add_reports_info').dialog('close');
                $.messager.alert('提示', '添加信息成功');
            }
            else {
                $.messager.alert('提示', result.Message);
            }
        }
    });
}
//===================================================
//修改报告信息
function edit_report_info() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    $("#copy_add").css("display", "none");
    if (selected_report) {
        line = $('#report_edit_task').datagrid("getRowIndex", selected_report);
        var rowss = $("#report_edit_task").datagrid("getSelections");
        //$('#add_reports_info').form("reset");
        $('#add_reports_info').dialog({
            width: 650,
            height: 520,
            modal: true,
            title: '修改报告信息',
            border: false,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    editInfoSave();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#add_reports_info').dialog('close');
                }
            }]
        });
        $("#add_reports_info").form('load', rowss[0]);
        $(".test :text").attr("disabled", "disabled");
        $("#TemplateChoose1").combobox({"disabled":true});
        $("#TemplateChoose1").combobox("setValue", selected_report.tm_id);
        $("#Inspection_result").combobox("setValue", selected_report.Inspection_result);
        if (selected_report.figure == "True") {
            $("#figure").combobox("select", "1");
        } else if (selected_report.figure == "False") {
            $("#figure").combobox("select", "0");
        }
    } else {
        $.messager.alert('提示', '请选择操作的报告！');
    }

}
//保存修改信息
function editInfoSave() {

    //如果选中要复制得试验就将隐藏表单中id回显
    var selectRow = $("#report_edit_task").datagrid("getSelected");

    var tm_id = $("#TemplateChoose1").combobox("getValue");
    var Template = $("#TemplateChoose1").combobox("getText");
    var url = selectRow.report_url//+ ".docx";
    //form表单提交
    $('#add_reports_info').form('submit', {
        url: "/ReportManagement/EditReportBaseInfo",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {

            param.report_format = selectRow.report_format;
            param.report_name = Template;
            param.report_url = url;
            param.id = selectRow.id;
            param.tm_id = tm_id;
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#report_edit_task').datagrid('reload');
                $('#add_reports_info').dialog('close');
                $.messager.alert('提示', '修改信息成功');

            }
            else {
                $.messager.alert('提示', result.Message);
            }
        }
    });
}
//预览报告
function view_word_temp() {
    var TemplateChoose = $("#TemplateChoose1").combobox("getValue");
    //var Template = $("#TemplateChoose").combobox("getText");
    if (TemplateChoose != "") {
        //$.ajax({
        //    url: "",
        //    type: 'POST',
        //    success: function (data) {
        //        var url = "/upload_Folder/Lossless_report/" + TemplateChoose;//+ ".docx";
        //        var new_url = $("#read_doc").prop("href");//+ "___hxw";
        //        var strs = new_url.split('___hxw');
        //        new_url = strs[0];
        //        $("#read_doc").prop("href", new_url + "___hxw&url=" + url);
        //        document.getElementById('read_doc').click();
        //    }
        //});
        
        let cookie_val = getCookie("UserCount");

        $("#WordRead").prop("href", WordUrlSpit_Read[0] + "?id=" + TemplateChoose + "&OperateType=Template&UserCount_=" + cookie_val + WordUrlSpit_Read[1]);
        document.getElementById('WordRead').click();

    } else {
        $.messager.alert('提示', '请选择模板')
    }
}
//===================================================
//写入检测数据
function WriteTestData() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    //if (selected_report) {
    //    line = $('#report_edit_task').datagrid("getRowIndex", selected_report);
    //    if (selected_report.report_name == "热处理模板3") {
    //        $.messager.alert('提示', '该模板不做结构化数据处理');
    //    } else if (selected_report.report_name == "目视报告模板_VT_63") {
    //        $.messager.alert('提示', '该模板不做结构化数据处理');
    //    } else if (selected_report.report_name == "DT模板空白_5版") {
    //        TestData_dialog('form_3');
    //    } else if (selected_report.report_name == "ECT-涡流检验报告模板RPV") {
    //        TestData_dialog('form_4');
    //    } else if (selected_report.report_name == "ECT-涡流检验报告模板SG") {
    //        TestData_dialog('form_5');
    //    } else if (selected_report.report_name == "LT-氦检漏报告模板_4版123") {
    //        TestData_dialog('form_6');
    //    } else if (selected_report.report_name == "MT-磁轭法和触头法磁粉检验报告3版123") {
    //        TestData_dialog('form_7');
    //    } else if (selected_report.report_name == "MT-磁粉检验报告床式") {
    //        TestData_dialog('form_8');
    //    } else if (selected_report.report_name == "RT-射线照相评定记录2") {
    //        TestData_dialog('form_9');
    //    } else if (selected_report.report_name == "UT-超声波测厚报告") {
    //        TestData_dialog('form_10');
    //    } else if (selected_report.report_name == "UT-超声波检验报告1-正页") {
    //        TestData_dialog('form_11');
    //    } else if (selected_report.report_name == "水压试验报告模板21") {
    //        $.messager.alert('提示', '该模板不做结构化数据处理');
    //    } else if (selected_report.report_name == "自动超声波检验报告1-检验报告") {
    //        TestData_dialog('form_13');
    //    } else if (selected_report.report_name == "UT-超声波检验报告2-扫查示意图") {
    //        TestData_dialog('form_14');
    //    } else if (selected_report.report_name == "UT-超声波检验报告3-对接焊缝缺陷记录") {
    //        TestData_dialog('form_15');
    //    } else if (selected_report.report_name == "UT-超声波检验报告3-角焊缝缺陷记录（横）") {
    //        TestData_dialog('form_16');
    //    } else if (selected_report.report_name == "UT-超声波检验报告3-角焊缝缺陷记录（竖）") {
    //        TestData_dialog('form_17');
    //    } else if (selected_report.report_name == "UT-超声波检验报告3-小接管角焊缝缺陷记录") {
    //        TestData_dialog('form_18');
    //    } else if (selected_report.report_name == "UT-超声波检验报告4-近表面附加扫查") {
    //        TestData_dialog('form_19');
    //    } else if (selected_report.report_name == "NDE报告附页") {
    //        TestData_dialog('form_20');
    //    } else if (selected_report.report_name == "RT-射线照相封口焊评定记录3") {
    //        TestData_dialog('form_21');
    //    } else if (selected_report.report_name == "自动超声波检验报告2-缺陷信号记录") {
    //        TestData_dialog('form_22');
    //    } else if (selected_report.report_name == "自动超声波检验报告3-校验数据表") {
    //        TestData_dialog('form_23');
    //    } else if (selected_report.report_name == "自动超声波检验报告4-扫查清单") {
    //        TestData_dialog('form_24');
    //    } else if (selected_report.report_name == "自动超声波检验报告5焊缝数据记录单") {
    //        TestData_dialog('form_25');
    //    } else if (selected_report.report_name == "PT-液体渗透检验报告") {
    //        TestData_dialog('form_26');
    //    } else if (selected_report.report_name == "RT-射线检验报告1") {
    //        TestData_dialog('form_27');
    //    } else {
    //        $.messager.alert('提示', '该模板不做结构化数据处理');
    //    }
    //} else {
    //    $.messager.alert('提示', '请选择报告');
    //}
    if (selected_report) {
        line = $('#report_edit_task').datagrid("getRowIndex", selected_report);
        if (selected_report.tm_id == "1") {
            $.messager.alert('提示', '该模板不做结构化数据处理');
        } else if (selected_report.tm_id == "2") {
            $.messager.alert('提示', '该模板不做结构化数据处理');
        } else if (selected_report.tm_id == "3") {
            TestData_dialog('form_3');
        } else if (selected_report.tm_id == "4") {
            TestData_dialog('form_4');
        } else if (selected_report.tm_id == "5") {
            TestData_dialog('form_5');
        } else if (selected_report.tm_id == "6") {
            TestData_dialog('form_6');
        } else if (selected_report.tm_id == "7") {
            TestData_dialog('form_7');
        } else if (selected_report.tm_id == "8") {
            TestData_dialog('form_8');
        }  else if (selected_report.tm_id == "10") {
            TestData_dialog('form_10');
        } else if (selected_report.tm_id == "11") {
            TestData_dialog('form_11');
        } else if (selected_report.tm_id == "12") {
            $.messager.alert('提示', '该模板不做结构化数据处理');
        } else if (selected_report.tm_id == "13") {
            TestData_dialog('form_13');
        } else if (selected_report.tm_id == "26") {
            TestData_dialog('form_26');
        } else if (selected_report.tm_id == "27") {
            TestData_dialog('form_27');
        } else {
            $.messager.alert('提示', '该模板不做结构化数据处理');
        }
    } else {
        $.messager.alert('提示', '请选择报告');
    }
}
//打开检测数据弹窗
function TestData_dialog(id) {

    $('#' + id + ' .Device_test').combobox({
        url: "/ReportManagement/GetEquipmentInfo",
        valueField: 'Value',
        textField: 'Text',
        type: 'POST',
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    //添加探头
    $('#' + id + ' .Probe_test').unbind("click").bind('click', function () {
        Probe_equipment();
    });
    //添加试块
    $('#' + id + ' .testBlock').unbind("click").bind('click', function () {
        testBlockEquipment();
    });

    var selected_report = $("#report_edit_task").datagrid("getSelected");

    //设备回显
    $.ajax({
        url: '/ReportManagement/GetReportEquipmentInfo',
        type: 'POST',
        data: {
            id: selected_report.id,
            tm_id: selected_report.tm_id
        },
        success: function (data) {
            var result = $.parseJSON(data);
            if (result.Success == true) {
                for (var i = 0; i < result.Data.length; i++) {
                    console.log(result.Data[i].equipment_id);
                    $('#' + id + ' .Device_test').eq(i).combobox('setValue', result.Data[i].equipment_id);
                    // $('#' + id + ' .Device_test').eq(i).combobox('setText', result.Data[i].equipment_num);
                }

            }
            else {
                $.messager.alert('提示', result.Message);

            }
        }
    });

    $('#' + id).dialog({
        width: 800,
        height: 550,
        modal: true,
        title: '写入检测数据',
        border: false,
        buttons: [{
            text: '确定',
            iconCls: 'icon-ok',
            handler: function () {
                save_testData(id);
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#' + id).dialog('close');
            }
        }]
    });

    var rowss = $("#report_edit_task").datagrid("getSelections");
    $('#' + id).form('load', rowss[0]);

    checkbox_prop(selected_report);
    //    document.getElementsByName("Data1").prop("checked", "checked");
}
//checkbox判断
function checkbox_prop(selected_report) {
    console.log(selected_report)
    //数组
    var num = new Array(selected_report.Data1, selected_report.Data2, selected_report.Data3, selected_report.Data4, selected_report.Data5, selected_report.Data6, selected_report.Data7, selected_report.Data8, selected_report.Data9, selected_report.Data10,
        selected_report.Data11, selected_report.Data12, selected_report.Data13, selected_report.Data14, selected_report.Data15, selected_report.Data16, selected_report.Data17, selected_report.Data18, selected_report.Data19, selected_report.Data20,
        selected_report.Data21, selected_report.Data22, selected_report.Data23, selected_report.Data24, selected_report.Data25, selected_report.Data26, selected_report.Data27, selected_report.Data28, selected_report.Data29, selected_report.Data30,
        selected_report.Data31, selected_report.Data32, selected_report.Data33, selected_report.Data34, selected_report.Data35, selected_report.Data36, selected_report.Data37, selected_report.Data38, selected_report.Data39, selected_report.Data40,
        selected_report.Data41, selected_report.Data42, selected_report.Data43, selected_report.Data44, selected_report.Data45, selected_report.Data46, selected_report.Data47, selected_report.Data48, selected_report.Data49, selected_report.Data50,
        selected_report.Data51, selected_report.Data52, selected_report.Data53, selected_report.Data54, selected_report.Data55, selected_report.Data56, selected_report.Data57, selected_report.Data58, selected_report.Data59, selected_report.Data60,
        selected_report.Data61, selected_report.Data62, selected_report.Data63, selected_report.Data64
        );
    console.log(num);
    for (var i = 0; i < 64; i++) {
        if (num[i] == "1,flag") {
            $("input[name=Data" + (i + 1) + "]").prop("checked", 'checked');
        } else if (num[i] == "flag") {
            $("input[name=Data1" + (i + 1) + "]").prop("checked", false);
        }
    }

}
//保存检测数据
function save_testData(id) {
    //if ($('#' + id + ' .Device_test').combobox('getValue') == "" || $('#' + id + ' .Device_test').combobox('getValue') == $('#' + id + ' .Device_test').combobox('getText')) {
    //    $.messager.alert('提示', '请选择有效试样设备');
    //    return;
    //}
    var Device_test_value = $('#' + id + ' .Device_test');
    var Device_test_text = $('#' + id + ' .Device_test');
    var equipment_id = "";
    var equipment_name = "";
    var equipment_name_R = "";
    for (var i = 0; i < Device_test_value.length; i++) {
        if (i == 0) {
            equipment_id = Device_test_value.eq(i).combobox('getValue');
            equipment_name = Device_test_text.eq(i).combobox('getText')
        } else {
            equipment_id = equipment_id + "," + Device_test_value.eq(i).combobox('getValue');
            equipment_name = equipment_name + "," + Device_test_text.eq(i).combobox('getText')
        }
    }

    //去除最后一个逗号
    //console.log(equipment_id);
    //equipment_id = (equipment_id.substring(equipment_id.length - 2) == ',,') ? equipment_id.substring(0, equipment_id.length - 2) : equipment_id;
    //equipment_name = (equipment_name.substring(equipment_name.length - 2) == ',,') ? equipment_name.substring(0, equipment_name.length - 2) : equipment_name;
    //form表单提交
    var selected_report = $("#report_edit_task").datagrid("getSelected");

    $('#' + id).form('submit', {
        url: "/ReportManagement/AddTextData",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            console.log(param)
            param.equipment_id = equipment_id;
            param.equipment_name = equipment_name;
            param.report_num = selected_report.report_num;
            param.report_name = selected_report.report_name;
            param.report_id = selected_report.id;
            param.tm_id = selected_report.tm_id;
            param.tp_id = selected_report.tp_id

        },
        success: function (data) {

            var result = $.parseJSON(data);
            if (result.Success == true) {
                $('#' + id).dialog('close');
                $('#report_edit_task').datagrid('reload');
                $.messager.alert('提示', '保存成功');
            }
            else {
                $.messager.alert('Tips', result.Message);
            }
        }
    });
}
//===================================================
//添加探头
function Probe_equipment() {
    var selectRow = $('#report_edit_task').datagrid('getSelected');
    if (selectRow) {
        $('#Probe_info').dialog({
            width: 900,
            height: 520,
            modal: true,
            title: '添加探头',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Probe_info').dialog('close');
                }
            }]
        });
        $('#Probe_info').form('reset');
    }

    //已授权且在用探头
    $('#Standard_Authorized').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/ReportManagement/load_Probe_test",//接收一般处理程序返回来的json数据 
        columns: [[
           { field: 'id', title: '探头Id' },
          { field: 'Probe_name', title: '探头名' },
           { field: 'Probe_num', title: '探头编号' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#Standard_Authorized').datagrid('selectRow', 0);
        },
        onDblClickRow: function (index, row) {
            $('#Standard_add').click();
        }
    });
    //定义pagination加载内容
    var p = $('#Standard_Authorized').datagrid('getPager');
    (p).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });
    //已选择探头
    $('#Standard_verification').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            report_id: selectRow.id
        },
        url: "/ReportManagement/load_Probe_test2",//接收一般处理程序返回来的json数据 
        columns: [[

           { field: 'Probe_name', title: '探头名' },
           { field: 'Probe_num', title: '探头编号' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#Standard_verification').datagrid('selectRow', 0);
        },
        onDblClickRow: function (index, row) {
            $('#Standard_remove').click();
        }
    });
    //定义pagination加载内容
    var p = $('#Standard_verification').datagrid('getPager');
    (p).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });

    //添加探头
    $('#Standard_add').unbind('click').bind('click', function () {
        //var rowss2 = $('#Standard_Authorized').datagrid('getSelections'); //获取选中数据

        //var ids1 = [];
        //for (var i = 0; i < rowss2.length; i++) {
        //    ids1.push(rowss2[i].id);
        //}
        //var ids = ids1.join(';');
        var selectRow1 = $('#report_edit_task').datagrid('getSelected');
        var selectRow_Authorized = $('#Standard_Authorized').datagrid('getSelected');


        if (selectRow_Authorized) {
            $.ajax({
                url: "/ReportManagement/add_Probe_test",
                type: 'POST',
                data: {
                    probe_id: selectRow_Authorized.id,
                    report_id: selectRow1.id,
                    report_num: selectRow1.report_num
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {

                        $('#Standard_verification').datagrid('reload');
                    }
                    else {
                        $.messager.alert('提示', result.Message);
                    }

                }
            });
        } else {
            $.messager.confirm('确认', '请选择要操作的探头！');
        }

    })

    //删除已经选择探头
    $('#Standard_remove').unbind('click').bind('click', function () {
        //var rowss3 = $('#Standard_verification').datagrid('getSelections'); //获取选中数据

        //var records_nums1 = [];
        //for (var i = 0; i < rowss3.length; i++) {
        //    records_nums1.push(rowss3[i].records_num);
        //}
        //var records_nums = records_nums1.join(';');
        var selectRow1 = $('#report_edit_task').datagrid('getSelected');
        var selectRow_verification = $("#Standard_verification").datagrid("getSelected");
        if (selectRow_verification) {
            $.ajax({
                url: "/ReportManagement/del_Probe_test",
                type: 'POST',
                data: {
                    ID: selectRow_verification.id,
                    report_id: selectRow1.id,
                    report_num: selectRow1.report_num
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {

                        $('#Standard_verification').datagrid('reload');
                    }
                    else {
                        $.messager.alert('提示', result.Message);
                    }
                }
            });
        } else {
            $.messager.confirm('确认', '请选择要操作的行！');
        }

    })

    //搜索标准试验
    $("#search_test2").unbind('click').bind('click', function () {
        var selectRow_Authorized = $('#not_submitted_commissioned').datagrid('getSelected');

        $('#Standard_Authorized').datagrid(
        {
            type: 'POST',
            dataType: "json",
            url: "Test_commissioned_application.ashx?&cmd=search_test",//接收一般处理程序返回来的json数据                
            queryParams: {
                Acceptance_standard: selectRow_Authorized.Acceptance_standard,
                search: $("#search_test").combobox('getValue'),
                search1: $("#search_test1").textbox('getText')
            }
        });
    })

}
//添加试块
function testBlockEquipment() {
    var selectRow = $('#report_edit_task').datagrid('getSelected');
    if (selectRow) {
        $('#TestBlock').dialog({
            width: 950,
            height: 520,
            modal: true,
            title: '添加试块',
            draggable: true,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#TestBlock').dialog('close');
                }
            }]
        });
        $('#TestBlock').form('reset');
    }
    //已授权且在用探头
    $('#Probe_Authorized').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            report_id: selectRow.id
        },
        url: "/ReportManagement/load_Probe_test2",//接收一般处理程序返回来的json数据 
        columns: [[
            { field: 'id', title: '探头Id' },
            { field: 'Probe_name', title: '探头名' },
            { field: 'Probe_num', title: '探头编号' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#Probe_Authorized').datagrid('selectRow', 0);
           
          
        },
        onSelect: function () {
            var Probe_AuthorizedSelect = $('#Probe_Authorized').datagrid('getSelected');
            $('#TestBlock_verification').datagrid({
                queryParams: {
                    ReportID: selectRow.id,
                    ProbeID: Probe_AuthorizedSelect.id
                },
                url: "/ReportManagement/GetTestTestBlock",//接收一般处理程序返回来的json数据 
            })
            //定义pagination加载内容
            var p = $('#TestBlock_verification').datagrid('getPager');
            (p).pagination({
                layout: ['first', 'prev', 'last', 'next']
            });
        }
    });
    //定义pagination加载内容
    var p = $('#Probe_Authorized').datagrid('getPager');
    (p).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });
    //试块
    $('#TestBlock_Authorized').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        url: "/ReportManagement/GetTestBlockLibrary",//接收一般处理程序返回来的json数据 
        columns: [[
            //{ field: 'id', title: '试块Id' },
            { field: 'CalBlock', title: '试块名' },
            { field: 'CalBlockNum', title: '试块编号' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#TestBlock_Authorized').datagrid('selectRow', 0);
        },
        onDblClickRow: function (index, row) {
            $('#TestBlock_add').click();
        }
    });
    //定义pagination加载内容
    var p = $('#TestBlock_Authorized').datagrid('getPager');
    (p).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });

  
    //已选择试块
    $('#TestBlock_verification').datagrid({
        nowrap: false,
        striped: true,
        //rownumbers: true,
        singleSelect: true,
        border: false,
        fitColumns: true,
        fit: true,
        pagination: true,
        pageSize: 40,
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
      
        columns: [[

            { field: 'CalBlock', title: '试块名' },
            { field: 'CalBlockNum', title: '试块编号' }
        ]],
        onLoadSuccess: function (data) {
            //默认选择行
            $('#TestBlock_verification').datagrid('selectRow', 0);
        },
        onDblClickRow: function (index, row) {
            $('#TestBlock_remove').click();
        }
    });
    //定义pagination加载内容
    var p = $('#TestBlock_verification').datagrid('getPager');
    (p).pagination({
        layout: ['first', 'prev', 'last', 'next']
    });

    //添加试块
    $('#TestBlock_add').unbind('click').bind('click', function () {
        var selectRow1 = $('#report_edit_task').datagrid('getSelected');
        var Probe_Authorized = $('#Probe_Authorized').datagrid('getSelected');
        var TestBlock_Authorized = $('#TestBlock_Authorized').datagrid('getSelected');
        if (!Probe_Authorized) {
            $.messager.confirm('确认', '请选择要操作的探头！');
            return;
        } 
        if (!TestBlock_Authorized) {
            $.messager.confirm('确认', '请选择要操作的试块！');
            return;
        }
        $.ajax({
            url: "/ReportManagement/Add_TestTestBlock",
            type: 'POST',
            data: {
                ProbeID: Probe_Authorized.id,
                ReportID: selectRow1.id,
                CalBlockID: TestBlock_Authorized.CalBlockID
            },
            success: function (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {

                    $('#TestBlock_verification').datagrid('reload');
                }
                else {
                    $.messager.alert('提示', result.Message);
                }

            }
        });

    })

    //删除已经选择探头
    $('#TestBlock_remove').unbind('click').bind('click', function () {
        var selectRow1 = $('#report_edit_task').datagrid('getSelected');
        var TestBlock_verification = $("#TestBlock_verification").datagrid("getSelected");

        if (TestBlock_verification) {
            $.ajax({
                url: "/ReportManagement/Delete_TestTestBlock",
                type: 'POST',
                data: {
                    ID: TestBlock_verification.ID
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $('#TestBlock_verification').datagrid('reload');
                    }
                    else {
                        $.messager.alert('提示', result.Message);
                    }
                }
            });
        } else {
            $.messager.confirm('确认', '请选择要操作的行！');
        }

    })
}
//载入报告
function load_report_model() {
    var selectRow_report = $("#report_edit_task").datagrid("getSelected");
    // var templet_id = "";
    // templet_id = $("#TemplateChoose").combobox("getValue");

    if (selectRow_report) {
        line = $('#report_edit_task').datagrid("getRowIndex", selectRow_report);
        if (selectRow_report.report_url == "") {
            //载入模板
            report_model_1(selectRow_report);
        } else {
            $.messager.confirm('任务提示', '您已经载入过报告，确定要重新载入吗？', function (r) {
                if (r) {
                    //载入模板
                    report_model_1(selectRow_report);
                }
            })
        }
        //模板没选择操作
    } else {

        $.messager.alert('提示', '请选择报告！');

    }
    try {
        $.messager.progress('close');
    }
    catch (r) { }

}
//载入报告模板
var new_id_url = "";
function report_model_1(selectRow_report) {
    $.ajax({
        url: "/ReportManagement/Filling_report",
        type: 'POST',
        data: {
            id: selectRow_report.id,
            tm_id: selectRow_report.tm_id,
            report_format: selectRow_report.report_format,
            report_num: selectRow_report.report_num,
        },
        success: function (data) {

            var result = $.parseJSON(data);
            if (result.Success == true) {
                let cookie_val = getCookie("UserCount");

                $("#reports_edit_1").prop("href", WordUrlSpit[0] + "?id=" + selectRow_report.id + '&save_type=Lossless_report_&OperateType=Report&UserCount_=' + cookie_val+WordUrlSpit[1]);
                document.getElementById('reports_edit_1').click();
                $('#report_edit_task').datagrid('reload');


            }
            else {
                $.messager.alert('提示', result.Message);
            }
        }
    });
}
//附件添加到报告
function AddAccessory() {
    var selectRow_report = $("#report_edit_task").datagrid("getSelected");

    $.ajax({
        url: "/ReportManagement/AddAccessory",
        type: 'POST',
        data: {
            report_id: selectRow_report.id,
            report_url: selectRow_report.report_url
        },
        success: function (data) {
            if (data == "不存在附件") {
                $.messager.alert('提示', "不存在该报告的附件，请上传之后再附加！")
            } else {
                $("#reports_edit_1").prop("href", new_id_url + "___hxw&url=" + data);
                document.getElementById('reports_edit_1').click();
            }
        }
    });
}
//===================================================
//报告在线编辑
function edit_online() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        let cookie_val = getCookie("UserCount");

        $("#reports_edit_1").prop("href", WordUrlSpit[0] + "?id=" + selected_report.id + '&Condition=true&save_type=Lossless_report_&OperateType=Report&UserCount_=' + cookie_val + WordUrlSpit[1]);
        document.getElementById('reports_edit_1').click();

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}
//获取cookie值
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

//===================================================
//删除信息
function DataDel() {
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        $.messager.confirm('删除任务提示', '您确认要删除选中报告信息吗', function (r) {
            if (r) {
                $.ajax({
                    url: "/ReportManagement/DataDel",
                    dataType: "text",
                    type: 'POST',
                    data: {
                        id: selected_report.id,
                        report_num: selected_report.report_num
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        if (result.Success == true) {
                            $('#report_edit_task').datagrid('reload');
                            $.messager.alert('提示', '删除信息成功');

                        }
                        else {
                            $.messager.alert('提示', result.Message);
                        }

                    }
                });
            }
        })

    } else {
        $.messager.alert('提示', '请选择报告');
    }
}
//===================================================
//提交报告审核
function submit_report() {

    //加载室
    loadRoom();

    $('#level_Inspection').combobox({
        value: 0,
        data: [
              { 'value': '0', 'text': 'Ⅱ' },
              { 'value': '1', 'text': 'Ⅲ' }
        ],
    });
    var selected_report = $("#report_edit_task").datagrid("getSelected");
    if (selected_report) {
        if (selected_report.report_url != "") {
            $('#submit_dialog').form("reset");
            $('#level_date').datetimebox('setValue', loaddatetime(new Date()));
            $('#submit_dialog').dialog({
                width: 480,
                height: 310,
                modal: true,
                title: '选择级别',
                draggable: true,
                buttons: [{
                    text: '确定',
                    iconCls: 'icon-ok',
                    handler: function () {
                        SubmitReview();

                    }
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#submit_dialog').dialog('close');
                    }
                }]
            });
        } else {
            $.messager.alert('提示', '请载入报告！');
        }


    } else {
        $.messager.alert('提示', '请选择要操作的行！');

    }
}
//室
function loadRoom() {

    $('#room').combobox({
        url: "/Common/LoadRoomCombobox",
        valueField: 'Value',
        textField: 'Text',
        required: true,
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onSelect: function () {
            var GroupId = $("#room").combobox("getValue")
            //详细位置---领取信息dialog
            $('#group').combobox({
                url: "/Common/LoadGroupCombobox",
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

//获取当天年月日
function loaddate(date) {
    var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
    var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0"
        + (date.getMonth() + 1);
    return date.getFullYear() + '-' + month + '-' + day;
}
//获取当天年月日时分秒
function loaddatetime(date) {
    var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
    var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0"
        + (date.getMonth() + 1);
    var hor = date.getHours();
    var min = date.getMinutes();
    var sec = date.getSeconds();
    return date.getFullYear() + '-' + month + '-' + day + " " + hor + ":" + min + ":" + sec;
}

//提交审核
function SubmitReview() {
    var select = $("#report_edit_task").datagrid("getSelected");
    if ($("#group").combobox("getValue") == "") {
        $.messager.alert('提示', '请选择组！');
        return;
    }
    if ($("#group").combobox("getValue") == $("#group").combobox("getText")) {
        $.messager.alert('提示', '选择得组无效！');
        return;
    }
    $('#submit_dialog').form('submit', {
        url: "/ReportManagement/SubmitEditReport",//接收一般处理程序返回来的json数据     
        onSubmit: function (param) {
            param.id = select.id;
            param.Inspection_personnel = select.Inspection_personnel;
            param.report_num = select.report_num;
            param.group = $("#group").combobox("getValue");
            param.level_Inspection = $("#level_Inspection").combobox("getText");
            param.level_date = $("#level_date").datetimebox("getValue");
            param.report_url = select.report_url;

        },
        success: function (data) {
            if (data) {
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    //选择当前添加节点
                    $('#report_edit_task').datagrid('reload');
                    $('#submit_dialog').dialog('close');
                    $.messager.alert('Tips', result.Message);
                } else {
                    $.messager.alert('Tips', result.Message);
                }
            }
        }
    });
}
//===================================================
//管理编制/未提交编制和历史编制—— &&模板选择
function management_all() {

    $('#management_all').combobox({
        value: 0,
        data: [
              { 'value': '0', 'text': '待编制' },
             { 'value': '1', 'text': '历史编制' }
        ],
        onSelect: function () {

            //历史编制  1    未提交 0

            load_list();
        }
    });

    //$('#TemplateChoose').combobox({
    //    data: [
    //          { 'value': '1.docx', 'text': '热处理模板3' },//**************不可编辑报告
    //          { 'value': '2.doc', 'text': '目视报告模板_VT_63' },//**************不可编辑报告
    //          { 'value': '3.docx', 'text': 'DT模板空白_5版' },//*************不可编辑报告
    //          { 'value': '4.doc', 'text': 'ECT-涡流检验报告模板RPV' },
    //          { 'value': '5.doc', 'text': 'ECT-涡流检验报告模板SG' },
    //          { 'value': '6.doc', 'text': 'LT-氦检漏报告模板_4版123' },
    //          { 'value': '7.doc', 'text': 'MT-磁轭法和触头法磁粉检验报告3版123' },
    //          { 'value': '8.doc', 'text': 'MT-磁粉检验报告床式' },
    //          //{ 'value': '9.docx', 'text': 'RT-射线照相评定记录2' },//*************不可编辑报告
    //          { 'value': '10.doc', 'text': 'UT-超声波测厚报告' },
    //          { 'value': '11.doc', 'text': 'UT-超声波检验报告1-正页' },
    //          { 'value': '12.doc', 'text': '水压试验报告模板21' },//***********不可编辑报告
    //          { 'value': '13.doc', 'text': '自动超声波检验报告1-检验报告' },
    //          //{ 'value': '14.doc', 'text': 'UT-超声波检验报告2-扫查示意图' },//**********不可编辑报告
    //          //{ 'value': '15.docx', 'text': 'UT-超声波检验报告3-对接焊缝缺陷记录' },//******不可编辑报告
    //          //{ 'value': '16.docx', 'text': 'UT-超声波检验报告3-角焊缝缺陷记录（横）' },//不可编辑报告
    //          //{ 'value': '17.docx', 'text': 'UT-超声波检验报告3-角焊缝缺陷记录（竖）' },//不可编辑报告
    //          //{ 'value': '18.docx', 'text': 'UT-超声波检验报告3-小接管角焊缝缺陷记录' },//不可编辑报告
    //          //{ 'value': '19.docx', 'text': 'UT-超声波检验报告4-近表面附加扫查' },//不可编辑报告
    //          //{ 'value': '20.docx', 'text': 'NDE报告附页' },//不可编辑报告
    //          //{ 'value': '21.docx', 'text': 'RT-射线照相封口焊评定记录3' },//不可编辑报告
    //          //{ 'value': '22.docx', 'text': '自动超声波检验报告2-缺陷信号记录' },//不可编辑报告
    //          //{ 'value': '23.docx', 'text': '自动超声波检验报告3-校验数据表' },//不可编辑报告
    //          //{ 'value': '24.docx', 'text': '自动超声波检验报告4-扫查清单' },//不可编辑报告
    //          //{ 'value': '25.docx', 'text': '自动超声波检验报告5焊缝数据记录单' },//不可编辑报告
    //          { 'value': '26.doc', 'text': 'PT-液体渗透检验报告' },
    //          { 'value': '27.doc', 'text': 'RT-射线检验报告1' }

    //    ]
    //});

    $('#TemplateChoose1').combobox({
        url: "/ReportManagement/LoadTemplateCombobox",
        valueField: 'Value',
        textField: 'Text',
        required: true,
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        //value: "4.doc",
        //data: [
        //      { 'value': '1.docx', 'text': '热处理模板3' },//不可编辑报告
        //      { 'value': '2.doc', 'text': '目视报告模板_VT_63' },//**************不可编辑报告
        //      { 'value': '3.docx', 'text': 'DT模板空白_5版' },//*************不可编辑报告
        //      { 'value': '4.doc', 'text': 'ECT-涡流检验报告模板RPV' },
        //      { 'value': '5.doc', 'text': 'ECT-涡流检验报告模板SG' },
        //      { 'value': '6.doc', 'text': 'LT-氦检漏报告模板_4版123' },
        //      { 'value': '7.doc', 'text': 'MT-磁轭法和触头法磁粉检验报告3版123' },
        //      { 'value': '8.doc', 'text': 'MT-磁粉检验报告床式' },
        //      //{ 'value': '9.docx', 'text': 'RT-射线照相评定记录2' },//*************不可编辑报告
        //      { 'value': '10.doc', 'text': 'UT-超声波测厚报告' },
        //      { 'value': '11.doc', 'text': 'UT-超声波检验报告1-正页' },
        //      { 'value': '12.doc', 'text': '水压试验报告模板21' },//***********不可编辑报告
        //      { 'value': '13.doc', 'text': '自动超声波检验报告1-检验报告' },
        //      //{ 'value': '14.doc', 'text': 'UT-超声波检验报告2-扫查示意图' },//**********不可编辑报告
        //      //{ 'value': '15.docx', 'text': 'UT-超声波检验报告3-对接焊缝缺陷记录' },//******不可编辑报告
        //      //{ 'value': '16.docx', 'text': 'UT-超声波检验报告3-角焊缝缺陷记录（横）' },//不可编辑报告
        //      //{ 'value': '17.docx', 'text': 'UT-超声波检验报告3-角焊缝缺陷记录（竖）' },//不可编辑报告
        //      //{ 'value': '18.docx', 'text': 'UT-超声波检验报告3-小接管角焊缝缺陷记录' },//不可编辑报告
        //      //{ 'value': '19.docx', 'text': 'UT-超声波检验报告4-近表面附加扫查' },//不可编辑报告
        //      //{ 'value': '20.docx', 'text': 'NDE报告附页' },//不可编辑报告
        //      //{ 'value': '21.docx', 'text': 'RT-射线照相封口焊评定记录3' },//不可编辑报告
        //      //{ 'value': '22.docx', 'text': '自动超声波检验报告2-缺陷信号记录' },//不可编辑报告
        //      //{ 'value': '23.docx', 'text': '自动超声波检验报告3-校验数据表' },//不可编辑报告
        //      //{ 'value': '24.docx', 'text': '自动超声波检验报告4-扫查清单' },//不可编辑报告
        //      //{ 'value': '25.docx', 'text': '自动超声波检验报告5焊缝数据记录单' },//不可编辑报告
        //      { 'value': '26.doc', 'text': 'PT-液体渗透检验报告' },
        //      { 'value': '27.doc', 'text': 'RT-射线检验报告1' }
        //],
        onSelect: function () {
            var list_value = $("#TemplateChoose1").combobox("getValue");
            if (list_value == "1")//热处理模板3
            {
                $(".play_3").css("display", "none");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "none");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "none");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "none");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "none");
                $(".play_16").css("display", "none");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "block");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "none");
                $(".play_26").css("display", "block");
                $(".play_28").css("display", "block");
                $(".play_29").css("display", "block");

            } else if (list_value == "2") //目视报告模板_VT_63
            {
                $(".play_3").css("display", "none");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "none");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "none");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "none");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "block");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "none");
                $(".play_26").css("display", "block");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "3")//DT模板空白_5版
            {
                $(".play_3").css("display", "none");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "none");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "none");
                $(".play_10").css("display", "none");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "none");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "block");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "none");
                $(".play_26").css("display", "block");
                $(".play_28").css("display", "block");
                $(".play_29").css("display", "none");

            } else if (list_value == "12") //水压试验报告模板21
            {
                $(".play_3").css("display", "none");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "none");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "none");
                $(".play_10").css("display", "none");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "none");
                $(".play_15").css("display", "none");
                $(".play_16").css("display", "none");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "block");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "none");
                $(".play_26").css("display", "block");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "4") //ECT-涡流检验报告模板RPV
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "block");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "block");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "5")//ECT-涡流检验报告模板SG
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "none");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "block");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "block");
                $(".play_23").css("display", "block");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "6") //LT-氦检漏报告模板_4版123
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "none");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "7")//MT-磁轭法和触头法磁粉检验报告3版123
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "block");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");
            } else if (list_value == "8") //MT-磁粉检验报告床式
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "block");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "10") //UT-超声波测厚报告
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "none");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "none");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "11") //UT-超声波检验报告1-正页
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "block");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "13") //自动超声波检验报告1-检验报告
            {

                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "block");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "block");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "26") //PT-液体渗透检验报告
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "block");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else if (list_value == "27")//RT-射线检验报告1
            {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "none");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "none");
                $(".play_11").css("display", "none");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "none");
                $(".play_18").css("display", "none");
                $(".play_19").css("display", "none");
                $(".play_20").css("display", "none");
                $(".play_21").css("display", "none");
                $(".play_22").css("display", "none");
                $(".play_23").css("display", "none");
                $(".play_24").css("display", "none");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "none");
                $(".play_28").css("display", "none");
                $(".play_29").css("display", "none");

            } else {
                $(".play_3").css("display", "block");
                $(".play_4").css("display", "block");
                $(".play_5").css("display", "block");
                $(".play_6").css("display", "block");
                $(".play_7").css("display", "block");
                $(".play_8").css("display", "block");
                $(".play_9").css("display", "block");
                $(".play_10").css("display", "block");
                $(".play_11").css("display", "block");
                $(".play_12").css("display", "block");
                $(".play_13").css("display", "block");
                $(".play_14").css("display", "block");
                $(".play_15").css("display", "block");
                $(".play_16").css("display", "block");
                $(".play_17").css("display", "block");
                $(".play_18").css("display", "block");
                $(".play_19").css("display", "block");
                $(".play_20").css("display", "block");
                $(".play_21").css("display", "block");
                $(".play_22").css("display", "block");
                $(".play_23").css("display", "block");
                $(".play_24").css("display", "block");
                $(".play_25").css("display", "block");
                $(".play_26").css("display", "block");
                $(".play_28").css("display", "block");
                $(".play_29").css("display", "block");
                $(".play_30").css("display", "block");

            }

        },
    });

    //$('#group').combobox({
    //    url: "/Common/LoadGroupCombobox",
    //    valueField: 'id',
    //    textField: 'Department_name',
    //    onSelect: function () {
    //        var id = $('#group').combobox('getValue');
    //        $('#Audit_personnel').combobox({
    //            url: "/Common/LoadPersonnelCombobox",
    //            valueField: 'User_count',
    //            textField: 'User_name',
    //            queryParams: {
    //                "id": id
    //            }
    //        })
    //    },
    //    //本地联系人数据模糊索引
    //    filter: function (q, row) {
    //        var opts = $(this).combobox('options');
    //        return row[opts.textField].indexOf(q) >= 0;
    //    }
    //});



    $('#figure').combobox({
        data: [
            { 'value': '1', 'text': '是' },
           { 'value': '0', 'text': '否' }
        ]
    });

    $('#Inspection_result').combobox({
        data: [
           { 'value': 1, 'text': '合格' },
           { 'value': 0, 'text': '不合格' }

        ]
    });
}


/*
*functionName:
*function:下载图片、文件
*Param: 文件路径：fileurl_ 
*Param: 文件名称：filename_ 
*Param: 文件格式：fileFormat_ 
*author:黄小文
*date:2018-11-09
*/
function DownloadFile(fileurl_, filename_, fileFormat_) {


    if (fileFormat_ == "" || fileFormat_ == null) {
        $.messager.alert('提示', '非文件无法下载');

        return;
    }

    var DataFormat = fileFormat_.toLowerCase();
    //ie 下载
    if (!!window.ActiveXObject || "ActiveXObject" in window) {//判断是否为IE
        // alert(1);
        //图片的保存方法
        if (DataFormat == ".png" || DataFormat == ".jpg" || DataFormat == ".jpeg") {
            if (window.navigator.msSaveBlob) {//IE10+方法           
                var canvas_ = document.createElement('canvas');
                var img_ = document.createElement('img');
                img_.onload = function (e) {
                    canvas_.width = img_.width;
                    canvas_.height = img_.height;
                    var context = canvas_.getContext('2d');
                    context.drawImage(img_, 0, 0, img_.width, img_.height);
                    window.navigator.msSaveBlob(canvas_.msToBlob(), filename_ + fileFormat_);///保存图片
                }
                img_.src = fileurl_;
            }
        }
        else {

            window.location = fileurl_;
        }
    }
        //谷歌等浏览器下载方法
    else {

        var $a = $("<a></a>").attr("href", fileurl_).attr("download", filename_ + fileFormat_);
        $a[0].click();


    }


}

