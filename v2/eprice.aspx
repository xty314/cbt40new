<%@ Page Language="C#" AutoEventWireup="true" CodeFile="eprice.aspx.cs" Inherits="v2_eprice" MasterPageFile="layout/MasterPage.master" %>

<%@ Register Src="./layout/ContentHeader.ascx" TagPrefix="uc1" TagName="ContentHeader" %>
<%@Import Namespace="System.Data.SqlClient" %>
<%@Import Namespace ="System.Data" %>
<asp:Content ContentPlaceHolderId="AdditionalCSS" runat="server">
    <%--<link href="plugins/jqgrid/css/octicons.css" rel="stylesheet" />--%>
    	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/octicons/4.4.0/font/octicons.css">
    <link href="plugins/jqgrid/css/ui.jqgrid-bootstrap4.css" rel="stylesheet" />

    <link href="plugins/jqgrid/css/responsive.css" rel="stylesheet" />
</asp:Content>

<asp:Content ContentPlaceHolderId="Header" runat="server">

    <uc1:ContentHeader runat="server" ID="ContentHeader"  />

</asp:Content>
<asp:Content ContentPlaceHolderId="Content" runat="server">
   <!-- Main content -->
    <section class="content">
      <div class="card">
        <div class="card-header">
            <div class="row">      
                <div class="form-group col-12 col-sm-2">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
                 <div class="form-group col-12 col-sm-2">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
                 <div class="form-group col-12 col-sm-2">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
                 <div class="form-group col-12 col-sm-2">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
                 <div class="form-group col-12 col-sm-2">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
                 <div class="form-group col-12 col-sm-2">
                    <label for="exampleInputEmail1">Email address</label>
                    <input type="email" class="form-control" id="exampleInputEmail1" placeholder="Enter email">
                  </div>
            </div>
  
        </div>
        <!-- /.card-header -->
        <div class="card-body">
           <table id="jqGrid"></table>
            <div id="jqGridPager"></div>
        </div>
        <!-- /.card-body -->
      </div>
      <!-- /.card -->
    </section>
    <!-- /.content -->

</asp:Content>

<asp:Content ContentPlaceHolderId="AdditionalJS" runat="server">
    <script src="plugins/jqgrid/js/i18n/grid.locale-en.js"></script>
     <script src="plugins/jqgrid/js/jquery.jqGrid.min.js"></script>
         <script type="text/javascript"> 
             $(document).ready(function () {
                 //$(window).resize(function () {
                 //    var outerwidth = $('#grid').width();
                 //    $('#list').setGridWidth(outerwidth); // setGridWidth method sets a new width to the grid dynamically
                 //});
                 $(window).on("resize", function () {
                     var $grid = $("#jqGrid"),
                         newWidth = $grid.closest(".ui-jqgrid").parent().width();
                     $grid.jqGrid("autoWidth", false);
                 });
                 //$.jgrid.defaults.responsive = true;
                 $.jgrid.defaults.styleUI = 'Bootstrap4';
                 $.jgrid.defaults.iconSet = "Octicons";
                 $("#jqGrid").jqGrid({
                     url: '/ajax/eprice.ashx',
                     mtype: "GET",
                     
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