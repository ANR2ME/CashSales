$(document).ready(function () {

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list_mstcoa").setGridParam({ url: base_url + 'ChartOfAccount/GetList'/*, postData: { filters: null }, page: '1'*/ }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#Name').val('').text('').removeClass('errormessage');
        $('#Code').val('').text('').removeClass('errormessage');
        $('#mstcoa_form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

    $("#delete_confirm_div").dialog('close');
    $("#lookup_div_coa").dialog('close');
    $("#mstcoa_form_div").dialog('close');
    $('#ParentId').hide();
    $('#ParentCode').hide();

    $("#list_mstcoa").jqGrid({
        url: base_url + 'ChartOfAccount/GetList',
        datatype: "json",
        colNames: ['ID', 'Account Code', 'Account Name', 'Group', 'Level', 'Parent ID', 'Parent Code', 'Parent Name', 'Legacy','Cash&Bank', 'Legacy Code', 'Leaf', 'Used By System'],
        colModel: [
				  { name: 'Id', index: 'Id', width: 40, hidden: true},
				  { name: 'Code', index: 'Code', width: 120, classes: "grid-col" },
				  { name: 'name', index: 'name', width: 250 },
                  { name: 'group', index: 'group', width: 90, formatter:'select', stype: 'select', editoptions: { value: getSelectOption("#AccountGroup") } },
                  { name: 'level', index: 'level', width: 50 },
                  { name: 'parentid', index: 'parentid', width: 40, hidden: true },
                  { name: 'parentcode', index: 'parentcode', width: 80, classes: "grid-col" },
                  { name: 'parent', index: 'parent', width: 200 },
                  { name: 'islegacy', index: 'islegacy', width: 40, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
                  { name: 'iscashbankaccount', index: 'iscashbankaccount', width: 70, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
                  { name: 'legacycode', index: 'legacycode', width: 80, hidden: true },
                  { name: 'isleaf', index: 'isleaf', width: 40, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
                  { name: 'isusedbysystem', index: 'isusedbysystem', width: 90, stype: 'select', editoptions: { value: ':;true:Y;false:N' } },
        ],
        page: '1',
        pager: $('#pager_mstcoa'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Code',
        scrollrows: true,
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "ASC",
        width: $("#mstcoa_toolbar").width(),
        height: $(window).height() - 200,
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

            rowIsSystem = $(this).getRowData(cl).isusedbysystem;
            if (rowIsSystem == 'true') {
                rowIsSystem = "Y";
            } else {
                rowIsSystem = "N";
            }
            $(this).jqGrid('setRowData', ids[i], { isusedbysystem: rowIsSystem });

            //rowGroup = $(this).getRowData(cl).group;
            //if (rowGroup == 1) {
            //    rowGroup = "Asset";
            //} else if (rowGroup == 2){
            //    rowGroup = "Expense";
            //} else if (rowGroup == 3) {
            //    rowGroup = "Liability";
            //} else if (rowGroup == 4) {
            //    rowGroup = "Equity";
            //} else if (rowGroup == 5) {
            //    rowGroup = "Revenue";
            //}
            //$(this).jqGrid('setRowData', ids[i], { group: rowGroup});

        }
    }
      
});
    $("#list_mstcoa").jqGrid('navGrid', '#toolbar_coa', { del: false, add: false, edit: false, search: false/*, refreshstate: "current"*/ })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //GRID LOOKUP +++++++++++++++
    $("#lookup_table_coa").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        colNames: ['Id', 'Account Code', 'Account Name', 'Account Group'],
        colModel: [
                  { name: 'Id', index: 'Id', width: 40, hidden: true },
				  { name: 'Code', index: 'Code', width: 120 },
				  { name: 'Name', index: 'Name', width: 150 },
                  { name: 'Group', index: 'Group', width: 90, formatter: 'select', stype: 'select', editoptions: { value: getSelectOption("#AccountGroup") } },
            ],
        page: '1',
        pager: $('#lookup_pager_coa'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'Code',
        viewrecords: true,
        sortorder: "ASC",
        width: $("#lookup_div_coa").width() - 10,
        height: $("#lookup_div_coa").height() - 115,
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

                 //rowGroup = $(this).getRowData(cl).Group;
                 //if (rowGroup == 1) {
                 //    rowGroup = "Asset";
                 //} else if (rowGroup == 2) {
                 //    rowGroup = "Expense";
                 //} else if (rowGroup == 3) {
                 //    rowGroup = "Liability";
                 //} else if (rowGroup == 4) {
                 //    rowGroup = "Equity";
                 //} else if (rowGroup == 5) {
                 //    rowGroup = "Revenue";
                 //}
                 //$(this).jqGrid('setRowData', ids[i], { Group: rowGroup });

             }
         }
    });
    $("#lookup_table_coa").jqGrid('navGrid', '#lookup_toolbar_coa', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //END GRID

    $('#mstcoa_btn_reload').click(function () {
        ReloadGrid();
    });

    $('#mstcoa_btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstcoa.aspx');
    });

    $('#btncoa').click(function () {
        var v_accGroup = $("input[name='radiogroup']:checked").val();
        var v_accLevel = $("input[name='radiolevel']:checked").val() - 1;
        if (v_accLevel > -1) {
            $("#lookup_table_coa").setGridParam({ url: base_url + 'ChartOfAccount/Lookup?Level=' + v_accLevel + '&Group=' + v_accGroup }).trigger("reloadGrid");
            $('#lookup_div_coa').dialog('open');
        }
    });

    $('#mstcoa_btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        var id = jQuery("#list_mstcoa").jqGrid('getGridParam', 'selrow');
        if (id) {
            // vStatusSaving = 1;//edit data mode
            var ret = jQuery("#list_mstcoa").jqGrid('getRowData', id);
            $('#ParentId').val(ret.Id);
            $('#ParentCode').val(ret.Code);
            $('#ParentName').val(ret.name);
            $('#Code').val(ret.Code);
            switch (ret.group) {
                case ("1"): //"Asset"
                    $('#radiogroup1').prop('checked', true);
                    break;
                case ("2"): //"Expense"
                    $('#radiogroup2').prop('checked', true);
                    break;
                case ("3"): //"Liability"
                    $('#radiogroup3').prop('checked', true);
                    break;
                case ("4"): //"Equity"
                    $('#radiogroup4').prop('checked', true);
                    break;
                case ("5"): //"Revenue"
                    $('#radiogroup5').prop('checked', true);
                    break;
            }
            switch (parseInt(ret.level) + 1) {
                case (1):
                    $('#radiolevel1').prop('checked', true);
                    break;
                case (2):
                    $('#radiolevel2').prop('checked', true);
                    break;
                case (3):
                    $('#radiolevel3').prop('checked', true);
                    break;
                case (4):
                    $('#radiolevel4').prop('checked', true);
                    break;
                case (5):
                    $('#radiolevel5').prop('checked', true);
                    break;
            }
        }
        $('#mstcoa_form_div').dialog('open');
    });

    $('#mstcoa_form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#mstcoa_form_div").dialog('close');
    });

    $('#mstcoa_btn_edit').click(function () {        
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list_mstcoa").jqGrid('getGridParam', 'selrow');
        if (id) {
            $.ajax({
                dataType: "json",
                url: base_url + "ChartOfAccount/GetInfo?Id=" + id,
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
                            $("#mstcoa_form_btn_save").data('kode', id);
                            $('#Code').val(result.Code);
                            $('#Name').val(result.Name);
                            $('#ParentId').val(result.ParentId);
                            $('#ParentCode').val(result.ParentCode);
                            $('#ParentName').val(result.Parent);
                            $('#LegacyCode').val(result.LegacyCode);

                            switch (result.Group) {
                                case (1):
                                    $('#radiogroup1').prop('checked', true);
                                    break;
                                case (2):
                                    $('#radiogroup2').prop('checked', true);
                                    break;
                                case (3):
                                    $('#radiogroup3').prop('checked', true);
                                    break;
                                case (4):
                                    $('#radiogroup4').prop('checked', true);
                                    break;
                                case (5):
                                    $('#radiogroup5').prop('checked', true);
                                    break;
                            }

                            switch (result.Level) {
                                case (1) :
                                    $('#radiolevel1').prop('checked', true);
                                    break;
                                case (2) :
                                    $('#radiolevel2').prop('checked', true);
                                    break;
                                case (3) :
                                    $('#radiolevel3').prop('checked', true);
                                    break;
                                case (4) :
                                    $('#radiolevel4').prop('checked', true);
                                    break;
                                case (5) :
                                    $('#radiolevel5').prop('checked', true);
                                    break;
                            }
                            $('#mstcoa_form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#mstcoa_btn_del').click(function () {
        clearForm("#frm");

        var id = jQuery("#list_mstcoa").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_mstcoa").jqGrid('getRowData', id);

            $('#delete_confirm_btn_submit').data('Code', id);
            $("#delete_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_id').val('');
        $('#delete_confirm_name').text('');

        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {
        $.ajax({
            url: base_url + "ChartOfAccount/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Code')
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


    $("#mstcoa_form_btn_save").click(function () {

        var code = $('#Code').val();
        var name = $('#Name').val();
        var parentid = $('#ParentId').val();
        var level = $("input[name='radiolevel']:checked").val()
        var group = $("input[name='radiogroup']:checked").val()
        var legacycode = $('#LegacyCode').val();

        var submitURL = '';
        var id = $("#mstcoa_form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'ChartOfAccount/Update';
        }
        // Insert
        else {
            submitURL = base_url + 'ChartOfAccount/Insert';
        }

        $.ajax({
            url: submitURL,
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: id, Code: code, Name: name, ParentId: parentid,
                Group: group, Level: level, LegacyCode: legacycode
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
                    $("#mstcoa_form_div").dialog('close')
                }
            }
        });
    });

    $("#lookup_btn_add_coa").click(function () {

        var id = jQuery("#lookup_table_coa").jqGrid('getGridParam', 'selrow');

        if (id) {
           // vStatusSaving = 1;//edit data mode
            var ret = jQuery("#lookup_table_coa").jqGrid('getRowData', id);
            $('#ParentId').val(ret.Id);
            $('#ParentCode').val(ret.Code).data('kode', id);
            $('#ParentName').val(ret.Name);
            $('#Code').val(ret.Code);

            $("#lookup_div_coa").dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $("#lookup_btn_cancel_coa").click(function () {
        $("#lookup_div_coa").dialog('close');
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
                this.selectedIndex = -1;
            $('#Code').data('kode', 0);
        });
    }


}); //END DOCUMENT READY