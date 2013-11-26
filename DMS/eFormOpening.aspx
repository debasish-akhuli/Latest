<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="eFormOpening.aspx.cs" Inherits="DMS.eFormOpening" %>
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
        function showLoading() {
            document.getElementById("dvLoading1").style.display = 'block';
            document.getElementById("dvLoading2").style.display = 'block';
        }
        function hideLoading() {
            document.getElementById("dvLoading1").style.display = 'none';
            document.getElementById("dvLoading2").style.display = 'none';
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
                    Workflow Execution
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
                        <div style="float:left; width:51%;">
                            <div class="normal_common" id="div1" runat="server">
                                <span style="float:right;">
                                    <asp:Button ID="cmdEdit" runat="server" CssClass="wf_mas_add" Text="Edit" onclick="cmdEdit_Click" />
                                </span>
                            </div>
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
                        </div>
                        <div style="float:right; width:48%;">
                            <div class="normal_common" id="divDocType" runat="server">
                                <span class="wf_mas_cap">Document Type</span>
                                <asp:DropDownList ID="ddDocType" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDocType_SelectedIndexChanged"></asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common" id="divDept" runat="server">
                                <span class="wf_mas_cap">Department</span>
                                <asp:DropDownList ID="ddDept" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDept_SelectedIndexChanged"></asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common" id="divUpldLoc" runat="server">
                                <span class="wf_mas_cap">Uploaded Location</span>
                                <asp:Label ID="lblLocation" runat="server" Font-Bold="true" Width="250px"></asp:Label>
                            </div>
                            <div class="normal_common" id="divAssignedWFL" runat="server">
                                <span class="wf_mas_cap">Workflow</span>
                                <asp:HiddenField ID="hfWFLID" runat="server" />
                                <asp:Label ID="lblWFL" runat="server" Font-Bold="true"></asp:Label>
                                <span style="float:right; padding-right:50px; font-weight:bold;"><asp:Button ID="cmdViewWFL" Text="View" CssClass="wf_mas_add" ToolTip="Show Details" OnClick="cmdViewWFL_Click" OnClientClick="javascript: return showLoading();" runat="server" /></span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Name</span>
                                <asp:Label ID="lblDocName" runat="server" Text="" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txtDocName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Description</span>
                                <asp:TextBox ID="txtDocDesc" runat="server" CssClass="wf_mas_field_m" TextMode="MultiLine"></asp:TextBox>
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
                            <div class="normal_common">
                                <asp:Button ID="cmdSave" runat="server" CssClass="wf_mas_add" Text="Save" onclick="cmdSave_Click" />
                            </div>
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--Workflow Details View Start--%>
    <div id="dvLoading1" ></div>
    <div style="top:-100px;" id="dvLoading2" >
    <div class="lightbox_1">
    
    <div class="lightbox_2">
    <asp:UpdatePanel ID="UpdatePanel22" runat="server" UpdateMode="Conditional">
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="cmdViewWFL" EventName="click"></asp:AsyncPostBackTrigger>
    </Triggers>
    <ContentTemplate>
    <div class="lightbox_3">
    <div style="float:left; font-weight:bold; border-bottom:1px solid #666; font-size:16px; width:100%;">
        Workflow Details
        </div>
    <div class="normal_feature">
    <asp:Label CssClass="lightbox_label" ID="Lblwf" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfname" runat="server" Text=""></asp:Label></div>
    <div class="normal_feature"><asp:Label  CssClass="lightbox_label" ID="Lbldept" runat="server"  Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfdept" runat="server" Text=""></asp:Label></div>
    <div class="normal_feature"><asp:Label  CssClass="lightbox_label" ID="Lbldoc" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfDoctype" runat="server" Text=""></asp:Label></div>
    <div class="normal_feature"><asp:Label  CssClass="lightbox_label" ID="LblFld" runat="server" Text=""></asp:Label><asp:Label CssClass="lightbox_label2" ID="lblWfFld" runat="server" Text=""></asp:Label></div>
                        
    <div style="width:100%; float:left; padding-top:20px;">
    <div style="overflow:auto; height:290px;">
        <asp:GridView style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" ForeColor="Black" ID="gv" runat="server">
        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
        <FooterStyle BackColor="#CCCC99" />
        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" />
        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center" />
        <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    </div>
    <div class="normal_feature">
    <div style="color:#FF0000; text-align:center; font-weight:bold; font-size:16px;  border-bottom:1px solid #666; padding:10px;">
        <asp:Label ID="MsgNodet" runat="server"></asp:Label> 
    </div>
    </div>
    <div  class="normal_feature" style="text-align:right;">                         
        <input type="button" id="ImageButton1" class="padd_marg1" onclick="javascript: return hideLoading();" />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel> 
    </div>
    
    </div>
    </div>
    <%--Workflow Details View End--%>
</asp:Content>