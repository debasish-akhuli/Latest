<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="doc_type_mast.aspx.cs" Inherits="DMS.doc_type_mast" %>
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
        });

        function SetValue() {
            var e = document.getElementById("ctl00_ContentPlaceHolder1_ddFormType");
            var SelectedItem = e.options[e.selectedIndex].value;
            if (SelectedItem == "Non Editable") {
                if (document.getElementById("<%=txtTag1.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag1FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag1FieldNo.ClientID %>").value = "1";
                }
                if (document.getElementById("<%=txtTag2.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag2FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag2FieldNo.ClientID %>").value = "2";
                }
                if (document.getElementById("<%=txtTag3.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag3FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag3FieldNo.ClientID %>").value = "3";
                }
                if (document.getElementById("<%=txtTag4.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag4FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag4FieldNo.ClientID %>").value = "4";
                }
                if (document.getElementById("<%=txtTag5.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag5FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag5FieldNo.ClientID %>").value = "5";
                }
                if (document.getElementById("<%=txtTag6.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag6FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag6FieldNo.ClientID %>").value = "6";
                }
                if (document.getElementById("<%=txtTag7.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag7FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag7FieldNo.ClientID %>").value = "7";
                }
                if (document.getElementById("<%=txtTag8.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag8FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag8FieldNo.ClientID %>").value = "8";
                }
                if (document.getElementById("<%=txtTag9.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag9FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag9FieldNo.ClientID %>").value = "9";
                }
                if (document.getElementById("<%=txtTag10.ClientID %>").value.trim() == "") {
                    document.getElementById("<%=txtTag10FieldNo.ClientID %>").value = "0";
                }
                else {
                    document.getElementById("<%=txtTag10FieldNo.ClientID %>").value = "10";
                }
            }
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
                    Document Type Master
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
                                <span class="wf_mas_cap">Document Type ID</span>
                                <asp:TextBox ID="txtDocTypeID" MaxLength="10" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Document Type Name</span>
                                <asp:TextBox ID="txtDocTypeName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Form Type</span>
                                <asp:DropDownList ID="ddFormType" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddFormType_SelectedIndexChanged">
                                    <asp:ListItem Value="Non Editable" Text="Non Editable"></asp:ListItem>
                                    <asp:ListItem Value="Editable" Text="Editable"></asp:ListItem>
                                    <asp:ListItem Value="eForm" Text="eForm"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common" id="divSignFieldNo1" runat="server">
                                <span class="wf_mas_cap">Sign Field No 1</span>
                                <asp:TextBox ID="txtSignFieldNo1" runat="server" Text="0" CssClass="wf_mas_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divSignDateFieldNo1" runat="server">
                                <span class="wf_mas_cap">Sign Date Field No 1</span>
                                <asp:TextBox ID="txtSignDateFieldNo1" runat="server" Text="0" CssClass="wf_mas_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divSignFieldNo2" runat="server">
                                <span class="wf_mas_cap">Sign Field No 2</span>
                                <asp:TextBox ID="txtSignFieldNo2" runat="server" Text="0" CssClass="wf_mas_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divSignDateFieldNo2" runat="server">
                                <span class="wf_mas_cap">Sign Date Field No 2</span>
                                <asp:TextBox ID="txtSignDateFieldNo2" runat="server" Text="0" CssClass="wf_mas_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divSignFieldNo3" runat="server">
                                <span class="wf_mas_cap">Sign Field No 3</span>
                                <asp:TextBox ID="txtSignFieldNo3" runat="server" Text="0" CssClass="wf_mas_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divSignDateFieldNo3" runat="server">
                                <span class="wf_mas_cap">Sign Date Field No 3</span>
                                <asp:TextBox ID="txtSignDateFieldNo3" runat="server" Text="0" CssClass="wf_mas_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_det_add_holder" style="margin-left:140px;">
                                    <asp:Button ID="cmdAddMaster" runat="server" CssClass="wf_mas_add" Text="Add" onclick="cmdAddMaster_Click" />
                                </span>
                            </div>
                        </div>
                        <div style="float:right; width:50%;">
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 1</span>
                                <asp:TextBox ID="txtTag1" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag1FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 2</span>
                                <asp:TextBox ID="txtTag2" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag2FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 3</span>
                                <asp:TextBox ID="txtTag3" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag3FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 4</span>
                                <asp:TextBox ID="txtTag4" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag4FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 5</span>
                                <asp:TextBox ID="txtTag5" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag5FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 6</span>
                                <asp:TextBox ID="txtTag6" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag6FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 7</span>
                                <asp:TextBox ID="txtTag7" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag7FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 8</span>
                                <asp:TextBox ID="txtTag8" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag8FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 9</span>
                                <asp:TextBox ID="txtTag9" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag9FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Tag Name 10</span>
                                <asp:TextBox ID="txtTag10" MaxLength="50" onchange="SetValue();" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <asp:TextBox ID="txtTag10FieldNo" runat="server" Text="0" CssClass="small_field" onkeyup="checkInput(this)"></asp:TextBox>                                
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
                                    <AlternatingRowStyle BackColor="White"  />
                                    <Columns>
                                    <asp:TemplateField Visible="False">
                                    <ItemTemplate >
                                    <asp:Label ID="lbAutoID" runat="server" Text='<%#Eval("doc_type_id")%>' ></asp:Label>
                                    </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc Type ID" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("doc_type_id")%></ItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Doc Type Name" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("doc_type_name")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditDocTypeName" runat="server" Text='<%#Eval("doc_type_name")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 1" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag1")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag1" runat="server" Text='<%#Eval("tag1")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 2" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag2")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag2" runat="server" Text='<%#Eval("tag2")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 3" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag3")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag3" runat="server" Text='<%#Eval("tag3")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 4" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag4")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag4" runat="server" Text='<%#Eval("tag4")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 5" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag5")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag5" runat="server" Text='<%#Eval("tag5")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 6" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag6")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag6" runat="server" Text='<%#Eval("tag6")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 7" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag7")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag7" runat="server" Text='<%#Eval("tag7")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 8" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag8")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag8" runat="server" Text='<%#Eval("tag8")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 9" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag9")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag9" runat="server" Text='<%#Eval("tag9")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tag 10" HeaderStyle-Width="200" ItemStyle-Width="200">  
                                        <ItemTemplate ><%#Eval("tag10")%></ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtEditTag10" runat="server" Text='<%#Eval("tag10")%>' Width="100%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <HeaderStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></HeaderStyle>
                                        <ItemStyle Width="200px" HorizontalAlign="Left" CssClass="GVRec"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:CommandField HeaderText="Action" ShowDeleteButton="True" ShowEditButton="True" />
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