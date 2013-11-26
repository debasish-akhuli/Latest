<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="ClientManagement.aspx.cs" Inherits="DMS.ClientManagement" %>
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

        function checkInput(ob) {
            var invalidChars = /[^0-9]/gi
            if (invalidChars.test(ob.value)) {
                ob.value = ob.value.replace(invalidChars, "");
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("a.close").hide();

            $("td.show_close").mouseover(function () {
                $(this).children('a.close').show();
            });

            $("td.show_close").mouseout(function () {
                $(this).children('a.close').hide();
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
                    Client Management
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div style="float:left; width:50%;">
                            <div class="normal_common">
                                <span class="wf_mas_cap">Company</span>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Company Code</span>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:TextBox ID="txtCompCode" runat="server" CssClass="wf_mas_field" ReadOnly="true"></asp:TextBox>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Company Name</span>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:TextBox ID="txtCompName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Contact Person</span>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:TextBox ID="txtContactPersonName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Email ID</span>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:TextBox ID="txtEmailID" runat="server" CssClass="wf_mas_field" ReadOnly="true"></asp:TextBox>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Contact No</span>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:TextBox ID="txtContactNo" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_det_add_holder" style="margin-left:140px;">
                                    <asp:Button ID="cmdUpdate" runat="server" CssClass="wf_mas_add" Text="Update" onclick="cmdUpdate_Click" />
                                </span>
                            </div>
                        </div>
                        <div style="float:right; width:50%;">
                            <div class="normal_common">
                                <span class="wf_mas_cap">Status</span>
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:DropDownList ID="ddStatus" CssClass="wf_det_field2" runat="server">
                                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="Pending" Text="Pending"></asp:ListItem>
                                    <asp:ListItem Value="Inactive" Text="Inactive"></asp:ListItem>
                                    <asp:ListItem Value="Expired" Text="Expired"></asp:ListItem>
                                </asp:DropDownList>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Space</span>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:DropDownList ID="ddSpace" CssClass="wf_det_field2" runat="server" AutoPostBack="true" onselectedindexchanged="ddSpace_SelectedIndexChanged"></asp:DropDownList>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" runat="server">
                                <span class="wf_mas_cap">Max # of Users</span>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:TextBox ID="txtMaxNoOfUsers" runat="server" Text="0" CssClass="wf_mas_field5" AutoPostBack="true" OnTextChanged="CtrlChanged" onkeyup="checkInput(this)"></asp:TextBox>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="Div1" class="normal_common" runat="server">
                                <span class="wf_mas_cap">Space Rate</span>
                                <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:Label ID="lblSpace" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblSpaceRate" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblSpaceRate1" runat="server" Text="0.00"></asp:Label>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="Div2" class="normal_common" runat="server">
                                <span class="wf_mas_cap">Per User Rate</span>
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:Label ID="lblPerUserRate" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblPerUserRate1" runat="server" Text="0.00"></asp:Label>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="Div3" class="normal_common" runat="server">
                                <span class="wf_mas_cap">Total Rate</span>
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                <asp:Label ID="lblTotalRate" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblTotalRate1" runat="server" Text="0.00"></asp:Label>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
