
$(function () {
    report_management();
    //报告搜索
    $('#report_search').unbind('click').bind('click', function () {
        report_search();
    });
    //下载报告
    $('#download_info').unbind('click').bind('click', function () {
        var rows = $("#report_management").datagrid("getSelections");
        download_info(rows);
    });
    //预览打印报告pdf
    $('#download_report_pdf').unbind("click").bind("click", function () {
        download_report_pdf();
    });

    //搜索
    $('#search').combobox({
        value: 'Subassembly_name',
        data: [
                { 'value': 'Subassembly_name', 'text': '部件名称' },
                { 'value': 'report_name', 'text': '报告名称' },
                { 'value': 'Job_num', 'text': '工号' },
                { 'value': 'circulation_NO', 'text': '流转卡号' },
                { 'value': 'Inspection_date', 'text': '检验时间' },//检验时间
                { 'value': 'Inspection_personnel', 'text': '报告人（用户名）' },//检验人
                { 'value': 'report_num', 'text': '报告编号' },
                { 'value': 'Audit_personnel', 'text': '审核人（用户名）' }
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
                { 'value': 'Audit_personnel', 'text': '审核人' }
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
                { 'value': 'Audit_personnel', 'text': '审核人' }
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
                { 'value': 'Audit_personnel', 'text': '审核人' }

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
    //页面加载
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
                PageId: '113',
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
                            url: "LosslessReport_Monitor.ashx?&cmd=load_list",//接收一般处理程序返回来的json数据        
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

    //加载页面
    function report_search() {
        $('#report_management').datagrid(
          {
              url: "LosslessReport_Monitor.ashx?&cmd=load_list",//接收一般处理程序返回来的json数据    
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
})
function download_info(rows) {
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
            DownloadCheck: '0',
        //    search: $("#search").combobox('getValue'),
        //    key: $("#key").textbox('getText'),
        //    search1: $("#search1").combobox('getValue'),
        //    key1: $("#key1").textbox('getText'),
        //    search2: $("#search2").combobox('getValue'),
        //    key2: $("#key2").textbox('getText'),
        //    search3: $("#search3").combobox('getValue'),
        //    key3: $("#key3").textbox('getText')
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
//下载报告
//function download_info() {
//    var selected_report = $("#report_management").datagrid("getSelected");

//    if (selected_report && selected_report.state_ != '6') {
//        //if (selected_report.accessory_format != "") {
//        console.log(selected_report.report_url);
//        // window.location.href = ;
//        //为添加downloam名字
//        var str = selected_report.report_url.split("/");
//        var str1 = str[3];
//        var str2 = str1.split('.');
//        //console.log(selected_report.report_url);
//        $("#download_info").attr("href", selected_report.report_url);
//        //console.log(selected_report.report_url);
//        console.log(str2[0]);
//        $("#download_info").attr("download", str2[0]);
//        //console.log(str2[0]);
//        //} else {
//        //    $.messager.alert('提示', '没有上传附件文件！');
//        //}
//    } else {
//        $.messager.alert('提示', '请选择操作行！');
//    }
//}

//预览报告文件
function download_report_pdf() {


    var selected_report = $("#report_management").datagrid("getSelected");

    if (selected_report && selected_report.state_!='6') {
        $.messager.progress({
            text: '正在打开报告.....'
        });
        $.ajax({
            url: "LosslessReport_Monitor.ashx?&cmd=Preview_Report",
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
        $.messager.alert('提示', '预览失败');
    }
}