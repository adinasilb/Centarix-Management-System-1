$(function (e) {
	$(".call-resource-notes").on("click", function (e) {
		var url = "/Protocols/ResourceNotesModal?ResourceID=" + $(this).attr(id);
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
});