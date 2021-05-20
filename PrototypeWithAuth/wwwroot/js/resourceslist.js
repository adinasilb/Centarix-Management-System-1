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

	$(".resource-icons .favorite").on("click", function (e) {
		var fullIcon = $(".FilledIn").attr("filled-value");
		var emptyIcon = $(".Empty").attr("filled-value");
		var reloadPageBool = $(this).attr("data-reload");
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
				if (reloadPageBool == "True") {
					$.ajax({
						async: true,
						url: "/Protocols/_ResourcesListIndex?IsReload=true",
						type: 'GET',
						cache: true,
						success: function (results) {
							$(".resources-favorites-partial").html(results);
						}
					});
				}
				else {
					if (icon.hasClass(emptyIcon)) {
						icon.removeClass(emptyIcon);
						icon.addClass(fullIcon);
					}
					else {
						icon.addClass(emptyIcon);
						icon.removeClass(fullIcon);
					}
				}
			}
		});
	});

	$(".resource-icons .share").on("click", function (e) {
		var url = "/Protocols/ShareModal?ID=" + $(this).val() + "&ModelsEnum=Resource"; 
		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				$.fn.OpenModal("shared-modal", "share-modal", data);
				$.fn.EnableMaterialSelect('#userlist', 'select-options-userlist')
			}
		});
	});

	
});