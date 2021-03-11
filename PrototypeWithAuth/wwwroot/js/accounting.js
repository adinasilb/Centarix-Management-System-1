$(function () {
	$(".form-check.accounting-select .form-check-input ").on("click", function (e) {
		 if (!$(this).is(':checked')) 
		 {
                 $(this).closest("tr").removeClass("clicked-border-acc");			
         }
		var activeVendor = $(".activeVendor").val();
		if(activeVendor == "" && $(this).is(":checked"))
		{
		//	alert("reset vendor")
			 $(".activeVendor").val($(this).attr("vendorid"))
		}
		var addToSelectedButton = $("#add-to-selected");
		var paySelectedButton = $("#pay-selected");
	

		var selectedButton;
		if (addToSelectedButton.length>0) {
			selectedButton = addToSelectedButton;
		}
		else if (paySelectedButton.length>0) {
			selectedButton = paySelectedButton;
		}
	
		if ($(".form-check.accounting-select .form-check-input:checked").length) {
			if( $(".activeVendor").val() !=$(this).attr("vendorid"))
			{
				//alert("active vendors are ot equal - not doing anything")
				$(this).removeAttr("checked");
				$(this).prop("checked", false);
				//alert("count checked: "+$(".form-check.accounting-select .form-check-input:checked").length)
				return false;
			}
	
			//alert("after if -continuing with if ")
			 $(this).closest("tr").addClass("clicked-border-acc");

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
		var vendorid = $(this).attr("vendor");
		var paymentstatusid = $(this).attr("paymentstatus");
		var typeEnum = $(this).attr("type");
		console.log("vendor: " + vendorid);
		console.log("payment status: " + paymentstatusid);
		//var $itemurl = "Requests/TermsModal/?id=" + @TempData["RequestID"] + "&isSingleRequest=true"
		var itemurl = "/Requests/PaymentsPayModal/?vendorid=" + vendorid + "&paymentstatusid=" + paymentstatusid + "&accountingPaymentsEnum=" + typeEnum;
		$("#loading").show();
		$.fn.CallModal(itemurl, "payments-pay");
	});

	$("#pay-selected").off("click").on("click", function (e) {
		var typeEnum = $(this).attr("type");
		var arrayOfSelected = $(".form-check.accounting-select .form-check-input:checked").map(function () {
			return $(this).attr("id")
		}).get()
		console.log("arrayOfSelected: " + arrayOfSelected);
		$("#loading").show();
		$.ajax({
			type: "GET",
			url: "/Requests/PaymentsPayModal/?"+"accountingPaymentsEnum=" + typeEnum,
			traditional: true,
			data: { 'requestIds': arrayOfSelected },
			cache: true,
			success: function (data) {
				$.fn.OpenModal("modal", "payments-pay", data)
				$("#loading").hide();
			}
		});
	});

	$(".invoice-add-all").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var vendorid = $(this).attr("vendor");
		var itemUrl = "/Requests/AddInvoiceModal/?vendorid=" + vendorid;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "add-invoice");
	});

	$("#add-to-selected").off("click").on("click", function (e) {
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
		var requestid = $(this).attr("request");
		var itemUrl = "/Requests/AddInvoiceModal/?requestid=" + requestid;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "add-invoice");
	});

});

