$(function () {
	$(".resources-tags-big").on("click", function (e) {
		var inputID = $(this).attr("id");
		var hiddenClass = "HiddenCategoryBool" + inputID;
		var hiddenElementBool = $("." + hiddenClass);
		//alert("hiddenElementBool id" + hiddenElementBool.attr("id"));
		//alert("hiddenElementBool value " + hiddenElementBool.attr("value"));
		if ($(this).hasClass("selected")) {
			hiddenElementBool.val("false");
			//alert("hiddenElementBool value changed to " + hiddenElementBool.attr("value"));
			$(this).removeClass("selected");
		}
		else {
			hiddenElementBool.val("true");
			//alert("hiddenElementBool value changed to " + hiddenElementBool.attr("value"));
			$(this).addClass("selected");
		}
	});
});