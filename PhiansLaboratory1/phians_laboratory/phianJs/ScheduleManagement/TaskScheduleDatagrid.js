var line = 0;
/** 
* 取得url参数 
*/
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); // 构造一个含有目标参数的正则表达式对象  
    var r = window.location.search.substr(1).match(reg);  // 匹配目标参数  
    if (r != null) return unescape(r[2]); return null; // 返回参数值  
}
var taskId = getUrlParam('taskId');
$(function () {
    TaskScheduleDatagrid();//信息列表加载
});

//*******************************************信息列表***********************************************************************************************************************************************************************************************************
function TaskScheduleDatagrid() {
   
    $('#TaskScheduleDatagrid').datagrid({
        nowrap: false,
        striped: true,
        rownumbers: true,
        border: true,
        fitColumns: true,
        pagination: true,
        ctrlSelect: true,
        fit: true,
        pageSize: 15,
        pageList: [15, 30, 45, 60],
        pageNumber: 1,
        type: 'POST',
        queryParams: {
            taskId: taskId
        },
        dataType: 'json',
        url: "/ScheduleManagement/GetTaskScheduleDetailList",
        columns: [[
       { field: 'MotorNum', title: 'MotorNum' },//MTR单号
     
       { field: 'EquipmentNum', title: 'EquipmentNum' },//第几周
       { field: 'FixtureCode', title: 'FixtureCode' },//测试项目编号
       { field: 'ControllersStr', title: 'ControllersStr' },//测试样品的类型
         {
             field: 'BeginTime', title: 'BeginTime'
         },
       {
           field: 'EndTime', title: 'EndTime'
       }
        ]],
        onLoadSuccess: function (data) {
            $('#TaskScheduleDatagrid').datagrid('selectRow', line);

        },
       // toolbar: "#MTR_information_toolbar"
    });
};
