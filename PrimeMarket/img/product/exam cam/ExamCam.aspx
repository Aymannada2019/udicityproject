<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamCam.aspx.cs" Inherits="Cpanel.Exams.ExamCam" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .tp-loader.spinner0 {
    width: 40px;
    height: 40px;
    background: url(../assets/img/icons/loader.gif) no-repeat center center;
    background-color: #fff;
    box-shadow: 0px 0px 20px 0px rgba(0,0,0,0.15);
    -webkit-box-shadow: 0px 0px 20px 0px rgba(0,0,0,0.15);
    margin-top: -240px;
    margin-left: 200px;
    -webkit-animation: tp-rotateplane 1.2s infinite ease-in-out;
    animation: tp-rotateplane 1.2s infinite ease-in-out;
    border-radius: 3px;
    -moz-border-radius: 3px;
    -webkit-border-radius: 3px;
}
    </style>
    <%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    
</head>
<body onload="window.opener.close();">

<script src='<%=ResolveUrl("~/Webcam_Plugin/jquery.webcam.js") %>' type="text/javascript"></script>
<script type="text/javascript">
    var pageUrl = '<%=ResolveUrl("ExamCam.aspx") %>';
    var ActionFunction = "GetAutoCapturedImage";
    $(function () {
        jQuery("#webcam").webcam({
            width: 320,
            height: 240,
            mode: "save",
            swffile: '<%=ResolveUrl("~/Webcam_Plugin/jscam.swf") %>',
            debug: function (type, status) {
            //$('#camStatus').append(type + ": " + status + '<br /><br />');
        },
        onSave: function (data) {
            $.ajax({
                type: "POST",
                url: pageUrl + "/" + ActionFunction,
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    if (ActionFunction == "GetUserCapturedImage") {
                        //$("#loadingimg").css('display', 'none');
                        $("[id*=imgCapture]").css("visibility", "visible");
                        $("[id*=imgCapture]").attr("src", r.d);
                    }
                    //alert(r.d);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        },
        onCapture: function () {
            webcam.save(pageUrl);
        }
    });
});
    function Capture() {
        ActionFunction = "GetUserCapturedImage";
        document.getElementById('CaptureSound').play();
        //document.getElementById('loadingimg').innerHTML="Capturing...";
        //$("#loadingimg").css('display', 'block');
        webcam.capture();
        //document.getElementById('loadingimg').innerHTML = "";
    return false;
}
    window.setInterval(function () {
        ActionFunction = "GetAutoCapturedImage";
        webcam.capture();
    // 2 minutes = 120 seconds = 120,000 ms
}, 90000);
</script>
<form id="form1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>--%>
<asp:Label runat="server" style="font-size:12pt;color:green;" ID="lbl1" Text="Ensure that your webcam is connected and ready, then click <b>Allow button</b>. After that Press the <b>Capture button</b> to capture a clear photo for your face. This photo will be printed on your certificate for validation purposes. You can repeat capturing your photo by clicking the Capture button again." ToolTip="إضغط على الزر العلوى لإلتقاط صورة شخصية مناسبة لوجهك الكريم، هذه الصورة سوف نقوم بوضعها على الشهادة لأغراض التحقق من الشخصية. ويمكنك إعادة إلتقاط الصورة بالضغط مرة أخرى على نفس الزر.">
</asp:Label>
<br />
            <br />
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td align="center">
            <span style="color:red;font-size:14pt;">
                1- Click Allow button
            </span>
            <br />
            <u>Live Camera</u>
        </td>
        <td>
        </td>
        <td align="center">
            <span style="color:red;font-size:14pt;">
                2- Look to the webcam, smile, then click Capture button
            </span>
            <br />
            <u>Certificate Photo </u>
        </td>
    </tr>
    <tr>
        <td align="center">
            <div id="webcam">
            </div>
        </td>
        <td>
            &nbsp;
        </td>
        <td align="center">
            <asp:Image ID="imgCapture" runat="server" Style="visibility: hidden; width: 320px;
                height: 240px" />
            <%--<div id="overlay" style="background-color: #1CBBB4;display:block;z-index:1000;" class="tp-loader spinner0">
                                <div class="dot1">
                                </div><div class="dot2">
                                </div><div class="bounce1">
                                </div><div class="bounce2">
                                </div><div class="bounce3">
                                </div>
                            </div>--%>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
        </td>
        <td style="text-align:center;">
            <br />
            
            <asp:Button ID="btnCapture" Text="Capture" runat="server" OnClientClick="return Capture();" />
        </td>
    </tr>
</table>
            <br />
            <span style="color:red;font-size:14pt;display:block;text-align:center;">
                3- Leave this page opened and go to exam page to start/resume your exam.
            </span>
<br />

<span id="camStatus"></span>
            
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <span id="loadingimg" style="display:block;"></span>
    <audio id="CaptureSound">
	<source src="camerashutter.mp3"/>
        </audio>
</form>
</body>
</html>
