$(function () {
	$(".resources-tags-big").on("click", function (e) {
		if ($(this).hasClass("selected")) {
			$(this).removeClass("selected");
		}
		else {
			$(this).addClass("selected");
		}
	});
});