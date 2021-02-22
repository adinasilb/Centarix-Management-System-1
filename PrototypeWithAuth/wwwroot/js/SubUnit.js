
$(function () {

	$.fn.ShowResults = function ($inputBox, $value) { //this function ensures that the value passed back won't be NaN or undefined --> it'll instead send back a blank
		var theResult = parseFloat($value);
		theResult = theResult.toFixed(2);
		theResult = isFinite(theResult) && theResult || "";
		$inputBox.val(theResult);
	}

	$.fn.CalculateSumPlusVat = function () {
		var $exchangeRate = $("#exchangeRate").val();
		if ($exchangeRate == "0") {
			$exchangeRate = "1";
        }
		//console.log("sumShek: " + sumShek);

		//console.log("VatPercentage: " + VatPercentage);
		//console.log("vatCalc: " + vatCalc);
		//$("#Request_VAT").val(vatCalc)
		//var vatInShekel = $("#Request_VAT").val();
		if ($("#sum-dollars").prop("disabled")) {
			$sumDollars = parseFloat($("#cost").val()) / $exchangeRate;
			$iptBox = $('input[name="sum-dollars"]');
			$.fn.ShowResults($iptBox, $sumDollars);
		}
		else if ($("#cost").prop("readonly")) {
			$sumShekel = $("#sum-dollars").val() * $exchangeRate;
			$iptBox = $("#cost");
			$.fn.ShowResults($iptBox, $sumShekel);
		}
		$sumShekel = parseFloat($("#cost").val());
		var vatCalc = $sumShekel * .17;
		//$vatOnshekel = $sumShekel * parseFloat(vatCalc);
		$('#vat').val(vatCalc.toFixed(2));
		$('.vatInDollars').val((vatCalc / $exchangeRate).toFixed(2));
		$sumTotalVatShekel = $sumShekel + vatCalc;
		$iptBox = $("input[name='sumPlusVat-Shekel']");
		$.fn.ShowResults($iptBox, $sumTotalVatShekel);
		$sumTotalVatDollars = $sumTotalVatShekel / $exchangeRate;
		$iptBox = $("input[name='sumPlusVat-Dollar']");
		$.fn.ShowResults($iptBox, $sumTotalVatDollars);
	};
	$.fn.CalculateUnitAmounts = function () {
		$unitSumShekel = parseFloat($("#cost").val()) / $("#unit").val();
		$iptBox = $("input[name='unit-price-shekel']");
		$.fn.ShowResults($iptBox, $unitSumShekel);
		var $exchangeRate = $("#exchangeRate").val();
		$unitSumDollars = $unitSumShekel / $exchangeRate;
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
		var $priceShekels = $unitPrice * $("#unit").val();
		$iptBox = $("#cost");
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
		var $priceDollars = $unitPrice * $("#unit").val();
		var $iptBox = $("input[name='sum-dollars']");
		$.fn.ShowResults($iptBox, $priceDollars);
		var $exchangeRate = $("#exchangeRate").val();
		$priceShekels = $priceDollars * $exchangeRate;
		$iptBox = $("#cost");
		$.fn.ShowResults($iptBox, $priceShekels);
		$unitPriceShekels = $unitPrice * $exchangeRate;
		$iptBox = $("input[name='unit-price-shekel']");
		$.fn.ShowResults($iptBox, $unitPriceShekels)
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		$.fn.CalculateSumPlusVat();
	};
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
		switch (currencyType) {
			case "USD":
				$("#cost").prop("readonly", true);
				$("#cost").addClass('disabled-text');
				$("#sum-dollars").prop("disabled", false);
				$("#sum-dollars").removeClass('disabled-text');


				$("#unit-price-dollars").prop("disabled", false);
				$("#unit-price-dollars").removeClass('disabled-text');
				$("#unit-price-dollars").prop("readonly", false);
				if (!$('.subUnitsCard').hasClass('d-none')) {
					$("#subunit-price-dollars").prop("disabled", false);
					$("#subunit-price-dollars").removeClass('disabled-text');
					$("#subunit-price-dollars").prop("readonly", false);
				}
				if (!$('.subSubUnitsCard').hasClass('d-none')) {
					$("#subsubunit-price-dollars").prop("disabled", false);
					$("#subsubunit-price-dollars").removeClass('disabled-text');
					$("#subsubunit-price-dollars").prop("readonly", false);
				}
				$(".request-cost-dollar-icon").removeClass('disabled-text');

				$("#unit-price-shekel").prop("disabled", true);
				$("#unit-price-shekel").addClass('disabled-text');
				$("#unit-price-shekel").prop("readonly", true);
				if (!$('.subUnitsCard').hasClass('d-none')) {
					$("#subunit-price-shekel").prop("disabled", true);
					$("#subunit-price-shekel").addClass('disabled-text');
					$("#subunit-price-shekel").prop("readonly", true);
				}
				if (!$('.subSubUnitsCard').hasClass('d-none')) {
					$("#subsubunit-price-shekel").prop("disabled", true);
					$("#subsubunit-price-shekel").addClass('disabled-text');
					$("#subsubunit-price-shekel").prop("readonly", true);
				}
				$(".request-cost-shekel-icon").addClass('disabled-text');
				break;
			case "NIS":
			case undefined: //for the reorder modal
				$("#cost").prop("readonly", false);
				$("#cost").removeClass('disabled-text');
				$("#sum-dollars").prop("disabled", true);
				$("#sum-dollars").addClass('disabled-text');


				$("#unit-price-dollars").prop("disabled", true);
				$("#unit-price-dollars").addClass('disabled-text');
				$("#unit-price-dollars").prop("readonly", true);
				if (!$('.subUnitsCard').hasClass('d-none')) {
					$("#subunit-price-dollars").prop("disabled", true);
					$("#subunit-price-dollars").addClass('disabled-text');
					$("#subunit-price-dollars").prop("readonly", true);
				}
				if (!$('.subSubUnitsCard').hasClass('d-none')) {
					$("#subsubunit-price-dollars").prop("disabled", true);
					$("#subsubunit-price-dollars").addClass('disabled-text');
					$("#subsubunit-price-dollars").prop("readonly", true);
				}
				$(".request-cost-dollar-icon").addClass('disabled-text');

				$("#unit-price-shekel").prop("disabled", false);
				$("#unit-price-shekel").removeClass('disabled-text');
				$("#unit-price-shekel").prop("readonly", false);
				if (!$('.subUnitsCard').hasClass('d-none')) {
					$("#subunit-price-shekel").prop("disabled", false);
					$("#subunit-price-shekel").removeClass('disabled-text');
					$("#subunit-price-shekel").prop("readonly", false);
				}
				if (!$('.subSubUnitsCard').hasClass('d-none')) {
					$("#subsubunit-price-shekel").prop("disabled", false);
					$("#subsubunit-price-shekel").removeClass('disabled-text');
					$("#subsubunit-price-shekel").prop("readonly", false);
				}
				$(".request-cost-shekel-icon").removeClass('disabled-text');

				break;
		}
	};




	$("#unit").change(function () {
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
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

	$("#cost").change(function (e) {
		$.fn.CalculateSumPlusVat();
		$.fn.CalculateUnitAmounts();
		$.fn.CalculateSubUnitAmounts();
		$.fn.CalculateSubSubUnitAmounts();
		//$.fn.CheckCurrency(); //for the reorder modal
	});

	$("#sum-dollars").change(function (e) {
		alert("in change sum")
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
});

