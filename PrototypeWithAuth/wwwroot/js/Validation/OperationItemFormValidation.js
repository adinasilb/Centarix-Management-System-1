$(function () {
	$(".product-name").rules("add", {
				required: true,
	});

	$(".product-hebrew-name").rules("add", {
				required: true,
	});

	$(".catalog-number").rules("add", {rules:{
			required: true,
				remote:{
		url: '/Requests/CheckUniqueVendorAndCatalogNumber',
		type: 'POST',
		data: { "VendorID":function(){ return $("#vendorList").val()}, "CatalogNumber": function(){return $("#Request_Product_CatalogNumber").val() } , "ProductID": function(){if ($(".turn-edit-on-off").length > 0) {
		return $(".turn-edit-on-off").attr("productID");
	}else{return null}}},
			}
	},
		  messages: {
       remote: "this product has already been created"
   }});
	

	$(".parent-category").rules("add", {
			selectRequired: true,
	});

	$(".sub-category").rules("add", {
			selectRequired: true,
	});

	$(".vendor").rules("add", {
			selectRequired: true,
	});

	$(".quote-number").rules("add", {
		required: true,
	});

	$(".quote-date").rules("add", {
		required: true,
	});
	$(".expected-supply-days").rules("add", {
			min: 1,
			integer: true,
			required: true
	});

	$(".warranty").rules("add", {
		min: 1,
		integer: true,
	});

	$(".expected-supply-days").rules("add", {
		min: 1,
		integer: true,
		required: true
	});

	$(".exchange-rate").rules("add", {
		min: 1,
		number: true,
	});
	$(".units").rules("add",  {
			required: true,
			number: true,
			min: 1,
			integer: true
	});

	$(".unit-type").rules("add", {
		selectRequired: true,
	});

	$(".cost").rules("add", {
			required: false,
			number: true,
			min: 1
	});

	$(".sum-dollars").rules("add", {
			required: false,
			number: true,
			min: 1
	});
});