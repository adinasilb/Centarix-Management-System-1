$(".delete-file-perm").on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	var link = $('#submitDelete').attr("href");
	console.log("link: " + link)
	var formData = new FormData($("#DeleteDocumentModalForm")[0]);
	$.ajax({
		url: link,
		method: 'POST',
		data: formData,
		success: (partialResult) => {
			$("#DeleteDocumentsModal").replaceWith('');
			$foldername = $("#FolderName").val();
			$requestId = $("#RequestID").val();
			//$.fn.OpenDocumentsModal();
		},
		processData: false,
		contentType: false
	})
	return false;
});

$.fn.OpenDocumentsModal = function (enumString, requestId) {
	$("#documentsModal").replaceWith('');
	//$(".modal-backdrop").first().removeClass();
	$.ajax({
		async: true,
		url: "Requests/DocumentsModal?id=" + requestId + "&RequestFolderNameEnum=" + enumString,
		type: 'GET',
		cache: false,
		success: function (data) {
			var modal = $(data);
			$('body').append(modal);
			$("#documentsModal").modal({
				backdrop: false,
				keyboard: true,
			});
			$(".modal").modal('show');
		}
	});
};