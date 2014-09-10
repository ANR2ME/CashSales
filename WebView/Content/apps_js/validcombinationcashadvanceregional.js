$(document).ready(function () {

    // Initialize
    $('#dialogEdit').dialog('close');
    $('#dialogDelete').dialog('close');

    function ReloadGrid() {


        $("#list_validcombinationcashadvanceregional").setGridParam({ url: base_url + 'generalinvoice/GetOfficialReceiptList', postData: { filters: null, JobId: value }, page: 'last' }).trigger("reloadGrid");
    }

    /*================================================ List ================================================*/
    jQuery("#list_validcombinationcashadvanceregional").jqGrid({
        url: base_url + 'generalinvoice/GetOfficialReceiptList',
        datatype: "json",
        colNames: ['Personal Name', 'Balance USD', 'Balance IDR', 'End Date'],
        colModel: [{ name: 'personalname', index: 'personalname', width: 130, align: "center" },
				  { name: 'balanceusd', index: 'balanceusd', width: 80, align: "center" },
				  { name: 'balanceidr', index: 'balanceidr', width: 80, align: "center" },
				  { name: 'enddate', index: 'enddate', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } }
        ],
        page: '1', // last page
        pager: jQuery('#pager_list_validcombinationcashadvanceregional'),
        altRows: true,
        rowNum: 50,
        rowList: [50, 100, 150],
        imgpath: 'themes/start/images',
        sortname: 'generalno',
        viewrecords: true,
        shrinkToFit: false,
        sortorder: "asc",
        width: $("#validcombinationcashadvanceregional_form_toolbar").width(),
        height: $(window).height() - 200,
    });//END GRID
    $("#list_validcombinationcashadvanceregional").jqGrid('navGrid', '#generalinvoice_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    /*================================================ List ================================================*/

    $("#generalinvoice_btn_reload").click(function () {
        ReloadGrid();
    });





}); //END DOCUMENT READY
