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

    ReportLine_Init();//MTR报告折线图
    AbnormalReportChart_Init();//异常报告柱状图
    AbnormalReportPie_Init();//异常报告饼状图
    AbnormalReport_Datagrid_Init(other_height);//测你们，试项目类型列表初始化
});


//报告折线图
function ReportLine_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container"));
    option = {
        title: {
            text: '报告通过折线图'
           
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}%，最终通过率：{c1}%'
        },
        
        legend: {
            data: ['一次通过率', '最终通过率']
        },
        toolbox: {
            show: true,
            feature: {
                dataZoom: {
                    yAxisIndex: 'none'
                },
                dataView: { readOnly: false },
                magicType: { type: ['line', 'bar'] },
                restore: {},
                saveAsImage: {}
            }
        },
        xAxis: {
            type: 'category',
            boundaryGap: false,
            data: []
        },
        yAxis: {
            type: 'value',
            name: '比率%',
            axisLabel: {
                formatter: '{value}'
            }
        },
        series: [
            {
                name: '一次通过率',
                type: 'line',
                data: []
                
            },
            {
                name: '最终通过率',
                type: 'line',
                data: []
            }
        ]
    };

    myChart.setOption(option);
    GetReportStatisticsLoad();
}
//加载折线图
function GetReportStatisticsLoad() {
    var myChartBar = echarts.init(document.getElementById("container"));//柏拉图
    $.ajax({
        url: "/StatisticalManagement/GetReportStatistics",
        //  data: { "StartTime": StartTime, "type": type, "EndTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////柏拉图
                //var sum = 0;
                var rateSum = 0;
                var OncePassCountrate = [];//一次报告通过率
                var NoErrorCountrate = [];//最终报告通过率
                var ReportName = [];
                var OncePassCount = [];
                var TotalCount = [];
                var NoErrorCount = [];
                var toopTipStr = "";
                for (var i = 0; i < data.Data.length; i++) {
                    ReportName[i] = data.Data[i].Name;
                    OncePassCount[i] = data.Data[i].OncePassCount;
                    NoErrorCountrate[i] = data.Data[i].NoErrorCountrate;
                    OncePassCountrate[i] = (data.Data[i].OncePassCount / data.Data[i].TotalCount * 100).toFixed(2);
                    NoErrorCountrate[i] = (data.Data[i].NoErrorCount / data.Data[i].TotalCount * 100).toFixed(2);
                }
                //状态分析 柱状图
                myChartBar.setOption({        //加载数据图表
                   
                    xAxis: {
                        data: ReportName
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: '一次通过率',
                        data: OncePassCountrate
                    },
                    {
                        // 根据名字对应到相应的系列
                        name: '最终通过率',
                        data: NoErrorCountrate
                    },
                    ]
                });
            }
        }
    })
}
//测试项目类型对跌停
function AbnormalReportChart_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("Test_Container"));
    option = {
        title: {
            text: '异常报告分析图'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}，{a1}：{c1}%'
        },

        grid: {
            left: 100,
            right: 250
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
        AbnormalReport_Datagrid_Load(name)
    });
    GetErrorReportStatisticsLoad();
}
////加载异常报告饼状图
function AbnormalReportPie_Init() {
    // 使用刚指定的配置项和数据显示图表。
    // 基于准备好的dom，初始化echarts实例
    var myChartPie = echarts.init(document.getElementById("Test_ContainerPie"));
    //饼状图
    option1 = {
        title: {
            text: '异常报告占比图',
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
                name: '数量',
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
        var name = params.name;
        AbnormalReport_Datagrid_Load(name)
    });
}
//加载异常报告
function GetErrorReportStatisticsLoad() {
    var myChartBar = echarts.init(document.getElementById("Test_Container"));//柏拉图
    var myChartPie = echarts.init(document.getElementById("Test_ContainerPie"));//占比图
    $.ajax({
        url: "/StatisticalManagement/GetErrorReportStatistics",
        // data: { "StartTime": StartTime, "type": type, "EndTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////柏拉图
                var sum = 0;
                var rateSum = 0;
                var rate = [];
                var ErrorReportName = [];
                var StatisticsCount = [];
                var sum = 0;//获取总的个数
                for (var i = 0; i < data.Data.length; i++) {
                    ErrorReportName[i] = data.Data[i].Name;
                    // temp = temp + data.Data.StatisticsList[i].StatisticsCount;
                    //  rate[i] = data.Data[i].StatisticsCount / sum;
                    sum = sum + data.Data[i].StatisticsCount;
                }
                var toopTipStr = "";
                for (var i = 0; i < data.Data.length; i++) {
                    ErrorReportName[i] = data.Data[i].Name;
                    // temp = temp + data.Data.StatisticsList[i].StatisticsCount;
                    rate[i] = (data.Data[i].StatisticsCount / sum * 100).toFixed(2);
                    StatisticsCount[i] = data.Data[i].StatisticsCount;
                  //  toopTipStr += '{b' + 2 * i + '} {a' + 2 * i + '} 合格数：{c' + 2 * i + '}，合格率：{c' + (2 * i + 1) + '}%<br/>'
                }
                //状态分析 柱状图
                myChartBar.setOption({        //加载数据图表
                   
                    xAxis: {
                        data: ErrorReportName
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
                var ErrorReportArr = [];
                for (var i = 0; i < data.Data.length; i++) {
                    var ErrorReportArrJson = {};
                    ErrorReportArrJson.name = data.Data[i].Name;
                    ErrorReportArrJson.value = data.Data[i].StatisticsCount;
                    ErrorReportArr.push(ErrorReportArrJson);
                }
                //  console.log(MTRArr);
                //圆饼图
                myChartPie.setOption({        //加载数据图表
                    legend: {
                        data: ErrorReportName
                    },
                    series: [{
                        data: ErrorReportArr
                    }]
                });

            }
        }
    })
}
////加载异常报告列表初始化
function AbnormalReport_Datagrid_Init(other_height) {
    $('#TestList').datagrid({
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
            $('#TestList').datagrid('selectRow', 0);//默认选中第一行
        },
        // toolbar: "#borrow_records_toolbar"
    });
    var json = {
        "rows": [],
        "total": 0,
        "success": true
    };
    $('#TestList').datagrid("loadData", json);
};
////加载异常报告显示列表
function AbnormalReport_Datagrid_Load(name) {
    //switch (searchName) {
    //    case "1": searchName = "MTRstate"; break;
    //    case "2": searchName = "Conculsion"; break;
    //    default:;
    //}
    $('#TestList').datagrid({
        queryParams: {
            searchName: "applytype",
            searchValue: name
        },
        url: "/StatisticalManagement/GetErrorMtrInfoList",//接收一般处理程序返回来的json数据     
    })
}