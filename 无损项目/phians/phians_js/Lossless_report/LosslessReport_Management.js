//报告信息

$(function () {
    //检测报告信息
    /************报告管理******************/
    report_management();
    /*************报告管理******************/
    // report_management();
    //搜索
    $('#search').combobox({
        value: 'Subassembly_name',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告名称' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' },
                { 'value': 'EditOverdue', 'text': '编制逾期' },
                { 'value': 'ReviewOverdue', 'text': '审核逾期' },
                { 'value': 'IssueOverdue', 'text': '签发逾期' }
                //{ 'value': 'report_num', 'text': '报告编号' },
                //{ 'value': 'Job_num', 'text': '工号' },
                //{ 'value': 'circulation_NO', 'text': '流转卡号' },
                ////{ 'value': '', 'text': '报告类型' },
                //{ 'value': 'Project_name', 'text': '产品（项目名称）' },
                ////{ 'value': '', 'text': '零件', hidden:true},
                //{ 'value': 'Subassembly_name', 'text': '部件' },
                //{ 'value': 'clientele_department', 'text': '委托部门' },
                //{ 'value': 'contract_num', 'text': '委托编号' }
        ],
        onSelect: function () {
            var value = $('#search').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span").html('<input id="key" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span'));

            } else {
                $("#key_span").html('<input id="key" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span");
            }
        }

    });

    $('#report_arrange_group').combobox({
        value: 'report_Audit',
        data: [
                { 'value': 'report_Audit', 'text': '报告审核' },
                { 'value': 'report_Edit', 'text': '报告编辑' },
                { 'value': 'report_Issue', 'text': '报告签发' },
                { 'value': 'report_Type', 'text': '报告类型' },
                { 'value': 'passing_rate', 'text': '一次通过率' },
                { 'value': 'error_type', 'text': '错误类型' },
                { 'value': 'Edittime', 'text': '编制耗时' },
                { 'value': 'Reviewtime', 'text': '审核耗时' },
                { 'value': 'Issuetime', 'text': '批准耗时' },
                { 'value': 'Alltime', 'text': '总耗时' }
        ],
        onLoadSuccess: function () {
            $("#group_personal").show();//组
            $("#personal").hide();//人员
            $("#allTime").hide();//人员
            
            $("#statistics_type").show();//所有组或者组内人
        },
        onSelect: function () {
            var formatter = $("#report_arrange_group").combobox("getValue");
            if (formatter == "report_Audit") {
                $("#group_personal").show();//组
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").hide();//人员
            } else if (formatter == "report_Edit") {
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#group_personal").show();//组
                $("#allTime").hide();//人员
            } else if (formatter == "report_Issue") {
                $("#personal").hide();//人员
                $("#group_personal").show();//组
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").hide();//人员
            } else if (formatter == "report_Type") {
                $("#personal").hide();//人员
                $("#group_personal").hide();//组
                $("#statistics_type").hide();//所有组或者组内人
                $("#allTime").hide();//人员
            } else if (formatter == "passing_rate") {
                $("#personal").hide();//人员
                $("#group_personal").show();//组
                $("#statistics_type").hide();//所有组或者组内人
                $("#statisticsChk2").prop("checked", "checked");
            } else if (formatter == "error_type") {
                $("#personal").show();//人员
                $("#group_personal").show();//组
                $("#statistics_type").hide();//所有组或者组内人
                $("#allTime").hide();//人员
            } else if (formatter == "error_type") {
                $("#personal").show();//人员
                $("#group_personal").show();//组
                $("#statistics_type").hide();//所有组或者组内人
                $("#allTime").hide();//人员
            }
            else if (formatter == "Edittime") {
                $("#group_personal").show();//组
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
            }
            else if (formatter == "Reviewtime") {
                $("#group_personal").show();//组
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
            }
            else if (formatter == "Issuetime") {
                $("#group_personal").show();//组
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
            }
            else if (formatter == "Alltime") {
                $("#group_personal").show();//组
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
            }
        }
      
    });
    $('#search1').combobox({
        value: 'report_name',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告类别' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' },
                { 'value': 'EditOverdue', 'text': '编制逾期' },
                { 'value': 'ReviewOverdue', 'text': '审核逾期' },
                { 'value': 'IssueOverdue', 'text': '签发逾期' }
        ],
        onSelect: function () {
            var value = $('#search1').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span1").html('<input id="key1" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span1'));

            } else {
                $("#key_span1").html('<input id="key1" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span1");
            }
        }
    });
    $('#search2').combobox({
        value: 'Job_num',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告类别' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' },
                 { 'value': 'EditOverdue', 'text': '编制逾期' },
                { 'value': 'ReviewOverdue', 'text': '审核逾期' },
                { 'value': 'IssueOverdue', 'text': '签发逾期' }
        ],
        onSelect: function () {
            var value = $('#search2').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span2").html('<input id="key2" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span2'));

            } else {
                $("#key_span2").html('<input id="key2" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span2");
            }
        }
    });
    $('#search3').combobox({
        value: 'report_num',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告类别' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人' },
                { 'value': 'EditOverdue', 'text': '编制逾期' },
                { 'value': 'ReviewOverdue', 'text': '审核逾期' },
                { 'value': 'IssueOverdue', 'text': '签发逾期' }

        ],
        onSelect: function () {
            var value = $('#search3').combobox('getValue');
            if (value == "Inspection_date") {
                $("#key_span3").html('<input id="key3" style="width: 110px;" class="easyui-datebox"  />')
                //  $('#key1').attr('class', 'easyui-datebox');
                $.parser.parse($('#key_span3'));

            } else {
                $("#key_span3").html('<input id="key3" style="width: 110px;" class="easyui-textbox"  />')
                $.parser.parse("#key_span3");
            }
        }
    });

    $('#read_search').combobox({
        data: [
                { 'value': 'accessory_name', 'text': '附件名称' },
        ]
    });
    //加载下载条件
    loadcheckdownload();

    //报告预览
    $('#view_report').unbind('click').bind('click', function () {
        view_report();
    });
    //下载报告word版
    $('#Download_word').unbind("click").bind("click", function () {
        Download_word();
    });

    //报告打印
    $('#report_print').unbind('click').bind('click', function () {
        report_print();
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
    //报告搜索
    $('#report_search').unbind('click').bind('click', function () {
        report_search();
    });
    //修改申请
    $('#edit_apply').unbind('click').bind('click', function () {
        edit_apply();
    });
    //报废申请
    $('#ScrapApply').unbind('click').bind('click', function () {
        ScrapApply();
    });
    //加载统计报告页面
    $('#report_arrange').unbind('click').bind('click', function () {
        report_arrangeMain();
        //加载统计条件
        check_statistics();
        //loadcheckstatistics();

    });


    //统计报告数据查询
    $('#date_select_').unbind('click').bind('click', function () {
        var formatter = $("#report_arrange_group").combobox("getValue");
        echart1("container", "bar", formatter);
    });

    //批量下载
    $('#BatchDownload').unbind('click').bind('click', function () {
        BatchDownload();
    });

    //预览打印报告pdf
    $('#download_report_pdf').unbind("click").bind("click", function () {
        download_report_pdf();
    });
    //查看附件
    $('#read_report_info').unbind('click').bind('click', function () {

        read_report_info();
    });
    //附件搜索
    $('#read_search_info').unbind('click').bind('click', function () {
        var selectRow = $("#report_management").datagrid("getSelected");
        var search = $("#read_search").combobox('getValue');
        var key = $("#read_search1").textbox('getText');
        $('#read_table').datagrid({
            queryParams: {
                report_id: selectRow.id,
                search: search,
                key: key
            },
            dataType: "json",
            url: "LosslessReport_Edit.ashx?&cmd=load_accessory"//接收一般处理程序返回来的json数据    
        });
    });

});
//下载报告
function Download_word() {
    var selected_files = $("#report_management").datagrid("getSelected");//获取选中报告的行的信息
    if (selected_files) {
        $.ajax({
            url: "LosslessReport_Management.ashx?&cmd=download_word",//接收一般处理程序返回来的json数据 
            type: 'POST',
            data: {
                file_Url: selected_files.report_url,
                report_num: selected_files.report_num,
                report_format: selected_files.report_format
            },
            success: function (data) {
                window.location.href = data
                $.messager.alert("提示", "下载成功！")
            }
        });
    } else {
        $.messager.alert("提示", "请选择您需要下载的报告！")
    }
};
//查看附件
function read_report_info() {
    var selectRow = $("#report_management").datagrid("getSelected");
    var search = $("#read_search").combobox('getValue');
    var key = $("#read_search1").textbox('getText');
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
            key: key
        },
        dataType: "json",
        url: "LosslessReport_Edit.ashx?&cmd=load_accessory",//接收一般处理程序返回来的json数据        
        columns: [[
       { field: 'report_id', title: '报告id', sortable: 'true' },
       { field: 'accessory_name', title: '附件名称' },
       { field: 'accessory_format', title: '文件格式' },
       { field: 'add_personnel', title: '添加人' },
       { field: 'add_date', title: '添加时间' },
       { field: 'remarks  ', title: '说明' }
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

//加载数据表格
function report_management() {
    var tabs_width = screen.width - 182;
    var other_height = window.outerHeight - document.body.clientHeight;
    //iframe可用高度
    var _height = screen.availHeight - 107 - 35 - other_height;
    //显示分页条数
    var _height1 = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 300;
    var num = parseInt(_height1 / 25);
    if (tabs_width < 1110) {
        tabs_width = 1100;

    }
    if (_height < 650) {
        _height = 650;
    }
    var search = $("#search").combobox('getValue');
    var key = $("#key").textbox('getText');
    var search1 = $("#search1").combobox('getValue');
    var key1 = $("#key1").textbox('getText');
    var search2 = $("#search2").combobox('getValue');
    var key2 = $("#key2").textbox('getText');
    var search3 = $("#search3").combobox('getValue');
    var key3 = $("#key3").textbox('getText');
    $.ajax({
        url: "LosslessReport_Edit.ashx?&cmd=loadPageShowSetting",
        type: 'POST',
        data: {
            PageId: '104',
        },
        success: function (result) {
            if (result) {
                $('#report_management').datagrid(
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
                        url: "LosslessReport_Management.ashx?&cmd=load_list",//接收一般处理程序返回来的json数据        
                        columns: [[
                         { field: 'id', title: '序号', hidden: true, sortable: false },
                            { field: 'clientele_department', title: '用户(委托部门)', hidden: true, sortable: false },
                            { field: 'clientele', title: '委托人', hidden: false, sortable: false },
                            { field: 'application_num', title: '订单号', hidden: true, sortable: false },
                            { field: 'Project_name', title: '项目名称', hidden: true, sortable: false },
                            { field: 'Subassembly_name', title: '部件名称', hidden: true, sortable: false },
                            { field: 'Material', title: '材质', hidden: false, sortable: false },
                            { field: 'Type_', title: '规    格', hidden: false, sortable: false },
                            { field: 'Chamfer_type', title: '坡口型式', hidden: false, sortable: false },
                            { field: 'Drawing_num', title: '图号', hidden: false, sortable: false },
                            { field: 'Procedure_', title: '检验规程', hidden: false, sortable: false },
                            { field: 'Inspection_context', title: '检验内容', hidden: false, sortable: false },
                            { field: 'Inspection_opportunity', title: '检验时机', hidden: false, sortable: false },
                            { field: 'circulation_NO', title: '流转卡号', hidden: true, sortable: false },
                            { field: 'procedure_NO', title: '工序号', hidden: false, sortable: false },
                            { field: 'apparent_condition', title: '表面状态', hidden: false, sortable: false },
                            { field: 'manufacturing_process', title: '制造工艺', hidden: false, sortable: false },
                            { field: 'Batch_Num', title: '批次号', hidden: false, sortable: false },
                            { field: 'Inspection_NO', title: '检验编号', hidden: false, sortable: false },
                            {
                                field: 'Inspection_date', title: '检验日期', hidden: true, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length > 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }
                            },
                            {
                                field: 'Inspection_personnel_date', title: '检验人签字时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length > 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }
                            },
                            { field: 'Inspection_personnel', title: '检验人', hidden: false, sortable: false },
                            { field: 'level_Inspection', title: '检验级别', hidden: false, sortable: false },
                            { field: 'Audit_personnel', title: '审核人员', hidden: false, sortable: false },
                            { field: 'Audit_date', title: '审核时间', hidden: false, sortable: false },
                            { field: 'return_info', title: '退回原因', hidden: false, sortable: false },
                            { field: 'level_Audit', title: '审核级别', hidden: false, sortable: false },
                            { field: 'issue_personnel', title: '签发人员', hidden: false, sortable: false },
                            { field: 'issue_date', title: '签发时间', hidden: false, sortable: false },
                            { field: 'laboratorian_', title: '授权检验师', hidden: false, sortable: false },
                            {
                                field: 'laboratorian_date', title: '授权检验师时间', hidden: false, sortable: false, formatter: function (value, row, index) {
                                    if (value) {
                                        if (value.length > 10) {
                                            value = value.substr(0, 10)
                                            return value;
                                        }
                                    }
                                }
                            },
                            { field: 'figure', title: '附图', hidden: false, sortable: false },
                            { field: 'disable_report_num', title: '不符合项报告号', hidden: false, sortable: false },
                            { field: 'report_num', title: '总报告编号', hidden: true, sortable: false },
                            { field: 'report_format', title: '报告格式', hidden: false, sortable: false },
                            { field: 'report_name', title: '报告名称', hidden: true, sortable: false },
                            { field: 'Inspection_result', title: '检验结果', hidden: false, sortable: false },
                            { field: 'remarks', title: '备注', hidden: true, sortable: false },
                            {
                                field: 'state_', title: '状态', hidden: 'true', sortable: 'false',
                                formatter: function (value, row, index) {
                                    if (value == "1") { return "编辑"; } if (value == "2") { return "审核"; } if (value == "3") { return "签发"; } if (value == "4") { return "完成"; }
                                }
                            },
                            { field: 'Tubes_Size', title: '管子规格', hidden: false, sortable: false },
                            { field: 'Tubes_num', title: '管子数量', hidden: false, sortable: false },
                            { field: 'welding_method', title: '焊接方法', hidden: false, sortable: false },
                            { field: 'Job_num', title: '工号', hidden: true, sortable: false },
                            { field: 'heat_treatment', title: '热处理设备', hidden: false, sortable: false },
                            { field: 'Work_instruction', title: '作业指导书', hidden: false, sortable: false },
                            { field: 'ReportCreationTime', title: '报告创建时间', hidden: false, sortable: false }
                        ]],
                        
                        onBeforeLoad: function () {
                            var hiddenJson = $.parseJSON(result);
                            for (var i = 0; i < hiddenJson.length; i++) {
                                try {
                                    if (hiddenJson[i].hidden) {
                                        $('#report_management').datagrid('hideColumn', hiddenJson[i].fieldname);
                                    } else {
                                        $('#report_management').datagrid('showColumn', hiddenJson[i].fieldname);
                                    }
                                } catch (e) {

                                }
                               
                            }
                        },
                        queryParams: {
                            search: search,
                            key: key,
                            search1: search1,
                            key1: key1,
                            search2: search2,
                            key2: key2,
                            search3: search3,
                            key3: key3
                        },
                        onLoadSuccess: function (data) {
                            //默认选择行
                            $('#report_management').datagrid('selectRow', 0);
                        },
                        onSelect: function () {
                            //提示
                            //$('#report_management').datagrid('doCellTip', { cls: { 'background-color': 'red' }, delay: 1000 });
                            //view_taskinfo1();
                        },
                        sortOrder: 'asc',
                        toolbar: "#report_management_toolbar"
                    });
            }
            else {
                //  $.messager.alert('提示', data);
            }

        }

    });
}
//---预览报告
var new_url = $("#read_doc").prop("href");//+ "___hxw";
function view_report() {

    var selected_report = $("#report_management").datagrid("getSelected");
    if (selected_report) {
        $.ajax({
            url: "",
            type: 'POST',
            success: function (data) {
                var url = selected_report.report_url;

                $("#read_doc").prop("href", new_url + "___hxw&url=" + url);
                document.getElementById('read_doc').click();
            }
        });
    } else {
        $.messager.alert('提示', '请选择需要操作的报告');

    }
}

//修改申请
function edit_apply() {
    $('#group').combobox({
        url: "LosslessReport_Edit.ashx?&cmd=load_professional_department",
        valueField: 'id',
        textField: 'Department_name',
        onSelect: function () {
            var User_department = $('#group').combobox('getValue');
            $('#review_personnel').combobox({
                url: "LosslessReport_Edit.ashx?&cmd=load_responsible_people",
                valueField: 'User_count',
                textField: 'User_name',
                queryParams: {
                    "User_department": User_department
                }
            });
        },
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    var selectRow = $("#report_management").datagrid("getSelected");
    if (selectRow) {
        $('#importFileForm1').dialog({
            width: 500,
            height: 300,
            modal: true,
            title: '修改申请',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'editsave'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importFileForm1').dialog('close');
                }
            }]
        });
        var rowss = $("#report_management").datagrid("getSelections");
        $('#importFileForm1').form("load", rowss[0]);
        $('#editsave').unbind('click').bind('click', function () {
            if ($("#error_remark1").textbox("getText") == "") {
                $.messager.alert('消息', '申请原因不能为空');
                return;
            }
            //if ($("#other_remarks1").textbox("getText") == "") {
            //    $.messager.alert('消息', '说明不能为空');
            //    return;
            //}
            //form表单提交
            $('#importFileForm1').form('submit', {
                url: "LosslessReport_Management.ashx",
                onSubmit: function (param) {
                    param.cmd = 'Update_Report_Apply';
                    param.report_id = selectRow.id;
                    param.File_url = selectRow.report_url;
                    param.report_num = selectRow.report_num;
                    param.constitute_personnel = selectRow.Inspection_personnel;

                },
                success: function (data) {
                    if (data == 'T') {
                        $.messager.alert('提示', '修改报告成功');
                        $('#importFileForm1').dialog('close');
                        //$('#not_print_report_management').datagrid('reload');
                    }
                    else if (data == "F") {
                        $.messager.alert('提示', '修改报告失败！');
                    }
                    else {
                        $.messager.alert('提示', data);
                    }
                }
            });

        });
    } else {
        $.messager.alert('提示', "请选择需要操作的报告");
    }
}
//报废申请
function ScrapApply() {
    $('#group').combobox({
        url: "LosslessReport_Edit.ashx?&cmd=load_professional_department",
        valueField: 'id',
        textField: 'Department_name',
        onSelect: function () {
            var User_department = $('#group').combobox('getValue');
            $('#review_personnel').combobox({
                url: "LosslessReport_Edit.ashx?&cmd=load_responsible_people",
                valueField: 'User_count',
                textField: 'User_name',
                queryParams: {
                    "User_department": User_department
                }
            });
        },
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    var selectRow = $("#report_management").datagrid("getSelected");
    if (selectRow) {
        $('#importFileForm1').dialog({
            width: 500,
            height: 300,
            modal: true,
            title: '报废申请',
            draggable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                id: 'ScrapSave'
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#importFileForm1').dialog('close');
                }
            }]
        });
        var rowss = $("#report_management").datagrid("getSelections");
        $('#importFileForm1').form("load", rowss[0]);
        $('#ScrapSave').unbind('click').bind('click', function () {
            if ($("#error_remark1").textbox("getText") == "") {
                $.messager.alert('消息', '申请原因不能为空');
                return;
            }
            //if ($("#other_remarks1").textbox("getText") == "") {
            //    $.messager.alert('消息', '说明不能为空');
            //    return;
            //}
            //form表单提交
            $('#importFileForm1').form('submit', {
                url: "LosslessReport_Management.ashx",
                onSubmit: function (param) {
                    param.cmd = 'Update_Report_Apply';
                    param.flag = "Scrap";
                    param.report_id = selectRow.id;
                    param.File_url = selectRow.report_url;
                    param.report_num = selectRow.report_num;
                    param.constitute_personnel = selectRow.Inspection_personnel;

                },
                success: function (data) {
                    if (data == 'T') {
                        $.messager.alert('提示', '申请成功');
                        $('#importFileForm1').dialog('close');
                        $('#report_management').datagrid('reload');
                    }
                    else if (data == "F") {
                        $.messager.alert('提示', '申请失败！');
                    }
                    else {
                        $.messager.alert('提示', data);
                    }
                }
            });

        });
    } else {
        $.messager.alert('提示', "请选择需要操作的报告");
    }
}
//加载统计报告
function report_arrangeMain() {
    $('#report_arrangeMain').dialog({
        //width: 900,
        //height: 500,
        fit: true,
        modal: true,
        title: '报告统计',
        draggable: true,
        buttons: [{
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#report_arrangeMain').dialog('close');
            }

        }]
    })
};

//加载柱状图
function echart1(id, type, formatter) {

    // 基于准备好的dom，初始化echarts实例
    var new_x = new Array();
    var new_x_data = new Array();
    var yformatter;
    var sformatter;

    var person = 'error_count';
    // 基于准备好的dom，初始化echarts实例
    if (formatter == 'passing_rate') {
        yformatter = '{value}%';
        sformatter = '{c}%';
    }
    else {
        yformatter = '{value}';
        sformatter = '{c}';
    }
    //if(formatter==''){
    //    person = $("#error_personnel").combobox("getValue");
    //}
    var myChart = echarts.init(document.getElementById(id));
    //var combox=$('#comboboxlist').combobox('getValue');
    var combobox_value = $('#report_arrange_group').combobox('getText');
    var group_combobox = $('#comboxgroup').combobox('getText');
    //alert($("#comboxgroup").is(':visible'));
    //if ($("#comboxgroup").is(':visible')) {
    //};
    var title_text = group_combobox+combobox_value +"统计";
    var option = {
        color: ['#3398DB'],
        title: {
            text: title_text,
            x: 'center'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        toolbox: {

            show: true,

            feature: {

                saveAsImage: {

                    show: true,

                    excludeComponents: ['toolbox'],

                    pixelRatio: 2
                }
            }

        },
        grid: {
            left: '15%',
            //right: '4%',
            //bottom: '3%',
            //containLabel: true,
            y2: 140
        },
        xAxis: [
            {
                type: 'category',
                // data : ['部门1部门1', '部门1部门1部门', '部门1部门1部门', '部门4','部门5', '部门6', '部门8', '部门9', '部门10', '部门11', '部门12'],
                axisTick: {
                    alignWithLabel: true
                },
                axisLabel: {
                    interval: 0,//横轴信息全部显示  
                    rotate: 35,//-30度角倾斜显示 

                }
            }
        ],
        yAxis: [
            {
                type: 'value',
                min: 0,
                axisLabel: {
                    formatter: yformatter
                }

            }
        ],
        series: [
            {
                name: '统计',
                type: type,
                barWidth: '60%',
                label: {
                    normal: {
                        show: true,
                        position: 'top',
                        formatter: sformatter
                    }
                }
            }
        ]
    };
    var rds = document.getElementsByName("statistics");
    var rdVal;
    for (var i = 0; i < rds.length; i++) {
        if (rds.item(i).checked) {
            rdVal = rds.item(i).getAttribute("value");
            break;
        }
        else {
            continue;
        }
    }
    $.ajax({
        type: "post",
        async: true,            //异步请求（同步请求将会锁住浏览器，用户其他操作必须等待请求完成才可以执行）
        url: "LosslessReport_Management.ashx?&cmd=Report_Arrange",
        data: {
            report_type: $("#report_arrange_group").combobox("getValue"),
            checktype: rdVal,
            dateStart: $("#dateStart").textbox("getText"),
            dateEnd: $("#dateEnd").textbox("getText"),
            comboxgroup: $("#comboxgroup").combobox("getText"),
            day: $("#allTimeH").textbox('getText'),
            comboxperson: $("#comboxperson").combobox("getValue")

        },
        success: function (data) { // 接口调用成功回调函数
            // data 为服务器返回的数据
            if (data == 'F') {
                $.messager.alert('提示', '失败！');
            } else if (data == '开始时间大于结束时间') {
                $.messager.alert('提示', '开始时间大于结束时间！');
            } else if (data == '数据为空') {
                $.messager.alert('提示', '数据为空！');
            }

            else {
                var data_1 = JSON.parse(data);
                for (var i = 0; i < data_1.length; i++) {
                    new_x.push(data_1[i].User_name);
                    new_x_data.push((data_1[i].sumCount));
                }
                //获取Y轴最大值
                var ymax = Math.max.apply(null, new_x_data);
                ymax = ymax + (10 - ymax % 10) + 10;
                myChart.setOption({
                    xAxis: {
                        data: new_x
                    },
                    yAxis: {
                        max: ymax,
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        data: new_x_data
                    }]
                });
            }
        }
    });
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
}

//报告管理搜索
function report_search() {
    $('#report_management').datagrid(
      {
          url: "LosslessReport_Management.ashx?&cmd=load_list",//接收一般处理程序返回来的json数据    
          queryParams: {
              search: $("#search").combobox('getValue'),
              key: $("#key").textbox('getText'),
              search1: $("#search1").combobox('getValue'),
              key1: $("#key1").textbox('getText'),
              search2: $("#search2").combobox('getValue'),
              key2: $("#key2").textbox('getText'),
              search3: $("#search3").combobox('getValue'),
              key3: $("#key3").textbox('getText')

          },
          onLoadSuccess: function (data) {
              $('#report_management').datagrid('selectRow', 0);
          }
      }).datagrid('resize');
}

//报告打印
var new_url1 = $("#Certificate_print").prop("href");//+ "___hxw";
function report_print() {
    var selected_report = $("#report_management").datagrid("getSelected");

    if (selected_report) {
        var url = selected_report.report_url;
        $("#Certificate_print").prop("href", new_url1 + "___hxw&url=" + url);
        document.getElementById('Certificate_print').click();
    } else {
        $.messager.alert('提示', '请选择需要操作的报告');

    }
}
//加载人员组
function check_statistics() {
    $('#comboxgroup').combobox({
        url: "LosslessReport_Management.ashx?&cmd=select_group",
        valueField: 'User_count',
        textField: 'User_name',
        required: true,
        onLoadSuccess: function (data) {
            var val = $(this).combobox('getData');
            $(this).combobox('select', val[0].User_count);
            $('#comboxperson').combobox({
                url: "LosslessReport_Management.ashx?&cmd=select_person",
                valueField: 'User_count',
                textField: 'User_name',
                queryParams: {
                    "comboxgroup": $("#comboxgroup").combobox("getText")
                },
                //本地联系人数据模糊索引
                filter: function (q, row) {
                    var opts = $(this).combobox('options');
                    return row[opts.textField].indexOf(q) >= 0;
                },
                onSelect: function () {
                    var formatter = $("#report_arrange_group").combobox("getValue");

                }

                //echart1("container", "bar", formatter);
                //}
            });
            //echart1("container", "bar", "report_num");
        },

        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onSelect: function () {
            var formatter = $("#report_arrange_group").combobox("getValue");
            var val = $(this).combobox('getData');
            // $(this).combobox('select', val[0].User_count);
            $('#comboxperson').combobox({
                url: "LosslessReport_Management.ashx?&cmd=select_person",
                valueField: 'User_count',
                textField: 'User_name',
                queryParams: {
                    "comboxgroup": $("#comboxgroup").combobox("getText")
                },
                //本地联系人数据模糊索引
                filter: function (q, row) {
                    var opts = $(this).combobox('options');
                    return row[opts.textField].indexOf(q) >= 0;
                },
                //onSelect: function () {
                //    var formatter = $("#report_arrange_group").combobox("getValue");
                //    echart1("container", "bar", formatter);
                //}
            });
            echart1("container", "bar", formatter);

        }
    })
}
//function loadcheckstatistics() {
//    //按搜索条件下载
//    $('#statisticsChk1').unbind('click').bind('click', function () {
//        $("#statisticsChk1").prop("checked", "checked");
//        $("#statisticsChk2").prop("checked", false);
//        $("#statisticsChk").val("1");
//        //$("#comboxgroup").combobox('setText', "");
//        $("#comboxperson").combobox('setText',"");
//        var formatter = $("#report_arrange_group").combobox("getValue");
//        echart1("container", "bar", formatter);
//    });
//    //按选择下载
//    $('#statisticsChk2').unbind('click').bind('click', function () {
//        $("#statisticsChk2").prop("checked", "checked");
//        $("#statisticsChk1").prop("checked", false);
//        $("#statisticsChk").val("2");
//        $("#comboxperson").combobox('setText', "");
//        var selected_group = $("#comboxgroup").combobox("getText");
//        if (selected_group=='') {
//            $.messager.alert("提示", "请选择组")
//        }
//        else {
//            var formatter = $("#report_arrange_group").combobox("getValue");
//            echart1("container", "bar", formatter);
//        }

//    });
//}
//加载下载条件
function loadcheckdownload() {
    //按搜索条件下载
    $('#condition1').unbind('click').bind('click', function () {
        $("#condition1").prop("checked", "checked");
        $("#condition2").prop("checked", false);
        $("#condition").val("1");
    });
    //按选择下载
    $('#condition2').unbind('click').bind('click', function () {
        $("#condition2").prop("checked", "checked");
        $("#condition1").prop("checked", false);
        $("#condition").val("0");
    });
}



//下载
function BatchDownload() {
    var DownloadCheck = $("#condition").val();

    //下载标志 DownloadCheck=1 选择下载 DownloadCheck=0 按条件下载

    var rows = $("#report_management").datagrid("getSelections");

    //选择下载
    if (DownloadCheck == "1") {
        if (rows) {
            Download(rows);
        }
        else {
            $.messager.alert("提示", "请选择行")
        }
    }
    else {
        Download(rows);
    }



}
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
        $.messager.alert('提示', '请选择需要操作的报告');
    }
}

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
            url: "LosslessReport_Edit.ashx?&cmd=downloadAccessory",
            dataType: "text",
            type: 'POST',
            data: {
                ids: ids,
            },
            success: function (data) {
                if (data) {
                    window.location.href = data;
                }
                else {
                    $.messager.alert('提示', '删除信息失败！');
                }

            }
        });
    } else {
        $.messager.alert('提示', '请选择操作行！');
    }
}
//查看附件
function read_report() {
    var selectRow = $("#report_issue_task").datagrid("getSelected");
    var search = $("#read_search").combobox('getValue');
    var key = $("#read_search1").textbox('getText');
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
            key: key
        },
        dataType: "json",
        url: "LosslessReport_Edit.ashx?&cmd=load_accessory",//接收一般处理程序返回来的json数据        
        columns: [[
       { field: 'report_id', title: '报告id', sortable: 'true' },
       { field: 'accessory_name', title: '附件名称' },
       { field: 'accessory_format', title: '文件格式' },
       { field: 'add_personnel', title: '添加人' },
       { field: 'add_date', title: '添加时间' },
       { field: 'remarks  ', title: '说明' }
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

function Download(rows) {
    var id = []
    var report_url = [];
    for (var i = 0; i < rows.length; i++) {
        id.push(rows[i].id);
        report_url.push(rows[i].report_url);

    }
    var ids = id.join(",");
    var report_urls = report_url.join(",");
    $.ajax({
        type: "post",
        async: true,            //异步请求（同步请求将会锁住浏览器，用户其他操作必须等待请求完成才可以执行）
        url: "LosslessReport_Management.ashx?&cmd=download",
        data: {
            report_urls: report_urls,
            ids: ids,
            DownloadCheck: $("#condition").val(),
            search: $("#search").combobox('getValue'),
            key: $("#key").textbox('getText'),
            search1: $("#search1").combobox('getValue'),
            key1: $("#key1").textbox('getText'),
            search2: $("#search2").combobox('getValue'),
            key2: $("#key2").textbox('getText'),
            search3: $("#search3").combobox('getValue'),
            key3: $("#key3").textbox('getText')
        },
        success: function (data) { // 接口调用成功回调函数
            // data 为服务器返回的数据
            if (data == "F") {
                $.messager.alert('提示', '下载失败！');
            } else {

                window.location = data;
            }
        }
    })
}

//预览报告文件
function download_report_pdf() {


    var selected_report = $("#report_management").datagrid("getSelected");

    if (selected_report) {
        $.messager.progress({
            text: '正在打开报告.....'
        });
        $.ajax({
            url: "LosslessReport_Management.ashx?&cmd=Preview_Report",
            dataType: "text",
            type: 'POST',
            data: {
                id: selected_report.id
            },
            success: function (data) {
                $.messager.progress('close');
                if (data == 'F') {
                    $.messager.alert('提示', '预览错误！');
                } else {
                    var url_ = "/pdf_read/web/viewer.html?___hxw&url=" + data;
                    window.parent.addTab(selected_report.report_num + "报告文件查看", url_);
                    $('#download_report_pdf').linkbutton('enable');
                    $('#download_report_pdf').bind("click")
                }
            },
            error: function (e) {
                $.messager.progress('close');
            },
        });

    } else {
        $.messager.alert('提示', '请选择要查看的报告');
    }
}
