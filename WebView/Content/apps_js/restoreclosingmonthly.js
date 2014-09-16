$(document).ready(function () {

    $('#lookup_period_div').dialog('close');

    //---------------------------------------------LOOK UP COA------------------------------------------------------------------------------------

    $('#period_lookup_add').click(function () {
        var id = jQuery("#lookup_period").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#lookup_period").jqGrid('getRowData', id);

            $('#txtmonthperiod').val(ret.period);
            $('#txtyearperiod').val(ret.year);
            $('#txtbeginningperiod').val(ret.beginning);
            $('#txtendingperiod').val(ret.enddate);

            $('#lookup_period_div').dialog('close');
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        };
    });

    $('#period_lookup_cancel').click(function () {
        $('#lookup_period_div').dialog('close');
    });

    $('#btnperiod').click(function () {

        // Clear Search string jqGrid
        $('input[id*="gs_"]').val("");

        var lookUpURL = base_url + 'ClosingMonthly/GetListRestoreClosingByGL';
        if ($('#rbarap').is(':checked')) {
            lookUpURL = base_url + 'ClosingMonthly/GetListRestoreClosingByARAP';
        }

        var lookupGrid = $('#lookup_period');
        lookupGrid.setGridParam({
            url: lookUpURL
        }).trigger("reloadGrid");

        $('#lookup_period_div').dialog('open');
    });



    $("#lookup_period").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        colNames: ['Period', 'Year', 'Beginning', 'End Date', 'Status', 'Closing'],
        colModel: [
				  { name: 'period', index: 'period', width: 60 },
				  { name: 'year', index: 'yearperiod', width: 60 },
                  { name: 'beginning', index: 'beginningperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'enddate', index: 'enddateperiod', width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } },
                  { name: 'status', index: 'status', width: 80 },
                  { name: 'closing', index: 'closing', width: 100 , align: "center", formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "M d, Y" } }
        ],
        page: '1',
        pager: $('#pager_lookup_period'),
        rowNum: 50,
        rowList: [50, 100, 150],
        sortname: 'beginningperiod',
        viewrecords: true,
        sortorder: "ASC",
        width: $("#lookup_period_div").width() - 10,
        height: $("#lookup_period_div").height() - 100,
        gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowStatus = $(this).getRowData(cl).status;
		          if (rowStatus == 'false') {
		              status = "UnClose";
		          } else {
		              status = "Close";
		          }
		          $(this).jqGrid('setRowData', ids[i], { status: status });

		      }
		  }
    });//END GRID
    $("#lookup_period").jqGrid('navGrid', '#form_toolbar', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });


    // Proses
    $("#posting_form_btn_save").click(function () {
        $.messager.confirm('Confirm', 'Are you sure you want to process Restore Closing Monthly?', function (r) {
            if (r) {

                var isGL = false;
                if ($('#rbgeneralledger').is(':checked')) {
                    isGL = true;
                }
                var isARAP = false;
                if ($('#rbarap').is(':checked')) {
                    isARAP = true;
                }

                $.ajax({
                    contentType: "application/json",
                    type: 'POST',
                    url: base_url + "ClosingMonthly/RestoreProcess",
                    data: JSON.stringify({
                        Period: $('#txtmonthperiod').val(), YearPeriod: $('#txtyearperiod').val(), IsGL: isGL, IsARAP: isARAP
                    }),
                    success: function (result) {
                        if (result.isValid) {
                            $.messager.alert('Information', result.message, 'info');
                        }
                        else {
                            $.messager.alert('Warning', result.message, 'warning');
                        }
                    }
                });
            }
        });
    });

    //---------------------------------------------END LOOK UP COA--------------------------------------------------------------------------------

}); //END DOCUMENT READY