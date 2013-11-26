<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="drawer_mast.aspx.cs" Inherits="DMS.drawer_mast" %>
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
        function FormValidation() {
            if (document.getElementById("<%=txtDrwName.ClientID %>").value.trim() == "") {
                alert("Please Enter Drawer Name");
                document.getElementById("<%=txtDrwName.ClientID %>").focus();
                return false;
            }
            else if (document.getElementById("<%=txtDrwDesc.ClientID %>").value.trim() == "") {
                alert("Please Enter Drawer Description");
                document.getElementById("<%=txtDrwDesc.ClientID %>").focus();
                return false;
            }
            else {
                return true;
            }
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
                    Drawer Creation
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                    <div style="float:left; width:50%;">
                        <div class="normal_common" id="divCompany" runat="server">
                            <span class="wf_mas_cap">Company</span>
                            <asp:DropDownList ID="ddCompany" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCompany_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Drawer Name</span>
                            <asp:TextBox ID="txtDrwName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Drawer Description</span>
                            <asp:TextBox ID="txtDrwDesc" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        
                        <div class="normal_common" style="display:none;">
                            <span class="wf_mas_cap">Selected Cabinet</span>
                            <asp:TextBox ID="txtLocation" runat="server" CssClass="wf_mas_field" ReadOnly="true"></asp:TextBox>   
                            <asp:HiddenField ID="hfUUID" runat="server" />                         
                        </div>
                        <div class="normal_common">
                            <span class="wf_mas_cap">Cabinet</span>
                            <asp:DropDownList ID="ddCabinet" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                            <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                        </div>
                        <div class="normal_common">
                            <span class="wf_det_add_holder">
                                <asp:Button ID="cmdAddMaster" runat="server" CssClass="wf_mas_add" Text="Add" onclick="cmdAddMaster_Click" />
                            </span>
                        </div>
                        </div>
                        <div style="margin-top:20px;" class="normal_common">
                            <div style="overflow:auto; height:300px;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:HiddenField ID="hfMsg" runat="server" />
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
                                    <AlternatingRowStyle BackColor="White"  />
                                    <Columns>
                                    <asp:TemplateField Visible="False">  
                                    <ItemTemplate >    
                                    <asp:Label ID="lbAutoID" runat="server" Text='<%#Eval("drw_uuid")%>' ></asp:Label>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cabinet" HeaderStyle-Width="150" ItemStyle-Width="150">  
                                    <ItemTemplate ><asp:Label ID="lblCabinet" runat="server" Text='<%#Eval("cab_name")%>' ></asp:Label></ItemTemplate>
                                    <%--<EditItemTemplate>
                                    <asp:DropDownList ID="ddlEditCabinet"  CssClass="wf_det_field2" runat="server" Width="100%"></asp:DropDownList>
                                    
                                    </EditItemTemplate>--%>                                    
                                    <HeaderStyle Width="150px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Drawer Name" HeaderStyle-Width="150" ItemStyle-Width="150">  
                                    <ItemTemplate ><%#Eval("drw_name")%></ItemTemplate>
                                    <HeaderStyle Width="150px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                    <ItemStyle Width="150px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Drawer Description" HeaderStyle-Width="400" ItemStyle-Width="400" >  
                                    <ItemTemplate > <%#Eval("drw_desc")%></ItemTemplate>
                                    <EditItemTemplate>
                                    <asp:HiddenField ID="hdCabinet" runat="server" Value='<%#Eval("cab_name")%>' />
                                    <asp:TextBox ID="txtEditDrwDesc" runat="server" CssClass="wf_mas_field" Text='<%#Eval("drw_desc")%>' Width="100%"></asp:TextBox>                                    
                                    </EditItemTemplate>
                                    <HeaderStyle Width="400px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                    <ItemStyle Width="400px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <%--<asp:CommandField HeaderText="Action" ShowDeleteButton="true" ShowEditButton="True" />--%>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="true" CommandName="Update"
                                                Text="Update"></asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                                                Text="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Edit"
                                                Text="Edit" Visible='<%#Eval("Permission").ToString()=="M"? true: Eval("Permission").ToString()=="D"? true:false %>'></asp:LinkButton>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="true" CommandName="Delete"
                                                Text="Delete" Visible='<%#Eval("Permission").ToString()=="D"? true:false %>'></asp:LinkButton>
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
</asp:Content>
