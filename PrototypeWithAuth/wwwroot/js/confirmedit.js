
$(function () {
    $(".save-request-edits").on("click", function (e) {
        $.fn.CloseModal("confirm-edit");
        console.log("save request edits");
        var visualDiv = "";
        var formData = new FormData($("#myForm")[0]);
        $("#myForm").data("validator").settings.ignore = "";
        var valid = $("#myForm").valid();
        console.log("valid form: " + valid)
        if (!valid) {
            $("#myForm").data("validator").settings.ignore = ':not(select:hidden, .location-error:hidden,input:visible, textarea:visible)';
            $('.turn-edit-on-off').prop('checked', true);
            console.log("not valid data");
            return false;
        }
        var url = '';
        if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {
            console.log("has class suppliers or accounting");
            url = "/Vendors/Edit";
            var selectedTab = $('.nav-tabs .active').parent().index() + 1;
            formData.set("Tab", selectedTab)
        }
        else if ($('.turn-edit-on-off').hasClass('users')) {
            console.log("has class users");
            url = "/Admin/EditUser";
        }
        else if ($('.turn-edit-on-off').hasClass('orders')) {
            $("#loading").show();
            console.log("has class orders");
            url = "/Requests/EditModalView";
        }
        else if ($('.turn-edit-on-off').hasClass('locations')) {
            //console.log("has class locations");
            //if ($('.turn-edit-on-off').attr("section-type") == "LabManagement") {
            $("#loading").show();
            //	console.log("has class locations in labmanage");
            //	var visualContainerId = $(".hasVisual").attr("parent-id");
            //	url = "/Locations/VisualLocations/?VisualContainerId=" + visualContainerId;

            //	visualDiv = $(".VisualBoxColumn");
            //}
            //else {
            //console.log("has class locations in requests");
            url = "/Requests/ReceivedModalVisual";
            visualDiv = $(".visualView");
            //} 
        }
        else if ($('.turn-edit-on-off').hasClass('protocols')) {
            formData.set("ModalType", "Summary")
            var tab = $(".protocol-tab.active");
            var selectedTab = tab.parent().index() + 1;
            formData.set("Tab", selectedTab)
            console.log(selectedTab);
            $(".selectedTab").val(selectedTab);
            url = "/Protocols/CreateProtocol?IncludeSaveLines=true";
        }
        else {
            alert("didn't go into any edits");
        }
        console.log("url: " + url);
        //console.log(...formData)
        $.ajax({
            processData: false,
            contentType: false,
            data: formData,
            async: true,
            url: url,
            type: 'POST',
            cache: false,
            success: function (data) {
                $("#loading").hide();
                if ($('.turn-edit-on-off').hasClass('operations') || $('.turn-edit-on-off').hasClass('orders')) {
                    $(".editModal").html(data);
                    //$.fn.OnOpenModalView();
                    $.fn.LoadEditModalDetails();
                    $("[data-toggle='tooltip']").tooltip();
                }
                if ($('.turn-edit-on-off').hasClass('locations')) {
                    //alert("got data for locations");
                    //console.log(data)
                    var pageType = $('#masterPageType').val();
                    console.log(pageType)
                 
                    if (pageType == "LabManagementLocations" || pageType == 'RequestLocation') {
                        console.log('reload location ')
                        //Reload visual of locations box
                        var visualContainerId = $(".hasVisual").attr("parent-id");
                        var urlLocations = "/Locations/VisualLocations/?VisualContainerId=" + visualContainerId;
                        $.ajax({
                            async: true,
                            url: urlLocations,
                            type: 'GET',
                            cache: true,
                            success: function (d) {
                                $(".hasVisual").html(d);
                                $("#loading").hide();
                            }
                        });
                    }
                    else if ($('.turn-edit-on-off').attr("section-type") == "Requests") {

                        console.log("reloading ajax partial view...");
                        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "GET");

                    }
                    else {
                        //visualDiv.html(data);
                    }

                }
                else {
                    $.fn.getMenuItems();
                    //reload index pages
                    if ($('.turn-edit-on-off').hasClass('operations')) {
                        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "GET");
                    }
                    else if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {
                        $(".VendorData").html(data);
                        $.fn.LoadEditModalDetails();
                        $("[data-toggle='tooltip']").tooltip();
                        $.ajax({
                            async: true,
                            url: '/Vendors/_IndexForPayment?SectionType=' + $('#SectionType').val(),
                            type: 'GET',
                            cache: true,
                            success: function (data) {
                                $('.indexTable').html(data);
                            }
                        });
                    }
                    else if ($('.turn-edit-on-off').hasClass('users')) {
                        var url = "";
                        var errorMessage = data;
                        var pageType = $('#PageType').val();
                        if (pageType == "UsersWorkers") {
                            url = "/ApplicationUsers/_Details"
                        }
                        else {
                            url = "/Admin/_Index"
                        }
                        $.ajax({
                            async: true,
                            url: url,
                            type: 'GET',
                            cache: true,
                            success: function (data) {
                                if (data.includes("Invalid Url")) {
                                    alert("in invalid url");
                                    $.fn.OpenModal('invalid-right-modal', 'right-invalid', data);
                                }
                                else {
                                    $('#usersTable').html(data);
                                    $("#OriginalStatusID").attr("CentarixID", $("#CentarixID").val());
                                    if (errorMessage.length > 0) {
                                        $(".error-message").html(errorMessage);
                                    }
                                }
                            }
                        });

                    } else if ($('.turn-edit-on-off').hasClass('orders')) {
                        var viewClass = "_IndexTableData";
                        if ($('#masterSidebarType').val() === 'Cart') {
                            viewClass = "_IndexTableDataByVendor";
                        }
                        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/" + viewClass + "/", "." + viewClass, "GET");
                    }
                    else if ($('.turn-edit-on-off').hasClass('protocols')) {
                        $("._IndexTable").html(data)
                        $.fn.ProtocolsMarkReadonly("_IndexTable");
                        var modalType = $(".modalType").val();
                        $("." + modalType).removeClass("d-none")

                    }
                }
                $.fn.TurnToDetails("edits");
                //sets up error message if it has the setup in the view
                //if ($(".hasErrorMessage").length > 0) {
                //	alert("error message: " + $(".hasErrorMessage").val());
                //	$(".error-message").html($(".hasErrorMessage").val());
                //}
            },
            error: function (xhr) {
                if ($('.turn-edit-on-off').hasClass('operations') || $('.turn-edit-on-off').hasClass('orders')) {
                    console.log(xhr.responseText);
                    $(".editModal").html(xhr.responseText);
                    $.fn.LoadEditModalDetails();
                    $('.modal-content a:first').tab('show');
                    $("[data-toggle='tooltip']").tooltip();
                }
                if ($('.turn-edit-on-off').hasClass('locations')) {
                    $("#location").html(xhr.responseText);
                    $(".error-message").removeClass("d-none");
                }
                else if ($('.turn-edit-on-off').hasClass('protocols')) {
                    $.fn.OpenModal("modal", "edits", xhr.responseText)
                    $("._CreateProtocol").html(jqxhr.responseText);
                    $(".mdb-select").materialSelect();
                }
                else {
                    $.fn.OpenModal("modal", "edits", xhr.responseText)
                    $(".mdb-select").materialSelect();
                   // $.fn.OnOpenModal();
                }
            }
        });

    });


    $(".dont-save-request-edits").on("click", function (e) {
        $.fn.CloseModal("confirm-edit");
        console.log("don't save request edits");
        var selectedTab = $('.nav-tabs .active').parent().index() + 1;
        var url = '';
        var section = "";
        var reloadDiv = $('.partial-div');
        var currentPermissions = "";
        var id = $('.turn-edit-on-off').val();
        var controller = "/Requests/";
        var viewClass = "_ItemHeader";

        if ($('.turn-edit-on-off').hasClass('suppliers')) {
            section = "LabManagement";
            url = "/Vendors/VendorData?id=" + id + "&SectionType=" + section + "&Tab=" + selectedTab;
            viewClass = "_VendorHeader";
            controller = "/Vendors/";

        } else if ($('.turn-edit-on-off').hasClass('accounting')) {
            section = "Accounting";
            url = "/Vendors/VendorData?id=" + id + "&SectionType=" + section + "&Tab=" + selectedTab;
            viewClass = "_VendorHeader";
            controller = "/Vendors/";

        } else if ($('.turn-edit-on-off').hasClass('users')) {
            //alert("in users");
            url = "/Admin/EditUserPartial?id=" + id + "&Tab=" + selectedTab;
            currentPermissions = $(".permissions-checks:visible")[0]?.classList.toString().split(" ").join(".");

        } else if ($('.turn-edit-on-off').hasClass('orders')) {
            selectedTab = $('.tab-content').children('.active').attr("value");
            console.log(selectedTab)
            section = $("#masterSectionType").val();
            url = "/Requests/ItemData?id=" + id + "&Tab=" + selectedTab + "&SectionType=" + section;

        } else if ($('.turn-edit-on-off').hasClass('locations')) {
            /*selectedTab = $('.tab-content').children('.active').attr("value");
            console.log(selectedTab)*/
            section = $("#masterSectionType").val();
            url = "/Requests/_LocationTab?id=" + id;
            reloadDiv = $("#location");

        } else {
            alert("didn't go into any edits");
        }
        console.log("url: " + url);
        $.ajax({
            url: url,
            type: 'GET',
            cache: true,
            success: function (data) {
                console.log("cancel edit successful!")
                //open the confirm edit modal
                reloadDiv.html(data);

                //$('.name').val($('.old-name').val())

                if ($('.turn-edit-on-off').hasClass('users')) {
                    $('.userName').val($('#FirstName').val() + " " + $('#LastName').val())
                    $('.mark-readonly').prop('disabled', true) //for uplodad image button
                    console.log(currentPermissions)
                    $.fn.HideAllPermissionsDivs();
                    if (currentPermissions != null) {
                        $(".main-permissions").hide();
                        $("." + currentPermissions).show()
                    }
                    else {
                        $.fn.ChangeUserPermissionsButtons();
                    }
                    $('#permissions .form-check :input[type=hidden]').remove(); /*remove automatically generated input cuz it causes the checkboxes to be hidden*/
                } else {
                    $.ajax({
                        url: controller + viewClass + "?id=" + id + "&SectionType=" + section,
                        type: 'GET',
                        cache: true,
                        success: function (data) {
                            $('.' + viewClass).html(data);
                            if ($('.turn-edit-on-off').hasClass('orders') || $('.turn-edit-on-off').hasClass('locations')) {
                                $.fn.LoadEditModalDetails();
                                $("[data-toggle='tooltip']").tooltip();
                            }
                            else if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {
                                $.fn.LoadEditModalDetails();
                            }

                        }
                    })
                }

            }
        });
    });


    $(".cancel-request-edits").off("click").on("click", function (e) {
        $.fn.CloseModal("confirm-edit");
        console.log("cancel request edits");
        $('.mark-readonly').attr("disabled", false);
        $('.mark-edditable').data("val", true);
        $('.edit-mode-switch-description').text("Edit Mode On");
        $('.turn-edit-on-off').attr('name', 'edit')
        if ($('.turn-edit-on-off').hasClass('operations') || $('.turn-edit-on-off').hasClass('orders')) {
            console.log("orders operations")
            //$.fn.EnableMaterialSelect('#parentlist', 'select-options-parentlist')
            //$.fn.EnableMaterialSelect('#sublist', 'select-options-sublist')
            $.fn.EnableMaterialSelect('#vendorList', 'select-options-vendorList')
            $.fn.EnableMaterialSelect('#currency', 'select-options-currency')
        }
        if ($('.turn-edit-on-off').hasClass('orders')) {
            console.log("orders")
            $.fn.EnableMaterialSelect('#Request_SubProject_ProjectID', 'select-options-Request_SubProject_ProjectID');
            $.fn.EnableMaterialSelect('#SubProject', 'select-options-SubProject');
            $.fn.EnableMaterialSelect('#unitTypeID', 'select-options-unitTypeID');
            $.fn.CheckUnitsFilled();
            $.fn.CheckSubUnitsFilled();
        }
        if ($('.turn-edit-on-off').hasClass('suppliers') || $('.turn-edit-on-off').hasClass('accounting')) {
            $.fn.EnableMaterialSelect('#VendorCategoryTypes', 'select-options-VendorCategoryTypes');
            $.fn.EnableMaterialSelect('#VendorCountries', 'select-options-VendorCountries');
        }
        if ($(this).hasClass('users')) {
            $.fn.EnableMaterialSelect('#NewEmployee_JobSubcategoryType_JobCategoryTypeID', 'select-options-NewEmployee_JobSubcategoryType_JobCategoryTypeID');
            $.fn.EnableMaterialSelect('#NewEmployee_DegreeID', 'select-options-NewEmployee_DegreeID');
            $.fn.EnableMaterialSelect('#NewEmployee_MaritalStatusID', 'select-options-NewEmployee_MaritalStatusID');
            $.fn.EnableMaterialSelect('#NewEmployee_CitizenshipID', 'select-options-NewEmployee_CitizenshipID');
            $.fn.EnableMaterialSelect('#NewEmployee_JobSubcategoryTypeID', 'select-options-NewEmployee_JobSubcategoryTypeID');
        }
        $('.turn-edit-on-off').prop('checked', true);
        $('.open-document-modal').attr("data-val", true);
    });

    $(".exit-edit-modal").off("click").on("click", function (e) {
        var url = $(this).attr("value");
        var itemurl = "/Requests/ConfirmExit/";
        console.log(itemurl);
        var formData = {
            SectionType: $('#masterSectionType').val(),
            PageType: $('#masterPageType').val(),
            URL: url,
            GUID: $('#GUID').val()
        }
        console.log(formData);
        $.ajax({
            contentType: false,
            processData: true,
            async: true,
            url: $itemurl,
            type: 'POST',
            data: formData,
            cache: true,
            success: function (data) {
                $("#loading").hide();
                $(".save-item").removeClass("save-item").off('click');
                if (url != "") {
                    location.href = url;
                }

                $.fn.CloseModal("confirm-exit");
                $.fn.CloseModal("edits");
                //$('.confirm-exit-modal').remove();
                //$(".modal").modal('hide');
                //$(".modal").replaceWith('');

            }
        });
    })

    $(".return-edit-modal").off("click").on("click", function (e) {
        $.fn.CloseModal("confirm-exit");
        //$(".modal").attr("data-backdrop", true);
    })
});