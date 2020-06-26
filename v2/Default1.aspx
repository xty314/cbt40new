<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default1.aspx.cs" Inherits="v2_Default"  %>
<!DOCTYPE html>

<html lang="en">
<head>
 
    <script src="/asset/plugins/jqgrid/js/jquery-1.11.0.min.js"></script>
    <script src="/asset/plugins/jqgrid/js/i18n/grid.locale-en.js"></script>
    <script src="/asset/plugins/jqgrid/js/jquery.jqGrid.min.js"></script>
    <!-- This is the localization file of the grid controlling messages, labels, etc.
    <!-- A link to a jQuery UI ThemeRoller theme, more than 22 built-in and many more custom -->
          <link href="adminLTE/css/adminlte.min.css" rel="stylesheet" />
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css"> 
    <!-- The link to the CSS that the grid needs -->
   
    <link href="/asset/plugins/jqgrid/css/ui.jqgrid-bootstrap.css" rel="stylesheet" />

   
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/js/bootstrap.min.js"></script>
    <meta charset="utf-8" />
    <title>jqGrid Loading Data - Million Rows from a REST service</title>
</head>
<body>
<div style="margin-left:20px">
    <table id="jqGrid"></table>
    <div id="jqGridPager"></div>
</div>
    <script type="text/javascript"> 
        $(document).ready(function () {
            $.jgrid.styleUI.Bootstrap.base.rowTable = "table table-bordered table-striped";
            $("#jqGrid").jqGrid({
                url: '/ajax/eprice.ashx',
                mtype: "GET",
                styleUI: 'Bootstrap',
                datatype: "json",
                colModel: [
                    { label: 'code', name: 'id', sorttype: 'integer' },
                    { label: 'name', name: 'name',width:"300"},
                    { label: 'supplier_code', name: 'supplier_code' },
                    { label: 'barcode', name: 'barcode' },
                    { label: 'code', name: 'id', key: true},
                    { label: 'name_cn', name: 'name_cn' },
                    { label: 'is_website_item', name: 'is_website_item', formatter: 'checkbox'},
                  
                ],
      
                viewrecords: true,
                height: "auto",
                width: "auto",
                rowNum: 10,
                rowList: [10, 20, 100],
                rownumbers: true, // show row numbers
                rownumWidth: 50, // the width of the row numbers columns
                caption: "Products / Category Primary Grid View",
                pager: "#jqGridPager",
            });
        });

   </script>

    
</body>
</html>