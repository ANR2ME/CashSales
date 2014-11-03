$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstBarring/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Description').val('').text('').removeClass('errormessage');
        $('#Name').val('').text('').removeClass('errormessage');
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#lookup_div_machine").dialog('close');
    $("#lookup_div_blanket").dialog('close');
    $("#lookup_div_leftbaritem").dialog('close');
    $("#lookup_div_rightbaritem").dialog('close');
    $("#lookup_div_uom").dialog('close');
    $("#lookup_div_itemtype").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#delete_confirm_div").dialog('close');


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstBarring/GetList',
        datatype: "json",
        colNames: ['ID', 'Name', 'Category', 'ItemType Id','ItemType Name','UoM Id', 'UoM',
                   'Sku', 'Roll No', 'Contact Id','Contact Name',
                   'Machine Id', 'Machine Name', 'Blanket Id', 'Blanket Name',
                   'LeftBar Item Id', 'LeftBar Name', 'RightBar Id', 'RightBar Name',
                   'AC', 'AR', 'Thickness', 'KS', 'Quantity',
                   'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'name', index: 'name', width: 130 },
                  { name: 'category', index: 'category', width: 80 },
                  { name: 'itemtypeid', index: 'itemtypeid', width: 100 },
                  { name: 'itemtypename', index: 'itemtypename', width: 100 },
                  { name: 'uomid', index: 'uomid', width: 100 },
                  { name: 'uom', index: 'uom', width: 100 },
                  { name: 'sku', index: 'sku', width: 80 },
                  { name: 'rollno', index: 'rollno', width: 80 },
                  { name: 'contactid', index: 'contactid', width: 130 },
                  { name: 'contactname', index: 'contactname', width: 130 },
                  { name: 'machineid', index: 'machineid', width: 80 },
                  { name: 'machinename', index: 'machinename', width: 100 },
                  { name: 'blanketid', index: 'blanketid', width: 130 },
                  { name: 'blanketname', index: 'blanketname', width: 130 },
                  { name: 'leftbaritemid', index: 'leftbaritemid', width: 100 },
                  { name: 'leftbaritemname', index: 'leftbaritemname', width: 110 },
                  { name: 'rightbaritemid', index: 'rightbaritemid', width: 110 },
                  { name: 'rightbaritemname', index: 'rightbaritemname', width: 130 },
                  { name: 'ac', index: 'ac', width: 117, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'ar', index: 'ar', width: 117, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'thickness', index: 'thickness', width: 117, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'ks', index: 'ks', width: 117, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'quantity', index: 'quantity', width: 117, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
                url: base_url + "MstBarring/GetInfo?Id=" + id,
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
                            $('#Category').val(result.Category);
                            $('#ItemTypeId').val(result.ItemTypeId);
                            $('#ItemType').val(result.ItemType);
                            $('#UoMId').val(result.UoMId);
                            $('#UoM').val(result.UoM);
                            $('#Sku').val(result.Sku);
                            $('#RollNo').val(result.RollNo);
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#MachineId').val(result.MachineId);
                            $('#Machine').val(result.Machine);
                            $('#BlanketItemId').val(result.BlanketItemId);
                            $('#BlanketItem').val(result.BlanketItem);
                            $('#LeftBarItemId').val(result.LeftBarItemId);
                            $('#LeftBarItem').val(result.LeftBarItem);
                            $('#RightBarItemId').val(result.RightBarItemId);
                            $('#RightBar').val(result.RightBar);
                            $('#AC').numberbox('setValue', (result.AC));
                            $('#AR').numberbox('setValue', (result.AR));
                            $('#thickness').numberbox('setValue', (result.thickness));
                            $('#KS').numberbox('setValue', (result.KS));
                            $('#Quantity').numberbox('setValue', (result.Quantity));
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
            url: base_url + "MstBarring/Delete",
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
            submitURL = base_url + 'MstBarring/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'MstBarring/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ItemTypeId: $("#ItemTypeId").val(), Sku: $("#Sku").val(), Name: $("#Name").val(),
                Category: $("#Category").val(), UoMId: $("#UoMId").val(), RollNo: $("#RollNo").val(),
                ContactId: $("#ContactId").val(), MachineId: $("#MachineId").val(), BlanketItemId: $("#BlanketItemId").val(),
                LeftBarItemId: $("#LeftBarItemId").val(), RightBarItemId: $("#RightBarItemId").val(), AC: $("#AC").val(),
                AR: $("#AR").val(), thickness: $("#thickness").val(), KS: $("#KS").val(),
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
            $('#UoM').val(ret.name);

            $('#lookup_div_uom').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup uom----------------------------------------------------------------

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
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
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


    // -------------------------------------------------------Look Up machine-------------------------------------------------------
    $('#btnMachine').click(function () {
        var lookUpURL = base_url + 'MstMachine/GetList';
        var lookupGrid = $('#lookup_table_machine');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_machine').dialog('open');
    });

    jQuery("#lookup_table_machine").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_machine'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_machine").width() - 10,
        height: $("#lookup_div_machine").height() - 110,
    });
    $("#lookup_table_machine").jqGrid('navGrid', '#lookup_toolbar_machine', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_machine').click(function () {
        $('#lookup_div_machine').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_machine').click(function () {
        var id = jQuery("#lookup_table_machine").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_machine").jqGrid('getRowData', id);

            $('#MachineId').val(ret.id).data("kode", id);
            $('#Machine').val(ret.name);

            $('#lookup_div_machine').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup machine----------------------------------------------------------------

    // -------------------------------------------------------Look Up contact-------------------------------------------------------
    $('#btnContact').click(function () {
        var lookUpURL = base_url + 'MstContact/GetList';
        var lookupGrid = $('#lookup_table_contact');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_contact').dialog('open');
    });

    jQuery("#lookup_table_contact").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right' },
                  { name: 'name', index: 'name', width: 200 }],
        page: '1',
        pager: $('#lookup_pager_contact'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_contact").width() - 10,
        height: $("#lookup_div_contact").height() - 110,
    });
    $("#lookup_table_contact").jqGrid('navGrid', '#lookup_toolbar_contact', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_contact').click(function () {
        $('#lookup_div_contact').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_contact').click(function () {
        var id = jQuery("#lookup_table_contact").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_contact").jqGrid('getRowData', id);

            $('#ContactId').val(ret.id).data("kode", id);
            $('#Contact').val(ret.name);

            $('#lookup_div_contact').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup contact----------------------------------------------------------------

    // -------------------------------------------------------Look Up blanket-------------------------------------------------------
    $('#btnBlanketItem').click(function () {
        var lookUpURL = base_url + 'MstBarring/GetListBlanket';
        var lookupGrid = $('#lookup_table_blanket');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_blanket').dialog('open');
    });

    jQuery("#lookup_table_blanket").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name', 'SKU',
                     'Category', 'UoM Id', 'UoM', 'Quantity', 'Pending Receival', 'Pending Delivery', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'name', index: 'name', width: 100 },
                  { name: 'sku', index: 'sku', width: 100 },
                  { name: 'category', index: 'category', width: 100 },
                  { name: 'uomid', index: 'uomid', width: 80 },
                  { name: 'uom', index: 'uom', width: 100 },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_blanket'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_blanket").width() - 10,
        height: $("#lookup_div_blanket").height() - 110,
    });
    $("#lookup_table_blanket").jqGrid('navGrid', '#lookup_toolbar_blanket', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_blanket').click(function () {
        $('#lookup_div_blanket').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_blanket').click(function () {
        var id = jQuery("#lookup_table_blanket").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_blanket").jqGrid('getRowData', id);

            $('#BlanketItemId').val(ret.id).data("kode", id);
            $('#BlanketItem').val(ret.name);

            $('#lookup_div_blanket').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup blanket----------------------------------------------------------------

    // -------------------------------------------------------Look Up leftbaritem-------------------------------------------------------
    $('#btnLeftBarItem').click(function () {
        var lookUpURL = base_url + 'MstBarring/GetListBar';
        var lookupGrid = $('#lookup_table_leftbaritem');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_leftbaritem').dialog('open');
    });

    jQuery("#lookup_table_leftbaritem").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name', 'SKU',
                     'Category', 'UoM Id', 'UoM', 'Quantity', 'Pending Receival', 'Pending Delivery', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'name', index: 'name', width: 100 },
                  { name: 'sku', index: 'sku', width: 100 },
                  { name: 'category', index: 'category', width: 100 },
                  { name: 'uomid', index: 'uomid', width: 80 },
                  { name: 'uom', index: 'uom', width: 100 },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_leftbaritem'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_leftbaritem").width() - 10,
        height: $("#lookup_div_leftbaritem").height() - 110,
    });
    $("#lookup_table_leftbaritem").jqGrid('navGrid', '#lookup_toolbar_leftbaritem', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_leftbaritem').click(function () {
        $('#lookup_div_leftbaritem').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_leftbaritem').click(function () {
        var id = jQuery("#lookup_table_leftbaritem").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_leftbaritem").jqGrid('getRowData', id);

            $('#LeftBarItemId').val(ret.id).data("kode", id);
            $('#LeftBarItem').val(ret.name);

            $('#lookup_div_leftbaritem').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup leftbaritem----------------------------------------------------------------

    // -------------------------------------------------------Look Up rightbaritem-------------------------------------------------------
    $('#btnRightBarItem').click(function () {
        var lookUpURL = base_url + 'MstBarring/GetListBar';
        var lookupGrid = $('#lookup_table_rightbaritem');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_rightbaritem').dialog('open');
    });

    jQuery("#lookup_table_rightbaritem").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name', 'SKU',
                     'Category', 'UoM Id', 'UoM', 'Quantity', 'Pending Receival', 'Pending Delivery', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'name', index: 'name', width: 100 },
                  { name: 'sku', index: 'sku', width: 100 },
                  { name: 'category', index: 'category', width: 100 },
                  { name: 'uomid', index: 'uomid', width: 80 },
                  { name: 'uom', index: 'uom', width: 100 },
                  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 105, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_rightbaritem'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_rightbaritem").width() - 10,
        height: $("#lookup_div_rightbaritem").height() - 110,
    });
    $("#lookup_table_rightbaritem").jqGrid('navGrid', '#lookup_toolbar_rightbaritem', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    // Cancel or CLose
    $('#lookup_btn_cancel_rightbaritem').click(function () {
        $('#lookup_div_rightbaritem').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_rightbaritem').click(function () {
        var id = jQuery("#lookup_table_rightbaritem").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_rightbaritem").jqGrid('getRowData', id);

            $('#RightBarItemId').val(ret.id).data("kode", id);
            $('#RightBarItem').val(ret.name);

            $('#lookup_div_rightbaritem').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup rightbaritem----------------------------------------------------------------

}); //END DOCUMENT READY