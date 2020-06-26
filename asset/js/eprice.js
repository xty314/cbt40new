



$(function(){
    console.log(obj)
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
        postData:obj,
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

           

})