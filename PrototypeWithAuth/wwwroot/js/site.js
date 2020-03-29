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


//view documents on modal view
$(".view-docs").click(function (clickEvent) {
    console.log("order docs clicked!");
    clickEvent.preventDefault();
    clickEvent.stopPropagation();
    var title = $(this).val();
    $(".images-header").html(title + " Documents Uploaded:");
});

//PRICE PAGE ON MODAL VIEW//
//on opening of the price tag see if subunits and subsubunits should be enabled
$("#priceTab").click(function () {
    if ($("#unit-amount").val() && $("#unit-type").val()) {
        $.fn.CalculateUnitAmounts();
        $.fn.OpenSubUnits();
        $.fn.ChangeSubUnitDropdown();
    }
    if ($("#subunit-amount").val() && $("#subunit-type").val()) {
        $.fn.CalculateSubUnitAmounts();
        $.fn.OpenSubSubUnits();
        $.fn.ChangeSubSubUnitDropdown();
    }
    if ($("#subsubunit-amount").val() && $("#subsubunit-type").val()) {
        $.fn.CalculateSubSubUnitAmounts();
    }
    if ($("#vatInShekel").val()) {
        $.fn.CalculateTotalPlusVat();
    }
});

//Calculate unit amounts
$.fn.CalculateUnitAmounts = function () {

};
//calculate sub unit amounts
$.fn.CalculateSubUnitAmounts = function () {

};
//calculate sub sub unit amounts
$.fn.CalculateSubSubUnitAmounts = function () {

};
//Open subunits
$.fn.OpenSubUnits = function () {

};
//open sub sub units 
$.fn.OpenSubSubUnits = function () {

};
//change subunit dropdown
$.fn.ChangeSubUnitDropdown = function () {

};
//change sub sub unit dropdown
$.fn.ChangeSubSubUnitDropdown = function () {

};
//calculate total + vat
$.fn.CalculateTotalPlusVat = function () {

};
//enable or disable sumdollars and sumshekel
$.fn.EnableSumDollarsOrShekel = function () {

}


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