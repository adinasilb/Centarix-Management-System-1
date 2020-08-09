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
		},
		processData: false,
		contentType: false
	})
	return false;
});