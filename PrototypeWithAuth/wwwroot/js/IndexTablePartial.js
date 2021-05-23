$(".more").off('click').click(function () {
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
	$(".popover").off("click").on("click", ".share-request-fx", function () {
		var url = "/" + $(this).attr("data-controller") + "/" + $(this).attr("data-action") + "/?requestId=" + $(this).attr("data-route-request");
		//alert("url: " + url);
		$.ajax({
			async: true,
			url: url,
			traditional: true,
			type: "GET",
			cache: false,
			success: function (data) {
				$.fn.OpenModal("share-request-modal", "share-request", data)
				$.fn.EnableMaterialSelect('#userlist', 'select-options-userlist')
				$("#loading").hide();
				return false;
			}
		})
	});

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
	var $itemurl = "/Requests/EditQuoteDetails/?id=" + $(".key-vendor-id").val() + "&requestID=" + $(this).attr("value");
	$.fn.CallPageRequest($itemurl, "quote");
	return false;
});


$("body").on("click", ".load-order-details", function (e) {
	console.log("in order details");
	e.preventDefault();
	e.stopPropagation();
	$("#loading").show();
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

$(".load-product-details-summary").off('click').on("click", function (e) { //why is it being called twice if there's an off click??

	e.preventDefault();
	e.stopPropagation();
	$("#loading").show();
	console.log('in load products details summary');
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
	console.log($(".order-type" + val).val())
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
	var title = "Favorite";
	var requestFavorite = $(this);
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
	$("#priceSortContent1 .priceSort:checked").each(function (e) {
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
			CategorySelected: $('#categorySortContent .select-category').is(":checked"),
			SubCategorySelected: $('#categorySortContent .select-subcategory').is(":checked"),
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

