<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="eFormWFL.aspx.cs" Inherits="DMS.eFormWFL" %>
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
        });
        function OpenRedirect() {
            document.getElementById("divRedirectWF").style.display = "block";
            document.getElementById("divNormalWF").style.display = "none";
        }
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
                    Cabinet Creation
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                        <ContentTemplate>
                            <asp:HiddenField ID="hfMsg" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hfControls" runat="server" />
                    <asp:HiddenField ID="hfButtonStatus" runat="server" />
                    <asp:HiddenField ID="hfDropdownValues" runat="server" />
                    <asp:HiddenField ID="hfControls4Formula" runat="server" />
                    <asp:HiddenField ID="hfUUID" runat="server" />
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
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
                        <div style="float:right; width:48%;">
                            <div class="normal_common" style="padding-top:10px; background:#efdeed; height:450px; overflow:auto;">
                                <asp:Table ID="tblMain" CellPadding="5" CellSpacing="5" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                        </asp:TableCell>
                                        <asp:TableCell>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                            <div class="normal_common">
                                <asp:TextBox ID="txtTag1" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag2" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag3" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag4" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag5" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag6" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag7" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag8" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag9" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtTag10" runat="server" CssClass="wf_mas_field" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>