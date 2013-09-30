<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true" CodeBehind="FormFill.aspx.cs" Inherits="DMS.FormFill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="js/jquary_min.js"></script>
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

            $("#CheckInClose").click(function () {
                $('#divCheckInDef').show();
                $('#divCheckInLoading').hide();
                $('#divCheckIn').hide();
            });
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
            else if (document.getElementById("<%= hfPageControl.ClientID %>").value == "R") {
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
        function OpenRedirect() {
            document.getElementById("divRedirectWF").style.display = "block";
            document.getElementById("divNormalWF").style.display = "none";
        }

        function showCheckInLoading() {
            document.getElementById("divCheckInLoading").style.display = 'block';
            document.getElementById("divCheckIn").style.display = 'block';
            document.getElementById("divBrowseNewVer").style.display = 'block';
            document.getElementById("<%= hfSelCheckIn.ClientID %>").value = "OptNew";
            return false;
        }
        function hideCheckInLoading() {
            document.getElementById("divCheckInLoading").style.display = 'none';
            document.getElementById("divCheckIn").style.display = 'none';
            return true;
        }
        function CheckedOpt(SelOpt) {
            document.getElementById("<%= hfSelCheckIn.ClientID %>").value = SelOpt.toString();
            if (SelOpt == "OptNew") {
                document.getElementById("divBrowseNewVer").style.display = 'block';
            }
            else if (SelOpt == "OptExist") {
                document.getElementById("divBrowseNewVer").style.display = 'none';
            }
        }
//        window.onload = OpenDoc;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                                    <div style="float:left; margin-left:110px;"><asp:Button CssClass="TaskUpdtBtn1" ID="cmdProceed" runat="server" Text="Proceed" onclick="cmdProceed_Click" OnClientClick="javascript: PopHF();" /></div>
                                </div>
                            <div>
                               
                            </div>
                        </div>


                        <div id="divRunningWF" runat="server" style="width:100%; float:left;">
                            <div style="background:#FFF; -moz-border-radius:5px; width:100%; float:left; -webkit-border-radius:5px; -moz-border-radius:5px; padding-bottom:10px; padding-right:5px; ">
                                <div id="divMetaTag" runat="server" style="background:#fff; width:100%; float:left; padding-left:5px; padding-right:5px; margin-top:5px; ">
                                 <div style="float:left; font-weight:bold; color:#2a59e2;">
                                    <span>Attached Document: </span><asp:Label ID="lblAttachedDocName" runat="server" Text=""></asp:Label>
                                 </div>
                                 <div style="float:right; width:33%;">
                                    <span id="spanCheckOut" runat="server" style="float:left; display:block; margin-right:10px;"><asp:Button CssClass="TaskUpdtBtn1" ID="cmdCheckOut" runat="server" Text="Check Out" OnClick="cmdCheckOut_Click"/></span>
                                    <span id="spanCheckIn" runat="server" style="float:left; display:block; margin-right:10px;"><asp:Button CssClass="TaskUpdtBtn1" ID="cmdCheckIn" runat="server" Text="Check In" OnClientClick="javascript: return showCheckInLoading();" /></span>
                                    <asp:Button CssClass="TaskUpdtBtn1" ID="cmdAttachDoc" runat="server" Text="View Document" OnClientClick="javascript: OpenAttachedDoc();" />
                                </div>
                                <hr style="float:left; width:99%; border:1px solid #cccccc;" />
                                <div style="margin-bottom:10px; display:none; padding-top:5px; float:left; color:#023773; font-size:14px; font-weight:bold; width:100%;">Meta Data Details</div>
                                <div style="float:left; width:100%; display:none;">
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
                               
                                
                                <div style="float:right; margin-top:10px; display:none;">
                                
                                <div class="normal_common_g1_edit">
                                    <asp:Button ID="cmdUpdtMetaData" runat="server" Text="Update Metadata" onclick="cmdUpdtMetaData_Click" CssClass="TaskUpdtBtn1" />
                                </div>
                                
                                    
                                
                                
                             
                                </div>
                                </div>








                                <div id="divWFTaskUpdt" runat="server" style="background:#dbdbdb; width:97%; float:left; padding:10px; margin:5px;">
                                    <%--Left Side--%>
                                    <div style="width:50%; float:left; background:#ffffff; border-radius:5px; -moz-border-radius:5px; -khtml-border-radius:5px; -webkit-border-radius:5px; padding:5px;">
                                        <div style="margin-bottom:10px; width:100%; float:left; color:#023773; font-size:14px; font-weight:bold; border-bottom:1px solid #cccccc; line-height:30px;">Electronic Signatures / Comments</div>
                                        <div style="width:100%; float:left; padding-top:5px;">
                                            <div style="overflow:auto; height:200px;">
                                                <asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gvComment" runat="server">
                                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                                    <FooterStyle BackColor="#CCCC99" />
                                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
                                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                    <%--Right Side--%>
                                    <div style="width:47%; float:right; background:#ffffff; border-radius:5px; -moz-border-radius:5px; -khtml-border-radius:5px; -webkit-border-radius:5px; padding:5px;">
                                        <div style="margin-bottom:10px; width:100%; float:left; color:#023773; font-size:14px; font-weight:bold; border-bottom:1px solid #cccccc; line-height:30px;">Update Your Assigned Task</div>
                                        <div style="width:100%; float:left; display:none;">
                                            <span class="TaskUpdtLbl">Workflow ID</span>
                                            <asp:Label ID="lblWFID" CssClass="LeftLabel" runat="server"></asp:Label>
                                            <span style="float:right; padding-right:20px;" class="wf_det_add_holder"></span>
                                        </div>
                                        <div id="divBrowseNewVer" style="width:100%; float:left; display:none;">
                                            <span class="TaskUpdtLbl">New Document</span>
                                            <asp:FileUpload ID="btnBrowseNewVer" runat="server" CssClass="fieldBrowse" />
                                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                        </div>
                                        <div id="divNormalWF">
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
                                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
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
                                                <span style="float:left; margin-left:20px;">
                                                    <input type="button" id="cmdRedirect" class="wf_mas_add" onclick="javascript: OpenRedirect();" value="Redirect" />
                                                    <%--<asp:Button ID="cmdRedirect" runat="server" CssClass="wf_mas_add" Text="Redirect" OnClientClick="javascript: OpenRedirect();" />--%>
                                                </span>
                                            </div>
                                        </div>
                                        <div id="divRedirectWF" style="display:none;">
                                            <div style="width:100%; float:left;">
                                                <span class="TaskUpdtLbl">Workflow</span>
                                                <asp:DropDownList ID="ddWFName" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                                            </div>
                                            <div style="width:100%; margin-top:10px; margin-bottom:5px; float:left; margin-left:105px; ">
                                                <asp:Button ID="cmdStart" runat="server" CssClass="TaskUpdtBtn" Text="Start" onclick="cmdStart_Click" />
                                            </div>
                                        </div>

                                    </div>






                                    <div style="width:47%; float:left; padding-top:5px; display:none;">
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

    <%--Check In Popup start--%>
    <div id="divCheckInLoading" style="position:fixed; display:none; width:100%; height:100%; background:url(images/ui-bg_diagonals-thick_20_666666_40x40.png); left:0px; top:0px; opacity:.6; filter:alpha(opacity=60); z-index:101; font-size:100px;">
    </div>
    <div id="divCheckIn" style="width:100%; display:none; min-height:200px; position:fixed; top:40px; left:0px; z-index:1002;">
        <div style="width:600px; min-height:200px; margin:0 auto; top:0px; position:relative;">
            <div style="border:1px solid #999; float:left; width:98%; background:#fff; -moz-border-radius:8px; -webkit-border-radius:8px; border-radius:8px;">
                <div style="border-bottom:1px solid #999; padding:0px;" class="normal_common">
                    <div style="padding:5px; font-size:16px; color:#660066; font-weight:bold; background:#f0eded;">
                        Document Check In<img alt="" style="display:block; float:right; cursor:pointer;"
                            id="CheckInClose" src="images/close.png" />
                    </div>
                </div>
                <div id="divCheckInDef" class="normal_common">
                    <div class="normal_common" id="divCheckInDefault" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfSelCheckIn" runat="server" />
                                <div id="divCheckInOpt" runat="server" style="background-color: #ebe8ec;" class="normal_common">
                                    <div style="padding:5px; font-size:16px; font-weight:bold; background:#f6f4f6;">
                                        Select Your Option for
                                        <asp:Label ID="lblDocNameCheckIn" Text="" runat="server"></asp:Label>
                                    </div>
                                    <div>
                                        <div style="width:90%; float:left; height:80px; background:#fff; padding-top:20px; padding-left:10px;">
                                            <div style="float:left; width:90%; margin-bottom:10px;">
                                                <asp:RadioButton ID="OptExist" Text="Want to Checked In the Existing One" runat="server"
                                                    GroupName="GrpOpt" onClick="CheckedOpt(this.value)" />
                                            </div>
                                            <div style="float:left; width:90%; margin-bottom:10px;">
                                                <asp:RadioButton ID="OptNew" Text="Want to Checked In with a Revised Version" runat="server"
                                                    GroupName="GrpOpt" onClick="CheckedOpt(this.value)" Checked="true" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="normal_common">
                                    <div style="float:left; width:30%; height:30px; padding-left:10px; line-height:30px;">
                                        <asp:Button CssClass="Btn" ID="cmdCheckInWFL" runat="server" Text="Check In" OnClientClick="javascript: return hideCheckInLoading();" OnClick="cmdCheckInWFL_Click" />
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
</asp:Content>