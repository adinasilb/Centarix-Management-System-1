$(function () {
	$(".form-check.accounting-select .form-check-input ").on("click", function (e) {

		if (!$(this).is(':checked')) {

			$(this).closest("tr").attr("class", "text-center");
		}
		else {
			$.fn.AddBorderBySectionType(this);
		}
		var activeVendor = $(".activeVendor").val();
		if (activeVendor == "" && $(this).is(":checked")) {
			$(".activeVendor").val($(this).attr("vendorid"))
		}
		var selectedButton = $(this).closest("tbody").find(".button-for-selected-items");

		if ($(".form-check.accounting-select .form-check-input:checked").length) {
			var currentCurrency="";
			var differentCurrency = false;
			$(".form-check.accounting-select .form-check-input:checked").each(function(index){
					var classes =$(this).attr("class");
					console.log(classes)
					var classArray = classes.split(" ");
				for (var i = 0; i < classArray.length; i++) {
					console.log("class: "+classArray[i])
					if(classArray[i].indexOf("currencyEnum")==0)
					{							
						if(currentCurrency=="")
						{
							currentCurrency = classArray[i];
							return;
						}
						else if(currentCurrency != classArray[i])
						{
							differentCurrency = true;
							return;
						}

					}
				}
			});

			if (($(".activeVendor").val() != $(this).attr("vendorid")) ||differentCurrency) {
				var currentVendor =$(".activeVendor").val();
				var currentVendorDiv =".supplierName[value='"+currentVendor+"']";
				if($(".activeVendor").val() != $(this).attr("vendorid"))
				{ 
					$(currentVendorDiv).attr("id","vendorWarning" )
					window.location.href ="#vendorWarning"
					$(currentVendorDiv).removeAttr("id");
					$(currentVendorDiv).closest("table").next().find("tr.vendor-warning").removeClass("d-none");
					setTimeout(function(){ $(currentVendorDiv).closest("table").next().find("tr.vendor-warning").addClass("d-none"); }, 7000);
				}
				else if(differentCurrency){
				
					$(currentVendorDiv).attr("id","currencyWarning" )
					window.location.href ="#currencyWarning"
					$(currentVendorDiv).removeAttr("id");
					 $(currentVendorDiv).closest("table").next().find("tr.currency-warning").removeClass("d-none");
					setTimeout(function(){  $(currentVendorDiv).closest("table").next().find("tr.currency-warning").addClass("d-none"); }, 7000);
				}
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

	$.fn.AddBorderBySectionType = function (element) {
		$(element).closest("tr").addClass("clicked-border");
    }

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


	/*--------------------------------Accounting Payment Notifications--------------------------------*/
	$(".payments-pay-now").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var vendorid = $(this).attr("value");
		//var paymentstatusid = $(this).attr("paymentstatus");
		var typeEnum = $("#masterSidebarType").val();
		console.log("vendor: " + vendorid);
		//console.log("payment status: " + paymentstatusid);
		//var $itemurl = "Requests/TermsModal/?id=" + @TempData["RequestID"] + "&isSingleRequest=true"
		var itemurl = "/Requests/PaymentsPayModal/?vendorId=" + vendorid + "&accountingPaymentsEnum=" + typeEnum;
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

	$("body").off("click", ".pay-one").on("click", ".pay-one", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var typeEnum = $("#masterSidebarType").val();
		var paymentId = $(this).attr("value");
		var itemUrl = "/Requests/PaymentsPayModal/?paymentIds=" + paymentId + "&accountingPaymentsEnum=" + typeEnum;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "payments-pay");
	});
	//$(".pay-invoice-selected").off("click").on("click", function (e) {

		//var typeEnum = $(this).attr("type");
		//var itemUrl = "/Requests/PaymentsInvoiceModal/?accountingPaymentsEnum=" + typeEnum;
		//$.fn.LoadModalForSelectedItems(e, itemUrl, "payments-invoice");
	//});
	$("body").off("click", ".pay-selected").on("click", ".pay-selected", function (e) {
		var typeEnum = $(this).attr("type");
		var itemUrl = "/Requests/PaymentsPayModal/?accountingPaymentsEnum=" + typeEnum;
		var parameterName = 'paymentIds';
		$.fn.LoadModalForSelectedItems(e, itemUrl, "payments-pay", parameterName);
	});

	$("body").off("click", "invoice-add-all").on("click", "invoice-add-all", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log('invoice add all')
		var vendorid = $(this).attr("value");
		var itemUrl = "/Requests/AddInvoiceModal/?vendorid=" + vendorid;
		$("#loading").show();
		$.fn.CallModal(itemUrl, "add-invoice");
	});

	$(".add-to-selected").off("click").on("click", function (e) {
		console.log('add to selected')
		var itemUrl = "/Requests/AddInvoiceModal/";
		var parameterName = 'requestIds';
		$.fn.LoadModalForSelectedItems(e, itemUrl, "add-invoice", parameterName);

	});

	$("body").off("click", ".invoice-add-one").on("click", ".invoice-add-one", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log('invoice add one')
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
			//alert("in icon more popover remove share setup");
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
	$(".save-invoice").click(function (e) {
		console.log('save invoice')
		e.preventDefault();
		$("#myForm").data("validator").settings.ignore = "";
		var valid = $("#myForm").valid();
		console.log("valid form: " + valid)
		if (!valid) {
			e.preventDefault();
			if (!$('.activeSubmit').hasClass('disabled-submit')) {
				$('.activeSubmit').addClass('disabled-submit')
			}

		}
		else {
			var formData = new FormData($(".addInvoiceForm")[0]);
			$.ajax({
				contentType: false,
				processData: false,
				async: true,
				url: "/Requests/AddInvoiceModal",
				data: formData,
				traditional: true,
				type: "POST",
				cache: false,
				success: function (data) {
					$.fn.CloseModal("add-invoice");
					$("._IndexTableDataByVendor").html(data);
					return true;
				},
				error: function (jqxhr) {
					console.log("Error")
					//$.fn.OpenModal("modal", "payments-pay", jqxhr.responseText);
					$('.Accounting .error-message').html(jqxhr.responseText);
				}
			})
		}
		$("#myForm").data("validator").settings.ignore = ':not(select:hidden, .location-error:hidden,input:visible, textarea:visible)';
	});
});

