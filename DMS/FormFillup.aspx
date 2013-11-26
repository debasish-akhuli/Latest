<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormFillup.aspx.cs" Inherits="DMS.FormFillup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>myDOCbase</title>
    <link  href="common.css" rel="stylesheet" type="text/css" media="all" />
    <link href="editable.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript" src="js/jquary_min.js"></script>
    <style type="text/css" >
    *{ padding:0px; margin:0px;}
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
        });
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
        });

        function PopHF() {
            document.getElementById('<%= hfDocument.ClientID %>').value = document.getElementById("divNewVDocName").innerHTML;
        }
        function OpenAttachedDoc() {
            popupWindow("TempDownload//" + document.getElementById("<%= hfDocument.ClientID %>").value);
        }
        function OpenUpdtDoc() {
            popupWindow("TempDownload//" + document.getElementById("<%= hfUpdtDoc.ClientID %>").value);
        }
        function OpenDoc() {
            if (document.getElementById("<%= hfPageControl.ClientID %>").value == "F") {
                popupWindow("TempDownload//" + document.getElementById("<%= hfDocument.ClientID %>").value);
            }
            else if (document.getElementById("<%= hfPageControl.ClientID %>").value == "FE") {
                popupWindow("TempDownload//" + document.getElementById("<%= hfDocument.ClientID %>").value);
            }
            else if (document.getElementById("<%= hfPageControl.ClientID %>").value == "O") {
                popupWindow("TempDownload//" + document.getElementById("<%= hfDocument.ClientID %>").value);
            }
        }
        function popupWindow(url) {
            var width = 999;
            var height = 580;
            var left = (screen.width - width) / 2;
            var top = (screen.height - height) / 2;
            var params = 'width=' + width + ', height=' + height;
            params += ', top=' + top + ', left=' + left;
            params += ', directories=no';
            params += ', location=no';
            params += ', menubar=no';
            params += ', resizable=yes';
            params += ', scrollbars=no';
            params += ', status=no';
            params += ', toolbar=no';
            params += ', modal=yes';
            sList = window.open(url, 'windowname5', params);
            if (window.focus) { sList.focus() }
            return false;
        }
        function StartFunc() {
            document.getElementById("cmdProceed").style.display = 'none';
            document.getElementById("cmdDashboard").style.display = 'none';
            startTimer();
            OpenDoc();
        }
        window.onload = StartFunc;
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
    <div class="main">
      <div class="logo_bg"><%--<a href="#"><img src="images/logo.jpg" width="286" height="88" alt="" border="0" /></a>--%></div>
      <h1><a href="userhome.aspx" style="border:none; text-decoration:none; outline:none;"><img src="images/product_logo.png" border="0" alt="" /></a></h1>
    </div>
    </div>
    <div>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--For Session Expired Start--%>
    <div id="ExtendSessionLifeBox2" style="width:100%; z-index:10000; height:100%; opacity:.2; filter:alpha(opacity=20); background:#660066; display:none;">
    </div>
    <asp:Panel ID="ExtendSessionLifeBox" runat="server" style="height:100px; z-index:10001; width:250px; background-color:#660066; padding:20px; color:#fff; border:solid 1px #666; display:none; position:absolute;">
   <div style="width:100%; float:left">
  <asp:Label ID="ExtendSessionLifePrompt" runat="server" Text="Your session is going to expire in 10 minutes. Would you like to extend your Session for another 60 minutes?"></asp:Label>
  </div>
  <div style="width:100%; float:left">
  <div id="countDown"></div>
  </div>
  <div style="width:100%; float:left; padding-top:20px;">
  <div style="float:left; padding-right:20px;">
  <asp:UpdatePanel ID="up1" runat="server" >
  <ContentTemplate>
  <asp:Button ID="ExtendSessionLife" runat="server" Text="Yes" OnClientClick="startTimer(); return true;"/>
  </ContentTemplate>
  </asp:UpdatePanel>
  </div>
  <div style="float:left">
  <input type="button" id="CancelExtendSessionLife" value="No" onclick="CloseExtendSessionLifeBox(); return false;" />  
  </div>
  </div>
  
</asp:Panel>
<%--For Session Expired End--%>
<div class="main_body">
        <div class="main">
            <div class="normal_common">
                <div class="wf_mas_Heading"><span>Welcome, </span><asp:Label ID="lblUser" runat="server" Text="User Name"></asp:Label>
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
                                        <li><a href="eFormCreation.aspx">New eForm Creation</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
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
                                        <li><a href="eFormCreation.aspx">New eForm Creation</a></li>
                                        <li><a class="nob" href="doc_mast.aspx">New Document Upload</a></li>
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
                                    <li><a href="eFormCreation.aspx">New eForm Creation</a></li>
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
            <div style="padding-bottom:0px;" class="normal_common">
                <div class="wf_mas_Heading2">
                    Document View
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <asp:HiddenField ID="hfDocument" runat="server" />
                        <asp:HiddenField ID="hfUpdtDoc" runat="server" />
                        <asp:HiddenField ID="hfPageControl" runat="server" />
                        <div id="divNewVDocName" style="display:none;"></div>
                        
                        
                        
                        <div id="divFreshWF" runat="server">
                            
                                <div style="float:left; margin-left:300px; margin-top:50px; border-radius:5px; width:300px; -webkit-border-radius:5px; -moz-border-radius:5px; -khtml-border-radius:5px; background:#cccccc; padding:5px;">
                                    <div style="float:left; margin-bottom:10px;">
                                        <div>
                                    <h2>Document Properties</h2>
                                    <div id="divProperty" runat="server">
                                        <span class="text1" style="padding-top:-5px; ">
                                            <div id="Div1" runat="server">
                                                <div style=" float:left; margin-bottom:5px; width:300px;">
                                                    <div style=" margin-right:10px; float:left;"><label style=" font-weight:bold;">Doc Type : </label>  </div>                              
                                                    <div style="float:left;"><asp:Label ID="lblDocType" runat="server"></asp:Label></div>
                                                </div>
                                                <div style="float:left; margin-bottom:5px; width:300px;">
                                                    <div style="margin-right:10px; float:left;"><label style=" font-weight:bold; ">Uploaded By : </label>  </div>
                                                    <div style="float:left;"><asp:Label ID="lblUpldBy" runat="server"></asp:Label></div>
                                                </div>
                                                <div style=" float:left; margin-bottom:5px; width:300px;">
                                                    <div style="margin-right:10px; float:left;"><label style=" font-weight:bold; ">Uploaded Date : </label>  </div>
                                                    <div style="float:left;"><asp:Label ID="lblUpldDt" runat="server"></asp:Label></div>
                                                </div>
                                                <div style="float:left; width:300px;">
                                                    <div style="margin-right:10px; float:left;"><label style=" font-weight:bold; ">Status : </label> </div> 
                                                    <div style="float:left;"><asp:Label ID="lblDocStat" runat="server"></asp:Label></div>
                                                </div>
                                            </div>
                                        </span>
                                    </div>
                                </div>
                                    </div>
                                    <div style="float:left; margin-left:20px;">
                                        <asp:Button CssClass="TaskUpdtBtn1" ID="cmdProceed" runat="server" Text="Proceed" onclick="cmdProceed_Click" OnClientClick="javascript: PopHF();" />
                                        <asp:Button CssClass="TaskUpdtBtn1" ID="cmdDashboard" runat="server" Text="Go to my Dashboard" onclick="cmdDashboard_Click" />
                                        <asp:Button CssClass="TaskUpdtBtn1" ID="cmdBackSearch" runat="server" Text="Back" onclick="cmdBackSearch_Click" />
                                    </div>
                                </div>                          
                            <div>
                               
                            </div>
                        </div>


                        <div id="divRunningWF" runat="server" style="width:100%; float:left;">
                            <div style="background:#FFF; -moz-border-radius:5px; width:100%; float:left; -webkit-border-radius:5px; -moz-border-radius:5px; padding-bottom:10px; padding-right:5px; ">
                                <div id="divMetaTag" runat="server" style="background:#fff; width:100%; float:left; padding-left:5px; padding-right:5px; margin-top:5px; ">
                                 <div class="normal_common_g1_edit" style="float:right;">
                                        <asp:Button CssClass="TaskUpdtBtn1" ID="cmdAttachDoc" runat="server" Text="Attached Doc" OnClientClick="javascript: OpenAttachedDoc();" />
                                </div>
                                <hr style="float:left; width:99%; border:1px solid #cccccc;" />
                                <div style="margin-bottom:10px; padding-top:5px; float:left; color:#023773; font-size:14px; font-weight:bold; width:100%;">Meta Data Details</div>
                                <div style="float:left; width:100%;">
                                <div style="width:56%; float:left;">
                                    <div class="normal_common_g1" id="divTag1" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag1" Text="Tag1" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag1" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    <div class="normal_common_g1" id="divTag3" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag3" Text="Tag3" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag3" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    <div class="normal_common_g1" id="divTag5" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag5" Text="Tag5" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag5" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                     <div class="normal_common_g1" id="divTag7" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag7" Text="Tag7" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag7" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    <div class="normal_common_g1" id="divTag9" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag9" Text="Tag9" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag9" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    
                                </div>

                                
                                <div style="width:44%; float:right;">
                                    <div class="normal_common_g1" id="divTag2" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag2" Text="Tag2" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag2" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    <div class="normal_common_g1" id="divTag4" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag4" Text="Tag4" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag4" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    <div class="normal_common_g1" id="divTag6" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag6" Text="Tag6" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag6" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    <div class="normal_common_g1" id="divTag8" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag8" Text="Tag8" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag8" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                    <div class="normal_common_g1" id="divTag10" runat="server">
                                        <span class="wf_mas_cap"><asp:Label ID="lblTag10" Text="Tag10" runat="server"></asp:Label></span>
                                        <asp:TextBox ID="txtTag10" runat="server" CssClass="wf_mas_field_g1"></asp:TextBox>
                                    </div>
                                </div>
                                
                                </div>
                               
                                
                                <div style="float:right; margin-top:10px;">
                                
                                <div class="normal_common_g1_edit">
                                    <asp:Button ID="cmdUpdtMetaData" runat="server" Text="Update Metadata" onclick="cmdUpdtMetaData_Click" CssClass="TaskUpdtBtn1" />
                                </div>
                                
                                    
                                
                                
                             
                                </div>
                                </div>








                                <div id="divWFTaskUpdt" runat="server" style="background:#dbdbdb; width:97%; float:left; padding:10px; margin:5px;">
                                
                                    <div style="width:47%; float:right; background:#ffffff; border-radius:5px; -moz-border-radius:5px; -khtml-border-radius:5px; -webkit-border-radius:5px; padding:5px;">
                                        <div style="margin-bottom:10px; width:100%; float:left; color:#023773; font-size:14px; font-weight:bold; border-bottom:1px solid #cccccc; line-height:30px;">Update Your Assigned Task</div>
                                        <div style="width:100%; float:left; display:none;">
                                            <span class="TaskUpdtLbl">Workflow ID</span>
                                            <asp:Label ID="lblWFID" CssClass="LeftLabel" runat="server"></asp:Label>
                                            <span style="float:right; padding-right:20px;" class="wf_det_add_holder"></span>
                                        </div>
                                        <div style="width:100%; float:left;">
                                            <span class="TaskUpdtLbl">Workflow Name</span>
                                            <asp:Label ID="lblWFName" runat="server"></asp:Label>
                                        </div>
                                        <div style="width:100%; float:left;">
                                            <span class="TaskUpdtLbl">Stage</span>
                                            <asp:Label ID="lblStage" runat="server"></asp:Label>
                                        </div>
                                        <div style="width:100%; float:left; display:none;">
                                            <span class="TaskUpdtLbl">Document</span>
                                            <asp:Label ID="lblDocName" CssClass="LeftLabel" runat="server"></asp:Label>
                                        </div>
                                        <div style="width:100%; float:left;">
                                            <span class="TaskUpdtLbl">Task</span>
                                            <asp:DropDownList ID="ddTask" CssClass="wf_det_field2_small" runat="server"></asp:DropDownList>
                                        </div>
                                        <div style="width:100%; float:left;">
                                            <span class="TaskUpdtLbl">Queued Date</span>
                                            <asp:Label ID="lblAssignedDt" runat="server"></asp:Label>
                                        </div>
                                        <div style="width:100%; float:left;">
                                            <span class="TaskUpdtLbl">Due Date</span>
                                            <asp:Label ID="lblDueDt" runat="server"></asp:Label>
                                        </div>
                                        <div style="width:100%; float:left;">
                                            <span class="TaskUpdtLbl">Queued By</span>
                                            <asp:Label ID="lblAssignedBy" runat="server"></asp:Label>
                                        </div>
                                        <div style="width:100%; float:left;">
                                            <span class="TaskUpdtLbl">Comments</span>
                                            <asp:TextBox ID="txtComments" runat="server" CssClass="TaskUpdtMF"></asp:TextBox>
                                        </div>
                                        <div style="width:100%; margin-top:10px; margin-bottom:5px; float:left; margin-left:105px; ">
                                        <asp:Button ID="cmdUpdate" runat="server" CssClass="TaskUpdtBtn" Text="Update" onclick="cmdUpdate_Click" />
                                    </div>

                                    </div>
                                    <div style="width:47%; float:left; padding-top:5px;">
                                        <div style="margin-bottom:10px; width:100%; padding-top:5px; float:left; color:#023773; font-size:14px; font-weight:bold;">Append Extra File</div>
                                        <span class="TaskUpdtLbl">Append from </span>
                                        <asp:RadioButton ID="optFiling" Text="e-Filing System" runat="server" GroupName="grpAppend" Checked="true" AutoPostBack="true" oncheckedchanged="optFiling_CheckedChanged" />
                                        <asp:RadioButton ID="optLocal" Text="Local Drive" runat="server" GroupName="grpAppend" AutoPostBack="true" oncheckedchanged="optLocal_CheckedChanged" />
                                        
                                        <div id="divFiling" runat="server" style="background:#ccc;">                                        
                                            <div style="float:left; width:100%; padding-bottom:5px;">
                                                <span class="wf_mas_small">Cabinet</span>
                                                <asp:DropDownList ID="ddCabinet" CssClass="wf_det_field2_small" runat="server" AutoPostBack="true" onselectedindexchanged="ddCabinet_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div style="float:left; width:100%; padding-bottom:5px;">
                                                <span class="wf_mas_small">Drawer</span>
                                                <asp:DropDownList ID="ddDrawer" CssClass="wf_det_field2_small" runat="server" AutoPostBack="true" onselectedindexchanged="ddDrawer_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div style="float:left; width:100%; padding-bottom:5px;">
                                                <span class="wf_mas_small">Folder</span>
                                                <asp:DropDownList ID="ddFolder" CssClass="wf_det_field2_small" runat="server" AutoPostBack="true" onselectedindexchanged="ddFolder_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div style="float:left; width:100%;">
                                                <span class="wf_mas_small">Document</span>
                                                <asp:DropDownList ID="ddDocument" CssClass="wf_det_field2_small" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div id="divLocal" runat="server" style="float:left; width:100%; padding-bottom:5px;" >
                                        <span class="wf_mas_small" style="float:left; width:140px;">Select the source file</span>
                                            <span style="float:left;"><asp:FileUpload ID="fuBrowse" runat="server" /></span>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hfMsg" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div style="float:left; width:100%;">
                                        <div class="normal_common_g1_edit1">
                                            <asp:Button ID="cmdAppend" runat="server" CssClass="TaskUpdtBtn1" Text="Append" onclick="cmdAppend_Click" />
                                        </div>
                                       <div class="normal_common_g1_edit1">
                                        <asp:Button CssClass="TaskUpdtBtn1" ID="cmdUpdtDoc" runat="server" Text="View Updated Doc" OnClientClick="javascript: OpenUpdtDoc();" />
                                        </div>
                                        </div>
                                    </div>
                                    
                                </div>





                            </div>
                            </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    </form>
</body>
</html>