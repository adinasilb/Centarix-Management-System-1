  $("#protocolParentList").change(function () {
            var parentCategoryId = $("#protocolParentList").val();
            var url = "/Protocols/GetSubCategoryList";

            $.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
                var item = "<option value=''>Select a Subcategory</option>";
                $("#protocolSubList").empty();
                $.each(data, function (i, subCategory) {
                    item += '<option value="' + subCategory.protocolSubCategoryTypeID + '">' + subCategory.protocolSubCategoryTypeDescription + '</option>'
                });
                $("#protocolSubList").html(item);
            });
  });