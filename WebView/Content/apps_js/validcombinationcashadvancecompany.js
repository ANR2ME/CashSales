$(document).ready(function () {

    // Initialize
    $('#dialogEdit').dialog('close');
    $('#dialogDelete').dialog('close');

    function ReloadGrid() {
        $("#list_validcombinationcashadvancecompany").setGridParam({
            url: base_url + 'ValidCombinationBalanceCashCompany/GetValidCombCashAdvanceList',
            postData: { filters: null }, page: 'last'
        }).trigger("reloadGrid");
    }

    /*================================================ List ================================================*/
    jQuery("#list_validcombinationcashadvancecompany").jqGrid({
        url: base_url + 'ValidCombinationBalanceCashCompany/GetValidCombCashAdvanceList',
        datatype: "json",
        colNames: ['Personal Name', 'Balance USD','Balance IDR', 'End Date'],
        colModel: [{ name: 'personalname', index: 'employeename', width: 200, align: "center" },
				  { name: 'balanceusd', index: 'balanceusd', width: 100, align: "center" },
				  { name: 'balanceidr', index: 'balanceidr', width: 100, align: "center" },
				  { name: 'enddate', index: 'enddate', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } }
        ],
        page: '1', // last page
        pager: jQuery('#pager_list_validcombinationcashadvancecompany'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'EndDate',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "desc",
        width: $("#validcombinationcashadvancecompany_form_toolbar").width(),
        height: $(window).height() - 200,      
    });//END GRID
    $("#list_validcombinationcashadvancecompany").jqGrid('navGrid', '#generalinvoice_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    /*================================================ List ================================================*/

    $("#validcombinationcashadvancecompany_form_btn_reload").click(function () {
        ReloadGrid();
    });



   

}); //END DOCUMENT READY
