<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="DMS.home" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>myDOCbase</title>
    <link href="style.css" rel="stylesheet" type="text/css" media="all" />
    <link href="editable.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="js/jquary_min.js"></script>
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="SyntaxHighlighter.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="shCore.js" language="javascript"></script>
    <script type="text/javascript" src="shBrushJScript.js" language="javascript"></script>
    <script type="text/javascript" src="ModalPopups.js" language="javascript"></script>
    <script type="text/javascript" src="js/jquery-1.6.1.min.js"></script>
    <script type="text/javascript" src="js/jquery.zclip.js"></script>
    <script type="text/javascript" src="js/jquery.zclip.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".drop_nav .drop").addClass('hide');
            $(".drop_nav").mouseover(function () {
                $(this).children(".drop").addClass('show');
                $(this).children(".drop").removeClass('hide');
            });
            $(".drop_nav").mouseout(function () {
                $(".drop_nav .drop").addClass('hide');
                $(this).children(".drop").removeClass('show');
            });
            $(".drop_nav1").mouseover(function () {
                $(this).children(".drop1").addClass('show');
                $(this).children(".drop1").removeClass('hide');
            });
            $(".drop_nav1").mouseout(function () {
                $(this).children(".drop1").addClass('hide');
                $(this).children(".drop1").removeClass('show');
            });
            $("#copy-button").zclip({
                path: "js/ZeroClipboard.swf",
                copy: function () {
                    return $("#hfDocURL").val();
                }
            });
        });
    </script>
    <script type="text/javascript">
        var ActToMail = new String();
        function CheckedOpt(SelOpt) {
            document.getElementById("<%= hfSelCheckIn.ClientID %>").value = SelOpt.toString();
        }
        function AddMail() {
            var TextBoxVal = document.getElementById("<%= txtToMail.ClientID %>");
            var Flag = true;
            var e = document.getElementById("ddUser");
            var strMail = e.options[e.selectedIndex].value;
            var Arr = new Array();
            Arr = ActToMail.split(',');
            for (var i = 0; i < Arr.length; i++) {
                if (Arr[i] == strMail) {
                    Flag = false;
                    break;
                }
            }
            if (Flag) {
                ActToMail += ActToMail == "" ? strMail : "," + strMail;
            }
            TextBoxVal.value = ActToMail.toString();
        }
        function msgDisp(SelGrpID) {
            document.getElementById("<%= hfSelGrpID.ClientID %>").value = SelGrpID.toString();
        }
        function showDocURL() {
            if (document.getElementById("<%=hfDocument.ClientID%>").value == "") {
                alert("Please select a document");
                return false;
            }
            else {
                return true;
            }
        }

        function showLoading() {
            if (document.getElementById("<%=hfDocument.ClientID%>").value == "") {
                alert("Please select a document");
            }
            else {
                if (document.getElementById("<%= hfSelDocStat.ClientID %>").value == "Check Out") {
                    alert("This Document is Checked Out by " + document.getElementById("<%= hfCheckedOutByFullName.ClientID %>").value);
                }
                else {
                    if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "X") {
                        alert("You have no rights to perform this operation !!");
                    }
                    else if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "V") {
                        alert("You have no rights to perform this operation !!");
                    }
                    else {
                        document.getElementById("dvLoading1").style.display = 'block';
                        document.getElementById("dvLoading2").style.display = 'block';
                    }
                }
            }
        }
        function hideLoading() {
            document.getElementById("dvLoading1").style.display = 'none';
            document.getElementById("dvLoading2").style.display = 'none';
            return true;
        }
        function alerts() {
            var a = document.getElementById("<%=hfDocURL.ClientID%>").value;
            alert(a);

        }
        function showGroupLoading() {
            if (document.getElementById("<%=hfDocument.ClientID%>").value == "") {
                alert("Please select a document");
            }
            else {
                if (document.getElementById("<%= hfSelDocStat.ClientID %>").value == "Check Out") {
                    alert("This Document is Checked Out by " + document.getElementById("<%= hfCheckedOutByFullName.ClientID %>").value);
                }
                else {
                    document.getElementById("divGroupLoading").style.display = 'block';
                    document.getElementById("divGroup").style.display = 'block';
                }
            }
        }
        function hideGroupLoading() {
            document.getElementById("divGroupLoading").style.display = 'none';
            document.getElementById("divGroup").style.display = 'none';
            return true;
        }

        function showMoveLoading() {
            if (document.getElementById("<%=hfDocument.ClientID%>").value == "") {
                alert("Please select a document");
            }
            else {
                if (document.getElementById("<%= hfSelDocStat.ClientID %>").value == "Check Out") {
                    alert("This Document is Checked Out by " + document.getElementById("<%= hfCheckedOutByFullName.ClientID %>").value);
                }
                else {
                    if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "X") {
                        alert("You have no rights to perform this operation !!");
                    }
                    else if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "V") {
                        alert("You have no rights to perform this operation !!");
                    }
                    else {
                        document.getElementById("divMoveLoading").style.display = 'block';
                        document.getElementById("divMove").style.display = 'block';
                    }
                }
            }
        }
        function hideMoveLoading() {
            document.getElementById("divMoveLoading").style.display = 'none';
            document.getElementById("divMove").style.display = 'none';
            return true;
        }

        function DocSel4View() {
            if (document.getElementById("<%=hfDocument.ClientID%>").value == "") {
                alert("Please select a document");
                return false;
            }
            else {
                if (document.getElementById("<%=hfDocExt.ClientID%>").value == "pdf") {
                    rowDblClick(document.getElementById("<%= hf_SelDocID.ClientID %>").value);
                }
                else {
                    DocDownld(document.getElementById("<%= hf_SelDocID.ClientID %>").value);
                }
                return true;
            }
        }

        function FetchPermission() {
            if (document.getElementById("<%= hfDocument.ClientID %>").value == "") {
                alert("Please select a document");
            }
            else if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "X") {
                alert("You have no rights to perform this operation !!");
                return false;
            }
            else if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "V") {
                alert("You have no rights to perform this operation !!");
                return false;
            }
            else {
                return true;
            }
        }

        function FetchPermission4Folder() {
            if (document.getElementById("<%= hfSelFldPermission.ClientID %>").value == "X") {
                alert("You have no rights to perform this operation !!");
                return false;
            }
            else if (document.getElementById("<%= hfSelFldPermission.ClientID %>").value == "V") {
                alert("You have no rights to perform this operation !!");
                return false;
            }
            else {
                return true;
            }
        }

        function showCheckInLoading() {
            if (document.getElementById("<%= hfDocument.ClientID %>").value == "") {
                alert("Please select a document");
            }
            else {
                if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "X") {
                    alert("You have no rights to perform this operation !!");
                }
                else if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "V") {
                    alert("You have no rights to perform this operation !!");
                }
                else if (document.getElementById("<%= hfSelDocStat.ClientID %>").value == "Check Out") {
                    if (document.getElementById("<%= hfUserID.ClientID %>").value == "admin") {
                        document.getElementById("divCheckInLoading").style.display = 'block';
                        document.getElementById("divCheckIn").style.display = 'block';
                    }
                    else if (document.getElementById("<%= hfUserID.ClientID %>").value == document.getElementById("<%= hfCheckedOutBy.ClientID %>").value) {
                        document.getElementById("divCheckInLoading").style.display = 'block';
                        document.getElementById("divCheckIn").style.display = 'block';
                    }
                    else {
                        alert("This Document is Checked Out by " + document.getElementById("<%= hfCheckedOutByFullName.ClientID %>").value);
                    }
                }
                else {
                    alert("The Document is already Checked In");
                }
            }
        }
        function hideCheckInLoading() {
            document.getElementById("divCheckInLoading").style.display = 'none';
            document.getElementById("divCheckIn").style.display = 'none';
            return true;
        }

        function showCommentsLoading() {
            if (document.getElementById("<%=hfDocument.ClientID%>").value == "") {
                alert("Please select a document");
            }
            else {
                if (document.getElementById("<%= hfSelDocStat.ClientID %>").value == "Check Out") {
                    alert("This Document is Checked Out by " + document.getElementById("<%= hfCheckedOutByFullName.ClientID %>").value);
                }
                else {
                    if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "X") {
                        alert("You have no rights to perform this operation !!");
                    }
                    else {
                        document.getElementById("dvCommentsLoading1").style.display = 'block';
                        document.getElementById("dvCommentsLoading2").style.display = 'block';
                    }
                }
            }
        }
        function hideCommentsLoading() {
            document.getElementById("dvCommentsLoading1").style.display = 'none';
            document.getElementById("dvCommentsLoading2").style.display = 'none';
            return true;
        }

        function showMailLoading() {
            if (document.getElementById("<%=hfDocument.ClientID%>").value == "") {
                alert("Please select a document");
            }
            else {
                if (document.getElementById("<%= hfSelDocStat.ClientID %>").value == "Check Out") {
                    alert("This Document is Checked Out by " + document.getElementById("<%= hfCheckedOutByFullName.ClientID %>").value);
                }
                else {
                    if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "X") {
                        alert("You have no rights to perform this operation !!");
                    }
                    else if (document.getElementById("<%= hfSelDocPermission.ClientID %>").value == "V") {
                        alert("You have no rights to perform this operation !!");
                    }
                    else {
                        document.getElementById("divMailLoading").style.display = 'block';
                        document.getElementById("divMail").style.display = 'block';
                    }
                }
            }
        }
        function hideMailLoading() {
            document.getElementById("divMailLoading").style.display = 'none';
            document.getElementById("divMail").style.display = 'none';
            return true;
        }
        function CloseCheckInWindow() {
            $('#divCheckInDef').show();
            $('#divCheckInLoading').hide();
            $('#divCheckIn').hide();
            return true;
        }

        function ChkEmail() {
            var value1 = document.getElementById("<%= txtToMail.ClientID %>").value;
            document.getElementById("<%= hfEmailStat.ClientID %>").value = "Y";
            var seperator = ',';
            if (value1 == '') {
                document.getElementById("<%= hfEmailStat.ClientID %>").value = "N";
                alert('Please enter the Email ID!');
            }
            else if (value1 != '') {
                var result1 = value1.split(seperator);
                for (var i = 0; i < result1.length; i++) {
                    if (result1[i] != '') {
                        if (!validateEmail(result1[i])) {
                            document.getElementById("<%= hfEmailStat.ClientID %>").value = "N";
                            alert('Please check, email addresses not valid!');
                        }
                        else {
                            if (document.getElementById("<%= txtSubject.ClientID %>").value == '') {
                                document.getElementById("<%= hfEmailStat.ClientID %>").value = "N";
                                alert('Please enter the subject!');
                            }
                            else if (document.getElementById("<%= txtMsg.ClientID %>").value == '') {
                                document.getElementById("<%= hfEmailStat.ClientID %>").value = "N";
                                alert('Please enter the message!');
                            }
                        }
                    }
                }
            }
        }
        function validateEmail(field) {
            var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
            return (regex.test(field)) ? true : false;
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                if (document.getElementById("<%= hfMsg.ClientID %>").value != undefined && document.getElementById("<%= hfMsg.ClientID %>").value != "") {
                    alert(document.getElementById('<%= hfMsg.ClientID %>').value);
                    document.getElementById('<%= hfMsg.ClientID %>').value = "";
                }
            }
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler1);
            function EndRequestHandler1(sender, args) {
                if (document.getElementById("<%= hfMsg1.ClientID %>").value != undefined && document.getElementById("<%= hfMsg1.ClientID %>").value != "") {
                    ModalPopups.Indicator("idIndicator1", " ", document.getElementById('<%= hfMsg1.ClientID %>').value, { width: 300, height: 100 });

                    setTimeout('ModalPopups.Close(\"idIndicator1\");', 2000);
                    document.getElementById('<%= hfMsg1.ClientID %>').value = "";
                }
            }
            $("#close2").click(function () {
                $('#def').show();
                $('#dvLoading1').hide();
                $('#dvLoading2').hide();
            });

            $("#GroupClose").click(function () {
                $('#divGroupDef').show();
                $('#divGroupLoading').hide();
                $('#divGroup').hide();
            });

            $("#MoveClose").click(function () {
                $('#divMoveDef').show();
                $('#divMoveLoading').hide();
                $('#divMove').hide();
            });

            $("#CheckInClose").click(function () {
                $('#divCheckInDef').show();
                $('#divCheckInLoading').hide();
                $('#divCheckIn').hide();
            });

            $("#MailClose").click(function () {
                $('#divMailDef').show();
                $('#divMailLoading').hide();
                $('#divMail').hide();
            });

            $(".dropmain li").mouseover(function () {
                $(this).children('ul').addClass('g_show');
                $(this).children('ul').removeClass('g_hide');
            });

            $(".dropmain li").mouseout(function () {
                $(this).children('ul').removeClass('g_show')
                $(this).children('ul').addClass('g_hide')
            });

            $(".dropmain2 li").mouseover(function () {
                $(this).children('ul.dropmain3').addClass('g_show');
                $(this).children('ul.dropmain3').removeClass('g_hide');
            });

            $(".dropmain2 li").mouseout(function () {
                $(this).children('ul.dropmain3').addClass('g_hide');
                $(this).children('ul.dropmain3').removeClass('g_show');
            });
        });
    </script>
    <style type="text/css">
        *
        {
            padding: 0px;
            margin: 0px;
        }
    </style>
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
            document.getElementById("<%= ExtendSessionLife.ClientID %>").click();
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

        function StartOnloading() {
            hideLoading();
            startTimer();
        }

        function DocDownld(DocID) {
            var jsVar = "DownldDoc";
            document.getElementById("<%= hfSelDocID.ClientID %>").value = DocID.toString();
            __doPostBack5('callPostBack5', jsVar);
        }
        function rowDblClick(DocID) {
            var jsVar = "OpenDoc";
            document.getElementById("<%= hfSelDocID.ClientID %>").value = DocID.toString();
            __doPostBack5('callPostBack5', jsVar);
        }
        function __doPostBack5(eventTarget2, eventArgument2) {
            document.form1.__EVENTTARGET1.value = "";
            document.form1.__EVENTARGUMENT1.value = "";
            document.form1.__EVENTTARGET2.value = eventTarget2;
            document.form1.__EVENTARGUMENT2.value = eventArgument2;
            document.form1.submit();
        }
        window.onload = StartOnloading;
    </script>
    <script type="text/javascript" src="js/jquery.mCustomScrollbar.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
        <div class="main">
            <asp:HiddenField ID="hfCabinet" runat="server" />
            <asp:HiddenField ID="hfDrawer" runat="server" />
            <asp:HiddenField ID="hfFolder" runat="server" />
            <asp:HiddenField ID="hfSelDocID" runat="server" />
            
            <input type="hidden" name="__EVENTTARGET1" value="" />
            <input type="hidden" name="__EVENTARGUMENT1" value="" />
            <input type="hidden" name="__EVENTTARGET2" value="" />
            <input type="hidden" name="__EVENTARGUMENT2" value="" />
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <%--For Session Expired Start--%>
            <div id="ExtendSessionLifeBox2" style="width: 100%; z-index: 10000; height: 100%;
                opacity: .2; filter: alpha(opacity=20); background: #660066; display: none;">
            </div>
            <asp:Panel ID="ExtendSessionLifeBox" runat="server" Style="height: 100px; z-index: 10001;
                width: 250px; background-color: #660066; padding: 20px; color: #fff; border: solid 1px #666;
                display: none; position: absolute;">
                <div style="width: 100%; float: left">
                    <asp:Label ID="ExtendSessionLifePrompt" runat="server" Text="Your session is going to expire in 10 minutes. Would you like to extend your Session for another 60 minutes?"></asp:Label>
                </div>
                <div style="width: 100%; float: left">
                    <div id="countDown">
                    </div>
                </div>
                <div style="width: 100%; float: left; padding-top: 20px;">
                    <div style="float: left; padding-right: 20px;">
                        <asp:UpdatePanel ID="up1" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="ExtendSessionLife" runat="server" Text="Yes" OnClientClick="startTimer(); return true;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div style="float: left">
                        <input type="button" id="CancelExtendSessionLife" value="No" onclick="CloseExtendSessionLifeBox(); return false;" />
                    </div>
                </div>
            </asp:Panel>
            <%--For Session Expired End--%>
            <div class="logo_bg">
                <%--<a href="#">
                    <img src="images/logo.jpg" width="286" height="88" alt="" border="0" /></a>--%></div>
            <h1>
                <a href="userhome.aspx" style="border: none; text-decoration: none; outline: none;">
                    <img src="images/product_logo.png" border="0" alt="" /></a></h1>
        </div>
    </div>
    <div class="main_body">
        <div class="main">
            <div class="normal_common">
                <div class="wf_mas_Heading">
                    <span>e-Filing System for </span>
                    <asp:Label ID="lblUser" runat="server" Text="User Name"></asp:Label>
                    <div class="dropdown" id="divMenuSuperAdmin" runat="server">
                        <div class="menu_bg_n">
                            <ul>
                                <li class="drop_nav"><a href="#">Filing</a>
                                    <ul class="drop hide">
                                        <li><a href="home.aspx">e-Filing system</a></li>
                                        <li><a href="cabinet_mast.aspx">Cabinet</a></li>
                                        <li><a href="drawer_mast.aspx">Drawer</a></li>
                                        <li><a href="folder_mast.aspx">Folder</a></li>
                                        <li><a href="doc_type_mast.aspx">Document Type</a></li>
                                        <li><a href="BlankTempUpload.aspx">New Template Upload</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
                                        <%--<li><a href="grp_mast.aspx">Groups</a></li>
                                        <li><a class="nob" href="grp_doc.aspx">Group-wise Document</a></li>--%>
                                    </ul>
                                </li>
                                <li class="drop_nav"><a href="#">System</a>
                                    <ul class="drop hide">
                                        <li class="drop_nav1"><a href="#">User</a>
                                            <ul class="drop1 hide">
                                                <li><a href="dept_mast.aspx">Department</a></li>
                                                <li><a href="role_mast.aspx">Role</a></li>
                                                <li><a href="user_mast.aspx">New User</a></li>
                                                <li><a href="user_rights.aspx">User Rights</a></li>
                                                <li><a href="user_role.aspx" class="nob">User-wise Role Mapping</a></li>
                                            </ul>
                                        </li>
                                        <li><a href="workflow_mast.aspx">Workflow Definition</a></li>
                                        <li><a href="reset_pwd.aspx">Reset Password</a></li>
                                        <li><a href="mail_setup.aspx">Mail Setup</a></li>
                                        <li><a href="CompWiseStatistics.aspx">Statistics</a></li>
                                        <li><a href="ClientManagement.aspx">Client Management</a></li>
                                        <li><a class="nob" href="ServerConfig.aspx">Server Config</a></li>
                                    </ul>
                                </li>
                                <li class="no"><a href="logout.aspx">Logout</a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="dropdown" id="divMenuAdmin" runat="server">
                        <div class="menu_bg_n">
                            <ul>
                                <li class="drop_nav"><a href="#">Filing</a>
                                    <ul class="drop hide">
                                        <li><a href="home.aspx">e-Filing system</a></li>
                                        <li><a href="cabinet_mast.aspx">Cabinet</a></li>
                                        <li><a href="drawer_mast.aspx">Drawer</a></li>
                                        <li><a href="folder_mast.aspx">Folder</a></li>
                                        <li><a href="doc_type_mast.aspx">Document Type</a></li>
                                        <li><a href="BlankTempUpload.aspx">New Template Upload</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
                                        <%--<li><a href="grp_mast.aspx">Groups</a></li>
                                        <li><a class="nob" href="grp_doc.aspx">Group-wise Document</a></li>--%>
                                    </ul>
                                </li>
                                <li class="drop_nav"><a href="#">System</a>
                                    <ul class="drop hide">
                                        <li class="drop_nav1"><a href="#">User</a>
                                            <ul class="drop1 hide">
                                                <li><a href="dept_mast.aspx">Department</a></li>
                                                <li><a href="role_mast.aspx">Role</a></li>
                                                <li><a href="user_mast.aspx">New User</a></li>
                                                <li><a href="user_rights.aspx">User Rights</a></li>
                                                <li><a href="user_role.aspx" class="nob">User-wise Role Mapping</a></li>
                                            </ul>
                                        </li>
                                        <li><a href="workflow_mast.aspx">Workflow Definition</a></li>
                                        <li><a href="reset_pwd.aspx">Reset Password</a></li>
                                        <li><a class="nob" href="Billing.aspx">Statistics</a></li>
                                    </ul>
                                </li>
                                <li class="no"><a href="logout.aspx">Logout</a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="dropdown" id="divMenuNormal" runat="server">
                        <div class="menu_bg_n">
                            <ul>
                                <li id="menuGenHome" runat="server"><a href="userhome.aspx">Home</a></li>
                                <li class="drop_nav"><a href="#">Filing</a>
                                <ul class="drop hide">
                                    <li><a href="home.aspx">e-Filing system</a></li>
                                    <li><a href="cabinet_mast.aspx">Cabinet</a></li>
                                    <li><a href="drawer_mast.aspx">Drawer</a></li>
                                    <li><a href="folder_mast.aspx">Folder</a></li>
                                    <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
                                </ul>
                                </li>
                                <li id="menuGenSystem" runat="server" class="drop_nav"><a href="#">System</a>
                                    <ul class="drop hide">
                                        <li><a class="nob" href="reset_pwd.aspx">Reset Password</a></li>
                                    </ul>
                                </li>
                                <li class="no"><a href="logout.aspx">Logout</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="cabinet_top">
                &nbsp;</div>
            <div class="cabinet_bg">
                <div class="normal" style="position:relative;">
                    <div id="mcs2_container">
                        <div class="customScrollBox">
                            <div class="container">
                                <div style="height: 350px; overflow: auto;">
                                    <div id="divCabinet" runat="server" class="content">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvCabinet" runat="server" Style="border: none;" ShowHeader="False"
                                                    Width="100%" CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" CaptionAlign="Top"
                                                    EnableModelValidation="True" GridLines="None" Font-Size="12px" Font-Bold="true"
                                                    OnSelectedIndexChanged="gvCabinet_SelectedIndexChanged" DataKeyNames="cab_uuid">
                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <PagerStyle ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#c9c9c9" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left"
                                                        Font-Size="12px" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCabinet" runat="server" Text='<%#Eval("cab_uuid")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name" ShowHeader="False">
                                                            <ItemStyle CssClass="CabinetItem" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" CssClass="CabinetItem_Full_Link" runat="server"
                                                                    CommandName="Select" Text='<%#Eval("cab_name")%>' ToolTip='<%#Eval("cab_desc")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="normal_1">
                    <div id="mcs3_container">
                        <div class="customScrollBox">
                            <div class="container">
                                <div style="height: 350px; overflow: auto;">
                                    <div id="divDrawer" runat="server" class="content">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvCabinet" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:GridView ID="gvDrawer" runat="server" Style="border: none;" ShowHeader="False"
                                                    Width="100%" CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" CaptionAlign="Top"
                                                    EnableModelValidation="True" GridLines="None" Font-Size="12px" Font-Bold="true"
                                                    OnSelectedIndexChanged="gvDrawer_SelectedIndexChanged" DataKeyNames="drw_uuid">
                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <PagerStyle ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#c9c9c9" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left"
                                                        Font-Size="12px" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDrawer" runat="server" Text='<%#Eval("drw_uuid")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name" ShowHeader="False">
                                                            <ItemStyle CssClass="CabinetItem" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" CssClass="CabinetItem_Full_Link" runat="server"
                                                                    CommandName="Select" Text='<%#Eval("drw_name")%>' ToolTip='<%#Eval("drw_desc")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="normal_1">
                    <div id="mcs4_container">
                        <div class="customScrollBox">
                            <div class="container">
                                <div style="height: 350px; overflow: auto;">
                                    <div id="divFolder" runat="server" class="content">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvCabinet" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="gvDrawer" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:GridView ID="gvFolder" runat="server" Style="border: none;" ShowHeader="False"
                                                    Width="100%" CellPadding="4" ForeColor="Black" AutoGenerateColumns="False" CaptionAlign="Top"
                                                    EnableModelValidation="True" GridLines="None" Font-Size="12px" Font-Bold="true"
                                                    OnSelectedIndexChanged="gvFolder_SelectedIndexChanged" DataKeyNames="fld_uuid">
                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <PagerStyle ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#c9c9c9" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left"
                                                        Font-Size="12px" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFolder" runat="server" Text='<%#Eval("fld_uuid")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name" ShowHeader="False">
                                                            <ItemStyle CssClass="CabinetItem" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" CssClass="CabinetItem_Full_Link" runat="server"
                                                                    CommandName="Select" Text='<%#Eval("fld_name")%>' ToolTip='<%#Eval("fld_desc")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="normal_1">
                    <div id="mcs5_container">
                        <div class="customScrollBox">
                            <div class="container">
                                <div style="height: 350px; overflow: auto;">
                                    <div id="divDocument" runat="server" class="content">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="gvCabinet" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="gvDrawer" EventName="SelectedIndexChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="gvFolder" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hfDocument" runat="server" />
                                                <asp:HiddenField ID="hfDocName" runat="server" />
                                                <asp:HiddenField ID="hfDocExt" runat="server" />
                                                <asp:HiddenField ID="hfSelDocStat" runat="server" />
                                                <asp:HiddenField ID="hfCheckedOutBy" runat="server" />
                                                <asp:HiddenField ID="hfCheckedOutByFullName" runat="server" />
                                                <asp:HiddenField ID="hfUserID" runat="server" />
                                                <asp:HiddenField ID="hf_SelDocID" runat="server" />
                                                <asp:GridView ID="gvDocument" runat="server" Style="border: none;" ShowHeader="False"
                                                    Width="100%" CellPadding="4" AutoGenerateColumns="False" CaptionAlign="Top"
                                                    EnableModelValidation="True" GridLines="None" Font-Size="12px" Font-Bold="true"
                                                    OnSelectedIndexChanged="gvDocument_SelectedIndexChanged" OnRowDataBound="gvDocument_RowDataBound">
                                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                    <PagerStyle ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#c9c9c9" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left"
                                                        Font-Size="12px" VerticalAlign="Top" />
                                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDocument" runat="server" Text='<%#Eval("doc_id")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDocumentName" runat="server" Text='<%#Eval("doc_name")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDocStat" runat="server" Text='<%#Eval("doc_stat")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCheckedOutBy" runat="server" Text='<%#Eval("CheckedOutBy")%>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name" ShowHeader="False">
                                                            <ItemStyle CssClass="CabinetItem" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" CssClass="CabinetItem_Full_Link" runat="server" ForeColor='<%# Eval("doc_stat").ToString().Trim()=="Check Out"? System.Drawing.Color.Red:System.Drawing.Color.Black  %>'
                                                                    CommandName="Select" Text='<%#Eval("doc_name")%>' ToolTip='<%#Eval("DocDescSize")%>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="cabinet_bottom">
                &nbsp;</div>
            <div class="main_2">
                <div class="footer_bg">
                    <ul class="footer">
                        <asp:UpdatePanel ID="id1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfMsg" runat="server" />
                                <asp:HiddenField ID="hfMsg1" runat="server" />
                                <asp:HiddenField ID="hfSelDocPermission" runat="server" />
                                <asp:HiddenField ID="hfSelFldPermission" runat="server" />
                                <li>
                                    <asp:Button CssClass="Btn" ID="cmdView" runat="server" Text="View" OnClientClick="javascript: return DocSel4View()" /><%-- OnClick="cmdView_Click"--%>
                                </li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="cmdCheckOut" runat="server" Text="Check Out" OnClick="cmdCheckOut_Click" OnClientClick="javascript: return FetchPermission()" /></li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="btnCheckIn" runat="server" Text="Check In" OnClientClick="javascript: return showCheckInLoading()"
                                        OnClick="btnCheckIn_Click" /></li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="btnCopy" runat="server" Text="Copy" OnClientClick="javascript: return showLoading()"
                                        OnClick="btnCopy_Click" /></li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="btnEmail" runat="server" Text="Email" OnClientClick="javascript: return showMailLoading()"
                                        OnClick="btnEmail_Click" /></li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="btnMove" runat="server" Text="Move"
                                        OnClientClick="javascript: return showMoveLoading()" OnClick="btnMove_Click" /></li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="btnComments" runat="server" Text="WFL Status" OnClientClick="javascript: return showCommentsLoading()"
                                        OnClick="btnComments_Click" /></li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="btnGroup" Visible="false" runat="server" Text="Group"
                                        OnClientClick="javascript: return showGroupLoading()" OnClick="btnGroup_Click" /></li>
                                <asp:HiddenField ID="hfDocURL" Value="as" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <li>
                                    <asp:Button CssClass="Btn" ID="cmdUpload" runat="server" Text="Upload" OnClientClick="javascript: return FetchPermission4Folder()"
                                        OnClick="cmdUpload_Click" /></li>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <li>
                            <button id="copy-button" class="Btn" style="display:none;">
                                Copy Document URL</button></li>
                        <li>
                            <asp:Button CssClass="Btn" ID="cmdWorkflow" runat="server" Text="Workflow"
                                OnClick="cmdWorkflow_Click" /></li>
                        <%--<asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Always">
                            <ContentTemplate>--%>
                                <li class="footer_u_l_nob">
                                    <asp:Button CssClass="Btn" ID="cmdDel" runat="server" Text="Delete" OnClick="cmdDel_Click" /></li>
                            <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </ul>
                </div>
            </div>
            <div class="main_1">
                <div class="user_identy">
                    <div class="user_identy_top">
                    </div>
                    <div class="user_identy_bg">
                        <h2>User Identity</h2>
                        <span class="text">
                            <div class="user_identity_left">
                                Name :</div>
                            <div class="user_identity_right">
                                <asp:Label ID="lblUserName" runat="server"></asp:Label></div>
                        </span>
                        <span class="text">
                            <div class="user_identity_left">
                                Title :</div>
                            <div class="user_identity_right">
                                <asp:Label ID="lblUserTitle" runat="server"></asp:Label></div>
                        </span>
                        <span class="text">
                            <div class="user_identity_left">
                                Dept:</div>
                            <div class="user_identity_right">
                                <asp:Label ID="lblUserDept" runat="server"></asp:Label></div>
                        </span>
                    </div>
                    <div class="user_identy_bottom">
                    </div>
                </div>
                <div class="user_identy1">
                    <div class="user_identy_top">
                    </div>
                    <div class="user_identy_bg">
                        <h2>
                            Document Properties</h2>
                        <div id="divProperty" runat="server" style="min-height:140px;">
                            <%--[ please list here the properties of the list level filing system item selected below, e.g.,Owner, Security, where the user has access right to this item, when last updated, whether OCR exists, etc. ]--%>
                            <span class="text1" style="padding-top: -5px;">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div id="Div1" runat="server">
                                            <div style="width:100%; float:left; margin-bottom:5px; display:none;">
                                                <div style="width:35%; margin-right: 10px; float:left;">
                                                    <label style="font-weight: bold; width: 150px;">
                                                        Doc Type :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lblDocType" runat="server"></asp:Label></div>
                                            </div>
                                            <div style="width:100%; float:left; margin-bottom:5px;">
                                                <div style="width:35%; margin-right: 10px; float:left;">
                                                    <label style="font-weight: bold; width: 150px;">
                                                        Doc Name :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lbDocName" runat="server"></asp:Label></div>
                                            </div>
                                            <div style="width:100%; float:left; margin-bottom:5px;">
                                                <div style="width:35%; margin-right:10px; float:left;">
                                                    <label style="font-weight:bold; width:150px;">
                                                        Path :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lbPath" runat="server"></asp:Label></div>
                                            </div>
                                            <div style="width:100%; float:left; margin-bottom:5px;">
                                                <div style="width:35%; margin-right:10px; float:left;">
                                                    <label style="font-weight:bold; width:150px;">
                                                        Uploaded By :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lblUpldBy" runat="server"></asp:Label></div>
                                            </div>
                                            <div style="width:100%; float:left; margin-bottom:5px;">
                                                <div style="width:35%; margin-right:10px; float:left;">
                                                    <label style="font-weight:bold; width:150px;">
                                                        Uploaded Date :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lblUpldDt" runat="server"></asp:Label></div>
                                            </div>
                                            <div style="width:100%; float:left; margin-bottom:5px;">
                                                <div style="width:35%; margin-right:10px; float:left;">
                                                    <label style="font-weight:bold; width:150px;">
                                                        Document Size :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lblDocSize" runat="server"></asp:Label></div>
                                            </div>
                                            <div style="width:100%; float:left; margin-bottom:5px;">
                                                <div style="width:35%; margin-right:10px; float:left;">
                                                    <label style="font-weight:bold; width:150px;">
                                                        Status :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lblDocStat" runat="server"></asp:Label></div>
                                            </div>
                                            <div style="width:100%; float:left; display:none;">
                                                <div style="width:35%; margin-right:10px; float:left;">
                                                    <label style="font-weight:bold; width:150px;">
                                                        UUID :
                                                    </label>
                                                </div>
                                                <div style="width:50%; float:left;">
                                                    <asp:Label ID="lblDocUUID" runat="server"></asp:Label></div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </span>
                        </div>
                    </div>
                    <div class="user_identy_bottom">
                    </div>
                </div>
                <div class="user_identy1">
                    <div class="user_identy_top">
                    </div>
                    <div class="user_identy_bg">
                        <h2>
                            Quick Search</h2>
                        <span class="text1">
                            <div id="divSearchParam" runat="server" style="min-height:120px;">
                                <div style="width: 100%; float: left; margin-bottom: 3px;">
                                    <label style="float: left; width: 70px; padding-right: 10px;">
                                        Doc Type</label>
                                    <asp:DropDownList ID="ddDocType" CssClass="wf_det_field3" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 100%; float: left;">
                                    <label style="float: left; width: 70px; padding-right: 10px;">
                                        Doc Name</label>
                                    <asp:TextBox ID="txtDocName" runat="server" Text="" CssClass="wf_mas_field2"></asp:TextBox>
                                </div>
                                <div style="width: 100%; float: left; padding-left: 80px; margin-top: 3px;">
                                    <asp:Button ID="cmdSearch" runat="server" Text="Search" OnClick="cmdSearch_Click" />
                                </div>
                                <div style="float: right;">
                                    <a href="search_list.aspx">Advance Search</a>
                                </div>
                            </div>
                        </span>
                    </div>
                    <div class="user_identy_bottom">
                    </div>
                </div>
            </div>
            <%--Copy Popup start--%>
            <div id="dvLoading1" style="position: fixed; display: none; width: 100%; height: 100%;
                background: url(images/ui-bg_diagonals-thick_20_666666_40x40.png); left: 0px;
                top: 0px; opacity: .6; filter: alpha(opacity=60); z-index: 101; font-size: 100px;">
            </div>
            <div id="dvLoading2" style="width: 100%; display: none; height: 100%; position: fixed;
                top: 40px; left: 0px; z-index: 1002;">
                <div style="width: 600px; max-height: 500px; margin: 0 auto; top: 0px; position: relative;">
                    <div style="border: 1px solid #999; float: left; width: 100%; background: #fff; -moz-border-radius: 8px;
                        -webkit-border-radius: 8px; border-radius: 8px;">
                        <div style="border-bottom: 1px solid #999; padding: 0px;" class="normal_common">
                            <div style="padding: 5px; font-size: 16px; color: #660066; font-weight: bold; background: #f0eded;">
                                Document Copy<img alt="" style="display: block; float: right; cursor: pointer;" id="close2"
                                    src="images/close.png" />
                            </div>
                        </div>
                        <div id="def" class="normal_common">
                            <div class="normal_common" id="divDefault" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div id="divTV" runat="server" style="float: left; width: 100%; background-color: #ebe8ec;">
                                            <div style="padding: 5px; font-size: 16px; font-weight: bold; background: #f6f4f6;">
                                                Select the Location for
                                                <asp:Label ID="lblDocNameCopy" Text="" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div style="float: left; width: 100%;" class="normal_feature">
                                            <div style="padding: 0px;" class="normal_common">
                                                <div style="color: #FF0000; text-align: center; font-weight: bold; font-size: 16px;">
                                                    <div style="float: left;">
                                                        <asp:Label ID="MsgNodetCopy" runat="server"></asp:Label></div>
                                                </div>
                                                <div style="font-weight: bold; padding-left: 10px;">
                                                    <div class="normal_common">
                                                        <div style="float: left; width: 100%; display: block; margin-bottom: 10px; margin-top: 10px;">
                                                            <span class="wf_mas_cap">Cabinet</span>
                                                            <asp:DropDownList ID="ddCabinet1" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddCabinet1_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                        <div style="float: left; width: 100%; display: block; margin-bottom: 10px;">
                                                            <span class="wf_mas_cap">Drawer</span>
                                                            <asp:DropDownList ID="ddDrawer1" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddDrawer1_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                        <div style="float: left; width: 100%; display: block; margin-bottom: 10px;">
                                                            <span class="wf_mas_cap">Folder</span>
                                                            <asp:DropDownList ID="ddFolder1" CssClass="wf_det_field2_big" runat="server">
                                                            </asp:DropDownList>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <div style="float: left; width: 100%; margin-left: 50px; margin-bottom: 10px;">
                                                    <asp:Button CssClass="Btn" ID="cmdCopy" runat="server" Text="Copy" OnClick="cmdCopy_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--Copy Popup End--%>
            <%--Check In Popup start--%>
            <div id="divCheckInLoading" style="position: fixed; display: none; width: 100%; height: 100%;
                background: url(images/ui-bg_diagonals-thick_20_666666_40x40.png); left: 0px;
                top: 0px; opacity: .6; filter: alpha(opacity=60); z-index: 101; font-size: 100px;">
            </div>
            <div id="divCheckIn" style="width: 100%; display: none; min-height: 200px; position: fixed;
                top: 40px; left: 0px; z-index: 1002;">
                <div style="width: 600px; min-height: 200px; margin: 0 auto; top: 0px; position: relative;">
                    <div style="border: 1px solid #999; float: left; width: 98%; background: #fff; -moz-border-radius: 8px;
                        -webkit-border-radius: 8px; border-radius: 8px;">
                        <div style="border-bottom: 1px solid #999; padding: 0px;" class="normal_common">
                            <div style="padding: 5px; font-size: 16px; color: #660066; font-weight: bold; background: #f0eded;">
                                Document Check In<img alt="" style="display: block; float: right; cursor: pointer;"
                                    id="CheckInClose" src="images/close.png" />
                            </div>
                        </div>
                        <div id="divCheckInDef" class="normal_common">
                            <div class="normal_common" id="divCheckInDefault" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfSelCheckIn" runat="server" />
                                        <div id="divCheckInOpt" runat="server" style="background-color: #ebe8ec;" class="normal_common">
                                            <div style="padding: 5px; font-size: 16px; font-weight: bold; background: #f6f4f6;">
                                                Select Your Option for
                                                <asp:Label ID="lblDocNameCheckIn" Text="" runat="server"></asp:Label>
                                            </div>
                                            <div>
                                                <div style="width: 90%; float: left; height: 80px; background: #fff; padding-top: 20px;
                                                    padding-left: 10px;">
                                                    <div style="float: left; width: 90%; margin-bottom: 10px;">
                                                        <asp:RadioButton ID="OptExist" Text="Want to Checked In the Existing One" runat="server"
                                                            GroupName="GrpOpt" onClick="CheckedOpt(this.value)" />
                                                    </div>
                                                    <div style="float: left; width: 90%; margin-bottom: 10px;">
                                                        <asp:RadioButton ID="OptNew" Text="Want to Checked In with a Revised Version" runat="server"
                                                            GroupName="GrpOpt" onClick="CheckedOpt(this.value)" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="normal_feature">
                                            <div style="color: #FF0000; text-align: center; background: #000; width: 100%; height: 30px;
                                                line-height: 30px; float: left; font-weight: bold; font-size: 16px; border: 1px solid #999999;">
                                                <asp:Label ID="MsgNodetCheckIn" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="normal_common">
                                            <div style="float: left; width: 30%; height: 30px; padding-left: 10px; line-height: 30px;">
                                                <asp:Button CssClass="Btn" ID="cmdCheckIn" runat="server" Text="Check In" OnClick="cmdCheckIn_Click" OnClientClick="javascript: return CloseCheckInWindow();" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--Check In Popup End--%>
            
            <%--Mail Popup start--%>
            <div id="divMailLoading" style="position: fixed; display: none; width: 100%; height: 100%;
                background: url(images/ui-bg_diagonals-thick_20_666666_40x40.png); left: 0px;
                top: 0px; opacity: .6; filter: alpha(opacity=60); z-index: 101; font-size: 100px;">
            </div>
            <div id="divMail" style="width: 100%; display: none; min-height: 200px; position: fixed;
                top: 40px; left: 0px; z-index: 1002;">
                <div style="width: 600px; margin: 0 auto; top: 0px; position: relative;">
                    <div style="border: 1px solid #999; float: left; width: 98%; background: #fff; -moz-border-radius: 8px;
                        -webkit-border-radius: 8px; border-radius: 8px;">
                        <div style="border-bottom: 1px solid #999; padding: 0px;" class="normal_common">
                            <div style="padding: 5px; font-size: 16px; color: #660066; font-weight: bold; background: #f0eded;">
                                Document Mail<img alt="" style="display: block; float: right; cursor: pointer;" id="MailClose"
                                    src="images/close.png" />
                            </div>
                        </div>
                        <div id="divMailDef" class="normal_common">
                            <div style="min-height: 200px;">
                                <div class="normal_common" id="divMailDefault" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel101" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hfSelMail" runat="server" />
                                            <div id="divMailOpt" runat="server" style="background-color: #ebe8ec;" class="normal_common">
                                                <div style="padding: 5px; font-size: 16px; font-weight: bold; background: #f6f4f6;">
                                                    Enter Details for sending
                                                    <asp:Label ID="lblDocNameMail" Text="" runat="server"></asp:Label>
                                                </div>
                                                <div class="normal_common">
                                                    <div style="color: #FF0000; text-align: center; font-weight: bold; font-size: 16px;
                                                        background: #000000; border: 1px solid #999999; padding: 10px;">
                                                        <asp:Label ID="MsgNodetMail" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div class="normal_common">
                                                    <div style="width: 100%; float: left; padding-top: 10px;">
                                                        <div style="width: 100%; float: left; padding-top: 20px; padding-left: 10px;">
                                                            <span class="wf_mas_cap">Select User</span>
                                                            <asp:DropDownList ID="ddUser" CssClass="wf_det_field3_big" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:Button CssClass="Btn" ID="cmdAdd" runat="server" Text="Add" OnClientClick="javascript: AddMail()" />
                                                        </div>
                                                        <div style="width: 100%; float: left; padding-top: 5px; padding-left: 10px;">
                                                            <span class="wf_mas_cap">To</span>
                                                            <asp:TextBox ID="txtToMail" runat="server" CssClass="wf_mas_field4"></asp:TextBox>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                        <div style="width: 100%; float: left; padding-top: 5px; padding-left: 10px;">
                                                            <span class="wf_mas_cap">Subject</span>
                                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="wf_mas_field4"></asp:TextBox>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                        <div style="width: 100%; float: left; padding-top: 5px; padding-left: 10px;">
                                                            <span class="wf_mas_cap">Attachment</span>
                                                            <asp:Label ID="lblDocNameAttach" Text="" runat="server"></asp:Label>
                                                        </div>
                                                        <div style="width: 100%; float: left; padding-top: 5px; padding-left: 10px;">
                                                            <span class="wf_mas_cap">Message</span>
                                                            <asp:TextBox ID="txtMsg" runat="server" CssClass="wf_mas_field_m2" TextMode="MultiLine"></asp:TextBox>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <div style="float: left; padding-top: 10px; padding-bottom: 10px; margin-left: 160px;">
                                                    <asp:HiddenField ID="hfEmailStat" runat="server" />
                                                    <asp:Button CssClass="Btn" ID="cmdEmail" runat="server" Text="Send" OnClick="cmdEmail_Click" OnClientClick="javascript: return ChkEmail();" />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--Mail Popup End--%>
            <%--Group Popup start--%>
            <div id="divGroupLoading" style="position: fixed; display: none; width: 100%; height: 100%;
                background: url(images/ui-bg_diagonals-thick_20_666666_40x40.png); left: 0px;
                top: 0px; opacity: .6; filter: alpha(opacity=60); z-index: 101; font-size: 100px;">
            </div>
            <div id="divGroup" style="width: 100%; display: none; min-height: 200px; position: fixed;
                top: 40px; left: 0px; z-index: 1002;">
                <div style="width: 600px; margin: 0 auto; top: 0px; position: relative;">
                    <div style="border: 1px solid #999; float: left; width: 98%; background: #fff; -moz-border-radius: 8px;
                        -webkit-border-radius: 8px; border-radius: 8px;">
                        <div style="border-bottom: 1px solid #999; padding: 0px;" class="normal_common">
                            <div style="padding: 5px; font-size: 16px; color: #660066; font-weight: bold; background: #f0eded;">
                                Document Group Tagging<img alt="" style="display: block; float: right; cursor: pointer;"
                                    id="GroupClose" src="images/close.png" />
                            </div>
                        </div>
                        <div id="divGroupDef" class="normal_common">
                            <div style="min-height: 200px;">
                                <div class="normal_common" id="divGroupDefault" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hfSelGrpID" runat="server" />
                                            <div id="divGroupGV" runat="server" style="background-color: #ebe8ec;" class="normal_common">
                                                <div style="padding: 5px; font-size: 16px; font-weight: bold; background: #f6f4f6;">
                                                    Select Group for
                                                    <asp:Label ID="lblDocName" Text="" runat="server"></asp:Label>
                                                </div>
                                                <div>
                                                    <div style="width: 100%; float: left; padding-top: 20px;">
                                                        <asp:GridView ID="gvGroup" runat="server" Style="border: 1px solid #000;" Width="100%"
                                                            BackColor="White" CellPadding="4" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False"
                                                            OnSelectedIndexChanged="gvGroup_SelectedIndexChanged" CaptionAlign="Top" OnRowDataBound="gvGroup_RowDataBound"
                                                            EnableModelValidation="True">
                                                            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                            <FooterStyle BackColor="#CCCC99" />
                                                            <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                            <HeaderStyle BackColor="#8b8b8b" Font-Bold="True" HorizontalAlign="Center" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="White" />
                                                            <Columns>
                                                                <asp:TemplateField Visible="False">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbGrpid" runat="server" Text='<%#Eval("grp_id")%>'>
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Group Name" HeaderStyle-Width="100%" ItemStyle-Width="100%">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkGrp" CommandName="Select" CommandArgument="SelGrp" runat="server"
                                                                            Text='<%#Eval("grp_name")%>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                                                    <ItemStyle Width="100%" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="normal_feature">
                                                <div style="color: #FF0000; text-align: center; font-weight: bold; font-size: 16px;
                                                    background: #000000; border: 1px solid #999999; padding: 10px;">
                                                    <asp:Label ID="MsgNodet" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <div style="float: left; width: 100%; margin-left: 50px; margin-top: 50px;">
                                                    <asp:Button CssClass="Btn" ID="cmdGroup" runat="server" Text="Tag" OnClick="cmdGroup_Click" />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--Group Popup End--%>
            <%--Move Popup start--%>
            <div id="divMoveLoading" style="position: fixed; display: none; width: 100%; height: 100%;
                background: url(images/ui-bg_diagonals-thick_20_666666_40x40.png); left: 0px;
                top: 0px; opacity: .6; filter: alpha(opacity=60); z-index: 101; font-size: 100px;">
            </div>
            <div id="divMove" style="width: 100%; display: none; height: 100%; position: fixed;
                top: 40px; left: 0px; z-index: 1002;">
                <div style="width: 600px; max-height: 500px; margin: 0 auto; top: 0px; position: relative;">
                    <div style="border: 1px solid #999; float: left; width: 100%; background: #fff; -moz-border-radius: 8px;
                        -webkit-border-radius: 8px; border-radius: 8px;">
                        <div style="border-bottom: 1px solid #999; padding: 0px;" class="normal_common">
                            <div style="padding: 5px; font-size: 16px; color: #660066; font-weight: bold; background: #f0eded;">
                                Document Move<img alt="" style="display: block; float: right; cursor: pointer;" id="MoveClose"
                                    src="images/close.png" />
                            </div>
                        </div>
                        <div id="divMoveDef" class="normal_common">
                            <div class="normal_common" id="divMoveDefault" runat="server">
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div id="divMoveTV" runat="server" style="float: left; width: 100%; background-color: #ebe8ec;">
                                            <div style="padding: 5px; font-size: 16px; font-weight: bold; background: #f6f4f6;">
                                                Select the Location for
                                                <asp:Label ID="lblDocNameMove" Text="" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div style="float: left; width: 100%;" class="normal_feature">
                                            <div style="padding: 0px;" class="normal_common">
                                                <div style="color: #FF0000; text-align: center; font-weight: bold; font-size: 16px;">
                                                    <div style="float: left;">
                                                        <asp:Label ID="Label4" runat="server"></asp:Label></div>
                                                </div>
                                                <div style="font-weight: bold; padding-left: 10px;">
                                                    <div class="normal_common">
                                                        <div style="display: block; float: left; width: 100%; margin-bottom: 10px; margin-top: 10px;">
                                                            <span class="wf_mas_cap">Cabinet</span>
                                                            <asp:DropDownList ID="ddCabinet2" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddCabinet2_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                        <div style="display: block; float: left; width: 100%; margin-bottom: 10px;">
                                                            <span class="wf_mas_cap">Drawer</span>
                                                            <asp:DropDownList ID="ddDrawer2" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddDrawer2_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                        <div style="display: block; float: left; width: 100%; margin-bottom: 10px;">
                                                            <span class="wf_mas_cap">Folder</span>
                                                            <asp:DropDownList ID="ddFolder2" CssClass="wf_det_field2_big" runat="server">
                                                            </asp:DropDownList>
                                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="normal_common">
                                                <div style="float: left; width: 100%; margin-left: 50px; margin-bottom: 10px;">
                                                    <asp:Button CssClass="Btn" ID="cmdMove" runat="server" Text="Move" OnClick="cmdMove_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--Move Popup End--%>
        </div>
    </div>
    <div class="footer_main">
        <div class="footer_text">
            Powered by ADA modeling technology</div>
    </div>


    <%--Comments Popup start--%>
            <div id="dvCommentsLoading1" ></div>
            <div id="dvCommentsLoading2" >
            <div class="lightbox_1">
            <div class="lightbox_2">
            <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Always">
            <ContentTemplate>
            <div class="lightbox_3">
            <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
            Electronic Signatures / Comments
            </div>
            <div class="normal_feature">
            <div style="width:100%; float:left; padding-top:20px;">
            <div style="overflow:auto; height:200px;">
            <asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gvComment" runat="server">
            <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black"  />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"  />
            <AlternatingRowStyle BackColor="White"  />
            </asp:GridView>
            </div>
            </div>
            <div class="normal_feature">
            <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px;  border-bottom:1px solid #666; padding:10px;">
            <asp:Label ID="MsgNodetComments" runat="server"></asp:Label> 
            </div>
            </div>
            <div  class="normal_feature" style="text-align:right;">                         
            <input type="button" id="ImageButton1" class="padd_marg1" onclick="javascript: return hideCommentsLoading();" />
            </div>
            </ContentTemplate>
            </asp:UpdatePanel> 
            </div>
                       
            </div>
            </div>
            <%--Comments Popup End--%>
    </form>
</body>
</html>
