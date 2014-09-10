$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ClearGridFilter() {
        var grid = $("#list");
        grid[0].clearToolbar();
    }

    function ReloadGrid() {
        //ClearGridFilter();
        $("#list").setGridParam({ url: base_url + 'CashMutation/GetList', page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridByDate(StartDate, EndDate) {
        $("#list").setGridParam({ url: base_url + 'CashMutation/GetListByDate', postData: { filters: null, startdate: StartDate, enddate: EndDate }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $('#search_div').dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'CashMutation/GetList',
        datatype: "json",
        colNames: ['ID', 'CashBank Id', 'CashBank Name',
                   'Amount', 'Source', 'Source Id', 'Source Code', 'Mutation Date', 'Created At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 60, align: "center" },
    			  { name: 'cashbankid', index: 'cashbankid', width: 35, align: "center", hidden: true},
				  { name: 'cashbank', index: 'cashbank', width: 120 },
                  { name: 'amount', index: 'amount', width: 150, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
				  { name: 'sourcedocumenttype', index: 'sourcedocumenttype', width: 120, align: 'right' },
				  { name: 'sourcedocumentid', index: 'sourcedocumentid', width: 60, hidden: true },
                  { name: 'sourcedocumentcode', index: 'sourcedocumentcode', width: 80, stype: 'text' },
                  { name: 'mutationdate', index: 'mutationdate', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' }, searchoptions: { sopt: ['eq'] } },
                  { name: 'createdat', index: 'createdat', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGrid();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    $('#btn_search').click(function () {
        $('#StartDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#EndDate').datebox('setValue', $.datepicker.formatDate('mm/dd/yy', new Date()));
        $("#search_div").dialog("open");
    });

    $('#search_btn_submit').click(function () {
        ClearErrorMessage();
        ReloadGridByDate($('#StartDate').datebox('getValue'), $('#EndDate').datebox('getValue'));
        $("#search_div").dialog('close');
    });

    $('#search_btn_cancel').click(function () {
        $('#search_div').dialog('close');
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
                this.selectedIndex = 0;
        });
    }
}); //END DOCUMENT READY