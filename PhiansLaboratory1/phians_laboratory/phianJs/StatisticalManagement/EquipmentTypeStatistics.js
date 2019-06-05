$(function () {
    //下拉框
    $("#TypeChoose").combobox({
        value: "0.2m³",
        editable: false,
        data: [
            { 'value': '0.2m³', 'text': 'Utilization 0.2m³' },
            { 'value': '0.4m³', 'text': 'Utilization 0.4m³' },
            { 'value': '1.0m³', 'text': 'Utilization 1.0m³' },
            { 'value': 'Oven', 'text': 'Utilization Oven' },
            { 'value': 'Climate Vibration', 'text': 'Utilization Climate Vibration' },
            { 'value': 'Vibration RT', 'text': 'Utilization Vibration RT' },
            { 'value': 'Shock', 'text': 'Utilization Shock' },
            { 'value': 'Salt', 'text': 'Utilization Salt' },
            { 'value': 'Walk in', 'text': 'Utilization Walk in' }
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
                case 'Oven':
                    Chart1_Init();
                    break;
                case 'Climate Vibration':
                    Chart1_Init();
                    break;
                case 'Vibration RT':
                    Chart1_Init();
                    break;
                case 'Shock':
                    Chart1_Init();
                    break;
                case 'Salt':
                    Chart1_Init();
                    break;
                case 'Walk in':
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
    option = {
        title: {
            text: 'Utilization 0.2m³'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}%<br/>{a1}：{c1}%<br/>{a2}：{c2}%<br/>{a3}：{c3}%'
        },

        grid: {
            left: 200,
            right: 100
        },
        legend: {
            data: ['安装时间', '无测试时间', '维修时间', '测试时间']
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
                name: '安装时间',
                type: 'bar',
                barWidth: '40%',
                stack: '数量',
                data: []
            },
            {
                name: '无测试时间',
                type: 'bar',
                stack: '数量',
                data: []
            },
            {
                name: '维修时间',
                type: 'bar',
                stack: '数量',
                data: []
            },
            {
                name: '测试时间',
                type: 'bar',
                stack: '数量',
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
        url: "/StatisticalManagement/GetEquipmentStatisticsByEquipmentType",
        data: { "Year": planYear, "EquipmentType": typeValue },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                ////var sum = 0;
                var Month = [];
                var nonTestUseRatio = [];
                var testUseRatio = [];
                var installationUseRatio = [];
                var fixUseRatio = [];
                for (var i = 0; i < data.Data.length; i++) {
                    Month[i] = data.Data[i].Month;
                    nonTestUseRatio[i] = data.Data[i].NonTestUseRatio;
                    testUseRatio[i] = data.Data[i].TestUseRatio;
                    installationUseRatio[i] = data.Data[i].InstallationUseRatio;
                    fixUseRatio[i] = data.Data[i].FixUseRatio;
                }
                myChart.setOption({        //加载数据图表
                    title: { text: typeValue1 },
                    xAxis: {
                        data: Month
                    },
                    series: [{
                        // 根据名字对应到相应的系列
                        name: '安装时间',
                        data: installationUseRatio
                    },
                    {
                        name: '无测试时间',
                        data: nonTestUseRatio
                    },
                    {
                        name: '维修时间',
                        data: fixUseRatio
                    },
                    {
                        name: '测试时间',
                        data: testUseRatio
                    }]
                });
            }
        }
    });

}
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
        case 'Oven':
            Chart1_Init();
            break;
        case 'Climate Vibration':
            Chart1_Init();
            break;
        case 'Vibration RT':
            Chart1_Init();
            break;
        case 'Shock':
            Chart1_Init();
            break;
        case 'Salt':
            Chart1_Init();
            break;
        case 'Walk in':
            Chart1_Init();
            break;
    }
}