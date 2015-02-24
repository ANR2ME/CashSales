$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'ReceiptVoucher/GetList', postData: { filters: null }, page: 1 }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#listdetail").setGridParam({ url: base_url + 'ReceiptVoucher/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
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
    $("#reconcile_div").dialog('close');
    $("#form_div").dialog('close');
    $("#lookup_div_item").dialog('close');
    $("#lookup_div_cashbank").dialog('close');
    $("#lookup_div_receivable").dialog('close');
    $("#lookup_div_contact").dialog('close');
    $("#delete_confirm_div").dialog('close');


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'ReceiptVoucher/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Contact Id', 'Contact Name', 'Total Amount', 'CashBank Id', 'CashBank Name', 'Is Bank',
                   'Is GBCH', 'Due Date', 'Is Reconciled', 'ReconciliationDate',
                    'Is Confirmed', 'Confirmation Date', 'Receipt Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 50, align: "center" },
                  { name: 'code', index: 'code', width: 100 },
				  { name: 'contactid', index: 'contactid', width: 100, hidden:true },
                  { name: 'contact', index: 'contact', width: 100 },
                  { name: 'totalamount', index: 'totalamount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'cashbankid', index: 'cashbankid', width: 100, hidden:true },
                  { name: 'cashbank', index: 'cashbank', width: 100 },
                  { name: 'isbank', index: 'isbank', width: 100, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
                  { name: 'isgbch', index: 'isgbch', width: 100, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
                  { name: 'duedate', index: 'duedate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isreconciled', index: 'isreconciled', width: 100, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
                  { name: 'reconciliationdate', index: 'reconciliationdate', hidden:true,search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'Y/m/d' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
                  { name: 'confirmationdate', index: 'confirmationdate', hidden:true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'Y/m/d' } },
                  { name: 'receiptdate', index: 'receiptdate', width: 100, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		              rowIsConfirmed = "YES, " + $(this).getRowData(cl).confirmationdate;
		          } else {
		              rowIsConfirmed = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isconfirmed: rowIsConfirmed });

		          rowIsGBCH = $(this).getRowData(cl).isgbch;
		          if (rowIsGBCH == 'true') {
		              rowIsGBCH = "YES";
		          } else {
		              rowIsGBCH = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isgbch: rowIsGBCH });

		          rowIsBank = $(this).getRowData(cl).isbank;
		          if (rowIsBank == 'true') {
		              rowIsBank = "YES";
		          } else {
		              rowIsBank = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isbank: rowIsBank });

		          rowIsReconciled = $(this).getRowData(cl).isreconciled;
		          if (rowIsReconciled == 'true') {
		              rowIsReconciled = "YES, " + $(this).getRowData(cl).reconciliationdate;
		          } else {
		              rowIsReconciled = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isreconciled: rowIsReconciled });
		      }
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });



    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        //window.open(base_url + 'Print_Forms/Printmstbank.aspx');
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            window.open(base_url + "Report/ReportReceiptVoucher?Id=" + id);
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        $('#TotalAmount').numberbox('setValue', '');
        $('#ReceiptDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#DueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        document.getElementById("IsGBCH").selectedIndex = 1;
        document.getElementById("IsBank").selectedIndex = 1;
        $('#btnContact').removeAttr('disabled');
        $('#btnCashBank').removeAttr('disabled');
        //$('#TotalAmount').removeAttr('disabled');
        $('#IsGBCH').removeAttr('disabled');
        $('#IsBank').removeAttr('disabled');
        $('#tabledetail_div').hide();
        $('#ReceiptDateDiv').show();
        $('#ReceiptDateDiv2').hide();
        $('#DueDateDiv').show();
        $('#DueDateDiv2').hide();
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
                url: base_url + "ReceiptVoucher/GetInfo?Id=" + id,
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
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#CashBankId').val(result.CashBankId);
                            $('#CashBank').val(result.CashBank);
                            $('#TotalAmount').numberbox('setValue', result.TotalAmount);
                            var e = document.getElementById("IsGBCH");
                            if (result.IsGBCH == true) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            var f = document.getElementById("IsBank");
                            if (result.IsBank == true) {
                                f.selectedIndex = 0;
                            }
                            else {
                                f.selectedIndex = 1;
                            }
                            $('#ReceiptDate').datebox('setValue', dateEnt(result.ReceiptDate));
                            $('#ReceiptDate2').val(dateEnt(result.ReceiptDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#ReceiptDateDiv2').show();
                            $('#ReceiptDateDiv').hide();
                            $('#DueDateDiv2').show();
                            $('#DueDateDiv').hide();
                            $('#form_btn_save').hide();
                            $('#btnContact').attr('disabled', true);
                            $('#btnCashBank').attr('disabled', true);
                            $('#TotalAmount').attr('disabled', true);
                            $('#IsGBCH').attr('disabled', true);
                            $('#IsBank').attr('disabled', true);
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
                url: base_url + "ReceiptVoucher/GetInfo?Id=" + id,
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
                            $('#ContactId').val(result.ContactId);
                            $('#Contact').val(result.Contact);
                            $('#CashBankId').val(result.CashBankId);
                            $('#CashBank').val(result.CashBank);
                            $('#TotalAmount').numberbox('setValue', result.TotalAmount);
                            var e = document.getElementById("IsGBCH");
                            if (result.IsGBCH == true) {
                                e.selectedIndex = 0;
                            }
                            else {
                                e.selectedIndex = 1;
                            }
                            var f = document.getElementById("IsBank");
                            if (result.IsBank == true) {
                                f.selectedIndex = 0;
                            }
                            else {
                                f.selectedIndex = 1;
                            }
                            $('#ReceiptDate').datebox('setValue', dateEnt(result.ReceiptDate));
                            $('#ReceiptDate2').val(dateEnt(result.ReceiptDate));
                            $('#DueDate').datebox('setValue', dateEnt(result.DueDate));
                            $('#DueDate2').val(dateEnt(result.DueDate));
                            $('#ReceiptDateDiv2').show();
                            $('#ReceiptDateDiv').hide();
                            $('#DueDateDiv2').show();
                            $('#DueDateDiv').hide();
                            //$('#form_btn_save').hide();
                            $('#btnContact').removeAttr('disabled');
                            $('#btnCashBank').removeAttr('disabled');
                            //$('#TotalAmount').removeAttr('disabled');
                            $('#IsGBCH').removeAttr('disabled');
                            $('#IsBank').removeAttr('disabled');
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
            //$('#ConfirmationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#ConfirmationDate').datebox('setValue', ret.receiptdate);
            $('#idconfirm').val(ret.id);
            $('#confirmCode').val(ret.code);
            $('#confirmTotalAmount').numberbox('setValue', ret.totalamount);
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
                        url: base_url + "ReceiptVoucher/Unconfirm",
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
        $.ajax({
            url: base_url + "ReceiptVoucher/Confirm",
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
                    ReloadGrid();
                    $("#confirm_div").dialog('close');
                }
            }
        });
    });

    $('#confirm_btn_cancel').click(function () {
        $('#confirm_div').dialog('close');
    });

    $('#btn_reconcile').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $('#ConfirmationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#idreconcile').val(ret.id);
            $('#reconcileCode').val(ret.code);
            $('#reconcileTotalAmount').numberbox('setValue', ret.totalamount);
            $("#reconcile_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#btn_unreconcile').click(function () {
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list").jqGrid('getRowData', id);
            $.messager.reconcile('Confirm', 'Are you sure you want to unreconcile record?', function (r) {
                if (r) {
                    $.ajax({
                        url: base_url + "ReceiptVoucher/Unreconcile",
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
                                $("#delete_reconcile_div").dialog('close');
                            }
                        }
                    });
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#reconcile_btn_submit').click(function () {
        ClearErrorMessage();
        $.ajax({
            url: base_url + "ReceiptVoucher/Reconcile",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#idreconcile').val(), ReconciliationDate: $('#ReconciliationDate').datebox('getValue'),
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
                    $("#reconcile_div").dialog('close');
                }
            }
        });
    });

    $('#reconcile_btn_cancel').click(function () {
        $('#reconcile_div').dialog('close');
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
            url: base_url + "ReceiptVoucher/Delete",
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
        if ($('#ReceiptDate').datebox('getValue') == "") {
            return $($('#ReceiptDate').addClass('errormessage').before('<span class="errormessage">**' + "Receipt Date Belum Terisi" + '</span>'));

        }
        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'ReceiptVoucher/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'ReceiptVoucher/Insert';
        }

        var e = document.getElementById("IsGBCH");
        var gbch = e.options[e.selectedIndex].value;
        var f = document.getElementById("IsBank");
        var bank = f.options[f.selectedIndex].value;

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ContactId: $("#ContactId").val(), CashBankId: $("#CashBankId").val(),
                IsGBCH: gbch, IsBank: bank, TotalAmount: $("#TotalAmount").numberbox('getValue'),
                ReceiptDate: $('#ReceiptDate').datebox('getValue'), DueDate: $('#DueDate').datebox('getValue'),
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
        colNames: ['Code', 'Receivable Id', 'Receivable Code', 'Amount', 'Description'
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 100, sortable: false },
                  { name: 'receivableid', index: 'receivableid', width: 130, sortable: false, hidden: true },
                  { name: 'receivable', index: 'receivable', width: 130, sortable: false },
                  { name: 'amount', index: 'amount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
                  { name: 'description', index: 'description', width: 200, sortable: false }
        ],
        page: '1',
        pager: $('#pagerdetail'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Code',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $(window).width() - 700,
        height: $(window).height() - 500,
        gridComplete:
		  function () {
		  }
    });//END GRID Detail
    $("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: true })
                    .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    $('#btn_add_new_detail').click(function () {
        ClearData();
        clearForm('#item_div');
        $('#Amount').numberbox('setValue', '');
        $('#item_div').dialog('open');
    });

    $('#btn_edit_detail').click(function () {
        ClearData();
        clearForm("#item_div");
        var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "ReceiptVoucher/GetInfoDetail?Id=" + id,
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
                            $('#ReceivableId').val(result.ReceivableId);
                            $('#Receivable').val(result.Receivable);
                            $('#detailCode').val(result.Code);
                            $('#Amount').numberbox('setValue', result.Amount);
                            $('#Description').val(result.Description);
                            $('#ReceiptVoucherDetailId').val(result.ReceiptVoucherDetailId);
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
                        url: base_url + "ReceiptVoucher/DeleteDetail",
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
                                $('#TotalAmount').val(result.TotalAmount);
                                ReloadGridDetail();
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
    //--------------------------------------------------------Dialog Item-------------------------------------------------------------
    // item_btn_submit

    $("#item_btn_submit").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#item_btn_submit").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'ReceiptVoucher/UpdateDetail';
        }
            // Insert
        else {
            submitURL = base_url + 'ReceiptVoucher/InsertDetail';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ReceiptVoucherId: $("#id").val(), ReceivableId: $("#ReceivableId").val(), Description: $("#Description").val(),
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
                    $('#TotalAmount').val(result.TotalAmount);
                    ReloadGridDetail();
                    ReloadGrid();
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
        colNames: ['ID', 'Name', 'Address', 'Contact No.', 'PIC', 'PIC Contact No.', 'Email'
        ],
        colModel: [
    			  { name: 'id', index: 'id', hidden: true, width: 50, align: "center" },
                  { name: 'name', index: 'name', width: 100 },
                  { name: 'address', index: 'address', width: 250 },
                  { name: 'contactno', index: 'contactno', width: 150 },
                  { name: 'pic', index: 'pic', width: 180 },
                  { name: 'piccontactno', index: 'piccontactno', width: 150 },
                  { name: 'email', index: 'email', width: 180 },
        ],
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
    $("#lookup_table_contact").jqGrid('navGrid', '#lookup_toolbar_contact', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

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

    // -------------------------------------------------------Look Up cashbank-------------------------------------------------------
    $('#btnCashBank').click(function () {
        var lookUpURL = base_url + 'MstCashBank/GetList';
        var lookupGrid = $('#lookup_table_cashbank');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_cashbank').dialog('open');
    });

    jQuery("#lookup_table_cashbank").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['ID', 'Name', 'Description', 'Amount',
        ],
        colModel: [
    			  { name: 'id', index: 'id', hidden: true, width: 50, align: "center" },
                  { name: 'name', index: 'name', width: 100 },
                  { name: 'description', index: 'description', width: 250 },
                  { name: 'amount', index: 'amount', width: 150, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
        ],
        page: '1',
        pager: $('#lookup_pager_cashbank'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_cashbank").width() - 10,
        height: $("#lookup_div_cashbank").height() - 110,
    });
    $("#lookup_table_cashbank").jqGrid('navGrid', '#lookup_toolbar_cashbank', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_cashbank').click(function () {
        $('#lookup_div_cashbank').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_cashbank').click(function () {
        var id = jQuery("#lookup_table_cashbank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_cashbank").jqGrid('getRowData', id);

            $('#CashBankId').val(ret.id).data("kode", id);
            $('#CashBank').val(ret.name);
            $('#lookup_div_cashbank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup cashbank----------------------------------------------------------------

    // -------------------------------------------------------Look Up receivable-------------------------------------------------------
    $('#btnReceivable').click(function () {
        var lookUpURL = base_url + 'ReceiptVoucher/GetListReceivable';
        var lookupGrid = $('#lookup_table_receivable');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_receivable').dialog('open');
    });

    jQuery("#lookup_table_receivable").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Contact Id', 'Contact Name', 'Receivable Source', 'Source Id', 'Source Code',
                   'Amount', 'Remaining Amount', 'PendingClearanceAmount', 'Allowance Amount', 'Due Date',
        ],
        colModel: [
                  { name: 'code', index: 'code', width: 100 },
                  { name: 'contactid', index: 'contactid', hidden:true, width: 100 },
                  { name: 'contact', index: 'contact', width: 100 },
                  { name: 'receivablesource', index: 'receivablesource', width: 100 },
                  { name: 'receivablesourceid', index: 'receivablesourceid', hidden: true, width: 100 },
                  { name: 'receivablesourcecode', index: 'receivablesourcecode', width: 100 },
                  { name: 'amount', index: 'amount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'remainingamount', index: 'remainingamount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'pendingclearanceamount', index: 'pendingclearanceamount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'allowanceamount', index: 'allowanceamount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'duedate', index: 'duedate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: $('#lookup_pager_receivable'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#lookup_div_receivable").width() - 10,
        height: $("#lookup_div_receivable").height() - 110,
    });
    $("#lookup_table_receivable").jqGrid('navGrid', '#lookup_toolbar_receivable', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_receivable').click(function () {
        $('#lookup_div_receivable').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_receivable').click(function () {
        var id = jQuery("#lookup_table_receivable").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_receivable").jqGrid('getRowData', id);
            $('#ReceivableId').val(id);
            $('#Receivable').val(ret.code);
            $('#Amount').numberbox('setValue', ret.remainingamount);

            $('#lookup_div_receivable').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup receivable----------------------------------------------------------------


}); //END DOCUMENT READY