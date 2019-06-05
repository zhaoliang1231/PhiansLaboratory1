<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="read_pdf.aspx.cs" Inherits="phians.mainform.read_pdf" %>

<%@ Register Assembly="PageOffice, Version=3.0.0.1, Culture=neutral, PublicKeyToken=1d75ee5788809228" Namespace="PageOffice" TagPrefix="po" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <%= WebExtensions.CombresLink("read_pdf_Js") %>
    <%--<script type="text/javascript" src="../phians_js/read_pdf.js"></script>--%>
</head>
<body>
        <form id="form1" runat="server">
    <!--**************   卓正 PageOffice 客户端代码开始    ************************-->
	<script language="javascript" type="text/javascript">
	    function AfterDocumentOpened() {
	        //alert(document.getElementById("PDFCtrl1").Caption);
	    }
	    function Print() {
	        document.getElementById("PDFCtrl1").ShowDialog(4);
	    }
	    function SwitchFullScreen() {
	        document.getElementById("PDFCtrl1").FullScreen = !document.getElementById("PDFCtrl1").FullScreen;
	    }
	    function SwitchBKMK() {
	        document.getElementById("PDFCtrl1").BookmarksVisible = !document.getElementById("PDFCtrl1").BookmarksVisible;
	    }
	    function SetPageReal() {
	        document.getElementById("PDFCtrl1").SetPageFit(1);
	    }
	    function SetPageFit() {
	        document.getElementById("PDFCtrl1").SetPageFit(2);
	    }
	    function SetPageWidth() {
	        document.getElementById("PDFCtrl1").SetPageFit(3);
	    }
	    function ZoomIn() {
	        document.getElementById("PDFCtrl1").ZoomIn();
	    }
	    function ZoomOut() {
	        document.getElementById("PDFCtrl1").ZoomOut();
	    }
	    function FirstPage() {
	        document.getElementById("PDFCtrl1").GoToFirstPage();
	    }
	    function PreviousPage() {
	        document.getElementById("PDFCtrl1").GoToPreviousPage();
	    }
	    function NextPage() {
	        document.getElementById("PDFCtrl1").GoToNextPage();
	    }
	    function LastPage() {
	        document.getElementById("PDFCtrl1").GoToLastPage();
	    }
	    function RotateRight() {
	        document.getElementById("PDFCtrl1").RotateRight();
	    }
	    function RotateLeft() {
	        document.getElementById("PDFCtrl1").RotateLeft();
	    }

	</script>
    <!--**************   卓正 PageOffice 客户端代码结束    ************************-->
    <div style="width:1000px; height:800px;">
        <po:PDFCtrl ID="PDFCtrl1" runat="server" onload="PDFCtrl1_Load" 
            Theme="Office2007">
        </po:PDFCtrl>
        </div>
            </form>
</body>
</html>
