﻿$(document).ready(function () {
	var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

	function ClearErrorMessage() {
		$('span[class=errormessage]').text('').remove();
	}

	function ReloadGrid() {
		$("#list").setGridParam({ url: base_url + 'CashSalesReturn/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
	}

	function ReloadGridDetail() {
		$("#listdetail").setGridParam({ url: base_url + 'CashSalesReturn/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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
		colNames: ['ID', 'Code', 'Description', 'Return Date', 'CashSalesInvoice ID', 'CashSalesInvoice Code',
				   'Allowance', 'Is Confirmed', 'Confirmation Date', 'Total',
				   'CashBank ID', 'CashBank Name', 'Is Bank', 'Is Paid',
				   'Created At', 'Updated At', 'CashSalesReturnDetails'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'code', index: 'code', width: 100 },
				  { name: 'description', index: 'description', width: 100 },
				  { name: 'returndate', index: 'returndate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'cashsalesinvoiceid', index: 'cashsalesinvoiceid', width: 80 },
				  { name: 'cashsalesinvoice', index: 'cashsalesinvoice', width: 100 },
                  { name: 'allowance', index: 'allowance', width: 80, formatter: 'currency' },
				  { name: 'isconfirmed', index: 'isconfirmed', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'total', index: 'total', width: 80, formatter: 'currency' },
				  { name: 'cashbankid', index: 'cashbankid', width: 80 },
				  { name: 'cashbank', index: 'cashbank', width: 100 },
				  { name: 'isbank', index: 'isbank', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'ispaid', index: 'ispaid', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'cashsalesreturndetails', index: 'cashsalesreturndetails', width: 80 },
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
			  var ids = $(this).jqGrid('getDataIDs');
			  for (var i = 0; i < ids.length; i++) {
				  var cl = ids[i];
				  rowIsConfirmed = $(this).getRowData(cl).isconfirmed;
				  if (rowIsConfirmed == 'true') {
					  rowIsConfirmed = "YES";
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
		$('#ReturnDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
		$('#Description').removeAttr('disabled');
		$('#btnCashBank').removeAttr('disabled');
		$('#btnCashSalesInvoice').removeAttr('disabled');
		$('#Allowance').removeAttr('disabled');
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
							$('#Allowance').numberbox('setValue', (result.Allowance));
							$('#Total').numberbox('setValue', (result.Total));
							$('#CashBankId').val(result.CashBankId);
							$('#CashBankName').val(result.CashBank);
							$('#CashSalesInvoiceId').val(result.CashSalesInvoiceId);
							$('#CashSalesInvoiceName').val(result.CashSalesInvoice);
							$('#Description').removeAttr('disabled');
							$('#btnCashBank').removeAttr('disabled');
							$('#btnCashSalesInvoice').removeAttr('disabled');
							$('#Allowance').removeAttr('disabled');
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
			$('#ConfirmationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
			$('#confirmAllowance').numberbox('setValue', ret.allowance);
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
				Allowance: $('#confirmAllowance').numberbox('getValue'),
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
				ReturnDate: $("#ReturnDate").datebox('getValue'), Allowance: $('#Allowance').numberbox('getValue'),
				CashBankId: $('#CashBankId').val(), CashSalesInvoiceId: $('#CashSalesInvoiceId').val(),
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
		colNames: ['Code', 'CashSalesReturn Id', 'CashSalesReturn Code', 'CashSalesInvoiceDetail Id', 'CashSalesInvoiceDetail Code', 'Quantity', 'TotalPrice'],
		colModel: [
				  { name: 'code', index: 'code', width: 100, sortable: false },
				  { name: 'cashsalesreturnid', index: 'cashsalesreturnid', width: 130, sortable: false },
				  { name: 'cashsalesreturncode', index: 'cashsalesreturncode', width: 130, sortable: false },
				  { name: 'cashsalesinvoicedetailid', index: 'cashsalesinvoicedetailid', width: 130, sortable: false },
				  { name: 'cashsalesinvoicedetailcode', index: 'cashsalesinvoicedetailcode', width: 130, sortable: false },
				  { name: 'quantity', index: 'quantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
				  { name: 'totalprice', index: 'totalprice', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' }, sortable: false },
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
		width: $(window).width() - 700,
		height: $(window).height() - 500,
		gridComplete:
		  function () {
		  }
	});//END GRID Detail
	$("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false });
	//.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

	$('#btn_add_new_detail').click(function () {
		ClearData();
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
							$('#Quantity').val(result.Quantity);
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
		colNames: ['Id', 'Name', 'Description'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: 'right' },
				  { name: 'name', index: 'name', width: 200 },
				  { name: 'description', index: 'description', width: 200 }],
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
	$("#lookup_table_cashbank").jqGrid('navGrid', '#lookup_toolbar_cashbank', { del: false, add: false, edit: false, search: false })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
		var lookUpURL = base_url + 'CashSalesInvoice/GetList';
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
		colNames: ['Id', 'Code', 'Description'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: 'right' },
				  { name: 'code', index: 'code', width: 200 },
				  { name: 'description', index: 'description', width: 200 }],
		page: '1',
		pager: $('#lookup_pager_cashsalesinvoice'),
		rowNum: 20,
		rowList: [20, 30, 60],
		sortname: 'id',
		viewrecords: true,
		scrollrows: true,
		shrinkToFit: false,
		sortorder: "ASC",
		width: $("#lookup_div_cashsalesinvoice").width() - 10,
		height: $("#lookup_div_cashsalesinvoice").height() - 110,
	});
	$("#lookup_table_cashsalesinvoice").jqGrid('navGrid', '#lookup_toolbar_cashsalesinvoice', { del: false, add: false, edit: false, search: false })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
	    var lookUpURL = base_url + 'CashSalesInvoice/GetListDetail?Id=' + $('#CashSalesInvoiceId').val();
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
				  { name: 'code', index: 'code', width: 200, align: 'right' },
                  { name: 'cashsalesinvoiceid', index: 'cashsalesinvoiceid', width: 200 },
                  { name: 'cashsalesinvoice', index: 'cashsalesinvoice', width: 200 },
                  { name: 'itemid', index: 'itemid', width: 200 },
                  { name: 'item', index: 'item', width: 200 },
                  { name: 'quantity', index: 'quantity', width: 200 },
		],
		page: '1',
		pager: $('#lookup_pager_cashsalesinvoicedetail'),
		rowNum: 20,
		rowList: [20, 30, 60],
		sortname: 'id',
		viewrecords: true,
		scrollrows: true,
		shrinkToFit: false,
		sortorder: "ASC",
		width: $("#lookup_div_cashsalesinvoicedetail").width() - 10,
		height: $("#lookup_div_cashsalesinvoicedetail").height() - 110,
	});
	$("#lookup_table_cashsalesinvoicedetail").jqGrid('navGrid', '#lookup_toolbar_cashsalesinvoicedetail', { del: false, add: false, edit: false, search: false })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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