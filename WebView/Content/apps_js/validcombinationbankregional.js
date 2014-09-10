$(document).ready(function () {

    $('#lookup_div_validcombinationbankregional_detail').dialog('close');
    $('#lookup_mstcoa_div').dialog('close');
    //Table GI Detail
    jQuery("#list_validcombinationbankregional").jqGrid({
        datatype: "local",
        height: 80,
        width: 400,
        rowNum: 150,
        scrollrows: true,
        pager: $('#pager_list_validcombinationbankregional'),
        shrinkToFit: false,
        colNames: ['Account Code', 'Title', 'Reference', 'Balance USD', 'Balance IDR', 'Status', 'Date Close',
        ],
        colModel: [
            { name: 'accountcode', index: 'accountcode', width: 100 },
            { name: 'title', index: 'title', width: 100 },
            { name: 'reference', index: 'reference', width: 100 },
            { name: 'balanceusd', index: 'balanceusd', width: 120, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'balanceidr', index: 'balanceidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'status', index: 'status', width: 100 },
            { name: 'dateclose', index: 'dateclose', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
        ]
    });

    $('#validcombinationbankregional_form_btn_reload').click(function () {

    });

    
    //---------------------------------------------VALID COMBINATION BANK COMPANY DETAIL----------------------------------------------------------
    //Open Dialog
    $('#validcombinationbankregional_form_btn_add').click(function () {
        $('#lookup_div_validcombinationbankregional_detail').dialog('open');
    });

    //Close Dialog
    $('#lookup_btn_cancel_validcombinationbankregional_detail').click(function () {
        $('#lookup_div_validcombinationbankregional_detail').dialog('close');
    });
    $('#lookup_btn_add_validcombinationbankregional_detail').click(function () {
        $('#lookup_div_validcombinationbankregional_detail').dialog('close');
    });
    //---------------------------------------------END VALID COMBINATION BANK COMPANY DETAIL------------------------------------------------------

    //---------------------------------------------LOOK UP COA------------------------------------------------------------------------------------

    $('#mstcoa_lookup_add').click(function () {
        $('#lookup_mstcoa_div').dialog('close');
    });

    $('#mstcoa_lookup_cancel').click(function () {
        $('#lookup_mstcoa_div').dialog('close');
    });

    $('#btncoa').click(function () {
        $("#lookup_mstcoa").setGridParam({ url: base_url + 'mstcoa/LookupCOA?accLevel=-1' }).trigger("reloadGrid");
        $('#lookup_mstcoa_div').dialog('open');
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


    $("#mstcoa_lookup_add").click(function () {
        var id = jQuery("#lookup_mstcoa").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_mstcoa").jqGrid('getRowData', id);

            $('#lookup_mstcoa_div').dialog("close");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }

    });
    //---------------------------------------------END LOOK UP COA--------------------------------------------------------------------------------

}); //END DOCUMENT READY