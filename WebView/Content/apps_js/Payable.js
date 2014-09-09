$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'Payable/GetList' }).trigger("reloadGrid");
    }

    function ReloadGridByDate(StartDate, EndDate) {
        $("#list").setGridParam({ url: base_url + 'Payable/GetListByDate', postData: { startdate: StartDate, enddate: EndDate } }).trigger("reloadGrid");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $('#search_div').dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Payable/GetList',
        datatype: "json",
        colNames: ['ID', 'Contact Id', 'Contact Name', 'Code', 'Source', 'Source Id', 'Source Code',
                   'Amount', 'Remaining Amount', 'Pending Clearance Amount', 'Allowance Amount',
                   'Due Date', 'Is Completed', 'Completion Date', 'Created At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 60, align: "center" },
    			  { name: 'contactid', index: 'contactid', width: 35, align: "center", hidden: true},
				  { name: 'contact', index: 'contact', width: 120, search: true },
                  { name: 'code', index: 'code', width: 80 },
				  { name: 'payablesource', index: 'payablesource', width: 120, align: 'right' },
				  { name: 'payablesourceid', index: 'payablesourceid', width: 60, hidden: true },
                  { name: 'payablesourcecode', index: 'payablesourcecode', width: 80 },
                  { name: 'amount', index: 'amount', width: 150, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'remainingamount', index: 'amount', width: 125, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingclearanceamount', index: 'pendingclearanceamount', width: 160, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'allowanceamount', index: 'allowanceamount', width: 125, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'duedate', index: 'duedate', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'iscompleted', index: 'iscompleted', width: 100, stype: 'select', editoptions: { value: ':All;true:Yes;false:No' } },
                  { name: 'completiondate', index: 'completiondate', hidden: true, search: false, width: 105, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsCompleted = $(this).getRowData(cl).iscompleted;
		          if (rowIsCompleted == 'true') {
		              rowIsCompleted = "YES, " + $(this).getRowData(cl).completiondate;
		          } else {
		              rowIsCompleted = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { iscompleted: rowIsCompleted });

		      }
		  }

    });//END GRID
    $("#list").jqGrid('navGrid', '#toolbar_cont', { del: false, add: false, edit: false, search: true })
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