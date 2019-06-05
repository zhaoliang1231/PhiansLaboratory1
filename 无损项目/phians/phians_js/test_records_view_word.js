$(function () {

    //参数获取
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串
        // alert(url);
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
    var Request = new Object();
    Request = GetRequest();
    //隐藏域--获取用户名
    // document.getElementById('HiddenField1').value = Request['user_count'];


    //$('#iframe1').prop('contentWindow').document.getElementById("PageOfficeCtrl1").Close();
    document.getElementById("view_test_records").WebOpen(Request['url'], "docNormalEdit", "rtr");

    var i = 0;
    $('#data_input').textbox({
        iconCls: 'icon-edit',
        iconAlign: 'left',
        onChange: function () {
            // alert(1);            
          
            
            switch(i)
            {
                case 0:
                    document.getElementById("view_test_records").DataRegionList.GetDataRegionByName("PO_In_random_1_mm_range").Value = $('#data_input').textbox("getText");
                    break;
                case 1:
                    document.getElementById("view_test_records").DataRegionList.GetDataRegionByName("PO_In_random_2_mm_range").Value = $('#data_input').textbox("getText");
                    break;
                case 2:
                    document.getElementById("view_test_records").DataRegionList.GetDataRegionByName("PO_In_random_10_mm_range").Value = $('#data_input').textbox("getText");
                    break;
                case 3:
                    document.getElementById("view_test_records").DataRegionList.GetDataRegionByName("PO_In_random_30_mm_range").Value = $('#data_input').textbox("getText");
                    break;
               
            }            
            if (i == 3) {
                i = 0;
            } else
            {
                i = i + 1;
            }                       
            }
           
        
    });
  
    $('#A3').unbind('click').bind('click', function () {
        if (i == 3) {
            i = 0;
        } else {
            i = i + 1;
        }
    });

    $('#A2').unbind('click').bind('click', function () {
        if (i == 0) {
            i = 0;
        }
        else
        {
            i = i - 1;
        }
        
    });
})