
$(function () {
    var tabs_width = screen.width;
    //浏览器宽度-body高度
    var other_height = screen.availHeight - (window.outerHeight - window.top.document.body.clientHeight) - 100;
    $("#cc").css("height", other_height);
    $("#cc1").css("height", other_height);
    // $("#List").css("height", other_height - 400);
    //////////////////$("#TestList").css("height", other_height - 400);
    $("#Test_Container").css("width", tabs_width / 2);
    $("#Test_ContainerPie").css("width", tabs_width / 2);

    //类型下拉
    $('#Echar_Type').combobox({
        value: "1",
        data: [
            { 'value': '1', 'text': '状态分析' },//认可实验室内部
            { 'value': '2', 'text': '结果分析' },//认可实验室外部
        ]
    });
    MTRBarInit();//MTR柱状图
    MTRPieInit();//MTR饼状图
    List_Datagrid_Init(other_height);////MTR列表初始化


    TestChart_Init();//测试项目堆叠图
    TestPie_Init();//测试项目饼状图
    Test_List_Datagrid_Init(other_height);//测excvbns吧你们，试项目类型列表初始化
    //按时间搜索
    $('#search').unbind('click').bind('click', function () {
        SearchBarAndRate();
    });
    //按时间搜索
    $('#search1').unbind('click').bind('click', function () {
        SearchBarAndRate1();
    });
});
//按时间搜索
function SearchBarAndRate() {
    MtrInfoTotalStatisticsLoad();
}
//TESt按时间搜索
function SearchBarAndRate1() {
    GetTaskItemStatisticsLoad();
}
//MTR柱状图
function MTRBarInit() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container"));
    option = {
        title: {
            text: '状态分析图'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}，{a1}：{c1}%'

        },

        grid: {
            left: 200,
            right: 100
        },
        legend: {
            data: ['数量', '总占比']
        },
        xAxis: [
            {
                type: 'category',
                data: [],
                axisPointer: {
                    type: 'shadow'
                }
            }
        ],
        yAxis: [
            {
                type: 'value',
                name: '数量',
                axisLabel: {
                    formatter: '{value}'
                }

            },
            {
                type: 'value',
                name: '总占比%',
                axisLabel: {
                    formatter: '{value}'
                }
            }
        ],
        series: [
            {
                name: '数量',
                type: 'bar',
                barWidth: '40%',
                data: []
            },
            {
                name: '总占比',
                type: 'line',
                yAxisIndex: 1,
                data: []
            }
        ]
    };
    myChart.setOption(option);
    myChart.on('click', function (params) {
        var name = params.name;
        List_Datagrid_Load(name);
    });
    MtrInfoTotalStatisticsLoad();
}
////MTR饼状图
function MTRPieInit() {
    // 使用刚指定的配置项和数据显示图表。
    // 基于准备好的dom，初始化echarts实例
    var myChartPie = echarts.init(document.getElementById("container_pie"));

    //饼状图
    option = {
        title: {
            text: '占比图',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            left: 'left',
            data: ['直接访问', '邮件营销', '联盟广告', '视频广告', '搜索引擎']
        },
        series: [
            {
                name: '数量',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: [
                    { value: 335, name: '直接访问' },
                    { value: 310, name: '邮件营销' },
                    { value: 234, name: '联盟广告' },
                    { value: 135, name: '视频广告' },
                    { value: 1548, name: '搜索引擎' }
                ],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };
    myChartPie.setOption(option);
    myChartPie.on('click', function (params) {
        var name = params.name;
        List_Datagrid_Load(name);
    });
}
//加载柏拉图
function MtrInfoTotalStatisticsLoad() {
    var myChartBar = echarts.init(document.getElementById("container"));//柏拉图
    var myChartPie = echarts.init(document.getElementById("container_pie"));//占比图
    var type = $('#Echar_Type').combobox("getValue");
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetMtrInfoTotalStatistics",
        data: { "StartTime": StartTime, "type": type, "EndTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////柏拉图
                //var sum = 0;
                var rateSum = 0;
                var rate = [];
                var MTRName = [];
                var MTRCount = [];
                var StatisticsCount = [];

                sum = data.Data.TotalCount;//获取总数量
                var temp = 0;//累加个数
                for (var i = 0; i < data.Data.StatisticsList.length; i++) {
                    MTRName[i] = data.Data.StatisticsList[i].Name == null ? "无" : data.Data.StatisticsList[i].Name;
                    temp = temp + data.Data.StatisticsList[i].StatisticsCount;
                    rate[i] = (temp / sum * 100).toFixed(2);
                    StatisticsCount[i] = data.Data.StatisticsList[i].StatisticsCount;
                }
                //状态分析 柱状图
                myChartBar.setOption({        //加载数据图表
                    xAxis: {
                        data: MTRName
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: '数量',
                        data: StatisticsCount
                    },
                    {
                        // 根据名字对应到相应的系列
                        name: '总占比',
                        data: rate
                    },
                    ]
                });

                //状态分析 饼状图
                var MTRArr = [];
                for (var i = 0; i < data.Data.StatisticsList.length; i++) {
                    var MTRJson = {};
                    MTRJson.name = data.Data.StatisticsList[i].Name == null ? "无" : data.Data.StatisticsList[i].Name;
                    MTRJson.value = data.Data.StatisticsList[i].StatisticsCount;
                    MTRArr.push(MTRJson);
                }
                //  console.log(MTRArr);
                //圆饼图
                myChartPie.setOption({        //加载数据图表
                    legend: {
                        data: MTRName
                    },
                    series: [{
                        data: MTRArr
                    }]
                });

            }
        }
    })
}
////MTR列表初始化
function List_Datagrid_Init(other_height) {
    $('#DataList').datagrid({
        border: false,
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        //  fit: true,
        pagination: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        height: other_height - 440,
        fitColumns: true,
        type: 'POST',
        dataType: "json",
        //   url: "/StatisticalManagement/GetMtrInfoList",//接收一般处理程序返回来的json数据     
        columns: [[
            { field: 'MTRNO', title: 'MTR No.', sortable: true },//MTR单号
            { field: 'ProjectNo', title: 'Project No.' },//测试项目编号
            { field: 'Application', title: 'Application' },//测试样品的类型
            { field: 'CostCenter', title: 'Cost Center' },//项目所属成本中心
            { field: 'ProjectEng', title: 'Project Eng.' },//项目所属工程师
            { field: 'BU', title: 'Customer' },//客户
            { field: 'Purpose', title: 'Purpose' },//测试目的
            { field: 'FollowUp_n', title: 'Follow Up' },//跟进人
            { field: 'SampleNo', title: 'Sample No.' },//样品编号
            { field: 'SampleName', title: 'Sample Name' },//样品名称
            { field: 'SampleQty', title: 'Sample Qty' },//测试样品数量
            // { field: 'SamplePosition', title: 'Sample Position' },//样本位置
            //{
            //    field: 'Identification', title: 'Identification', formatter: function (value, row, index) {//标识
            //        if (value == true) {
            //            return "Normal";
            //        } else if (value == false) {
            //            return "Abnormal";
            //        }
            //    }
            //},
            {
                field: 'ReceivingDate', title: 'Receiving Date', width: 150, formatter: function (value, row, index) {//接收日期
                    if (value) {//格式化时间
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            {
                field: 'PlanStartTiming', title: 'Plan Start Date', width: 150, formatter: function (value, row, index) {//计划开始时间
                    if (value) {//格式化时间
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },
            {
                field: 'EstimatedCompletionTiming', title: 'Expected Complete Date', formatter: function (value, row, index) {//期望完成时间
                    if (value) {//格式化时间
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },

            {
                field: 'MTRState', title: 'Status', formatter: function (value, row, index) {//MTR状态,
                    switch (value) {
                        case 1: value = 'MTR评审'; break;
                        case 2: value = '样品接收'; break;
                        case 3: value = '测试记录'; break;
                        case 4: value = '报告编制'; break;
                        case 5: value = '报告审核'; break;
                        case 6: value = '报告审核退回'; break;
                        case 7: value = '报告签发'; break;
                        case 8: value = '报告签发退回'; break;
                        case 9: value = '报告完成'; break;
                        default: break;
                    }
                    return value;
                }
            },
            {
                field: 'OrdersReceived', title: 'Orders Received', formatter: function (value, row, index) {//订单接收时间
                    if (value) {//格式化时间
                        if (value.length >= 10) {
                            value = value.substr(0, 10)
                            return value;
                        }
                    }
                }
            },//
            {
                field: 'ReportModified', title: 'Report Modified(Y/N)', formatter: function (value, row, index) {//测试报告是否更改
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            {
                field: 'CNASLogo', title: 'CNAS Logo', width: 100, formatter: function (value, row, index) {//
                    if (value == true) {
                        return "Yes";
                    } else if (value == false) {
                        return "No";
                    }
                }
            },
            {
                field: 'remark', title: 'Remark', width: 150, formatter: function (value, row, index) {//说明
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
            $('#DataList').datagrid('selectRow', 0);//默认选中第一行
        }
        // toolbar: "#borrow_records_toolbar"
    });
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#DataList').datagrid("loadData", json);
};
////MTR列表显示列表
function List_Datagrid_Load(name) {
    var searchName = $("#Echar_Type").combobox("getValue");
    switch (searchName) {
        case "1": searchName = "MTRstate"; break;
        case "2": searchName = "Conculsion"; break;
        default: ;
    }
    $('#DataList').datagrid({
        queryParams: {
            searchName: searchName,
            searchValue: name
        },
        url: "/StatisticalManagement/GetMtrInfoList", //接收一般处理程序返回来的json数据     
    });
}
//测试项目类型对跌停
function TestChart_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("Test_Container"));
    option = {
        title: {
            text: '测试项目堆叠图',
            x: 'left'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            data: ['Fail', 'N/A', 'Pass']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'value'
        },
        yAxis: {
            type: 'category',
            data: ['周一', '周二', '周三']
        },
        series: [
            {
                name: 'Fail',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: [320, 302, 301, 334, 390, 330, 320]
            },
            {
                name: 'N/A',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: [120, 132, 101, 134, 90, 230, 210]
            },
            {
                name: 'Pass',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: [120, 132, 101, 134, 90, 230, 210]
            }
        ]
    };
    myChart.setOption(option);

    GetTaskItemStatisticsLoad();
}
////测试项目类型饼状图
function TestPie_Init() {
    // 使用刚指定的配置项和数据显示图表。
    // 基于准备好的dom，初始化echarts实例
    var myChartPie = echarts.init(document.getElementById("Test_ContainerPie"));

    //饼状图
    option1 = {
        title: {
            text: '测试项目占比图',
            x: 'center'
        },
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        legend: {
            orient: 'vertical',
            left: 'left',
            data: ['Fail', 'Pass', 'N/A']
        },
        series: [
            {
                name: '访问来源',
                type: 'pie',
                radius: '55%',
                center: ['50%', '60%'],
                data: [
                    { value: 335, name: 'Fail' },
                    { value: 310, name: 'Pass' },
                    { value: 234, name: 'N/A' }
                ],
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                    }
                }
            }
        ]
    };
    myChartPie.setOption(option1);
    myChartPie.on('click', function (params) {
        var name = params.Name;
        Test_List_Datagrid_Init(name);
    });
}
////测试项目类型
function GetTaskItemStatisticsLoad() {
    var myChartBar = echarts.init(document.getElementById("Test_Container"));//柏拉图
    var myChartPie = echarts.init(document.getElementById("Test_ContainerPie"));//占比图
    var StartTime = $('#StartTime1').textbox("getText");
    var EndTime = $('#EndTime1').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetTaskItemStatistics",
        data: { "beginTime": StartTime, "endTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////堆叠图
                var ItemName = [];
                var FailCount = [];
                var NACount = [];
                var PassCount = [];
                //    var seriesJson = {};
                for (var i = 0; i < data.Data.length; i++) {
                    ItemName[i] = data.Data[i].Name;
                    FailCount[i] = data.Data[i].FailCount;
                    NACount[i] = data.Data[i].NACount;
                    PassCount[i] = data.Data[i].PassCount;
                    //组合堆叠图
                }
                //console.log(ItemName);
                //状态分析 柱状图
                myChartBar.setOption({        //加载数据图表
                    yAxis: {
                        data: ItemName
                    },
                    series: [
                        {
                            data: FailCount
                        },
                        {
                            data: NACount
                        },
                        {
                            data: PassCount
                        }
                    ]
                });

                ////状态分析 饼状图
                var TestArr = [];
                var FailCountNum = 0;
                var NACountNum = 0;
                var PassCountNum = 0;
                for (var i = 0; i < data.Data.length; i++) {
                    FailCountNum = FailCountNum + data.Data[i].FailCount;
                    NACountNum = NACountNum + data.Data[i].NACount;
                    PassCountNum = PassCountNum + data.Data[i].PassCount;
                }
                $('#TestList').datagrid({
                    data: data.Data
                });
                //圆饼图
                myChartPie.setOption({        //加载数据图表
                    series: [
                        {
                            data: [
                                { value: FailCountNum, name: 'Fail' },
                                { value: NACountNum, name: 'N/A' },
                                { value: PassCountNum, name: 'Pass' }
                            ]
                        }

                    ]
                });

            }
        }
    })
}
////测试项目类型列表初始化
function Test_List_Datagrid_Init(other_height) {
    $('#TestList').datagrid({
        border: false,
        nowrap: false,
        striped: true,
        rownumbers: true,
        ctrlSelect: true,
        //  fit: true,
        pagination: false,
        title: '',
        //   width:800,
        pageNumber: 1,
        height: other_height - 440,
        fitColumns: true,
        type: 'POST',
        dataType: "json",
        // url: "/StatisticalManagement/GetMtrInfoTotalStatistics",
        columns: [[
            { field: 'Name', title: '类型', width: '50' },//设备编号
            { field: 'FailCount', title: 'Fail', width: '50' },//设备名称
            { field: 'NACount', title: 'N/A', width: '50' },//设备类型
            { field: 'PassCount', title: 'PassCount', width: '50' },//设备归属
        ]],
        onLoadSuccess: function (data) {
            $('#TestList').datagrid('selectRow', 0);//默认选中第一行
        },
        // toolbar: "#borrow_records_toolbar"
    });
}
