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
		var fullIcon = $(".FilledIn").attr("filled-value");
		var emptyIcon = $(".Empty").attr("filled-value");
		alert("fullIcon: " + fullIcon);
		alert("emptyIcon: " + emptyIcon);
		var url = "/Protocols/FavoriteResources?ResourceID=" + $(this).val() + "&Favorite=";
		var icon = $(this).children("i");
		if (icon.hasClass(emptyIcon)) {
			url += "False";
		}
		else {
			url += "True";
		}
		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				if (icon.hasClass(emptyIcon)) {
					icon.removeClass(emptyIcon);
					icon.addClass(fullIcon);
				}
				else {
					icon.addClass(emptyIcon);
					icon.removeClass(fullIcon);
				}
			}
		});
	});
});