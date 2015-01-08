$(document).ready(function () {
	var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

	function ClearErrorMessage() {
		$('span[class=errormessage]').text('').remove();
	}

	function ReloadGrid() {
	    $("#list").setGridParam({ url: base_url + 'CustomPurchaseInvoice/GetList?findSKU=' + $('#findSKU').val(), postData: { filters: null }, page: 1 }).trigger("reloadGrid");
	}

	function ReloadGridBySKU() {
	    // Clear Search string jqGrid
	    //$('input[id*="gs_"]').val("");

	    var findSKU = $('#findSKU').val();

	    $("#list").setGridParam({ url: base_url + 'CustomPurchaseInvoice/GetList', postData: { filters: null, findSKU: findSKU }, page: '1' }).trigger("reloadGrid");
	}

	function ReloadGridDetail() {
		$("#listdetail").setGridParam({ url: base_url + 'CustomPurchaseInvoice/GetListDetail?Id=' + $("#id").val(), postData: { filters: null } }).trigger("reloadGrid");
	}

	function ClearData() {
		$('#Description').removeClass('errormessage');
		$('#Code').removeClass('errormessage');
		$('#form_btn_save').data('kode', '');
		$('#item_btn_submit').data('kode', '');
		ClearErrorMessage();
	}

	function onGBCH()
	{
	    if (document.getElementById('IsGBCH').value == 'True')
	    {
	        $('#GBCH_No').removeAttr('disabled');
	        $('#GBCHDueDateDiv').show();
	        $('#GBCHDueDateDiv2').hide();
	    } else {
	        $('#GBCH_No').attr('disabled', true);
	        $('#GBCHDueDateDiv').hide();
	        $('#GBCHDueDateDiv2').show();
	    }
	};

    document.getElementById('IsGBCH').onchange = function ()
	{
	    onGBCH();
    };

    // Arguments :
    //  verb : 'GET'|'POST'
    // example: open('POST', 'fileServer.jsp', {request: {key:"42", cols:[2, 3, 34]}}, '_blank');
    // Not: This will override window.open function
    //open = function (verb, url, data, target) {
    //    var form = document.createElement("form");
    //    form.action = url;
    //    form.method = verb;
    //    form.target = target || "_self";
    //    if (data) {
    //        for (var key in data) {
    //            var input = document.createElement("textarea");
    //            input.name = key;
    //            input.value = typeof data[key] === "object" ? JSON.stringify(data[key]) : data[key];
    //            form.appendChild(input);
    //        }
    //    }
    //    form.style.display = 'none';
    //    document.body.appendChild(form);
    //    form.submit();
    //};

	$("#form_div").dialog('close');
	$("#item_div").dialog('close');
	$("#check_div").dialog('close');
	$("#confirm_div").dialog('close');
	$("#paid_div").dialog('close');
	$("#lookup_div_contact").dialog('close');
	$("#lookup_div_cashbank").dialog('close');
	$("#lookup_div_warehouse").dialog('close');
	$("#lookup_div_item").dialog('close');
	$("#delete_confirm_div").dialog('close');


	//GRID +++++++++++++++
	$("#list").jqGrid({
		url: base_url + 'CustomPurchaseInvoice/GetList',
		datatype: "json",
		colNames: ['ID', 'Code', 'Description', 
				   'Discount', 'Tax', 'Delivery Cost', 'Allowance', 'Total', 'CoGS', 'Amount Paid', 'Is GroupPricing', 'Contact ID', 'Contact Name',
                   'Is Confirmed', 'Confirmation Date', 
                   'Is GBCH', 'GBCH No.', 'GBCH Due Date',
				   'CashBank ID', 'CashBank Name', 'Is Bank', 'Is Paid', 'Payment Date', 'Is Full Payment',
				   'Warehouse ID', 'Warehouse Name',
				   'Purchase Date', 'Due Date', 'Created At', 'Updated At'],
		colModel: [
				  { name: 'id', index: 'id', width: 50, align: "center" },
				  { name: 'code', index: 'code', width: 100 },
				  { name: 'description', index: 'description', width: 100 },
				  { name: 'discount', index: 'discount', width: 80, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'tax', index: 'tax', width: 80, decimal: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'shippingfee', index: 'shippingfee', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' } },
				  { name: 'allowance', index: 'allowance', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'total', index: 'total', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' } },
				  { name: 'cogs', index: 'cogs', width: 80, hidden: true, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'amountpaid', index: 'amountpaid', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' } },
                  { name: 'isgrouppricing', index: 'isgrouppricing', hidden: true, width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'contactid', index: 'contactid', width: 80, hidden: true },
                  { name: 'contact', index: 'contact', width: 100 },
                  { name: 'isconfirmed', index: 'isconfirmed', width: 80, boolean: { defaultValue: 'false' }, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
				  { name: 'confirmationdate', index: 'confirmationdate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'isgbch', index: 'isgbch', width: 80, boolean: { defaultValue: 'false' }, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
                  { name: 'gbchno', index: 'gbchno', width: 100, hidden: true },
                  { name: 'gbchduedate', index: 'gbchduedate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'cashbankid', index: 'cashbankid', width: 80, hidden: true },
				  { name: 'cashbank', index: 'cashbank', width: 100 },
				  { name: 'isbank', index: 'isbank', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'ispaid', index: 'ispaid', width: 80, boolean: { defaultValue: 'false' } },
                  { name: 'paymentdate', index: 'paymentdate', hidden: true, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'isfullpayment', index: 'isfullpayment', width: 80, boolean: { defaultValue: 'false' } },
				  { name: 'warehouseid', index: 'warehouseid', width: 80, hidden: true },
				  { name: 'warehouse', index: 'warehouse', width: 100 },
                  { name: 'purchasedate', index: 'purchasedate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
				      rowIsPaid = "YES, " + $(this).getRowData(cl).paymentdate;
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
				  rowIsGBCH = $(this).getRowData(cl).isgbch;
				  if (rowIsGBCH == 'true') {
				      rowIsGBCH = "YES, " + $(this).getRowData(cl).gbchno + ", " + $(this).getRowData(cl).gbchduedate;
				  } else {
				      rowIsGBCH = "NO";
				  }
				  $(this).jqGrid('setRowData', ids[i], { isgbch: rowIsGBCH });
				  rowIsGroupPricing = $(this).getRowData(cl).isgrouppricing;
				  if (rowIsGroupPricing == 'true') {
				      rowIsGroupPricing = "YES";
				  } else {
				      rowIsGroupPricing = "NO";
				  }
				  $(this).jqGrid('setRowData', ids[i], { isgrouppricing: rowIsGroupPricing });
			  }
		  }

	});//END GRID
	$("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

	$('#btn_find').click(function () {
	    ReloadGridBySKU();
	});

	$('#findSKU').keypress(function (e) {
	    if (e.keyCode == 13)
	        $('#btn_find').click();
	});

    //TOOL BAR BUTTON
	$('#btn_reload').click(function () {
		ReloadGrid();
	});

	$('#btn_print').click(function () {
	    //window.open(base_url + 'Print_Forms/Printmstbank.aspx');
	    var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
	    if (id) {
	        window.open(base_url + "Report/ReportPurchaseInvoice?Id=" + id);
	    } else {
	        $.messager.alert('Information', 'Please Select Data...!!', 'info');
	    }
	});

	$('#btn_add_new').click(function () {
		ClearData();
		clearForm('#frm');
		$('#PurchaseDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
		$('#DueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
		$('#Description').removeAttr('disabled');
		$('#btnContact').removeAttr('disabled');
		$('#btnCashBank').removeAttr('disabled');
		$('#btnWarehouse').removeAttr('disabled');
		$('#PurchaseDateDiv2').hide();
		$('#PurchaseDateDiv').show();
		$('#DueDateDiv2').hide();
		$('#DueDateDiv').show();
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
				url: base_url + "CustomPurchaseInvoice/GetInfo?Id=" + id,
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
							$('#ContactId').val(result.ContactId);
							$('#Contact').val(result.Contact);
							$('#CashBankId').val(result.CashBankId);
							$('#CashBankName').val(result.CashBank);
							$('#WarehouseId').val(result.WarehouseId);
							$('#WarehouseName').val(result.Warehouse);
							$('#Total').numberbox('setValue', result.Total);
							$('#PurchaseDate2').val(dateEnt(result.PurchaseDate));
							$('#DueDate2').val(dateEnt(result.DueDate));
							$('#Description').attr('disabled', true);
							$('#btnContact').attr('disabled', true);
							$('#btnCashBank').attr('disabled', true);
							$('#btnWarehouse').attr('disabled', true);
							$('#Total').attr('disabled', true);
							$('#PurchaseDateDiv').hide();
							$('#PurchaseDateDiv2').show();
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
				url: base_url + "CustomPurchaseInvoice/GetInfo?Id=" + id,
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
							$('#PurchaseDate').datebox('setValue', dateEnt(result.PurchaseDate));
							$('#DueDate').datebox('setValue', dateEnt(result.DueDate));
							$('#Total').numberbox('setValue', (result.Total));
							$('#ContactId').val(result.ContactId);
							$('#Contact').val(result.Contact);
							$('#CashBankId').val(result.CashBankId);
							$('#CashBankName').val(result.CashBank);
							$('#WarehouseId').val(result.WarehouseId);
							$('#WarehouseName').val(result.Warehouse);
							$('#Description').removeAttr('disabled');
							$('#btnContact').removeAttr('disabled');
							$('#btnCashBank').removeAttr('disabled');
							$('#btnWarehouse').removeAttr('disabled');
							$('#PurchaseDateDiv2').hide();
							$('#PurchaseDateDiv').show();
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

	$('#btn_check').click(function () {
	    var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
	    if (id) {
	        var ret = jQuery("#list").jqGrid('getRowData', id);
	        $('#checkDueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
	        $('#checkDiscount').numberbox('setValue', ret.discount);
	        $('#checkTax').numberbox('setValue', ret.tax);
	        $('#checkShippingFee').numberbox('setValue', ret.shippingfee);
	        $('#checkAllowance').numberbox('setValue', ret.allowance);
	        $('#idcheck').val(ret.id);
	        $('#checkCode').val(ret.code);
	        $("#check_div").dialog("open");
	    } else {
	        $.messager.alert('Information', 'Please Select Data...!!', 'info');
	    }
	});
   
	$('#check_btn_submit').click(function () {
	    ClearErrorMessage();
	    $.ajax({
	        url: base_url + "CustomPurchaseInvoice/Check?DailySalesProjection=" + $('#DailySalesProjection').numberbox('getValue') + "&IncludeSaturdaySales=" + document.getElementById('IncludeSaturdaySales').checked + "&IncludeSundaySales=" + document.getElementById('IncludeSundaySales').checked,
	        type: "POST",
	        contentType: "application/json",
	        data: JSON.stringify({
	            Id: $('#idcheck').val(), DueDate: $('#checkDueDate').datebox('getValue'),
	            Discount: $('#checkDiscount').numberbox('getValue'), Tax: $('#checkTax').numberbox('getValue'), ShippingFee: $('#checkShippingFee').numberbox('getValue'),
	            Allowance: $('#checkAllowance').numberbox('getValue'),
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
	                $.messager.alert('Check Funds', "Dana Tersedia", 'info');
	                $("#check_div").dialog('close');
	                ReloadGrid();
	            }
	        }
	    });
	});

	$('#check_btn_print').click(function () {
	    window.open(base_url + "Report/ReportFunds?DailySalesProjection=" + $('#DailySalesProjection').numberbox('getValue') + "&IncludeSaturdaySales=" + document.getElementById('IncludeSaturdaySales').checked + "&IncludeSundaySales=" + document.getElementById('IncludeSundaySales').checked +
	        "&Id=" + $('#idcheck').val() + "&DueDate=" + $('#checkDueDate').datebox('getValue') +
	        "&Discount=" + $('#checkDiscount').numberbox('getValue') + "&Tax=" + $('#checkTax').numberbox('getValue') + "&ShippingFee=" + $('#checkShippingFee').numberbox('getValue') + "&Allowance=" + $('#checkAllowance').numberbox('getValue'));
	});


	$('#check_btn_cancel').click(function () {
	    $('#check_div').dialog('close');
	});

	$('#btn_confirm').click(function () {
		var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#list").jqGrid('getRowData', id);
		    //$('#ConfirmationDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
			$('#ConfirmationDate').datebox('setValue', ret.purchasedate);
			$('#confirmDiscount').numberbox('setValue', ret.discount);
			$('#confirmTax').numberbox('setValue', ret.tax);
			$('#confirmShippingFee').numberbox('setValue', ret.shippingfee);
			$('#confirmCode').val(ret.code);
			//$('#confirmAllowance').numberbox('setValue', ret.allowance);
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
						url: base_url + "CustomPurchaseInvoice/UnConfirm",
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
			url: base_url + "CustomPurchaseInvoice/Confirm",
			type: "POST",
			contentType: "application/json",
			data: JSON.stringify({
				Id: $('#idconfirm').val(), ConfirmationDate: $('#ConfirmationDate').datebox('getValue'),
				Discount: $('#confirmDiscount').numberbox('getValue'), Tax: $('#confirmTax').numberbox('getValue'),
				ShippingFee: $('#confirmShippingFee').numberbox('getValue'),
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
				    $("#confirm_div").dialog('close');
				    ReloadGrid();
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
	        //$('#PaymentDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
	        var paymentdate = ret.confirmationdate;
	        if (ret.paymentdate != null && ret.paymentdate != undefined && ret.paymentdate.trim() != "") paymentdate = ret.paymentdate;
	        $('#PaymentDate').datebox('setValue', paymentdate);
	        $('#paidAllowance').numberbox('setValue', ret.allowance);
	        $('#AmountPaid').numberbox('setValue', ret.amountpaid);
	        $('#paidTotal').numberbox('setValue', ret.total);
	        $('#paidCode').val(ret.code);
	        //$('#IsGBCH').val(ret.isgbch);
	        //document.getElementById('IsGBCH').value = 'true';
	        $('#GBCH_No').val(ret.gbchno);
	        $('#GBCHDueDate2').val(ret.gbchduedate);
	        if (ret.gbchduedate != null && $.trim(ret.gbchduedate) != "") {
	            $('#GBCHDueDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date(ret.gbchduedate)));
	        }
	        $('#idpaid').val(ret.id);
	        var e = document.getElementById("IsGBCH");
	        if (ret.isgbch == "NO") {
	            e.selectedIndex = 0;
	        }
	        else {
	            e.selectedIndex = 1;
	        }
	        onGBCH();
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
	                    url: base_url + "CustomPurchaseInvoice/UnPaid",
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
	        url: base_url + "CustomPurchaseInvoice/Paid",
	        type: "POST",
	        contentType: "application/json",
	        data: JSON.stringify({
	            Id: $('#idpaid').val(), PaymentDate: $('#PaymentDate').datebox('getValue'),
	            AmountPaid: $('#AmountPaid').numberbox('getValue'),
	            Allowance: $('#paidAllowance').numberbox('getValue'), 
	            IsGBCH: document.getElementById('IsGBCH').value, GBCH_No: $('#GBCH_No').val(),
	            GBCH_DueDate: $('#GBCHDueDate').datebox('getValue'),
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
			url: base_url + "CustomPurchaseInvoice/Delete",
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
			submitURL = base_url + 'CustomPurchaseInvoice/Update';
		}
			// Insert
		else {
			submitURL = base_url + 'CustomPurchaseInvoice/Insert';
		}

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
				Id: id, Code: $("#Code").val(), Description: $("#Description").val(),
				PurchaseDate: $("#PurchaseDate").datebox('getValue'), DueDate: $("#DueDate").datebox('getValue'),
				ContactId: $('#ContactId').val(), CashBankId: $('#CashBankId').val(), WarehouseId: $('#WarehouseId').val(),
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
		colNames: ['Code', 'CustomPurchaseInvoice Id', 'CustomPurchaseInvoice Code', 'Item Id', 'Item SKU', 'Item Name', 'Quantity', 'Discount', 'Listed Unit Price', 'Amount', 'CoGS'],
		colModel: [
				  { name: 'code', index: 'code', hidden:true, width: 100 },
				  { name: 'cashsalesinvoiceid', index: 'cashsalesinvoiceid', hidden:true, width: 130, sortable: false },
				  { name: 'cashsalesinvoice', index: 'cashsalesinvoice', hidden:true, width: 130 },
				  { name: 'itemid', index: 'itemid', width: 80, sortable: false, hidden: true },
                  { name: 'itemsku', index: 'itemsku', width: 80 },
				  { name: 'item', index: 'item', width: 80 },
				  { name: 'quantity', index: 'quantity', width: 80, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
                  { name: 'discount', index: 'discount', width: 80, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
                  { name: 'listedunitprice', index: 'listedunitprice', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
				  { name: 'amount', index: 'amount', width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
				  { name: 'cogs', index: 'cogs', hidden:true, width: 100, formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0' }, sortable: false },
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
	    $('#detailDiscount').numberbox('setValue', '');
	    $('#ListedUnitPrice').numberbox('setValue', '');
		clearForm('#item_div');
		$('#item_div').dialog('open');
	});

	$('#btn_edit_detail').click(function () {
		ClearData();
		clearForm("#item_div");
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			$.ajax({
				dataType: "json",
				url: base_url + "CustomPurchaseInvoice/GetInfoDetail?Id=" + id,
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
							$('#ListedUnitPrice').numberbox('setValue', result.ListedUnitPrice);
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
						url: base_url + "CustomPurchaseInvoice/DeleteDetail",
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
			submitURL = base_url + 'CustomPurchaseInvoice/UpdateDetail';
		}
			// Insert
		else {
			submitURL = base_url + 'CustomPurchaseInvoice/InsertDetail';
		}

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
			    Id: id, CustomPurchaseInvoiceId: $("#id").val(), ItemId: $("#ItemId").val(),
			    Quantity: $("#Quantity").numberbox('getValue'), Discount: $('#detailDiscount').numberbox('getValue'),
			    ListedUnitPrice: $('#ListedUnitPrice').numberbox('getValue'),
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

    // -------------------------------------------------------Look Up Contact-------------------------------------------------------
	$('#btnContact').click(function () {

	    var lookUpURL = base_url + 'MstContact/GetList';
	    var lookupGrid = $('#lookup_table_contact');
	    lookupGrid.setGridParam({
	        url: lookUpURL
	    }).trigger("reloadGrid");
	    $('#lookup_div_contact').dialog('open');
	});

	$("#lookup_table_contact").jqGrid({
	    url: base_url,
	    datatype: "json",
	    mtype: 'GET',
	    colNames: ['Id', 'Name', 'Description'],
	    colModel: [
				  { name: 'id', index: 'id', hidden:true, width: 80, align: 'right' },
				  { name: 'name', index: 'name', width: 200 },
				  { name: 'description', index: 'description', width: 200 }],
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


    // ---------------------------------------------End Lookup Contact----------------------------------------------------------------


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
		var lookUpURL = base_url + 'MstItem/GetList';
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
		colNames: ['Id', 'SKU', 'Name', 'Item Type', 'UoM'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: 'right', hidden:true },
                  { name: 'sku', index: 'sku', width: 100 },
                  { name: 'name', index: 'name', width: 100 },
                  { name: 'itemtype', index: 'itemtype', width: 120 },
                  { name: 'uom', index: 'uom', width: 100 },
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

			$('#ItemId').val(ret.id).data("kode", id);
			$('#Item').val(ret.name);

			$('#lookup_div_item').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});


	// ---------------------------------------------End Lookup item----------------------------------------------------------------


}); //END DOCUMENT READY