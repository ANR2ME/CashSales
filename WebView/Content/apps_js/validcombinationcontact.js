$(document).ready(function () {

    function ReloadGrid() {

        var custType = $('#ddlcontacttype').val();

        $("#table_detail").setGridParam({
            url: base_url + 'ValidCombinationContact/GetValidCombContactList',
            postData: { filters: null, customertype: custType }, page: 'last'
        }).trigger("reloadGrid");
    }

    //Table Detail
    jQuery("#table_detail").jqGrid({
        url: base_url + 'ValidCombinationContact/GetValidCombContactList',
        datatype: "json",
        colNames: ['Period', 'Year', 'ContactId', 'Code', 'Name', 'Begin Period Closing', 'End Period Closing', 'Current Debit USD',
        'Current Debit IDR', 'Current Credit USD', 'Current Credit IDR', 'Current Debit in IDR', 'Current Credit in IDR', 'Previous Debit USD',
        'Previous Debit IDR', 'Previous Credit USD', 'Previous Credit IDR', 'Previous Debit in IDR', 'Previous Credit in IDR', 'Close', 'Date Close', 'Company'],
        colModel: [
            { name: 'period', index: 'period', align: "center", width: 40 },
            { name: 'yearperiod', index: 'yearperiod', align: "center", width: 60 },
            { name: 'contactid', index: 'contactid', width: 120, hidden: true },
            { name: 'contactcode', index: 'contactcode', width: 100, align: "center" },
            { name: 'contactname', index: 'contactname', width: 200 },
            { name: 'beginperiodclosing', index: 'beginperiodclosing', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
            { name: 'endperiodclosing', index: 'endperiodclosing', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
            { name: 'currentdebitusd', index: 'currentdebitusd', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'currentdebitidr', index: 'currentdebitidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'currentcreditusd', index: 'currentcreditusd', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'currentcreditidr', index: 'currentcreditidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'currentdebitinidr', index: 'currentdebitinidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'currentcreditinidr', index: 'currentcreditinidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'previousdebitusd', index: 'previousdebitusd', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'previousdebitidr', index: 'previousdebitidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'previouscreditusd', index: 'previouscreditusd', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'previouscreditidr', index: 'previouscreditidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'previousdebitindr', index: 'previousdebitindr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'previouscreditinidr', index: 'previouscreditinidr', width: 115, align: "right", formatter: 'currency', formatoptions: { decimalSeparator: ".", thousandsSeparator: ",", decimalPlaces: 2, prefix: "", suffix: "", defaultValue: '0.00' } },
            { name: 'close', index: 'close', width: 80 },
            { name: 'dateclose', index: 'dateclose', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
            { name: 'company', index: 'company', width: 150 }
        ],
        page: 'last', // last page
        pager: jQuery('#pager_list_detail'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'yearperiod',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "asc",
        width: $(window).width() - 75,
        height: $(window).height() - 500,
        onSelectRow: function (id) {
            $('#txtbeginperiod').val($(this).getRowData(id).period);
            $('#textyearbeginperiod').val($(this).getRowData(id).yearperiod);
            $('#txtbeginperioddateclosing').val($(this).getRowData(id).beginperiodclosing);
            $('#txtendperioddateclosing').val($(this).getRowData(id).endperiodclosing);
            $('#txtcontactcode').val($(this).getRowData(id).contactcode);
            $('#txtcontactname').val($(this).getRowData(id).contactname);
            $('#txtdebitusd').val($(this).getRowData(id).previousdebitusd);
            $('#txtcreditusd').val($(this).getRowData(id).previouscreditusd);
            $('#txtdebitinidr').val($(this).getRowData(id).previousdebitindr);
            $('#txtcreditinidr').val($(this).getRowData(id).previouscreditinidr);
            $('#txtdebitidr').val($(this).getRowData(id).previousdebitidr);
            $('#txtcreditidr').val($(this).getRowData(id).previouscreditidr);

            var totalDebitIdr = 0;
            totalDebitIdr = parseFloat($('#txtdebitidr').val()) + parseFloat($('#txtdebitinidr').val());
            if (isNaN(totalDebitIdr))
                totalDebitIdr = 0;
            totalCreditIdr = parseFloat($('#txtcreditidr').val()) + parseFloat($('#txtcreditinidr').val());
            if (isNaN(totalCreditIdr))
                totalCreditIdr = 0;

            $('#txttotaldebitinidr').val(totalDebitIdr);
            $('#txttotalcreditinidr').val(totalCreditIdr);
        },
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

                rowApproved = $(this).getRowData(cl).approved;
                if (rowApproved == 'true') {
                    rowApproved = "YES";
                } else {
                    rowApproved = "";
                }
                $(this).jqGrid('setRowData', ids[i], { approved: rowApproved });

                rowPaid = $(this).getRowData(cl).paid;
                if (rowPaid == 'true') {
                    rowPaid = "YES";
                } else {
                    rowPaid = "";
                }
                $(this).jqGrid('setRowData', ids[i], { paid: rowApproved });
            }
        }
    });//END GRID
    $("#table_detail").jqGrid('navGrid', '#cashadvance_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#validcombination_form_btn_reload').click(function () {
        ReloadGrid();
    });

    $('#ddlcontacttype').live("change", function () {
        ReloadGrid();
    });

}); //END DOCUMENT READY