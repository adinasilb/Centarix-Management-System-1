// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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

$("#unit-amount").change(function () {
    console.log("Unit amount changed");
});
$("#unit-type").change(function () {
    var selected = $(':selected', this);
    var optgroup = selected.closest('optgroup').attr('label');
    console.log("Unit type changed " + optgroup);

    //the following is based on the fact that the unit types and parents are seeded with primary key values
    if (optgroup == "Weight/Volume" || optgroup == "Test") {
        $("#subunit-type").children().remove("optgroup[label='Units']");
        $("#subsubunit-type").children().remove("optgroup[label='Units']");
    }
    if (optgroup == "Test") {
        $("#subunit-type").children().remove("optgroup[label='Weight/Volume']");
        $("#subsubunit-type").children().remove("optgroup[label='Weight/Volume']");
    }
});
$("#subunit-type").change(function () {
    var selected = $(':selected', this);
    var optgroup = selected.closest('optgroup').attr('label');
    console.log("Unit type changed " + optgroup);

    //the following is based on the fact that the unit types and parents are seeded with primary key values
    if (optgroup == "Weight/Volume" || optgroup == "Test") {
        $("#subsubunit-type").children().remove("optgroup[label='Units']");
    }
    if (optgroup == "Test") {
        $("#subsubunit-type").children().remove("optgroup[label='Weight/Volume']");
    }
});