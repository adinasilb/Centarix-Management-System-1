$('.createVendorForm').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
	rules: {
		"Vendor.VendorEnName": "required",
		"Vendor.VendorHeName": "required",
		"VendorCategoryTypes": { selectRequired: true },
		"Vendor.VendorBuisnessID": {
			/*required: true,
			number: true,
			min: 1*/
		
			required: true,
			remote: {
				url: '/Vendors/CheckUniqueCompanyIDAndCountry',
				type: 'POST',
				data: {
					"VendorID": function () { return $("#vendorList").val() },
					"CatalogNumber": function () { return $("#Requests_0__Product_CatalogNumber").val() },
					"ProductID": function () {
						if ($(".turn-edit-on-off").length > 0) {
							return $(".turn-edit-on-off").attr("productID");
						} else { return null }
					}
				},
			},
		},
		"Vendor.VendorCountry": { selectRequired: true },
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