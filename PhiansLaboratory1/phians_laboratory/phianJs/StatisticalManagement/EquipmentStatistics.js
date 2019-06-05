var clickNum = 0;//search点击次数
$(function () {
    var tabs_width = screen.width - 182;
    //GetEquipmentStatistics1();//MTR报告折线图
    $('#TypeChoose').combobox({
        value: "0",
        editable: false,
        data: [
            { 'value': '0', 'text': 'Trend of QT & Demand Utilization' },
            { 'value': '1', 'text': 'Key Machines Capability Utilization' },
            { 'value': '2', 'text': 'Key Machines Capability Utilization' }
        ],
        onSelect: function () {
            var typeValue = $("#TypeChoose").combobox("getValue");
            switch (typeValue) {
                case '0':
                    InitChart1(); //
                    $("#Chart1").css("display", "block");
                    $("#Chart2").css("display", "none");
                    $("#Chart3").css("display", "none");
                    break;
                case '1':
                    $("#container2").css('width', tabs_width);
                    $("#container2").css('height', '600px');
                    InitChart2();
                    $("#Chart1").css("display", "none");
                    $("#Chart2").css("display", "block");
                    $("#Chart3").css("display", "none");
                    break;
                case '2':
                    $("#container3").css('width', tabs_width);
                    $("#container3").css('height', '600px');
                    InitChart3();
                    $("#Chart1").css("display", "none");
                    $("#Chart2").css("display", "none");
                    $("#Chart3").css("display", "block");
                    break;
            }
        }
    });
    InitChart1();
    //搜索图表
    $("#search").unbind("click").bind('click', function () {
        if ($('#allCheckbox input:checked').length === 0) {
            $.messager.alert('Tips', "Please select at least one operation type!");
        } else {
            equipmentType();
        }
    });
    //操作类型点击事件
    var oUl = document.getElementById("allCheckbox");//获取div
    oUl.onclick = function (ev) {//事件委托
        var ev = ev || window.event;//兼容
        var target = ev.target || ev.srcElement;//兼容ie
        if (target.nodeName.toLowerCase() === 'input') {//判断点击源是否是input元素
            if ($('#allCheckbox input:checked').length === 0) {
                $.messager.alert('Tips', "Please select at least one operation type!");
            } else {
                SearchChart();
            }
        }
    };
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
//搜索图表
function SearchChart() {
    var typeValue = $("#TypeChoose").combobox("getValue");

    switch (typeValue) {
        case '0':
            InitChart1();//
            $("#Chart1").css("display", "block"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "none");
            break;
        case '1':
            InitChart2();
            $("#Chart1").css("display", "none"); $("#Chart2").css("display", "block"); $("#Chart3").css("display", "none");
            break;
        case '2':
            InitChart3();
            $("#Chart1").css("display", "none"); $("#Chart2").css("display", "none"); $("#Chart3").css("display", "block");
            break;
    }
};
var arrChecked1 = [];
var arrCheckedStr;
//加载设备类型
function equipmentType() {

    clickNum++;
    $('#EquipmentTypeDialog').dialog({
        width: 800,
        height: 500,
        modal: true,
        title: 'EquipmentType',
        draggable: true,
        buttons: [{
            text: 'Choose',
            iconCls: 'icon-ok',
            handler: function () {

                SearchChart();
                $('#EquipmentTypeDialog').dialog('close');
            }
        }, {
            text: 'Close',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#EquipmentTypeDialog').dialog('close');
            }
        }]
    });
    equipmentTypeAll();
};

function equipmentTypeAll() {
    var equipmentType;
    var str = '';
    var str1 = '';
    $.ajax({
        url: "/StatisticalManagement/GetEquipmentType",//接收一般处理程序返回来的json数据
        type: 'POST',
        dataType: 'json',
        success: function (data) {
            equipmentType = data.Data;
            for (var i = 0; i < equipmentType.length; i++) {
                str += '<div class="h_dline" id="allCheck" style="height: 23px; margin-top: 30px;margin-left: 25px;float:left;">' + '<label for="equipmentType' + i + '" class="h_d1" style="width: 100px;float:left;margin-top:5px">' + equipmentType[i] + '</label> ' + '<input type="checkbox"name="equipmentType" id="equipmentType' + i + '" value="' + equipmentType[i] + '" style="height:25px" />' + '</div>';
            }
            str1 += '<div class="h_dline" style="height: 23px; margin-top: 20px;margin-left: 25px;float:left;width:100%">' + '<label for="allAndNotAll" class="h_d1" style="width: 100px;float:left;margin-top:5px"> 全选/反选' + '</label> ' + '<input type="checkbox" id="allAndNotAll" style="height:25px" />' + '</div>';
            $("#equipmentTypeForm").html(str1 + str);

            //全选、反选
            $("#allAndNotAll").click(function () {
                arrChecked1 = [];
                if (this.checked) {
                    $("input[name='equipmentType']:checkbox").each(function () {
                        $(this).attr("checked", false);
                        $(this).click();
                    });
                } else {
                    $("input[name='equipmentType']:checkbox").each(function () {
                        $(this).attr("checked", true);
                        $(this).click();
                    });
                }
            });
            if (clickNum == 1) {
                $("#allAndNotAll").attr("checked", true);
                $("input[name='equipmentType']:checkbox").each(function () {
                    $(this).attr("checked", false);
                    arrChecked1.push(this.value); //push 进数组
                    $(this).click();
                });
            }
            //追加
            var len = $('#allCheck input[type="checkbox"]').length;
            for (let i = 0; i < len; i++) {
                $("#equipmentType" + i).click(function () {
                    if ($(this).is(':checked')) {
                        arrChecked1.push(this.value); //push 进数组

                    } else {
                        delete arrChecked1[i]; //移除数组中未选择的checkbox
                    }
                });
                //回显
                if (arrChecked1.length > 0) {
                    for (let j = 0; j < arrChecked1.length; j++) {
                        //var arrCheckedValue = arrChecked1[j];
                        $('#allCheck input[type="checkbox"][value="' + arrChecked1[j] + '"]').prop("checked", "checked");
                    }
                }
            }
        }
    });
};
// 图1
function InitChart1() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container1"));
    option = {
        title: {
            text: 'Trend of QT & Demand Utilization'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}days，{a1}：{c1}%'
        },

        legend: {
            data: ['Average QT(days)', 'Utilization(total Wt.Av)']
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

        yAxis: [{
            type: 'value',
            name: 'QT(Days)',
            axisLabel: {
                formatter: '{value}day'
            }
        }, {
            type: 'value',
            name: 'Utilization(%)',
            axisLabel: {
                formatter: '{value}%'
            }
        }
        ],
        series: [
            {
                name: 'QT',
                type: 'line',
                data: [],
                itemStyle: {
                    normal: {
                        lineStyle: {
                            width: 2.5
                        }
                    }
                }
            },
            {
                name: 'Utilization',
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
    GetEquipmentStatistics1();
};
//图2
function InitChart2() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container2"));
    option = {
        title: {
            text: 'Key Machines Capability Utilization'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}%'
        },

        legend: {
            data: ['Average QT(days)', 'Utilization(total Wt.Av)']
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

        yAxis: [{
            type: 'value',
            name: 'Utilization(%)',
            axisLabel: {
                formatter: '{value}%'
            }
        }
        ],
        series: [
            {
                name: 'Utilization',
                type: 'line',
                data: [],
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
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
    GetEquipmentStatistics2();
};
//图3
function InitChart3() {
    // 基于准备好的dom，初始化echarts实例
    var myChart = echarts.init(document.getElementById("container3"));
    option = {
        title: {
            text: 'Key Machines Capability Utilization'
        },
        tooltip: {
            trigger: 'axis',
            formatter: '{b0}<br/> {a0}：{c}}%'
        },

        legend: {
            data: ['Average QT(days)', 'Utilization(total Wt.Av)']
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

        yAxis: [{
            type: 'value',
            name: 'Utilization(%)',
            axisLabel: {
                formatter: '{value}%'
            }
        }
        ],
        series: [
            {
                name: 'Utilization',
                type: 'line',
                data: [],
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
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
    GetEquipmentStatistics3();
};
//加载折线图
function GetEquipmentStatistics1() {
    var myChartBar = echarts.init(document.getElementById("container1"));//柏拉图
    var planYear = $("#planYear").datebox("getValue");//
    var arrChecked = [];
    arrCheckedStr = arrChecked1.join(",");
    $('#allCheckbox input[type="checkbox"]:checked').each(function () {
        arrChecked.push(this.value); //push 进数组
    });

    $.ajax({
        url: "/StatisticalManagement/GetEquipmentStatistics",
        data: { 'Year': planYear, 'JobType1': arrChecked[0], 'JobType2': arrChecked[1], 'JobType3': arrChecked[2], 'JobType4': arrChecked[3], 'EquipmentType': arrCheckedStr },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var EquipmentCount = []; //设备个数
                var EquipmentTime = []; //设备耗时
                var Month = []; //月份
                var UseRatio = []; //利用率
                var EquipmentTimeToday = [];//耗时天数
                for (var i = 0; i < data.Data.length; i++) {
                    EquipmentCount[i] = data.Data[i].EquipmentCount;
                    EquipmentTime[i] = data.Data[i].EquipmentTime;
                    Month[i] = data.Data[i].Month;
                    UseRatio[i] = data.Data[i].UseRatio;
                    EquipmentTimeToday[i] = data.Data[i].EquipmentTimeToday;
                }

                //状态分析 柱状图
                myChartBar.setOption({ //加载数据图表
                    xAxis: {
                        data: Month
                    },
                    series: [
                        {
                            // 根据名字对应到相应的系列
                            name: 'Average QT(days)',
                            data: EquipmentTimeToday
                        },
                        {
                            // 根据名字对应到相应的系列
                            name: 'Utilization(total Wt.Av)',
                            data: UseRatio
                        }
                    ]
                });
            }
        }
    });
};
//加载折线图
function GetEquipmentStatistics2() {
    var myChartBar = echarts.init(document.getElementById("container2"));//柏拉图
    var planYear = $("#planYear").datebox("getValue");//
    var arrChecked = [];
    arrCheckedStr = arrChecked1.join(",");
    $('#allCheckbox input[type="checkbox"]:checked').each(function () {
        arrChecked.push(this.value); //push 进数组
    });

    $.ajax({
        url: "/StatisticalManagement/GetEquipmentStatistics",
        data: { 'Year': planYear, 'JobType1': arrChecked[0], 'JobType2': arrChecked[1], 'JobType3': arrChecked[2], 'JobType4': arrChecked[3], 'EquipmentType': arrCheckedStr },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var EquipmentCount = []; //设备个数
                var EquipmentTime = []; //设备耗时
                var Month = []; //月份
                var UseRatio = []; //利用率
                for (var i = 0; i < data.Data.length; i++) {
                    EquipmentCount[i] = data.Data[i].EquipmentCount;
                    EquipmentTime[i] = data.Data[i].EquipmentTime;
                    Month[i] = data.Data[i].Month;
                    UseRatio[i] = data.Data[i].UseRatio;
                }

                //状态分析 柱状图
                myChartBar.setOption({ //加载数据图表
                    xAxis: {
                        data: Month
                    },
                    series: [
                        {
                            // 根据名字对应到相应的系列
                            name: 'Utilization(total Wt.Av)',
                            data: UseRatio
                        }
                    ]
                });
            }
        }
    });
};
//加载折线图
function GetEquipmentStatistics3() {
    var myChartBar = echarts.init(document.getElementById("container3"));//柏拉图
    var planYear = $("#planYear").datebox("getValue");//
    var arrChecked = [];
    arrCheckedStr = arrChecked1.join(",");
    $('#allCheckbox input[type="checkbox"]:checked').each(function () {
        arrChecked.push(this.value); //push 进数组
    });

    $.ajax({
        url: "/StatisticalManagement/GetEquipmentStatistics",
        data: { 'Year': planYear, 'JobType1': arrChecked[0], 'JobType2': arrChecked[1], 'JobType3': arrChecked[2], 'JobType4': arrChecked[3], 'EquipmentType': arrCheckedStr },
        type: "POST",
        dataType: "json",
        success: function (data) {
            if (data.Success) {
                var EquipmentCount = []; //设备个数
                var EquipmentTime = []; //设备耗时
                var Month = []; //月份
                var UseRatio = []; //利用率
                for (var i = 0; i < data.Data.length; i++) {
                    EquipmentCount[i] = data.Data[i].EquipmentCount;
                    EquipmentTime[i] = data.Data[i].EquipmentTime;
                    Month[i] = data.Data[i].Month;
                    UseRatio[i] = data.Data[i].UseRatio;
                }

                //状态分析 柱状图
                myChartBar.setOption({ //加载数据图表
                    xAxis: {
                        data: Month
                    },
                    series: [
                        {
                            // 根据名字对应到相应的系列
                            name: 'Utilization(total Wt.Av)',
                            data: UseRatio
                        }
                    ]
                });
            }
        }
    });
};