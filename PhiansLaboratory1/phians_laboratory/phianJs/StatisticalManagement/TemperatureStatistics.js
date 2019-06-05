$(function () {
    ReportLine_Init();//MTR报告折线图
});

//报告折线图
function ReportLine_Init() {
    //房间下拉框内容
    $('#RoomNum').combobox({
        url: "/MonitoringManagement/GetRoomNum",
        valueField: 'Value',
        textField: 'Text',
        onLoadSuccess: function (data) {
            $('#RoomNum').combobox('setValue', data[0].Value);
            GetReportStatisticsLoad();
           
        },
        onSelect:function() {
            GetReportStatisticsLoad();
        },
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) >= 0;
        }
    });
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container"));
    option = {
        title: {
            text: '温度/湿度折线图'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}℃，{a1}：{c1}%'
        },
        
        legend: {
            data: ['温度', '湿度']
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
            //boundaryGap: false,
            data: []
        },
        yAxis:[ {
            type: 'value',
            name: '温度',
            axisLabel: {
                formatter: '{value}°C'
            },
            min:15
        },{
            type: 'value',
            name: '湿度',
            axisLabel: {
                formatter: '{value}%'
            },
            min:30
        }
        ],
        series: [
            {
                name: '温度',
                type: 'line',
                data: [],
                itemStyle : {
                    normal : {
                        lineStyle:{
                            width:2.5
                        }
                    }
                }
            },
            {
                name: '湿度',
                type: 'line',
                yAxisIndex: 1,
                data: [],
                itemStyle: {
                    normal: {
                        lineStyle: {
                            width: 2.5
                        }
                    }
                }
            }

        ]
    };
    myChart.setOption(option);
    //搜索折线图
    $("#search").unbind("click").bind('click', function () {
        GetReportStatisticsLoad();
    });
}
//加载折线图
function GetReportStatisticsLoad() {
    var myChartBar = echarts.init(document.getElementById("container"));//柏拉图
    var beginTime = $("#StartTime").datebox("getValue");//开始时间
    var endTime = $("#EndTime").datebox("getValue");//结束时间
    var roomId = $("#RoomNum").combobox("getValue");//房间号
    $.ajax({
        url: "/StatisticalManagement/GetHumidityTemperatureStatistics",
        data: { "beginTime": beginTime, "roomId": roomId, "endTime": endTime },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var LowerLimit = [];//温度下限
                var Temperature = [];//温度
                var TopLimit = [];//温度上限
                var humidity = [];//湿度
                var humidityLowerLimit = [];//湿度下限
                var humidityTopLimit = [];//湿度上限
                var RecordTime = [];
                for (var i = 0; i < data.Data.length; i++) {
                    //console.log(i);
                    LowerLimit[i] = data.Data[i].LowerLimit;
                    Temperature[i] = data.Data[i].Temperature;
                    TopLimit[i] = data.Data[i].TopLimit;
                    humidity[i] = data.Data[i].humidity;
                    humidityLowerLimit[i] = data.Data[i].humidityLowerLimit;
                    humidityTopLimit[i] = data.Data[i].humidityTopLimit;
                    
                    var stime = data.Data[i].RecordTime;   　　//stime='/Date(1460649600000)/'
                    var start = eval('new ' + stime.substr(1, stime.length - 2));    //start='Fri Apr 15 2016 00:00:00 GMT+0800 (中国标准时间)'
                    var covertTime = start.getFullYear() + '/' + (start.getMonth() + 1) + '/' + start.getDate() + ' ' + start.getHours() + ':' + start.getMinutes();
                    RecordTime[i] = covertTime;
                }
                
                //状态分析 柱状图
                myChartBar.setOption({        //加载数据图表

                    xAxis: {
                        data: RecordTime
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: '温度',
                        data: Temperature
                    },
                    {
                        // 根据名字对应到相应的系列
                        name: '湿度',
                        data: humidity
                    }
                    ]
                });
            }
        }
    })
};