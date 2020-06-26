<%@ Page Language="C#" AutoEventWireup="true" CodeFile="eprice.aspx.cs" Inherits="v2_eprice" MasterPageFile="~/MasterPage/MasterPage.master" %>


<%@ Register Src="~/MasterPage/layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>



<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>
<asp:Content ContentPlaceHolderId="AdditionalCSS" runat="server">
    <%--<link href="plugins/jqgrid/css/octicons.css" rel="stylesheet" />--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/octicons/4.4.0/font/octicons.css">
    <link href="/asset/plugins/jqgrid/css/ui.jqgrid-bootstrap4.css" rel="stylesheet" />
    <link href="/asset/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <%--<link href="plugins/jqgrid/css/responsive.css" rel="stylesheet" />--%>
</asp:Content>

<asp:Content ContentPlaceHolderId="Header" runat="server">

    <uc1:ContentHeader runat="server" ID="ContentHeader"  />

</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">
   <!-- Main content -->
      
    <section class="content">
       
        <div class="row">

         <div class="form-group col-12 col-sm-2">
                     <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal" data-whatever="@mdo">
                          <i class="right fas fa-plus-circle"></i> New Item

                     </button>
                  </div>
            
       



            
        </div>
      <div class="card card-outline">
        <div class="card-header filter-header">
            <form runat="server" id="FilterForm">
            <div class="flex-container row">
                <div class="flex-item col-lg-2 col-sm-12 col-md-5"> 
                     <asp:DropDownList ID="SupplierDropDownList"  class="form-control form-control-sm" 
                         AutoPostBack="true" 
                         OnSelectedIndexChanged="SupplierDropDownList_SelectedIndexChanged"
                         runat="server">
                        
                    </asp:DropDownList>
                    
                </div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5">  
                    <input type="text" class="form-control form-control-sm" placeholder="Search Mail">
                
                </div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5">  
                    <asp:DropDownList ID="CatDropDownList" runat="server" class="form-control form-control-sm"  AutoPostBack="true" OnSelectedIndexChanged="CatDropDownList_SelectedIndexChanged" ></asp:DropDownList>

                </div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5"> 
                    
                   <asp:DropDownList ID="sCatDropDownList" runat="server" class="form-control form-control-sm"  AutoPostBack="true" OnSelectedIndexChanged="sCatDropDownList_SelectedIndexChanged" ></asp:DropDownList>


                </div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5">
                    <asp:DropDownList ID="ssCatDropDownList" runat="server"  class="form-control form-control-sm" AutoPostBack="true"></asp:DropDownList>
           


                </div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5"><div class="input-group input-group-sm">
                  <input type="text" class="form-control" placeholder="Keyword" >
                  <div class="input-group-append">
                    <div class="btn btn-primary">
                      <i class="fas fa-search"></i>
                    </div>
                  </div>
                </div></div>
            </div>
          </form>
     <%--         <div class="card-search-filter">
                
           <input type="" value="" />
               
            </div>--%>
 <%--           <form runat="server" class="form-inline ml-0 "  role="form">
             
                  
                <div class="form-group col-12 col-sm-2">
                  
                    <asp:DropDownList ID="SupplierDropDownList" class="form-control form-control-sm" runat="server">

                    </asp:DropDownList>
                  </div>
                 <div class="input-group input-group-sm">
                  
                    <input type="text" class="form-control" placeholder="Search Mail">
                  </div>

          

        </form>--%>
                     
  <%--             <div class="form-group form-floating-group mt-4">
        <input type="text" id="name" class="form-control" required>
        <label class="form-control-placeholder" for="name">Nsdfsdf</label>
      </div>--%>
       <%--          <div class="card-tools">
                    
                <div class="input-group input-group-sm">
                  <input type="text" class="form-control" placeholder="Search Mail">
                  <div class="input-group-append">
                    <div class="btn btn-primary">
                      <i class="fas fa-search"></i>
                    </div>
                  </div>
                </div>
              </div>--%>
        </div>

        <!-- /.card-header -->
        <div class="card-body" id="gridwrap">
           <table id="jqGrid"></table>
            <div id="jqGridPager"></div>
        </div>
        <!-- /.card-body -->
      </div>
      <!-- /.card -->
    </section>
    <!-- /.content -->


    <!--modal-->
    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel"> <i class="fas fa-cookie"></i> Add New Item</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <form >
            <div class="row">
          <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Button Name</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
                       <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Price(GST Inc.)</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
                       <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Cost(GST Inc.)</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
                      <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Cost(GST Exc.)</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
            <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Invoice Description</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
              <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Kitchen Description</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
              <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Supplier</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
             <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Department</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
              <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">1st Category</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
              <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Stock</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
              <div class="form-group col-6">
            <label for="recipient-name" class="col-form-label">Warning Stock</label>
            <input type="text" class="form-control form-control-sm" id="recipient-name">
          </div>
     
        </div>
        </form>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary">Save</button>
      </div>
    </div>
  </div>
      
</div>

</asp:Content>

<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">
    <script src="/asset/plugins/jqgrid/js/i18n/grid.locale-en.js"></script>
     <script src="/asset/plugins/jqgrid/js/jquery.jqGrid.min.js"></script>
    <script src="/asset/plugins/select2/js/select2.full.min.js"></script>
    <script type="text/javascript"> 
var obj={
    supplier:$("#<%=SupplierDropDownList.ClientID%>").val(),
    cat:$("#<%=CatDropDownList.ClientID%>").val(),
    scat:$("#<%=sCatDropDownList.ClientID%>").val()||-1,
    sscat:$("#<%=ssCatDropDownList.ClientID%>").val()||-1
}
    $(function(){
       
        $("#<%=SupplierDropDownList.ClientID%>").select2({
         placeholder: "Select a state",
  
        });             
    })
    </script>
    <script src="/asset/js/eprice.js"></script>
</asp:Content>