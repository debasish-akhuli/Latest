<%@ Page Title="myDOCbase" Language="C#" MasterPageFile="adm_mast_entry.Master" AutoEventWireup="true" CodeBehind="ClientRegistration.aspx.cs" Inherits="DMS.ClientRegistration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="js/jquary_min.js"></script>
    <script language="javascript" type="text/javascript">
        function checkInput(ob) {
            var invalidChars = /[^0-9]/gi
            if (invalidChars.test(ob.value)) {
                ob.value = ob.value.replace(invalidChars, "");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_body">
        <div class="main">
            <div class="normal_common">
                <div class="wf_mas_Heading"><span>Welcome</span>
                    
                </div>
            </div>
            <div style="padding-bottom:0px;" class="normal_common">
                <div class="wf_mas_Heading2">
                    Registration
                </div>
            </div>
            <div class="normal_common">
                <div class="wf_mas_doc_holder" style="min-height:300px;">
                    <div class="wf_mas_doc_holder_padd">
                        <div style="float:left; width:50%;">
                            <div class="normal_common">
                                <span class="wf_mas_cap">Company Name</span>
                                <asp:TextBox ID="txtCompName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Contact Person</span>
                                <asp:TextBox ID="txtContactPersonName" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Contact No</span>
                                <asp:TextBox ID="txtContactNo" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Email ID</span>
                                <asp:TextBox ID="txtEmailID" runat="server" CssClass="wf_mas_field"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div class="normal_common">
                                <span class="wf_det_add_holder" style="margin-left:140px;">
                                    <asp:Button ID="cmdRegister" runat="server" CssClass="wf_mas_add" Text="Register" onclick="cmdRegister_Click" />
                                </span>
                            </div>
                        </div>
                        <div style="float:right; width:50%;">
                            <div class="normal_common" style="display:none;">
                                <span class="wf_mas_cap">Status</span>
                                <asp:DropDownList ID="ddStatus" CssClass="wf_det_field2" runat="server">
                                    <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="Pending" Text="Pending"></asp:ListItem>
                                    <asp:ListItem Value="Inactive" Text="Inactive"></asp:ListItem>
                                    <asp:ListItem Value="Expired" Text="Expired"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="normal_common">
                                <span class="wf_mas_cap">Space</span>
                                <asp:DropDownList ID="ddSpace" CssClass="wf_det_field2" runat="server" AutoPostBack="true" onselectedindexchanged="ddSpace_SelectedIndexChanged"></asp:DropDownList>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div id="Div4" class="normal_common" runat="server">
                                <span class="wf_mas_cap">Max # of Users</span>
                                <asp:TextBox ID="txtMaxNoOfUsers" runat="server" Text="0" CssClass="wf_mas_field5" AutoPostBack="true" OnTextChanged="CtrlChanged" onkeyup="checkInput(this)"></asp:TextBox>
                                <span style="padding-left:5px; float:left; font-weight:bold; font-size:24px; color:Red;">*</span>
                            </div>
                            <div id="Div1" class="normal_common" runat="server">
                                <span class="wf_mas_cap">Space Rate</span>
                                <asp:Label ID="lblSpace" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblSpaceRate" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblSpaceRate1" runat="server" Text="0.00"></asp:Label>
                            </div>
                            <div id="Div2" class="normal_common" runat="server">
                                <span class="wf_mas_cap">Per User Rate</span>
                                <asp:Label ID="lblPerUserRate" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblPerUserRate1" runat="server" Text="0.00"></asp:Label>
                            </div>
                            <div id="Div3" class="normal_common" runat="server">
                                <span class="wf_mas_cap">Total Rate</span>
                                <asp:Label ID="lblTotalRate" runat="server" Text="0.00" Visible="false"></asp:Label>
                                <asp:Label ID="lblTotalRate1" runat="server" Text="0.00"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
