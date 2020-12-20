
$(function () {

	$.fn.ShowResults = function ($inputBox, $value) { //this function ensures that the value passed back won't be NaN or undefined --> it'll instead send back a blank
		var theResult = parseFloat($value);
		theResult = theResult.toFixed(2);
		theResult = isFinite(theResult) && theResult || "";
		$inputBox.val(theResult);
	}

	$.fn.CalculateSumPlusVat = function () {
		var $exchangeRate = $("#Request_ExchangeRate").val();
		var sumShek = $("#Request_Cost").val();
		//console.log("sumShek: " + sumShek);
		var vatCalc = sumShek * .17;
		//console.log("VatPercentage: " + VatPercentage);
		//console.log("vatCalc: " + vatCalc);
		//$("#Request_VAT").val(vatCalc)
		//var vatInShekel = $("#Request_VAT").val();
		if ($("#sum-dollars").prop("disabled")) {
			$sumDollars = parseFloat($("#Request_Cost").val()) / $exchangeRate;
			$iptBox = $('input[name="sum-dollars"]');
			$.fn.ShowResults($iptBox, $sumDollars);
		}
		else if ($("#Request_Cost").prop("readonly")) {
			$sumShekel = $("#sum-dollars").val() * $exchangeRate;
			$iptBox = $("input[name='Request.Cost']");
			$.fn.ShowResults($iptBox, $sumShekel);
		}
		$sumShekel = parseFloat($("#Request_Cost").val());
		//$vatOnshekel = $sumShekel * parseFloat(vatCalc);
		$('#Request_VAT').val(vatCalc.toFixed(2));
		$('.vatInDollars').val((vatCalc / $exchangeRate).toFixed(2));
		$sumTotalVatShekel = $sumShekel + vatCalc;
		$iptBox = $("input[name='sumPlusVat-Shekel']");
		$.fn.ShowResults($iptBox, $sumTotalVatShekel);
		$sumTotalVatDollars = $sumTotalVatShekel / $exchangeRate;
		$iptBox = $("input[name='sumPlusVat-Dollar']");
		$.fn.ShowResults($iptBox, $sumTotalVatDollars);
	};
	$.fn.CalculateUnitAmounts = function () {
		$unitSumShekel = parseFloat($("#Request_Cost").val()) / $("#Request_Unit").val();
		$iptBox = $("input[name='unit-price-shekel']");
		$.fn.ShowResults($iptBox, $unitSumShekel);
		var $exchangeRate = $("#Request_ExchangeRate").val();
		$unitSumDollars = $unitSumShekel / $exchangeRate;
		$iptBox = $("input[name='unit-price-dollars']");
		$.fn.ShowResults($iptBox, $unitSumDollars);
	};

	$.fn.CalculateSubUnitAmounts = function () {
		$subUnitSumShekel = $("#unit-price-shekel").val() / $("#Request_SubUnit").val();
		$iptBox = $("input[name='subunit-price-shekel']");
		$.fn.ShowResults($iptBox, $subUnitSumShekel);
		var $exchangeRate = $("#Request_ExchangeRate").val();
		$subUnitSumDollars = $subUnitSumShekel / $exchangeRate;
		$iptBox = $("input[name='subunit-price-dollars']");
		$.fn.ShowResults($iptBox, $subUnitSumDollars);
	};

	$.fn.CalculateSubSubUnitAmounts = function () {
		$subSubUnitSumShekel = $("#subunit-price-shekel").val() / $("#Request_SubSubUnit").val();
		$iptBox = $("input[name='subsubunit-price-shekel']");
		$.fn.ShowResults($iptBox, $subSubUnitSumShekel);
		var $exchangeRate = $("#Request_ExchangeRate").val();
		$subSubUnitSumDollars = $subSubUnitSumShekel / $exchangeRate;
		$iptBox = $("input[name='subsubunit-price-dollars']");
		$.fn.ShowResults($iptBox, $subSubUnitSumDollars);
	};
	$.fn.CalculatePriceShekels = function () {
		var $exchangeRate = $("#Request_ExchangeRate").val();
		var $unitPrice = $("#unit-price-shekel").val();
		var $priceShekels = $unitPrice * $("#Request_Unit").val();
		$iptBox = $("input[name='Request.Cost']");
		$.fn.ShowResults($iptBox, $priceShekels);		
		var $priceDollars = $priceShekels / $exchangeRate;
		var $iptBox = $("input[name='sum-dollars']");
		$.fn.ShowResults($iptBox, $priceDollars);
		$unitPriceDollars = $unitPrice / $exchangeRate;
		$iptBox = $("input[name='unit-price-dollars']");
		$.fn.ShowResults($iptBox, $unitPriceDollars)
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		$.fn.CalculateSumPlusVat();
	};
	$.fn.CalculatePriceDollars = function () {
		var $unitPrice = $("#unit-price-dollars").val();
		var $priceDollars = $unitPrice * $("#Request_Unit").val();
		var $iptBox = $("input[name='sum-dollars']");
		$.fn.ShowResults($iptBox, $priceDollars);
		var $exchangeRate = $("#Request_ExchangeRate").val();
		$priceShekels = $priceDollars * $exchangeRate;
		$iptBox = $("input[name='Request.Cost']");
		$.fn.ShowResults($iptBox, $priceShekels);
		$unitPriceShekels = $unitPrice * $exchangeRate;
		$iptBox = $("input[name='unit-price-shekel']");
		$.fn.ShowResults($iptBox, $unitPriceShekels)
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		$.fn.CalculateSumPlusVat();
	};
	$.fn.EnableSubUnits = function () {
		$("#Request_SubUnit").prop("disabled", false);
		$.fn.EnableMaterialSelect('#Request_SubUnitTypeID', 'select-options-Request_SubUnitTypeID');
	};
	$.fn.ChangeSubUnitDropdown = function () {
		console.log("change subunit dropdown");
		var selected = $(':selected', $("#Request_UnitTypeID"));
		var selected2 = $(':selected', $("#select-options-Request_UnitTypeID"));
		//console.log("u selected: " + selected);
		var optgroup = selected.closest('optgroup').attr('label');
		var optgroup2 = selected2.closest('optgroup').attr('label');
		console.log("u optgroup: " + optgroup);
		console.log("u optgroup2: " + optgroup2);
		//the following is based on the fact that the unit types and parents are seeded with primary key values
		var selectedIndex = $('#select-options-Request_SubUnitTypeID').find(".active").index();
		console.log("select index" + selectedIndex)
		var subOptgroup = $(':selected', $("#Request_SubUnitTypeID")).closest('optgroup').attr('label');
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

		$('#Request_SubUnitTypeID').destroyMaterialSelect();
		$('#Request_SubUnitTypeID').prop('selectedIndex', selectedIndex);
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
				$("#Request_SubUnitTypeID optgroup[label='Units'] option").prop('disabled', false);
				$("#Request_SubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);


				break;
			case "Weight/Volume":
				//alert("inside optgroup weight/volume TESTING");
				//$(".subunit-subunit").hide();
				//$("#select-options-Request_SubUnitTypeID option").prop('hidden', true);
				//$("#Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").prop('disabled', true).prop('hidden', true);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").prop('disabled', false).prop('hidden', false);
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Units'] li").hide();
				//$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume'] li").show();
				$("#Request_SubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$("#Request_SubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);

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
				$("#Request_SubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$('#select-options-Request_SubUnitTypeID li.optgroup:nth-child(3)').addClass('.active');
				$("#Request_SubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', true);

				break;
		}
		$("#Request_SubUnitTypeID").materialSelect();
		switch (optgroup2) {
			case "Units":
				console.log("inside optgroup2 units");
				$("#select-options-Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Weight/Volume":
				console.log("inside optgroup2 weight/volume");
				$("#select-options-Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Test":
				console.log("inside optgroup2 test");
				$("#select-options-Request_SubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				break;
		}
	};
	//change sub sub unit dropdown
	$.fn.ChangeSubSubUnitDropdown = function () {
		console.log("in change subsubunitdropdown");
		var selected = $(':selected', $("#Request_SubUnitTypeID"));
		var selected2 = $(':selected', $("#select-options-Request_SubUnitTypeID"));
		var optgroup = selected.closest('optgroup').attr('label');
		var optgroup2 = selected.closest('optgroup').attr('label');
		var selectedIndex = $('#select-options-Request_SubSubUnitTypeID').find(".active").index();
		console.log("select index" + selectedIndex)
		var subOptgroup = $(':selected', $("#Request_SubSubUnitTypeID")).closest('optgroup').attr('label');
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

		$('#Request_SubSubUnitTypeID').destroyMaterialSelect();
		$('#Request_SubSubUnitTypeID').prop('selectedIndex', selectedIndex);
		switch (optgroup) {
			case "Units":
				//$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				//$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				$("#Request_SubSubUnitTypeID optgroup[label='Units'] option").prop('disabled', false);
				$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);
				break;
			case "Weight/Volume":
				//$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				$("#Request_SubSubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);
				break;
			case "Test":
				//$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				//$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				$("#Request_SubSubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
				$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', true);
				break;
		}
		$("#Request_SubSubUnitTypeID").materialSelect();
		switch (optgroup2) {
			case "Units":
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Weight/Volume":
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
				break;
			case "Test":
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
				$("#select-options-Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
				break;
		}
	};


	$.fn.EnableSubSubUnits = function () {
		$("#Request_SubSubUnit").prop("disabled", false);
		$.fn.EnableMaterialSelect('#Request_SubSubUnitTypeID', 'select-options-Request_SubSubUnitTypeID');
	};
	$.fn.DisableSubUnits = function () {
		$("#Request_SubUnit").prop("disabled", true);
		$("#Request_SubUnitTypeID").destroyMaterialSelect();
		$("#Request_SubUnitTypeID").prop("disabled", true);
		$("#Request_SubUnitTypeID").materialSelect();

		//$("#select-options-Request_SubUnitTypeID").prop("disabled", true);
		//$("#select-options-Request_SubUnitTypeID").attr("aria-disabled", true);
		//disable validation
		//$('#Request_SubUnitTypeID').rules("remove", "selectRequired");
	};

	$.fn.DisableSubSubUnits = function () {
		$("#Request_SubSubUnit").prop("disabled", true);
		$("#Request_SubSubUnitTypeID").destroyMaterialSelect();
		$("#Request_SubSubUnitTypeID").prop("disabled", true);
		$("#Request_SubSubUnitTypeID").materialSelect();
	};
	$.fn.CheckUnitsFilled = function () {
		console.log("in check units function");
		if (($("#edit #Request_Unit").val() > 0 && $("#edit #Request_UnitTypeID").val())
			|| ($("#select-options-Request_Unit").val() > 0 && $("#select-options-Request_UnitTypeID").val())) {
			//console.log("both have values");
			$.fn.EnableSubUnits();
			$('.subUnitsCard').removeClass('d-none');
			$('.sub-close').removeClass('d-none');
			$('.addSubUnitCard').addClass('d-none');
			$('.RequestSubsubunitCard').removeClass('d-none');
			$("#Request_SubUnit").addClass('mark-readonly');
			$("#Request_SubUnitTypeID").addClass('mark-readonly');
			$.fn.ChangeSubUnitDropdown();
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
		if (($("#Request_SubUnit").val() > 0 && $("#Request_SubUnitTypeID").val())
			|| ($("#Request_SubUnit").val() > 0 && $("#select-options-Request_SubUnitTypeID").val())) {
			$.fn.EnableSubSubUnits();
			$('.subSubUnitsCard').removeClass('d-none');
			$('.subsub-close').removeClass('d-none');
			$('.addSubSubUnitCard').addClass('d-none');
			//console.log("about to change subsubunitdropdown");
			$("#Request_SubSubUnit").addClass('mark-readonly');
			$("#Request_SubSubUnitTypeID").addClass('mark-readonly');
			$.fn.ChangeSubSubUnitDropdown();
		}
		//else {
		//	$.fn.DisableSubSubUnits();
		//}
		//$.fn.CalculateSubUnitAmounts();
		//$.fn.CalculateSubSubUnitAmounts();
	}

	$.fn.CheckCurrency = function () {
		var currencyType = $("#currency").val();
		switch (currencyType) {
			case "dollar":
				$("#Request_Cost").prop("readonly", true);
				$("#sum-dollars").prop("disabled", false);

				$("#unit-price-dollars").prop("disabled", false);
				$("#unit-price-shekel").prop("disabled", true);
				break;
			case "shekel":
				$("#Request_Cost").prop("readonly", false);
				$("#sum-dollars").prop("disabled", true);

				$("#unit-price-dollars").prop("disabled", true);
				$("#unit-price-shekel").prop("disabled", false);
				break;
		}
	};




	$("#Request_Unit").change(function () {
		//alert("request unit changed");
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	});

	//$(".modal").on("click", "#Request_UnitTypeID", function () {
	//	alert("modal Request_UnitTypeID was clicked");
	//	$.fn.CheckUnitsFilled();


	//});

	$(".modal").on("change", "#Request_UnitTypeID", function () {
		//	alert("modal Request_UnitTypeID was changed");
		//$.fn.CheckUnitsFilled();
		$.fn.ChangeSubUnitDropdown();
		$.fn.ChangeSubSubUnitDropdown();
	});
	$(".modal").on("change", "#Request_SubUnitTypeID", function () {
		//	alert("modal Request_SubUnitTypeID was changed");
		$.fn.ChangeSubSubUnitDropdown();
	});

	$("#unit-type-select").on("change", function () {
		//console.log("unit type id changed");
		//alert("select options change was selected");
		//$.fn.CheckUnitsFilled();
		$.fn.ChangeSubUnitDropdown();
		$.fn.ChangeSubSubUnitDropdown();
	});

	$("#Request_SubUnit").change(function () {
		//console.log("about to check subunitsfilled");
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	});
	$("#Request_UnitTypeID").change(function () {
		$.fn.ChangeSubUnitDropdown();
		$.fn.ChangeSubSubUnitDropdown();
	});
	$("#Request_SubUnitTypeID").change(function () {
		//	console.log("about to check subunitsfilled");
		//$.fn.CheckSubUnitsFilled();
	});
	$("#select-options-Request_SubUnitTypeID").change(function () {
		//	console.log("about to check subunitsfilled");
		//$.fn.CheckSubUnitsFilled();
	});

	$("#Request_SubSubUnit").change(function () {
		//	console.log("about to check subunitsfilled");
		$.fn.CalculateSubSubUnitAmounts();
	});

	$("#Request_SubSubUnitTypeID").change(function () {
		//	console.log("about to check subunitsfilled");
		//	$.fn.CheckSubUnitsFilled();
	});
	$("#select-options-Request_SubSubUnitTypeID").change(function () {
		//console.log("about to check subunitsfilled");
		//	$.fn.CheckSubUnitsFilled();
	});

	//PRICE PAGE ON MODAL VIEW//
	$("#price-tab").click(function () {
		//$.fn.CheckUnitsFilled();
		//$.fn.CheckSubUnitsFilled();
		//I don't think that we need $.fn.CheckSubSubUnitsFilled over here b/c we don't need to enable or disable anything and the CalculateSubSubUnits should already run
		$.fn.CalculateSumPlusVat();
		$.fn.CheckCurrency();
	});

	$("#currency").change(function (e) {
		alert("Currency changed!");
		$.fn.CheckCurrency();
	});
	$(".modal").on("change","#currency", function (e) {
		alert("Currency changed!");
		$.fn.CheckCurrency();
	});
	$("#Request_ExchangeRate").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
	});

	$("#Request_Cost").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();

	});

	$("#sum-dollars").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		$.fn.updateDebt();
	});



	$('.addSubUnit').click(function () {
		$.fn.CheckUnitsFilled();
	})

	$('.addSubSubUnit').click(function () {
		$.fn.CheckSubUnitsFilled();
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
		$("#Request_SubUnit").removeClass('mark-readonly');
		$("#Request_SubSubUnit").removeClass('mark-readonly');
		$("#Request_SubUnitTypeID").removeClass('mark-readonly');
		$("#Request_SubSubUnitTypeID").removeClass('mark-readonly');

	})

	$('.subsub-close').click(function () {
		$.fn.DisableSubSubUnits();
		$('.subSubUnitsCard').addClass('d-none');
		$('.subsub-close').addClass('d-none');
		$('.addSubSubUnitCard').removeClass('d-none');
		$("#Request_SubSubUnit").removeClass('mark-readonly');
		$("#Request_SubSubUnitTypeID").removeClass('mark-readonly');
	})

	$("#unit-price-dollars").change(function () {
		$.fn.CalculatePriceDollars()
	})
	$("#unit-price-shekel").change(function () {
		$.fn.CalculatePriceShekels();
	})
});

