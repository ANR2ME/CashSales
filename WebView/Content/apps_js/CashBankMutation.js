$(document).ready(function () {

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'CashBankMutation/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#id').val('');
        $('#Code').val('');
        $('#SourceCashBankId').val('');
        $('#SourceCashBank').val('');
        $('#TargetCashBankId').val('');
        $('#TargetCashBank').val('');
        $('#Amount').numberbox('setValue', '');
        $('#form_btn_save').data('kode', '');
        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $("#lookup_div_sourcecashbank").dialog('close');
    $("#lookup_div_targetcashbank").dialog('close');
    $("#confirm_div").dialog('close');
    $("#delete_confirm_div").dialog('close');


    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'CashBankMutation/GetList',
        datatype: "json",
        colNames: ['ID', 'Code', 'Source CashBank Id', 'Source CashBank Name', 'Target CashBank Id', 'Target CashBank Name',
                   'Amount', 'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'code', index: 'code', width: 100 },
                  { name: 'sourcecashbankid', index: 'sourcecashbankid', hidden: true, width: 150, align: "center" },
                  { name: 'sourcecashbank', index: 'sourcecashbank', width: 150 },
                  { name: 'targetcashbankid', index: 'targetcashbankid', hidden: true, width: 150, align: "center" },
                  { name: 'targetcashbank', index: 'targetcashbank', width: 150 },
                  { name: 'amount', index: 'amount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 100, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
                  { name: 'confirmationdate', index: 'confirmationdate', hidden:true, width: 150, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'Y/m/d' } },
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
		       }
		   }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

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
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "CashBankMutation/GetInfo?Id=" + id,
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
                            $('#SourceCashBankId').val(result.SourceCashBankId);
                            $('#SourceCashBank').val(result.SourceCashBank);
                            $('#TargetCashBankId').val(result.TargetCashBankId);
                            $('#TargetCashBank').val(result.TargetCashBank);
                            $('#Amount').numberbox('setValue', (result.Amount));
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
            $('#confirmCode').val(ret.code);
            $('#confirmSourceCashBankId').val(ret.sourcecashBankid);
            $('#confirmSourceCashBank').val(ret.sourcecashbank);
            $('#confirmTargetCashBankId').val(ret.targetcashbankid);
            $('#confirmTargetCashBank').val(ret.targetcashbank);
            $('#confirmAmount').numberbox('setValue', (ret.amount));
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
                        url: base_url + "CashBankMutation/Unconfirm",
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
            url: base_url + "CashBankMutation/Confirm",
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
            url: base_url + "CashBankMutation/Delete",
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
            submitURL = base_url + 'CashBankMutation/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'CashBankMutation/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, SourceCashBankId: $("#SourceCashBankId").val(), TargetCashBankId: $("#TargetCashBankId").val(),
                Amount: $("#Amount").numberbox('getValue')
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

    // -------------------------------------------------------Look Up SourceCashBank-------------------------------------------------------
    $('#btnSourceCashBank').click(function () {

        var lookUpURL = base_url + 'MstCashBank/GetList';
        var lookupGrid = $('#lookup_table_sourcecashbank');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_sourcecashbank').dialog('open');
    });

    $("#lookup_table_sourcecashbank").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description', 'Amount', 'Is Bank'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right', hidden: true },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 200 },
                  { name: 'amount', index: 'amount', width: 150, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'isbank', index: 'isbank', width: 100 },
        ],
        page: '1',
        pager: $('#lookup_pager_sourcecashbank'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_sourcecashbank").width() - 10,
        height: $("#lookup_div_sourcecashbank").height() - 110,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsBank = $(this).getRowData(cl).isbank;
		          if (rowIsBank == 'true') {
		              rowIsBank = "YES";
		          } else {
		              rowIsBank = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isbank: rowIsBank });

		      }
		  }
    });
    $("#lookup_table_sourcecashbank").jqGrid('navGrid', '#lookup_toolbar_sourcecashbank', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_sourcecashbank').click(function () {
        $('#lookup_div_sourcecashbank').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_sourcecashbank').click(function () {
        var id = jQuery("#lookup_table_sourcecashbank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_sourcecashbank").jqGrid('getRowData', id);

            $('#SourceCashBankId').val(ret.id).data("kode", id);
            $('#SourceCashBank').val(ret.name);

            $('#lookup_div_sourcecashbank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup SourceCashBank----------------------------------------------------------------

    // -------------------------------------------------------Look Up TargetCashBank-------------------------------------------------------
    $('#btnTargetCashBank').click(function () {

        var lookUpURL = base_url + 'MstCashBank/GetList';
        var lookupGrid = $('#lookup_table_targetcashbank');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");
        $('#lookup_div_targetcashbank').dialog('open');
    });

    $("#lookup_table_targetcashbank").jqGrid({
        url: base_url,
        datatype: "json",
        mtype: 'GET',
        colNames: ['Id', 'Name', 'Description', 'Amount', 'Is Bank'],
        colModel: [
                  { name: 'id', index: 'id', width: 80, align: 'right', hidden: true },
                  { name: 'name', index: 'name', width: 200 },
                  { name: 'description', index: 'description', width: 200 },
                  { name: 'amount', index: 'amount', width: 150, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
                  { name: 'isbank', index: 'isbank', width: 100 },
        ],
        page: '1',
        pager: $('#lookup_pager_targetcashbank'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#lookup_div_targetcashbank").width() - 10,
        height: $("#lookup_div_targetcashbank").height() - 110,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsBank = $(this).getRowData(cl).isbank;
		          if (rowIsBank == 'true') {
		              rowIsBank = "YES";
		          } else {
		              rowIsBank = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isbank: rowIsBank });

		      }
		  }
    });
    $("#lookup_table_targetcashbank").jqGrid('navGrid', '#lookup_toolbar_targetcashbank', { del: false, add: false, edit: false, search: true })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    // Cancel or CLose
    $('#lookup_btn_cancel_targetcashbank').click(function () {
        $('#lookup_div_targetcashbank').dialog('close');
    });

    // ADD or Select Data
    $('#lookup_btn_add_targetcashbank').click(function () {
        var id = jQuery("#lookup_table_targetcashbank").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_table_targetcashbank").jqGrid('getRowData', id);

            $('#TargetCashBankId').val(ret.id).data("kode", id);
            $('#TargetCashBank').val(ret.name);

            $('#lookup_div_targetcashbank').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });


    // ---------------------------------------------End Lookup TargetCashBank----------------------------------------------------------------



}); //END DOCUMENT READY