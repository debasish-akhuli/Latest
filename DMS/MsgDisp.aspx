<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MsgDisp.aspx.cs" Inherits="DMS.MsgDisp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" language="javascript">
        function Trans2Main() {
            if (document.getElementById("<%= hfPageControl.ClientID %>").value == "F") {
                if (window.opener && !window.opener.closed) {
                    window.opener.document.getElementById("divNewVDocName").innerHTML = document.getElementById('<%= hfDocGuID.ClientID %>').value;
                    window.opener.document.getElementById("cmdProceed").style.display = 'block';
                    window.opener.document.getElementById("cmdDashboard").style.display = 'none';
                }
            }
            else if (document.getElementById("<%= hfPageControl.ClientID %>").value == "FE") {
                if (window.opener && !window.opener.closed) {
                    window.opener.document.getElementById("divNewVDocName").innerHTML = document.getElementById('<%= hfDocGuID.ClientID %>').value;
                    window.opener.document.getElementById("cmdProceed").style.display = 'block';
                    window.opener.document.getElementById("cmdDashboard").style.display = 'none';
                }
            }
            else if (document.getElementById("<%= hfPageControl.ClientID %>").value == "R") {
                if (window.opener && !window.opener.closed) {
                    window.opener.document.getElementById("divNewVDocName").innerHTML = document.getElementById('<%= hfDocGuID.ClientID %>').value;
                }
            }
            window.close();
        }
    </script>
    <script language="javascript" type="text/javascript">
        var alertTimerIdS = 0;
        var alertTimerIdE = 0;

        var c = 0;
        var t;
        var timer_is_on = 0;

        function BrowserWidth() {
            var theWidth;
            if (window.innerWidth) {
                theWidth = window.innerWidth;
            }
            else if (document.documentElement && document.documentElement.clientWidth) {
                theWidth = document.documentElement.clientWidth;
            }
            else if (document.body) {
                theWidth = document.body.clientWidth;
            }
            return theWidth;
        }

        function BrowserHeight() {
            var theHeight;
            if (window.innerHeight) {
                theHeight = window.innerHeight;
            }
            else if (document.documentElement && document.documentElement.clientHeight) {
                theHeight = document.documentElement.clientHeight;
            }
            else if (document.body) {
                theHeight = document.body.clientHeight;
            }
            return theHeight;
        }

        function DisplayExtendSessionLifeBox() {

            var boxElement = document.getElementById("<%= ExtendSessionLifeBox.ClientID %>");
            if (boxElement) {
                boxElement.style.display = "block";
                var bw = BrowserWidth();
                var bh = BrowserHeight();
                var boxElementWidth = (boxElement.clientWidth) ? boxElement.clientWidth : boxElement.offsetWidth;
                var boxElementHeight = (boxElement.clientHeight) ? boxElement.clientHeight : boxElement.offsetHeight;

                var boxElementTop = (bh / 2) - (boxElementHeight / 2);
                var boxElementLeft = (bw / 2) - (boxElementWidth / 2);
                boxElement.style.top = boxElementTop;
                boxElement.style.left = boxElementLeft;
                boxElement.style.position = "absolute";
            }

            var boxElement2 = document.getElementById("ExtendSessionLifeBox2");
            if (boxElement2) {
                boxElement2.style.display = "block";
                var bw = BrowserWidth();
                var bh = BrowserHeight();
                var boxElement2Width = (boxElement2.clientWidth) ? boxElement2.clientWidth : boxElement2.offsetWidth;
                var boxElement2Height = (boxElement2.clientHeight) ? boxElement2.clientHeight : boxElement2.offsetHeight;

                //var boxElement2Top = (bh / 2) - (boxElement2Height / 2);
                //var boxElement2Left = (bw / 2) - (boxElement2Width / 2);
                boxElement2.style.top = "0px";
                boxElement2.style.left = "0px";
                boxElement2.style.position = "fixed";
            }
            clearTimeout(alertTimerIdS);
            c = 20; //set count down time
            doTimer();
        }

        function CloseExtendSessionLifeBox() {
            var boxElement = document.getElementById("<%= ExtendSessionLifeBox.ClientID %>");
            var boxElement2 = document.getElementById("ExtendSessionLifeBox2");
            //checking if the element exists before trying to use it
            if (boxElement) {
                boxElement.style.display = "none";
            }
            if (boxElement2) {
                boxElement2.style.display = "none";
            }
            clearTimeout(alertTimerIdE);
        }
        function startTimer() {
            CloseExtendSessionLifeBox();
            alertTimerIdS = setTimeout(DisplayExtendSessionLifeBox, 2880000);
            alertTimerIdE = setTimeout(CloseExtendSessionLifeBox, 3480000);
        }
        window.onload = startTimer;

        function timedCount() {
            document.getElementById('countDown').innerHTML = c;
            c = c - 1;
            t = setTimeout("timedCount()", 1000);
        }

        function doTimer() {
            if (!timer_is_on) {
                timer_is_on = 1;
                timedCount();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%--For Session Expired Start--%>
        <div id="ExtendSessionLifeBox2" style=" width:100%; z-index:10000; height:100%; opacity:.2; filter:alpha(opacity=20); background:#660066; display:none;">
        </div>
        <asp:Panel ID="ExtendSessionLifeBox" runat="server" style="height:100px; z-index:10001; width:250px; background-color:#660066; padding:20px; color:#fff; border:solid 1px #666; display:none; position:absolute;">
            <div style=" width:100%; float:left">
                <asp:Label ID="ExtendSessionLifePrompt" runat="server" Text="Your session is going to expire in 10 minutes. Would you like to extend your Session for another 60 minutes?"></asp:Label>
            </div>
            <div style=" width:100%; float:left">
                <div id="countDown"></div>
            </div>
            <div style=" width:100%; float:left; padding-top:20px;">
                <div style=" float:left; padding-right:20px;">
                    <asp:UpdatePanel ID="up1" runat="server" >
                        <ContentTemplate>
                            <asp:Button ID="ExtendSessionLife" runat="server" Text="Yes" OnClientClick="startTimer(); return true;"/>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div style=" float:left">
                    <input type="button" id="CancelExtendSessionLife" value="No" onclick="CloseExtendSessionLifeBox(); return false;" />  
                </div>
            </div>
        </asp:Panel>
        <%--For Session Expired End--%>
    <div style="width:100%; height:100%; background-color:#105777; color:#cccccc;">
        <asp:HiddenField ID="hfPageControl" runat="server" />
        <asp:HiddenField ID="hfDocGuID" runat="server" />
        <script type="text/javascript" language="javascript">
            window.onload = Trans2Main;
        </script>
        <%--The entered data has been saved. Now click on "Save" button to complete the process.--%>
    </div>
    </form>
</body>
</html>