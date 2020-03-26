// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//global Exchange Rate variable (usd --> nis)

function showmodal() {
    $("#modal").modal('show');
};

//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
$("#parentlist").change(function () {
    var parentCategoryId = $("#parentlist").val();
    var url = "/Requests/GetSubCategoryList";

    $.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
        var item = "";
        $("#sublist").empty();
        $.each(data, function (i, subCategory) {
            item += '<option value="' + subCategory.productSubcategoryID + '">' + subCategory.productSubcategoryDescription + '</option>'
        });
        $("#sublist").html(item);
    });
});

$("#vendorList").change(function () {
    //get the new vendor id selected
    var vendorid = $("#vendorList").val();
    $.fn.ChangeVendorBusinessId(vendorid);
});

$.fn.ChangeVendorBusinessId = function (vendorid) {
    var newBusinessID = "";
    //will throw an error if its a null value so tests it here
    if (vendorid > 0) {
        //load the url of the Json Get from the controller
        var url = "/Vendors/GetVendorBusinessID";
        $.getJSON(url, { VendorID: vendorid }, function (data) {
            //get the business id from json
            newBusinessID = data.vendorBuisnessID;
            //cannot only use the load outside. apparently it needs this one in order to work
            $(".vendorBusinessId").html(newBusinessID);
        })
    }
    //if nothing was selected want to load a blank
    $(".vendorBusinessId").html(newBusinessID);
    //put the business id into the form
}

//PRICE PAGE ON MODAL VIEW//

$("#unit-type").change(function () {
    $.fn.OpenSubUnits();
});

$.fn.OpenSubUnits = function () {
    var selected = $(':selected', $("#unit-type"));
    var optgroup = selected.closest('optgroup').attr('label');

    if (selected.val() != "") {
        console.log("inside the if");
        $(".RequestSubunitCard .form-control").prop('disabled', false);
    }
    //needed to do an else if b/c it wasn't entering the else for some reason
    else if (selected.val() == "") {
        console.log("inside the else");
        $(".RequestSubunitCard .form-control").prop('disabled', true);
        $(".RequestSubsubunitCard .form-control").prop('disabled', true);
    }
    //the following is based on the fact that the unit types and parents are seeded with primary key values

    //take out the sub sub units??

    switch (optgroup) {
        case "Units":
            $("#subunit-type optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
            $("#subunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
            break;
        case "Weight/Volume":
            $("#subunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
            $("#subunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
            break;
        case "Test":
            $("#subunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
            $("#subunit-type optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
            break;
    }
}

$("#subunit-type").change(function () {
    $.fn.OpenSubSubUnits();
});

$.fn.OpenSubSubUnits = function () {
    var selected = $(':selected', $("#subunit-type"));
    var optgroup = selected.closest('optgroup').attr('label');
    console.log("Unit type changed " + optgroup);

    if (selected > "") {
        $(".RequestSubsubunitCard .form-control").attr('disabled', false);
    }
    else {
        $(".RequestSubsubunitCard .form-control").attr('disabled', true);
    }
    //the following is based on the fact that the unit types and parents are seeded with primary key values
    switch (optgroup) {
        case "Units":
            $("#subsubunit-type optgroup[label='Units']").prop('disabled', false).prop('hidden', false);
            $("#subsubunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
            break;
        case "Weight/Volume":
            $("#subsubunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
            $("#subsubunit-type optgroup[label='Weight/Volume']").prop('disabled', false).prop('hidden', false);
            break;
        case "Test":
            $("#subsubunit-type optgroup[label='Units']").prop('disabled', true).prop('hidden', true);
            $("#subsubunit-type optgroup[label='Weight/Volume']").prop('disabled', true).prop('hidden', true);
            break;
    }
}


//Automatic updates on prices and sums for Modal View --> Pricing
$("#unit-amount").change(function () {
    $.fn.ChangeUnitAndSum();
});
$("#subunit-amount").change(function () {
    console.log("Subunit amount changed");
    $.fn.GetSubUnitPrice();
    $.fn.GetSubSubUnitPrice();
});
$("#subsubunit-amount").change(function () {
    console.log("Subsubunit amount changed");
    $.fn.GetSubSubUnitPrice();
});
//when sum is put in in dollars
$("#sumDollars").change(function () {
    $.fn.ChangeUnitAndSum();
});
//when sum is put in in shekel
$("#sumShekel").change(function () {
    var $sumDollars = $(this).val * $("#Request_ExchangeRate"); //NEED TO DO THIS AGAIN AFTER THE EXCHANGE RATE IS CHANGED
    $('input[name="sumDollars"]').val($sumDollars);
    $.fn.ChangeUnitAndSum;
})
$("#vatInShekel").change(function () {
    console.log("Vat changed");
    $.fn.ChangeVAT();
});
$.fn.ChangeUnitAndSum = function () {
    var $unitAmt = $("#unit-amount").val();
    var $sumDollars = $("#sumDollars").val();
    if ($sumDollars != Infinity && $sumDollars != NaN) {
        $unitSumDollars = $sumDollars / $unitAmt;
    }
    console.log("Unit sum in dollars: " + $unitSumDollars);
    $('input[name="unit-price-dollars"]').val($unitSumDollars);
    $.fn.ChangeShekalim();
    $.fn.GetSubUnitPrice();
    $.fn.GetSubSubUnitPrice();
}
$.fn.ChangeShekalim = function () {
    var $exchangeRate = $("#Request_ExchangeRate").val();
    //change the sum
    $sumDollars = $("#sumDollars").val();
    $sumShekel = $sumDollars * $exchangeRate;
    $('input[name="sumShekel"').val($sumShekel);

    //change the unit
    $unitPriceDollars = $("#unit-price-dollars").val();
    $unitPriceShekel = $unitPriceDollars * $exchangeRate;
    $('input[name="unit-price-shekel"]').val($unitPriceShekel);

    //change the subunit 
    $subunitPriceDollars = $("#subunit-price-dollars").val();
    $subunitPriceShekel = $subunitPriceDollars * $exchangeRate;
    $('input[name="subunit-price-shekel"]').val($subunitPriceShekel);

    //change the subsubunit
    $subsubunitPriceDollars = $("#subsubunit-price-dollars").val();
    $subsubunitPriceShekel = $subsubunitPriceDollars * $exchangeRate;
    $('input[name="subsubunit-price-shekel"]').val($subsubunitPriceShekel);

    $.fn.ChangeVAT();
}

//CURRENTLY NOT WORKING
$.fn.ChangeVAT = function () {
    var $exchangeRate = $("#Request_ExchangeRate").val();
    var vatInShekel = $("#vatInShekel").val();
    console.log("vat in shekel: " + vatInShekel);
    var sumShekel = $("#sumShekel").val();
    console.log("sum in shekel: " + sumShekel);
    var $sumPlusVatShekel = parseInt(sumShekel) + parseInt(vatInShekel);
    console.log("sum plus vat in shekel: " + $sumPlusVatShekel);
    if ($sumPlusVatShekel != Infinity && $sumPlusVatShekel != NaN) {
        $('input[name="SumPlusVat-Shekel"]').val($sumPlusVatShekel);
    }
    //IS THIS THE BEST WAY OF DOING IT OR SHOULD I CONVERT THE INPUT I JUST DID??
    var vatInDollars = vatInShekel / $exchangeRate;
    console.log("Vat in dollars: " + vatInDollars);
    var $sumPlusVatDollars = parseInt($("#sumDollars").val()) + parseInt(vatInDollars);
    console.log("sum plus vat in dollars: " + $sumPlusVatDollars);
    if ($sumPlusVatDollars != Infinity && $sumPlusVatDollars != NaN) {
        $('input[name="SumPlusVat-Dollar"]').val($sumPlusVatDollars);
    }
    console.log("-----------------------------------");
}

//get price for subunits
$.fn.GetSubUnitPrice = function () {
    var $exchangeRate = $("#Request_ExchangeRate").val();
    var subunitpricedollars = $("#unit-price-dollars").val() / $("#subunit-amount").val();
    console.log("subunit price dollars " + subunitpricedollars);
    var subunitpriceshekel = subunitpricedollars * $exchangeRate;
    console.log("subunit price shekel " + subunitpriceshekel);
    if (subunitpricedollars != Infinity && subunitpricedollars != NaN) {
        $('input[name="subunit-price-dollars"]').val(subunitpricedollars);
    }
    if (subunitpriceshekel != Infinity && subunitpriceshekel != NaN) {
        $('input[name="subunit-price-shekel"]').val(subunitpriceshekel);
    }
}

//get price for subsubunits
$.fn.GetSubSubUnitPrice = function () {
    var $exchangeRate = $("#Request_ExchangeRate").val();
    var subsubunitpricedollars = $("#subunit-price-dollars").val() / $("#subsubunit-amount").val();
    console.log("subsubunit price dollars " + subsubunitpricedollars);
    var subsubunitpriceshekel = subsubunitpricedollars * $exchangeRate;
    console.log("subsubunit price shekel " + subsubunitpriceshekel);
    if (subsubunitpricedollars != Infinity && subsubunitpricedollars != NaN) {
        $('input[name="subsubunit-price-dollars"]').val(subsubunitpricedollars);
    }
    if (subsubunitpriceshekel != Infinity && subsubunitpriceshekel != NaN) {
        $('input[name="subsubunit-price-shekel"]').val(subsubunitpriceshekel);
    }
}


//load the vendor business id with the vendor business id corresponding to the newly selected vendor from the vendor list

//view documents on modal view
$(".view-docs").click(function (clickEvent) {
    console.log("order docs clicked!");
    clickEvent.preventDefault();
    clickEvent.stopPropagation();
    var title = $(this).val();
    $(".images-header").html(title + " Documents Uploaded:");
});

//on opening of the price tag see if subunits and subsubunits should be enabled
$("#priceTab").click(function () {
    console.log("price tab clicked!");
    if ($("#unit-amount").val() && $("#unit-type").val()) {
        $.fn.OpenSubUnits();
        $.fn.ChangeUnitAndSum();
    }
    if ($("#subunit-amount").val() && $("#subunit-type").val()) {
        console.log("sub unit amount was filled out!");
        $.fn.OpenSubSubUnits();
        $.fn.GetSubUnitPrice();
    }
    if ($("#subsubunit-amount").val() && $("#subsubunit-type").val()) {
        console.log("sub sub unit amount was filled out!");
        $.fn.GetSubSubUnitPrice();
    }
    if ($("#vatInShekel").val()) {
        $.fn.ChangeVAT();
    }
});

//change what is inputted (dollar vs shekel when changing this)
//not changing values b/c that will be done when the sum is put in
$("#currency").change(function () {
    console.log("currency type changed to " + $(this).val());
    if ($(this).val() == "shekel") {
        $("#sumDollar").attr("disabled", true);
        $("#sumShekel").attr("disabled", false);
    }
    else if ($(this).val() == "dollar") {
        $("#sumDollar").attr("disabled", false);
        $("#sumShekel").attr("disabled", true);
    }
});

////change expected supply days automatically
//$("#Request_ExpectedSupplyDays").change(function () {
//    console.log("Request_expectedsupplydays changed!")
//    var invoiceDate = $("#Request_ParentRequest_InvoiceDate").val();
//    console.log("Invoice Date: " + invoiceDate);
//    var invoiceDateinDateFormat = new Date(invoiceDate);
//    console.log("Invoice Date in Date Format: " + invoiceDateinDateFormat);
//    var expectedSupplyDays = $(this).val();
//    console.log("Expected Supply Days: " + expectedSupplyDays);
//    var expectedSupplyDate = invoiceDateinDateFormat.setMonth(3);
//    console.log("Expected Supply Date: " + expectedSupplyDate);
//});
////change warranty month automatically
//$("#Request_Warranty").change(function () {
//    console.log("Request_Warranty changed!")
//});

//function addMonths(date, months) {
//    var d = date.getDate();
//    date.setMonth(date.getMonth() + +months);
//    if (date.getDate() != d) {
//        date.setDate(0);
//    }
//};