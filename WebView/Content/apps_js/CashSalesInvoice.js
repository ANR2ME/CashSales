$(document).ready(function () {
	var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

	function ClearErrorMessage() {
		$('span[class=errormessage]').text('').remove();
	}

	function ReloadGrid() {
		$("#list").setGridParam({ url: base_url + 'CashSalesInvoice/GetList', postData: { filters: null }, page: 1 }).trigger("reloadGrid");
	}

	function ReloadGridDetail() {
		$("#listdetail").setGridParam({ url: base_url + 'CashSalesInvoice/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
	}

	function onManualAssignedPrice() {
	    if (document.getElementById('IsManualPriceAssignment').value == 'True') {
	        $('#AssignedPrice').removeAttr('disabled');
	    } else {
	        $('#AssignedPrice').attr('disabled', true);
	    }
	};

	document.getElementById('IsManualPriceAssignment').onchange = function () {
	    onManualAssignedPrice();
	};

	function CalcTotal() {
	    var tot = parseFloat($('#confirmTotal').numberbox('getValue'));
	    var disc = parseFloat($('#confirmDiscount').numberbox('getValue'));
	    var tax = parseFloat($('#confirmTax').numberbox('getValue'));
	    var fee = parseFloat($('#confirmShippingFee').numberbox('getValue'));
	    tot = (tot * (100.0 - disc) / 100.0);
	    tot = (tot * (100.0 + tax) / 100.0);
	    tot += fee;
	    $('#confirmTotal2').numberbox('setValue', tot);
	};

	//document.getElementById('confirmDiscount').onchange = function () {
	//    CalcTotal();
	//};

	//document.getElementById('confirmTax').onchange = function () {
	//    CalcTotal();
	//};

	//$('#confirmDiscount').bind('keypress', function (e) {
	//    CalcTotal();
	//});

	//$('#confirmTax').bind('keypress', function (e) {
	//    CalcTotal();
	//});

	function ClearData() {
		$('#Description').removeClass('errormessage');
		$('#Code').removeClass('errormessage');
		$('#form_btn_save').data('kode', '');
		$('#item_btn_submit').data('kode', '');
		ClearErrorMessage();
	}

	$("#form_div").dialog('close');
	$("#item_div").dialog('close');
	$("#confirm_div").dialog('close');
	$("#paid_div").dialog('close');
	$("#lookup_div_cashbank").dialog('close');
	$("#lookup_div_warehouse").dialog('close');
	$("#lookup_div_item").dialog('close');
	$("#delete_confirm_div").dialog('close');


	//GRID +++++++++++++++
	$("#list").jqGrid({
		url: base_url + 'CashSalesInvoice/GetList',
		datatype: "json",
		colNames: ['ID', 'Code', 'Description', 
				   'Discount', 'Tax', 'Shipping Fee', 'Allowance', 'Amount Paid', 'Total', 'CoGS', 'Profit/Loss', 'Is Confirmed', 'Confirmation Date',
				   'CashBank ID', 'CashBank Name', 'Is Bank', 'Is Paid', 'Is Full Payment',
				   'Warehouse ID', 'Warehouse Name',
				   'Sales Date', 'Due Date', 'Created At', 'Updated At'],
		colModel: [
				  { name: 'id', index: 'id', width: 50, align: "center" },
				  { name: 'code', index: 'code', width: 100 },
				  { name: 'description', index: 'description', width: 150 },
				  { name: 'discount', index: 'discount', width: 80, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'tax', index: 'tax', width: 80, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'shippingfee', index: 'shippingfee', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
				  { name: 'allowance', index: 'allowance', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'amountpaid', index: 'amountpaid', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'total', index: 'total', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
				  { name: 'cogs', index: 'cogs', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'profitloss', index: 'profitloss', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 80, boolean: { defaultValue: 'false' }, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
				  { name: 'confirmationdate', index: 'confirmationdate', hidden:true, search: false, width: 120, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'cashbankid', index: 'cashbankid', width: 80, hidden:true },
				  { name: 'cashbank', index: 'cashbank', width: 100 },
				  { name: 'isbank', index: 'isbank', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'ispaid', index: 'ispaid', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'isfullpayment', index: 'isfullpayment', width: 100, boolean: { defaultValue: 'false' } },
				  { name: 'warehouseid', index: 'warehouseid', width: 85, hidden:true },
				  { name: 'warehouse', index: 'warehouse', width: 100 },
                  { name: 'salesdate', index: 'salesdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'duedate', index: 'duedate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
				  rowIsFullpayment = $(this).getRowData(cl).isfullpayment;
				  if (rowIsFullpayment == 'true') {
					  rowIsFullpayment = "YES";
				  } else {
					  rowIsFullpayment = "NO";
				  }
				  $(this).jqGrid('setRowData', ids[i], { isfullpayment: rowIsFullpayment });

				  rowIsManualPriceAssignment = $(this).getRowData(cl).ismanualpriceassignment;
				  if (rowIsManualPriceAssignment == 'true') {
				      rowIsManualPriceAssignment = "YES";
				  } else {
				      rowIsManualPriceAssignment = "NO";
				  }
				  $(this).jqGrid('setRowData', ids[i], { ismanualpriceassignment: rowIsManualPriceAssignment });
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
	    //window.open(base_url + 'Print_Forms/Printmstbank.aspx');
	    var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
	    if (id) {
	        window.open(base_url + "Report/ReportSalesInvoice?Id=" + id);
	    } else {
	        $.messager.alert('Information', 'Please Select Data...!!', 'info');
	    }
	});

	$('#btn_print2').click(function () {
	    //window.open(base_url + 'Print_Forms/Printmstbank.aspx');
	    var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
	    if (id) {
	        window.open(base_url + "Report/ReportSalesInvoiceForCustomer?Id=" + id);
	    } else {
	        $.messager.alert('Information', 'Please Select Data...!!', 'info');
	    }
	});

	$('#btn_add_new').click(function () {
		ClearData();
		clearForm('#frm');
		$('#SalesDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
		$('#DueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
		$('#Description').removeAttr('disabled');
		$('#btnCashBank').removeAttr('disabled');
		$('#btnWarehouse').removeAttr('disabled');
		//$('#Discount').removeAttr('disabled');
		//$('#Tax').removeAttr('disabled');
		//$('#Allowance').removeAttr('disabled');
		$('#SalesDateDiv2').hide();
		$('#SalesDateDiv').show();
		$('#DueDateDiv2').hide();
		$('#DueDateDiv').show();
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
				url: base_url + "CashSalesInvoice/GetInfo?Id=" + id,
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
							$('#WarehouseId').val(result.WarehouseId);
							$('#WarehouseName').val(result.Warehouse);
							$('#Discount').numberbox('setValue', result.Discount);
							$('#Tax').numberbox('setValue', result.Tax);
							$('#ShippingFee').numberbox('setValue', result.ShippingFee);
							$('#Allowance').numberbox('setValue', result.Allowance);
							$('#Total').numberbox('setValue', result.Total);
							$('#SalesDate2').val(dateEnt(result.SalesDate));
							$('#DueDate2').val(dateEnt(result.DueDate));
							$('#Description').attr('disabled', true);
							$('#btnCashBank').attr('disabled', true);
							$('#btnWarehouse').attr('disabled', true);
							$('#Discount').attr('disabled', true);
							$('#Tax').attr('disabled', true);
							$('#ShippingFee').attr('disabled', true);
							$('#Allowance').attr('disabled', true);
							$('#Total').attr('disabled', true);
							$('#SalesDateDiv').hide();
							$('#SalesDateDiv2').show();
							$('#DueDateDiv').hide();
							$('#DueDateDiv2').show();
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
				url: base_url + "CashSalesInvoice/GetInfo?Id=" + id,
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
							$('#SalesDate').datebox('setValue', dateEnt(result.SalesDate));
							$('#DueDate').datebox('setValue', dateEnt(result.DueDate));
							//$('#Discount').numberbox('setValue', (result.Discount));
							//$('#Tax').numberbox('setValue', (result.Tax));
							//$('#Allowance').numberbox('setValue', (result.Allowance));
							$('#Total').numberbox('setValue', (result.Total));
							$('#CashBankId').val(result.CashBankId);
							$('#CashBankName').val(result.CashBank);
							$('#WarehouseId').val(result.WarehouseId);
							$('#WarehouseName').val(result.Warehouse);
							$('#Description').removeAttr('disabled');
							$('#btnCashBank').removeAttr('disabled');
							$('#btnWarehouse').removeAttr('disabled');
							//$('#Discount').removeAttr('disabled');
							//$('#Tax').removeAttr('disabled');
							//$('#Allowance').removeAttr('disabled');
							$('#SalesDateDiv2').hide();
							$('#SalesDateDiv').show();
							$('#DueDateDiv2').hide();
							$('#DueDateDiv').show();
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
			$('#confirmDiscount').numberbox('setValue', ret.discount);
			$('#confirmTax').numberbox('setValue', ret.tax);
			$('#confirmShippingFee').numberbox('setValue', ret.shippingfee);
			$('#confirmTotal').numberbox('setValue', ret.total);
			$('#confirmTotal2').numberbox('setValue', ret.total);
			$('#confirmCode').val(ret.code);
			$('#idconfirm').val(ret.id);
			CalcTotal();
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
						url: base_url + "CashSalesInvoice/UnConfirm",
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
			url: base_url + "CashSalesInvoice/Confirm",
			type: "POST",
			contentType: "application/json",
			data: JSON.stringify({
				Id: $('#idconfirm').val(), ConfirmationDate: $('#ConfirmationDate').datebox('getValue'),
				Discount: $('#confirmDiscount').numberbox('getValue'), Tax: $('#confirmTax').numberbox('getValue'),
				ShippingFee: $('#confirmShippingFee').numberbox('getValue'),
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
	        $('#AmountPaid').numberbox('setValue', ret.amountpaid);
	        $('#paidTotal').numberbox('setValue', ret.total);
	        $('#paidCode').val(ret.code);
	        $('#idpaid').val(ret.id);
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
	                    url: base_url + "CashSalesInvoice/UnPaid",
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
	        url: base_url + "CashSalesInvoice/Paid",
	        type: "POST",
	        contentType: "application/json",
	        data: JSON.stringify({
	            Id: $('#idpaid').val(), AmountPaid: $('#AmountPaid').numberbox('getValue'),
	            Allowance: $('#paidAllowance').numberbox('getValue'),
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
			url: base_url + "CashSalesInvoice/Delete",
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
			submitURL = base_url + 'CashSalesInvoice/Update';
		}
			// Insert
		else {
			submitURL = base_url + 'CashSalesInvoice/Insert';
		}

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
				Id: id, Code: $("#Code").val(), Description: $("#Description").val(),
				SalesDate: $("#SalesDate").datebox('getValue'), DueDate: $("#DueDate").datebox('getValue'),
				//Discount: $("#Discount").numberbox('getValue'), Tax: $('#Tax').numberbox('getValue'),
				//Allowance: $('#Allowance').numberbox('getValue'),
				CashBankId: $('#CashBankId').val(), WarehouseId: $('#WarehouseId').val(),
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
		colNames: ['Code', 'CashSalesInvoice Id', 'CashSalesInvoice Code', 'Item Id', 'Item Name', 'Quantity', 'Amount', 'CoGS', 'PriceMutation Id', 'Manual Discount', 'Is Manual Price Assignment', 'Assigned Price'],
		colModel: [
				  { name: 'code', index: 'code', width: 100, sortable: false },
				  { name: 'cashsalesinvoiceid', index: 'cashsalesinvoiceid', hidden:true, width: 130, sortable: false },
				  { name: 'cashsalesinvoice', index: 'cashsalesinvoice', hidden:true, width: 150, sortable: false },
				  { name: 'itemid', index: 'itemid', width: 80, hidden:true, sortable: false },
				  { name: 'item', index: 'item', width: 80, sortable: false },
				  { name: 'quantity', index: 'quantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
				  { name: 'amount', index: 'amount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
				  { name: 'cogs', index: 'cogs', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
				  { name: 'pricemutationid', index: 'pricemutationid', hidden:true, width: 105, sortable: false },
                  { name: 'discount', index: 'discount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
                  { name: 'ismanualpriceassignment', index: 'ismanualpriceassignment', width: 165, boolean: { defaultValue: 'false' } },
                  { name: 'assignedprice', index: 'assignedprice', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 0, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
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
		width: $(window).width() - 850,
		height: $(window).height() - 500,
		gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsManualPriceAssignment = $(this).getRowData(cl).ismanualpriceassignment;
		          if (rowIsManualPriceAssignment == 'true') {
		              rowIsManualPriceAssignment = "YES";
		          } else {
		              rowIsManualPriceAssignment = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { ismanualpriceassignment: rowIsManualPriceAssignment });

		      }
		  }
	});//END GRID Detail
	$("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false })
	    .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	$('#btn_add_new_detail').click(function () {
		ClearData();
		clearForm('#item_div');
		$('#Quantity').numberbox('setValue', '');
		$('#detailDiscount').numberbox('setValue', '');
		var e = document.getElementById("IsManualPriceAssignment");
		e.selectedIndex = 0;
		$('#AssignedPrice').numberbox('setValue', '');
		onManualAssignedPrice();
		$('#item_div').dialog('open');
	});

	$('#btn_edit_detail').click(function () {
		ClearData();
		clearForm("#item_div");
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			$.ajax({
				dataType: "json",
				url: base_url + "CashSalesInvoice/GetInfoDetail?Id=" + id,
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
							$('#ItemId').val(result.ItemId);
							$('#Item').val(result.Item);
							$('#Quantity').numberbox('setValue', result.Quantity);
							$('#detailDiscount').numberbox('setValue', result.Discount);
							var e = document.getElementById("IsManualPriceAssignment");
							if (result.IsManualPriceAssignment == true) {
							    e.selectedIndex = 1;
							}
							else {
							    e.selectedIndex = 0;
							}
							$('#AssignedPrice').numberbox('setValue', result.AssignedPrice);
							onManualAssignedPrice();
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
						url: base_url + "CashSalesInvoice/DeleteDetail",
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
	//--------------------------------------------------------Dialog Item-------------------------------------------------------------
	// item_btn_submit

	$("#item_btn_submit").click(function () {

		ClearErrorMessage();

		var submitURL = '';
		var id = $("#item_btn_submit").data('kode');

		// Update
		if (id != undefined && id != '' && !isNaN(id) && id > 0) {
			submitURL = base_url + 'CashSalesInvoice/UpdateDetail';
		}
			// Insert
		else {
			submitURL = base_url + 'CashSalesInvoice/InsertDetail';
		}

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
			    Id: id, CashSalesInvoiceId: $("#id").val(), CashSalesInvoiceDetailId: $("#CashSalesInvoiceDetailId").val(),
			    ItemId: $("#ItemId").val(), Quantity: $("#Quantity").val(), Discount: $('#detailDiscount').numberbox('getValue'),
			    IsManualPriceAssignment: document.getElementById("IsManualPriceAssignment").value,
			    AssignedPrice: $('#AssignedPrice').numberbox('getValue'),
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
				  { name: 'id', index: 'id', hidden:true, width: 80, align: 'right' },
				  { name: 'name', index: 'name', width: 200 },
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

	// -------------------------------------------------------Look Up Warehouse-------------------------------------------------------
	$('#btnWarehouse').click(function () {
		var lookUpURL = base_url + 'MstWarehouse/GetList';
		var lookupGrid = $('#lookup_table_warehouse');
		lookupGrid.setGridParam({
			url: lookUpURL
		}).trigger("reloadGrid");
		$('#lookup_div_warehouse').dialog('open');
	});

	jQuery("#lookup_table_warehouse").jqGrid({
		url: base_url,
		datatype: "json",
		mtype: 'GET',
		colNames: ['Id', 'Code', 'Name', 'Description'],
		colModel: [
				  { name: 'id', index: 'id', hidden:true, width: 80, align: 'right' },
				  { name: 'code', index: 'code', width: 200 },
				  { name: 'name', index: 'name', width: 200 },
				  { name: 'description', index: 'description', width: 200 }],
		page: '1',
		pager: $('#lookup_pager_warehouse'),
		rowNum: 20,
		rowList: [20, 30, 60],
		sortname: 'id',
		viewrecords: true,
		scrollrows: true,
		shrinkToFit: false,
		sortorder: "ASC",
		width: $("#lookup_div_warehouse").width() - 10,
		height: $("#lookup_div_warehouse").height() - 110,
	});
	$("#lookup_table_warehouse").jqGrid('navGrid', '#lookup_toolbar_warehouse', { del: false, add: false, edit: false, search: true })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	// Cancel or CLose
	$('#lookup_btn_cancel_warehouse').click(function () {
		$('#lookup_div_warehouse').dialog('close');
	});

	// ADD or Select Data
	$('#lookup_btn_add_warehouse').click(function () {
		var id = jQuery("#lookup_table_warehouse").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#lookup_table_warehouse").jqGrid('getRowData', id);

			$('#WarehouseId').val(ret.id).data("kode", id);
			$('#WarehouseName').val(ret.name);

			$('#lookup_div_warehouse').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});


	// ---------------------------------------------End Lookup Warehouse----------------------------------------------------------------

	// -------------------------------------------------------Look Up item-------------------------------------------------------
	$('#btnItem').click(function () {
	    //var lookUpURL = base_url + 'MstItem/GetList';
	    var lookUpURL = base_url + 'WarehouseItem/GetListItem?Id=' + $('#WarehouseId').val();
		var lookupGrid = $('#lookup_table_item');
		lookupGrid.setGridParam({
			url: lookUpURL
		}).trigger("reloadGrid");
		$('#lookup_div_item').dialog('open');
	});

	jQuery("#lookup_table_item").jqGrid({
		url: base_url,
		datatype: "json",
		mtype: 'GET',
		//colNames: ['Id', 'SKU', 'Name', 'Item Type', 'UoM'],
		//colModel: [
		//		  { name: 'id', index: 'id', width: 80, align: 'right', hidden:true },
		//		  { name: 'sku', index: 'sku', width: 100 },
        //          { name: 'name', index: 'name', width: 100 },
        //          { name: 'itemtype', index: 'itemtype', width: 120 },
        //          { name: 'uom', index: 'uom', width: 100 },
	    //],
		colNames: ['ID', 'SKU', 'Item Name', 'Item Type Id', 'Item Type Name',
                       'Category', 'Uom Id', 'UoM Name', 'Quantity', 'Pending Delivery', 'Pending Receival'
		],
		colModel: [
                  { name: 'id', index: 'id', width: 80, align: "center", frozen: true, hidden: true },
                  { name: 'sku', index: 'sku', width: 80 },
                  { name: 'item', index: 'item', width: 200, frozen: true },
                  { name: 'itemtypeid', index: 'itemtypeid', width: 80, align: "center", hidden: true },
                  { name: 'itemtype', index: 'itemtype', width: 200 },
                  { name: 'category', index: 'category', width: 100, hidden: true },
                  { name: 'uomid', index: 'uomid', width: 100, hidden: true },
                  { name: 'uom', index: 'uom', width: 100 },
                  { name: 'quantity', index: 'quantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingdelivery', index: 'pendingdelivery', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingreceival', index: 'pendingreceival', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
		],
		page: '1',
		pager: $('#lookup_pager_item'),
		rowNum: 20,
		rowList: [20, 30, 60],
		sortname: 'id',
		viewrecords: true,
		scrollrows: true,
		shrinkToFit: false,
		sortorder: "ASC",
		width: $("#lookup_div_item").width() - 10,
		height: $("#lookup_div_item").height() - 110,
	});
	$("#lookup_table_item").jqGrid('navGrid', '#lookup_toolbar_item', { del: false, add: false, edit: false, search: true })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	// Cancel or CLose
	$('#lookup_btn_cancel_item').click(function () {
		$('#lookup_div_item').dialog('close');
	});

	// ADD or Select Data
	$('#lookup_btn_add_item').click(function () {
		var id = jQuery("#lookup_table_item").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#lookup_table_item").jqGrid('getRowData', id);

			$('#ItemId').val(ret.id).data("kode", id); //ret.id
			$('#Item').val(ret.item); //ret.name

			$('#lookup_div_item').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});


	// ---------------------------------------------End Lookup item----------------------------------------------------------------


}); //END DOCUMENT READY