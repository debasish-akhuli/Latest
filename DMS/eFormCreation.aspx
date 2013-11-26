<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="eFormCreation.aspx.cs" Inherits="DMS.eFormCreation" %>
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
    </script>
    <link href="eForm.css" rel="stylesheet" type="text/css" media="all" />
    <script type="text/javascript">
        function ButtonStatus(ButtonName) {
            document.getElementById("<%= hfButtonStatus.ClientID %>").value = ButtonName.toString();
            var ddl = document.getElementById("<%= ddControlType.ClientID %>");
            var Text = ddl.options[ddl.selectedIndex].text;
            var Value = ddl.options[ddl.selectedIndex].value;
            var DDControls = document.getElementById("divDDControls");
            var dvDDDataType = document.getElementById("divDDDataType");
            var SpanMxLen = document.getElementById("divMaxLength");
            var SpanMnVal = document.getElementById("divMinVal");
            var SpanMxVal = document.getElementById("divMaxVal");
            var SpanDD4Frml = document.getElementById("divDD4Formula");
            var SpanFrml = document.getElementById("divFormula");
            if (Value == "D") {
                DDControls.style.display = "block";
                dvDDDataType.style.display = "none";

                SpanMxLen.style.display = "none";
                SpanMnVal.style.display = "none";
                SpanMxVal.style.display = "none";
                SpanDD4Frml.style.display = "none";
                SpanFrml.style.display = "none";
            }
            else {
                DDControls.style.display = "none";
                dvDDDataType.style.display = "block";

                var ddlDataType = document.getElementById("<%= ddDataType.ClientID %>");
                var ValDataType = ddlDataType.options[ddlDataType.selectedIndex].value;
                if (ValDataType == "Text") {
                    SpanMxLen.style.display = "block";
                    SpanMnVal.style.display = "none";
                    SpanMxVal.style.display = "none";
                    SpanDD4Frml.style.display = "none";
                    SpanFrml.style.display = "none";
                }
                else if (ValDataType == "Numeric") {
                    SpanMxLen.style.display = "none";
                    SpanMnVal.style.display = "block";
                    SpanMxVal.style.display = "block";
                    SpanDD4Frml.style.display = "none";
                    SpanFrml.style.display = "none";
                }
                else if (ValDataType == "Date") {
                    SpanMxLen.style.display = "none";
                    SpanMnVal.style.display = "none";
                    SpanMxVal.style.display = "none";
                    SpanDD4Frml.style.display = "none";
                    SpanFrml.style.display = "none";
                }
                else if (ValDataType == "Formula") {
                    SpanMxLen.style.display = "none";
                    SpanMnVal.style.display = "none";
                    SpanMxVal.style.display = "none";
                    SpanDD4Frml.style.display = "block";
                    SpanFrml.style.display = "block";
                }
            }
        }

        function AddDDValues() {
            document.getElementById("<%= hfDropdownValues.ClientID %>").value = document.getElementById("<%= hfDropdownValues.ClientID %>").value + "~" + document.getElementById("<%= txtDropdownValues.ClientID %>").value;
            document.getElementById("<%= txtDropdownValues.ClientID %>").value = "";
        }

        function CreateFormula() {
            var ddl0 = document.getElementById("<%= ddLabelDesc.ClientID %>");
            var Text0 = ddl0.options[ddl0.selectedIndex].text;
            var Value0 = ddl0.options[ddl0.selectedIndex].value;
            document.getElementById("<%= txtFormula.ClientID %>").value = document.getElementById("<%= txtFormula.ClientID %>").value + Value0;
            document.getElementById("<%= hfControls4Formula.ClientID %>").value = document.getElementById("<%= hfControls4Formula.ClientID %>").value + "," + Value0;
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
                    eForm Creation
                </div>
            </div>
            
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                        <div class="left_part" style="padding-left:10px; padding-right:10px;">
                            <div class="normal_common" style="padding-top:10px;">
                                <span class="body_cap">Label Description</span>
                                <asp:TextBox ID="txtLabelText" Text="" runat="server" CssClass="txt_field"></asp:TextBox>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfMsg" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common">
                                <span class="body_cap">Control Type</span>
                                <asp:DropDownList ID="ddControlType" runat="server" CssClass="dd_field" onchange="javascript: return ButtonStatus('B');">
                                    <asp:ListItem Value="T" Text="Text"></asp:ListItem>
                                    <asp:ListItem Value="D" Text="Dropdown"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="normal_common" id="divDDControls" style="display:none;">
                                <span class="body_cap">Dropdown Items</span>
                                <asp:TextBox ID="txtDropdownValues" runat="server" Text="" CssClass="txt_field"></asp:TextBox>
                                <input id="cmdAddDDValues" class="btn" type="button" value="+" onclick="javascript: return AddDDValues()" />
                            </div>
                            <div class="normal_common" id="divDDDataType" style="display:block;">
                                <span class="body_cap">Data Type</span>
                                <asp:DropDownList ID="ddDataType" runat="server" CssClass="dd_field" onchange="javascript: return ButtonStatus('C');"> <%--C is just used for Control--%>
                                    <asp:ListItem Value="Text" Text="Text"></asp:ListItem>
                                    <asp:ListItem Value="Numeric" Text="Numeric"></asp:ListItem>
                                    <asp:ListItem Value="Date" Text="Date"></asp:ListItem>
                                    <asp:ListItem Value="Formula" Text="Formula"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="normal_common" id="divMaxLength" style="display:block;">
                                <span class="body_cap">Max Length</span>
                                <asp:TextBox ID="txtMaxLength" runat="server" Text="0" CssClass="txt_field_small"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divMinVal" style="display:none;">
                                <span class="body_cap">Min Value</span>
                                <asp:TextBox ID="txtMinVal" runat="server" Text="0" CssClass="txt_field_small"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divMaxVal" style="display:none;">
                                <span class="body_cap">Max Value</span>
                                <asp:TextBox ID="txtMaxVal" runat="server" Text="0" CssClass="txt_field_small"></asp:TextBox>
                            </div>
                            <div class="normal_common" id="divDD4Formula" style="display:none;">
                                <span class="body_cap">Select Label</span>
                                <asp:DropDownList ID="ddLabelDesc" runat="server" CssClass="dd_field">
                                </asp:DropDownList>
                                <input id="cmdCreateFormula" type="button" class="btn" value="+" onclick="javascript: return CreateFormula()" />
                            </div>
                            <div class="normal_common" id="divFormula" style="display:none;">
                                <span class="body_cap">Formula</span>
                                <asp:TextBox ID="txtFormula" runat="server" Text="" CssClass="txt_field"></asp:TextBox>
                            </div>
                            <div class="normal_common">
                                <span class="body_cap">Position</span>
                                <asp:DropDownList ID="ddPosition" runat="server" CssClass="dd_field">
                                    <asp:ListItem Text="At the Beginning"></asp:ListItem>
                                </asp:DropDownList>
                                <span style="float:left; padding-left:20px;" id="span1">
                                    <asp:Button ID="cmdInsert" runat="server" Text="Insert" CssClass="btn" onclick="cmdInsert_Click" OnClientClick="javascript: return ButtonStatus('G')" />
                                </span>
                            </div>
                            <div class="normal_common">
                                <div style="width:100%; height:305px; font-size:10px; float:left; padding-top:20px; display:block; overflow:auto;">
                                    <asp:GridView ID="gvFields" runat="server" style="border:1px solid #000;" Width="100%" BackColor="White" CellPadding="4" 
                                        ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" onselectedindexchanged="gvFields_SelectedIndexChanged" 
                                        onrowdeleting="gvFields_RowDeleting" CaptionAlign="Top">
                                        <RowStyle BackColor="#F7F7DE" HorizontalAlign="Center"/>
                                        <FooterStyle BackColor="#CCCC99"/>
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black"/>
                                        <SelectedRowStyle BackColor="#dae2ea" Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center"/>
                                        <HeaderStyle BackColor="#cccccc" Font-Bold="True" HorizontalAlign="Center"/>
                                        <AlternatingRowStyle BackColor="White"/>
                                        <Columns>
                                            <asp:TemplateField Visible="False">
                                                <ItemTemplate >
                                                    <asp:Label ID="lbSerialNo" runat="server" Text='<%#Eval("SerialNo")%>' ></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Serial No">  
                                                <ItemTemplate ><%#Eval("SerialNo")%></ItemTemplate>                                   
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Label ID" Visible="false">  
                                                <ItemTemplate ><%#Eval("LabelID")%></ItemTemplate>                                   
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Control ID" Visible="False">  
                                                <ItemTemplate ><%#Eval("ControlID")%></ItemTemplate>                                   
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Control Type" Visible="false">
                                                <ItemTemplate ><%#Eval("ControlType")%></ItemTemplate>                                   
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data Type" Visible="false">
                                                <ItemTemplate ><%#Eval("DataType")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Max Length" Visible="false">
                                                <ItemTemplate ><%#Eval("MaxLength")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MinVal" Visible="false">
                                                <ItemTemplate ><%#Eval("MinVal")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MaxVal" Visible="false">
                                                <ItemTemplate ><%#Eval("MaxVal")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Formula" Visible="false">  
                                                <ItemTemplate ><%#Eval("Formula")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Label Desc"> 
                                                <ItemTemplate ><%#Eval("LabelDesc")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Controls for Formula" Visible="false">
                                                <ItemTemplate ><%#Eval("Controls4Formula")%></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate >
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Text="Delete" OnClientClick="javascript: return ButtonStatus('Del');" tooltip ='Delete'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="right_part">
                            <div class="normal_common" id="divDocType" runat="server" style="padding-top:10px; background:#dddddd;">
                                <span class="wf_mas_cap" style="margin-left:10px;">Document Type</span>
                                <asp:DropDownList ID="ddDocType" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common" id="divUpldCabinet" runat="server" style="background:#dddddd;">
                                <span class="wf_mas_cap" style="margin-left:10px;">Cabinet</span>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddCabinet" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddCabinet_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" id="divUpldDrawer" runat="server" style="background:#dddddd;">
                                <span class="wf_mas_cap" style="margin-left:10px;">Drawer</span>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddDrawer" CssClass="wf_det_field2_big" runat="server" AutoPostBack="true" onselectedindexchanged="ddDrawer_SelectedIndexChanged"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" id="divUpldFolder" runat="server" style="background:#dddddd;">
                                <span class="wf_mas_cap" style="margin-left:10px;">Folder</span>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">                                
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddFolder" CssClass="wf_det_field2_big" runat="server"></asp:DropDownList>
                                    <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="normal_common" style="background:#dddddd;">
                                <span class="body_cap" style="margin-left:10px;">eForm Name</span>
                                <asp:TextBox ID="txtFormName" Text="" runat="server" CssClass="txt_field"></asp:TextBox>
                                <span style="float:left; padding-left:10px;" id="span2">
                                    <asp:Button ID="cmdSave" runat="server" CssClass="btn" Text="Save" OnClick="cmdSave_Click" OnClientClick="javascript: return ButtonStatus('S')" />
                                </span>
                                <asp:HiddenField ID="hfControls" runat="server" />
                                <asp:HiddenField ID="hfButtonStatus" runat="server" />
                                <asp:HiddenField ID="hfDropdownValues" runat="server" />
                                <asp:HiddenField ID="hfControls4Formula" runat="server" />
                            </div>
                            <div class="normal_common" style="background:#efdeed; height:314px; overflow:auto;">
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>