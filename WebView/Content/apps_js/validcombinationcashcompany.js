$(document).ready(function () {

    $('#lookup_div_validcombinationcashcompany_detail').dialog('close');
    $('#lookup_mstcoa_div').dialog('close');
    

    function ReloadGrid() {
        $("#list_validcombinationcashcompany").setGridParam({
            url: base_url + 'ValidCombinationCashCompany/GetValidCombCashList',
            postData: { filters: null }, page: 'last'
        }).trigger("reloadGrid");
    }


    jQuery("#list_validcombinationcashcompany").jqGrid({
        url: base_url + 'ValidCombinationCashCompany/GetValidCombCashList',
        datatype: "json",
        colNames: ['Account Code', 'Title', 'Balance USD', 'Balance IDR', 'Status', 'Date Close'],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 120 },
            { name: 'title', index: 'AccountName', width: 300 },
            { name: 'balanceusd', index: 'balanceusd', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'balanceidr', index: 'balanceidr', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'status', index: 'Closing', width: 100 },
            { name: 'dateclose', index: 'ClosingDate', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
        ],
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'ClosingDate',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "desc",
        scrollrows: true,
        pager: $('#pager_list_validcombinationcashcompany'),
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
    $("#list_validcombinationcashcompany").jqGrid('navGrid', '#generalinvoice_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#validcombinationcashcompany_form_btn_reload').click(function () {
        ReloadGrid();
    });


    //---------------------------------------------VALID COMBINATION BANK COMPANY DETAIL----------------------------------------------------------
    //Open Dialog
    $('#validcombinationcashcompany_form_btn_add').click(function () {
        $('#lookup_div_validcombinationcashcompany_detail').dialog('open');
    });

    //Close Dialog
    $('#lookup_btn_cancel_validcombinationcashcompany_detail').click(function () {
        $('#lookup_div_validcombinationcashcompany_detail').dialog('close');
    });

    $('#lookup_btn_add_validcombinationcashcompany_detail').click(function () {

        var ValidId="";
        var submitURL = "";
        if (ValidId == undefined || ValidId == "")
            submitURL = base_url + 'ValidCombinationCashCompany/Insert'
        else
            submitURL = base_url + 'ValidCombinationCashCompany/Update'

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
                        $('#lookup_div_validcombinationcashcompany_detail').dialog('close');
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

    //---------------------------------------------LOOK UP COA - CASH ------------------------------------------------------------------------------------

    // Browse
    $('#btncoa').click(function () {
        $("#lookup_mstcoa").setGridParam({ url: base_url + 'MstCoa/LookUpCOAValidCombCash' }).trigger("reloadGrid");
        $('#lookup_mstcoa_div').dialog('open');
    });

    // Add
    $('#mstcoa_lookup_add').click(function () {
        var id = jQuery("#lookup_mstcoa").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_mstcoa").jqGrid('getRowData', id);

            $('#txtaccountcode').val(ret.accountcode).data("kode", id);
            $('#txtaccountname').val(ret.accounttitle);

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
        colNames: ['Account Code', 'Account Name', 'Account Type'],
        colModel: [
                  { name: 'accountcode', index: 'accountcode', width: 80 },
                  { name: 'accounttitle', index: 'accounttitle', width: 150 },
                  { name: 'accountype', index: 'accounttype', width: 100 }
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