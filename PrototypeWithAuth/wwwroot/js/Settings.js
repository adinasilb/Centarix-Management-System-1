$(function () {
    $(".category-field2").off("click").on("click", function (e) {
        e.preventDefault();
        $.fn.OpenSettingsForm($(this).attr("data-catid"));
        $.fn.SelectNewCategory($(this), 2);
    });

    $.fn.OpenSettingsForm = function (catID) {
        var cattype2Field = $(".modaltypeval2").val();
        var url = "/Requests/_SettingsForm/?ModelType=" + cattype2Field + "&CategoryID=" + catID;
        $.ajax({
            async: true,
            url: url,
            type: "GET",
            cache: true,
            success: function (data) {
                $(".settings-form").html(data);
                $(".mdb-select").materialSelect();
                return false;
            }
        });
    };

    $(".category-field1").off("click").on("click", function (e) {
        e.preventDefault();
        var cattype1Field = $(".modaltypeval1");
        if (cattype1Field.val() == 'ParentCategory') {
            console.log("in first if");
            $.fn.FillSubcategoryList($(this).attr("data-catid"));
        }
        $.fn.SelectNewCategory($(this), 1);
    });

    $.fn.SelectNewCategory = function (element, columnid) {
        switch (columnid) {
            case 1:
                $(".category-field1").removeClass("selected");
                break;
            case 2:
                $(".category-field2").removeClass("selected");
        }
        element.addClass("selected");
    }

    $.fn.FillSubcategoryList = function (catID) {
        var catType2 = $(".modaltypeval2").val();
        var url = "/Requests/_CategoryList/?modelType=" + catType2 + "&ColumnNumber=" + 2 + "&ParentCategoryID=" + catID;
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

    $(".details-form").off("click").on("click", function () {
        var customFieldCounter = $(".customFieldCounter").attr("value");
        $.ajax({
            async: true,
            url: "/Requests/_CustomField?" + customFieldCounter,
            type: "GET",
            cache: true,
            success: function (data) {
                $(".custom-fields-details").html(data);
                $(".mdb-select-" + customFieldCounter).materialSelect();
                $(".customFieldCounter").attr("value", customFieldCounter + 1);
                return false;
            }
        });
    });
});