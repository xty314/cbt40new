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
            <form runat="server">
            <div class="flex-container row">
                <div class="flex-item col-lg-2 col-sm-12 col-md-5"> 
                     <asp:DropDownList ID="SupplierDropDownList" class="form-control form-control-sm"  runat="server">

                    </asp:DropDownList>
                    
                </div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5">  <input type="text" class="form-control form-control-sm" placeholder="Search Mail"></div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5">  <input type="text" class="form-control form-control-sm" placeholder="Search Mail"></div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5">  <input type="text" class="form-control form-control-sm" placeholder="Search Mail"></div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5">  <input type="text" class="form-control form-control-sm" placeholder="Search Mail"></div>
                <div class="flex-item col-lg-2 col-sm-12 col-md-5"><div class="input-group input-group-sm">
                  <input type="text" class="form-control" placeholder="Search Mail">
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
             $(document).ready(function () {
                $("#<%=SupplierDropDownList.ClientID%>").select2();
                 //var width = $("#gridwrap").width();
             
              
                 $.jgrid.defaults.styleUI = 'Bootstrap4';
                 $.jgrid.defaults.iconSet = "Octicons";
                 $("#jqGrid").jqGrid({
                     url: '/ajax/eprice.ashx',
                     mtype: "POST",
                     
                     datatype: "json",
                     colModel: [
                         { label: 'code', name: 'id', sorttype: 'integer', editable: true },
                         { label: 'name', name: 'name', width: "300" },
                         { label: 'supplier_code', name: 'supplier_code' },
                         { label: 'barcode', name: 'barcode' },
                         { label: 'code', name: 'id', key: true },
                         { label: 'name_cn', name: 'name_cn' },
                         { label: 'web item', name: 'is_website_item', formatter: 'checkbox' },

                     ],
                     //menubar: true,
                     viewrecords: true,
                     altRows: true,
                     autowidth: true,
                     hoverrows: true,
                     height: "auto",
                      postData:{"test":{a:1,b:2}},
                     //width:width,
                     responsive:true,
                     rowNum: 10,
                     rowList: [10, 20, 100],
                     rownumbers: true, // show row numbers
                     rownumWidth: 50, // the width of the row numbers columns
                     caption: "Products / Category Primary Grid View",
                     pager: "#jqGridPager",
            
                 });
                 $('#jqGrid').navGrid('#jqGridPager',
                     // the buttons to appear on the toolbar of the grid
                     { edit: false, add: false, del: false, search:false, refresh: false, view: false, position: "left", cloneToTop: false },
                     // options for the Edit Dialog
                     {
                         editCaption: "The Edit Dialog",
                         recreateForm: true,
                         checkOnUpdate: true,
                         checkOnSubmit: true,
                         closeAfterEdit: true,
                         errorTextFormat: function (data) {
                             return 'Error: ' + data.responseText
                         }
                     },
                     // options for the Add Dialog
                     {
                         closeAfterAdd: true,
                         recreateForm: true,
                         errorTextFormat: function (data) {
                             return 'Error: ' + data.responseText
                         }
                     },
                     // options for the Delete Dailog
                     {
                         errorTextFormat: function (data) {
                             return 'Error: ' + data.responseText
                         }
                     },
                     {
                         multipleSearch: true,
                         showQuery: true
                     } // search options - define multiple search
                 );

             });
         

   </script>
</asp:Content>