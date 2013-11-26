<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="user_role.aspx.cs" Inherits="DMS.user_role" %>
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
                    User wise Role Mapping Master
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div style="float:left; width:415px;">
                            <div class="normal_common" id="divCompany" runat="server">
                                <span class="wf_mas_cap">Company</span>
                                <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">User ID</span>
                                <asp:DropDownList ID="ddUser" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Role</span>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfMsg" runat="server" />
                                        <asp:DropDownList ID="ddRole" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddRole_SelectedIndexChanged"></asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="wf_det_add_holder">
                                    <asp:Button ID="cmdAddMaster" runat="server" CssClass="wf_mas_add" Text="Add" onclick="cmdAddMaster_Click" />
                                </span>
                            </div>
                        </div>
                        <div style="float:right; margin-left:10px;">
                            <div class="normal_common">
                                <span style="float:left; width:98%; font-weight:bold; font-size:12px; background-color:#cccccc; padding:5px;">Associated User & Workflows for the selected Role</span>
                            </div>
                            <div class="normal_common" style="margin-top:-10px;">
                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddRole" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                <asp:GridView ID="gvAssociatedRole" runat="server" style="border:1px solid #000;" 
                                            Width="100%" BackColor="White" CellPadding="4" 
                                        ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" OnPageIndexChanging="gvAssociatedRole_PageIndexChanging"
                                        CaptionAlign="Top" EnableModelValidation="True" AllowPaging="True">
                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <PagerSettings Position="Top" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black"  />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"  />
                                    <AlternatingRowStyle BackColor="White"/>
                                    <Columns>
                                        <asp:TemplateField HeaderText="User Name & ID" HeaderStyle-Width="75" ItemStyle-Width="75">  
                                            <ItemTemplate ><%#Eval("name")%></ItemTemplate>
                                            <HeaderStyle Width="75px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="75px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Workflow" HeaderStyle-Width="100" ItemStyle-Width="100" >  
                                            <ItemTemplate ><%#Eval("wf_name")%></ItemTemplate>
                                            <HeaderStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                            <ItemStyle Width="100px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>                            
                        <div style="margin-top:20px;" class="normal_common">
                            <div style="overflow:auto; height:300px;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:GridView ID="gvDispRec" runat="server" style="border:1px solid #000;" 
                                        Width="100%" BackColor="White" CellPadding="4" 
                                    ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" OnPageIndexChanging="gvDispRec_PageIndexChanging"
                                    onrowediting="gvDispRec_RowEditing" 
                                    onrowcancelingedit="gvDispRec_RowCancelingEdit" 
                                    onrowupdating="gvDispRec_RowUpdating" 
                                    OnRowDataBound="gvDispRec_RowDataBound"
                                    onrowdeleting="gvDispRec_RowDeleting" CaptionAlign="Top" 
                                    EnableModelValidation="True" AllowPaging="True">
                                    <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#CCCC99" />
                                        <PagerSettings Position="Top" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black"  />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"  />
                                    <AlternatingRowStyle BackColor="White"/>
                                    <Columns>
                                    <asp:TemplateField Visible="False">
                                    <ItemTemplate >
                                    <asp:Label ID="lbAutoID" runat="server" Text='<%#Eval("role_id")%>' ></asp:Label>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User ID" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                    <ItemTemplate ><%#Eval("user_id")%></ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditUser" CssClass="wf_det_field2" runat="server" Width="100%"></asp:DropDownList>
                                    <asp:HiddenField ID="hdUser" runat="server" Value='<%#Eval("user_id")%>' />
                                    </EditItemTemplate>
                                    <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                    <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name & Email ID" HeaderStyle-Width="400" ItemStyle-Width="400">  
                                        <ItemTemplate ><%#Eval("name")%></ItemTemplate>
                                        <HeaderStyle Width="400px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="400px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Role" HeaderStyle-Width="300" ItemStyle-Width="300" >  
                                    <ItemTemplate ><%#Eval("role_name")%></ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditRole"  CssClass="wf_det_field2" runat="server" Width="100%"></asp:DropDownList>
                                    <asp:HiddenField ID="hdRole" runat="server" Value='<%#Eval("role_name")%>' />
                                    </EditItemTemplate>
                                    <HeaderStyle Width="300px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                    <ItemStyle Width="300px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:CommandField HeaderText="Action" ShowDeleteButton="True" />
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
</asp:Content>
