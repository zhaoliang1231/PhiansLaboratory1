
var a = [

    {
        Month: 'Jan',
        Maintenance : 32,
        Service: 23,
        Installation: 12,
        Test: 56,
    },

  

]



$(function () {
    var tabs_width = screen.width - 182;
    //iframe可用高度
    var _height = $(".tab-content").height();
    //设置左边树的样式大小
    $('#department_layout').layout('panel', 'west').panel('resize', {
        width: 300,
        height: _height
    });
    //设置右边列表的样式大小
    $('#department_layout').layout('panel', 'east').panel('resize', {
        width: tabs_width - 300
        //height: _height
    });
    //按时间搜索
    $('#search').unbind('click').bind('click', function () {
        var Type_Value = $("#Type_choose").combobox("getValue");
        switch (Type_Value) {
            case '0':
                Chart2_Init();
                Chart1_Init();//测试项目堆叠图
                $("#Chart1").css("display", "block"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                break;
            case '1':
                Chart3_Init();
                $("#Chart1").css("display", "none"); $("#Chart2").css("display", "block"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                break;
            case '2':
                Chart4_Init();
                $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "block"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                break;
            case '3':
                Chart5_Init();
                $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "block"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                break;
            case '4':
                Chart6_Init();
                $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "block"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                break;
            case '5':
                Chart7_Init();
                $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "block"); $("#Chart7").css("display", "none");
                break;
            case '6':
                Chart8_Init();
                $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "block");
                break;
        }
    });
    Chart2_Init();
    Chart1_Init();//测试项目堆叠图初始化首页
    //下拉框
    $("#Type_choose").combobox({
        value: "0",
        editable: false,
        data: [
           { 'value': '0', 'text': 'JE LEB Headcount(00-2040&00-2050&VM)' },
           { 'value': '1', 'text': 'Received Test Items' },
           { 'value': '2', 'text': 'Received Motors' },
           { 'value': '3', 'text': 'MTR Queue Time(Hours)' },
           { 'value': '4', 'text': 'MTR Backlog(By Months)' },
           { 'value': '5', 'text': 'Productivity' },
           { 'value': '6', 'text': 'On-Time-Delivery(OTD)' }
        ],
        onSelect: function () {
            var Type_Value = $("#Type_choose").combobox("getValue");
            switch (Type_Value) {
                case '0':
                    Chart2_Init();
                    Chart1_Init();//测试项目堆叠图
                    $("#Chart1").css("display", "block"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                    break;
                case '1':
                    Chart3_Init();
                    $("#Chart1").css("display", "none"); $("#Chart2").css("display", "block"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                    break;
                case '2':
                    Chart4_Init();
                    $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "block"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                    break;
                case '3':
                    Chart5_Init();
                    $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "block"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                    break;
                case '4':
                    Chart6_Init();
                    $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "block"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "none");
                    break;
                case '5':
                    Chart7_Init();
                    $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "block"); $("#Chart7").css("display", "none");
                    break;
                case '6':
                    Chart8_Init();
                      $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none"); $("#Chart4").css("display", "none"); $("#Chart5").css("display", "none"); $("#Chart6").css("display", "none"); $("#Chart7").css("display", "block");
                    break;
            }
        }
    });
    $('#department_layout').layout('resize');//页面重置，初始化
    department_tree_init();//******************************************************组织架构管理树初始化  


})
//***********************************************************************************图一-1
function Chart1_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container1"));
    option = {
        tooltip: {
            trigger: 'axis',

            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            data: ['Staff', 'IDL']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: []
        },
        yAxis: {
            type: 'value'

        },
        series: [
            {
                name: 'Staff',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: []
            },
            {
                name: 'IDL',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: []
            }
        ]
    };
    myChart.setOption(option);
    Chart1_Load();
}
//****************************************************************************图一-2
function Chart2_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container2"));
    option = {
        tooltip: {
            trigger: 'axis',

            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            data: ['Staff', 'IDL']
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: []
        },
        yAxis: {
            type: 'value'

        },
        series: [
            {
                name: 'Staff',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: []
            },
            {
                name: 'IDL',
                type: 'bar',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'insideRight'
                    }
                },
                data: []
            }
        ]
    };
    myChart.setOption(option);
}
//***********************************************************************************图一1-2加载
function Chart1_Load() {
    var myChart1 = echarts.init(document.getElementById("container1"));//堆叠图
    var myChart2 = echarts.init(document.getElementById("container2"));//堆叠图
    //   var myChart2 = echarts.init(document.getElementById("container2"));//堆叠图
    var node = $('#department_info').tree('getSelected');
    var orgId;
    if (node) {
        orgId = node.id;
    } else {
        orgId = "";
    }
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetHeadCountStatistics",
        data: { "beginTime": StartTime, "endTime": EndTime, "orgId": orgId },
        type: "POST",
        dataType: "json",
        success: function(data) {
            if (data.Success) {
                ////堆叠图
                var Name = [];
                var StaffCount = [];
                var IDLCount = [];
                //    var seriesJson = {};
                for (var i = 0; i < data.Data.length; i++) {
                    Name[i] = data.Data[i].Name;
                    StaffCount[i] = data.Data[i].StaffCount;
                    IDLCount[i] = data.Data[i].IDLCount;
                    //组合堆叠图
                }
                //状态分析 柱状图
                myChart1.setOption({ //加载数据图表
                    xAxis: {
                        data: Name
                    },
                    series: [
                        {
                            data: StaffCount
                        },
                        {
                            data: IDLCount
                        }
                    ]
                });
                myChart2.setOption({ //加载数据图表
                    xAxis: {
                        data: Name
                    },
                    series: [
                        {
                            data: StaffCount
                        },
                        {
                            data: IDLCount
                        }
                    ]
                });


            }
        }
    });
}
//**********************************************************************************图二
function Chart3_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container3"));
    option = {
        title: {
            text: 'No.of Rexeived Test Items'

        },
        tooltip: {
            trigger: 'axis',
            // formatter: '{b0}<br/> {a0}：{c}'
        },

        legend: {
            data: ['Received Test Items']
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
            data: []
        },
        yAxis: {
            type: 'value',
            name: 'Test Item',
            axisLabel: {
                formatter: '{value}'
            }
        },
        series: [
            {
                name: '数量',
                type: 'line',
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: []
            }

        ]
    };

    myChart.setOption(option);
    Chart3_Load();
}
//********************************************************************************图二加载
function Chart3_Load() {
    var myChart = echarts.init(document.getElementById("container3"));//柏拉图
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetReceivedTestItemStatistics",
        data: { "beginTime": StartTime, "endTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////var sum = 0;

                var Name = [];
                var StatisticsCount = [];
                for (var i = 0; i < data.Data.length; i++) {
                    Name[i] = data.Data[i].Name;
                    StatisticsCount[i] = data.Data[i].StatisticsCount;
                }
                //状态分析 柱状图
                myChart.setOption({        //加载数据图表

                    xAxis: {
                        data: Name
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: 'Test Item ',
                        data: StatisticsCount
                    }
                    ]
                });
            }
        }
    })
}
//***********************************************************************图三

function Chart4_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container4"));
    option = {
        title: {
            text: 'No.of Received Motors',
            x: 'center'

        },
        tooltip: {
            trigger: 'axis',
            // formatter: '{b0}<br/> {a0}：{c}'
        },

        legend: {
            data: ['Received Motors']
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
            data: []
        },
        yAxis: {
            type: 'value',
            name: 'Motor QTY',
            axisLabel: {
                formatter: '{value}'
            }
        },
        series: [
            {
                name: '数量',
                type: 'line',
                data: [],
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                }
            }

        ]
    };

    myChart.setOption(option);
    Chart4_Load();
}
//图三加载
function Chart4_Load() {
    var myChart = echarts.init(document.getElementById("container4"));//柏拉图
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetReceivedMotorStatistics",
        data: { "beginTime": StartTime, "endTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////var sum = 0;
                var Name = [];
                var StatisticsCount = [];
                for (var i = 0; i < data.Data.length; i++) {
                    Name[i] = data.Data[i].Name;
                    StatisticsCount[i] = data.Data[i].StatisticsCount;
                }
                //状态分析 柱状图
                myChart.setOption({        //加载数据图表

                    xAxis: {
                        data: Name
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: 'Test Item ',
                        data: StatisticsCount
                    }
                    ]
                });
            }
        }
    })
}
//图四
function Chart5_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container5"));
    $('#BU').combobox({
        url: "/StatisticalManagement/GetBuSelectList",
        valueField: 'Name',
        textField: 'Name',
        required: true,
        //本地联系人数据模糊索引
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        },
        loadFilter: function (data) {
            return (data.Data);
        },
        onSelect: function () {
            var BU = $('#BU').combobox('getValue');
            Chart5_Load(BU);//初始化加载
        }
    });
    option = {
        title: {
            text: 'MTR Queue Time'

        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}'
        },

        legend: {
            data: ['Average QT']
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
            data: []
        },
        yAxis: {
            type: 'value',
            name: 'Hours',
            axisLabel: {
                formatter: '{value}'
            }
        },
        series: [
            {
                name: 'Hours',
                type: 'line',
                data: [],
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                }
            }
        ]
    };

    myChart.setOption(option);
    Chart5_Load("");//初始化加载
}
//图四加载
function Chart5_Load(BU) {
    var myChart = echarts.init(document.getElementById("container5"));//柏拉图
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetMTRQueueTimeStatistics",
        data: { "beginTime": StartTime, "endTime": EndTime, "BU": BU },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////var sum = 0;
                var Name = [];
                var StatisticsCount = [];
                for (var i = 0; i < data.Data.length; i++) {
                    Name[i] = data.Data[i].Name;
                    StatisticsCount[i] = data.Data[i].StatisticsCount;
                }
                //状态分析 柱状图
                myChart.setOption({        //加载数据图表

                    xAxis: {
                        data: Name
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: 'Hours',
                        data: StatisticsCount
                    }
                    ]
                });
            }
        }
    })
}
//图五
function Chart6_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container6"));
    option = {
        title: {
            text: 'MTR Backlog'

        },
        tooltip: {
            trigger: 'axis',
        },

        legend: {
            data: ['MTR Backlog(LAB I+II)']
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
            data: []
        },
        yAxis: {
            type: 'value',
            name: 'MRT Qty',
            axisLabel: {
                formatter: '{value}'
            }
        },
        series: [
            {
                name: 'MTR Backlog(LAB I+II)',
                type: 'line',
                data: [],
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                }

            }
        ]
    };

    myChart.setOption(option);
    Chart6_Load();
}
//图五加载
function Chart6_Load() {

    var myChart = echarts.init(document.getElementById("container6"));//柏拉图
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetMtrBacklogStatistics",
        data: { "beginTime": StartTime, "endTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////var sum = 0;
                var Name = [];
                var StatisticsCount = [];
                for (var i = 0; i < data.Data.length; i++) {
                    Name[i] = data.Data[i].Name;
                    StatisticsCount[i] = data.Data[i].BacklogCount;
                }
                //状态分析 柱状图
                myChart.setOption({        //加载数据图表

                    xAxis: {
                        data: Name
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: 'MTR Backlog(LAB I+II)',
                        data: StatisticsCount
                    }
                    ]
                });
            }
        }
    });

}
//图六
function Chart7_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container7"));
    option = {
        title: {
            text: 'Productivity'

        },
        tooltip: {
            trigger: 'axis',
        },

        legend: {
            data: ['Productivity', 'Base Line', 'Target(30%)']
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
            data: []
        },
        yAxis: {
            type: 'value',
            name: '',
            axisLabel: {
                formatter: '{value}'
            }
        },
        series: [
            {
                name: 'Productivity',
                type: 'line',
                data: [],
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                }
            },
            {
                name: 'Base Line',
                type: 'line',
                data: [],
                itemStyle: {
                    normal: {
                        lineStyle: {
                            type: 'solid'  //'dotted'虚线 'solid'实线
                        }
                    }
                }
            },
            {
                name: 'Target(30%)',
                type: 'line',
                data: [],
                itemStyle: {
                    normal: {
                        lineStyle: {
                            type: 'solid'  //'dotted'虚线 'solid'实线
                        }
                    }
                }
            }
        ]
    };

    myChart.setOption(option);
    Chart7_Load();
}
//图六加载
function Chart7_Load() {
    var myChart = echarts.init(document.getElementById("container7"));
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    
    var StatisticsCount = [];
    $.ajax({
        url: "/StatisticalManagement/GetProductivityStatistic",
        data: { "beginTime": StartTime, "endTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var monthStr = '';
                for (var i = 0; i < data.Data.length; i++) {
                  //  Name[i] = data.Data[i].Name;
                    StatisticsCount[i] = data.Data[i].StatisticsCount;
                    monthStr += '<label>' +data.Data[i].Name +'：</label><input type="number" class="textCount" data-name ="' + data.Data[i].Name + '" data-statistics ="' + data.Data[i].StatisticsCount +'" style="width:100px;height:24px;border-radius:4px;outline:none; border:1px solid #cfcfcf;"/>&nbsp;&nbsp;';

                }
                $("#month").html(monthStr);

            }
        }
    });
    $("#Add").unbind("click").bind("click",
        function() {
            //获取每个时间的范围值
            var ValueRange = [];
            var max = [];
            //获取最大值
            var maxVal = $("#max").textbox("getText");
            var min = [];
            var Name = [];
            //获取最小值
            var minVal = $("#min").textbox("getText");
            if (maxVal == 0 || maxVal == "" || minVal == "" || minVal == 0) {
                $.messager.alert("Tips", "输入框只能数值");
                return;
            }

            $(".textCount").each(function() {
                var val = $(this).val();
                if (val == "" || val == 0) {
                    $.messager.alert("Tips", "输入框只能数值");
                    return;
                }
                Name.push($(this).data('name'));
                ValueRange.push(($(this).data('statistics') / val).toFixed(2));
                min.push(minVal);
                max.push(maxVal);
            });

            //状态分析 柱状图
            myChart.setOption({ //加载数据图表
                xAxis: {
                    data: Name
                },
                series: [
                    {
                        // 根据名字对应到相应的系列
                        name: 'Productivity',
                        data: ValueRange
                    }, {
                        // 根据名字对应到相应的系列
                        name: 'Base Line',
                        data: max
                    }, {
                        // 根据名字对应到相应的系列
                        name: 'Target(30%)',
                        data: min
                    }
                ]
            });
        });
}
//图七
function Chart8_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container8"));
    option = {
        title: {
            text: 'QTD'
        },
        tooltip: {
            trigger: 'axis'
        },

        grid: {
            left: 200,
            right: 100
        },
        legend: {
            data: ['TotalMTR (LAB I+II)', 'Completed MTR within Schedule', 'QTD%']
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
                name: 'No. of MTR',
                axisLabel: {
                    formatter: '{value}'
                }

            },
             {
                 type: 'value',
                 name: 'OTD%',
                 axisLabel: {
                     formatter: '{value}'
                 }

             }
        ],
        series: [
            {
                name: 'TotalMTR (LAB I+II)',
                type: 'bar',
                barWidth: '10%',
                stack: '数量',
                data: []
            },
            {
                name: 'Completed MTR within Schedule',
                type: 'bar',
                barWidth: '10%',
                stack: '数量',
                data: []
            },
            {
                name: 'QTD%',
                type: 'line',
                yAxisIndex: 1,
                data: [],
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                }
            }
        ]
    };
    myChart.setOption(option);
    Chart8_Load();
}
//图七加载
function Chart8_Load() {

    var myChart = echarts.init(document.getElementById("container8"));//柏拉图
    var StartTime = $('#StartTime').textbox("getText");
    var EndTime = $('#EndTime').textbox("getText");
    $.ajax({
        url: "/StatisticalManagement/GetMtrBacklogStatistics",
        data: { "beginTime": StartTime, "endTime": EndTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////var sum = 0;
                var Name = [];
                var anTimeStatisticsCount = [];
                var totalStatisticsCount = [];
                var rateStatisticsCount = [];
                for (var i = 0; i < data.Data.length; i++) {
                    Name[i] = data.Data[i].Name;
                    anTimeStatisticsCount[i] = data.Data[i].AntimeCount;
                    totalStatisticsCount[i] = data.Data[i].BacklogCount + data.Data[i].AntimeCount;
                    rateStatisticsCount[i] = (data.Data[i].AntimeCount / (data.Data[i].AntimeCount + data.Data[i].BacklogCount) * 100).toFixed(2)
                }
                //状态分析 柱状图
                myChart.setOption({        //加载数据图表

                    xAxis: {
                        data: Name
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: 'TotalMTR (LAB I+II)',
                        data: totalStatisticsCount
                    },
                      {
                          name: 'Completed MTR within Schedule',
                          data: anTimeStatisticsCount
                      },
                        {
                            name: 'QTD%',
                            data: rateStatisticsCount
                        }
                                ]
                });
            }
        }
    });

}
//********************************************************************组织架构管理树初始化********************************************************************
//********************************************************************组织架构管理树
function department_tree_init() {
    $('#department_info').tree({
        url: '/SystemSettings/GetDepartmentList',
        method: 'post',
        required: true,
        top: 0,
        fit: true,
        onContextMenu: function (e, node) {//右击菜单栏操作

        },
        onBeforeExpand: function (node, param) {//树节点展开
            $('#department_info').tree('options').url = "/SystemSettings/GetDepartmentList?ParentId=" + node.id;
        },
        onSelect: function () {
            Chart1_Load();
        }
    });

};