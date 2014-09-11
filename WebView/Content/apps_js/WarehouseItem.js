$(document).ready(function () {
    var a;
    $("#detail_panel").hide();

    function ReloadGridListWarehouse() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
            url: base_url + 'MstWarehouse/GetList',
            datatype: "json",
            mtype: 'GET',
            colNames: ['ID', 'Code', 'Name', 'Description', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen: true },
                      { name: 'code', index: 'code', width: 80 },
                      { name: 'name', index: 'name', width: 200},
                      { name: 'description', index: 'description', width: 250 },
                      { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
            ],
            page: '1',
            pager: jQuery('#pager'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#toolbar").width(),
            height: $(window).height() - 190,
            onSelectRow: function (id) {
                var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
                if (id) {
                    $("#list_detail").jqGrid('GridUnload');
                    $("#detail_panel").show();
                    ReloadGridViewItem(id);

                } else {
                    $.messager.alert('Information', 'Data Not Found...!!', 'info');
                };
            },
            gridComplete:
              function () {
              }

        });//END GRID
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
    //    $("#list").setGridParam({ url: base_url + 'MstWarehouse/GetList', page: '1' }).trigger("reloadGrid");

         }

    function ReloadGridListItem() {
        $("#detail_panel").hide();
        jQuery("#list").jqGrid({
            url: base_url + 'MstItem/GetList',
            datatype: "json",
            mtype: 'GET',
            colNames: ['ID', 'SKU', 'Name', 'Item Type Id', 'Item Type Name',
                     'Category', 'UoM Id', 'UoM', 'Quantity',
                     'Selling Price', 'AvgPrice',
                     'Pending Receival', 'Pending Delivery', 'Created At', 'Updated At'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen: true },
                      { name: 'sku', index: 'sku', width: 100 },
                      { name: 'name', index: 'name', width: 200 },
                      { name: 'itemtypeid', index: 'itemtypeid', width: 80, hidden: true },
                      { name: 'itemtype', index: 'itemtype', width: 200 },
                      { name: 'category', index: 'category', width: 100, hidden: true },
                      { name: 'uomid', index: 'uomid', width: 80, hidden: true },
                      { name: 'uom', index: 'uom', width: 100 },
                      { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'sellingprice', index: 'sellingprice', width: 80, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'avgprice', index: 'avgprice', width: 80, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                      { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
            ],
            page: '1',
            pager: jQuery('#pager'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#toolbar").width(),
            height: $(window).height() - 190,
            onSelectRow: function (id) {
                var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
                if (id) {
                    $("#list_detail").jqGrid('GridUnload');
                    $("#detail_panel").show();
                    ReloadGridViewWarehouse(id);
                } else {
                    $.messager.alert('Information', 'Data Not Found...!!', 'info');
                };
            },
            gridComplete:
              function () {
              }

        });//END GRID
        $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list").jqGrid('setFrozenColumns');
    }

    function ReloadGridViewItem(id) {
     
        $("#list_detail").jqGrid({
            url: base_url + 'WarehouseItem/GetListItem?Id=' + id,
            datatype: "json",
            mtype: 'GET',
            colNames: ['Item ID', 'SKU', 'Item Name', 'Item Type Id', 'Item Type Name',
                       'Category', 'Uom Id', 'UoM Name', 'Quantity', 'Pending Delivery', 'Pending Receival'
            ],
            colModel: [
                      { name: 'itemid', index: 'itemid', width: 80, align: "center", frozen: true, hidden: true },
                      { name: 'sku', index: 'sku', width: 80 },
                      { name: 'item', index: 'item', width: 200, frozen: true },
                      { name: 'itemtypeid', index: 'itemtypeid', width: 80, align: "center", hidden: true },
                      { name: 'itemtype', index: 'itemtype', width: 200 },
                      { name: 'category', index: 'category', width: 100, hidden: true },
                      { name: 'uomid', index: 'uomid', width: 100, hidden: true },
                      { name: 'uom', index: 'uom', width: 100 },
                      { name: 'quantity', index: 'quantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                      { name: 'pendingdelivery', index: 'pendingdelivery', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                      { name: 'pendingreceival', index: 'pendingreceival', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
            ],
            page: '1',
            pager: $('#pager_detail'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'itemid',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#detail_toolbar").width(),
            height: $(window).height() - 190,
            gridComplete:
              function () {
              }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list_detail").jqGrid('setFrozenColumns');
    }

    function ReloadGridViewWarehouse(id) {
        
        $("#list_detail").jqGrid({
            url: base_url + 'WarehouseItem/GetListWarehouse?Id=' + id,
            datatype: "json",
            mtype: 'GET',
            colNames: ['ID', 'Code', 'Name', 'Description'],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen: true },
                      { name: 'code', index: 'code', width: 80 },
                      { name: 'name', index: 'name', width: 200 },
                      { name: 'description', index: 'description', width: 250 },
            ],
            page: '1',
            pager: $('#pager_detail'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "asc",
            width: $("#detail_toolbar").width(),
            height: $(window).height() - 190,
            gridComplete:
              function () {
              }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list_detail").jqGrid('setFrozenColumns');
    }


    ReloadGridListWarehouse();

    $('#btn_warehouse').click(function () {
        a = 'warehouse';
        $("#list").jqGrid('GridUnload');
        ReloadGridListWarehouse();
    });
 
    $('#btn_item').click(function () {
        a = 'item';
        $("#list").jqGrid('GridUnload');
        ReloadGridListItem();
    });


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstWarehouse/GetList', //'PaymentVoucher/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Name', 'Description', 'Created At', 'Updated At'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: "center" },
                  { name: 'code', index: 'code', width: 80 },
                  { name: 'name', index: 'name', width: 80 },
                  { name: 'description', index: 'description', width: 250 },
                  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: jQuery('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        onSelectRow: function (id) {
            var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
            if (id) {
                $("#list_detail").setGridParam({ url: base_url + 'WarehouseItem/GetListItem?Id=' + id, postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
            } else {
                $.messager.alert('Information', 'Data Not Found...!!', 'info');
            };
        },
        gridComplete:
          function () {
          }

    });//END GRID
    $("#list").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: true })
              .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        if (a == 'item') {
            $("#list").jqGrid('GridUnload');
            ReloadGridListItem();
        }
        else {
            $("#list").jqGrid('GridUnload');
            ReloadGridListWarehouse();
        }
        
    });

    $('#detail_btn_reload').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (a == 'item') {
            $("#list_detail").jqGrid('GridUnload');
            ReloadGridViewWarehouse(id);
        }
        else {
            $("#list_detail").jqGrid('GridUnload');
            ReloadGridViewItem(id);

        }

    });
    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    //GRID Detail+++++++++++++++
   
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

}); //END DOCUMENT READY