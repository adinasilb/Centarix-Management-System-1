$(function () {
	$(".ResourceNotesModal").on("click", function (e) {
		var url = "/Protocols/ResourceNotesModal";
		$.ajax({
			processData: false,
			contentType: false,
			data: new FormData($("#myForm")[0]),
			async: true,
			url: url,
			type: 'POST',
			cache: false,
			success: function (error) {
				if (!error) {
					$.fn.CloseModal('resource-notes-modal');
				}
				else {
					//eventually show error here on the modal
					alert(error);
				}
			}
		});
	});
});