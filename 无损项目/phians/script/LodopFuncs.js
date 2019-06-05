
//====页面引用CLodop云打印必须的JS文件：====

//让其它电脑的浏览器通过本机打印（适用例子）：
var oscript = document.createElement("script");
oscript.src ="/CLodopfuncs.js";
var head = document.head || document.getElementsByTagName("head")[0] || document.documentElement;
head.insertBefore( oscript,head.firstChild );
//让本机浏览器打印(更优先一点)：
oscript = document.createElement("script");
oscript.src ="http://localhost:8000/CLodopfuncs.js?priority=1";
var head = document.head || document.getElementsByTagName("head")[0] || document.documentElement;
head.insertBefore( oscript,head.firstChild );
//为本机浏览器打印加一个后补端口8001(这种兼顾做法可能报错不用理它)：
oscript = document.createElement("script");
oscript.src ="http://localhost:8001/CLodopfuncs.js?priority=2";
var head = document.head || document.getElementsByTagName("head")[0] || document.documentElement;
head.insertBefore( oscript,head.firstChild );

//====获取LODOP对象的主过程：====
function getLodop(oOBJECT,oEMBED){
    var strCLodopInstall="<br><font color='#FF00FF'>CLodop云打印服务(localhost本地)未安装启动!点击这里<a href='CLodopPrint_Setup_for_Win32NT.exe' target='_self'>执行安装</a>,安装后请刷新页面。</font>";
    var strCLodopUpdate="<br><font color='#FF00FF'>CLodop云打印服务需升级!点击这里<a href='CLodopPrint_Setup_for_Win32NT.exe' target='_self'>执行升级</a>,升级后请刷新页面。</font>";
    var LODOP;
    try{
        var isIE = (navigator.userAgent.indexOf('MSIE')>=0) || (navigator.userAgent.indexOf('Trident')>=0);
        try{ LODOP=getCLodop();} catch(err) {};
        if (!LODOP && document.readyState!=="complete") {alert("C-Lodop没准备好，请稍后再试！"); return;};
        if (!LODOP) {  
		 if (isIE) document.write(strCLodopInstall); else
		 document.documentElement.innerHTML=strCLodopInstall+document.documentElement.innerHTML;
                 return;
        } else {

	         if (CLODOP.CVERSION<"2.0.6.8") { 
			if (isIE) document.write(strCLodopUpdate); else
			document.documentElement.innerHTML=strCLodopUpdate+document.documentElement.innerHTML;
		 };
		 if (oEMBED && oEMBED.parentNode) oEMBED.parentNode.removeChild(oEMBED);
		 if (oOBJECT && oOBJECT.parentNode) oOBJECT.parentNode.removeChild(oOBJECT);	
	};
        //===如下空白位置适合调用统一功能(如注册语句、语言选择等):===
        LODOP.SET_LICENSES("", "641424750555055555756656856128", "688858710010010811411756128900", "");
        //===========================================================
        return LODOP;
    } catch(err) {alert("getLodop出错:"+err);};
};

function needCLodop(){
	return true;
};
