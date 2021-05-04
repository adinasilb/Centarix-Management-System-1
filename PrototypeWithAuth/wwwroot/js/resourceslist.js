$(function (e) {
	$(".call-resource-notes").on("click", function (e) {
		var url = "/Protocols/ResourceNotesModal?ResourceID=" + $(this).attr("id");
		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				$.fn.OpenModal('res-notes-modal', 'resource-notes-modal', data);
			}
		});
	});

	$(".favorite-protocol").on("click", function (e) {
		var fullIcon = "icon-favorite-24px";
		var emptyIcon = "icon-favorite_border-24px";
		var icon = $(this).children("i");
		if (icon.hasClass(emptyIcon)) {
			icon.removeClass(emptyIcon);
			icon.addClass(fullIcon);
		}
		else {
			icon.addClass(emptyIcon);
			icon.removeClass(fullIcon);
		}
	});
});