$(document).ready(function () {
	var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;


	function ClearErrorMessage() {
		$('span[class=errormessage]').text('').remove();
	}

	function ReloadGrid() {
		$("#list").setGridParam({ url: base_url + 'BarringWorkOrder/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
	}

	function ReloadGridDetail() {
		$("#listdetail").setGridParam({ url: base_url + 'BarringWorkOrder/GetListDetail?Id=' + $("#id").val(), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
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

	$("#confirm_div").dialog('close');
	$("#finished_div").dialog('close');
	$("#rejected_div").dialog('close');
	$("#form_div").dialog('close');
	$("#item_div").dialog('close');
	$("#lookup_div_contact").dialog('close');
	$("#lookup_div_barring").dialog('close');
	$("#lookup_div_warehouse").dialog('close');
	$("#delete_confirm_div").dialog('close');
	$("#IsCut").hide();
	$("#IsSideSealed").hide();
	$("#IsBarPrepared").hide();
	$("#IsAdhesiveTapeApplied").hide();
	$("#IsBarMounted").hide();
	$("#IsBarHeatPressed").hide();
	$("#IsBarPullOffTested").hide();
	$("#IsQCAndMarked").hide();
	$("#IsPackaged").hide();

	//GRID +++++++++++++++
	$("#list").jqGrid({
		url: base_url + 'BarringWorkOrder/GetList',
		datatype: "json",
		colNames: ['ID', 'Code', 'Contact Id', 'Contact Name', 'Warehouse Id',
					'Warehouse Code', 'Warehouse Name', 'QTY Received', 'QTY Final',
					'QTY Rejected', 'Is Confirmed', 'Confirmation Date', 'Created At', 'Updated At'
		],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'code', index: 'code', width: 100 },
				  { name: 'contactid', index: 'contactid', width: 100 },
				  { name: 'contactname', index: 'contactname', width: 100 },
				  { name: 'warehouseid', index: 'warehouseid', width: 100 },
				  { name: 'warehousecode', index: 'warehousecode', width: 100 },
				  { name: 'warehouse', index: 'warehouse', width: 100 },
				  { name: 'quantityreceived', index: 'quantityreceived', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
				  { name: 'quantityfinal', index: 'quantityfinal', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
				  { name: 'quantityrejected', index: 'quantityrejected', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' }, sortable: false },
				  { name: 'isconfirmed', index: 'isconfirmed', width: 100 },
				  { name: 'confirmationdate', index: 'confirmationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
					  rowIsConfirmed = "YES";
				  } else {
					  rowIsConfirmed = "NO";
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
		$('#btnContact').removeAttr('disabled');
		$('#btnWarehouse').removeAttr('disabled');
		$('#Code').removeAttr('disabled');
		$('#QuantityReceived').removeAttr('disabled');
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
				url: base_url + "BarringWorkOrder/GetInfo?Id=" + id,
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
							$('#Code').val(result.Code);
							$('#ContactId').val(result.ContactId);
							$('#Contact').val(result.Contact);
							$('#WarehouseId').val(result.WarehouseId);
							$('#Warehouse').val(result.Warehouse);
							$('#QuantityReceived').val(result.QuantityReceived);
							$('#form_btn_save').hide();
							$('#btnContact').attr('disabled', true);
							$('#btnWarehouse').attr('disabled', true);
							$('#Code').attr('disabled', true);
							$('#QuantityReceived').attr('disabled', true);
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
				url: base_url + "BarringWorkOrder/GetInfo?Id=" + id,
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
							$('#Code').val(result.Code);
							$('#ContactId').val(result.ContactId);
							$('#Contact').val(result.Contact);
							$('#WarehouseId').val(result.WarehouseId);
							$('#Warehouse').val(result.Warehouse);
							$('#QuantityReceived').val(result.QuantityReceived);
							$('#btnContact').removeAttr('disabled');
							$('#btnWarehouse').removeAttr('disabled');
							$('#Code').removeAttr('disabled');
							$('#QuantityReceived').removeAttr('disabled');
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
						url: base_url + "BarringWorkOrder/Unconfirm",
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
			url: base_url + "BarringWorkOrder/Confirm",
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
			url: base_url + "BarringWorkOrder/Delete",
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
			submitURL = base_url + 'BarringWorkOrder/Update';
		}
			// Insert
		else {
			submitURL = base_url + 'BarringWorkOrder/Insert';
		}

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
				Id: id, ContactId: $("#ContactId").val(),
				WarehouseId: $("#WarehouseId").val(), Code: $("#Code").val(),
				QuantityReceived: $('#QuantityReceived').numberbox('getValue')
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
		colNames: ['BarringOrderId', 'Barring Id', 'Barring Name', 'IsBarRequired', 'HasLeftBar', 'LeftBarId', 'LeftBarName',
					'HasRightBar', 'RightBarId', 'RightBarName','IsCut','IsSideSealed',  'IsBarPrepared',
					'IsAdhesiveTapeApplied','IsBarMounted',    'IsBarHeatPressed',    'IsBarPullOffTested',
					'IsQCAndMarked',   'IsPackaged',  'IsRejected',     'Rejected Date', 'Is Finished' ,'Finished Date' 
		],
		colModel: [
				  { name: 'barringorderid', index: 'barringorderid', width: 100, sortable: false },
				  { name: 'barringid', index: 'barringid', width: 100, sortable: false },
				  { name: 'barringname', index: 'barringname', width: 100, sortable: false },
				  { name: 'isbarrequired', index: 'isbarrequired', width: 100, sortable: false },
				  { name: 'hasleftbar', index: 'hasleftbar', width: 100, sortable: false },
				  { name: 'leftbarid', index: 'leftbarid', width: 100, sortable: false },
				  { name: 'leftbarname', index: 'leftbarname', width: 100, sortable: false },
				  { name: 'hasrightbar', index: 'hasrightbar', width: 100, sortable: false },
				  { name: 'rightbarid', index: 'rightbarid', width: 100, sortable: false },
				  { name: 'rightbarname', index: 'rightbarname', width: 100, sortable: false },
				  { name: 'iscut', index: 'iscut', width: 100, sortable: false },
				  { name: 'issidesealed', index: 'issidesealed', width: 100, sortable: false },
				  { name: 'isbarprepared', index: 'isbarprepared', width: 100, sortable: false },
				  { name: 'isadhesivetapeapplied', index: 'isadhesivetapeapplied', width: 100, sortable: false },
				  { name: 'isbarmounted', index: 'isbarmounted', width: 100, sortable: false },
				  { name: 'isbarheatpressed', index: 'isbarheatpressed', width: 100, sortable: false },
				  { name: 'isbarpullofftested', index: 'isbarpullofftested', width: 100, sortable: false },
				  { name: 'isqcandmarked', index: 'isqcandmarked', width: 100, sortable: false },
				  { name: 'ispackaged', index: 'ispackaged', width: 100, sortable: false },
				  { name: 'isrejected', index: 'isrejected', width: 100, sortable: false },
				  { name: 'rejecteddate', index: 'rejecteddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'isfinished', index: 'isfinished', width: 100, sortable: false },
				  { name: 'finisheddate', index: 'finisheddate', sortable: false, search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },

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
		width: $(window).width() -600,
		height: $(window).height() - 500,
		onSelectRow: function (id) {
			var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
			if (id) {
				$.ajax({
					dataType: "json",
					url: base_url + "BarringWorkOrder/GetInfoDetail?Id=" + id,
					success: function (result) {
						if (result.Id == null) {
							$.messager.alert('Information', 'Data Not Found...!!', 'info');
						}
						else {
							$("#IsCut").hide();
							$("#IsSideSealed").hide();
							$("#IsBarPrepared").hide();
							$("#IsAdhesiveTapeApplied").hide();
							$("#IsBarMounted").hide();
							$("#IsBarHeatPressed").hide();
							$("#IsBarPullOffTested").hide();
							$("#IsQCAndMarked").hide();
							$("#IsPackaged").hide();
							if (JSON.stringify(result.Errors) != '{}') {
								var error = '';
								for (var key in result.Errors) {
									error = error + "<br>" + key + " " + result.model.Errors[key];
								}
								$.messager.alert('Warning', error, 'warning');
							}
							else {
								if (result.IsCut == false) { $("#IsCut").show(); }
								else if (result.IsSideSealed == false) { $("#IsSideSealed").show(); }
								else if (result.IsBarPrepared == false) { $("#IsBarPrepared").show(); }
								else if (result.IsAdhesiveTapeApplied == false) { $("#IsAdhesiveTapeApplied").show(); }
								else if (result.IsBarMounted == false) { $("#IsBarMounted").show(); }
								else if (result.IsBarHeatPressed == false) { $("#IsBarHeatPressed").show(); }
								else if (result.IsBarPullOffTested == false) { $("#IsBarPullOffTested").show(); }
								else if (result.IsQCAndMarked == false) { $("#IsQCAndMarked").show(); }
								else if (result.IsPackaged == false) { $("#IsPackaged").show(); }

							}
						}
					}
				});
			} else {
				$.messager.alert('Information', 'Data Not Found...!!', 'info');
			};
		},
		gridComplete:
		  function () {
		  }
	});//END GRID Detail
	$("#listdetail").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: false });
	//.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

	$('#btn_add_new_detail').click(function () {
		ClearData();
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
				url: base_url + "BarringWorkOrder/GetInfoDetail?Id=" + id,
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
							$("#item_btn_submit").data('kode', result.Id);
							var e = document.getElementById("IsBarRequired");
							if (result.IsBarRequired == true) {
								e.selectedIndex = 0;
							}
							else {
								e.selectedIndex = 1;
							}
							var f = document.getElementById("HasLeftBar");
							if (result.HasLeftBar == true) {
								f.selectedIndex = 0;
							}
							else {
								f.selectedIndex = 1;
							}
							var g = document.getElementById("HasRightBar");
							if (result.HasRightBar == true) {
								g.selectedIndex = 0;
							}
							else {
								g.selectedIndex = 1;
							}
							$('#BarringId').val(result.BarringId);
							$('#Barring').val(result.Barring);
							$('#BarringRightBarId').val(result.BarringRightBarId);
							$('#BarringRightBar').val(result.BarringRightBar);
							$('#BarringLeftBarId').val(result.BarringLeftBarId);
							$('#BarringLeftBar').val(result.BarringLeftBar);
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
						url: base_url + "BarringWorkOrder/DeleteDetail",
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

	$('#btn_finish_detail').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$('#FinishedDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
			$('#idfinished').val(id);
			$("#finished_div").dialog("open");
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#btn_unfinish_detail').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#list").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to unfinish record?', function (r) {
				if (r) {
					$.ajax({
						url: base_url + "BarringWorkOrder/Unfinish",
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
							}
						}
					});
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#finished_btn_submit').click(function () {
		ClearErrorMessage();
		$.ajax({
			url: base_url + "BarringWorkOrder/Finish",
			type: "POST",
			contentType: "application/json",
			data: JSON.stringify({
				Id: $('#idfinished').val(), FinishedDate: $('#FinishedDate').datebox('getValue'),
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
					$("#finished_div").dialog('close');
				}
			}
		});
	});

	$('#finished_btn_cancel').click(function () {
		$('#finished_div').dialog('close');
	});

	$('#btn_reject_detail').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$('#FinishedDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
			$('#idrejected').val(id);
			$("#rejected_div").dialog("open");
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#btn_unreject_detail').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#list").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to unreject record?', function (r) {
				if (r) {
					$.ajax({
						url: base_url + "BarringWorkOrder/Unreject",
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
							}
						}
					});
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#rejected_btn_submit').click(function () {
		ClearErrorMessage();
		$.ajax({
			url: base_url + "BarringWorkOrder/Reject",
			type: "POST",
			contentType: "application/json",
			data: JSON.stringify({
				Id: $('#idrejected').val(), RejectedDate: $('#FinishedDate').datebox('getValue'),
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
					$("#rejected_div").dialog('close');
				}
			}
		});
	});

	$('#rejected_btn_cancel').click(function () {
		$('#rejected_div').dialog('close');
	});

	function UpdateProgress(detailid,progress) {
		$.ajax({
			url: base_url + "BarringWorkOrder/ProgressDetail?Id=" + detailid +"&" +"Progress=" + progress,
			type: "POST",
			contentType: "application/json",
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
					$("#IsCut").hide();
					$("#IsSideSealed").hide();
					$("#IsBarPrepared").hide();
					$("#IsAdhesiveTapeApplied").hide();
					$("#IsBarMounted").hide();
					$("#IsBarHeatPressed").hide();
					$("#IsBarPullOffTested").hide();
					$("#IsQCAndMarked").hide();
					$("#IsPackaged").hide();
					ReloadGridDetail();
				}
			}
		});
	}

	$('#IsCut').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsCut");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#IsSideSealed').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsSideSealed");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#IsBarPrepared').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsBarPrepared");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#IsAdhesiveTapeApplied').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsAdhesiveTapeApplied");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#IsBarMounted').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsBarMounted");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#IsBarHeatPressed').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsBarHeatPressed");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#IsBarPullOffTested').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsBarPullOffTested");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});


	$('#IsQCAndMarked').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsQCAndMarked");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	$('#IsPackaged').click(function () {
		var id = jQuery("#listdetail").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#listdetail").jqGrid('getRowData', id);
			$.messager.confirm('Confirm', 'Are you sure you want to progress record?', function (r) {
				if (r) {
					UpdateProgress(id, "IsPackaged");
				}
			});
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		}
	});

	//--------------------------------------------------------Dialog Item-------------------------------------------------------------
	// coreidentification_btn_submit

	$("#item_btn_submit").click(function () {

		ClearErrorMessage();

		var submitURL = '';
		var id = $("#item_btn_submit").data('kode');

		// Update
		if (id != undefined && id != '' && !isNaN(id) && id > 0) {
			submitURL = base_url + 'BarringWorkOrder/UpdateDetail';
		}
			// Insert
		else {
			submitURL = base_url + 'BarringWorkOrder/InsertDetail';
		}
		var e = document.getElementById("IsBarRequired");
		var isbar = e.options[e.selectedIndex].value;
		var f = document.getElementById("HasLeftBar");
		var hasleft = f.options[f.selectedIndex].value;
		var g = document.getElementById("HasRightBar");
		var hasright = g.options[g.selectedIndex].value;

		$.ajax({
			contentType: "application/json",
			type: 'POST',
			url: submitURL,
			data: JSON.stringify({
				Id: id, BarringId: $("#BarringId").val(), IsBarRequired: isbar, HasLeftBar: hasleft,
				HasRightBar: hasright, BarringOrderId: $("#id").val()
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


	// coreidentification_btn_cancel
	$('#item_btn_cancel').click(function () {
		clearForm('#item_div');
		$("#item_div").dialog('close');
	});
	//--------------------------------------------------------END Dialog Item-------------------------------------------------------------

	// -------------------------------------------------------Look Up warehouse-------------------------------------------------------
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
		colNames: ['Id', 'Code','Name'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: 'right' },
				   { name: 'code', index: 'code', width: 200 },
				  { name: 'name', index: 'name', width: 200 }],
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
	$("#lookup_table_warehouse").jqGrid('navGrid', '#lookup_toolbar_warehouse', { del: false, add: false, edit: false, search: false })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

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
			$('#Warehouse').val(ret.name);

			$('#lookup_div_warehouse').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});


	// ---------------------------------------------End Lookup warehouse----------------------------------------------------------------


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


	// -------------------------------------------------------Look Up barring-------------------------------------------------------
	$('#btnBarring').click(function () {
		var lookUpURL = base_url + 'MstBarring/GetList';
		var lookupGrid = $('#lookup_table_barring');
		lookupGrid.setGridParam({
			url: lookUpURL
		}).trigger("reloadGrid");
		$('#lookup_div_barring').dialog('open');
	});

	jQuery("#lookup_table_barring").jqGrid({
		url: base_url,
		datatype: "json",
		mtype: 'GET',
		colNames: ['Id','Name'],
		colModel: [
				  { name: 'id', index: 'id', width: 80, align: 'right' },
				 
				  { name: 'name', index: 'name', width: 200 }
		],

		page: '1',
		pager: $('#lookup_pager_barring'),
		rowNum: 20,
		rowList: [20, 30, 60],
		sortname: 'id',
		viewrecords: true,
		scrollrows: true,
		shrinkToFit: false,
		sortorder: "ASC",
		width: $("#lookup_div_barring").width() - 10,
		height: $("#lookup_div_barring").height() - 110,
	});
	$("#lookup_table_barring").jqGrid('navGrid', '#lookup_toolbar_barring', { del: false, add: false, edit: false, search: false })
		   .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

	// Cancel or CLose
	$('#lookup_btn_cancel_barring').click(function () {
		$('#lookup_div_barring').dialog('close');
	});

	// ADD or Select Data
	$('#lookup_btn_add_barring').click(function () {
		var id = jQuery("#lookup_table_barring").jqGrid('getGridParam', 'selrow');
		if (id) {
			var ret = jQuery("#lookup_table_barring").jqGrid('getRowData', id);

			$('#BarringId').val(ret.id).data("kode", id);
			$('#Barring').val(ret.name);

			$('#lookup_div_barring').dialog('close');
		} else {
			$.messager.alert('Information', 'Please Select Data...!!', 'info');
		};
	});


	// ---------------------------------------------End Lookup barring----------------------------------------------------------------

}); //END DOCUMENT READY