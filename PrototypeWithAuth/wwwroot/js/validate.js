$(function () {
	jQuery.validator.setDefaults({
		ignore: "",

	});
	$.validator.addMethod("selectRequired", function (value, element) {
		console.log("value" + value);
		return this.optional(element)|| value != '';
	}, 'Field is required');
    $('.addItemForm').validate({ // initialize the Plugin
			rules: {
				"Request.CatalogNumber": {
					required: true,
					number: true
					},
			"data[data-activates][select-options-parentlist]":"required",
			},
			messages: {
      
			}
		
    //submitHandler: function (form) {
    //    alert('valid form submitted');
    //    return false;
    //}
});

});
$.fn.validateItemTab = function () {
	$(".request-price-tab").prop("disabled", true);
	$(".request-location-tab").prop("disabled", true);
	$(".request-order-tab").prop("disabled", true);

	console.log("in $.fn.validateItemTab");
	//valid = $("#parentlist").attr('aria-invalid');
	//if (valid == "true" || $("#parentlist").val() == "") {
	//	console.log("valid: " + valid);
	//	return;
	//}
	valid = $("#Request_Product_ProductName").attr('aria-invalid');
	if (valid == "true" || $("#Request_Product_ProductName").val() == "") {
		console.log("valid: " + valid);
		return;
	}
	valid = $("#sublist").attr('aria-invalid');
	if (valid == "true" || $("#sublist").val() == "") {
		return;
	}
	valid = $("#Request_SubProject_ProjectID").attr('aria-invalid');
	if (valid == "true" || $("#Request_SubProject_ProjectID").val() == "") {
		return;
	}
	console.log("valid1: " + valid);
	valid = $("#SubProject").attr('aria-invalid');
	if (valid == "true" || $("#SubProject").val() == "") {
		return;
	}
	valid = $("#vendorList").attr('aria-invalid');
	if (valid == "true" || $("#vendorList").val() == "") {
		return;
	}
	valid = $("#Request_Warranty").attr('aria-invalid');
	if (valid == "true") {
		return;
	}
	valid = $("#Request_ParentQuote_QuoteDate").attr('aria-invalid');
	if (valid == "true" || $("#Request_ParentQuote_QuoteDate").val() == "") {
		return;
	}
	valid = $("#Request_ParentQuote_QuoteNumber").attr('aria-invalid');
	if (valid == "true" || $("#Request_ParentQuote_QuoteNumber").val() == "") {
		return;
	}
	valid = $("#Request_ExpectedSupplyDays").attr('aria-invalid');
	if (valid == "true") {
		return;
	}
	valid = $("#Request_CatalogNumber").attr('aria-invalid');
	if (valid == "true" || $("#Request_CatalogNumber").val() == "") {
		return;
	}
	if (valid == "false" || valid == undefined) {
		console.log("made it here!!")
		$(".request-price-tab").prop("disabled", false);
		$(".request-location-tab").prop("disabled", false);
		$(".request-order-tab").prop("disabled", false);
	}
	return valid;
}
