﻿//const { ajax } = require("jquery");
$(function () {

    $(".popover-more").off('click').click(function () {
        var val = $(this).val();
        $('[data-toggle="popover"]').popover('dispose');
        $(this).popover({
            sanitize: false,
            placement: 'bottom',
            html: true,
            content: function () {
                return $('#' + val).html();
            }
        });
        $(this).popover('toggle');
        $(".popover .share-request-fx").click(function (e) {
            e.preventDefault();
            //switch this to universal share request and the modelsenum send in
            var url = "/" + $(this).attr("data-controller") + "/" + $(this).attr("data-action") + "/?ID=" + $(this).attr("data-route-request") + "&ModelsEnum=Request";

            $.ajax({
                async: true,
                url: url,
                traditional: true,
                type: "GET",
                cache: false,
                success: function (data) {
                    $.fn.OpenModal("shared-modal", "share-modal", data)
                    $.fn.EnableMaterialSelect('#ApplicationUserIDs', 'select-options-ApplicationUserIDs')
                    $("#loading").hide();
                    return false;
                }
            })
        });
        $(".icon-more-popover").off("click").on("click", ".remove-share", function (e) {
            var ControllersEnum = "";
            var shareNum = "";
            if ($(this).hasClass("requests")) { //THIS IF IS NOT WORKING
                ControllersEnum = "Requests";
            }
            shareNum = $(this).attr("data-share-resource-id");
            var url = "/" + ControllersEnum + "/RemoveShare?ID=" + shareNum + "&ModelsEnum=" + $("#masterSectionType").val();

            $.ajax({
                async: true,
                url: url,
                type: 'GET',
                cache: true,
                success: function (e) {
                    if (!e) {
                        $.ajax({
                            async: true,
                            url: "/Requests/_IndexSharedTable",
                            type: 'GET',
                            cache: true,
                            success: function (data) {
                                $("._IndexSharedTable").html(data);
                            }
                        })
                    }
                }
            });
        });
        $(".popover .load-confirm-delete").click( function (e) {
    console.log("in confirm delete");
    e.preventDefault();
    e.stopPropagation();
    $("#loading").show();
    var $itemurl = "/Requests/DeleteModal/?id=" + $(this).attr("value") + "&" + $.fn.getRequestIndexString();
    $.fn.CallPageRequest($itemurl, "delete");
    return false;
});
    });
    $('._IndexTableData [data-toggle = "tooltip"], ._IndexTableDataByVendor [data-toggle = "tooltip"]' ).off('click').on("click", function (e) {
        e.preventDefault();
        console.log('prevent default')
    });
    //$("body").off("click", ".share-request").on("click", ".share-request", function (e) {
    //	alert("share request");
    //	var url = "/" + $(this).attr("data-controller") + "/" + $(this).attr("data-action") + "/?requestId=" + $(this).attr("data-route-request");
    //	alert("share request: " + url);
    //	$.ajax({
    //		async: true,
    //		url: "/Requests/ShareRequest/?id=" + val,
    //		traditional: true,
    //		type: "GET",
    //		cache: false,
    //		success: function (data) {
    //			$.fn.OpenModal("share-request", "share-request", data)
    //			$("#loading").hide();
    //		}
    //	})
    //});

    //});

    //$(document).off("click", ".popover .share-request").on("click", ".popover .share-request", function () {
    //	alert('it works!');
    //});

    //$(".popover").on("click", function (e) {
    //	alert("popover clicked!");
    //});

    //$("body").off("click", ".share-request").on("click", ".share-request", function (e) {
    //	var url = "/" + $(this).attr("data-controller") + "/" + $(this).attr("data-action") + "/?requestId=" + $(this).attr("data-route-request");
    //	alert("share request: " + url);
    //	$.ajax({
    //		async: true,
    //		url: url,
    //		traditional: true,
    //		type: "GET",
    //		cache: false,
    //		success: function (data) {
    //			$.fn.OpenModal("share-request", "share-request", data)
    //			alert(data);
    //			$("#loading").hide();
    //			return false;
    //		}
    //	})
    //});

    $(".load-quote-details").on("click", function (e) {
        console.log("in order details");
        e.preventDefault();
        e.stopPropagation();
        $("#loading").show();
        var $itemurl = "/Requests/EditQuoteDetails/?id=" + $(".key-vendor-id").val() + "&requestIds=" + $(this).attr("value");
        $.fn.CallPageRequest($itemurl, "quote");
        return false;
    });


    $("body").off("click", ".load-order-details").on("click", ".load-order-details", function (e) {
        //alert('in function')
        e.preventDefault();
        e.stopImmediatePropagation();
        console.log("in order details");
        //e.stopPropagation();
        $("#loading").show();
        var section = $("#masterSectionType").val()
        //takes the item value and calls the Products controller with the ModalView view to render the modal inside
        var $itemurl = "/Requests/ReOrderFloatModalView/?id=" + $(this).attr("value") + "&" + $.fn.getRequestIndexString()
        $.fn.CallPageRequest($itemurl, "reorder");
        console.log("after call page request");
        $(".temprequesthiddenfors").first().html('');
        return false;
    });

    $(".load-product-details").off('click').on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(".tooltip").remove();
        $("#loading").show();
        var $itemurl = "/Requests/EditModalView/?id=" + $(this).attr('value') + "&SectionType=" + $("#masterSectionType").val();
        $.fn.CallPageRequest($itemurl, "details");
        return false;
    });

    //$("body, .modal").off('click', ".load-product-details-summary").on("click", ".load-product-details-summary", function (e) {

    $(".load-product-details-summary").off('click').on("click", function (e) {

        e.preventDefault();
        e.stopPropagation();
        $(".tooltip").remove();
        $("#loading").show();
        console.log('in load products details summary');
        //takes the item value and calls the Products controller with the ModalView view to render the modal inside
        var isProprietary = $(".request-status-id").attr("value") == 7 ? true : false;
        var $itemurl = "/Requests/EditModalView/?id=" + $(this).attr("value") + "&isEditable=false" + "&SectionType=" + $("#masterSectionType").val() + "&isProprietary=" + isProprietary;
        /*alert("$itemurl: " + $itemurl);*/
        $.fn.CallPageRequest($itemurl, "summary");
        return false;
    });

    $(".load-receive-and-location").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#loading").show();
        //takes the item value and calls the Products controller with the ModalView view to render the modal inside
        var $itemurl = "/Requests/ReceivedModal?RequestID=" + $(this).attr("value") + "&" + $.fn.getRequestIndexString()
        $.fn.CallPageRequest($itemurl, "received");
        return false;
    });

    $(".order-approved-operation").off('click').on("click", function (e) {
        console.log("approving");
        e.preventDefault();
        $("#loading").show();
        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Operations/Order/?id=" + $(this).attr("value"), "._IndexTableWithCounts", "GET");
        return false;
    });

    $(".approve-order").off('click').on("click", function (e) {
        console.log("approving");
        var val = $(this).attr("value");
        console.log("val: " + val);
        e.preventDefault();
        $("#loading").show();
        console.log("a tag: " + $(".order-type" + val))
        console.log("order type: " + $(".order-type" + val).attr("value"))
        if ($(".order-type" + val).attr("value") == "1") {
            console.log("confirm email")
            $.ajax({
                async: true,
                url: "/Requests/Approve/?id=" + val,
                traditional: true,
                type: "GET",
                cache: false,
                success: function (data) {
                    //alert("about to open modal");
                    $.fn.OpenModal("emailModal", "confirm-email", data)
                    $("#loading").hide();
                }
            })
        }
        else if ($(".order-type" + val).attr("value") == "2") {
            console.log("cart")
            $.ajax({
                async: true,
                url: "/Requests/_CartTotalModal/?requestID=" + val + "&sectionType=" + $('#masterSectionType').val(),
                traditional: true,
                type: "GET",
                cache: false,
                success: function (data) {
                    $.fn.OpenModal('cart-total-modal', 'cart-total', data)
                    $("#loading").hide();
                }
            })
        }
        else {
            alert("error- shouldn't get here!");
        }
        return false;
    });


    var requestFavoritesHasRun = false; //This is preventing the double click
    $(".request-favorite").off("click").on("click", function (e) {
        //$(this).off("click");
        //alert("in click fr");
        if (!requestFavoritesHasRun) {
            requestFavoritesHasRun = true;
            $("#loading").show();
            var requestFavorite = $(this);
            //alert(" in favorite request fx");
            var emptyHeartClass = "icon-favorite_border-24px";
            var fullHeartClass = "icon-favorite-24px";
            var unfav = "request-unlike";
            var title = "Favorite";
            var FavType = "favorite";
            var sidebarType = $('#masterSidebarType').val();
            if (requestFavorite.hasClass("request-unlike")) {
                FavType = "unlike";
                $.ajax({
                    async: true,
                    url: "/Requests/RequestFavorite/?requestID=" + requestFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
                    traditional: true,
                    type: "GET",
                    cache: false,
                    success: function (data) {
                        requestFavoritesHasRun = false;
                        requestFavorite.children("i").addClass(emptyHeartClass);
                        requestFavorite.children("i").removeClass(fullHeartClass);
                        requestFavorite.attr("data-original-title", title);
                        requestFavorite.removeClass(unfav);
                        $("#loading").hide();
                        if (sidebarType == 'Favorites') {
                            $('[data-toggle="tooltip"]').tooltip('dispose'); //is this the right syntax?
                            $('._IndexTable').html(data);
                        }
                    }
                })
            }
            else {
                title = "Unfavorite";
                $.ajax({
                    async: true,
                    url: "/Requests/RequestFavorite/?requestID=" + requestFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
                    traditional: true,
                    type: "GET",
                    cache: false,
                    success: function (data) {
                        requestFavoritesHasRun = false;
                        requestFavorite.children("i").removeClass(emptyHeartClass);
                        requestFavorite.children("i").addClass(fullHeartClass);
                        requestFavorite.attr("data-original-title", title);
                        requestFavorite.addClass(unfav);
                        $("#loading").hide();

                    }
                })
            }
            //$.fn.FavoriteRequests(requestFavorite);
        }
    });

    $.fn.FavoriteRequests = function (requestFavorite) {

    };

    $(".create-calibration").off('click').on("click", function (e) {
        e.preventDefault();

        $.ajax({
            async: true,
            url: "/Calibrations/CreateCalibration?requestid=" + $(this).attr("value"),
            type: "GET",
            cache: false,
            success: function (data) {
                $('.render-body').html(data)
                $('#myForm a:first').tab('show');
            }
        });
        return false;
    });

    $(".page-item a").off('click').on("click", function (e) {
        console.log("next page");
        e.preventDefault();
        $("#loading").show();
        var pageNumber = parseInt($(this).html());
        $('.page-number').val(pageNumber);
        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "POST");
        return false;
    });

    $("#Months, #Years").off("change").on("change", function (e) {
        /*var years = [];
        years = $("#Years").val();
        var months = [];
        months = $("#Months").val();*/
        $('.page-number').val(1);
        $.fn.ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTable/", "._IndexTable", "GET" /*, undefined, "", months, years*/);
        return false;
    });

    $(".load-terms-modal").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#loading").show();

        var $itemurl = "/Requests/TermsModal/?vendorID=" + $(this).val() + "&" + $.fn.getRequestIndexString();
        console.log("itemurl: " + $itemurl);
        $.fn.CallPageRequest($itemurl, "termsmodal");
        return false;
    });

    $.fn.LoadModalForSelectedItems = function (e, itemUrl, modalClass) {
        e.preventDefault();
        e.stopPropagation();
        var arrayOfSelected = $(".form-check.accounting-select .form-check-input:checked").map(function () {
            return $(this).attr("id")
        }).get()
        //alert('before loading');
        console.log("arrayOfSelected: " + arrayOfSelected);
        $("#loading").show();
        $.ajax({
            type: "GET",
            url: itemUrl,
            traditional: true,
            data: { 'requestIds': arrayOfSelected },
            cache: true,
            success: function (data) {
                //alert('success!');
                $.fn.OpenModal("modal", modalClass, data)
                $('#currency').materialSelect();
                $("#loading").hide();
            }
        });
    }
    $('.load-terms-for-selected').on('click', function (e) {
        var itemUrl = "/Requests/TermsModal/?" + $.fn.getRequestIndexString();
        $.fn.LoadModalForSelectedItems(e, itemUrl, "terms");
    })
    $('.update-quote-for-selected').on('click', function (e) {
        $.fn.LoadModalForSelectedItems(e, "/Requests/EditQuoteDetails/", "edit-quote");
    })


    $.fn.ajaxPartialIndexTable = function (status, url, viewClass, type, formdata, modalClass = "", /*months, years, */) {
        console.log("in ajax partial index call " + url);
        //alert('before bind filter')
        if ($("#inventoryFilterContent").length) {
            var selectedFilters = $.fn.BindSelectedFilters("");
            console.log('in if')
        }
        var monthsString = "";
        var yearsString = "";
        var months = $("#Months").val();
        var years = $("#Years").val();
        if (months != undefined) {
            months.forEach(month => monthsString += "&months=" + month)
        }
        if (years != undefined) {
            years.forEach(year => yearsString += "&years=" + year)
        }
/*        var selectedPriceSort = [];
        $("#priceSortContent1 .priceSort:checked").each(function (e) {
            selectedPriceSort.push($(this).attr("enum"));
        })
        var selectedPriceSortObj = { "SelectedPriceSort": selectedPriceSort }
        console.log(selectedPriceSortObj)*/
		if (formdata == undefined) {
			console.log("formdata is undefined");
			/*formdata = {
				PageNumber: $('.page-number').val(),
				RequestStatusID: status,
				PageType: $('#masterPageType').val(),
				SectionType: $('#masterSectionType').val(),
				SidebarType: $('#masterSidebarType').val(),
				//SelectedPriceSort: selectedPriceSort,
				SelectedCurrency: $('#tempCurrency').val(),
				SidebarFilterID: $('.sideBarFilterID').val(),
				CategorySelected: $('#categorySortContent .select-category').is(":checked"),
				SubCategorySelected: $('#categorySortContent .select-subcategory').is(":checked"),
				months: months,
				years: years,
				IsArchive: isArchive
			};
			console.log(formdata);*/
            if (!url.includes("?")) {
                url += "?"
            } else {
                url += "&";
            }
            url += $.fn.getRequestIndexString(status);
            url += monthsString;
            url += yearsString;
            //formdata = {}; //so won't crash when do object.assign()
            //console.log(formdata)
		}
		else {
			$.fn.CloseModal(modalClass);
        }
        //var objectsToAdd = [];
        //if (selectedFilters != undefined/*should also somehow check if anything is chosen...*/) { objectsToAdd.push(selectedFilters) }
        //if (selectedPriceSortObj != undefined) { objectsToAdd.push(selectedPriceSortObj) }
        if (selectedFilters != undefined/*should also somehow check if anything is chosen...*/) {
            formdata = $.fn.AddObjectToFormdata(formdata, selectedFilters);
            console.log(...formdata);
        }

        var contentType = true;
        var processType = true;
        if (formdata != undefined) {
            type = "POST"
            processType = false;
            contentType = false; 
        }
        
        $.ajax({
            contentType: contentType,
            processData: processType,
            async: true,
            url: url,
            data: formdata,
            traditional: true,
            type: type,
            cache: false,
            success: function (data) {
                $(viewClass).html(data);
                $(".tooltip").remove();
                $("#loading").hide();
                //workaround for price radio button not coming in when switching from nothing is here tab
                var id = "#nis";
                //alert($(id).prop('checked'))
                if ($('#tempCurrency').val() === "USD") {
                    id = "#usd";
                }
                if ($(id).attr("checked") !== "checked") {
                    console.log('checking button')
                    $(id).attr("checked", "checked");
                }
                return true;
            },
            error: function (jqxhr) {
                $('.error-message').html(jqxhr.responseText);
            }
        });

        return false;
    }
    $.fn.AddObjectToFormdata = function (formdata, object) {

            console.log('in add object to formdata')
            /*console.log(arrayOfObjects)
            var newFormData = formdata;
            for (var obj of arrayOfObjects) {
                Object.assign(newFormData, obj);
                console.log('obj ' + obj)
            } */
        if (formdata == undefined) {
            var formdata = new FormData();
        }
        for (var key in object) {

            if (Array.isArray(object[key])) {
                for (const val of object[key].values()) {
                    formdata.append(key, val);
                    console.log('key' + key);
                    console.log('val' + val)
                }
            }
            else {
                formdata.append(key, object[key]);
                console.log('key ' + key);
                console.log('value ' + object[key])
            }
        }
        console.log(...formdata);
    
        return formdata;
    }

});
