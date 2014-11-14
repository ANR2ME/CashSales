$(document).ready(function () {
	var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

	function ClearErrorMessage() {
		$('span[class=errormessage]').text('').remove();
	}

	function ReloadGrid() {
		$("#list").setGridParam({ url: base_url + 'CashSalesReturn/GetList', postData: { filters: null }, page: 1 }).trigger("reloadGrid");
	}

	function ReloadGridBySKU() {
	    // Clear Search string jqGrid
	    //$('input[id*="gs_"]').val("");

	    var findSKU = $('#findSKU').val();

	    $("#list").setGridParam({ url: base_url + 'CashSalesInvoice/GetList', postData: { filters: null, findSKU: findSKU }, page: '1' }).trigger("reloadGrid");
	}

	function ReloadGridDetail() {
		$("#listdetail").setGridParam({ url: base_url + 'CashSalesReturn/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
	}

	function ClearData() {
		$('#Description').removeClass('errormessage');
		$('#Code').removeClass('errormessage');
		$('#form_btn_save').data('kode', '');
		$('#cashsalesinvoicedetail_btn_submit').data('kode', '');
		ClearErrorMessage();
	}

	$("#form_div").dialog('close');
	$("#cashsalesinvoicedetail_div").dialog('close');
	$("#confirm_div").dialog('close');
	$("#paid_div").dialog('close');
	$("#lookup_div_cashbank").dialog('close');
	$("#lookup_div_cashsalesinvoice").dialog('close');
	$("#lookup_div_cashsalesinvoicedetail").dialog('close');
	$("#delete_confirm_div").dialog('close');


	//GRID +++++++++++++++
	$("#list").jqGrid({
		url: base_url + 'CashSalesReturn/GetList',
		datatype: "json",
		colNames: ['ID', 'Code', 'Description', 'CashSalesInvoice ID', 'CashSalesInvoice Code',
				   'Allowance', 'Total', 'Is Confirmed', 'Confirmation Date',
				   'CashBank ID', 'CashBank Name', 'Is Bank', 'Is Paid',
				   'Return Date', 'Created At', 'Updated At'],
		colModel: [
				  { name: 'id', index: 'id', width: 50, align: "center" },
				  { name: 'code', index: 'code', width: 100 },
				  { name: 'description', index: 'description', width: 150 },
                  { name: 'cashsalesinvoiceid', index: 'cashsalesinvoiceid', width: 125, hidden:true },
				  { name: 'cashsalesinvoice', index: 'cashsalesinvoice', width: 155 },
                  { name: 'allowance', index: 'allowance', width: 80, formatter: 'currency', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'total', index: 'total', width: 80, formatter: 'currency', formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 80, boolean: { defaultValue: 'false' }, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
				  { name: 'confirmationdate', index: 'confirmationdate', hidden:true, search: false, width: 120, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'cashbankid', index: 'cashbankid', width: 80, hidden:true },
				  { name: 'cashbank', index: 'cashbank', width: 100 },
				  { name: 'isbank', index: 'isbank', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'ispaid', index: 'ispaid', width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'returndate', index: 'returndate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		height: $(window).height() - 220,
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
				  rowIsBank = $(this).getRowData(cl).isbank;
				  if (rowIsBank == 'true') {
					  rowIsBank = "YES";
				  } else {
					  rowIsBank = "NO";
				  }
				  $(this).jqGrid('setRowData', ids[i], { isbank: rowIsBank });
				  rowIsPaid = $(this).getRowData(cl).ispaid;
				  if (rowIsPaid == 'true') {
					  rowIsPaid = "YES";
				  } else {
					  rowIsPaid = "NO";
				  }
				  $(this).jqGrid('setRowData', ids[i], { ispaid: rowIsPaid });
			  }
		  }

	});//END GRID
	$("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	$('#btn_find').click(function () {
	    ReloadGridBySKU();
	});

	//TOOL BAR BUTTON
	$('#btn_reload').click(function () {
		ReloadGrid();
	});

	$('#btn_print').click(function () {
	    //window.open(base_url + 'Print_Forms/Printmstbank.aspx');
	    var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
	    if (id) {
	        window.open(base_url + "Report/ReportSalesReturnInvoice?Id=" + id);
	    } else {
	        $.messager.alert('Information', 'Please Select Data...!!', 'info');
	    }
	});

	$('#btn_add_new').click(function () {
		ClearData();
		clearForm('#frm');
		$('#ReturnDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
		$('#Description').removeAttr('disabled');
		$('#btnCashBank').removeAttr('disabled');
		$('#btnCashSalesInvoice').removeAttr('disabled');
		//$('#Allowance').removeAttr('disabled');
		$('#ReturnDateDiv2').hide();
		$('#ReturnDateDiv').show();
		vStatusSaving = 0; //add data mode
		$('#form_btn_save').show();
		$('#tabledetail_div').hide();
		$('#form_div').dialog('open');
	});

	$('#btn_add_detail').click(function () {
		ClearData();
		clearForm('#frm');
		var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
		if (id) {
			$.ajax({
				dataType: "json",
				url: base_url + "CashSalesReturn/GetInfo?Id=" + id,
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
							$('#CashBankId').val(result.CashBankId);
							$('#CashBankName').val(result.CashBank);
							$('#CashSalesInvoiceId').val(result.CashSalesInvoiceId);
							$('#CashSalesInvoiceName').val(result.CashSalesInvoice);
							$('#Allowance').numberbox('setValue', result.Allowance);
							$('#Total').numberbox('setValue', result.Total);
							$('#ReturnDate2').val(dateEnt(result.ReturnDate));
							$('#Description').attr('disabled', true);
							$('#btnCashBank').attr('disabled', true);
							$('#btnCashSalesInvoice').attr('disabled', true);
							$('#Allowance').attr('disabled', true);
							$('#Total').attr('disabled', true);
							$('#ReturnDateDiv').hide();
							$('#ReturnDateDiv2').show();
							$('#form_btn_save').hide();
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
			vStatusSaving = 1;//edit data mode
			$.ajax({
				dataType: "json",
				url: base_url + "CashSalesReturn/GetInfo?Id=" + id,
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
							$('#ReturnDate').datebox('setValue', dateEnt(result.ReturnDate));
							//$('#Allowance').numberbox('setValue', (result.Allowance));
							$('#Total').numberbox('setValue', (result.Total));
							$('#CashBankId').val(result.CashBankId);
							$('#CashBankName').val(result.CashBank);
							$('#CashSalesInvoiceId').val(result.CashSalesInvoiceId);
							$('#CashSalesInvoiceName').val(result.CashSalesInvoice);
							$('#Description').removeAttr('disabled');
							$('#btnCashBank').removeAttr('disabled');
							$('#btnCashSalesInvoice').removeAttr('disabled');
							//$('#Allowance').removeAttr('disabled');
							$('#ReturnDateDiv2').hide();
							$('#ReturnDateDiv').show();
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
			$('#ConfirmationDate').datebox('setValue', ret.returndate);
			//$('#confirmAllowance').numberbox('setValue', ret.allowance);
			$('#idconfirm').val(ret.id);
			$('#confirmCode').val(ret.code);
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
						url: base_url + "CashSalesReturn/UnConfirm",
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
			url: base_url + "CashSalesReturn/Confirm",
			type: "POST",
			contentType: "application/json",
			data: JSON.stringify({
				Id: $('#idconfirm').val(), ConfirmationDate: $('#ConfirmationDate').datebox('getValue'),
				//Allowance: $('#confirmAllowance').numberbox('getValue'),
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

	$('#btn_paid').click(function () {
	    var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
	    if (id) {
	        var ret = jQuery("#list").jqGrid('getRowData', id);
	        $('#paidAllowance').numberbox('setValue', ret.allowance);
	        $('#paidTotal').numberbox('setValue', ret.total);
	        $('#idpaid').val(ret.id);
	        $('#paidCode').val(ret.code);
	        $("#paid_div").dialog("open");
	    } else {
	        $.messager.alert('Information', 'Please Select Data...!!', 'info');
	    }
	});

	$('#btn_unpaid').click(function () {
	    var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
	    if (id) {
	        var ret = jQuery("#list").jqGrid('getRowData', id);
	        $.messager.confirm('Confirm', 'Are you sure you want to unpaid record?', function (r) {
	            if (r) {
	                $.ajax({
	                    url: base_url + "CashSalesReturn/UnPaid",
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
	                            $("#delete_paid_div").dialog('close');
	                        }
	                    }
	                });
	            }
	        });
	    } else {
	        $.messager.alert('Information', 'Please Select Data...!!', 'info');
	    }
	});

	$('#paid_btn_submit').click(function () {
	    ClearErrorMessage();
	    $.ajax({
	        url: base_url + "CashSalesReturn/Paid",
	        type: "POST",
	        contentType: "application/json",
	        data: JSON.stringify({
	            Id: $('#idpaid').val(), Allowance: $('#paidAllowance').numberbox('getValue'),
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
	                $("#paid_div").dialog('close');
	            }
	        }
	    });
	});

	$('#paid_btn_cancel').click(function () {
	    $('#paid_div').dialog('close');
	});

	$('#btn_del').click(function () {
		clearForm("#frm");

		var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#list").jqGrid('getRowData', id);
			//if (ret.deletedimg != '') {
			//    $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
			//    return;
			//}
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
			url: base_url + "CashSalesReturn/Delete",
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
					ReloadGrid();
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
			submitURL = base_url + 'CashSalesReturn/Update';
		}
			// Insert
		else {
			submitURL = base_url + 'CashSalesReturn/Insert';
		}

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
				Id: id, Code: $("#Code").val(), Description: $("#Description").val(),
				ReturnDate: $("#ReturnDate").datebox('getValue'), 
				CashBankId: $('#CashBankId').val(), CashSalesInvoiceId: $('#CashSalesInvoiceId').val(),
				//Allowance: $('#Allowance').numberbox('getValue'),
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
					//var error = '';
					//for (var key in result.Errors) {
					//    error = error + "<br>" + key + " "+result.Errors[key];
					//}
					//$.messager.alert('Warning',error, 'warning');
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
		colNames: ['Code', 'CashSalesReturn Id', 'CashSalesReturn Code', 'CashSalesInvoiceDetail Id', 'CashSalesInvoiceDetail Code', 'Item SKU', 'Item Name', 'Quantity', 'TotalPrice', 'CoGS'],
		colModel: [
				  { name: 'code', index: 'code', width: 100, hidden:true },
				  { name: 'cashsalesreturnid', index: 'cashsalesreturnid', hidden:true, width: 130 },
				  { name: 'cashsalesreturn', index: 'cashsalesreturn', hidden:true, width: 140 },
				  { name: 'cashsalesinvoicedetailid', index: 'cashsalesinvoicedetailid', hidden:true,width: 100 },
				  { name: 'cashsalesinvoicedetail', index: 'cashsalesinvoicedetail', width: 150 },
                  { name: 'itemsku', index: 'itemsku', width: 80 },
                  { name: 'item', index: 'item', width: 100 },
				  { name: 'quantity', index: 'quantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'totalprice', index: 'totalprice', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'cogs', index: 'cogs', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
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
	$("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false })
	                .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	$('#btn_add_new_detail').click(function () {
	    ClearData();
	    $('#Quantity').numberbox('setValue', '');
		clearForm('#cashsalesinvoicedetail_div');
		$('#cashsalesinvoicedetail_div').dialog('open');
	});

	$('#btn_edit_detail').click(function () {
		ClearData();
		clearForm("#cashsalesinvoicedetail_div");
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			$.ajax({
				dataType: "json",
				url: base_url + "CashSalesReturn/GetInfoDetail?Id=" + id,
				success: function (result) {
					if (result.Id == null) {
						$.messager.alert('Information', 'Data Not Found...!!', 'info');
					}
					else {
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
							$("#cashsalesinvoicedetail_btn_submit").data('kode', result.Id);
							$('#CashSalesInvoiceDetailId').val(result.CashSalesInvoiceDetailId);
							$('#CashSalesInvoiceDetail').val(result.CashSalesInvoiceDetail);
							$('#Quantity').numberbox('setValue', result.Quantity);
							$('#cashsalesinvoicedetail_div').dialog('open');
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
						url: base_url + "CashSalesReturn/DeleteDetail",
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
								$('#Total').val(result.Total);
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
	//--------------------------------------------------------Dialog CashSalesInvoiceDetail-------------------------------------------------------------
	// cashsalesinvoicedetail_btn_submit

	$("#cashsalesinvoicedetail_btn_submit").click(function () {

		ClearErrorMessage();

		var submitURL = '';
		var id = $("#cashsalesinvoicedetail_btn_submit").data('kode');

		// Update
		if (id != undefined && id != '' && !isNaN(id) && id > 0) {
			submitURL = base_url + 'CashSalesReturn/UpdateDetail';
		}
			// Insert
		else {
			submitURL = base_url + 'CashSalesReturn/InsertDetail';
		}

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
				Id: id, CashSalesReturnId: $("#id").val(), CashSalesReturnDetailId: $("#CashSalesReturnDetailId").val(), CashSalesInvoiceDetailId: $("#CashSalesInvoiceDetailId").val(), Quantity: $("#Quantity").val(),
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
					$('#Total').val(result.Total);
					ReloadGridDetail();
					ReloadGrid();
					$("#cashsalesinvoicedetail_div").dialog('close')
				}
			}
		});
	});


	// cashsalesinvoicedetail_btn_cancel
	$('#cashsalesinvoicedetail_btn_cancel').click(function () {
		clearForm('#cashsalesinvoicedetail_div');
		$("#cashsalesinvoicedetail_div").dialog('close');
	});
	//--------------------------------------------------------END Dialog CashSalesInvoiceDetail-------------------------------------------------------------


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

	// -------------------------------------------------------Look Up CashBank-------------------------------------------------------
	$('#btnCashBank').click(function () {

		var lookUpURL = base_url + 'MstCashBank/GetList';
		var lookupGrid = $('#lookup_table_cashbank');
		lookupGrid.setGridParam({
			url: lookUpURL
		}).trigger("reloadGrid");
		$('#lookup_div_cashbank').dialog('open');
	});

	$("#lookup_table_cashbank").jqGrid({
		url: base_url,
		datatype: "json",
		mtype: 'GET',
		colNames: ['Id', 'Name', 'Description', 'Amount'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: 'right', hidden:true },
				  { name: 'name', index: 'name', width: 100 },
				  { name: 'description', index: 'description', width: 200 },
                  { name: 'amount', index: 'amount', width: 150, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0.00' } },
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
			$('#CashBankName').val(ret.name);

			$('#lookup_div_cashbank').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});

	
	// ---------------------------------------------End Lookup CashBank----------------------------------------------------------------

	// -------------------------------------------------------Look Up CashSalesInvoice-------------------------------------------------------
	$('#btnCashSalesInvoice').click(function () {
		var lookUpURL = base_url + 'CashSalesInvoice/GetPaidList';
		var lookupGrid = $('#lookup_table_cashsalesinvoice');
		lookupGrid.setGridParam({
			url: lookUpURL
		}).trigger("reloadGrid");
		$('#lookup_div_cashsalesinvoice').dialog('open');
	});

	jQuery("#lookup_table_cashsalesinvoice").jqGrid({
		url: base_url,
		datatype: "json",
		mtype: 'GET',
		colNames: ['Id', 'Code', 'Description', 'Discount', 'Tax', 'Delivery Cost', 'Allowance', 'Amount Paid', 'Total'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: 'right', hidden:true },
				  { name: 'code', index: 'code', width: 100 },
				  { name: 'description', index: 'description', width: 200 },
                  { name: 'discount', index: 'discount', hidden:true, width: 80, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'tax', index: 'tax', width: 80, hidden: true, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'shippingfee', index: 'shippingfee', hidden: true, width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
				  { name: 'allowance', index: 'allowance', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'amountpaid', index: 'amountpaid', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'total', index: 'total', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
		],
		page: '1',
		pager: $('#lookup_pager_cashsalesinvoice'),
		rowNum: 20,
		rowList: [20, 30, 60],
		sortname: 'id',
		viewrecords: true,
		scrollrows: true,
		shrinkToFit: false,
		sortorder: "DESC",
		width: $("#lookup_div_cashsalesinvoice").width() - 10,
		height: $("#lookup_div_cashsalesinvoice").height() - 110,
	});
	$("#lookup_table_cashsalesinvoice").jqGrid('navGrid', '#lookup_toolbar_cashsalesinvoice', { del: false, add: false, edit: false, search: true })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	// Cancel or CLose
	$('#lookup_btn_cancel_cashsalesinvoice').click(function () {
		$('#lookup_div_cashsalesinvoice').dialog('close');
	});

	// ADD or Select Data
	$('#lookup_btn_add_cashsalesinvoice').click(function () {
		var id = jQuery("#lookup_table_cashsalesinvoice").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#lookup_table_cashsalesinvoice").jqGrid('getRowData', id);

			$('#CashSalesInvoiceId').val(ret.id).data("kode", id);
			$('#CashSalesInvoiceName').val(ret.code);

			$('#lookup_div_cashsalesinvoice').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});


	// ---------------------------------------------End Lookup CashSalesInvoice----------------------------------------------------------------

	// -------------------------------------------------------Look Up cashsalesinvoicedetail-------------------------------------------------------
	$('#btnCashSalesInvoiceDetail').click(function () {
	    var lookUpURL = base_url + 'CashSalesInvoice/GetPaidListDetail?Id=' + $('#CashSalesInvoiceId').val();
		var lookupGrid = $('#lookup_table_cashsalesinvoicedetail');
		lookupGrid.setGridParam({
			url: lookUpURL
		}).trigger("reloadGrid");
		$('#lookup_div_cashsalesinvoicedetail').dialog('open');
	});

	jQuery("#lookup_table_cashsalesinvoicedetail").jqGrid({
		url: base_url,
		datatype: "json",
		mtype: 'GET',
		colNames: ['Code', 'CashSalesInvoice Id', 'CashSalesInvoice Code', 'Item Id', 'Item Name', 'Quantity'],
		colModel: [
				  //{ name: 'id', index: 'id', width: 80, align: 'right' },
				  { name: 'code', index: 'code', width: 80, align: 'right' },
                  { name: 'cashsalesinvoiceid', index: 'cashsalesinvoiceid', hidden:true, width: 80 },
                  { name: 'cashsalesinvoice', index: 'cashsalesinvoice', width: 100 },
                  { name: 'itemid', index: 'itemid', hidden:true, width: 80 },
                  { name: 'item', index: 'item', width: 200 },
                  { name: 'quantity', index: 'quantity', width: 100 },
		],
		page: '1',
		pager: $('#lookup_pager_cashsalesinvoicedetail'),
		rowNum: 20,
		rowList: [20, 30, 60],
		sortname: 'id',
		viewrecords: true,
		scrollrows: true,
		shrinkToFit: false,
		sortorder: "DESC",
		width: $("#lookup_div_cashsalesinvoicedetail").width() - 10,
		height: $("#lookup_div_cashsalesinvoicedetail").height() - 110,
	});
	$("#lookup_table_cashsalesinvoicedetail").jqGrid('navGrid', '#lookup_toolbar_cashsalesinvoicedetail', { del: false, add: false, edit: false, search: true })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	// Cancel or CLose
	$('#lookup_btn_cancel_cashsalesinvoicedetail').click(function () {
		$('#lookup_div_cashsalesinvoicedetail').dialog('close');
	});

	// ADD or Select Data
	$('#lookup_btn_add_cashsalesinvoicedetail').click(function () {
		var id = jQuery("#lookup_table_cashsalesinvoicedetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#lookup_table_cashsalesinvoicedetail").jqGrid('getRowData', id);

			$('#CashSalesInvoiceDetailId').val(id).data("kode", id);
			$('#CashSalesInvoiceDetail').val(ret.code);

			$('#lookup_div_cashsalesinvoicedetail').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});


	// ---------------------------------------------End Lookup cashsalesinvoicedetail----------------------------------------------------------------


}); //END DOCUMENT READY