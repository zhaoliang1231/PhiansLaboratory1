var lin = 0;

$(function () {
    var tabs_width = screen.width - 182;
    ////设置左边树的样式大小
    //$('#department_layout').layout('panel', 'west').panel('resize', {
    //    width: 300,
    //    //height:300
    //    //height: _height
    //});
    ////设置右边列表的样式大小
    //$('#department_layout').layout('panel', 'east').panel('resize', {
    //    width: tabs_width - 300,
    //    //height:300
    //    //height: _height
    //});

    //$('#department_layout').layout('resize');//页面重置，初始化


   
   
    //科室统计方法调用
    department_statistics()
    //合格不合格
    loadcheckdownload();
    
    
    $('#report_arrange_group').combobox({
        value: 'report_Edit',
        data: [
                { 'value': 'report_Edit', 'text': '报告编辑' },
                { 'value': 'report_Audit', 'text': '报告审核' },
                { 'value': 'report_Issue', 'text': '报告签发' },
                { 'value': 'report_Type', 'text': '报告类型' },
                { 'value': 'passing_rate', 'text': '一次通过率' },
                { 'value': 'error_type', 'text': '错误类型' },
                { 'value': 'Edittime', 'text': '编制耗时' },
                { 'value': 'Reviewtime', 'text': '审核耗时' },
                { 'value': 'Issuetime', 'text': '批准耗时' },
                { 'value': 'Alltime', 'text': '总耗时' },
                { 'value': 'IssueSendBack', 'text': '签发退回次数' },
                { 'value': 'overdue', 'text': '逾期统计' },
                { 'value': 'overdueReportType', 'text': '逾期类型统计' },
                { 'value': 'editAlloverdue', 'text': '编制累计耗时' },
                { 'value': 'ReviewAlloverdue', 'text': '审核累计耗时' },
                { 'value': 'IssueAlloverdue', 'text': '签发累计耗时' },
        ],
        onLoadSuccess: function () {
            $("#department").show(); //科室
            $("#group_personal").show();//组
            $("#overdueStatistics").hide()//逾期统计
            $("#personal").hide();//人员
            $("#allTime").hide();//人员
            $("#frequency").hide();//次数

            $("#statistics_type").show();//所有组或者组内人
        },
        onSelect: function () {
            var formatter = $("#report_arrange_group").combobox("getValue");
            if (formatter == "report_Audit") {
                $("#group_personal").show();//组
                $("#department").show(); //科室
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").hide();//人员
                $("#overdue").hide()//逾期统计
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
                $("#qualifiedAll").hide();//合格不合格
            } else if (formatter == "report_Edit") {
                $("#personal").hide();//人员
                $("#department").show(); //科室
                $("#statistics_type").show();//所有组或者组内人
                $("#group_personal").show();//组
                $("#allTime").hide();//人员
                $("#overdue").hide()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
            } else if (formatter == "report_Issue") {
                $("#personal").hide();//人员
                $("#department").show(); //科室
                $("#group_personal").show();//组
                $("#allTime").hide();//人员
                $("#overdue").hide()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
            } else if (formatter == "report_Type") {
                $("#personal").hide();//人员
                $("#department").hide(); //科室
                $("#group_personal").hide();//组
                $("#allTime").hide();//人员
                $("#overdue").hide()//逾期统计
                $("#ReportTemplate").hide()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#frequency").hide();//次数
            } else if (formatter == "passing_rate") {
                $("#personal").hide();//人员
                $("#department").show(); //科室
                $("#group_personal").show();//组
                $("#overdue").hide()//逾期统计
                $("#allTime").hide();//天数
                $("#qualifiedAll").show();//合格不合格
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
            } else if (formatter == "error_type") {
                //$("#personal").show();//人员
                $("#department").show(); //科室
                $("#group_personal").show();//组
                $("#statistics_type").hide();//所有组或者组内人
                $("#allTime").hide();//人员
                $("#overdue").hide()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
            } //else if (formatter == "error_type") {
            //    $("#personal").show();//人员
            //    $("#department").show(); //科室
            //    $("#group_personal").show();//组
            //    $("#statistics_type").hide();//所有组或者组内人
            //    $("#allTime").hide();//人员
            //    $("#overdue").hide()//逾期统计
            //    $("#qualifiedAll").hide();//合格不合格
            //}
            else if (formatter == "Edittime") {
                $("#group_personal").show();//组
                $("#department").show(); //科室
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
                $("#overdue").hide()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
            }
            else if (formatter == "Reviewtime") {
                $("#group_personal").show();//组
                $("#personal").hide();//人员
                $("#department").show(); //科室
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
                $("#overdue").hide()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
            }
            else if (formatter == "Issuetime") {
                $("#group_personal").show();//组
                $("#department").show(); //科室
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
                $("#overdue").hide()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })
            }
            else if (formatter == "Alltime") {
                $("#group_personal").show();//组
                $("#department").show(); //科室
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#allTime").show();//人员
                $("#overdue").hide()//逾期统计
                $("#frequency").hide();//次数
                $("#qualifiedAll").hide();//合格不合格
            } else if (formatter == "overdue") { //当选择逾期统计时
                $("#group_personal").show();//组
                $("#personal").hide();//人员
                $("#department").show();//组
                $("#overdue").show()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#allTime").hide();//耗时
                $("#ReportTemplate").hide();//逾期类型统计
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required:true
                })
                $('#overdueStatistics').combobox({
                    url: "/StatisticalManagement/GetDicitionaryData",
                    valueField: 'ProjectNameValue',
                    textField: 'Project_name',
                    editable: false,
                    required: true,
                    queryParams: {
                       
                    },
                    //本地联系人数据模糊索引
                    filter: function (q, row) {
                        var opts = $(this).combobox('options');
                        return row[opts.textField].indexOf(q) >= 0;
                    }

                })
            } else if (formatter == "overdueReportType") {
                $("#personal").hide();//人员
                $("#department").hide();//科室
                $("#overdue").show()//逾期统计
                $("#qualifiedAll").hide();//合格不合格
                $("#group_personal").hide();//组
                $("#allTime").hide();//耗时
                $("#personal").hide();//人员
                $("#ReportTemplate").show();//人员
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: true
                })
                $('#GetReportTemplate').combobox({
                    url: "/StatisticalManagement/GetReportTemplate",
                    valueField: 'FileNum',
                    textField: 'FileName',
                    editable: false,
                    required: true,
                    queryParams: {

                    },
                    //本地联系人数据模糊索引
                    filter: function (q, row) {
                        var opts = $(this).combobox('options');
                        return row[opts.textField].indexOf(q) >= 0;
                    }

                })
                //预期统计
                $('#overdueStatistics').combobox({
                    url: "/StatisticalManagement/GetDicitionaryData",
                    valueField: 'ProjectNameValue',
                    textField: 'Project_name',
                    editable: false,
                    required: true,
                    queryParams: {

                    },
                    //本地联系人数据模糊索引
                    filter: function (q, row) {
                        var opts = $(this).combobox('options');
                        return row[opts.textField].indexOf(q) >= 0;
                    }

                })
            } else if (formatter == "editAlloverdue") {
                $("#department").show();
                $("#group_personal").show();
                $("#allTime").show();
                $("#overdue").hide();
                $("#ReportTemplate").hide();
                $("#personal").hide();
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })

            } else if (formatter == "ReviewAlloverdue") {
                $("#department").show();
                $("#group_personal").show();
                $("#allTime").show();
                $("#overdue").hide();
                $("#ReportTemplate").hide();
                $("#personal").hide();
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })

            } else if (formatter == "IssueAlloverdue") {
                $("#department").show();
                $("#group_personal").show();
                $("#allTime").show();
                $("#overdue").hide();
                $("#ReportTemplate").hide();
                $("#personal").hide();
                $("#frequency").hide();//次数
                $('#comboxgroup').combobox({
                    required: false
                })

            } else if (formatter == "IssueSendBack") {
                $("#group_personal").show();//组
                $("#department").show(); //科室
                $("#personal").hide();//人员
                $("#statistics_type").show();//所有组或者组内人
                $("#frequency").show();//次数
                $("#overdue").hide()//逾期统计
                $("#allTime").hide();//耗时
                $('#comboxgroup').combobox({
                    required: false
                })
            }
        }

    });

    //统计报告数据查询
    $('#date_select_').unbind('click').bind('click', function () {
        $("#echartsBox").show();
        var formatter = $("#report_arrange_group").combobox("getValue");
        var StatisticsKey;
        var DepartmentGroupID;
        var GroupName;
        if ($("#comboxdepartment").combobox("getValue") != "" && $("#comboxgroup").combobox("getValue") != "") {
            StatisticsKey = 1; // 统计标识（0/1）
            DepartmentGroupID = $("#comboxdepartment").combobox('getValue'); //科室ID
            GroupName = $("#comboxgroup").combobox('getValue');//组值
            echart1("container", "bar", formatter, StatisticsKey, DepartmentGroupID, GroupName);
        } else if ($("#comboxdepartment").combobox("getValue") != "" && $("#comboxgroup").combobox("getValue") == "") {
            StatisticsKey = 0; // 统计标识（0/1）
            DepartmentGroupID = $("#comboxdepartment").combobox('getValue'); //科室ID
            GroupName = $("#comboxgroup").combobox('getValue');//组值
            echart1("container", "bar", formatter, StatisticsKey, DepartmentGroupID, GroupName);
        }
    });
});


/*
*functionName:
*function:加载柱状图
*Param: id 传入渲染DOM id名（container）
*Param: 图形类型 type
*Param: 统计类型的value值  formatter
*Param: 统计的标识 （0/1)  StatisticsKey
*Param: 科室ID    DepartmentGroupID
*Param: 班组组值  GroupName
*author:赵亮
*date:2018-11-14
*/

function echart1(id, type, formatter, StatisticsKey, DepartmentGroupID, GroupName) {
  
    //统计报告列表查询
    $('#echartsSearch').unbind('click').bind('click', function () {
        echartsSearch(StatisticsKey, DepartmentGroupID, GroupName);

    });
    //统计报告导出
    $('#exportButton').unbind('click').bind('click', function () {
        exportButton(StatisticsKey, DepartmentGroupID, GroupName);

    });
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
    var department_combobox = $("#comboxdepartment").combobox('getText')
    var group_combobox = $('#comboxgroup').combobox('getText');

    //alert($("#comboxgroup").is(':visible'));
    //if ($("#comboxgroup").is(':visible')) {
    //};
    var title_text = group_combobox + department_combobox + combobox_value + "统计";
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
            x: '20%',
            x2:'25%',
            y2: 230
        },
        xAxis: [
            {
                type: 'category',
                data:[],
                axisTick: {
                    alignWithLabel: true,
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
                barWidth: 30,
                label: {
                    normal: {
                        show: true,
                        position: 'top',
                        formatter: sformatter
                    }
                },
                data:[]
            }
        ]
    };

    $.ajax({
        type: "post",
        async: true,            //异步请求（同步请求将会锁住浏览器，用户其他操作必须等待请求完成才可以执行）
        url: "/StatisticalManagement/report_arrange",
        data: {
            StatisticsType: $("#report_arrange_group").combobox('getValue'),
            StatisticsKey: StatisticsKey,
            DepartmentGroupID: DepartmentGroupID,
            GroupName: GroupName,
            ElapsedTimeDay: $("#allTimeH").numberbox('getValue'),
            dateStart: $("#dateStart").datebox('getValue'),
            dateEnd: $("#dateEnd").datebox('getValue'),
            OverdueTimeDay: $("#overdueStatistics").combobox('getValue'),
            ReportOverdueName: $("#overdueStatistics").combobox('getText'),
            ReportTypeName: $("#GetReportTemplate").combobox('getText'),
            SendBackCount: $("#allFrequency").numberbox('getValue')
        },
        success: function (data) { // 接口调用成功回调函数
            var formatter = $("#report_arrange_group").combobox("getValue");
            var comboxgroupValue = $("#comboxgroup").combobox('getValue')
            //if (formatter == "overdue") {
            //    if (comboxgroupValue == "") {
            //        $.messager.alert('提示', '班组不能为空')
            //    }
            //}
            // data 为服务器返回的数据
            var result = $.parseJSON(data);
            //if (formatter == "Edittime" || formatter == "Reviewtime" || formatter == "Issuetime" || formatter == "Alltime" || formatter == "editAlloverdue" || formatter == "ReviewAlloverdue" || formatter == "IssueAlloverdue") {
            //    if (result.Success == false) {
            //        $.messager.alert('提示', result.Message)
            //    }
            //}
          
            //if (result.rows.length == 0) {
            //    $.messager.alert('提示', '请添加数据再查询!')
            //}
            var new_x = [];//x轴 数据
            var new_y = [];
            if (result.success == true) {
                for (var i = 0; i < result.rows.length; i++) {
                    new_x.push(result.rows[i].StatisticsType); //x轴 数据遍历进空数组
                    new_y.push(result.rows[i].StatisticsCount);//y轴 数据遍历进空数组
                }
                
                //列表下拉框值
                $("#echartsList").combobox({
                    valueField: 'StatisticsCount',
                    textField: 'StatisticsType',
                    data: result.rows
                })
                //获取Y轴最大值
                var ymax = Math.max.apply(null, new_y);
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
                        data: new_y
                    }]
                });
            } else {
                $.messager.alert("提示", result.Message)
            }
        }
            
    });
    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option);
}

/*
*functionName:
*function:加载科室
*author:赵亮
*date:2018-11-14
*/
function department_statistics() {
    $('#comboxdepartment').combobox({
        url: "/StatisticalManagement/select_group",
        valueField: 'GroupId',
        textField: 'GroupName',
        editable: false,
        queryParams: {
            GroupParentId: '8CFF8E9F-F539-41C9-80CE-06A97F481391'
        },
        required: true,

        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        onSelect: function () {
            var departmentValue = $("#comboxdepartment").combobox("getValue");
            $('#comboxgroup').combobox({
                url: "/StatisticalManagement/select_group",
                valueField: 'GroupId',
                textField: 'GroupName',
                queryParams: {
                    GroupParentId: departmentValue,

                },
                
            });
        }

    })
   
}
//合格不合格checked框
function loadcheckdownload() {
    //一次性通过checked框
    $('#qualifiedChecked').unbind('click').bind('click', function () {
        $("#qualifiedChecked").prop("checked", "checked");
        $("#unqualifiedChecked").prop("checked", false);
        $("#condition").val("0");
    });
    //多次checked框
    $('#unqualifiedChecked').unbind('click').bind('click', function () {
        $("#unqualifiedChecked").prop("checked", "checked");
        $("#qualifiedChecked").prop("checked", false);
        $("#condition").val("1");
    });

}

/*
*functionName:
*function:加载图形列表查询下拉框数据
*author:赵亮
*date:2018-11-14
*/
function echartsSearch(StatisticsKey, DepartmentGroupID, GroupName) {
    
    //var StatisticsType = $("#report_arrange_group").combobox("getValue");
    //var GroupName = $("#comboxgroup").combobox("getValue");
    //var Day = $("#allTimeH").numberbox("getValue");
    //var dateStart = $("#dateStart").datebox("getValue");
    //var dateEnd = $("#dateEnd").datebox("getValue");
    var ListType = $("#echartsList").combobox("getText");
    $("#echartsDatagrid").datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        singleSelect: true,
        //autoRowHeight: true,
        border: true,
        fitColumns: true,
        rownumbers: true,
        //fit: true,
        height: '400',
        //width: tabs_width,
        pagination: true,
        pageSize: 10,
        pageList: [5, 10, 15, 20],
        pageNumber: 1,
        type: 'POST',
        dataType: "json",
        queryParams: {
            ListType: ListType,
            StatisticsType: $("#report_arrange_group").combobox('getValue'),
            StatisticsKey: StatisticsKey,
            DepartmentGroupID: DepartmentGroupID,
            GroupName: GroupName,
            ElapsedTimeDay: $("#allTimeH").numberbox('getValue'),
            dateStart: $("#dateStart").datebox('getValue'),
            dateEnd: $("#dateEnd").datebox('getValue'),
            OverdueTimeDay: $("#overdueStatistics").combobox('getValue'),
            ReportOverdueName: $("#overdueStatistics").combobox('getText'),
            ReportTypeName: $("#GetReportTemplate").combobox('getText'),
            SendBackCount: $("#allFrequency").numberbox('getValue'),
            ListTypeKey: $("#condition").val()
        },
        url: "/StatisticalManagement/Report_ArrangeList", //接收一般处理程序返回来的json数据        
        columns: [[
            //{ field: 'id', title: '序号', hidden: false, sortable: false, width: 100 },
            { field: 'report_num', title: '总报告编号', hidden: false, sortable: false, width: 100 },
            { field: 'report_name', title: '报告名称', hidden: false, sortable: false, width: 100 },
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
            { field: 'Inspection_personnel', title: '检验人', hidden: false, sortable: false, width: 100 },
            { field: 'Audit_personnel', title: '审核人员', hidden: false, sortable: false, width: 100 },
            { field: 'Audit_date', title: '审核时间', hidden: false, sortable: false, width: 100 },
            { field: 'issue_personnel', title: '签发人员', hidden: false, sortable: false, width: 100 },
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
            { field: 'error_remarks', title: '错误类型', hidden: false, sortable: false, width: 100 },
              {
                  field: 'return_flag', title: '是否一次通过', formatter: function (value, row, index) {//状态
                      if (value == 0) {
                          return "是";
                      } else if (value == 1) {
                          return "否";
                      } 
                  }
              },
        ]],
        onLoadSuccess: function (data) {
            $("#exportReport").css('display', 'inline-block')
            var selected_report = $("#echartsDatagrid").datagrid("getSelected");
            var line = $('#echartsDatagrid').datagrid("getRowIndex", selected_report);
            //默认选择行
            $('#echartsDatagrid').datagrid('selectRow', line);
            var selectRow = $("#echartsDatagrid").datagrid("getSelected");
            if (!selectRow) {
                $('#echartsDatagrid').datagrid('selectRow', 0);
            }
        },
        sortOrder: 'asc',
        //toolbar: "#echartsBox"

       
    })
   
    //默认选择行
    $('#echartsDatagrid').datagrid('selectRow', 0);

}
//报告导出

function exportButton(StatisticsKey, DepartmentGroupID, GroupName) {
    var selected_report = $("#echartsDatagrid").datagrid("getSelected");
    if (selected_report) {
        $.ajax({
            type: "post",
            async: true,            //异步请求（同步请求将会锁住浏览器，用户其他操作必须等待请求完成才可以执行）
            url: "/StatisticalManagement/Report_ExportExcl",
            data: {
                StatisticsType: $("#report_arrange_group").combobox('getValue'),
                StatisticsKey: StatisticsKey,
                DepartmentGroupID: DepartmentGroupID,
                GroupName: GroupName,
                ElapsedTimeDay: $("#allTimeH").numberbox('getValue'),
                dateStart: $("#dateStart").datebox('getValue'),
                dateEnd: $("#dateEnd").datebox('getValue'),
                OverdueTimeDay: $("#overdueStatistics").combobox('getValue'),
                ReportOverdueName: $("#overdueStatistics").combobox('getText'),
                ReportTypeName: $("#GetReportTemplate").combobox('getText'),
                ListType: $("#echartsList").combobox('getText'),
                SendBackCount: $("#allFrequency").numberbox('getValue'),
                ListTypeKey: $("#condition").val()
            },
            success: function (data) { // 接口调用成功回调函数
                var result = $.parseJSON(data);
                if (result.Success == true) {
                    window.location.href = result.Message;
                    $('#echartsDatagrid').datagrid('reload');
                    $.messager.alert({
                        title: '提示',
                        msg: '报告导出完成!',
                        top: 500,
                    });
                } else {
                    $.messager.alert({
                        title: '提示',
                        msg: result.Message,
                        top: 500,
                    });
                }
            }

        });
    } else {
          $.messager.alert('提示', '请选择需要导出的报告！');
    }
   
}

