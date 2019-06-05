$(function(){
    //$('#test').datagrid({  
    //    title: '统计 报表',  
    //    iconCls: 'icon-save',  
    //    width: 1000,  
    //    height: 550,  
    //    url: "Handler.ashx",//接收一般处理程序返回来的json数据  
        
       

    //    columns: [[  
      
    //    { field: 'name',title:'Item ID', width: 100 },//field后面就改为你自己的数据表字段，然后可以调整宽度什么的          
    //    ]],  
    //    pagination: true,  
    //    rownumbers: true  
    //})


    //$('#testtree').tree({
    //    url: "../test/tree_json_test.ashx",
    //    method: 'get',
    //    required: true,


    //});
    $('#loginbtn_word').unbind('click').bind('click', function () {
        var url = $("#report_Auditing").attr("href");
        //alert(url)
       // var arr_split1 = url.split('|');
        var arr_split = url.split('==|');
        
        //alert(arr_split[0]);
        //alert(arr_split[0]);
        //alert(arr_split[1]);
        //var url="<%=PageOffice.PageOfficeLink.OpenWindow("../mainform/report_Issue_page.aspx?","width=screen.width ;height=screen.height;")%>";
       // $("#report_Auditing").prop("href", arr_split[0] + "==|&" + "url=/upload_Folder/certificate_Template/f31e152f-f8bd-4760-bf77-b3467e674933.doc");
        //alert($("#report_Auditing").attr("href"));
        document.getElementById("report_Auditing").click();
        
    })

}


)