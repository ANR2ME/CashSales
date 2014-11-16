$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'Memorial/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'Memorial/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#form_btn_save').data('kode', '');
        $('#item_btn_submit').data('kode', '');
        ClearErrorMessage();
    }

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

    $("#item_div").dialog('close');
    $("#confirm_div").dialog('close');
    $("#form_div").dialog('close');
    $("#lookup_div_account").dialog('close');
    $("#delete_confirm_div").dialog('close');
    $("#AccountId").hide();

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Memorial/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Description', 'Amount', 'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 70 },
                  { name: 'description', index: 'description', width: 150 },
                  { name: 'amount', index: 'amount', width: 100, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, hidden :true },
                  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 120, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updatedat', index: 'updatedat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsConfirmed = $(this).getRowData(cl).isconfirmed;
		          if (rowIsConfirmed == 'true') {
		              rowIsConfirmed = "Y";
		          } else {
		              rowIsConfirmed = "N";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isconfirmed: rowIsConfirmed });
		      }
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
        $('#TotalAmount').numberbox('clear');
        $('#Description').removeAttr('disabled');
        $('#TotalAmount').removeAttr('disabled');
        $('#tabledetail_div').hide();
        $('#form_btn_save').show();
        $('#form_div').dialog('open');
    });

    $('#btn_add_detail').click(function () {
        ClearData();
        clearForm('#frm');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "Memorial/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#Code').val(result.Code);
                            $('#Description').val(result.Description);
                            $('#TotalAmount').numberbox('setValue', result.Amount);
                            $('#form_btn_save').hide();
                            $('#btnAccount').removeAttr('disabled');
                            $('#Description').attr('disabled', true);
                            $('#TotalAmount').attr('disabled', true);
                            $('#tabledetail_div').show();
                            ReloadGridDetail();
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "Memorial/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#Code').val(result.Code);
                            $('#Description').val(result.Description);
                            $('#TotalAmount').numberbox('setValue', result.TotalAmount);
                            $('#Description').removeAttr('disabled');
                            $('#TotalAmount').removeAttr('disabled');
                            $('#tabledetail_div').hide();
                            $('#form_btn_save').show();
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_confirm').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#ConfirmationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idconfirm').val(ret.id);
            $("#confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_unconfirm').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to unconfirm record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "Memorial/Unconfirm",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: id,
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
                            }
                            else {
                                ReloadGrid();
                                $("#delete_confirm_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#confirm_btn_submit').click(function () {
        ClearErrorMessage();
        $("#confirm_div").dialog('close');
        $.ajax({
            url: base_url + "Memorial/Confirm",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idconfirm').val(), ConfirmationDate: $('#ConfirmationDate').datebox('getValue'),
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
                }
                else {
                    //$("#confirm_div").dialog('close');
                    ReloadGrid();
                }
            }
        });
    });

    $('#confirm_btn_cancel').click(function () {
        $('#confirm_div').dialog('close');
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
            url: base_url + "Memorial/Delete",
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
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#id").val();

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'Memorial/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'Memorial/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, Description: $("#Description").val(), Amount : $("#TotalAmount").numberbox('getValue'),
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

    //GRID Detail+++++++++++++++
    $("#listdetail").jqGrid({
        url: base_url,
        datatype: "json",
        colNames: ['Code', 'Account Id', 'Account Code', 'Account', 'Status', 'Amount'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 70, sortable: false },
                  { name: 'accountid', index: 'accountid', width: 130, sortable: false, hidden: true },
                  { name: 'accountcode', index: 'accountcode', width: 80, sortable: false },
                  { name: 'account', index: 'account', width: 150, sortable: false },
                  { name: 'status', index: 'status', width: 40, sortable: false },
                  { name: 'amount', index: 'amount', width: 100, align: 'right', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
        ],
        //page: '1',
        //pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Code',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#form_div").width() - 3,
        height: $(window).height() - 500,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowStatus= $(this).getRowData(cl).status;
		          if (rowStatus == 1) {
		              rowStatus = "Debit";
		          } else {
		              rowStatus = "Credit";
		          }
		          $(this).jqGrid('setRowData', ids[i], { status: rowStatus });
		      }
		  }
    });//END GRID Detail
    $("#listdetail").jqGrid('navGrid', '#pagerdetail', { del: false, add: false, edit: false, search: false });
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#Amount').numberbox('clear');
        $('#item_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "Memorial/GetInfoDetail?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#item_btn_submit").data('kode', result.Id);
                            $('#btnAccount').removeAttr('disabled');
                            $('#AccountId').val(result.AccountId);
                            $('#Account').val(result.Name);
                            $('#Amount').numberbox('setValue', result.Amount);
                            var e = document.getElementById("Status");
                            if (result.Status == 1) {
                                e.selectedIndex = 0;
                            }
                            else if (result.Status == 2){
                                e.selectedIndex = 1;
                            }
                            $('#Description').val(result.Description);
                            $('#MemorialDetailId').val(result.Id);
                            $('#item_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_del_detail').click(function () {
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#listdetail").jqGrid('getRowData', id);
            $.messager.confirm('Confirm', 'Are you sure you want to delete record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "Memorial/DeleteDetail",
                        type: "POST",
                        contentType: "application/json",
                        data: JSON.stringify({
                            Id: id,
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
                            }
                            else {
                                ReloadGridDetail();
                                $("#delete_confirm_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });
    //--------------------------------------------------------Dialog Item-------------------------------------------------------------
    // item_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'Memorial/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'Memorial/InsertDetail';
        }

        var e = document.getElementById("Status");
        var status = e.selectedIndex + 1;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, MemorialId: $("#id").val(), AccountId: $("#AccountId").val(), Status: status,
                Amount: $("#Amount").numberbox('getValue'),
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
                    ReloadGridDetail();
                    $("#item_div").dialog('close')
                }
            }
        });
    });


    // item_btn_cancel
    $('#item_btn_cancel').click(function () {
        clearForm('#item_div');
        $("#item_div").dialog('close');
    });
    //--------------------------------------------------------END Dialog Item-------------------------------------------------------------

    // -------------------------------------------------------Look Up account-------------------------------------------------------
    $('#btnAccount').click(function () {
        var lookUpURL = base_url + 'ChartOfAccount/GetLeaves';
        var lookupGrid = $('#lookup_table_account');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_account').dialog('open');
    });

    jQuery("#lookup_table_account").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Account Code', 'Account Name', 'Group', 'Level', 'Parent ID', 'Parent Code', 'Parent Name', 'Legacy','CashBank', 'Legacy Code'],
        colModel: [
				  { name: 'Id', index: 'Id', width: 40, hidden: true},
				  { name: 'Code', index: 'Code', width: 80, classes: "grid-col" },
				  { name: 'name', index: 'name', width: 250 },
                  { name: 'group', index: 'group', width: 90 },
                  { name: 'level', index: 'level', width: 50 },
                  { name: 'parentid', index: 'parentid', width: 40, hidden: true },
                  { name: 'parentcode', index: 'parentid', width: 80, classes: "grid-col" },
                  { name: 'parent', index: 'parent', width: 80 },
                  { name: 'islegacy', index: 'islegacy', width: 40, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
                  { name: 'iscashbankaccount', index: 'iscashbankaccount', width: 60, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
                  { name: 'legacycode', index: 'legacycode', width: 80, hidden: true },
        ],
        page: '1',
        pager: $('#lookup_pager_account'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_account").width() - 10,
        height: $("#lookup_div_account").height() - 110,
        gridComplete:
         function () {
             var ids = $(this).jqGrid('getDataIDs');
             for (var i = 0; i < ids.length; i++) {
                 var cl = ids[i];
                 rowIsLegacy = $(this).getRowData(cl).islegacy;
                 if (rowIsLegacy == 'true') {
                     rowIsLegacy = "Y";
                 } else {
                     rowIsLegacy = "N";
                 }
                 $(this).jqGrid('setRowData', ids[i], { islegacy: rowIsLegacy });

                 rowIsCashBankAccount = $(this).getRowData(cl).iscashbankaccount;
                 if (rowIsCashBankAccount == 'true') {
                     rowIsCashBankAccount = "Y";
                 } else {
                     rowIsCashBankAccount = "N";
                 }
                 $(this).jqGrid('setRowData', ids[i], { iscashbankaccount: rowIsCashBankAccount });

                 rowIsLeaf = $(this).getRowData(cl).isleaf;
                 if (rowIsLeaf == 'true') {
                     rowIsLeaf = "Y";
                 } else {
                     rowIsLeaf = "N";
                 }
                 $(this).jqGrid('setRowData', ids[i], { isleaf: rowIsLeaf });

                 rowGroup = $(this).getRowData(cl).group;
                 if (rowGroup == 1) {
                     rowGroup = "Asset";
                 } else if (rowGroup == 2) {
                     rowGroup = "Expense";
                 } else if (rowGroup == 3) {
                     rowGroup = "Liability";
                 } else if (rowGroup == 4) {
                     rowGroup = "Equity";
                 } else if (rowGroup == 5) {
                     rowGroup = "Revenue";
                 }
                 $(this).jqGrid('setRowData', ids[i], { group: rowGroup });

             }
         }
    });
    $("#lookup_table_account").jqGrid('navGrid', '#lookup_toolbar_account', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_account').click(function () {
        $('#lookup_div_account').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_account').click(function () {
        var id = jQuery("#lookup_table_account").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_account").jqGrid('getRowData', id);

            $('#AccountId').val(ret.Id).data("kode", id);
            $('#Account').val(ret.name);
            $('#lookup_div_account').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup account----------------------------------------------------------------

}); //END DOCUMENT READY