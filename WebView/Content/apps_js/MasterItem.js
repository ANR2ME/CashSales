$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstItem/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#lookup_div_itemtype").dialog('close');
    $("#lookup_div_uom").dialog('close');
    $("#delete_confirm_div").dialog('close');


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstItem/GetList',
        datatype: "json",
        colNames: ['ID', 'Name', 'Item Type Id', 'Item Type Name', 'SKU',
                   'Category', 'UoM Id','UoM','Quantity',
                   'Selling Price', 'AvgPrice', 'Margin',
                   'Pending Receival', 'Pending Delivery', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
				  { name: 'name', index: 'name', width: 100 },
                  { name: 'itemtypeid', index: 'itemtypeid', width: 80, hidden: true },
                  { name: 'itemtype', index: 'itemtype', width: 100 },
                  { name: 'sku', index: 'sku', width: 100 },
                  { name: 'category', index: 'category', width: 100 },
                  { name: 'uomid', index: 'uomid', width: 80, hidden: true },
                  { name: 'uom', index: 'uom', width: 100 },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'sellingprice', index: 'sellingprice', width: 80, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'avgprice', index: 'avgprice', width: 80, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'margin', index: 'margin', width: 80, formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true});

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        vStatusSaving = 0; //add data mode	
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "MstItem/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.model.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#Name').val(result.Name);
                            $('#SKU').val(result.Sku);
                            $('#Category').val(result.Category);
                            $('#ItemTypeId').val(result.ItemTypeId);
                            $('#ItemTypeName').val(result.ItemType);
                            $('#UoMId').val(result.UoMId);
                            $('#UoMName').val(result.UoM);
                            $('#Quantity').numberbox('setValue', (result.Quantity));
                            $('#SellingPrice').numberbox('setValue', (result.SellingPrice));
                            $('#Margin').numberbox('setValue', (result.Margin));
                            //$('#PendingDelivery').numberbox('setValue', (result.PendingDelivery));
                            //$('#PendingReceival').numberbox('setValue', (result.PendingReceival));
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    $('#btn_del').click(function () {
        clearForm("#frm");

        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#delete_confirm_btn_submit').data('Id', ret.id);
            $("#delete_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_btn_submit').val('');
        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {

        $.ajax({
            url: base_url + "MstItem/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Id'),
            }),
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                    $("#delete_confirm_div").dialog('close');
                }
                else {
                    ReloadGrid();
                    $("#delete_confirm_div").dialog('close');
                }
            }
        });
    });

    $('#form_btn_cancel').click(function () {
        vStatusSaving = 0;
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'MstItem/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstItem/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Name: $("#Name").val(), ItemTypeId: $("#ItemTypeId").val(), SellingPrice : $("#SellingPrice").val(),
                Sku: $("#SKU").val(), Category: $("#Category").val(), UoMId: $("#UoMId").val(), Margin: $("#Margin").val()
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    ReloadGrid();
                    $("#form_div").dialog('close')
                }
            }
        });
    });

    function clearForm(form) {

        $(':input', form).each(function () {
            var type = this.type;
            var tag = this.tagName.toLowerCase(); // normalize case
            if (type == 'text' || type == 'password' || tag == 'textarea')
                this.value = "";
            else if (type == 'checkbox' || type == 'radio')
                this.checked = false;
            else if (tag == 'select')
                this.selectedIndex = 0;
        });
    }

    // -------------------------------------------------------Look Up Itemtype-------------------------------------------------------
    $('#btnItemType').click(function () {

        var lookUpURL = base_url + 'MstItemType/GetList';
        var lookupGrid = $('#lookup_table_itemtype');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_itemtype').dialog('open');
    });

    $("#lookup_table_itemtype").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description'],
        colModel: [
                  { name: 'id', index: 'id', width: 50, align: 'right' },
                  { name: 'name', index: 'name', width: 150 },
                  { name: 'address', index: 'address', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_itemtype'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_itemtype").width() - 10,
        height: $("#lookup_div_itemtype").height() - 110,
    });
    $("#lookup_table_itemtype").jqGrid('navGrid', '#lookup_toolbar_itemtype', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_itemtype').click(function () {
        $('#lookup_div_itemtype').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_itemtype').click(function () {
        var id = jQuery("#lookup_table_itemtype").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_itemtype").jqGrid('getRowData', id);

            $('#ItemTypeId').val(ret.id).data("kode", id);
            $('#ItemTypeName').val(ret.name);

            $('#lookup_div_itemtype').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    
    // ---------------------------------------------End Lookup ItemType----------------------------------------------------------------

    // -------------------------------------------------------Look Up uom-------------------------------------------------------
    $('#btnUoM').click(function () {
        var lookUpURL = base_url + 'MstUoM/GetList';
        var lookupGrid = $('#lookup_table_uom');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_uom').dialog('open');
    });

    jQuery("#lookup_table_uom").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_uom'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_uom").width() - 10,
        height: $("#lookup_div_uom").height() - 110,
    });
    $("#lookup_table_uom").jqGrid('navGrid', '#lookup_toolbar_uom', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_uom').click(function () {
        $('#lookup_div_uom').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_uom').click(function () {
        var id = jQuery("#lookup_table_uom").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_uom").jqGrid('getRowData', id);

            $('#UoMId').val(ret.id).data("kode", id);
            $('#UoMName').val(ret.name);

            $('#lookup_div_uom').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup uom----------------------------------------------------------------

}); //END DOCUMENT READY