// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showmodal() {
    $("#modal").modal('show');
};

//change product subcategory dropdown according to the parent categroy selection when a parent category is selected
$("#parentlist").change(function () {
    var parentCategoryId = $("#parentlist").val();
    var url = "/Products/GetSubCategoryList";

    $.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
        var item = "";
        $("#sublist").empty();
        $.each(data, function (i, subCategory) {
            item += '<option value="' + subCategory.productSubcategoryID + '">' + subCategory.productSubcategoryDescription + '</option>'
        });
        $("#sublist").html(item);
    });
});

//load the vendor business id with the vendor business id corresponding to the newly selected vendor from the vendor list
$("#vendorList").change(function () {
    //get the new vendor id selected
    var vendorid = $("#vendorList").val();
    //load the url of the Json Get from the controller
    
});

$.fn.ChangeVendorBusinessId = function ($vendorid) {
    var url = "/Requests/GetVendorBusinessID";

    $.getJSON(url, { VendorID: vendorid }, function (data) {
        var newBusId = "" + data.vendorBuisnessID;
        $(".vendorBusinessId").html(newBusId);
    })
}