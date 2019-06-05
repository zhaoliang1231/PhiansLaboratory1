$(function () {
    //下拉框
    $("#TypeChoose").combobox({
        value: "0.2m³",
        editable: false,
        data: [
            { 'value': '0.2m³', 'text': 'Chamber 0.2m³' },
            { 'value': '0.4m³', 'text': 'Chamber 0.4m³' },
            { 'value': '1.0m³', 'text': 'Chamber 1.0m³' },
            { 'value': 'High Temp Chamber', 'text': 'High Temp Chamber' },
            { 'value': 'Climate Shaker', 'text': 'Climate Shaker' },
            { 'value': 'RT Shaker', 'text': 'RT Shaker' },
            { 'value': 'Thermal Shock', 'text': 'Thermal Shock' },
            { 'value': 'Salt Spray', 'text': 'Salt Spray' },
            { 'value': 'Walk Chamber', 'text': 'Walk Chamber' }
        ],
        onSelect: function () {
            var typeValue = $("#TypeChoose").combobox("getValue");
            switch (typeValue) {
                case '0.2m³':
                    Chart1_Init(); //测试项目堆叠图
                    break;
                case '0.4m³':;
                    Chart1_Init();
                    break;
                case '1.0m³':
                    Chart1_Init();
                    break;
                case 'High Temp Chamber':
                    Chart1_Init();
                    break;
                case 'Climate Shaker':
                    Chart1_Init();
                    break;
                case 'RT Shaker':
                    Chart1_Init();
                    break;
                case 'Thermal Shock':
                    Chart1_Init();
                    break;
                case 'Salt Spray':
                    Chart1_Init();
                    break;
                case 'Walk Chamber':
                    Chart1_Init();
                    break;
            }
        }
    });
    Chart1_Init(); //测试项目堆叠图
    //搜索图表
    $("#search").unbind("click").bind('click', function () {
        SearchChart();
    });
});
function yearFormatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y;
};
function yearParser(s) {
    if (!s) return new Date();
    var y = s;
    var date;
    if (!isNaN(y)) {
        return new Date(y, 0, 1);
    } else {
        return new Date();
    }
};
//图表1-9基础渲染
function Chart1_Init() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container1"));
    //ar typeChoose = $("#TypeChoose").combobox("getText");
    option = {
        title: {
            text: 'Chamber 0.2m³'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}%',
            magicType: {show: true, type: ['line', 'bar']}
        },

        grid: {
            left: 200,
            right: 100
        },
        legend: {
            data: ['Chamber 0.2m³']
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
                name: 'Utilization(%)',
                axisLabel: {
                    formatter: '{value}%'
                }

            }
        ],
        series: [
            {
                name: 'Chamber 0.2m³',
                type: 'bar',
                barWidth: '40%',
                //stack: '数量',
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: []
            },
            {
                name: 'Chamber 0.2m³',
                type: 'line',
                //barWidth: '40%',
                //stack: '数量',
                
                data: []
            }
        ]
    };
    myChart.setOption(option);
    Chart1_Load();
};

//根据设备类型加载图
function Chart1_Load() {
    var myChart = echarts.init(document.getElementById("container1"));//柏拉图
    var typeValue = $("#TypeChoose").combobox("getValue");
    var typeValue1 = $("#TypeChoose").combobox("getText");
    var planYear = $('#planYear').datebox("getValue");
    $.ajax({
        url: "/StatisticalManagement/GetEquipmentTypeTestStatistics",
        data: { "Year": planYear, "EquipmentType": typeValue },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var Month = [];
                var testUseRatio = [];
                for (var i = 0; i < data.Data.length; i++) {
                    Month[i] = data.Data[i].Month;
                    testUseRatio[i] = data.Data[i].TestUseRatio;
                }
                myChart.setOption({        //加载数据图表
                    title: { text: typeValue1 },
                    legend: {
                        data: [typeValue1]
                    },
                    xAxis: {
                        data: Month
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: typeValue1,
                        data: testUseRatio
                    }, {
                        // 根据名字对应到相应的系列
                        name: '1',
                        data: testUseRatio
                    }]
                });
            }
        }
    });
};
//搜索
function SearchChart() {
    var typeValue = $("#TypeChoose").combobox("getValue");
    switch (typeValue) {
        case '0.2m³':
            Chart1_Init(); //测试项目堆叠图
            break;
        case '0.4m³':;
            Chart1_Init();
            break;
        case '1.0m³':
            Chart1_Init();
            break;
        case 'High Temp Chamber':
            Chart1_Init();
            break;
        case 'Climate Shaker':
            Chart1_Init();
            break;
        case 'RT Shaker':
            Chart1_Init();
            break;
        case 'Thermal Shock':
            Chart1_Init();
            break;
        case 'Salt Spray':
            Chart1_Init();
            break;
        case 'Walk Chamber':
            Chart1_Init();
            break;
    }
}