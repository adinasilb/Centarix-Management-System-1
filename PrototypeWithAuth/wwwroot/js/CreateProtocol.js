  $("#protocolParentList").change(function () {
            var parentCategoryId = $("#protocolParentList").val();
            var url = "/Protocols/GetSubCategoryList";

      	$.getJSON(url, { ParentCategoryId: parentCategoryId }, function (data) {
			  $("#protocolSubList").children("option").each(function (i, option) {
				option.remove();
			});
			var firstitem1 = '<option value=""> Select Subcategory</option>';
			
			$("#protocolSubList").append(firstitem1);

			$.each(data, function (i, subCategory) {
                var newitem1 = '<option value="' + subCategory.protocolSubCategoryTypeID + '">' + subCategory.protocolSubCategoryTypeDescription + '</option>'
				$("#protocolSubList").append(newitem1);
            });
			$("#protocolSubList").materialSelect();
			return false;
		});
  });
