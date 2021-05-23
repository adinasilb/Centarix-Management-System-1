
$(function () {

	$.fn.ShowResults = function ($inputBox, $value) { //this function ensures that the value passed back won't be NaN or undefined --> it'll instead send back a blank
		var theResult = parseFloat($value);
		theResult = theResult.toFixed(2);
		theResult = isFinite(theResult) && theResult || "";
		$inputBox.val(theResult);
	}

	$.fn.CalculateSumPlusVat = function (index = "0") {
		var $exchangeRate = $("#exchangeRate").val();
		var inverseExchangeRate = 1/$exchangeRate
		if ($exchangeRate == "0") {
			$exchangeRate = "1";
		}
		var dollarId = "sum-dollars";
		var shekelId = "cost"
		var vatId = "vat";
		var vatDollarId = "vatInDollars";
		var totalVatId = "sumPlusVat-Shekel";
		var totalVatDollarId = "sumPlusVat-Dollar";
		if ($('#masterSectionType').val() == "Operations")
		{
			dollarId = dollarId + index;
			shekelId = "Requests_" + index + "__Cost";
			vatId = "Requests_" + index + "__VAT";
			vatDollarId = vatDollarId + index;
			totalVatId = "Requests_" + index + "__TotalWithVat";
			totalVatDollarId = totalVatDollarId + index;
		}
		//console.log("sumShek: " + sumShek);

		//console.log("VatPercentage: " + VatPercentage);
		//console.log("vatCalc: " + vatCalc);
		//$("#Request_VAT").val(vatCalc)
		//var vatInShekel = $("#Request_VAT").val();
		if ($("#" + dollarId).prop("disabled") || $("#" + dollarId).hasClass("disabled")) {
			$sumDollars = parseFloat($("#" + shekelId).val()) * inverseExchangeRate;
			console.log("sumDollars"+$sumDollars)
			$iptBox = $('#'+dollarId);
			$.fn.ShowResults($iptBox, $sumDollars);
		}
		else if ($("#" + shekelId).prop("readonly")) {
			$sumShekel = $("#" + dollarId).val() * $exchangeRate;
			console.log("shekel " + $sumShekel)
			$iptBox = $("#" + shekelId);
			$.fn.ShowResults($iptBox, $sumShekel);
		}
		$sumShekel = parseFloat($("#" + shekelId).val());
		var vatCalc = $sumShekel * .17;
		//$vatOnshekel = $sumShekel * parseFloat(vatCalc);
		$('#' + vatId).val(vatCalc.toFixed(2));
		console.log("vat calc " + vatCalc)
		$('#' + vatDollarId).val((vatCalc * inverseExchangeRate).toFixed(2));
		$sumTotalVatShekel = $sumShekel + vatCalc;
		$iptBox = $("#" + totalVatId);
		$.fn.ShowResults($iptBox, $sumTotalVatShekel);
		$sumTotalVatDollars = $sumTotalVatShekel * inverseExchangeRate;
		$iptBox = $("#" + totalVatDollarId);
		$.fn.ShowResults($iptBox, $sumTotalVatDollars);
	};
	$.fn.CalculateUnitAmounts = function () {
		$unitSumShekel = parseFloat($("#cost").val()) / $("#unit").val();
		$iptBox = $("input[name='unit-price-shekel']");
		$.fn.ShowResults($iptBox, $unitSumShekel);
		var $exchangeRate = $("#exchangeRate").val();
		var inverseExchangeRate = 1 / $exchangeRate;
		$unitSumDollars = $unitSumShekel * inverseExchangeRate;
		$iptBox = $("input[name='unit-price-dollars']");
		$.fn.ShowResults($iptBox, $unitSumDollars);
	};

	$.fn.CalculateSubUnitAmounts = function () {
		$subUnitSumShekel = $("#unit-price-shekel").val() / $("#subUnit").val();
		$iptBox = $("input[name='subunit-price-shekel']");
		$.fn.ShowResults($iptBox, $subUnitSumShekel);
		var $exchangeRate = $("#exchangeRate").val();
		$subUnitSumDollars = $subUnitSumShekel / $exchangeRate;
		$iptBox = $("input[name='subunit-price-dollars']");
		$.fn.ShowResults($iptBox, $subUnitSumDollars);

		//for the reorder modal
		$subunit = $("#subUnit");
		if ($subunit.val() != null && $subunit.val() > 0) {
			//$.fn.EnableSubUnits();
			//$.fn.EnableMaterialSelect('#Request_SubUnitTypeID', 'select-options-Request_SubUnitTypeID');
		}
		else {
			//console.log('disabling');
			//$.fn.DisableSubUnits();
		}
	};

	$.fn.CalculateSubSubUnitAmounts = function () {
		$subSubUnitSumShekel = $("#subunit-price-shekel").val() / $("#subSubUnit").val();
		$iptBox = $("input[name='subsubunit-price-shekel']");
		$.fn.ShowResults($iptBox, $subSubUnitSumShekel);
		var $exchangeRate = $("#exchangeRate").val();
		$subSubUnitSumDollars = $subSubUnitSumShekel / $exchangeRate;
		$iptBox = $("input[name='subsubunit-price-dollars']");
		$.fn.ShowResults($iptBox, $subSubUnitSumDollars);
		//for the reorder modal
		$subsubunit = $("#subSubUnit");
		if ($subsubunit.val() != null && $subsubunit.val() > 0) {
			//$.fn.EnableSubSubUnits();
		}
		else {
			//$.fn.DisableSubSubUnits();
		}
	};
	$.fn.CalculatePriceShekels = function () {
		var $exchangeRate = $("#exchangeRate").val();
		var $unitPrice = $("#unit-price-shekel").val();
		$.fn.CalculateSum();
		$unitPriceDollars = $unitPrice / $exchangeRate;
		console.log("exchange rate" + $exchangeRate)
		$iptBox = $("input[name='unit-price-dollars']");
		$.fn.ShowResults($iptBox, $unitPriceDollars)

		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	};
	$.fn.CalculatePriceDollars = function () {
		console.log("calculate dollars")
		var $unitPriceDollars = $("#unit-price-dollars").val();
		var $exchangeRate = $("#exchangeRate").val();
		$priceShekels = $unitPriceDollars * $exchangeRate;
		$iptBox = $("#unit-price-shekel");
		$.fn.ShowResults($iptBox, $priceShekels);
		var $priceDollars = $unitPriceDollars * $("#unit").val();
		var $iptBox = $("input[name='sum-dollars']");
		$.fn.ShowResults($iptBox, $priceDollars);
		
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		$.fn.CalculateSumPlusVat();
	};
	$.fn.CalculateSum = function () {
		var $exchangeRate = $("#exchangeRate").val();
		var $unitPrice = $("#unit-price-shekel").val();
		var $priceShekels = $unitPrice * $("#unit").val();
		$iptBox = $("#cost");
		$.fn.ShowResults($iptBox, $priceShekels);
		var $priceDollars = $priceShekels / $exchangeRate;
		var $iptBox = $("input[name='sum-dollars']");
		$.fn.ShowResults($iptBox, $priceDollars);
		$.fn.CalculateSumPlusVat();
    }
	$.fn.EnableSubUnits = function () {
		$("#subUnit").prop("disabled", false);
		$.fn.EnableMaterialSelect('#subUnitTypeID', 'select-options-subUnitTypeID');
	};
	$.fn.ChangeSubUnitDropdown = function () {
		console.log("change subunit dropdown");
		var selected = $(':selected', $("#unitTypeID"));
		var selected2 = $(':selected', $("#select-options-unitTypeID"));
		//console.log("u selected: " + selected);
		var optgroup = selected.closest('optgroup').attr('label');
		var optgroup2 = selected2.closest('optgroup').attr('label');
		console.log("u optgroup: " + optgroup);
		console.log("u optgroup2: " + optgroup2);
		//the following is based on the fact that the unit types and parents are seeded with primary key values
		var selectedIndex = $('#select-options-subUnitTypeID').find(".active").index();
		console.log("select index" + selectedIndex)
		var subOptgroup = $(':selected', $("#subUnitTypeID")).closest('optgroup').attr('label');
		switch (subOptgroup) {
			case "Units":
				console.log("Units")
				selectedIndex = selectedIndex - 1;
				break;
			case "Weight/Volume":
				console.log("Volume")
				selectedIndex = selectedIndex - 2;
				break;
			case "Test":
				console.log("Test")
				selectedIndex = selectedIndex - 3;
				console.log("select index test" + selectedIndex)
				break;
		}

		$('#subUnitTypeID').destroyMaterialSelect();
		$('#subUnitTypeID').prop('selectedIndex', selectedIndex);
		switch (optgroup) {
			case "Units":
				console.log("inside optgroup units");
				//$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				//	$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").show();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").show();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").css("display", "none");
				$("#subUnitTypeID optgroup[label='Units'] option").prop('disabled', false);
				$("#subUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);


				break;
			case "Weight/Volume":
				//$(".subunit-subunit").hide();
				//$("#select-options-Request_SubUnitTypeID option").prop('hidden', true);
				//$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").hide();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").show();
				$("#subUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$("#subUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);

				//	$("#Request_SubUnitTypeID").hide();
				break;
			case "Test":
				//console.log("inside optgroup test");
				//$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").hide();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").hide();
				$("#subUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$('#select-options-subUnitTypeID li.optgroup:nth-child(3)').addClass('.active');
				$("#subUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', true);

				break;
		}
		$("#subUnitTypeID").materialSelect();
		//$("#subUnit").prop("disabled", false);
		//$.fn.EnableMaterialSelect('#subUnitTypeID', 'select-options-subUnitTypeID');
		switch (optgroup2) {
			case "Units":
				console.log("inside optgroup2 units");
				$("#select-options-subUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				$("#select-options-subUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Weight/Volume":
				console.log("inside optgroup2 weight/volume");
				$("#select-options-subUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-subUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Test":
				console.log("inside optgroup2 test");
				$("#select-options-subUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-subUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				break;
		}
	};
	//change sub sub unit dropdown
	$.fn.ChangeSubSubUnitDropdown = function () {
		console.log("in change subsubunitdropdown");
		var selected = $(':selected', $("#subUnitTypeID"));
		var selected2 = $(':selected', $("#select-options-subUnitTypeID"));
		var optgroup = selected.closest('optgroup').attr('label');
		var optgroup2 = selected2.closest('optgroup').attr('label');
		var selectedIndex = $('#select-options-subSubUnitTypeID').find(".active").index();
		console.log("select index" + selectedIndex)
		var subOptgroup = $(':selected', $("#subSubUnitTypeID")).closest('optgroup').attr('label');
		switch (subOptgroup) {
			case "Units":
				console.log("Units")
				selectedIndex = selectedIndex - 1;
				break;
			case "Weight/Volume":
				console.log("Volume")
				selectedIndex = selectedIndex - 2;
				break;
			case "Test":
				console.log("Test")
				selectedIndex = selectedIndex - 3;
				console.log("select index test" + selectedIndex)
				break;
		}

		$('#subSubUnitTypeID').destroyMaterialSelect();
		$('#subSubUnitTypeID').prop('selectedIndex', selectedIndex);
		console.log($('#subSubUnitTypeID').prop('selectedIndex'));
		switch (optgroup) {
			case "Units":
				//$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				//$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				$("#subSubUnitTypeID optgroup[label='Units'] option").prop('disabled', false);
				$("#subSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);
				break;
			case "Weight/Volume":
				//$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				$("#subSubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$("#subSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);
				break;
			case "Test":
				//$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				$("#subSubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$("#subSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', true);
				break;
		}
		$("#subSubUnitTypeID").materialSelect();
		//$.fn.EnableMaterialSelect('#subSubUnitTypeID', 'select-options-subUnitTypeID');
		switch (optgroup2) {
			case "Units":
				$("#select-options-subSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				$("#select-options-subSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Weight/Volume":
				$("#select-options-subSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-subSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Test":
				$("#select-options-subSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-subSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				break;
		}
	};


	$.fn.EnableSubSubUnits = function () {
		$("#subSubUnit").prop("disabled", false);
		$.fn.EnableMaterialSelect('#subSubUnitTypeID', 'select-options-subSubUnitTypeID');
	};
	$.fn.DisableSubUnits = function () {
		$("#subUnit").prop("disabled", true);
		$("#subUnitTypeID").destroyMaterialSelect();
		$("#subUnitTypeID").prop("disabled", true);
		$("#subUnitTypeID").materialSelect();

		//$("#select-options-Request_SubUnitTypeID").prop("disabled", true);
		//$("#select-options-Request_SubUnitTypeID").attr("aria-disabled", true);
		//disable validation
		//$('#Request_SubUnitTypeID').rules("remove", "selectRequired");
	};

	$.fn.DisableSubSubUnits = function () {
		$("#subSubUnit").prop("disabled", true);
		$("#subSubUnitTypeID").destroyMaterialSelect();
		$("#subSubUnitTypeID").prop("disabled", true);
		$("#subSubUnitTypeID").materialSelect();
	};
	$.fn.CheckUnitsFilled = function () {
		console.log("in check units function");
		if (($("#edit #unit").val() > 0 && $("#edit #unitTypeID").val())
			|| ($("#select-options-unit").val() > 0 && $("#select-options-unitTypeID").val())) {
			//console.log("both have values");
			$('.subUnitsCard').removeClass('d-none');
			$('.sub-close').removeClass('d-none');
			$('.addSubUnitCard').addClass('d-none');
			$('.RequestSubsubunitCard').removeClass('d-none');
			$("#subUnit").addClass('mark-readonly');
			$("#subUnit").prop("disabled", false);
			$("#subUnitTypeID").addClass('mark-readonly');
			$.fn.ChangeSubUnitDropdown();
			$.fn.CheckCurrency();
			if ($("#quoteStatus").val() == "1" || $("#quoteStatus").val() == "2") {
				console.log("no quote")
				$(".requestPriceQuote").prop("disabled", true);
			}
		}
		//else {
		//	$.fn.DisableSubUnits();
		//	$.fn.DisableSubSubUnits();
		//}
		//$.fn.CalculateUnitAmounts();
		//$.fn.CalculateSubUnitAmounts();
		//$.fn.CalculateSubSubUnitAmounts();
	};
	$.fn.CheckSubUnitsFilled = function () {
		if (($("#subUnit").val() > 0 && $("#subUnitTypeID").val())
			|| ($("#subUnit").val() > 0 && $("#select-options-subUnitTypeID").val())) {
			//$.fn.EnableSubSubUnits();
			$('.subSubUnitsCard').removeClass('d-none');
			$('.subsub-close').removeClass('d-none');
			$('.addSubSubUnitCard').addClass('d-none');
			//console.log("about to change subsubunitdropdown");
			$("#subSubUnit").addClass('mark-readonly');
			$("#subSubUnit").prop("disabled", false);
			$("#subSubUnitTypeID").addClass('mark-readonly');
			$.fn.ChangeSubSubUnitDropdown();
			$.fn.CheckCurrency();
			if ($("#quoteStatus").val() == "1" || $("#quoteStatus").val() == "2") {
				console.log("no quote")
				$(".requestPriceQuote").prop("disabled", true);
            }
		}
		//else {
		//	$.fn.DisableSubSubUnits();
		//}
		//$.fn.CalculateSubUnitAmounts();
		//$.fn.CalculateSubSubUnitAmounts();
	}

	$.fn.CheckCurrency = function () {
		console.log('check currency')
		var currencyType = $("#currency").val();
		var shekelSelector = "#cost";
		var dollarSelector = "#sum-dollars";
		if ($('#masterSectionType').val() == "Operations") {
			shekelSelector = ".shekel-cost";
			dollarSelector = ".dollar-cost";
		}
		var isRequestQuote = $(".isRequest").is(":checked")
		switch (currencyType) {
			case "USD":
				$(shekelSelector).prop("readonly", true);
				$(shekelSelector).addClass('disabled-text');
				$(dollarSelector).prop("disabled", false);
				$(dollarSelector).removeClass("disabled");
				$(dollarSelector).removeClass('disabled-text');
				$(dollarSelector).addClass('requestPriceQuote');
				$(shekelSelector).removeClass('requestPriceQuote');


				$("#unit-price-dollars").prop("disabled", false);
				$("#unit-price-dollars").removeClass('disabled-text');
				$("#unit-price-dollars").prop("readonly", false);
				$(".request-cost-dollar-icon").removeClass('disabled-text');

				$("#unit-price-shekel").prop("disabled", true);
				$("#unit-price-shekel").addClass('disabled-text');
				$("#unit-price-shekel").prop("readonly", true);
				$(".request-cost-shekel-icon").addClass('disabled-text');
				break;
			case "NIS":
			case undefined: //for the reorder modal
				$(shekelSelector).prop("readonly", false);
				$(shekelSelector).removeClass('disabled-text');
				$(dollarSelector).prop("disabled", true);
				$(dollarSelector).addClass('disabled-text');
				$(shekelSelector).addClass('requestPriceQuote');
				$(dollarSelector).removeClass('requestPriceQuote');


				$("#unit-price-dollars").prop("disabled", true);
				$("#unit-price-dollars").addClass('disabled-text');
				$("#unit-price-dollars").prop("readonly", true);
				$(".request-cost-dollar-icon").addClass('disabled-text');

				$("#unit-price-shekel").prop("disabled", false);
				$("#unit-price-shekel").removeClass('disabled-text');
				$("#unit-price-shekel").prop("readonly", false);
				$(".request-cost-shekel-icon").removeClass('disabled-text');

				break;
		}
		if (isRequestQuote) {
			$(".requestPriceQuote ").attr("disabled", true);
        }
	};




	$("#unit").change(function () {
		if ($("#currency").val() == "USD") {
			$.fn.CalculatePriceDollars()
		}
		else
		{
			$.fn.CalculatePriceShekels()
        }
		console.log('in unit change');
	//	$.fn.CalculateUnitAmounts();
	//	$.fn.CalculateSubUnitAmounts();
	//	$.fn.CalculateSubSubUnitAmounts();
	});

	$(".modal").on("change", "#unitTypeID", function () {
		//$.fn.CheckUnitsFilled();
		$.fn.ChangeSubUnitDropdown();
		$.fn.ChangeSubSubUnitDropdown();
	});
	$(".modal").on("change", "#subUnitTypeID", function () {
		$.fn.ChangeSubSubUnitDropdown();
	});

	$("#unit-type-select").on("change", function () {
		alert('in id function');
		$.fn.ChangeSubUnitDropdown();
		$.fn.ChangeSubSubUnitDropdown();
	});

	$("#subUnit").change(function () {
		//console.log("about to check subunitsfilled");
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	});
	$("#unitTypeID").change(function () {
		console.log("about to check unitsfilled");
		$(".addSubUnit").prop('disabled', false);
		$.fn.ChangeSubUnitDropdown();
		$.fn.ChangeSubSubUnitDropdown();
	});
	$("body, .modal").on("change", "#subUnitTypeID",(function () {
		console.log("about to check subunitsfilled");
		$(".addSubSubUnit").prop('disabled', false);
	}));
	$("#select-options-subUnitTypeID").change(function () {
		//console.log("about to check subunitsfilled select");
		//$.fn.CheckSubUnitsFilled();
	});

	$("#subSubUnit").change(function () {
		//	console.log("about to check subunitsfilled");
		$.fn.CalculateSubSubUnitAmounts();
	});

	$("#subSubUnitTypeID").change(function () {
		//	console.log("about to check subunitsfilled");
		//	$.fn.CheckSubUnitsFilled();
	});
	$("#select-options-subSubUnitTypeID").change(function () {
		//console.log("about to check subunitsfilled");
		//	$.fn.CheckSubUnitsFilled();
	});

	//PRICE PAGE ON MODAL VIEW//
	$("#price-tab").click(function () {
		//$.fn.CheckUnitsFilled();
		//$.fn.CheckSubUnitsFilled();
		//I don't think that we need $.fn.CheckSubSubUnitsFilled over here b/c we don't need to enable or disable anything and the CalculateSubSubUnits should already run
		$.fn.CalculateSumPlusVat();
		//$.fn.CheckCurrency();
	});

	$("#currency").change(function (e) {
		$.fn.CheckCurrency();
	});
	$(".modal").on("change", "#currency", function (e) {
		$.fn.CheckCurrency();
	});
	$("#exchangeRate").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	});

	$("body").off("change", "#cost, .cost").on("change", "#cost, .cost",function (e) {
		console.log("change cost")
		var index = 0;
		if ($('#masterSectionType').val() == "Operations") {
			index = $(this).attr('data-val');
		}
		$.fn.CalculateSumPlusVat(index);
		if ($('#masterSectionType').val() != "Operations") {
			$.fn.CalculateUnitAmounts();
			$.fn.CalculateSubUnitAmounts();
			$.fn.CalculateSubSubUnitAmounts();
		}
		//$.fn.CheckCurrency(); //for the reorder modal
	});

	$("#sum-dollars").change(function (e) {
		//alert("in change sum")
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		//$.fn.updateDebt();
	});



	$('.addSubUnit').click(function () {
		$.fn.CheckUnitsFilled();
		$.fn.EnableMaterialSelect('#subUnitTypeID', 'select-options-subUnitTypeID');
	})

	$('.addSubSubUnit').click(function () {
		$.fn.CheckSubUnitsFilled();
		$.fn.EnableMaterialSelect('#subSubUnitTypeID', 'select-options-subSubUnitTypeID');
	})


	$('.sub-close').click(function () {
		$.fn.DisableSubUnits();
		$.fn.DisableSubSubUnits();
		$('.subUnitsCard').addClass('d-none');
		$('.sub-close').addClass('d-none');
		$('.addSubUnitCard').removeClass('d-none');
		$('.addSubSubUnitCard').removeClass('d-none');
		$('.RequestSubsubunitCard').addClass('d-none');
		$('.subSubUnitsCard').addClass('d-none');
		$('.subsub-close').addClass('d-none');
		$("#subUnit").removeClass('mark-readonly');
		$("#subSubUnit").removeClass('mark-readonly');
		$("#subUnitTypeID").removeClass('mark-readonly');
		$("#subSubUnitTypeID").removeClass('mark-readonly');

	})

	$('.subsub-close').click(function () {
		$.fn.DisableSubSubUnits();
		$('.subSubUnitsCard').addClass('d-none');
		$('.subsub-close').addClass('d-none');
		$('.addSubSubUnitCard').removeClass('d-none');
		$("#subSubUnit").removeClass('mark-readonly');
		$("#subSubUnitTypeID").removeClass('mark-readonly');
	})

	$("#unit-price-dollars").change(function () {
		$.fn.CalculatePriceDollars()
	})
	$("#unit-price-shekel").change(function () {
		$.fn.CalculatePriceShekels();
	})
	$('.unit-type-select').change(function () {
		$.fn.UpdatePricePerUnitLabel('.price-per-unit-label', $('#select-options-unitTypeID li.active.selected span').text());
	})
	$('body').on('change', '.subunit-type-select', function (e) {
		//alert('got here')
		$.fn.UpdatePricePerUnitLabel('.price-per-subunit-label', $('#select-options-subUnitTypeID li.active.selected span').text())
	})
	$('body').on('change', '.sub-subunit-type-select', function (e) {
		//alert('got here')
		$.fn.UpdatePricePerUnitLabel('.price-per-sub-subunit-label', $('#select-options-subSubUnitTypeID li.active.selected span').text())
	})
	$.fn.UpdatePricePerUnitLabel = function (className, unitName) {
		if (unitName != "") {
			var newLabel = 'Price Per ' + unitName;
			console.log(newLabel);
			$(className).html(newLabel);
		}
    }
});

