﻿$(".more").off('click').click(function () {
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

});

$(".share-request").on("click", function (e) {
	alert("share request");
	$.ajax({
		async: true,
		url: "/Requests/ShareRequest/?id=" + val,
		traditional: true,
		type: "GET",
		cache: false,
		success: function (data) {
			$.fn.OpenModal("share-request", "share-request", data)
			$("#loading").hide();
		}
	})
});

$(".load-quote-details").on("click", function (e) {
	console.log("in order details");
	e.preventDefault();
	e.stopPropagation();
	$("#loading").show();
	var $itemurl = "/Requests/EditQuoteDetails/?id=" + $(".key-vendor-id").val() + "&requestID=" + $(this).attr("value");
	$.fn.CallPageRequest($itemurl, "quote");
	return false;
});


$(".load-order-details").off('click').on("click", function (e) {
	console.log("in order details");
	e.preventDefault();
	e.stopPropagation();
	$("#loading").show();
	var selectedPriceSort = [];
	$("#priceSortContent .priceSort:checked").each(function (e) {
		selectedPriceSort.push($(this).attr("enum"));
	})
	var section = $("#masterSectionType").val()
	//takes the item value and calls the Products controller with the ModalView view to render the modal inside
	var $itemurl = "/Requests/ReOrderFloatModalView/?id=" + $(this).attr("value") + "&" + $.fn.getRequestIndexString()
	$.fn.CallPageRequest($itemurl, "reorder");
	return false;
});

$(".load-product-details").off('click').on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	$("#loading").show();
	var $itemurl = "/Requests/EditModalView/?id=" + $(this).val() + "&SectionType=" + $("#masterSectionType").val();
	$.fn.CallPageRequest($itemurl, "details");
	return false;
});

//$("body, .modal").off('click', ".load-product-details-summary").on("click", ".load-product-details-summary", function (e) {

$(".load-product-details-summary").off('click').on("click", function (e) {

	e.preventDefault();
	e.stopPropagation();
	$("#loading").show();
	//takes the item value and calls the Products controller with the ModalView view to render the modal inside
	var $itemurl = "/Requests/EditModalView/?id=" + $(this).attr("value") + "&isEditable=false" + "&SectionType=" + $("#masterSectionType").val();
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
	ajaxPartialIndexTable($(".request-status-id").val(), "/Operations/Order/?id=" + $(this).attr("value"), "._IndexTableWithCounts", "GET");
	return false;
});

$(".approve-order").off('click').on("click", function (e) {
	console.log("approving");
	var val = $(this).attr("value");
	e.preventDefault();
	$("#loading").show();
	console.log(".order-type" + val)
	if ($(".order-type" + val).val() == "1") {
		console.log("terms")
		$.ajax({
			async: true,
			url: "/Requests/Approve/?id=" + val,
			traditional: true,
			type: "GET",
			cache: false,
			success: function (data) {
				$.fn.OpenModal("termsModal", "terms", data)
				$("#loading").hide();
			}
		})
	}
	else if ($(".order-type" + val).val() == "2") {
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
		ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/Approve/?id=" + val, "._IndexTableWithCounts", "GET");
	}
	return false;
});

$(".request-favorite").on("click", function (e) {
	var emptyHeartClass = "icon-favorite_border-24px";
	var fullHeartClass = "icon-favorite-24px";
	var fav = "request-favorite";
	var unfav = "request-unlike";
	var title = "Like";
	var requestFavorite = $(this);
	var FavType = "favorite";
	if (requestFavorite.hasClass("request-unlike")) {
		FavType = "unlike";
		$.ajax({
			async: true,
			url: "/Requests/RequestFavorite/?requestID=" + requestFavorite.attr("value") + "&Favtype=" + FavType,
			traditional: true,
			type: "GET",
			cache: false,
			success: function (data) {
				requestFavorite.children("i").addClass(emptyHeartClass);
				requestFavorite.children("i").removeClass(fullHeartClass);
				requestFavorite.attr("data-original-title", title);
				requestFavorite.removeClass(unfav);
				$("#loading").hide();
			}
		})
	}
	else {
		title = "Unlike";
		$.ajax({
			async: true,
			url: "/Requests/RequestFavorite/?requestID=" + requestFavorite.attr("value") + "&Favtype=" + FavType,
			traditional: true,
			type: "GET",
			cache: false,
			success: function (data) {
				requestFavorite.children("i").removeClass(emptyHeartClass);
				requestFavorite.children("i").addClass(fullHeartClass);
				requestFavorite.attr("data-original-title", title);
				requestFavorite.addClass(unfav);
				$("#loading").hide();
			}
		})
	}

});


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
	ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "GET");
	return false;
});

$("#Months, #Years").off("change").on("change", function (e) {
	var years = [];
	years = $("#Years").val();
	var months = [];
	months = $("#Months").val();
	ajaxPartialIndexTable($(".request-status-id").val(), "/Requests/_IndexTableData/", "._IndexTableData", "GET", undefined, "", months, years);
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

function ajaxPartialIndexTable(status, url, viewClass, type, formdata, modalClass = "", months, years) {
	console.log("in ajax partial index call" + url);
	var selectedPriceSort = [];
	$("#priceSortContent .priceSort:checked").each(function (e) {
		selectedPriceSort.push($(this).attr("enum"));
	})
	var contentType = true;
	var processType = true;
	if (formdata == undefined) {
		console.log("formdata is undefined");
		formdata = {
			PageNumber: $('.page-number').val(),
			RequestStatusID: status,
			PageType: $('#masterPageType').val(),
			SectionType: $('#masterSectionType').val(),
			SidebarType: $('#masterSidebarType').val(),
			SelectedPriceSort: selectedPriceSort,
			SelectedCurrency: $('#tempCurrency').val(),
			SidebarFilterID: $('.sideBarFilterID').val(),
			months: months,
			years: years
		};
		console.log(formdata);
	}
	else {
		$.fn.CloseModal(modalClass);
		contentType = false;
		processType = false;
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
			$("#loading").hide();
			return true;
		}
	});

	return false;
}

