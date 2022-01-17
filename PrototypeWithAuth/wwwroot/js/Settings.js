$(function () {
    $(".category-field").on("click", function () {
        var categoryType = $("#Model").val();
        switch (categoryType) {
            case 'ParentCategory':
                $.fn.FillSubcategoryList($(this).attr("data-catid"));
                break;
        }
    });

    $.fn.FillSubcategoryList = function (catID) {
        var url = "/Requests/_CategoryList/model=" + $("#FirstModel").val() + "&ParentCategoryID=" + catID;
        $.ajax({
            async: true,
            url: url,
            type: "GET",
            cache: true,
            success: function (data) {
                $(".category-list-2").html(data);
            }
        });
    };
});