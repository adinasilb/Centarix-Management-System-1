$(function () {
	$(".resources-tags-big").on("click", function (e) {
		var inputID = $(this).attr("id");
		alert("input id: " + inputID);
		var input = "<input type='hidden' id='" + inputID + "'>";
		if ($(this).hasClass("selected")) {
			$(this).removeClass("selected");
			input.appendTo($("#myForm")).show();
		}
		else {
			$(this).addClass("selected");
		}
	});
});