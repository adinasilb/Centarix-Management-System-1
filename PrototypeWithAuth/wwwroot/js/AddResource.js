
$(function () {
	$(".resources-tags-big").on("click", function (e) {
		console.log("in resource tags big click");
		var inputID = $(this).attr("id");
		console.log("input id: " + inputID);
		var hiddenClass = "HiddenCategoryBool" + inputID;
		console.log("hiddenClass: " + hiddenClass);
		var hiddenElementBool = $("." + hiddenClass);
		//alert("hiddenElementBool id" + hiddenElementBool.attr("id"));
		//alert("hiddenElementBool value " + hiddenElementBool.attr("value"));
		if ($(this).hasClass("selected")) {
			console.log("has class selected");
			hiddenElementBool.attr("value", "false");
			//alert("hiddenElementBool value changed to " + hiddenElementBool.attr("value"));
			$(this).removeClass("selected");
		}
		else {
			hiddenElementBool.attr("value", "true");
			console.log("does not have class selected");
			//alert("hiddenElementBool value changed to " + hiddenElementBool.attr("value"));
			$(this).addClass("selected");
		}
	});

	$(".get-pubmedinfo").on("click", function (e) {
		//alert("message: " + $("span[data-valmsg-for='Resource.PubMedID']").text());
		var url = "/Protocols/GetPubMedFromAPI";
		$.getJSON(url, { PubMedID: $("#Resource_PubMedID").val().trim() }, function (data) {
			var titleObject = $("#Resource_Title");
			var firstAuthorObject = $("#Resource_FirstAuthor");
			var lastAuthorObject = $("#Resource_LastAuthor");
			var journalObject = $("#Resource_Journal");
			var urlObject = $("#Resource_Url");
			var urlLink = $(".add-url");
			var readonly = "disabled";

			var spanObject = $("span[data-valmsg-for='Resource.PubMedID']");
			if (data.success == true) {
				titleObject.val(data.resource.title);
				titleObject.addClass(readonly);
				firstAuthorObject.val(data.resource.firstAuthor);
				firstAuthorObject.addClass(readonly)
				lastAuthorObject.val(data.resource.lastAuthor);
				lastAuthorObject.addClass(readonly)
				journalObject.val(data.resource.journal);
				journalObject.addClass(readonly)
				urlObject.val(data.resource.url);
				urlObject.addClass(readonly);

				// the following lines are undone in the else
				urlLink.removeClass("disabled");
				urlLink.attr("href", data.resource.url);
				urlLink.children("a").removeClass("disabled-color1");
				urlLink.children("a").addClass("protocols-color");

				spanObject.text("");
			}
			else {
				titleObject.val("");
				firstAuthorObject.val("");
				lastAuthorObject.val("");
				journalObject.val("");
				urlObject.val("");
				titleObject.removeClass(readonly);
				firstAuthorObject.removeClass(readonly);
				lastAuthorObject.removeClass(readonly);
				journalObject.removeClass(readonly);
				urlObject.removeClass(readonly);

				urlLink.addClass("disabled");
				urlLink.attr("href", "");
				urlLink.children("a").addClass("disabled-color1");
				urlLink.children("a").removeClass("protocols-color");

				spanObject.text("Invalid PubMedID Or the API is down");
			}
		});
	});
	$("#Resource_Url").on("change", function (e) {
		var urlLink = $(".add-url");
		if ($(this).val().length > 0) {
			alert("has url");
			urlLink.removeClass("disabled");
			var url = $(this).val();
			var startLink = url.substring(0, 8);
			if (startLink != "https://") {
				url = "https://" + url;
			}
			urlLink.attr("href", url);
			urlLink.children("a").removeClass("disabled-color1");
			urlLink.children("a").addClass("protocols-color");
		}
		else {
			alert("no url");
			urlLink.addClass("disabled");
			urlLink.attr("href", "");
			urlLink.children("a").addClass("disabled-color1");
			urlLink.children("a").removeClass("protocols-color");
		}
	});

	$(".AddFile").on("click", function (e) {
		$("#ResourceImage").click();
	});
	$("#ResourceImage").on("change", function (e) {
		var fileName = e.target.files[0].name;
		$("#file-name").val(fileName);
	});

});