$(document).ready(function () {

    $('#lookup_div_validcombinationbankcompany_detail').dialog('close');
    $('#lookup_mstcoa_div').dialog('close');

    function ReloadGrid() {
        $("#list_validcombinationbankcompany").setGridParam({
            url: base_url + 'ValidCombinationBankCompany/GetValidCombBankList',
            postData: { filters: null }, page: 'last'
        }).trigger("reloadGrid");
    }

    jQuery("#list_validcombinationbankcompany").jqGrid({
        url: base_url + 'ValidCombinationBankCompany/GetValidCombBankList',
        datatype: "json",
        colNames: ['Account Code', 'Title', 'Balance USD', 'Balance IDR', 'Status', 'Date Close'],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 120 },
            { name: 'title', index: 'title', width: 300 },
            { name: 'balanceusd', index: 'balanceusd', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'balanceidr', index: 'balanceidr', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'status', index: 'status', width: 100 },
            { name: 'dateclose', index: 'dateclose', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
        ],
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'accountcode',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "asc",
        scrollrows: true,
        pager: $('#pager_list_validcombinationbankcompany'),
        width: $("#validcombinationcashadvancecompany_form_toolbar").width(),
        height: $(window).height() - 200,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowDel = $(this).getRowData(cl).deleted;
		          if (rowDel == 'true') {
		              img = "<img src ='" + base_url + "content/assets/images/remove.png' title='Data has been deleted !' width='16px' height='16px'>";
		          } else {
		              img = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { deleted: img });

		          rowStatus = $(this).getRowData(cl).status;
		          if (rowStatus == 'true') {
		              rowStatus = "Close";
		          } else {
		              rowStatus = "";
		          }
		          $(this).jqGrid('setRowData', ids[i], { status: rowStatus });

		      }
		  }
    });
    $("#list_validcombinationbankcompany").jqGrid('navGrid', '#generalinvoice_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#validcombinationbankcompany_form_btn_reload').click(function () {
        ReloadGrid();
    });


    //---------------------------------------------VALID COMBINATION BANK COMPANY DETAIL----------------------------------------------------------
    //Open Dialog
    $('#validcombinationbankcompany_form_btn_add').click(function () {
        $('#lookup_div_validcombinationbankcompany_detail').dialog('open');
    });

    //Close Dialog
    $('#lookup_btn_cancel_validcombinationbankcompany_detail').click(function () {
        $('#lookup_div_validcombinationbankcompany_detail').dialog('close');
    });

    // Save
    $('#lookup_btn_add_validcombinationbankcompany_detail').click(function () {
        var ValidId = "";
        var submitURL = "";
        if (ValidId == undefined || ValidId == "")
            submitURL = base_url + 'ValidCombinationBankCompany/Insert'
        else
            submitURL = base_url + 'ValidCombinationBankCompany/Update'

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: ValidId,
                AccountId: $('#txtaccountcode').data("kode"),
                ClosingDate: $('#txtdate').datebox("getValue"),
                BalanceIDR: $('#txtbeginningbalanceidr').numberbox("getValue"),
                BalanceUSD: $('#txtbeginningbalanceusd').numberbox("getValue")
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        $('#lookup_div_validcombinationbankcompany_detail').dialog('close');
                        ReloadGrid();
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    });
    //---------------------------------------------END VALID COMBINATION BANK COMPANY DETAIL------------------------------------------------------

    //---------------------------------------------LOOK UP COA------------------------------------------------------------------------------------

    // Browse
    $('#btncoa').click(function () {
        $("#lookup_mstcoa").setGridParam({ url: base_url + 'MstCOA/LookUpCOAValidCombBank' }).trigger("reloadGrid");
        $('#lookup_mstcoa_div').dialog('open');
    });

    // Add
    $('#mstcoa_lookup_add').click(function () {
        var id = jQuery("#lookup_mstcoa").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_mstcoa").jqGrid('getRowData', id);

            $('#txtaccountcode').val(ret.accountcode).data("kode", id);
            $('#txtaccountname').val(ret.bankname);

            $('#lookup_mstcoa_div').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    // Close
    $('#mstcoa_lookup_cancel').click(function () {
        $('#lookup_mstcoa_div').dialog('close');
    });

    $("#lookup_mstcoa").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        colNames: ['Account Code', 'Account Name', 'Acc No.', 'Currency'],
        colModel: [
				  { name: 'accountcode', index: 'accountcode', width: 80 },
				  { name: 'bankname', index: 'bankname', width: 150 },
                  { name: 'accno', index: 'accno', width: 80 },
                  { name: 'currency', index: 'currency', width: 80 }
        ],
        page: '1',
        pager: $('#pager_lookup_mstcoa'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'accountcode',
        viewrecords: true,
        sortorder: "ASC",
        width: $("#lookup_mstcoa_div").width() - 10,
        height: $("#lookup_mstcoa_div").height() - 100
    });
    $("#lookup_mstcoa").jqGrid('navGrid', '#form_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });


    //---------------------------------------------END LOOK UP COA--------------------------------------------------------------------------------

}); //END DOCUMENT READY