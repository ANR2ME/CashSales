$(document).ready(function () {
    // Initialize
    var usercode = "";
    var group = "master";

    /*================================================ JQuery Tabs ================================================*/
    $('ul.tabs').each(function () {
        // For each set of tabs, we want to keep track of
        // which tab is active and it's associated content
        var $active, $content, $links = $(this).find('a');

        // If the location.hash matches one of the links, use that as the active tab.
        // If no match is found, use the first link as the initial active tab.
        $active = $($links.filter('[href="' + location.hash + '"]')[0] || $links[0]);
        $active.addClass('active');

        $content = $($active[0].hash);

        // Hide the remaining content
        $links.not($active).each(function () {
            $(this.hash).hide();
        });

        // Bind the click event handler
        $(this).on('click', 'a', function (e) {
            // Make the old tab inactive.
            $active.removeClass('active');
            $content.hide();

            // Update the variables with the new link and content
            $active = $(this);
            $content = $(this.hash);

            // Make the tab active.
            $active.addClass('active');
            $content.show();

            // Prevent the anchor's default click action
            e.preventDefault();
        });
    });
    /*================================================ END JQuery Tabs ================================================*/

    // Reload
    $('#ua_form_btn_reload').click(function () {
        $("#tbl_users").trigger("reloadGrid");

        // Clear and Reload all grid
        $("#tbl_access_master").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#tbl_access_report").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#tbl_access_file").jqGrid("clearGridData", true).trigger("reloadGrid");
        $("#tbl_access_proses").jqGrid("clearGridData", true).trigger("reloadGrid");

    });
    
    function GetUserAccess() {
        $.ajax({
            dataType: "json",
            url: base_url + "UserAccess/GetUserAccess?UserId=" + usercode + "&groupName=" + group,
            success: function (result) {
                PopulateUserAccess(group, result);
            }
        });
    }

    function PopulateUserAccess(group, objUserAccess) {
        if (objUserAccess != null) {
            for (var i = 0; i < objUserAccess.listUserAccess.length; i++) {
                var newData = {};
                newData.code = objUserAccess.listUserAccess[i].MenuId;
                newData.name = objUserAccess.listUserAccess[i].Name;
                newData.read = objUserAccess.listUserAccess[i].View;
                newData.write = objUserAccess.listUserAccess[i].Create;
                newData.edit = objUserAccess.listUserAccess[i].Edit;
                newData.delete = objUserAccess.listUserAccess[i].Delete;
                newData.undelete = objUserAccess.listUserAccess[i].UnDelete;
                newData.print = objUserAccess.listUserAccess[i].Print;

                // New Record
                var group_menu = objUserAccess.listUserAccess[i].GroupName.toLowerCase();
                jQuery("#tbl_access_" + group_menu).jqGrid('addRowData', $("#tbl_access_" + group_menu).getGridParam("reccount") + 1, newData);
            }
        }
    }

    // ---- View
    function UpdateAllowView(menuId, isAllow) {
        var userId = 0;
        var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        if (id) {
            userId = id;
        } else {
            $.messager.alert('Information', 'Please Select User...!!', 'info');
            return;
        };

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/AllowView",
            data: JSON.stringify({
                UserId:userId, MenuId: menuId, IsAllow: isAllow
            }),
            success: function (result) {
                if (!result.isValid) {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    }

    $("input[name=cbAllowView]").live("click", function () {
        UpdateAllowView($(this).attr('rel'), $(this).is(":checked"));
    });

    function cboxAllowView(cellvalue, options, rowObject) {
        return '<input name="cbAllowView" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END View

    // ---- Edit
    function UpdateAllowEdit(menuId, isAllow) {
        var userId = 0;
        var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        if (id) {
            userId = id;
        } else {
            $.messager.alert('Information', 'Please Select User...!!', 'info');
            return;
        };

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/AllowEdit",
            data: JSON.stringify({
                UserId: userId, MenuId: menuId, IsAllow: isAllow
            }),
            success: function (result) {
                if (!result.isValid) {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    }

    $("input[name=cbAllowEdit]").live("click", function () {
        UpdateAllowEdit($(this).attr('rel'), $(this).is(":checked"));
    });

    function cboxAllowEdit(cellvalue, options, rowObject) {
        return '<input name="cbAllowEdit" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Edit

    // ---- Delete
    function UpdateAllowDelete(menuId, isAllow) {
        var userId = 0;
        var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        if (id) {
            userId = id;
        } else {
            $.messager.alert('Information', 'Please Select User...!!', 'info');
            return;
        };

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/AllowDelete",
            data: JSON.stringify({
                UserId: userId, MenuId: menuId, IsAllow: isAllow
            }),
            success: function (result) {
                if (!result.isValid) {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    }

    $("input[name=cbAllowDelete]").live("click", function () {
        UpdateAllowDelete($(this).attr('rel'), $(this).is(":checked"));
    });

    function cboxAllowDelete(cellvalue, options, rowObject) {
        return '<input name="cbAllowDelete" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Delete

    // ---- UnDelete
    function UpdateAllowUnDelete(menuId, isAllow) {
        var userId = 0;
        var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        if (id) {
            userId = id;
        } else {
            $.messager.alert('Information', 'Please Select User...!!', 'info');
            return;
        };

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/AllowUnDelete",
            data: JSON.stringify({
                UserId: userId, MenuId: menuId, IsAllow: isAllow
            }),
            success: function (result) {
                if (!result.isValid) {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    }

    $("input[name=cbAllowUnDelete]").live("click", function () {
        UpdateAllowUnDelete($(this).attr('rel'), $(this).is(":checked"));
    });

    function cboxAllowUnDelete(cellvalue, options, rowObject) {
        return '<input name="cbAllowUnDelete" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END UnDelete

    // ---- Print
    function UpdateAllowPrint(menuId, isAllow) {
        var userId = 0;
        var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        if (id) {
            userId = id;
        } else {
            $.messager.alert('Information', 'Please Select User...!!', 'info');
            return;
        };

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/AllowPrint",
            data: JSON.stringify({
                UserId: userId, MenuId: menuId, IsAllow: isAllow
            }),
            success: function (result) {
                if (!result.isValid) {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    }

    $("input[name=cbAllowPrint]").live("click", function () {
        UpdateAllowPrint($(this).attr('rel'), $(this).is(":checked"));
    });

    function cboxAllowPrint(cellvalue, options, rowObject) {
        return '<input name="cbAllowPrint" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Print

    // ---- Create
    function UpdateAllowCreate(menuId, isAllow) {
        var userId = 0;
        var id = jQuery("#tbl_users").jqGrid('getGridParam', 'selrow');
        if (id) {
            userId = id;
        } else {
            $.messager.alert('Information', 'Please Select User...!!', 'info');
            return;
        };

        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/AllowCreate",
            data: JSON.stringify({
                UserId: userId, MenuId: menuId, IsAllow: isAllow
            }),
            success: function (result) {
                if (!result.isValid) {
                    $.messager.alert('Warning', result.message, 'warning');
                }
            }
        });
    }

    $("input[name=cbAllowCreate]").live("click", function () {
        UpdateAllowCreate($(this).attr('rel'), $(this).is(":checked"));
    });

    function cboxAllowCreate(cellvalue, options, rowObject) {
        return '<input name="cbAllowCreate" rel="' + rowObject.code + '" type="checkbox"' + (cellvalue ? ' checked="checked"' : '') +
            '/>';
    }

    // ---- END Create

    // Table User Access Group - Master
    jQuery("#tbl_access_master").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Un-Delete', 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center' },
                  { name: 'name', index: 'name', width: 160 },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowCreate, formatoptions: { disabled: false }
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowEdit, formatoptions: { disabled: false }
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'undelete', index: 'undelete', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowPrint, formatoptions: { disabled: false }
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_master").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    // END Table User Access Group - Master

    // Table User Access Group - File
    jQuery("#tbl_access_file").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Un-Delete', 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center' },
                  { name: 'name', index: 'name', width: 160 },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowCreate, formatoptions: { disabled: false }
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowEdit, formatoptions: { disabled: false }
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'undelete', index: 'undelete', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowUnDelete, formatoptions: { disabled: false }
                  },
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowPrint, formatoptions: { disabled: false }
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_file").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    // END Table User Access Group - File

    // Table User Access Group - Report
    jQuery("#tbl_access_report").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Un-Delete', 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center' },
                  { name: 'name', index: 'name', width: 160 },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'undelete', index: 'undelete', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_report").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    // END Table User Access Group - Report

    // Table User Access Group - Setting
    jQuery("#tbl_access_proses").jqGrid({
        url: base_url + 'index.html',
        datatype: "json",
        mtype: 'GET',
        colNames: ['Code', 'Name', 'View', 'Create', 'Edit', 'Delete', 'Un-Delete', 'Print'],
        colModel: [{ name: 'code', index: 'code', width: 50, align: 'center' },
                  { name: 'name', index: 'name', width: 160 },
                  {
                      name: 'read', index: 'read', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: cboxAllowView, formatoptions: { disabled: false }
                  },
                  {
                      name: 'write', index: 'write', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'edit', index: 'edit', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'delete', index: 'delete', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'undelete', index: 'undelete', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  },
                  {
                      name: 'print', index: 'print', width: 50, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: false },
                      hidden: true
                  }
        ],
        sortname: 'kode',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 580,
        height: 380
    });
    $("#tbl_access_proses").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false });
    // END Table User Access Group - Setting

    // Table User List
    jQuery("#tbl_users").jqGrid({
        url: base_url + 'User/GetList',
        datatype: "json",
        colNames: ['Code', 'Name', 'Department', 'Title', 'App. GM'],
        colModel: [{ name: 'usercode', index: 'usercode', width: 80 },
                  { name: 'username', index: 'username', width: 200 },
                  { name: 'department', index: 'department', width: 80 },
                  { name: 'title', index: 'title', width: 100 },
                  {
                      name: 'app_acc', index: 'app_acc', width: 60, align: 'center', sortable: false,
                      editable: true,
                      edittype: 'checkbox', editoptions: { value: "1:0" },
                      formatter: "checkbox", formatoptions: { disabled: true }
                  }
        ],
        onSelectRow: function (id) {

            //var url = window.location.href.split('#');
            //if (url[1] != undefined && url[1] != '')
            //    group = url[1].substr(5, url[1].length - 5);


            // Clear and Reload all grid
            $("#tbl_access_master").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_access_report").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_access_file").jqGrid("clearGridData", true).trigger("reloadGrid");
            $("#tbl_access_proses").jqGrid("clearGridData", true).trigger("reloadGrid");

            usercode = id;
            GetUserAccess();
        },
        sortname: 'id',
        viewrecords: true,
        gridview: true,
        shrinkToFit: false,
        scroll: 1,
        sortorder: "asc",
        width: 450,
        height: 420
    });
    $("#tbl_users").jqGrid('navGrid', '#toolbar_lookup_table_so_container', { del: false, add: false, edit: false, search: false })
           .jqGrid('filterToolbar', { stringResult: true, searchOnEnter: false });
    // Table User List

    // Save
    $('#ua_form_btn_save').click(function () {

        // Menu Type List
        var MenuTypeList = PopulateMenuTypeList();
        // END Menu Type List

        if (MenuTypeList == undefined || MenuTypeList == null || MenuTypeList.length == 0) {
            $.messager.alert('Information', 'Data is empty..', 'info');
            return;
        }


        $.ajax({
            contentType: "application/json",
            type: 'POST',
            url: base_url + "UserAccess/SaveUserAccessType",
            data: JSON.stringify({
                UserCode: usercode, MenuTypeList: MenuTypeList
            }),
            success: function (result) {
                if (result.isValid) {
                    $.messager.alert('Information', result.message, 'info', function () {
                        window.location = base_url + "UserAccess";
                    });
                }
                else {
                    $.messager.alert('Warning', result.message, 'warning');
                }


            }
        });
    });

    // Menu Type List
    function PopulateMenuTypeList() {
        var menuList = [];
        // Master
        var ids = $("#tbl_access_master").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_master").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        // File
        var ids = $("#tbl_access_file").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_file").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        // Report
        var ids = $("#tbl_access_report").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_report").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        // Proses
        var ids = $("#tbl_access_proses").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var datagrid = jQuery("#tbl_access_proses").jqGrid('getRowData', ids[i]);
            var obj = {};
            obj['Input'] = datagrid.write;
            obj['Lihat'] = datagrid.read;
            obj['Edit'] = datagrid.edit;
            obj['Hapus'] = datagrid.delete;
            obj['Un_Hapus'] = datagrid.undelete;
            obj['Print'] = datagrid.print;
            obj['Kode'] = datagrid.code;
            menuList.push(obj);
        }

        return menuList
    }
    // END Menu Type List
});