$(function () {
	$(".form-check.accounting-select .form-check-input ").on("click", function (e) {

		if (!$(this).is(':checked')) {

			$(this).closest("tr").attr("class", "text-center");
		}
		else {
			//alert("is checked")
			$(this).closest("tr").addClass("clicked-border-acc");
		}
		var activeVendor = $(".activeVendor").val();
		if (activeVendor == "" && $(this).is(":checked")) {
			//	alert("reset vendor")
			$(".activeVendor").val($(this).attr("vendorid"))
		}
		var addToSelectedButton = $(this).closest("tbody").find(".add-to-selected");
		var paySelectedButton = $(this).closest("tbody").find(".pay-selected");


		var selectedButton;
		if (addToSelectedButton.length > 0) {
			selectedButton = addToSelectedButton;
		}
		else if (paySelectedButton.length > 0) {
			selectedButton = paySelectedButton;
		}

		if ($(".form-check.accounting-select .form-check-input:checked").length) {
			if ($(".activeVendor").val() != $(this).attr("vendorid")) {
				//	alert("active vendors are not equal - not doing anything")
				$(this).removeAttr("checked");
				$(this).prop("checked", false);
				//alert("count checked: "+$(".form-check.accounting-select .form-check-input:checked").length)
				$(this).closest("tr").attr("class", "text-center");
				return false;
			}

			if (selectedButton.hasClass("hidden")) {
				selectedButton.removeClass("hidden");
			}

		}
		else {
			if (!selectedButton.hasClass("hidden")) {
				selectedButton.addClass("hidden");
			}
			$(".activeVendor").val($(this).attr(""))
		}
	});


	$(".remove-invoice-item").off("click").on("click", function (e) {
		e.stopPropagation();
		e.preventDefault();
		$(this).closest("tr").replaceWith("");
		if ($(".invoice-request").length == 1) {
			$(".remove-invoice-item").replaceWith("");
		}
	});



	$("#share-payment").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("in share payments site.js");
	});

	function SharePayment(e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("in share payments fx site.js");
	};

	$("body").on("click", "#share-payment", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("in share payments body fx site.js");
	});

	/*--------------------------------Accounting Payment Notifications--------------------------------*/
	$(".payments-pay-now").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var vendorid = $(this).attr("value");
		var paymentstatusid = $(this).attr("paymentstatus");
		var typeEnum = $("#masterSidebarType").val();
		console.log("vendor: " + vendorid);
		console.log("payment status: " + paymentstatusid);
		//var $itemurl = "Requests/TermsModal/?id=" + @TempData["RequestID"] + "&isSingleRequest=true"
		var itemurl = "/Requests/PaymentsPayModal/?vendorid=" + vendorid + "&paymentstatusid=" + paymentstatusid + "&accountingPaymentsEnum=" + typeEnum;
		$("#loading").show();
		$.fn.CallModal(itemurl, "payments-pay");
	});

	//$(".payments-invoice-pay").off("click").on("click", function (e) {
	//	e.preventDefault();
	//	e.stopPropagation();
	//	var vendorid = $(this).attr("value");
	//	var paymentstatusid = $(this).attr("paymentstatus");
	//	var typeEnum = $("#masterSidebarType").val();
	//	console.log("vendor: " + vendorid);
	//	//var $itemurl = "Requests/TermsModal/?id=" + @TempData["RequestID"] + "&isSingleRequest=true"
	//	var itemurl = "/Requests/PaymentsInvoiceModal/?vendorid=" + vendorid + "&paymentstatusid=" + paymentstatusid + "&accountingPaymentsEnum=" + typeEnum;
	//	$("#loading").show();
	//	$.fn.CallModal(itemurl, "payments-invoice");
	//});
	$(".pay-invoice-one").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var typeEnum = $("#masterSidebarType").val();
		var paymentid = $(this).attr("value");
		var itemUrl = "/Requests/PaymentsInvoiceModal/?paymentId=" + paymentid + "&accountingPaymentsEnum=" + typeEnum;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "payments-invoice");
	});

	$(".pay-one").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var typeEnum = $("#masterSidebarType").val();
		var requestid = $(this).attr("value");
		var itemUrl = "/Requests/PaymentsPayModal/?requestid=" + requestid + "&accountingPaymentsEnum=" + typeEnum;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "payments-pay");
	});
	//$(".pay-invoice-selected").off("click").on("click", function (e) {
	//	var typeEnum = $(this).attr("type");
	//	var arrayOfSelected = $(".form-check.accounting-select .form-check-input:checked").map(function () {
	//		return $(this).attr("id")
	//	}).get()
	//	console.log("arrayOfSelected: " + arrayOfSelected);
	//	$("#loading").show();
	//	$.ajax({
	//		type: "GET",
	//		url: "/Requests/PaymentsInvoiceModal/?" + "accountingPaymentsEnum=" + typeEnum,
	//		traditional: true,
	//		data: { 'requestIds': arrayOfSelected },
	//		cache: true,
	//		success: function (data) {
	//			$.fn.OpenModal("modal", "payments-invoice", data)
	//			$("#loading").hide();
	//		}
	//	});
	//});
	$(".pay-selected").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		//alert('pay selected')
		var typeEnum = $(this).attr("type");
		var arrayOfSelected = $(".form-check.accounting-select .form-check-input:checked").map(function () {
			return $(this).attr("id")
		}).get()
		//alert('before loading');
		console.log("arrayOfSelected: " + arrayOfSelected);
		$("#loading").show();
		$.ajax({
			type: "GET",
			url: "/Requests/PaymentsPayModal/?" + "accountingPaymentsEnum=" + typeEnum,
			traditional: true,
			data: { 'requestIds': arrayOfSelected },
			cache: true,
			success: function (data) {
				//alert('success!');
				$.fn.OpenModal("modal", "payments-pay", data)
				$("#loading").hide();
			}
		});
	});

	$(".invoice-add-all").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var vendorid = $(this).attr("value");
		var itemUrl = "/Requests/AddInvoiceModal/?vendorid=" + vendorid;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "add-invoice");
	});

	$(".add-to-selected").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		//alert("add to selected")
		var arrayOfSelected = $(".form-check.accounting-select .form-check-input:checked").map(function () {
			return $(this).attr("id")
		}).get()
		console.log("arrayOfSelected: " + arrayOfSelected);
		//var itemUrl = "/Requests/AddInvoiceModal/?requestids=" + arrayOfSelected;
		$("#loading").show();
		$.ajax({
			type: "GET",
			url: "/Requests/AddInvoiceModal/",
			traditional: true,
			data: { 'requestIds': arrayOfSelected },
			cache: true,
			success: function (data) {
				$.fn.OpenModal("modal", "add-invoice", data)
				$("#loading").hide();
			}
		});
	});

	$(".invoice-add-one").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var requestid = $(this).attr("value");
		var itemUrl = "/Requests/AddInvoiceModal/?requestid=" + requestid;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "add-invoice");
	});

	$(".more, .accNotification").off('click').click(function () {
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

		//set up remove share on here
		
		$(".icon-more-popover").off("click").on("click", ".remove-share", function (e) {
			var ModelsEnum = "";
			var shareNum = "";
			alert("in icon more popover remove share setup");
			if ($(this).hasClass("resources")) { //THIS IF IS NOT WORKING
				ModelsEnum = "Resource";
				shareNum = $(this).attr("data-share-resource-id");
			}
			if ($(this).hasClass("resources")) { //THIS IF IS NOT WORKING
				ModelsEnum = "Resource";
				shareNum = $(this).attr("data-share-resource-id");
			}
			var url = "/Protocols/RemoveShare?ShareID=" + shareNum + "&ModelsEnum=" + ModelsEnum;
			$.ajax({
				async: true,
				url: url,
				type: 'GET',
				cache: true,
				success: function (data) {
					$.ajax({
						async: true,
						url: "/Protocols/_ResourcesListIndex?sidebarEnum=" + $("#SidebarEnum").val(),
						type: 'GET',
						cache: true,
						success: function (d) {
							$(".resources-shared-partial").html(d);
						}
					})
				}
			});
		});
	});
});

