$(function () {
	$(".product-name").each(function(){
		$(this).rules("add", {
			required: true,
		});
	});

	$(".product-hebrew-name").each(function(){
			$(this).rules("add", {
			required: true,
		});
	});

	$(".catalog-number").each(function(){
		var thisElement =  $(this);
			$(this).rules("add", {
					required: true,
					remote:{
						url: '/Requests/CheckUniqueVendorAndCatalogNumber',
						type: 'POST',
						data: { "VendorID":function(){ return $("#vendorList").val()}, "CatalogNumber": function(){return $(thisElement).val() } , "ProductID": function(){if ($(".turn-edit-on-off").length > 0) {
						return $(".turn-edit-on-off").attr("productID");
						}else{return null}}},
					},
			       messages: {
			 remote: "this product has already been created"
					  }
		});
	});
	

	$(".parent-category").each(function(){
		$(this).rules("add", {
			selectRequired: true,
		});
	});

	$(".sub-category").each(function(){
		$(this).rules("add", {
		selectRequired: true,
		});
	});

	$(".vendor").each(function(){
		$(this).rules("add", {
			selectRequired: true,
		});
	});

	$(".quote-number").each(function(){
		$(this).rules("add", {
		required: true,
		});
	});

	$(".quote-date").each(function(){
		$(this).rules("add", {
		required: true,
		});
	});

	$(".expected-supply-days").each(function(){
		$(this).rules("add", {
		min: 1,
		integer: true,
		required: true
		});
	});


	$(".warranty").each(function(){
		$(this).rules("add", {
			min: 1,
			integer: true,
		});
	});

	$(".expected-supply-days").each(function(){
		$(this).rules("add", {
		min: 1,
		integer: true,
		required: true
		});
	});

	$(".exchange-rate").each(function(){
		$(this).rules("add", {
		min: 1,
		number: true,
		});
	});

	$(".unit").each(function(){
		$(this).rules("add", {
		required: true,
		number: true,
		min: 1,
		integer: true
		});
	});

	$(".unit-type").each(function(){
		$(this).rules("add", {
		selectRequired: true,
		});
	});

	$(".cost").each(function(){
		$(this).rules("add", {
		required: false,
		number: true,
		min: 1
		});
	});

	$(".sum-dollars").each(function(){
		$(this).rules("add", {
			required: false,
			number: true,
			min: 1
		});
	});
});