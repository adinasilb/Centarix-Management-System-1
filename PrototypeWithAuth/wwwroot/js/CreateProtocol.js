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

$(".addMaterial").click(function(){
	 var url="/Protocols/AddMaterialModal?materialTypeID="+$(this).val();
	 $.fn.CallPageRequest(url, "addMaterial");
});
$(".saveProtocol").click(function(e){
	e.preventDefault();
	 var formData =new FormData($("#myForm")[0]);
	$.ajax({
			url: "/Protocols/CreateProtocol",
			traditional: true,
			data:formData,
			contentType: false,
			processData: false,
			type: "POST",
			success: function (data) {
				$("_CreateProtocol").html(data)
				$("ul.tabs li:eq(1)").addClass("active").show();
				$(".tab-content:eq(1)").show();
			}
	});
});

$(".saveMaterial").click(function(e){
	e.preventDefault();
	var formData = new FormData($(".materialForm")[0]);
	$.ajax({
			url: "/Protocols/AddMaterialModal",
			traditional: true,
			data:formData,
			contentType: false,
			processData: false,
			type: "POST",
			success: function (data) {
				$("#Materials").html(data);
				$.fn.CloseModal('add-material');
			}
	});
});
