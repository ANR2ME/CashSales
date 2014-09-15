$(document).ready(function () {
    var a;
    $("#detail_panel").hide();

    $("#delete_confirm_div").dialog('close');
    $("#form_div").dialog('close');

    function ClearErrorMessage() {
        $('span[class=errormessage]').text('').remove();
    }

    function ReloadGrid() {
        $("#list").setGridParam({ url: base_url + 'MstItemType/GetList', postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ReloadGridDetail() {
        $("#list_detail").jqGrid('GridUnload');
        ReloadGridViewItem(jQuery("#list").jqGrid('getGridParam', 'selrow'));
       // $("#listdetail").setGridParam({ url: base_url + 'QuantityPricing/GetList?Id=' + jQuery("#list").jqGrid('getGridParam', 'selrow'), postData: { filters: null }, page: 'first' }).trigger("reloadGrid");
    }

    function ClearData() {
        $('#id').val('');
        $('#Discount').numberbox('setValue', '');
        $('#MinQuantity').numberbox('setValue', '');
        $('#IsInfiniteMaxQuantity').select('0');
        $('#MaxQuantity').numberbox('setValue', '');
        //onInfiniteMaxQuantity();
        $('#form_btn_save').data('kode', '');

        ClearErrorMessage();
    }

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

    function onInfiniteMaxQuantity() {
        if (document.getElementById('IsInfiniteMaxQuantity').value == 'True') {
            $('#MaxQuantity').attr('disabled', true);
        } else {
            $('#MaxQuantity').removeAttr('disabled');
        }
    };

    document.getElementById('IsInfiniteMaxQuantity').onchange = function () {
        onInfiniteMaxQuantity();
    };

    function ReloadGridViewItem(id) {
     
        $("#list_detail").jqGrid({
            url: base_url + 'QuantityPricing/GetList?Id=' + id,
            datatype: "json",
            mtype: 'GET',
            colNames: ['ID', 'Discount', 'Min Quantity', 'Is Infinite Max QTY', 'Max Quantity',
                       'Created At', 'Updated At',
            ],
            colModel: [
                      { name: 'id', index: 'id', width: 80, align: "center", frozen: true },
                      { name: 'discount', index: 'discount', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'minquantity', index: 'minquantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'isinfinitemaxquantity', index: 'isinfinitemaxquantity', width: 130 },
                      { name: 'maxquantity', index: 'maxquantity', width: 100, formatter: 'integer', formatoptions: { thousandsSeparator: ",", defaultValue: '0' } },
                      { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				      { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },

            ],
            page: '1',
            pager: $('#pager_detail'),
            rowNum: 20,
            rowList: [20, 30, 60],
            sortname: 'id',
            viewrecords: true,
            scrollrows: true,
            shrinkToFit: false,
            sortorder: "DESC",
            width: $("#detail_toolbar").width(),
            height: $(window).height() - 200,
            gridComplete:
		  function () {
		      var ids = $(this).jqGrid('getDataIDs');
		      for (var i = 0; i < ids.length; i++) {
		          var cl = ids[i];
		          rowIsInfiniteMaxQuantity = $(this).getRowData(cl).isinfinitemaxquantity;
		          if (rowIsInfiniteMaxQuantity == 'true') {
		              rowIsInfiniteMaxQuantity = "YES";
		          } else {
		              rowIsInfiniteMaxQuantity = "NO";
		          }
		          $(this).jqGrid('setRowData', ids[i], { isinfinitemaxquantity: rowIsInfiniteMaxQuantity });
		          
		      }
		  }

        });//END GRID Detail
        $("#list_detail").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
        jQuery("#list_detail").jqGrid('setFrozenColumns');
    }

    //GRID +++++++++++++++
    $("#list").jqGrid({
        url: base_url + 'MstItemType/GetList',
        datatype: "json",
        colNames: ['ID', 'Name', 'Description', 'Created At', 'Updated At'],
        colModel: [
    			  { name: 'id', index: 'id', width: 80, align: "center" },
				  { name: 'name', index: 'name', width: 80 },
                  { name: 'description', index: 'description', width: 250 },
				  { name: 'createdat', index: 'createdat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
				  { name: 'updateat', index: 'updateat', search: false, width: 100, align: "center", formatter: 'date', formatoptions: { srcformat: 'Y-m-d', newformat: 'm/d/Y' } },
        ],
        page: '1',
        pager: jQuery('#pager'),
        rowNum: 20,
        rowList: [20, 30, 60],
        sortname: 'id',
        viewrecords: true,
        scrollrows: true,
        shrinkToFit: false,
        sortorder: "DESC",
        width: $("#toolbar").width(),
        height: $(window).height() - 200,
        onSelectRow: function (id) {
            var id = jQuery("#list").jqGrid('getGridParam', 'selrow');
            if (id) {
                $("#list_detail").jqGrid('GridUnload');
                $("#detail_panel").show();
                ReloadGridViewItem(id);
            } else {
                $.messager.alert('Information', 'Data Not Found...!!', 'info');
            };
        },
        gridComplete:
          function () {
          }

    });//END GRID
    $("#list").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });
    $("#list").jqGrid('navGrid', '#pagerdetail1', { del: false, add: false, edit: false, search: true });
              //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true });

    //TOOL BAR BUTTON
    $('#btn_reload').click(function () {
        ReloadGridDetail();
    });

    $('#detail_btn_reload').click(function () {
        ReloadGridDetail();
    });

    $('#btn_print').click(function () {
        window.open(base_url + 'Print_Forms/Printmstbank.aspx');
    });

    //GRID Detail+++++++++++++++
   
    //.jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });

    $('#btn_add_new').click(function () {
        ClearData();
        clearForm('#frm');
        onInfiniteMaxQuantity();
        $('#form_div').dialog('open');
    });

    $('#btn_edit').click(function () {
        ClearData();
        clearForm("#frm");
        var id = jQuery("#list_detail").jqGrid('getGridParam', 'selrow');
        if (id) {
            vStatusSaving = 1;//edit data mode
            $.ajax({
                dataType: "json",
                url: base_url + "QuantityPricing/GetInfo?Id=" + id,
                success: function (result) {
                    if (result.Id == null) {
                        $.messager.alert('Information', 'Data Not Found...!!', 'info');
                    }
                    else {
                        if (JSON.stringify(result.Errors) != '{}') {
                            var error = '';
                            for (var key in result.Errors) {
                                error = error + "<br>" + key + " " + result.Errors[key];
                            }
                            $.messager.alert('Warning', error, 'warning');
                        }
                        else {
                            $("#form_btn_save").data('kode', result.Id);
                            $('#id').val(result.Id);
                            $('#Discount').numberbox('setValue', result.Discount);
                            $('#MinQuantity').numberbox('setValue', result.MinQuantity);
                            var e = document.getElementById("IsInfiniteMaxQuantity");
                            if (result.IsInfiniteMaxQuantity == true) {
                                e.selectedIndex = 1;
                            }
                            else {
                                e.selectedIndex = 0;
                            }
                            $('#MaxQuantity').numberbox('setValue', result.MaxQuantity);
                            onInfiniteMaxQuantity();
                            $('#form_div').dialog('open');
                        }
                    }
                }
            });
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });


    $('#btn_delete').click(function () {
        clearForm("#frm");

        var id = jQuery("#list_detail").jqGrid('getGridParam', 'selrow');
        if (id) {
            var ret = jQuery("#list_detail").jqGrid('getRowData', id);
            //if (ret.deletedimg != '') {
            //    $.messager.alert('Warning', 'RECORD HAS BEEN DELETED !', 'warning');
            //    return;
            //}
            $('#delete_confirm_btn_submit').data('Id', ret.id);
            $("#delete_confirm_div").dialog("open");
        } else {
            $.messager.alert('Information', 'Please Select Data...!!', 'info');
        }
    });

    $('#delete_confirm_btn_cancel').click(function () {
        $('#delete_confirm_btn_submit').val('');
        $("#delete_confirm_div").dialog('close');
    });

    $('#delete_confirm_btn_submit').click(function () {

        $.ajax({
            url: base_url + "QuantityPricing/Delete",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify({
                Id: $('#delete_confirm_btn_submit').data('Id'),
            }),
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                    $("#delete_confirm_div").dialog('close');
                }
                else {
                    ReloadGridDetail();
                    $("#delete_confirm_div").dialog('close');
                }
            }
        });
    });

    $('#form_btn_cancel').click(function () {
        clearForm('#frm');
        $("#form_div").dialog('close');
    });

    $("#form_btn_save").click(function () {

        ClearErrorMessage();

        var submitURL = '';
        var id = $("#form_btn_save").data('kode');

        // Update
        if (id != undefined && id != '' && !isNaN(id) && id > 0) {
            submitURL = base_url + 'QuantityPricing/Update';
        }
            // Insert
        else {
            submitURL = base_url + 'QuantityPricing/Insert';
        }

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: submitURL,
            data: JSON.stringify({
                Id: id, ItemTypeId: jQuery("#list").jqGrid('getGridParam', 'selrow'), Discount: $("#Discount").numberbox('getValue'), MinQuantity: $("#MinQuantity").numberbox('getValue'),
                MaxQuantity: $("#MaxQuantity").numberbox('getValue'), IsInfiniteMaxQuantity: document.getElementById("IsInfiniteMaxQuantity").value,
            }),
            async: false,
            cache: false,
            timeout: 30000,
            error: function () {
                return false;
            },
            success: function (result) {
                if (JSON.stringify(result.Errors) != '{}') {
                    for (var key in result.Errors) {
                        if (key != null && key != undefined && key != 'Generic') {
                            $('input[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                            $('textarea[name=' + key + ']').addClass('errormessage').after('<span class="errormessage">**' + result.Errors[key] + '</span>');
                        }
                        else {
                            $.messager.alert('Warning', result.Errors[key], 'warning');
                        }
                    }
                }
                else {
                    ReloadGridDetail();
                    $("#form_div").dialog('close')
                }
            }
        });
    });



}); //END DOCUMENT READY