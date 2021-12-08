$('.createVendorForm').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
	rules: {
		"Vendor.VendorEnName": "required",
		"Vendor.VendorHeName": "required",
		"VendorCategoryTypes": { selectRequired: true },
		"Vendor.VendorBuisnessID": {
			required: true,
			remote: {
				url: '/Vendors/CheckUniqueCompanyIDAndCountry',
				type: 'POST',
				data: {
					"CompanyID": function () { return $("#Vendor_VendorBuisnessID").val() },
					"CountryID": function () { return $("#VendorCountries").val() },
					"VendorID": function () {
						if ($(".turn-edit-on-off").length > 0) {
							return $(".turn-edit-on-off").attr("value");
						} else { return null }
					}
				},
			},
		},
		"Vendor.CountryID": { selectRequired: true },
		"Vendor.VendorCity": "required",
		"Vendor.VendorTelephone": {
			required: true,
			minlength: 9
		},
		"Vendor.VendorCellPhone": {
			minlength: 9
		},
		"Vendor.VendorFax": {
			minlength: 9
		},
		"Vendor.OrdersEmail": {
			email: true,
			required: true
		},
		"Vendor.QuotesEmail": {
			email: true,
			required: true
        },
		"Vendor.InfoEmail": {
			email: true
		},
		"Vendor.VendorAccountNum": {
			number: true,
			min: 1,
			integer: true
		}
	},

	messages: {
		"Vendor.VendorBuisnessID": {
			remote: "this company has already been added"
		},
	}
	
});
$(".contact-name").rules("add", "required");
$(".contact-email").rules("add", {
	required: true,
	email: true
});
$(".contact-phone").rules("add", {
	required: true,
	minlength: 9
});

$("body").off("change", '#VendorCountries').on("change", '#VendorCountries', function () {
	console.log("in change country")
	$('.error').addClass("beforeCallValid");
	$('#Vendor_VendorBuisnessID').valid();
	$(".error:not(.beforeCallValid)").addClass("afterCallValid")
	$(".error:not(.beforeCallValid)").removeClass("error")
	$("label.afterCallValid").remove()
	$(".error").removeClass('beforeCallValid')
	$(".afterCallValid").removeClass('error')
	$(".afterCallValid").removeClass('afterCallValid')

});