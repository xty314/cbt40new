<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="v2_test"  MasterPageFile="layout/MasterPage.master" %>

<%@ Register Src="layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>


<%--<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>--%>
<asp:Content ContentPlaceHolderId="AdditionalCSS" runat="server">
   
    
</asp:Content>

<asp:Content ContentPlaceHolderId="Header" runat="server">
   
    <uc1:ContentHeader runat="server" ID="ContentHeader" GTitle="Gpos" />

</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">

  <!-- Main content -->
    <form runat="server">
          <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
       <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    </form>
    
 <!-- /.content -->
</asp:Content>
<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">


</asp:Content>