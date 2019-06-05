var line = 0;
var WordUrlSpit = new Array();//new一个新的数组
var word_link;//设置一个全局变量
//******************************************************方法******************************************************************//
$(function () {
    word_link = $("#WordRevision").attr("href");//获取office控件链接 URL
    WordUrlSpit = word_link.split("?");//以问号对获取的数组进行分割

    word_link2 = $("#WordRead").attr("href");//获取office控件链接 URL
    WordUrlSpit_Read = word_link2.split("?");

});

//加载信息
$(function () {

    //搜索
    $('#search').combobox({
        data: [
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'procedure_NO', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                //{ 'value': '', 'text': '报告类型' },
                { 'value': 'Project_name', 'text': '产品（项目名称）' },
                //{ 'value': '', 'text': '零件' },
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'clientele_department', 'text': '委托部门' },
                { 'value': 'ReviewOverdue', 'text': '审核逾期' },
                { 'value': 'IssueOverdue', 'text': '签发逾期' }
        ]
    });
   
});
//检测报告编制-报告信息
$(function () {

    //管理待处理/已处理
    management_all();
    //加载列表
    load_list();

    //搜索
    $("#search_info").unbind('click').bind('click', function () {
        search();
    })
    //预览报告
    $('#Preview_Report').unbind('click').bind('click', function () {
        view_report();
    });
    //退回报告编制
    $('#Back_report').unbind('click').bind('click', function () {
        write_method_equipment();
    });
    //查看退回原因
    $('#return_reason').unbind('click').bind('click', function () {
        write_method_equipment1();
    });
    //签发通过
    $('#submit_Report').unbind('click').bind('click', function () {
        submit_report();
    });
    //查看附件
    $('#read_Report').unbind('click').bind('click', function () {
        view_report1();
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
    //查看附件
    $('#read_report_info').unbind('click').bind('click', function () {

        read_report_info();
    });
    //附件搜索
    //$('#read_search_info').unbind('click').bind('click', function () {
    //    var selectRow = $("#report_issue_task").datagrid("getSelected");
    //    var search = $("#read_search").combobox('getValue');
    //    var key = $("#read_search1").textbox('getText');
    //    $('#read_table').datagrid({
    //        queryParams: {
    //            report_id: selectRow.id,
    //            search: search,
    //            key: key
    //        },
    //        dataType: "json",
    //        url: "/ReportManagement/load_accessory"//接收一般处理程序返回来的json数据    
    //    });
    //});
    var history_flag = $('#management_all').combobox('getValue');
    //逾期时间
    $('#DaySetting').combobox({
        url: "/Common/LoadDaySetting",
        valueField: 'Value',
        textField: 'Text',
        onSelect: function () {
            $('#report_issue_task').datagrid("reload");

        },
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
});
//查看图片
function read_pic() {
    var getSelected = $('#read_table').datagrid('getSelected');
    if (getSelected) {
        var rowss = $('#read_table').datagrid('getSelections');
        $("#Picture_img").attr("src", "../.." + rowss[0].accessory_url);
        $('#Picture_form').dialog({
            width: 400,
            height: 300,
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
        if (selected_report.accessory_format != "") {

            // window.location.href = ;
            //为添加downloam名字
            var str = selected_report.accessory_url.split("/");
            var str1 = str[3];
            var str2 = str1.split('.');
            $("#download_read").attr("href", selected_report.accessory_url);
            $("#download_read").attr("download", str2[0]);
        } else {
            $.messager.alert('提示', '没有上传附件文件！');
        }
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
                if (result.Success) {
                    window.location.href = result;
                }
                else {
                    $.messager.alert('提示', result.Message);
                }

            }
        });
    } else {
        $.messager.alert('提示', '请选择操作行！');
    }
}
//查看附件
function read_report_info() {
    var selectRow = $("#report_issue_task").datagrid("getSelected");
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
        singleSelect: true,
        autoRowHeight: true,
        border: false,
        fit: true,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 15, 20],
        pageNumber: 1,
        type: 'POST',
        queryParams: {
            report_id: selectRow.id,
            search: search,
            //key: key
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
//加载列表
function load_list() {
    var tabs_width = screen.width - 300 - 182;
    var other_height = document.body.clientHeight;
    var search = $("#search").combobox('getValue');
    var key = $("#search1").textbox('getText');
    var history_flag = $('#management_all').combobox('getValue');
    //按钮显示隐藏
    if (history_flag == "0") {
        $(".link_button").css("display", "block");
        $(".history_link_button").css("display", "none");

    } else {
        $(".link_button").css("display", "none");
        $(".history_link_button").css("display", "block");
    }
    //显示分页条数
    var _height1 = window.screen.height - 400;
    var num = parseInt(_height1 / 25);
    $.ajax({
        url: "/ReportManagement/loadPageShowSetting",
        type: 'POST',

        data: {
            PageId: '103',
        },
        success: function (result) {
            var results = $.parseJSON(result);
            if (results.Success) {
                $('#report_issue_task').datagrid(
                    {
                        striped: true,
                        rownumbers: true,
                        //autoRowHeight: true,
                        ctrlSelect: true,
                        border: false,
                        fitColumns: true,
                        fit: true,
                        pagination: true,
                        pageSize: num,
                        pageList: [num, num + 10, num + 20, num + 30],
                        pageNumber: 1,
                        type: 'POST',
                        dataType: "json",
                        url: "/ReportManagement/LoadReportIssueList",//接收一般处理程序返回来的json数据        
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
                            {
                                field: 'Audit_date', title: '审核时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length >= 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }, width: 100
                            },
                            { field: 'return_info', title: '退回原因', hidden: false, sortable: false, width: 100 },
                            { field: 'level_Audit', title: '审核级别', hidden: false, sortable: false, width: 100 },
                            { field: 'issue_personnel_n', title: '签发人员', hidden: false, sortable: false, width: 100 },
                            {
                                field: 'issue_date', title: '签发时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length >= 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }, width: 100
                            },
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
                                if(value == '1'){return "合格"} if(value == '0'){return "不合格"}
                            }
                            },
                            {
                                field: 'state_', title: '状态', hidden: 'true', sortable: 'false',
                                formatter: function (value, row, index) {
                                    if (value == "1") { return "编辑"; } if (value == "2") { return "审核"; } if (value == "3") { return "签发"; } if (value == "4") { return "完成"; }
                                }, width: 100
                            },
                            { field: 'Tubes_Size', title: '管子规格', hidden: false, sortable: false, width: 100 },
                            { field: 'Tubes_num', title: '管子数量', hidden: false, sortable: false, width: 100 },
                            { field: 'welding_method', title: '焊接方法', hidden: false, sortable: false, width: 100 },
                            { field: 'Job_num', title: '工号', hidden: true, sortable: false, width: 100 },
                            { field: 'heat_treatment', title: '热处理设备', hidden: false, sortable: false, width: 100 },
                            { field: 'Work_instruction', title: '作业指导书', hidden: false, sortable: false, width: 100 },
                            {
                                field: 'ReportCreationTime', title: '报告创建时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length >= 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }, width: 100
                            },
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
                                    if (hiddenJson[i].hidden) {
                                        $('#report_issue_task').datagrid('hideColumn', hiddenJson[i].fieldname);
                                    } else {
                                        $('#report_issue_task').datagrid('showColumn', hiddenJson[i].fieldname);
                                    }
                                } catch (e) {

                                }

                            }
                        },
                        onLoadSuccess: function (data) {
                            //默认选择行
                            $('#report_issue_task').datagrid('selectRow', line);
                            var selectRow = $("#report_issue_task").datagrid("getSelected");
                            if (!selectRow) {
                                $('#report_issue_task').datagrid('selectRow', 0);
                            }
                        },
                        onSelect: function () {
                            //提示
                            //$('#report_issue_task').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
                            //view_taskinfo1();
                        },
                        rowStyler: function (index, row) {
                            var selectRow = $("#report_issue_task").datagrid("getSelected");
                            if (selectRow) {
                                var OperateDate = selectRow.OperateDate;
                                if (OperateDate) {
                                    var date = OperateDate.substring(0, 19);
                                    //  var OperateDateNum = date.replace(/-/g, '/').getTime(); 
                                    OperateDateNum = new Date(Date.parse(date.replace(/-/g, "/"))).getTime();
                                    var NowDate = new Date();
                                    var NowDateNum = NowDate.getTime();
                                    var days = $("#DaySetting").combobox('getValue');
                                    if (days) {
                                        var nTime = (NowDateNum - OperateDateNum);//获取天数的时间间隔
                                        var Nowdays = Math.floor(nTime / 86400000);
                                        if (parseFloat(Nowdays) > parseInt(days)) {
                                            return 'color:#f00;';

                                        }
                                    }

                                }
                            }

                        },
                        sortOrder: 'asc',
                        toolbar: "#reports_toolbar"
                    });
            }
            else {
                //  $.messager.alert('提示', data);
            }

        }

    });

}
//=========================================
//搜索
function search() {
    var history_flag = $('#management_all').combobox('getValue');
    $('#report_issue_task').datagrid(
    {
        url: "/ReportManagement/LoadReportIssueList",//接收一般处理程序返回来的json数据    
        queryParams: {
            search: $("#search").combobox('getValue'),
            key: $("#search1").textbox('getText'),
            history_flag: history_flag
        },
        onLoadSuccess: function (data) {
            $('#report_issue_task').datagrid('selectRow', 0);
        }
    }).datagrid('resize');
}
//=========================================

//预览报告——在线痕迹编辑保存
function view_report() {
    var selected_report = $("#report_issue_task").datagrid("getSelected");
    var ReturnNode = "IssueUpdate";
    if (selected_report) {

        $.ajax({
            url: "/ReportManagement/JudgingPersonnelQualifications",
            dataType: "text",
            type: 'POST',
            data: {
                TemplateID: selected_report.tm_id,
                AuthorizationType: 1
            },
            success: function (data) {
                if (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        let cookie_val = getCookie("UserCount");

                        $("#WordRevision").prop("href", WordUrlSpit[0] + '?ReturnNode=6&Condition=true&id=' + selected_report.id + "&UserCount_=" + cookie_val + "&TmId=" + selected_report.tm_id + WordUrlSpit[1]);
                        document.getElementById('WordRevision').click();

                    } else {
                        $.messager.alert('Tips', result.Message);
                    }
                }

            }
        });

        
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

//预览历史报告——只读
function view_report1() {
    var selected_report = $("#report_issue_task").datagrid("getSelected");
    if (selected_report) {

        let cookie_val = getCookie("UserCount");

        $("#WordRead").prop("href", WordUrlSpit_Read[0] + "?id=" + selected_report.id + "&OperateType=Report&UserCount_=" + cookie_val + WordUrlSpit_Read[1]);
        document.getElementById('WordRead').click();

    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');
    }
}
//=========================================
//退回报告编辑
function back_report() {
    var selectRow = $("#report_issue_task").datagrid("getSelected");
    if (selectRow) {
        line = $('#report_issue_task').datagrid("getRowIndex", selectRow);
        $('#Back_report_dialog').form("reset");
        $('#return_accessory').datagrid(
        {
            nowrap: false,
            striped: true,
            rownumbers: true,

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
             { field: 'Picture_name', title: '文件名称' },
             { field: 'Picture_format', title: '文件格式' },
             //{ field: 'Picture_url', title: '文件url' },
             { field: 'Add_personnel_n', title: '操作人员' },
             {
                 field: 'Add_time', title: '操作时间', formatter: function (value, row, index) {
                     if (value) {
                         if (value.length >= 10) {
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
            title: '退回报告',
            draggable: true,
            buttons: [{
                text: '选择退回原因',
                iconCls: 'icon-ok',
                handler: function () {
                    BackReport(selectRow);
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Back_report_dialog').dialog('close');
                }
            }]
        });

        //添加文件
        $('#add_Picture').unbind('click').bind('click', function () {
            var selectRow = $("#return_accessory").datagrid("getSelected");
            var selectRow_report = $("#report_issue_task").datagrid("getSelected");
            $('#add_Picture_dialog').dialog({
                title: '添加附件信息',
                width: 500,
                height: 300,
                top: 100,
                buttons: [{
                    text: '提交',
                    iconCls: 'icon-ok',
                    handler: function () {
                        save_Picture(selectRow_report);
                    }
                }, {
                    text: '取消',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $('#add_Picture_dialog').dialog('close');
                    }
                }]

            });
        })
        function save_Picture(selectRow_report) {
            $('#add_Picture_dialog').form('submit', {
                url: "/ReportManagement/",//接收一般处理程序返回来的json数据     
                onSubmit: function (param) {
                    param.cmd = 'save_accessory';
                    param.report_id = selectRow_report.id;
                    param.report_num = selectRow_report.report_num;
                },
                success: function (data) {
                    var result = $.parseJSON(data);
                    if (result.Success == true) {
                        $('#add_Picture_dialog').dialog('close');
                        $('#return_accessory').datagrid('reload');
                        $.messager.alert('提示', reuslt.Message);
                    } 
                    else {
                        $.messager.alert('提示', reuslt.Message);
                    }
                }
            });

        }
        //查看文件
        $('#open_Picture').unbind('click').bind('click', function () {
            var getSelected = $('#return_accessory').datagrid('getSelected');
            if (getSelected) {
                var rowss = $('#return_accessory').datagrid('getSelections');
                // $("#Picture_img").attr("src", "../.." + rowss[0].Picture_url);
                $('#open_Picture').attr("target", "_blank");
                $('#open_Picture').attr("href", "/mainform/Lossless_report/LookPicture.html?src='" + rowss[0].Picture_url + "'");
                //$('#Picture_form').dialog({
                //    width: 400,
                //    height: 300,
                //    top: 40,
                //    modal: true,
                //    title: '文件',
                //    draggable: true,
                //    buttons: [{
                //        text: '关闭',
                //        iconCls: 'icon-cancel',
                //        handler: function () {
                //            $('#Picture_form').dialog('close');
                //        }
                //    }]
                //});
            } else {
                $.messager.alert('提示', '请选择要操作的行！');
            }

        })
        //删除文件
        $('#del_Picture').unbind('click').bind('click', function () {
            var selectRow = $("#return_accessory").datagrid("getSelected");
            var selectRow_report = $("#report_issue_task").datagrid("getSelected");
            if (selectRow) {
                $.messager.confirm('删除提示', '您确认要删除选中信息吗', function (r) {
                    if (r) {
                        $.ajax({
                            url: "/ReportManagement/del_accessory",
                            dataType: "text",
                            type: 'POST',
                            data: {
                                report_num: selectRow_report.report_num,
                                id: selectRow.id
                            },
                            success: function (data) {
                                var result = $.parsrJSON(data);
                                if (result.Success == true) {
                                    $('#return_accessory').datagrid('reload');
                                    $.messager.alert('提示',result.Message);
                                }
                                else {
                                    $.messager.alert('提示', result.Message);
                                }

                            }
                        });
                    }
                })
            } else {
                $.messager.alert('提示', '请选择要操作的行！');
            }

        })


    } else {
        $.messager.alert('提示', '请选择报告要操作的报告');

    }
}
//退回报告
//function BackReport(selected_report) {
//    //if ($('#return_info').textbox('getText') == "") {
//    //    $.messager.alert('提示', '退回原因不能为空！');
//    //    return;
//    //}
//    write_method_equipment(selected_report);

//}
//查看原因
function write_method_equipment1() {
    var selectRow = $('#report_issue_task').datagrid('getSelected');
    if (selectRow) {
        $('#Standard_equipment_info1').dialog({
            width: 800,
            height: 550,
            modal: true,
            title: '报告退回原因',
            draggable: true,
            buttons: [{
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#Standard_equipment_info1').dialog('close');
                }
            }]
        });
        $('#Standard_equipment_info1').form('reset');
        $("#return_info2").textbox("setText", selectRow.return_info);
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
            pageSize: 10,
            pageNumber: 1,
            type: 'POST',
            dataType: "json",
            queryParams: {
                id: selectRow.id
            },
            url: "/ReportManagement/LoadErrorInfo",//接收一般处理程序返回来的json数据 
            columns: [[
                { field: 'error_remarks', title: '错误信息',width:100 },
               { field: 'addpersonnel_n', title: '发现人', width: 100 },
                { field: 'add_date', title: '发现时间', width: 100 },
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

//写入试验原因
function write_method_equipment() {
    var selectRow = $('#report_issue_task').datagrid('getSelected');
    if (selectRow) {
        $('#Standard_equipment_info').dialog({
            width: 800,
            height: 550,
            modal: true,
            title: '报告原因',
            draggable: true,
            buttons: [{
                text: '确定退回',
                iconCls: 'icon-ok',
                handler: function () {
                    //Standard_verification 加载列表种获取
                    var selectRow_verification = $("#Standard_verification").datagrid("getData");
                    var rowss_verification = $("#Standard_verification").datagrid("getData");
                    var return_info = $("#return_info1").textbox("getText");
                    var Project_name1 = [];
                    var Project_names = "";

                    if (selectRow_verification.total != 0) {
                        for (var i = 0; i < rowss_verification.rows.length; i++) {
                            Project_name1.push(rowss_verification.rows[i].Project_name);
                        }
                        Project_names = Project_name1.join(',');
                        if (Project_names == "其他" && return_info == "") {
                            $.messager.alert('提示', '退回原因不能为空！');
                            return;
                        }
                    } else {
                        if (return_info == "") {
                            $.messager.alert('提示', '退回原因不能为空！');
                            return;
                            Project_names = "";
                        }
                    }
                    $('#Back_report_dialog').form('submit', {
                        url: "/ReportManagement/BackIssueReport",//接收一般处理程序返回来的json数据     
                        onSubmit: function (param) {
                            param.cmd = 'BackIssueReport';
                            param.id = selectRow.id;
                            param.Inspection_personnel = selectRow.Inspection_personnel;
                            param.report_url = selectRow.report_url;
                            param.error_remarks = Project_names;
                            param.report_num = selectRow.report_num;
                            param.return_info = return_info;
                        },
                        success: function (data) {
                            var result = $.parseJSON(data);
                            if (result.Success == true) {
                                $.messager.alert('提示',result.Message);
                                $('#report_issue_task').datagrid('reload');
                                $('#Standard_equipment_info').dialog('close');
                            }
                            else {
                                $.messager.alert('提示',result.Message);

                            }
                        }
                    });
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    //关闭清除json
                    $('#Standard_verification').datagrid('loadData', { total: 0, rows: [] });
                    $('#Standard_equipment_info').dialog('close');
                }
            }]
        });
        $('#Standard_equipment_info').form('reset');
        //叉号关闭时清除选择标准得datagrid
        $(".panel-tool-close").unbind('click').bind('click', function () {
            //关闭清除json
            $('#Standard_verification').datagrid('loadData', { total: 0, rows: [] });
            $('#Standard_equipment_info').dialog('close');
        });
        //已授权退回原因
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
            //queryParams: {
            //    test_project: selectRow.test_project
            //},
            url: "/ReportManagement/AllErrorInfo",//接收一般处理程序返回来的json数据 
            columns: [[
               { field: 'Project_name', title: '退回信息' }
            ]],
            onDblClickRow: function (index, row) {
                $('#Standard_add').click();
            },
            onLoadSuccess: function (data) {
                $('#Standard_Authorized').datagrid('selectRow', 0);
            }
        });
        //定义pagination加载内容
        var p = $('#Standard_Authorized').datagrid('getPager');
        (p).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });

        //已邮报告原因设备
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
            url: "/ReportManagement/ReturnErrorInfo",//接收一般处理程序返回来的json数据 
            columns: [[
                { field: 'Project_name', title: '退回信息' }
            ]],
            onDblClickRow: function (index, row) {
                $('#Standard_remove').click();
            },
            onLoadSuccess: function (data) {
                $('#Standard_verification').datagrid('selectRow', 0);
            }
        });
        //定义pagination加载内容
        var p = $('#Standard_verification').datagrid('getPager');
        (p).pagination({
            layout: ['first', 'prev', 'last', 'next']
        });

        //添加报告原因
        $('#Standard_add').unbind('click').bind('click', function () {
            var rowss2 = $('#Standard_Authorized').datagrid('getSelections'); //获取选中数据

            //var ids1 = [];
            //for (var i = 0; i < rowss2.length; i++) {
            //    ids1.push(rowss2[i].id);
            //}
            //var ids = ids1.join(';');

            var selectRow_Authorized = $('#Standard_Authorized').datagrid('getSelected');
            //将选中的行转换成datagrid所需要的格式
            //已选择得试验
            var rows_chose = $("#Standard_verification").datagrid("getRows");//获取已选试验得所有数据
            var json = { "total": 0, "rows": [] }
            if (selectRow_Authorized) {
                json.total = rows_chose.length + rowss2.length;
                json.rows = rows_chose;
                json.rows.push(rowss2[0]);
                $.unique(json.rows);
                $('#Standard_verification').datagrid('loadData', json); //将数据绑定到datagrid  
            } else {
                $.messager.confirm('确认', '请选择要操作的原因！');
            }

        });

        //删除报告原因
        $('#Standard_remove').unbind('click').bind('click', function () {
            var rowss3 = $('#Standard_verification').datagrid('getSelections'); //获取选中数据

            //var records_nums1 = [];
            //for (var i = 0; i < rowss3.length; i++) {
            //    records_nums1.push(rowss3[i].records_num);
            //}
            //var records_nums = records_nums1.join(';');

            var rowss3 = $('#Standard_verification').datagrid('getSelections'); //获取选中数据
            var selectRow_verification = $("#Standard_verification").datagrid("getSelected");
            //将选中的行转换成datagrid所需要的格式
            var json = { "total": 0, "rows": [] }
            if (selectRow_verification) {
                json.total = rowss3.length;
                json.rows = rowss3;
                //var data = $.parseJSON(selectRow_Authorized);
                //获取选中行
                var rowIndex = $('#Standard_verification').datagrid('getRowIndex', selectRow_verification);
                $('#Standard_verification').datagrid('deleteRow', rowIndex);
            } else {
                $.messager.confirm('确认', '请选择要移除的原因！');
            }


        });

    } else {
        $.messager.alert('提示', '请选择你要操作得行');
    }
}
//=========================================
//报告完成
function submit_report() {
    var selected_report = $("#report_issue_task").datagrid("getSelected");
    var rowss = $("#report_issue_task").datagrid("getSelections");
    var id = [];
    var report_num = [];
    var report_url = [];
    for (var i = 0; i < rowss.length; i++) {
        id.push(rowss[i].id);
        report_num.push(rowss[i].report_num);
        report_url.push(rowss[i].report_url);
    }
    var ids = id.join(",");
    var report_nums = report_num.join(",");
    var report_urls = report_url.join(",");

  
    if (selected_report) {
        $('#level_date').datetimebox("setValue", loaddatetime(new Date()));
        $.messager.confirm('报告完成', '您确认要提交报告吗', function (r) {
            if (r) {
                $.ajax({
                    url: "/ReportManagement/SubmitIssueReport",
                    type: 'POST',
                    data: {
                        report_nums: report_nums,
                        report_urls: report_urls,
                        ids: ids
                    },
                    success: function (data) {
                        var result = $.parseJSON(data);
                        console.log(result)
                        if (result.Success == true) {
                            $.messager.alert('提示', result.Message);
                            $('#report_issue_task').datagrid('reload');

                        } else {
                            $.messager.alert('提示', result.Message);
                        }

                    }
                });
            }
        });

    }
    else {
        $.messager.alert('提示', '请选择要操作的行！');
    }
}

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
//=========================================
//管理待处理/已处理
function management_all() {

    $('#management_all').combobox({
        value: 0,
        data: [
              { 'value': '0', 'text': '待签发' },
              { 'value': '1', 'text': '历史签发' }
        ],
        onSelect: function () {

            //历史编制  1    未提交 0

            load_list();
        }
    });

}