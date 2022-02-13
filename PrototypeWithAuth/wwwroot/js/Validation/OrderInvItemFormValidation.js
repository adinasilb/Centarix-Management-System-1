
$(function () {
$('.ordersItemForm').validate({
	 normalizer: function( value ) {
    return $.trim( value );
  },
	rules: {
		/*"Requests[0].Product.ProductName": "required",*/ //insert val here?? has there
		
		"Requests[0].ParentQuote.QuoteNumber": {
			required: true
		},
		"Requests[0].ParentQuote.QuoteDate": {
			required: true,
			//mindate: new Date('1900-12-17T03:24:00')
		},
		"Requests[0].ArrivalDate": {
			required: true
		},
		"Requests[0].ExpectedSupplyDays": {
			min: 0,
			integer: true,
			required: true
		},
		"Requests[0].Warranty": {
			min: 0,
			integer: true
		},
		"Requests[0].URL": {
			//url: true
		},
		"Requests[0].ExchangeRate": {
			//required: function () {
			//	return $("#currency").val() == "dollar" || $("#currency").val() == null;
			//},
			number: true,
			min: 1
		},

		"Requests[0].Unit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		
		"Requests[0].Cost": {
			required: true,
			number: true,
			min: 1
		},
		"sum-dollars": {
			required: false,
			number: true,
			min: 1
		},
		"locationTypeSelected": "locationRequired",
		"subLocationSelected": "locationRequired",
		"locationVisualSelected": "locationRequired"

	},
	messages:{
/*		"subLocationSelected": "Please choose a location before submitting",
		"locationVisualSelected": "Please choose a location before submitting",
		"locationTypeSelected": "Please choose a location before submitting",*/
        ".catalog-number": {
            remote: "this product has already been created"
        },
		}
});

	$(".item-name").rules("add", "required");
	$(".catalog-number").rules("add", {
		required: true,
		remote: {
			url: '/Requests/CheckUniqueVendorAndCatalogNumber',
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
	});
	$("#parentlist").rules("add", "selectRequired");
	$("#sublist").rules("add", "selectRequired");
	$("#unitTypeID").rules("add", "selectRequired");
	$("#subUnitTypeID").rules("add", "selectRequired");
	$("#subSubUnitTypeID").rules("add", "selectRequired");
	$(".vendorList").rules("add", "selectRequired");
	$("#subUnit").rules("add", {
		required: true,
		number: true,
		greaterThan: 0
	});
	$("#subSubUnit").rules("add", {
		required: true,
		number: true,
		greaterThan: 0
	});

	$("body, .modal").off("change", '#vendorList').on("change", '#vendorList' , function(){
		//console.log("in change vendor")
		//$('#Request_0__Product_CatalogNumber').valid();
		$('.error').addClass("beforeCallValid");
		$('.catalog-number').valid();
		$(".error:not(.beforeCallValid)").addClass("afterCallValid")
		$(".error:not(.beforeCallValid)").removeClass("error")
		$("label.afterCallValid").remove()
		$(".error").removeClass('beforeCallValid')
		$(".afterCallValid").removeClass('error')
		$(".afterCallValid").removeClass('afterCallValid')

	});
	
});


