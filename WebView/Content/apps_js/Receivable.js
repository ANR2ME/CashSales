$(document).ready(function () {
    var vStatusSaving,//Status Saving data if its new or edit
		vMainGrid,
		vCode;
   

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'Receivable/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridByDate(StartDate, EndDate) {
        $("#list").setGridParam({ url: base_url + 'Receivable/GetListByDate', postData: { filters: null, startdate: StartDate, enddate: EndDate }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        ClearErrorMessage();
    }

    $("#form_div").dialog('close');
    $('#search_div').dialog('close');

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'Receivable/GetList',
        datatype: "json",
        colNames: ['ID', 'Contact Id', 'Contact Name', 'Code', 'Source', 'Source Id',
                   'Amount', 'Remaining Amount', 'Pending Clearance Amount', 'Allowance Amount',
                   'Due Date', 'Is Completed', 'Completion Date', 'Created At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 35, align: "center" },
    			  { name: 'cashbankid', index: 'cashbankid', width: 35, align: "center", hidden: true},
				  { name: 'name', index: 'name', width: 120 },
                  { name: 'code', index: 'code', width: 120 },
				  { name: 'source', index: 'source', width: 120, align: 'right' },
				  { name: 'sourceid', index: 'sourceid', width: 60 },
                  { name: 'amount', index: 'amount', width: 150, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'remainingamount', index: 'amount', width: 150, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'pendingclearanceamount', index: 'pendingclearanceamount', width: 160, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'allowanceamount', index: 'allowanceamount', width: 150, align: 'right', formatter: 'currency', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                  { name: 'duedate', index: 'duedate', search: false, width: 80, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
                  { name: 'iscompleted', index: 'iscompleted', width: 100 },
                  { name: 'completiondate', index: 'completiondate', search: false, width: 105, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
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
		              rowIsCompleted = "YES";
		          } else {
		              rowIsCompleted = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { iscompleted: rowIsCompleted });

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