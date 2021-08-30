$('.createVendorForm').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
	rules: {
		"Vendor.VendorEnName": "required",
		"Vendor.VendorHeName": "required",
		"VendorCategoryTypes": { selectRequired: true },
		/*"Vendor.VendorBusinessID": {
			required: true,
			remote: {
				url: '/Vendors/CheckUniqueCompanyIDPerCountry',
				type: 'POST',
				data: {
					"CompanyID": function () { return $("#Vendor_VendorBuisnessID").val() },
					"CountryName": function () { return $("#Vendor_VendorCountry").val() },
					"VendorID": function () {
						if ($(".turn-edit-on-off").length > 0) {
							return $("#Vendor_VendorID").val();
						} else { return null }
					}
				},
			},
		},*/
		"Vendor.VendorCountry": "required",
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
		"Vendor.InfoEmail": {
			email: true
		},
		"Vendor.VendorAccountNum": {
			number: true,
			min: 1,
			integer: true
		},


	},
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