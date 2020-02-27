﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showmodal() {
    $("#modal").modal('show');
};

$.fn.LoadCorrectSubcategory = function () {
    var $correctsub = $("#Request_Product_ProductSubcategory_ProductSubcategoryDescription").val();
    $("#sublist > [value='" + $correctsub + "']").attr("selected", "true");
    //idx = $("#sublist").find('option[value="' + $correctsub + '"]').index()
    //$("#sublist").get(0).selectedIndex = idx;
    //$("#sublist").find('option[value="' + $correctsub + '"]').attr('selected', 'selected');
    //works --> just don't want it here need to change to correct place $(".modal-footer").html($correctsub);
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