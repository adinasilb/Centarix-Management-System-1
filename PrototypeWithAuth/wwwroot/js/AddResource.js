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

	$(".get-pubmedinfo").on("click", function (e) {
		var url = "/Protocols/GetPubMedFromAPI";
		$.getJSON(url, { PubMedID: $("#Resource_PubMedID").val() }, function (data) {
			//get the business id from json
			var titleObject = $("#Resource_Title");
			var firstAuthorObject = $("#Resource_FirstAuthor");
			var lastAuthorObject = $("#Resource_LastAuthor");
			var journalObject = $("#Resource_Journal");
			var readonly = "disabled";
			titleObject.val(data.title);
			titleObject.addClass(readonly);
			firstAuthorObject.val(data.firstAuthor);
			firstAuthorObject.addClass(readonly)
			lastAuthorObject.val(data.lastAuthor);
			lastAuthorObject.addClass(readonly)
			journalObject.val(data.journal);
			journalObject.addClass(readonly)
		});
		//$.ajax({
		//	async: true,
		//	url: "/Protocols/GetPubMedFromAPI?PubMedID=" + $("#Resource_PubMedID").val(),
		//	type: 'GET',
		//	cache: false,
		//	success: function (data) {
				
		//	}
		//});
	});
});