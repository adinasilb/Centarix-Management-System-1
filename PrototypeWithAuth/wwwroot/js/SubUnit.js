
$(function () {
    $.fn.ShowResults = function ($inputBox, $value) { //this function ensures that the value passed back won't be NaN or undefined --> it'll instead send back a blank
        var theResult = parseFloat($value);
        theResult = theResult.toFixed(2);
        theResult = (isFinite(theResult) && theResult != 0) ? theResult : "";
        $inputBox.val(theResult);
    }

    $.fn.CalculateSumPlusVat = function (modalClass) {
        var $exchangeRate = $("." + modalClass +" #exchangeRate").val();
        var inverseExchangeRate = 1 / $exchangeRate
        if ($exchangeRate == "0") {
            $exchangeRate = "1";
        }
        var dollarId = "sum-dollars";
        var shekelId = "cost"
        var vatId = "vat";
        var vatDollarId = "vatInDollars";
        var totalVatId = "sumPlusVat-Shekel";
        var totalVatDollarId = "sumPlusVat-Dollar";
        //console.log("sumShek: " + sumShek);

        //console.log("VatPercentage: " + VatPercentage);
        //console.log("vatCalc: " + vatCalc);
        //$("#Request_VAT").val(vatCalc)
        //var vatInShekel = $("#Request_VAT").val();
        if ($("." + modalClass + " #" + dollarId).prop("disabled") || $("." + modalClass +" #" + dollarId).hasClass("disabled")) {
            $sumDollars = parseFloat($("." + modalClass +" #" + shekelId).val()) * inverseExchangeRate;
            console.log("sumDollars" + $sumDollars)
            $iptBox = $("." + modalClass +' #' + dollarId);
            $.fn.ShowResults($iptBox, $sumDollars);
        }
        else if ($("." + modalClass +" #" + shekelId).prop("readonly")) {
            $sumShekel = $("." + modalClass +" #" + dollarId).val() * $exchangeRate;
            console.log("shekel " + $sumShekel)
            $iptBox = $("." + modalClass +" #" + shekelId);
            $.fn.ShowResults($iptBox, $sumShekel);
        }
        $sumShekel = parseFloat($("." + modalClass +" #" + shekelId).val());
        //console.log("sum shekel " + $sumShekel)
        //if ($sumShekel == "NaN") {
        //	$sumShekel = 0;
        //      }
        var vatCalc = $sumShekel * .17;
        //$vatOnshekel = $sumShekel * parseFloat(vatCalc);
        $.fn.ShowResults($("." + modalClass +' #' + vatId), vatCalc.toFixed(2));
        //$('#' + vatDollarId).val((vatCalc * inverseExchangeRate).toFixed(2));
        $.fn.ShowResults($("." + modalClass +' #' + vatDollarId), (vatCalc * inverseExchangeRate).toFixed(2));
        $sumTotalVatShekel = $sumShekel + vatCalc;
        $iptBox = $("." + modalClass +" #" + totalVatId);
        $.fn.ShowResults($iptBox, $sumTotalVatShekel);
        $sumTotalVatDollars = $sumTotalVatShekel * inverseExchangeRate;
        $iptBox = $("." + modalClass +" #" + totalVatDollarId);
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

    $.fn.CalculateSubUnitAmounts = function (modalClass) {
        $subUnitSumShekel = $("." + modalClass + " #unit-price-shekel").val() / $("." + modalClass +" #subUnit").val();
        $iptBox = $("." + modalClass +" input[name='subunit-price-shekel']");
        $.fn.ShowResults($iptBox, $subUnitSumShekel);
        var $exchangeRate = $("." + modalClass +" #exchangeRate").val();
        $subUnitSumDollars = $subUnitSumShekel / $exchangeRate;
        $iptBox = $("." + modalClass +" input[name='subunit-price-dollars']");
        $.fn.ShowResults($iptBox, $subUnitSumDollars);

        //for the reorder modal
        $subunit = $("." + modalClass +" #subUnit");
    
    };

    $.fn.CalculateSubSubUnitAmounts = function (modalClass) {
        $subSubUnitSumShekel = $("." + modalClass + " #subunit-price-shekel").val() / $("." + modalClass +" #subSubUnit").val();
        console.log("subunitpriceshekel: " + $("." + modalClass +" #subunit-price-shekel").val());
        console.log("subsubunitval: " + $("." + modalClass +" #subSubUnit").val());
        console.log("$subSubUnitSumShekel: " + $subSubUnitSumShekel);
        $iptBox = $("." + modalClass +" input[name='subsubunit-price-shekel']");
        $.fn.ShowResults($iptBox, $subSubUnitSumShekel);
        var $exchangeRate = $("." + modalClass +" #exchangeRate").val();
        $subSubUnitSumDollars = $subSubUnitSumShekel / $exchangeRate;
        $iptBox = $("." + modalClass +" input[name='subsubunit-price-dollars']");
        $.fn.ShowResults($iptBox, $subSubUnitSumDollars);
        //for the reorder modal
        $subsubunit = $("." + modalClass +" #subSubUnit");
    };
    $.fn.CalculatePriceShekels = function (modalClass) {
        var $exchangeRate = $("." + modalClass +" #exchangeRate").val();
        var $unitPrice = $("." + modalClass +" #unit-price-shekel").val();
        $.fn.CalculateSum(modalClass);
        $unitPriceDollars = $unitPrice / $exchangeRate;
        console.log("exchange rate" + $exchangeRate)
        $iptBox = $("." + modalClass +" input[name='unit-price-dollars']");
        $.fn.ShowResults($iptBox, $unitPriceDollars)

        $.fn.CalculateSubUnitAmounts(modalClass);
        $.fn.CalculateSubSubUnitAmounts(modalClass);
    };
    $.fn.CalculatePriceDollars = function (modalClass) {
        console.log("calculate dollars")
        var $unitPriceDollars = $("."+modalClass+" #unit-price-dollars").val();
        var $exchangeRate = $("." + modalClass +" #exchangeRate").val();
        $priceShekels = $unitPriceDollars * $exchangeRate;
        $iptBox = $("." + modalClass +" #unit-price-shekel");
        $.fn.ShowResults($iptBox, $priceShekels);
        var $priceDollars = $unitPriceDollars * $("." + modalClass +" #unit").val();
        var $iptBox = $("." + modalClass +" input[name='sum-dollars']");
        $.fn.ShowResults($iptBox, $priceDollars);

        $.fn.CalculateSubUnitAmounts(modalClass);
        $.fn.CalculateSubSubUnitAmounts(modalClass);
        $.fn.CalculateSumPlusVat(modalClass);
    };
    $.fn.CalculateSum = function (modalClass) {
        var $exchangeRate = $("." + modalClass +" #exchangeRate").val();
        var $unitPrice = $("." + modalClass +" #unit-price-shekel").val();
        var $priceShekels = $unitPrice * $("." + modalClass +" #unit").val();
        $iptBox = $("." + modalClass +" #cost");
        $.fn.ShowResults($iptBox, $priceShekels);
        var $priceDollars = $priceShekels / $exchangeRate;
        var $iptBox = $("." + modalClass +" input[name='sum-dollars']");
        $.fn.ShowResults($iptBox, $priceDollars);
        $.fn.CalculateSumPlusVat(modalClass);
    }
   
    $.fn.ChangeSubUnitDropdown = function (modalClass) {
        console.log("change subunit dropdown");
        var selected = $(':selected', $("." + modalClass +" #unitTypeID"));
        var selected2 = $(':selected', $("." + modalClass +" #select-options-unitTypeID"));
        //console.log("u selected: " + selected);
        var optgroup = selected.closest('optgroup').attr('label');
        var optgroup2 = selected2.closest('optgroup').attr('label');
        console.log("u optgroup: " + optgroup);
        console.log("u optgroup2: " + optgroup2);
        //the following is based on the fact that the unit types and parents are seeded with primary key values
        var selectedIndex = $("." + modalClass +' #select-options-subUnitTypeID').find(".active").index();
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

        $("." + modalClass +' #subUnitTypeID').destroyMaterialSelect();
        $("." + modalClass +' #subUnitTypeID').prop('selectedIndex', selectedIndex);
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
                $("." + modalClass +" #subUnitTypeID optgroup[label='Units'] option").prop('disabled', false);
                $("." + modalClass +" #subUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);


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
                $("." + modalClass +" #subUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
                $("." + modalClass +" #subUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);

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
                $("." + modalClass +" #subUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
                $("." + modalClass +' #select-options-subUnitTypeID li.optgroup:nth-child(3)').addClass('.active');
                $("." + modalClass +" #subUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', true);

                break;
        }
        $("." + modalClass +" #subUnitTypeID").materialSelect();
        //$("#subUnit").prop("disabled", false);
        //$.fn.EnableMaterialSelect('#subUnitTypeID', 'select-options-subUnitTypeID');
        switch (optgroup2) {
            case "Units":
                console.log("inside optgroup2 units");
                $("." + modalClass +" #select-options-subUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
                $("." + modalClass +" #select-options-subUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
                break;
            case "Weight/Volume":
                console.log("inside optgroup2 weight/volume");
                $("." + modalClass +" #select-options-subUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
                $("." + modalClass +" #select-options-subUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
                break;
            case "Test":
                console.log("inside optgroup2 test");
                $("." + modalClass +" #select-options-subUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
                $("." + modalClass +" #select-options-subUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
                break;
        }
    };
    //change sub sub unit dropdown
    $.fn.ChangeSubSubUnitDropdown = function (modalClass) {
        console.log("in change subsubunitdropdown");
        var selected = $(':selected', $("#subUnitTypeID"));
        var selected2 = $(':selected', $("." + modalClass +" #select-options-subUnitTypeID"));
        var optgroup = selected.closest('optgroup').attr('label');
        var optgroup2 = selected2.closest('optgroup').attr('label');
        var selectedIndex = $("." + modalClass +' #select-options-subSubUnitTypeID').find(".active").index();
        console.log("select index" + selectedIndex)
        var subOptgroup = $(':selected', $("." + modalClass +" #subSubUnitTypeID")).closest('optgroup').attr('label');
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

        $("." + modalClass +' #subSubUnitTypeID').destroyMaterialSelect();
        $("." + modalClass +' #subSubUnitTypeID').prop('selectedIndex', selectedIndex);
        console.log($("." + modalClass +' #subSubUnitTypeID').prop('selectedIndex'));
        switch (optgroup) {
            case "Units":
                //$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
                //$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
                $("." + modalClass +" #subSubUnitTypeID optgroup[label='Units'] option").prop('disabled', false);
                $("." + modalClass +" #subSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);
                break;
            case "Weight/Volume":
                //$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
                //$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
                $("." + modalClass +" #subSubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
                $("." + modalClass +" #subSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', false);
                break;
            case "Test":
                //$("#Request_SubSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
                //$("#Request_SubSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
                $("." + modalClass +" #subSubUnitTypeID optgroup[label='Units'] option").prop('disabled', true);
                $("." + modalClass +" #subSubUnitTypeID optgroup[label='Weight/Volume'] option").prop('disabled', true);
                break;
        }
        $("." + modalClass +" #subSubUnitTypeID").materialSelect();
        //$.fn.EnableMaterialSelect('#subSubUnitTypeID', 'select-options-subUnitTypeID');
        switch (optgroup2) {
            case "Units":
                $("." + modalClass +" #select-options-subSubUnitTypeID optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
                $("." + modalClass +" #select-options-subSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
                break;
            case "Weight/Volume":
                $("." + modalClass +" #select-options-subSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
                $("." + modalClass +" #select-options-subSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
                break;
            case "Test":
                $("." + modalClass +" #select-options-subSubUnitTypeID optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
                $("." + modalClass +" #select-options-subSubUnitTypeID optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
                break;
        }
    };

    $.fn.DisableSubUnits = function (modalClass) {
        $("." + modalClass +" #subUnit").prop("disabled", true);
        $("." + modalClass +" #subUnitTypeID").destroyMaterialSelect();
        $("." + modalClass +" #subUnitTypeID").prop("disabled", true);
        $("." + modalClass +" #subUnitTypeID").materialSelect();
    };

    $.fn.DisableSubSubUnits = function (modalClass) {
        $("." + modalClass +" #subSubUnit").prop("disabled", true);
        $("." + modalClass +" #subSubUnitTypeID").destroyMaterialSelect();
        $("." + modalClass +" #subSubUnitTypeID").prop("disabled", true);
        $("." + modalClass +" #subSubUnitTypeID").materialSelect();
    };
    $.fn.CheckUnitsFilled = function (modalClass) {
        console.log("in check units function");
        if (($("." + modalClass + " #edit #unit").val() > 0 && $("." + modalClass +" #edit #unitTypeID").val())
            || ($("." + modalClass + " #select-options-unit").val() > 0 && $("." + modalClass +" #select-options-unitTypeID").val())) {
            //console.log("both have values");
            $("." + modalClass +' .subUnitsCard').removeClass('d-none');
            $("." + modalClass +' .sub-close').removeClass('d-none');
            $("." + modalClass +' .addSubUnitCard').addClass('d-none');
            $("." + modalClass +' .RequestSubsubunitCard').removeClass('d-none');
            $("." + modalClass +" #subUnit").addClass('mark-readonly');
            $("." + modalClass +" #subUnit").prop("disabled", false);
            $("." + modalClass +" #subUnitTypeID").addClass('mark-readonly');
            $.fn.ChangeSubUnitDropdown(modalClass);
            $.fn.CheckCurrency(modalClass);
            if ($("." + modalClass + " #quoteStatus").val() == "1" || $("." + modalClass +" #quoteStatus").val() == "2") {
                console.log("no quote")
                $("." + modalClass +" .requestPriceQuote").prop("disabled", true);
            }
        }

    };
    $.fn.CheckSubUnitsFilled = function (modalClass) {
        if (($("." + modalClass + " #subUnit").val() > 0 && $("." + modalClass +" #subUnitTypeID").val())
            || ($("." + modalClass + " #subUnit").val() > 0 && $("." + modalClass +" #select-options-subUnitTypeID").val())) {
            $("." + modalClass +' .subSubUnitsCard').removeClass('d-none');
            $("." + modalClass +' .subsub-close').removeClass('d-none');
            $("." + modalClass +' .addSubSubUnitCard').addClass('d-none');
            //console.log("about to change subsubunitdropdown");
            $("." + modalClass +" #subSubUnit").addClass('mark-readonly');
            $("." + modalClass +" #subSubUnit").prop("disabled", false);
            $("." + modalClass +" #subSubUnitTypeID").addClass('mark-readonly');
            $.fn.ChangeSubSubUnitDropdown(modalClass);
            $.fn.CheckCurrency(modalClass);
            if ($("." + modalClass + " #quoteStatus").val() == "1" || $("." + modalClass +" #quoteStatus").val() == "2") {
                console.log("no quote")
                $("." + modalClass +" .requestPriceQuote").prop("disabled", true);
            }
        }
    }

    $.fn.CheckCurrency = function (modalClass) {
        console.log('check currency')
        var currencyType = $("." + modalClass + " #currency").val();
        var shekelSelector = "." + modalClass +" #cost";
        var dollarSelector = "." + modalClass +" #sum-dollars";
        var isRequestQuote = false; //always false for now $(".isRequest").is(":checked")
        var warning = false;
        var warningMessage = "Warning: Default currency for selected Vendor is ";
        switch (currencyType) {
            case "USD":
                //if ($("#VendorCurrencyID").attr("value") != '1') {
                //	console.log("inside warning");
                //	warning = true;
                //	warningMessage += "Shekel"
                //}
                //else {
                //	warning = false;
                //            }
                $(shekelSelector).prop("readonly", true);
                $(shekelSelector).addClass('disabled-text');
                $(dollarSelector).not(".mark-roles-readonly").prop("disabled", false);
                $(dollarSelector).not(".mark-roles-readonly").removeClass("disabled");
                $(dollarSelector).not(".mark-roles-readonly").removeClass('disabled-text');
                $(dollarSelector).addClass('requestPriceQuote');
                $(shekelSelector).removeClass('requestPriceQuote');


                $("." + modalClass + " #unit-price-dollars").not(".mark-roles-readonly").prop("disabled", false);
                $("." + modalClass + " #unit-price-dollars").not(".mark-roles-readonly").removeClass('disabled-text');
                $("." + modalClass + " #unit-price-dollars").not(".mark-roles-readonly").prop("readonly", false);
                $("." + modalClass + " .request-cost-dollar-icon").not(".mark-roles-readonly").removeClass('disabled-text');

                $("." + modalClass + " #unit-price-shekel").prop("disabled", true);
                $("." + modalClass + " #unit-price-shekel").addClass('disabled-text');
                $("." + modalClass + " #unit-price-shekel").prop("readonly", true);
                $("." + modalClass + " .request-cost-shekel-icon").addClass('disabled-text');
                break;
            case "NIS":
            case undefined: //for the reorder modal
                //if ($("#VendorCurrencyID").attr("value") == '1') {
                //	console.log("inside warning for shekel");
                //	warning = true;
                //	warningMessage += "Dollars"
                //}
                //else {
                //	warning = false;
                //}
                $(shekelSelector).not(".mark-roles-readonly").prop("readonly", false);
                $(shekelSelector).not(".mark-roles-readonly").removeClass('disabled-text');
                $(dollarSelector).not(".mark-roles-readonly").prop("disabled", true);
                $(dollarSelector).not(".mark-roles-readonly").addClass('disabled-text');
                $(shekelSelector).not(".mark-roles-readonly").addClass('requestPriceQuote');
                $(dollarSelector).not(".mark-roles-readonly").removeClass('requestPriceQuote');


                $("." + modalClass + " #unit-price-dollars").not(".mark-roles-readonly").prop("disabled", true);
                $("." + modalClass + " #unit-price-dollars").not(".mark-roles-readonly").addClass('disabled-text');
                $("." + modalClass + " #unit-price-dollars").not(".mark-roles-readonly").prop("readonly", true);
                $("." + modalClass + " .request-cost-dollar-icon").not(".mark-roles-readonly").addClass('disabled-text');

                $("." + modalClass + " #unit-price-shekel").not(".mark-roles-readonly").prop("disabled", false);
                $("." + modalClass + " #unit-price-shekel").not(".mark-roles-readonly").removeClass('disabled-text');
                $("." + modalClass + " #unit-price-shekel").not(".mark-roles-readonly").prop("readonly", false);
                $("." + modalClass + " .request-cost-shekel-icon").not(".mark-roles-readonly").removeClass('disabled-text');

                break;
        }
        if (isRequestQuote) {
            $("." + modalClass +" .requestPriceQuote ").attr("disabled", true);
        }
    };


    $(".reorder-item .unit").change(function () {
        unitChange("reorder-item");
    });

    $(".add-or-edit-item .unit").change(function () {
        unitChange("add-or-edit-item");
    });
    function unitChange(modalClass) {
        console.log("unit change");
        if ($("."+modalClass+ " #currency").val() == "USD") {
            $.fn.CalculatePriceDollars(modalClass);
        }
        else {
            $.fn.CalculatePriceShekels(modalClass);
        }
    }

    $(".add-or-edit-item").on("change", "#unitTypeID", function () {
        unitTypeIDChange("add-or-edit-item");
    });

    $(".reorder-item").on("change", "#unitTypeID", function () {
        unitTypeIDChange("reorder-item");
    });
    function unitTypeIDChange(modalClass) {
        console.log("unitTypeIDChange")
        $("." + modalClass +" .addSubUnit").prop('disabled', false);
        $.fn.ChangeSubUnitDropdown(modalClass);
        $.fn.ChangeSubSubUnitDropdown(modalClass);
    }

    $("body, .modal").on("change", ".add-or-edit-item #subUnitTypeID", function () {
        $.fn.ChangeSubSubUnitDropdown("add-or-edit-item");
    });

    $(".add-or-edit-item #unit-type-select").on("change", function () {
        //alert('in id function');
        $.fn.ChangeSubUnitDropdown("add-or-edit-item");
        $.fn.ChangeSubSubUnitDropdown("add-or-edit-item");
    });

    $("body, .modal").on("change", ".reorder-item #subUnitTypeID", function () {
        $.fn.ChangeSubSubUnitDropdown("reorder-item");
    });

    $(".reorder-item #unit-type-select").on("change", function () {
        //alert('in id function');
        $.fn.ChangeSubUnitDropdown("reorder-item");
        $.fn.ChangeSubSubUnitDropdown("reorder-item");
    });

    $(".reorder-item #subUnit").change(function () {
        //console.log("about to check subunitsfilled");
        subUnitChange("reorder-item");
    });

    $(".add-or-edit-item #subUnit").change(function () {
        //console.log("about to check subunitsfilled");
        subUnitChange("add-or-edit-item");
    });

    function subUnitChange(modalClass) {
        console.log("subUnitChange")
        $.fn.CalculateSubUnitAmounts(modalClass);
        $.fn.CalculateSubSubUnitAmounts(modalClass);
    }

    $("body, .modal").on("change", ".add-or-edit-item #subUnitTypeID", (function () {
        $(".add-or-edit-item .addSubSubUnit").prop('disabled', false);
    }));

    $("body, .modal").on("change", ".reorder-item #subUnitTypeID", (function () {
        $(".reorder-item .addSubSubUnit").prop('disabled', false);
    }));


    $(".add-or-edit-item #subSubUnit").change(function () {
        $.fn.CalculateSubSubUnitAmounts("add-or-edit-item");
    });
    $(".reorder-item #subSubUnit").change(function () {
        $.fn.CalculateSubSubUnitAmounts("reorder-item");
    });

    //PRICE PAGE ON MODAL VIEW//
    $("#price-tab").click(function () {     
        $.fn.CalculateSumPlusVat("add-or-edit-item");
    });

    $("#currency").change(function (e) {
        $.fn.CheckCurrency("add-or-edit-item");
        if ($("#price").hasClass("active")) {
            $(this).attr("changed", "true");
            console.log("going to vendor currency warnings");
            $.fn.CheckForVendorCurrencyWarning($("#VendorCurrencyID").val(), $("#currency").val());
        }
    });

    $(".showUnitAmountWarning").change(function (e) {
        $("#loading").show();
        $.ajax({
            async: true,
            url: "/Requests/UnitWarningModal?SectionType=" + $("#masterSectionType").val(),
            type: "GET",
            cache: false,
            success: function (data) {
                $("#loading").hide();
                $.fn.OpenModal('unitWarningModal', 'unit-warning-modal', data)
            },
            error: function (jqxhr) {
                $("#loading").hide();
                return true;
            }
        });
    });
    $(".modal").on("change", "#currency", function (e) {
        $.fn.CheckCurrency("add-or-edit-item");
        $.fn.CheckForVendorCurrencyWarning($("#VendorCurrencyID").val(), $("#currency").val());
    });
    $("#exchangeRate").change(function (e) {
        $.fn.CalculateSumPlusVat("add-or-edit-item");
        $.fn.CalculateUnitAmounts("add-or-edit-item");
        $.fn.CalculateSubUnitAmounts("add-or-edit-item");
        $.fn.CalculateSubSubUnitAmounts("add-or-edit-item");
    });

    $("body").off("change", ".add-or-edit-item #cost, .add-or-edit-item .cost").on("change", ".add-or-edit-item #cost, .add-or-edit-item .cost", function (e) {
        costChange("add-or-edit-item", this);
    });
    $("body").off("change", ".reorder-item #cost, .reorder-item  .cost").on("change", ".reorder-item #cost, .reorder-item .cost", function (e) {
        costChange("reorder-item", this);
    });
    function costChange(modalClass, element) {
        console.log("change cost");
        var index = 0;
        $.fn.CalculateSumPlusVat(modalClass, index);
            $.fn.CalculateUnitAmounts(modalClass);
            $.fn.CalculateSubUnitAmounts(modalClass);
            $.fn.CalculateSubSubUnitAmounts(modalClass);
    }

    $(".add-or-edit-item #sum-dollars").change(function (e) {
        sumDollarsChange("add-or-edit-item");
    });
    $(".reorder-item #sum-dollars").change(function (e) {
        sumDollarsChange("reorder-item");
    });
    function sumDollarsChange(modalClass) {
        console.log("in change sum");
        $.fn.CalculateSumPlusVat(modalClass);
        $.fn.CalculateUnitAmounts(modalClass);
        $.fn.CalculateSubUnitAmounts(modalClass);
        $.fn.CalculateSubSubUnitAmounts(modalClass);
    }


    $('.reorder-item .addSubUnit').click(function () {
        addSubUnit("reorder-item");
    })

    $('.reorder-item .addSubSubUnit').click(function () {
        addSubSubUnit("reorder-item");
    })


    $('.add-or-edit-item .addSubUnit').click(function () {
        addSubUnit("add-or-edit-item");
    })

    $('.add-or-edit-item .addSubSubUnit').click(function () {
        addSubSubUnit("add-or-edit-item");
    })


    function addSubUnit(modalClass) {
        $.fn.CheckUnitsFilled(modalClass);
        $.fn.EnableMaterialSelect('#subUnitTypeID', 'select-options-subUnitTypeID');
    }

    function addSubSubUnit(modalClass) {
        $.fn.CheckSubUnitsFilled(modalClass);
        $.fn.EnableMaterialSelect('#subSubUnitTypeID', 'select-options-subSubUnitTypeID');
    }

    $('.reorder-item .sub-close').click(function () {
        subClose("reorder-item");
    });

    $('.add-or-edit-item .sub-close').click(function () {
        subClose("add-or-edit-item");
    });

    function subClose(modalClass) {
        $.fn.DisableSubUnits(modalClass);
        $.fn.DisableSubSubUnits(modalClass);
        $("." + modalClass + ' .subUnitsCard').addClass('d-none');
        $("." + modalClass + ' .sub-close').addClass('d-none');
        $("." + modalClass + ' .addSubUnitCard').removeClass('d-none');
        $("." + modalClass + ' .addSubSubUnitCard').removeClass('d-none');
        $("." + modalClass + ' .RequestSubsubunitCard').addClass('d-none');
        $("." + modalClass + ' .subSubUnitsCard').addClass('d-none');
        $("." + modalClass + ' .subsub-close').addClass('d-none');
        $("." + modalClass + " #subUnit").removeClass('mark-readonly');
        $("." + modalClass + " #subSubUnit").removeClass('mark-readonly');
        $("." + modalClass + " #subUnitTypeID").removeClass('mark-readonly');
        $("." + modalClass + " #subSubUnitTypeID").removeClass('mark-readonly');
    }

    $('.add-or-edit-item .subsub-close').click(function () {
        subSubClose("add-or-edit-item");
    })

    $('.reorder-item .subsub-close').click(function () {
        subSubClose("reorder-item");
    })

    function subSubClose(modalClass) {
        $.fn.DisableSubSubUnits(modalClass);
        $("." + modalClass + ' .subSubUnitsCard').addClass('d-none');
        $("." + modalClass + ' .subsub-close').addClass('d-none');
        $("." + modalClass + ' .addSubSubUnitCard').removeClass('d-none');
        $("." + modalClass + " #subSubUnit").removeClass('mark-readonly');
        $("." + modalClass + " #subSubUnitTypeID").removeClass('mark-readonly');
    }

    $(".reorder-item #unit-price-dollars").change(function () {
        $.fn.CalculatePriceDollars("reorder-item")
    })

    $(".add-or-edit-item #unit-price-dollars").change(function () {
        $.fn.CalculatePriceDollars("add-or-edit-item")
    })
    $(".reorder-item #unit-price-shekel").change(function () {
        $.fn.CalculatePriceShekels("reorder-item");
    })
    $(".add-or-edit-item #unit-price-shekel").change(function () {
        $.fn.CalculatePriceShekels("add-or-edit-item");
    })
    $('body').on('change', '.add-or-edit-item .unit-type-select', function () {
        $.fn.UpdatePricePerUnitLabel('.add-or-edit-item .price-per-unit-label', $('.add-or-edit-item #select-options-unitTypeID li.active.selected span').text());
    })
    $('body').on('change', '.add-or-edit-item .subunit-type-select', function (e) {
        //alert('got here')
        $.fn.UpdatePricePerUnitLabel('.add-or-edit-item .price-per-subunit-label', $('.add-or-edit-item #select-options-subUnitTypeID li.active.selected span').text())
    })
    $('body').on('change', '.add-or-edit-item .sub-subunit-type-select', function (e) {
        //alert('got here')
        $.fn.UpdatePricePerUnitLabel('.add-or-edit-item .price-per-sub-subunit-label', $('.add-or-edit-item #select-options-subSubUnitTypeID li.active.selected span').text())
    })
    $('body').on('change', '.reorder-item .unit-type-select', function () {
        $.fn.UpdatePricePerUnitLabel('.reorder-item .price-per-unit-label', $('.reorder-item #select-options-unitTypeID li.active.selected span').text());
    })
    $('body').on('change', '.reorder-item .subunit-type-select', function (e) {
        //alert('got here')
        $.fn.UpdatePricePerUnitLabel('.reorder-item .price-per-subunit-label', $('.reorder-item #select-options-subUnitTypeID li.active.selected span').text())
    })
    $('body').on('change', '.reorder-item .sub-subunit-type-select', function (e) {
        //alert('got here')
        $.fn.UpdatePricePerUnitLabel('.reorder-item .price-per-sub-subunit-label', $('.reorder-item #select-options-subSubUnitTypeID li.active.selected span').text())
    })
    $.fn.UpdatePricePerUnitLabel = function (className, unitName) {
        if (unitName != "") {
            var newLabel = 'Price Per ' + unitName;
            console.log(newLabel);
            $(className).html(newLabel);
        }
    }
});













