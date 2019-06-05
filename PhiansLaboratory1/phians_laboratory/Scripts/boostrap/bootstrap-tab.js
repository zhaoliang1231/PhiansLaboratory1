var addTabs = function (options) {
    //可以在此处验证session
    //var rand = Math.random().toString();
    //var id = rand.substring(rand.indexOf('.') + 1);
   // var url = window.location.protocol + '//' + window.location.host;
    //var url;
    //options.url =  options.url;
    id = "tab_" + options.id;
    var active_flag = false;
    if($("#" + id)){
        active_flag = $("#" + id).hasClass('active');
    }
    $(".active").removeClass("active");
    //如果TAB不存在，创建一个新的TAB

    if (!$("#" + id)[0]) {
        //固定TAB中IFRAME高度
        mainHeight = $(document.body).height();
        //创建新TAB的title
        title = '<li role="presentation" id="tab_' + id + '"><a href="#' + id + '" aria-controls="' + id + '" role="tab" data-toggle="tab"><i class="'+options.icon+'"></i>' + options.title;
        //是否允许关闭
        if (options.close) {
            title += ' <i class="glyphicon glyphicon-remove-sign" tabclose="' + id + '"></i>';
        }
        title += '</a></li>';
        //是否指定TAB内容
        //console.log(options.url);
        if (options.content) {
            content = '<div role="tabpanel" class="tab-pane" id="' + id + '">' + options.content + '</div>';
        } else {//没有内容，使用IFRAME打开链接
            content = '<div role="tabpanel" class="tab-pane" id="' + id + '"><iframe id="iframe_' + id + '" src="' + options.url +
                '" width="100%" height="100%" onload="changeFrameHeight(this)" frameborder="no" border="0" marginwidth="0" marginheight="0" scrolling="yes" allowtransparency="yes"></iframe></div>';
        }
        //加入TABS
        //判断如果标签大于5就不再追加
        var li_length = $(".nav-tabs li").length;
        if (li_length <= 8) {
           
            $(".nav-tabs").append(title);
            $(".tab-content").append(content);
        } else {
            layer.msg("标签太长！请先关闭打开页面");
        }

    }else{
        if(active_flag){
            $("#iframe_" + id).attr('src', $("#iframe_" + id).attr('src'));
        }
    }
    //激活TAB
    $("#tab_" + id).addClass('active');
    $("#" + id).addClass("active");
};
//content height
var changeFrameHeight = function (that) {
    $(that).height(document.documentElement.clientHeight - 135);
    $(that).parent(".tab-pane").height(document.documentElement.clientHeight - 130-150);
}
var closeTab = function (id) {
    //如果关闭的是当前激活的TAB，激活他的前一个TAB
    if ($("li.active").attr('id') == "tab_" + id) {
        $("#tab_" + id).prev().addClass('active');
        $("#" + id).prev().addClass('active');
    }
    //关闭TAB
    $("#tab_" + id).remove();
    $("#" + id).remove();
};
$(function () {
    $("[addtabs]").click(function () {
        addTabs({ id: $(this).attr("id"), title: $(this).attr('title'), close: true });
    });
    //关闭
    $(".nav-tabs").on("click", "[tabclose]", function (e) {
        id = $(this).attr("tabclose");
        closeTab(id);
    });
    //双击关闭
    $(".nav-tabs").dblclick(function () {
        closeTab(id);
    });
    window.onresize = function () {
        var target = $(".tab-content .active iframe");
        changeFrameHeight(target);
    }
});
