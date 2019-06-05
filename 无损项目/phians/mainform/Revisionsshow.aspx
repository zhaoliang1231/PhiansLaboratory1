<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Revisionsshow.aspx.cs" Inherits="phians.mainform.Revisionsshow" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>报告痕迹修改</title>
    <script src="/Scripts/jquery-1.6.4.min.js"></script>
</head>
<body>
    <script type="text/javascript">
       
        function Save() {
            document.getElementById("PageOfficeCtrl1").WebSave();
        }

        function AfterDocumentOpened() {
            refreshList();
        }

        function view_office_toolbar() {
            document.getElementById("PageOfficeCtrl1").OfficeToolbars = !document.getElementById("PageOfficeCtrl1").OfficeToolbars;
        }

        //获取当前痕迹列表
        function refreshList() {

            var i;
            document.getElementById("ul_Comments").innerHTML = "";
            for (i = 1; i <= document.getElementById("PageOfficeCtrl1").Document.Revisions.Count; i++) {
                var str = "";
                str = str + document.getElementById("PageOfficeCtrl1").Document.Revisions.Item(i).Author;
                var revisionDate = document.getElementById("PageOfficeCtrl1").Document.Revisions.Item(i).Date;
                //转换为标准时间
                str = str + " " + dateFormat(revisionDate, "yyyy-MM-dd HH:mm:ss");

                if (document.getElementById("PageOfficeCtrl1").Document.Revisions.Item(i).Type == "1") {
                    str = str + ' 插入：' + document.getElementById("PageOfficeCtrl1").Document.Revisions.Item(i).Range.Text;
                }
                else if (document.getElementById("PageOfficeCtrl1").Document.Revisions.Item(i).Type == "2") {
                    str = str + ' 删除：' + document.getElementById("PageOfficeCtrl1").Document.Revisions.Item(i).Range.Text;
                }
                else {
                    str = str + ' 调整格式或样式。';
                }
                document.getElementById("ul_Comments").innerHTML += "<li><a href='#' onclick='goToRevision(" + i + ")'>" + str + "</a></li>"
            }

        }
        //GMT时间格式转换为CST
        dateFormat = function (date, format) {
            date = new Date(date);
            var o = {
                'M+': date.getMonth() + 1, //month
                'd+': date.getDate(), //day
                'H+': date.getHours(), //hour
                'm+': date.getMinutes(), //minute
                's+': date.getSeconds(), //second
                'q+': Math.floor((date.getMonth() + 3) / 3), //quarter
                'S': date.getMilliseconds() //millisecond
            };

            if (/(y+)/.test(format))
                format = format.replace(RegExp.$1, (date.getFullYear() + '').substr(4 - RegExp.$1.length));

            for (var k in o)
                if (new RegExp('(' + k + ')').test(format))
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ('00' + o[k]).substr(('' + o[k]).length));

            return format;
        }

        //定位到当前痕迹
        function goToRevision(index) {
            //var sMac = "Sub myfunc() " + "\r\n"
            //         + "ActiveDocument.Revisions.Item(" + index + ").Range.Select " + "\r\n"
            //         + "End Sub ";

            //document.getElementById("PageOfficeCtrl1").RunMacro("myfunc", sMac);
            document.getElementById("PageOfficeCtrl1").Document.Revisions.Item(index).Range.Select();

        }

        //刷新列表
        function refresh_click() {
            refreshList();
        }

      
      
        //弹窗关闭函数
        function Close() {
            window.external.close();
        }


    </script>
    <div  style=" width:1000px; height:700px;">
        <div id="Div_Comments" style=" float:left; width:150px; height:700px; border:solid 1px red;">
        <h3>痕迹列表</h3>
        <input type="button" name="refresh" value="刷新"onclick=" return refresh_click()"/>
        <ul id="ul_Comments">
            
        </ul>
        </div>
      
<div style=" width:800px; height:700px;margin-left:10px;float:left">
	
    <po:PageOfficeCtrl ID="PageOfficeCtrl1" runat="server"></po:PageOfficeCtrl>
    </div>
    </div>
</body>
</html>
