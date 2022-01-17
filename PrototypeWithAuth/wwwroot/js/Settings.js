$(function () {
    $(".category-field").on("click", function (e) {
        e.preventDefault();
        var categoryType = $("#Model").val();
        switch (categoryType) {
            case 'ParentCategory':
                $.fn.FillSubcategoryList($(this).attr("data-catid"));
                break;
        }
    });

    $.fn.FillSubcategoryList = function (catID) {
        console.log("model val: " + $("#Model").val());
        var url = "/Requests/_CategoryList/?modelType=" + $("#Model").val() + "&ParentCategoryID=" + catID;
        console.log("url: " + url);
        $.ajax({
            async: true,
            url: url,
            type: "GET",
            cache: true,
            success: function (data) {
                $(".category-list-2").html(data);
                return false;
            }
        });
    };
});