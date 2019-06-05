
$(function () {

    // add month scale
    gantt.config.scale_unit = "week";
    gantt.config.step = 1;
    gantt.templates.date_scale = function (date) {
        var dateToStr = gantt.date.date_to_str("%d %M");
        var endDate = gantt.date.add(gantt.date.add(date, 1, "week"), -1, "day");
        return dateToStr(date) + " - " + dateToStr(endDate);
    };
    gantt.config.subscales = [
        { unit: "day", step: 1, date: "%D" }
    ];
    gantt.config.scale_height = 50;

    // configure milestone description
    gantt.templates.rightside_text = function (start, end, task) {
        if (task.type == gantt.config.types.milestone) {
            return task.text;
        }
        return "";
    };
    // add section to type selection: task, project or milestone
    gantt.config.lightbox.sections = [
		{ name: "description", height: 70, map_to: "TaskRemarks", type: "textarea", focus: true },

		{ name: "type", type: "typeselect", map_to: "type" },
		{ name: "time", height: 72, type: "duration", map_to: "auto" }
    ];


    gantt.config.columns = [
    { name: "text", label: "Test Title ", tree: true, width: '200' },
     { name: "MTRNO", label: "MTR NO.", width: '120', align: "center" },
    { name: "start_date", label: "Start Date", align: "center", width: '100' },
     { name: "end_date", label: "End Date", align: "center", width: '100' },
        { name: "FollowUp", label: "FollowUp", align: "center", width: '100' },
     { name: "TaskRemarks", label: "Remarks", align: "center", width: '200' }
 

    ];
    var PlanStartDate = $("#StartTime").text();
    var PlanEndDate = $("#EndTime").text();
    var FollowUp = $("#FollowUp").text();
    var MTRNO = $("#mtr").text();
    gantt.config.autofit = true;
    gantt.config.xml_date = "%Y-%m-%d %H:%i:%s"; // format of dates in XML
    gantt.config.grid_width = 700;
    gantt.init("ganttContainer"); // initialize gantt
    gantt.load("/ScheduleManagement/LoadProjectall?PlanStartDate=" + PlanStartDate + '&PlanEndDate=' + PlanEndDate + '&FollowUp=' + FollowUp + '&MTRNO=' + MTRNO, "json");

    // enable dataProcessor
    //var dp = new dataProcessor("/Home/Save");
    // dp.init(gantt);

});
