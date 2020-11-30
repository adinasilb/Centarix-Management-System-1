$('.createVendorForm').validate({
	rules: {
		"Vendor.VendorEnName": "required",
		"Vendor.VendorHeName": "required",
		"VendorCategoryTypes": "selectRequired",
		"Vendor.VendorBuisnessID": {
			required: true,
			number: true,
			min: 1
		},
		"Vendor.VendorCountry": "required",
		"Vendor.VendorCity": "required",
		"Vendor.VendorStreet": "required",
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
		"Vendor.VendorBank": "required",
		"Vendor.VendorBankBranch": "required",
		"Vendor.VendorAccountNum": {
			required: true,
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